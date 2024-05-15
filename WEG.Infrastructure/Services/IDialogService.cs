using WEG.Infrastructure.Dto.Dialog;
using WEG.Infrastructure.Dto;

namespace WEG.Infrastructure.Services
{
    public interface IDialogService
    {
        Task<IEnumerable<DialogDto>> StartDialogAsync(StartDialogDto dto);
        Task<ContinueDialogResponseDto> ContinueDialogAsync(ContinueDialogDto dto);

    }
}
