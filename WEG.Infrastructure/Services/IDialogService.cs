using WEG.Infrastructure.Dto.Dialog;

namespace WEG.Infrastructure.Services
{
    public interface IDialogService
    {
        DialogResponseDto GetDialogResponse(DialogRequestDto requestDto);
    }
}
