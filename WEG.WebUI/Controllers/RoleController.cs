using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEG.Infrastructure.Services;

namespace WEG_Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IRolesService _rolesService;
        public RoleController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet("get-today-roles")]
        public async Task<IActionResult> GetTodayRoles()
        {
            var result = await _rolesService.GetAllRolesAsync();
            return Ok(result);
        }
        [HttpPost("execute-role-changing")]
        public async Task<IActionResult> ExecuteRoleChanging()
        {
            try
            {
                await _rolesService.GenerateNewWordsAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
