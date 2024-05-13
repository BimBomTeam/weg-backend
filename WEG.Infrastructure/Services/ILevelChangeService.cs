using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Infrastructure.Dto;

namespace WEG.Infrastructure.Services
{
    public interface ILevelChangeService
    {
        Task<string> ChangeLevel(ChangeLevelRequestDto request);
    }
}
