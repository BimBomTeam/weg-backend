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
    [ApiController]
    [Route("api/[controller]")]
    public class AiCommunicationController : Controller
    {
        IAiCommunicationService aiCommunicationService;

        public AiCommunicationController(IAiCommunicationService aiCommunicationService)
        {
            this.aiCommunicationService = aiCommunicationService;
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
                var response = await aiCommunicationService.StartDialogAsync(dto.Role, dto.Level, dto.WordsStr);
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
                var response = await aiCommunicationService.ContinueDialogAsync(dto.Messages, dto.MessageStr);
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
