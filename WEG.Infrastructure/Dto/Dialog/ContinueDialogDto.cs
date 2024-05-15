namespace WEG.Infrastructure.Dto.Dialog
{
    public class ContinueDialogDto
    {
        public IEnumerable<DialogDto> Messages { get; set; }
        public string MessageStr { get; set; }
        public IEnumerable<WordDto> Words { get; set; }
    }
}
