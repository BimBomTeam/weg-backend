using WEG.Application.Commands;
using WEG.Domain.Entities;

namespace WEG.Infrastructure.Commands
{
    public interface IWordsCommand : IBaseCommand<Word,int>
    {
        Task ClearWordsForRoleProgress(int progressId, int roleId);

    }
}
