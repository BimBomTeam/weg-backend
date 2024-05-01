using System.ComponentModel;
using System.Text.Json.Serialization;

namespace WEG.Application.Claims
{
    internal struct PromptsJsonContainer
    {
        /// <summary>
        /// Format string. 
        /// {0} - level
        /// {1} - role
        /// </summary>
        [JsonPropertyName("generate-words-prompt")]
        public string? GenerateWordsPrompt { get; set; }

        /// <summary>
        /// Format string. 
        /// {0} - level
        /// {1} - role
        /// {2} - words list
        /// 
        /// </summary>
        [JsonPropertyName("start-chat-prompt")]
        public string StartChatPrompt { get; set; }

        [JsonPropertyName("get-mistakes-prompt")]
        public string GetMistakesPrompt { get; set; }
    }
}
