using Microsoft.AspNetCore.Mvc;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Services;

namespace WEG_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LevelChangeController : Controller
    {
        IDialogService dialogService;

        public DialogController(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }
        [HttpPost(Name = "change-level")]
        public async Task<IActionResult> GetResponse([FromBody] DialogResponseDevelopedAiDto request)
        {
            var response = dialogService.GetDialogResponse(request);
            return Ok(response);
        }

    }
}
