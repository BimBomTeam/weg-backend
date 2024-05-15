using WEG.Application.Commands;
using WEG.Domain.Entities;

namespace WEG.Infrastructure.Commands
{
    public interface IGameDayCommand : IBaseCommand<GameDay,int>
    {
        public Task<GameDay> CreateTodayAsync();
    }
}
