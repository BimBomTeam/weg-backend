using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
