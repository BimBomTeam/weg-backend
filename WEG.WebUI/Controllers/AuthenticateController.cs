using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WEG.Domain.Entities;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Models;
using WEG.Infrastructure.Services;

namespace WEG_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticateController(IAuthService getTokenService, UserManager<ApplicationUser> userManager)
        {
            _authService = getTokenService;
            _userManager = userManager;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var tokens = await _authService.LoginAsync(model);

                if (tokens == null)
                {
                    return BadRequest("Bad credentials");
                }

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(tokens?.AccessToken),
                    RefreshToken = tokens?.RefreshToken,
                    Expiration = tokens.AccessToken?.ValidTo
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout([FromBody] LoginModel model)
        {
            try
            {
                var token = await _authService.LogoutAsync(model);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var result = await _authService.RegisterAsync(model);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("refresh")] 
        public async Task<IActionResult> Refresh([FromBody] TokensDto dto)
        {
            try
            {
                if (dto == null || dto.AccessToken == null || dto.RefreshToken == null)
                    return BadRequest("Tokens are null");

                var tokens = await _authService.RefreshTokenAsync(dto);

                return Ok(tokens);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
