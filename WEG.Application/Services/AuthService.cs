﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WEG.Application.Claims;
using WEG.Infrastructure.Models;
using WEG.Infrastructure.Services;
using WEG.Domain.Entities;
using WEG.Infrastructure.Dto;

namespace WEG.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                throw new Exception("User already exists.");

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            return result;
        }
        public async Task<TokenModel?> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
            {
                return null;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(JwtClaims.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateToken(authClaims);

            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(3);

            await _userManager.UpdateAsync(user);

            return new TokenModel() { AccessToken = token, RefreshToken = refreshToken };
        }
        public async Task<JwtSecurityToken?> LogoutAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                throw new Exception("User not found");

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
           issuer: _configuration["JWT:ValidIssuer"],
           audience: _configuration["JWT:ValidAudience"],
           expires: DateTime.UtcNow.AddMinutes(0),
           claims: authClaims,
           signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
           );
            return token;
        }
        //public async Task<string> LoginTokenRefreshAsync(LoginModel model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        var userRoles = await _userManager.GetRolesAsync(user);

        //        var refreshToken = GenerateRefreshToken();

        //        user.RefreshToken = refreshToken;
        //        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(3);

        //        await _userManager.UpdateAsync(user);

        //        return refreshToken;
        //    }
        //    return null;
        //}

        private JwtSecurityToken GenerateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(60),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private bool VerifyToken(string token)
        {
            var validationParameters = new TokenValidationParameters()
            {
                //IssuerSigningToken = new BinarySecretSecurityToken(_key),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidateLifetime = false,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (SecurityTokenException)
            {
                return false;
            }
            catch (Exception e)
            {
                throw;
            }
            return validatedToken != null;
        }
        public async Task<TokensDto> RefreshTokenAsync(TokensDto tokens)
        {
            if (!VerifyToken(tokens.AccessToken.ToString()))
                throw new ArgumentException("Invalid token");

            var emailFromToken = (string)new JwtSecurityToken(tokens.AccessToken).Payload[JwtClaims.Email];
            var user = await _userManager.FindByEmailAsync(emailFromToken);
            if (user.RefreshToken != tokens.RefreshToken)
                throw new ArgumentException("Invalid token");

            var authClaims = new List<Claim>
                {
                    new Claim(JwtClaims.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            var newToken = GenerateToken(authClaims);
            var newRefreshToken = GenerateRefreshToken();
            return new TokensDto() { AccessToken = new JwtSecurityTokenHandler().WriteToken(newToken), RefreshToken = newRefreshToken };
        }
    }
}

