using Microsoft.AspNetCore.Http;
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

namespace WEG.Application.Services
{
    public class LevelChangeService : ILevelChangeService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task<IActionResult> ChangeLevel(ChangeLevelRequestDto request)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            // Znajdź użytkownika na podstawie adresu e-mail
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return NotFound("Użytkownik nie został znaleziony.");
            }

        }
    }
}
