using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WEG.Domain.Entities;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Dto.Dialog;
using WEG.Infrastructure.Services;

namespace WEG.Application.Services
{
    public class DialogService : IDialogService
    {
        private readonly IAiCommunicationService aiCommunicationService;
        private readonly IWordService wordService;
        private readonly IAuthService authService;
        public DialogService(IAiCommunicationService aiCommunicationService,
            IWordService wordService,
            IAuthService authService)
        {
            this.aiCommunicationService = aiCommunicationService;
            this.wordService = wordService;
            this.authService = authService;
        }
        public async Task<IEnumerable<DialogDto>> StartDialogAsync(StartDialogDto dto)
        {
            try
            {
                var user = await authService.GetUserFromRequest();

                var level = user.Level;

                var result = await aiCommunicationService.StartDialogAsync(dto.Role, level.ToString(), dto.WordsStr);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
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
