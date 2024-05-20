using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using WEG.Infrastructure.Dto.Roles;
using WEG.Infrastructure.Services;

namespace WEG.Application.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabaseAsync _db;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IConfiguration conf, ILogger<RedisCacheService> logger)
        {
            this._logger = logger;
            var connection = ConnectionMultiplexer.Connect(conf.GetConnectionString("HangfireConnection"));
            _db = connection.GetDatabase();
        }

        private async Task SaveRoleAsync(RolesRedisDto role)
        {
            string key = $"role:{role.Id}";
            var hashEntries = new HashEntry[]
            {
                new HashEntry("Id", role.Id),
                new HashEntry("Name", role.Name)
            };

            await _db.HashSetAsync(key, hashEntries);
            await _db.SetAddAsync("roles:keys", key);
        }
        public async Task ClearAllRolesAsync()
        {
            RedisValue[] keys = await _db.SetMembersAsync("roles:keys");

            foreach (var key in keys)
            {
                await _db.KeyDeleteAsync(key.ToString());
                await _db.SetRemoveAsync("roles:keys", key);
            }

            await _db.KeyDeleteAsync("roles:keys");
        }
        public async Task<bool> IsEmptyAsync()
        {
            try
            {
                long count = await _db.SetLengthAsync("roles:keys");
                return count < 1;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error is empty: " + DateTime.Now + ex);
                return true;
            }
        }

        public async Task SaveRolesAsync(IEnumerable<RolesRedisDto> roles)
        {
            foreach (var role in roles)
            {
                await SaveRoleAsync(role);
            }
        }
        public async Task<RolesRedisDto?> GetRoleAsync(string roleId)
        {
            string key = $"role:{roleId}";
            HashEntry[] hashEntries = await _db.HashGetAllAsync(key);

            if (hashEntries.Length == 0)
                return null;

            var role = new RolesRedisDto
            {
                Id = int.Parse(hashEntries.FirstOrDefault(h => h.Name == "Id").Value),
                Name = hashEntries.FirstOrDefault(h => h.Name == "Name").Value
            };

            return role;
        }

        public async Task<List<RolesRedisDto>> GetAllRolesAsync()
        {
            var roles = new List<RolesRedisDto>();
            // Retrieve all keys from the set
            RedisValue[] keys = await _db.SetMembersAsync("roles:keys");

            foreach (var key in keys)
            {
                HashEntry[] hashEntries = await _db.HashGetAllAsync(key.ToString());
                if (hashEntries.Length > 0)
                {
                    var role = new RolesRedisDto
                    {
                        Id = int.Parse(hashEntries.FirstOrDefault(h => h.Name == "Id").Value.ToString()),
                        Name = hashEntries.FirstOrDefault(h => h.Name == "Name").Value
                    };
                    roles.Add(role);
                }
            }
            return roles;
        }
    }
}
