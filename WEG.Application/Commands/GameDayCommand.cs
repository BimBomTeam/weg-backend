using WEG.Domain.Entities;
using WEG.Infrastructure.Commands;

namespace WEG.Application.Commands
{
    public class GameDayCommand : BaseCommand<GameDay, int>, IGameDayCommand
    {
        public GameDayCommand(ApplicationDbContext context) : base(context) { }

        public async Task<GameDay> CreateTodayAsync()
        {
            GameDay gameDay = new GameDay() { Date = DateOnly.FromDateTime(DateTime.Now) };
            var result = context.Add(gameDay);
            await SaveChangesAsync();

            return gameDay;
        }
    }
}
