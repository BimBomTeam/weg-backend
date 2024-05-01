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
        WordsQuery(ApplicationDbContext context) : base(context) { }
    }
}
