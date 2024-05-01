using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Domain.Entities;
using WEG.Infrastructure.Commands;

namespace WEG.Application.Commands
{
    public class WordsCommand : BaseCommand<Word,int>, IWordsCommand
    {
        public WordsCommand(ApplicationDbContext context) : base(context) { }
    }
}
