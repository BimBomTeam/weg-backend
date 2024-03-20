using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using WEG.Infrastructure.Models;

namespace WEG.Infrastructure.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterModel model);
        Task<JwtSecurityToken?> LoginAsync(LoginModel model);
        JwtSecurityToken GetToken(List<Claim> authClaims);
    }
}