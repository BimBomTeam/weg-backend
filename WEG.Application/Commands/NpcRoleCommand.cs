using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Domain.Entities;
using WEG.Infrastructure.Commands;

namespace WEG.Application.Commands
{
    public class NpcRoleCommand : BaseCommand<NpcRole,int>, INpcRoleCommand
    {
        public NpcRoleCommand(ApplicationDbContext context) : base(context) { }
    }
}
