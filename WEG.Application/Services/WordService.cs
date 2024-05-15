using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WEG.Application.Services;
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
        private readonly IDailyProgressStatsCommand _dailyProgressCommand;
        private readonly IDailyProgressStatsQuery _dailyProgressQuery;
        private readonly IRolesService _rolesService;
        private readonly IWordsCommand _wordsCommand;
        private readonly IWordsQuery _wordsQuery;
        private readonly IGameDayService _gameDayService;
        private readonly IUserDailyProgressService _userDailyProgressService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public WordService(IAiCommunicationService aiService,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IDailyProgressStatsQuery dailyProgressQuery,
            IDailyProgressStatsCommand dailyProgressCommand,
            IRolesService rolesService,
            IWordsCommand wordsCommand,
            IWordsQuery wordsQuery,
            IGameDayService gameDayService,
            IUserDailyProgressService userDailyProgressService,
            IAuthService authService,
            IMapper mapper)
        {
            _aiService = aiService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _dailyProgressQuery = dailyProgressQuery;
            _dailyProgressCommand = dailyProgressCommand;
            _rolesService = rolesService;
            _wordsCommand = wordsCommand;
            _wordsQuery = wordsQuery;
            _gameDayService = gameDayService;
            _userDailyProgressService = userDailyProgressService;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WordDto>> GetWordsByRoleAsync(int roleId)
        {
            try
            {
                var role = await _rolesService.GetByIdRole(roleId);

                if (role == null)
                    throw new ArgumentException("No role with this id in database");

                //var userEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

                //if (userEmail == null)
                //    throw new ArgumentException("Invalid token");

                //var user = await _userManager.FindByEmailAsync(userEmail);
                var user = await _authService.GetUserFromRequest();

                if (user == null)
                    throw new ArgumentException("Invalid token");

                var userProgress = await _userDailyProgressService.GetUserTodayProgress(user.Id);

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

                foreach (var word in newWords)
                    await _wordsCommand.AddAsync(word);

                //Task.WaitAll(addWordsTasks.ToArray());
                await _wordsCommand.SaveChangesAsync();

                return _mapper.Map<IEnumerable<WordDto>>(newWords);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in words generating: {ex.Message}");
            }
        }
        public async Task CheckWordsAsync(IEnumerable<WordDto> words, string text)
        {
            await Task<IEnumerable<WordDto>>.Run(() =>
            {
                var wordDictionary = words.ToDictionary(w => w.Name, w => w, StringComparer.OrdinalIgnoreCase);

                string[] separators = { " ", ".", ",", "?", "!", ";", ":", "\t", "\n", "\r" };
                foreach (var word in text.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (wordDictionary.TryGetValue(word, out WordDto wordDto))
                    {
                        wordDto.State = WordProgressState.Learned.ToString();
                    }
                }
            });
        }
        public async Task SaveWords(IEnumerable<WordDto> wordsDto)
        {
            try
            {
                var user = await _authService.GetUserFromRequest();

                var words = _mapper.Map<IEnumerable<Word>>(wordsDto);

                var userProgress = await _userDailyProgressService.GetUserTodayProgress(user.Id);

                foreach (var word in words)
                    word.DailyProgressId = userProgress.Id;

                var addTasks = words.AsParallel().Select(word => _wordsCommand.AddAsync(word));

                Task.WaitAll(addTasks.ToArray());

                await _wordsCommand.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
