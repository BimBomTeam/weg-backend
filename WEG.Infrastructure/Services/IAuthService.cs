using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WEG.Domain.Entities;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Models;

namespace WEG.Infrastructure.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterModel model);
        Task<TokenModel?> LoginAsync(LoginModel model);
        Task LogoutAsync(string token);
        Task<TokensDto> RefreshTokenAsync(TokensDto token);
        Task<ApplicationUser> GetUserFromRequest();
    }
}