namespace WEG.Infrastructure.Dto
{
    public class TtsInputDto
    {
        public string Model { get; set; }
        public string Input { get; set; }
        public string Voice { get; set; }
        public decimal Speed { get; set; }
        public TtsInputDto(string input)
        {
            Model = "tts-1";
            Voice = "alloy";
            Speed = 1;
            Input = input;
        }
    }
}
