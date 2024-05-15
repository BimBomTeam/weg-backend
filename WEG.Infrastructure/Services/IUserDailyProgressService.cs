using WEG.Domain.Entities;

namespace WEG.Infrastructure.Services
{
    public interface IUserDailyProgressService
    {
        Task<DailyProgressStats> GetUserTodayProgress(string userId);
    }
}
