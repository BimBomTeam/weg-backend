using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Application.Commands;
using WEG.Domain.Entities;

namespace WEG.Infrastructure.Commands
{
    public interface IWordsCommand : IBaseCommand<Word,int>
    {
    }
}
