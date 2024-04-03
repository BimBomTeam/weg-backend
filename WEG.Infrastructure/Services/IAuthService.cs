﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Models;

namespace WEG.Infrastructure.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterModel model);
        Task<TokenModel?> LoginAsync(LoginModel model);
        //Task<string> LoginTokenRefreshAsync(LoginModel model);
        //JwtSecurityToken GetToken(List<Claim> authClaims);
        Task<JwtSecurityToken?> LogoutAsync(LoginModel model);

        Task<TokensDto> RefreshTokenAsync(TokensDto token);

    }
}