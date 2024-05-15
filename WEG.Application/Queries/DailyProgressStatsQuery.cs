using Microsoft.EntityFrameworkCore;
using WEG.Domain.Entities;
using WEG.Infrastructure.Queries;

namespace WEG.Application.Queries
{
    public class DailyProgressStatsQuery : BaseQuery<DailyProgressStats, int>, IDailyProgressStatsQuery
    {
        public DailyProgressStatsQuery(ApplicationDbContext context) : base(context) { }

        public async Task<DailyProgressStats?> GetUserDailyProgress(GameDay day, string userId)
        {
            var result = await context.DailyProgresses.FirstOrDefaultAsync(dp => dp.UserId == userId && dp.DayId == day.Id);
            return result;
        }
    }
}
