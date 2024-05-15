
using WEG.Domain.Entities;

namespace WEG.Infrastructure.Queries
{
    public interface IGameDayQuery : IBaseQuery<GameDay,int>
    {
        public Task<GameDay?> GetTodayGameDayAsync();
    }
}
