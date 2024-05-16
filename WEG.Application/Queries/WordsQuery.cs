using WEG.Domain.Entities;
using WEG.Infrastructure.Queries;
namespace WEG.Application.Queries
{
    public class WordsQuery : BaseQuery<Word,int>, IWordsQuery
    {
        public WordsQuery(ApplicationDbContext context) : base(context) { }

        public IEnumerable<Word> GetWordsByDailyProgressAndRole(int progressId, int roleId)
        {
            return context.Words.Where(x => x.DailyProgressId == progressId && x.RoleId == roleId);
        }
        public IEnumerable<Word> GetWordsByDailyProgress(int progressId)
        {
            return context.Words.Where(x => x.DailyProgressId == progressId);
        }
    }
}
