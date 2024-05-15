using WEG.Domain.Entities;
using WEG.Infrastructure.Commands;
using WEG.Infrastructure.Queries;
using WEG.Infrastructure.Services;

namespace WEG.Application.Services
{
    public class GameDayService : IGameDayService
    {
        private readonly IGameDayQuery gameDayQuery;
        private readonly IGameDayCommand gameDayCommand;
        public GameDayService(IGameDayQuery gameDayQuery, IGameDayCommand gameDayCommand)
        {
            this.gameDayQuery = gameDayQuery;
            this.gameDayCommand = gameDayCommand;
        }
        public async Task<GameDay> GetTodayDay()
        {
            var todayGameDay = await gameDayQuery.GetTodayGameDayAsync();
            if (todayGameDay == null)
            {
                todayGameDay = await gameDayCommand.CreateTodayAsync();
                await gameDayCommand.SaveChangesAsync();
            }
            return todayGameDay;
        }
    }
}
