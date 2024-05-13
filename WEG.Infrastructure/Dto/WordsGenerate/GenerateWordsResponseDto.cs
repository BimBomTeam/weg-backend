using System.Text.Json.Serialization;

namespace WEG.Infrastructure.Dto.WordsGenerate
{
    public class GenerateWordsResponseDto
    {
        [JsonPropertyName("words")]
        public IEnumerable<string> Words { get; set; }
    }
}
