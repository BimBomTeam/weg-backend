using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WEG.Domain.Entities;
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

            var user = await _userManager.FindByEmailAsync(model.Email);

            var token = await _authService.LoginTokenAsync(model);

            var refreshToken = await _authService.LoginTokenRefreshAsync(model);
            if (token == null)
            {
                return Unauthorized();
            }

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });



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
    }
}
