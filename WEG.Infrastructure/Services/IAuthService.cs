using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WEG.Infrastructure.Models;

namespace WEG.Infrastructure.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterModel model);
        Task<JwtSecurityToken?> LoginTokenAsync(LoginModel model);
        Task<string?> LoginTokenRefreshAsync(LoginModel model);
        JwtSecurityToken GetToken(List<Claim> authClaims);
        Task<IActionResult> RefreshTokenAsync(TokenModel tokenModel);
    }
}