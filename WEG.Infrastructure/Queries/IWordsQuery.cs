using WEG.Domain.Entities;

namespace WEG.Infrastructure.Queries
{
    public interface IWordsQuery : IBaseQuery<Word,int>
    {
        IEnumerable<Word> GetWordsByDailyProgressAndRole(int progressId, int roleId);
        IEnumerable<Word> GetWordsByDailyProgress(int progressId);
    }
}
