using Microsoft.Extensions.Configuration;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Dto.Dialog;
using WEG.Infrastructure.Services;

namespace WEG.Application.Services
{
    public class DialogService : IDialogService
    {
        private readonly IAiService aiService;
        private readonly IAiCommunicationService aiCommunicationService;
        public DialogService(IConfiguration config)
        {
            this.aiService = new AiService();
            this.aiCommunicationService = new AiCommunicationService(config);
        }
        
        public DialogResponseDevelopedAiDto GetDialogResponse(DialogResponseDevelopedAiDto message)
        {
            var responseStr = aiService.DevelopMessageByAi(message.Message);
            DialogResponseDevelopedAiDto dialogResponse = new DialogResponseDevelopedAiDto { Message = responseStr };
            return dialogResponse;
        }
        public async Task<DialogResponseDto> GetMessageFromAsync(DialogRequestDto message)
        {
            var responseStr = await aiCommunicationService.GetMessageFromAi(message.Message);
            DialogResponseDto dialogResponse = new DialogResponseDto() { Response = responseStr };
            return dialogResponse;
        }
    }
}
