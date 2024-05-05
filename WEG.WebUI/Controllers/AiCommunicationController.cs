using Microsoft.AspNetCore.Mvc;
using WEG.Application.Services;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Dto.Dialog;
using WEG.Infrastructure.Models;
using WEG.Infrastructure.Services;

namespace WEG_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiCommunicationController : Controller
    {
        IAiCommunicationService aiCommunicationService;
        IAiService aiService;

        public AiCommunicationController(IAiCommunicationService aiCommunicationService, IAiService aiService)
        {
            this.aiCommunicationService = aiCommunicationService;
            this.aiService = aiService;
        }

        [HttpPost("get-responseAI")]
        public async Task<IActionResult> GetFromAi([FromBody] DialogRequestDto request)
        {
            var response = await aiCommunicationService.GetMessageFromAi(request.Message);
            return Ok(response);
        }
        [HttpPost("start-dialog")]
        public async Task<IActionResult> StartDialog([FromBody] StartDialogDto dto)
        {
            var response = await aiCommunicationService.StartDialog(dto.Role, dto.Level, dto.WordsStr);
            return Ok(response);
        }
        [HttpPost("continue-dialog")]
        public async Task<IActionResult> ContinueDialog([FromBody] ContinueDialogDto dto)
        {
            var response = await aiCommunicationService.ContinueDialog(dto.Messages, dto.MessageStr);
            return Ok(response);
        }
    }
}
