using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEG.Infrastructure.Dto;

namespace WEG_Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        [HttpGet("get-today-roles")]
        public async Task<IActionResult> GetTodayRoles()
        {
            var result = new List<RoleDto>()
            {
                new RoleDto() { Id = 1, Name = "Wairtress" },
                new RoleDto() { Id = 2, Name = "Cooker" },
                new RoleDto() { Id = 3, Name = "Zoologist" },
                new RoleDto() { Id = 4, Name = "Buisnessman" },
                new RoleDto() { Id = 5, Name = "Teacher" }
            };
            return Ok(result);
        }
    }
}
