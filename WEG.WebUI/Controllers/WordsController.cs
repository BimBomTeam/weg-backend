using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEG.Infrastructure.Dto;

namespace WEG_Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WordsController : Controller
    {
        [HttpPost("get-words/{roleId}")]
        public async Task<IActionResult> GetWordsByRole(int roleId)
        {
            var result = new List<WordDto>() 
            {
                new WordDto() { Id = 1, Name = "XYZ", State = "InProgress", RoleId = roleId },
                new WordDto() { Id = 2, Name = "Second word", State = "Learned", RoleId = roleId },
                new WordDto() { Id = 3, Name = "VEEEERY LOOOOONG", State = "Approved", RoleId = roleId },
                new WordDto() { Id = 4, Name = "Short", State = "InProgress", RoleId = roleId },
                new WordDto() { Id = 5, Name = "Thats all.", State = "Approved", RoleId = roleId },
            };
            return Ok(result);
        }
    }
}
