using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WEG.Domain.Entities;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Dto.Dialog;
using WEG.Infrastructure.Services;

namespace WEG.Application.Services
{
    public class DialogService : IDialogService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAiCommunicationService aiCommunicationService;
        private readonly IWordService wordService;
        public DialogService(UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IAiCommunicationService aiCommunicationService,
            IWordService wordService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            this.aiCommunicationService = aiCommunicationService;
            this.wordService = wordService;
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
        public async Task<ContinueDialogResponseDto> ContinueDialogAsync(ContinueDialogDto dto)
        {
            try
            {
                var checkWordTask = Task.Run(() =>
                {
                    wordService.CheckWordsAsync(dto.Words, dto.MessageStr);
                });
                var getDialogResponse = Task.Run(() =>
                {
                    var response = aiCommunicationService.ContinueDialogAsync(dto.Messages, dto.MessageStr);
                    return response;
                });
                Task.WaitAll(checkWordTask, getDialogResponse);

                var result = new ContinueDialogResponseDto()
                {
                    Words = dto.Words,
                    Dialog = getDialogResponse.Result
                };
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
