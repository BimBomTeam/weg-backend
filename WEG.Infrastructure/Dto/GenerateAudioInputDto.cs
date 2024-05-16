using System.Diagnostics.CodeAnalysis;

namespace WEG.Infrastructure.Dto
{
    public class GenerateAudioInputDto
    {
        public string Input { get; set; }
        [AllowNull]
        public string Voice { get; set; }
    }
}
