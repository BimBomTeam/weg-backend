namespace WEG.Infrastructure.Services
{
    public interface IRolesService
    {
        public Task<IEnumerable<string>> GetRandomRolesFromPoolAsync(int count = 5);
    }
}
