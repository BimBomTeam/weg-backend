namespace WEG.Infrastructure.Dto.Dialog
{
    public class ContinueDialogResponseDto
    {
        public IEnumerable<DialogDto> Dialog { get; set; }
        public IEnumerable<WordDto> Words { get; set; }
    }
}
