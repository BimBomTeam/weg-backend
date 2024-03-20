using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using WEG.Application.Resources.Models;


namespace WEG.Application.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterModel model);
        Task<JwtSecurityToken?> LoginAsync(LoginModel model);
        JwtSecurityToken GetToken(List<Claim> authClaims);
    }
}