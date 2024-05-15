using WEG.Domain.Entities;
using WEG.Infrastructure.Commands;
using WEG.Infrastructure.Queries;
using WEG.Infrastructure.Services;

namespace WEG.Application.Services
{
    public class UserDailyProgressService : IUserDailyProgressService
    {
        private readonly IGameDayService gameDayService;
        private readonly IDailyProgressStatsQuery dailyProgressQuery;
        private readonly IDailyProgressStatsCommand dailyProgressCommand;
        public UserDailyProgressService(IGameDayService gameDayService,
            IDailyProgressStatsQuery dailyProgressQuery,
            IDailyProgressStatsCommand dailyProgressCommand)
        {
            this.gameDayService = gameDayService;
            this.dailyProgressQuery = dailyProgressQuery;
            this.dailyProgressCommand = dailyProgressCommand;
        }
        public async Task<DailyProgressStats> GetUserTodayProgress(string userId)
        {
            var todayGameDay = await gameDayService.GetTodayDay();

            var userProgress = await dailyProgressQuery.GetUserDailyProgress(todayGameDay, userId);

            if (userProgress == null)
            {
                DailyProgressStats dailyProgress = new DailyProgressStats()
                {
                    DayId = todayGameDay.Id,
                    UserId = userId
                };
                userProgress = await dailyProgressCommand.AddAsync(dailyProgress);
                await dailyProgressCommand.SaveChangesAsync();
            }
            return userProgress;
        }
    }
}
