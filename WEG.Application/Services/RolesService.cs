using AutoMapper;
using System.Text.Json;
using WEG.Application.Commands;
using WEG.Domain.Entities;
using WEG.Infrastructure.Commands;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Dto.Roles;
using WEG.Infrastructure.Queries;
using WEG.Infrastructure.Services;

namespace WEG.Application.Services
{
    public class RolesService : IRolesService
    {
        private readonly Random rnd;
        private readonly INpcRoleCommand roleCommand;
        private readonly INpcRolesQuery roleQuery;
        private readonly IRedisCacheService redisService;
        private readonly IGameDayService gameDayService;
        private readonly IMapper mapper;

        public RolesService(IRedisCacheService redisService,
            INpcRoleCommand npcRoleCommand,
            INpcRolesQuery npcRoleQuery,
            IGameDayService gameDayService,
            IMapper mapper)
        {
            rnd = new Random();
            this.redisService = redisService;
            this.roleCommand = npcRoleCommand;
            this.roleQuery = npcRoleQuery;
            this.gameDayService = gameDayService;
            this.mapper = mapper;
        }
        private async Task<IEnumerable<string>> GetRandomRolesFromPoolAsync(int count = 5)
        {
            try
            {
                return await Task.Run(() =>
            {
                string jsonFilePath = "Static/roles-pool.json";
                string jsonString = File.ReadAllText(jsonFilePath);
                var allRoles = JsonSerializer.Deserialize<List<string>>(jsonString);

                if (allRoles == null || allRoles.Count < 1)
                    throw new FileLoadException("File with roles is not correct");

                HashSet<int> pickedIndices = new HashSet<int>();
                List<string> randomElements = new List<string>();

                while (randomElements.Count < count)
                {
                    int index = rnd.Next(allRoles.Count);

                    if (pickedIndices.Add(index))
                        randomElements.Add(allRoles.ElementAt(index));
                }
                return randomElements;
            });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task GenerateNewWordsAsync()
        {
            try
            {
                var todayGameDay = await gameDayService.GetTodayDay();

                var roles = await GetRandomRolesFromPoolAsync();
                var dbRoles = new List<NpcRole>();
                foreach (var roleName in roles)
                {
                    NpcRole role = new NpcRole()
                    {
                        DayId = todayGameDay.Id,
                        Name = roleName
                    };
                    await roleCommand.AddAsync(role);
                    dbRoles.Add(role);
                }

                await roleCommand.SaveChangesAsync();

                List<RolesRedisDto> rolesRedisDtos = new List<RolesRedisDto>();
                foreach (var role in dbRoles)
                {
                    rolesRedisDtos.Add(new RolesRedisDto()
                    {
                        Id = role.Id,
                        Name = role.Name,
                    });
                }
                await redisService.ClearAllRolesAsync();
                await redisService.SaveRolesAsync(rolesRedisDtos);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            if (await redisService.IsEmptyAsync())
                await GenerateNewWordsAsync();

            var redisDtos = await redisService.GetAllRolesAsync();
            var result = new List<RoleDto>();
            foreach (var dto in redisDtos)
            {
                result.Add(new RoleDto() { Id = dto.Id, Name = dto.Name });
            }
            return result;
        }
        public async Task<RoleDto> GetByIdRole(int roleId)
        {
            RoleDto role = mapper.Map<RoleDto>(await redisService.GetRoleAsync(roleId.ToString()));
            if (role == null)
                role = mapper.Map<RoleDto>(await roleQuery.GetByIdAsync(roleId));

            return role;
        }
    }
}
