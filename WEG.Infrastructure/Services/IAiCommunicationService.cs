using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Dto.Boss;
using WEG.Infrastructure.Dto.WordsGenerate;

namespace WEG.Infrastructure.Services
{
    public interface IAiCommunicationService
    {
        Task<GenerateWordsResponseDto> GenerateWordsAsync(string level, string role);
        Task<IEnumerable<DialogDto>> ContinueDialogAsync(IEnumerable<DialogDto> messages, string messageStr);
        Task<IEnumerable<DialogDto>> StartDialogAsync(string role, string level, string wordsArray);
        Task<BossQuizUnitDto> GenerateBossQuizAsync(string wordtoTranslate);
    }
}
