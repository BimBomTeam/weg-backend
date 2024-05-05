﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WEG.Infrastructure.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WEG.Domain.Entities;
using WEG.Infrastructure.Services;
using WEG.Application.Claims;
using Azure;

namespace WEG.Application.Services
{
    public class LevelChangeService : ILevelChangeService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LevelChangeService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> ChangeLevel(ChangeLevelRequestDto request)
        {
            var userEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            if (userEmail != null)
            {
                var user = await _userManager.FindByEmailAsync(userEmail);

                if (!Enum.TryParse<UserLevel>(request.NewLevel, out var newLevel))
                {
                    throw new ArgumentException("Invalid language level.");
                }

                user.Level = newLevel;
                await _userManager.UpdateAsync(user);
                return "new level "+user.Level;

            }
            throw new Exception("User access error");


        }
    }
}
