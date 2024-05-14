using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Domain.Entities;

namespace WEG.Infrastructure.Queries
{
    public interface IWordsQuery : IBaseQuery<Word,int>
    {
        IEnumerable<Word> GetWordsByDailyProgressAndRole(int progressId, int roleId);
    }
}
