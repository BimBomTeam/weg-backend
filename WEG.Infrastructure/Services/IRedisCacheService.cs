using WEG.Infrastructure.Dto.Roles;

namespace WEG.Infrastructure.Services
{
    public interface IRedisCacheService
    {
        Task<List<RolesRedisDto>> GetAllRolesAsync();
        Task<RolesRedisDto?> GetRoleAsync(string roleId);
        Task SaveRolesAsync(IEnumerable<RolesRedisDto> roles);
        Task ClearAllRolesAsync();
        Task<bool> IsEmptyAsync();
    }
}
