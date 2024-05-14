using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WEG.Application.Claims;
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
        private readonly ILogger<AuthenticateController> _logger;

        public AuthenticateController(IAuthService getTokenService,ILogger<AuthenticateController>  logger)
        {
            _authService = getTokenService;
            this._logger = logger;
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
                _logger.LogInformation("User logged " + tokens);
                string? token = await HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token");
                var firstLogin = User.Claims.FirstOrDefault(claim => claim.Type == JwtClaims.FirstLogin)?.Value;
                var login = token.
                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(tokens?.AccessToken),
                    RefreshToken = tokens?.RefreshToken,
                    FirstLogin = firstLogin,
                    Expiration = tokens.AccessToken?.ValidTo
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during login user" + DateTime.Now + ex);
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
                _logger.LogInformation("Users logout" + DateTime.Now);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error logout user " + DateTime.Now + ex);
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
                _logger.LogInformation("User registered  " + DateTime.Now);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during registration " + DateTime.Now + ex);
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
                _logger.LogInformation("Tokens refresh  " + DateTime.Now);
                return Ok(tokens);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during refresh " + DateTime.Now + ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
