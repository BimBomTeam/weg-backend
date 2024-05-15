using WEG.Domain.Entities;

namespace WEG.Infrastructure.Services
{
    public interface IGameDayService
    {
        Task<GameDay> GetTodayDay();
    }
}
