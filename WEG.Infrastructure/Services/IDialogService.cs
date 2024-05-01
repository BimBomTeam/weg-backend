using WEG.Infrastructure.Dto;

namespace WEG.Infrastructure.Services
{
    public interface IDialogService
    {
        DialogResponseDto GetMessageFrom(DialogRequestDto message);
        DialogResponseDevelopedAiDto GetDialogResponse(DialogResponseDevelopedAiDto requestDto);
    }
}
