using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Domain.Entities;
using WEG.Infrastructure.Commands;

namespace WEG.Application.Commands
{
    public class DailyProgressStatsCommand : BaseCommand<DailyProgressStats,int>,IDailyProgressStatsCommand
    {
        public DailyProgressStatsCommand(ApplicationDbContext context) : base(context) { }
    }
}
