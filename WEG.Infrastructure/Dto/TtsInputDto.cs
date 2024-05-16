namespace WEG.Infrastructure.Dto
{
    public class TtsInputDto
    {
        public string Model { get; set; }
        public string Input { get; set; }
        public string Voice { get; set; }
        public decimal Speed { get; set; }
        public TtsInputDto(string input, string voice)
        {
            Model = "tts-1";
            if (string.IsNullOrEmpty(voice))
                Voice = "alloy";
            else
                Voice = voice;
            Speed = 1;
            Input = input;
        }
    }
}
