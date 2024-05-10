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
using Microsoft.AspNetCore.Authorization;

namespace WEG_Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AiCommunicationController : Controller
    {
        IAiCommunicationService aiCommunicationService;
        IAiService aiService;
        private readonly UserManager<ApplicationUser> userManager;
            public AiCommunicationController(IAiCommunicationService aiCommunicationService, IAiService aiService, UserManager<ApplicationUser> userManager)
            {
            this.aiCommunicationService = aiCommunicationService;
            this.aiService = aiService;
            this.userManager = userManager;
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
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null)
            {
                return BadRequest("User login error");
            }
            var user = await userManager.FindByEmailAsync(userEmail);
            var userLevel = user.Level.ToString(); 
            var response = await aiCommunicationService.StartDialog(dto.Role, userLevel, dto.WordsStr);
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
