using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        public AuthenticateController(IAuthService getTokenService)
        {
            _authService = getTokenService;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var tokens = await _authService.LoginAsync(model);

                if (tokens == null)
                    return Unauthorized("Invalid credentials");

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
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                string? token = await HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token");

                if (token == null)
                    return Unauthorized();

                await _authService.LogoutAsync(token);

                return Ok();
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
