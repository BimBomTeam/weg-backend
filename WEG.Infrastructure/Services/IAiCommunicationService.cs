using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEG.Infrastructure.Services
{
    public interface IAiCommunicationService
    {
        Task<string> GetMessageFromAi(string message);
    }
}
