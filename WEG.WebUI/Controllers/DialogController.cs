using Microsoft.AspNetCore.Mvc;
using WEG.Infrastructure.Services;
using WEG.Infrastructure.Dto;
using Microsoft.AspNetCore.Authorization;
using WEG_Server.Controllers;

namespace WEG.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DialogController : Controller
    {
        IDialogService dialogService;
        private readonly ILogger<AuthenticateController> _logger;

        public DialogController(IDialogService dialogService  , ILogger<AuthenticateController> logger)
        {
            this.dialogService = dialogService;
            this._logger = logger;
        }
        [HttpPost(Name = "get-response")]
        public async Task<IActionResult> GetResponse([FromBody] DialogResponseDevelopedAiDto request)
        {
            var response = dialogService.GetDialogResponse(request);
            _logger.LogInformation("Sent response " + DateTime.Now);
            return Ok(response);
        }

    }
}
