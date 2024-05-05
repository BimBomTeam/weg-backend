using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Dto.Dialog;

namespace WEG.Infrastructure.Services
{
    public interface IDialogService
    {
        Task<DialogResponseDto> GetMessageFromAsync(DialogRequestDto message);
        DialogResponseDevelopedAiDto GetDialogResponse(DialogResponseDevelopedAiDto requestDto);
    }
}
