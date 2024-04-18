using WEG.Infrastructure.Services;

namespace WEG.Application.Services
{
    public class AiService : IAiService
    {
        public string DevelopMessageByAi(string message)
        {
            return "Hello world";
        }
        // ma byc zwracane 5 ról

        /*GetRoles()
        {
            return GRPC.Get();
        }*/
    }
}
