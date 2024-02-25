using WEG.Infrastructure.Dto;

namespace WEG.Infrastructure.Services
{
    public interface IDialogService
    {
        DialogResponseDto GetDialogResponse(DialogRequestDto requestDto);
    }
}
