using WEG.Domain.Entities;

namespace WEG.Infrastructure.Queries
{
    public interface IDailyProgressStatsQuery : IBaseQuery<DailyProgressStats,int>
    {
        Task<DailyProgressStats?> GetUserDailyProgress(GameDay day, string userId);
    }
}
