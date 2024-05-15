using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Services;

namespace WEG_Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WordsController : Controller
    {
        private readonly IWordService _wordService;
        public WordsController(IWordService wordService)
        {
            _wordService = wordService;
        }

        [HttpPost("get-words/{roleId}")]
        public async Task<IActionResult> GetWordsByRole(int roleId)
        {
            try
            {
                var words = await _wordService.GetWordsByRoleAsync(roleId);
                return Ok(words);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("save-words")]
        public async Task<IActionResult> SaveWords([FromBody] IEnumerable<WordDto> wordsDto)
        {
            try
            {
                await _wordService.SaveWordsAsync(wordsDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
