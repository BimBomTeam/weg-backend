using WEG.Infrastructure.Dto.Dialog;
using WEG.Infrastructure.Services;

namespace WEG.Application.Services
{
    public class DialogService : IDialogService
    {
        private readonly IAiService aiService;
        public DialogService()
        {
            this.aiService = new AiService();
        }
        public DialogResponseDto GetDialogResponse(DialogRequestDto message)
        {
            string responseStr = aiService.DevelopMessageByAi(message.Message);
            DialogResponseDto dialogResponse = new DialogResponseDto() { Response = responseStr };
            return dialogResponse;
        }
    }
}
