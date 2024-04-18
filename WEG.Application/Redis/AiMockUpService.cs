using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Infrastructure.Services;

namespace WEG.Application.Redis
{
    public class AiMockUpService : IAiService// Tworzy przykładową tablice ról
    {
        static List<string> GetRoles()
        {
            //tu jest nasza lista NPCRoles
            return new List<string>() { "Kelner", "Kot", "Zoolog", "Dzielnicowy", "Kominiarz" };
        }

        public string DevelopMessageByAi(string message)
        {
            return "Przykładowa odpowiedź AI dla: " + message;
        }
    }
}
