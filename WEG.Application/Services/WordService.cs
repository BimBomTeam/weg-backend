using AutoMapper;
using StackExchange.Redis;
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
        private readonly IRolesService _rolesService;
        private readonly IWordsCommand _wordsCommand;
        private readonly IWordsQuery _wordsQuery;
        private readonly IUserDailyProgressService _userDailyProgressService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public WordService(IAiCommunicationService aiService,
            IRolesService rolesService,
            IWordsCommand wordsCommand,
            IWordsQuery wordsQuery,
            IUserDailyProgressService userDailyProgressService,
            IAuthService authService,
            IMapper mapper)
        {
            _aiService = aiService;
            _rolesService = rolesService;
            _wordsCommand = wordsCommand;
            _wordsQuery = wordsQuery;
            _userDailyProgressService = userDailyProgressService;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task AddWordsByRoleProgress(int roleId, int dailyProgressId, string level)
        {
            var role = await _rolesService.GetByIdRole(roleId);

            if (role == null)
                throw new ArgumentException("No role with this id in database");

            var generatedWords = await _aiService.GenerateWordsAsync(level, role.Name);

            var newWords = generatedWords.Words.Select((word) =>
            {
                return new Word()
                {
                    Name = word,
                    State = WordProgressState.InProgress,
                    DailyProgressId = dailyProgressId,
                    RoleId = roleId
                };
            });

            var addWordsTasks = newWords.AsParallel().Select(word => 
            _wordsCommand.AddAsync(word)
            );

            Task.WaitAll(addWordsTasks.ToArray());
            await _wordsCommand.SaveChangesAsync();
        }

        public async Task<IEnumerable<WordDto>> GetWordsByRoleAsync(int roleId)
        {
            try
            {
                var user = await _authService.GetUserFromRequest();

                string level = user.Level.ToString();

                var userProgress = await _userDailyProgressService.GetUserTodayProgress(user.Id);

                var storedWords = _wordsQuery.GetWordsByDailyProgressAndRole(userProgress.Id, roleId);

                if (storedWords == null || storedWords.Count() == 0)
                {
                    await AddWordsByRoleProgress(roleId, userProgress.Id, level);
                    storedWords = _wordsQuery.GetWordsByDailyProgressAndRole(userProgress.Id, roleId);
                }

                if (storedWords.Count() != WORDS_COUNT)
                {
                    await _wordsCommand.ClearWordsForRoleProgressAsync(userProgress.Id, roleId);
                    await _wordsCommand.SaveChangesAsync();
                    await AddWordsByRoleProgress(roleId, userProgress.Id, level);
                    storedWords = _wordsQuery.GetWordsByDailyProgressAndRole(userProgress.Id, roleId);
                }
                return _mapper.Map<IEnumerable<WordDto>>(storedWords);
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
        public async Task SaveWordsAsync(IEnumerable<WordDto> wordsDto)
        {
            try
            {
                var user = await _authService.GetUserFromRequest();

                var words = _mapper.Map<IEnumerable<Word>>(wordsDto);

                var userProgress = await _userDailyProgressService.GetUserTodayProgress(user.Id);

                foreach (var word in words)
                    word.DailyProgressId = userProgress.Id;

                foreach (var word in words)
                    _wordsCommand.Update(word);

                await _wordsCommand.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
