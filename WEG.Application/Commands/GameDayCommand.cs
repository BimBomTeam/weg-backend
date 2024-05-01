using WEG.Domain.Entities;
using WEG.Infrastructure.Commands;

namespace WEG.Application.Commands
{
    public class GameDayCommand : BaseCommand<GameDay, int>, IGameDayCommand
    {
        public GameDayCommand(ApplicationDbContext context) : base(context) { }
    }
}
