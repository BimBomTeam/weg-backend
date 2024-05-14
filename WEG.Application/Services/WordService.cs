using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEG.Application;
using WEG.Application.Commands;
using WEG.Application.Queries;
using WEG.Domain.Entities;
using WEG.Infrastructure.Dto;

namespace WEG.Infrastructure.Services
{
    public class WordService : IWordService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAiCommunicationService _aiService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GameDayQuery _gameDayQuery;
        private readonly GameDayCommand _gameDayCommand;
        private readonly DailyProgressStatsCommand _dailyProgressCommand;
        private readonly DailyProgressStatsQuery _dailyProgressQuery;

        public WordService(ApplicationDbContext context,
            IAiCommunicationService aiService,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            GameDayQuery gameDayQuery,
            GameDayCommand gameDayCommand,
            DailyProgressStatsQuery dailyProgressQuery,
            DailyProgressStatsCommand dailyProgressCommand)
        {
            _context = context;
            _aiService = aiService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _gameDayQuery = gameDayQuery;
            _gameDayCommand = gameDayCommand;
            _dailyProgressQuery = dailyProgressQuery;
            _dailyProgressCommand = dailyProgressCommand;
        }

        public async Task<IEnumerable<WordDto>> GetWordsByRoleAsync(int roleId)
        {
            var npcExists = !(await _context.NpcRoles.AnyAsync(nr => nr.Id == roleId));

            if (npcExists)
                throw new ArgumentException("No role with this id in database");

            var todayGameDay = await _gameDayQuery.GetTodayGameDayAsync();
            if (todayGameDay == null)
                todayGameDay = await _gameDayCommand.CreateTodayAsync();

            try
            {
                var userEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                if (userEmail != null)
                {
                    var user = await _userManager.FindByEmailAsync(userEmail);

                    var userProgress = await _dailyProgressQuery.GetUserDailyProgres(todayGameDay, user.Id);
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

                    string level = user.Level.ToString();

                    var generatedWords = await _aiService.GenerateWordsAsync(level, "role");

                    var newWords = new List<Word>();
                    foreach (var word in generatedWords.Words)
                    {

                        newWords.Add(new Word
                        {
                            Name = word,
                            State = WordProgressState.InProgress,
                            DailyProgressId = userProgress.Id,
                            RoleId = roleId
                        });
                    }
                    _context.Words.AddRange(newWords);
                    await _context.SaveChangesAsync();


                    return generatedWords.Words.Select(w => new WordDto
                    {
                        Name = w,
                        State = WordProgressState.InProgress.ToString(),
                        RoleId = roleId
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Wystąpił błąd podczas generowania słówek dla nowego NPC: {ex.Message}");
            }

            // Jeśli NPC istnieje, pobierz słówka przypisane do niego z bazy danych
            var words = await _context.Words
                .Where(w => w.RoleId == roleId)
                .Select(w => new WordDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    State = w.State.ToString(), // Konwersja enuma na string
                    RoleId = w.RoleId
                })
                .ToListAsync();

            return words;
        }

        public async Task UpdateWordStatusAsync(int wordId, WordProgressState newState)
        {
            var word = await _context.Words.FindAsync(wordId);

            if (word == null)
            {
                throw new ArgumentException("Word with the specified ID does not exist.");
            }

            word.State = newState;
            await _context.SaveChangesAsync();
        }
    }
}
