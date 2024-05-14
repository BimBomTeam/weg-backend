using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEG.Application;
using WEG.Application.Commands;
using WEG.Application.Queries;
using WEG.Domain.Entities;
using WEG.Infrastructure.Commands;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Queries;

namespace WEG.Infrastructure.Services
{
    public class WordService : IWordService
    {
        private const int WORDS_COUNT = 5;

        private readonly IAiCommunicationService _aiService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGameDayQuery _gameDayQuery;
        private readonly IGameDayCommand _gameDayCommand;
        private readonly IDailyProgressStatsCommand _dailyProgressCommand;
        private readonly IDailyProgressStatsQuery _dailyProgressQuery;
        private readonly IRolesService _rolesService;
        private readonly IWordsCommand _wordsCommand;
        private readonly IWordsQuery _wordsQuery;
        private readonly IMapper _mapper;
        public WordService(IAiCommunicationService aiService,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IGameDayQuery gameDayQuery,
            IGameDayCommand gameDayCommand,
            IDailyProgressStatsQuery dailyProgressQuery,
            IDailyProgressStatsCommand dailyProgressCommand,
            IRolesService rolesService,
            IWordsCommand wordsCommand,
            IWordsQuery wordsQuery,
            IMapper mapper)
        {
            _aiService = aiService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _gameDayQuery = gameDayQuery;
            _gameDayCommand = gameDayCommand;
            _dailyProgressQuery = dailyProgressQuery;
            _dailyProgressCommand = dailyProgressCommand;
            _rolesService = rolesService;
            _wordsCommand = wordsCommand;
            _wordsQuery = wordsQuery;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WordDto>> GetWordsByRoleAsync(int roleId)
        {
            try
            {
                var role = await _rolesService.GetByIdRole(roleId);

                if (role == null)
                    throw new ArgumentException("No role with this id in database");

                var todayGameDay = await _gameDayQuery.GetTodayGameDayAsync();
                if (todayGameDay == null)
                {
                    todayGameDay = await _gameDayCommand.CreateTodayAsync();
                    await _gameDayCommand.SaveChangesAsync();
                }

                var userEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

                if (userEmail == null)
                    throw new ArgumentException("Invalid token");

                var user = await _userManager.FindByEmailAsync(userEmail);

                if (user == null)
                    throw new ArgumentException("Invalid token");

                var userProgress = await _dailyProgressQuery.GetUserDailyProgres(todayGameDay, user.Id);
                //TODO: Make function to create daily progress
                if (userProgress == null)
                {
                    DailyProgressStats dailyProgress = new DailyProgressStats()
                    {
                        DayId = todayGameDay.Id,
                        UserId = user.Id
                    };
                    userProgress = await _dailyProgressCommand.AddAsync(dailyProgress);
                    await _dailyProgressCommand.SaveChangesAsync();
                }
                else
                {
                    var storedWords = _wordsQuery.GetWordsByDailyProgressAndRole(userProgress.Id, role.Id);

                    if (storedWords != null && storedWords.Count() > 0)
                    {
                        if (storedWords.Count() > WORDS_COUNT)
                        {
                            await _wordsCommand.ClearWordsForRoleProgress(userProgress.Id, role.Id);
                            await _wordsCommand.SaveChangesAsync();
                        }
                        else
                        {
                            return _mapper.Map<IEnumerable<WordDto>>(storedWords);
                        }
                    }
                }

                string level = user.Level.ToString();

                var generatedWords = await _aiService.GenerateWordsAsync(level, role.Name);

                var newWords = generatedWords.Words.Select((word) =>
                {
                    return new Word()
                    {
                        Name = word,
                        State = WordProgressState.InProgress,
                        DailyProgressId = userProgress.Id,
                        RoleId = roleId
                    };
                });

                IEnumerable<Task> addWordsTasks =
                    newWords.AsParallel().Select(word => _wordsCommand.AddAsync(word));

                Task.WaitAll(addWordsTasks.ToArray());
                await _wordsCommand.SaveChangesAsync();

                return _mapper.Map<IEnumerable<WordDto>>(newWords);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in words generating: {ex.Message}");
            }
        }
    }
}
