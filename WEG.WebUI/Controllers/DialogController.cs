using Microsoft.AspNetCore.Mvc;
using WEG.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using WEG.Infrastructure.Dto.Dialog;
using WEG.Infrastructure.Dto;

namespace WEG.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DialogController : Controller
    {
        IDialogService dialogService;

        public DialogController(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }
        [HttpPost(Name = "get-response")]
        public async Task<IActionResult> GetResponse([FromBody] DialogResponseDevelopedAiDto request)
        {
            var response = dialogService.GetDialogResponse(request);
            return Ok(response);
        }

    }
}
