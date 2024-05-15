using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using WEG.Application.Services;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Dto.Boss;
using WEG.Infrastructure.Dto.Dialog;
using WEG.Infrastructure.Dto.WordsGenerate;
using WEG.Infrastructure.Models;
using WEG.Infrastructure.Services;

namespace WEG_Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AiCommunicationController : Controller
    {
        private readonly IAiCommunicationService aiCommunicationService;
        private readonly IDialogService dialogService;

        public AiCommunicationController(
            IAiCommunicationService aiCommunicationService,
            IDialogService dialogService)
        {
            this.aiCommunicationService = aiCommunicationService;
            this.dialogService = dialogService;
        }

        [HttpPost("generate-words")]
        public async Task<IActionResult> GenerateWords([FromBody] GenerateWordsRequestDto request)
        {
            try
            {
                var response = await aiCommunicationService.GenerateWordsAsync(request.Level, request.Role);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("start-dialog")]
        public async Task<IActionResult> StartDialog([FromBody] StartDialogDto dto)
        {
            try
            {
                var response = await dialogService.StartDialogAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("continue-dialog")]
        public async Task<IActionResult> ContinueDialog([FromBody] ContinueDialogDto dto)
        {
            try
            {
                var response = await dialogService.ContinueDialogAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("get-boss-quiz")]
        public async Task<IActionResult> GetBossQuiz([FromBody] BossQuizRequestDto dto)
        {
            try
            {
                var response = await aiCommunicationService.GenerateBossQuizAsync(dto.Word);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
