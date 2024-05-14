using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WEG.Domain.Entities;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Dto.Dialog;
using WEG.Infrastructure.Queries;
using WEG.Infrastructure.Services;

namespace WEG.Application.Services
{
    public class DialogService : IDialogService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAiCommunicationService aiCommunicationService;
        private readonly INpcRolesQuery rolesQuery;
        public DialogService(UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IAiCommunicationService aiCommunicationService,
            INpcRolesQuery rolesQuery)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            this.aiCommunicationService = aiCommunicationService;
            this.rolesQuery = rolesQuery;
        }
        public async Task<IEnumerable<DialogDto>> StartDialogAsync(StartDialogDto dto)
        {
            var userEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            if (userEmail == null)
                throw new ArgumentException("Token is invalid");

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
                throw new ArgumentException("Token is invalid");

            var level = user.Level;

            var result = await aiCommunicationService.StartDialogAsync(dto.Role, level.ToString(), dto.WordsStr);

            return result;
        }
        public async Task<IEnumerable<DialogDto>> ContinueDialogAsync(ContinueDialogDto dto)
        {
            try
            {
                var response = await aiCommunicationService.ContinueDialogAsync(dto.Messages, dto.MessageStr);
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
