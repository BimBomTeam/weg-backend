using WEG.Infrastructure.Dto;

namespace WEG.Infrastructure.Services
{
    public interface IWordService
    {
        Task<IEnumerable<WordDto>> GetWordsByRoleAsync(int roleId);
        Task CheckWordsAsync(IEnumerable<WordDto> words, string text);
        Task SaveWordsAsync(IEnumerable<WordDto> wordsDto);
    }
}
