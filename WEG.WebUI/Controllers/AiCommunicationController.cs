using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WEG.Application.Services;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Dto.Dialog;
using WEG.Infrastructure.Models;
using WEG.Infrastructure.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using WEG.Domain.Entities;

namespace WEG_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiCommunicationController : Controller
    {
        IAiCommunicationService aiCommunicationService;
        IAiService aiService;
        private readonly UserManager<ApplicationUser> _userManager;
            public AiCommunicationController(IAiCommunicationService aiCommunicationService, IAiService aiService, UserManager<ApplicationUser> userManager)
        {
            this.aiCommunicationService = aiCommunicationService;
            this.aiService = aiService;
                _userManager = userManager;
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
            string? userId = User.FindFirst(ClaimTypes.Email)?.Value;
            string? userEmail = await HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "email");
            var user = await _userManager.FindByEmailAsync(userEmail);
            var level = user.Level.ToString();
            var xd = "A1";
            var response = await aiCommunicationService.StartDialog(dto.Role, xd, dto.WordsStr);
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
