using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Infrastructure.Dto;

namespace WEG.Infrastructure.Services
{
    public interface IAiCommunicationService
    {
        Task<string> GetMessageFromAi(string message);
        Task<IEnumerable<DialogDto>> ContinueDialog(IEnumerable<DialogDto> messages, string messageStr);
        Task<IEnumerable<DialogDto>> StartDialog(string role, string level, string wordsArray);
    }
}
