using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WEG.Infrastructure.Dto.Boss
{
    public class BossQuizAnswer
    {
        [JsonPropertyName("answer")]
        public string Word { get; set; }


        [JsonPropertyName("correct")]
        public bool IsCorrect { get; set; }

        public BossQuizAnswer(string word, bool isCorrect) {
            Word = word;
            IsCorrect = isCorrect;
        }
    }
}
