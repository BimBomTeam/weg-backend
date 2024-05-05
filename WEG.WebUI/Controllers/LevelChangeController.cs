using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Services;

namespace WEG_Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LevelChangeController : Controller
    {
        ILevelChangeService levelChangeService;
        public LevelChangeController(ILevelChangeService levelChangeService)
        {
            this.levelChangeService = levelChangeService;
        }
        [HttpPost(Name = "change-level")]
        public async Task<IActionResult> LevelChange([FromBody] ChangeLevelRequestDto request)
        {
            var response = await levelChangeService.ChangeLevel(request);
            return Ok(response);
        }

    }
}
