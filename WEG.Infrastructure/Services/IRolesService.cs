using WEG.Infrastructure.Dto;

namespace WEG.Infrastructure.Services
{
    public interface IRolesService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task GenerateNewWordsAsync();
    }
}
