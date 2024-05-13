using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WEG.Infrastructure.Dto.Boss
{
    public class BossQuizUnitDto
    {
        [JsonPropertyName("word")]
        public string Word { get; set; }

        [JsonPropertyName("answers")]
        public IEnumerable<BossQuizAnswer> Answers { get; set; }
    }
}
