using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Domain.Entities;
using WEG.Infrastructure.Queries;

namespace WEG.Application.Queries
{
    public class NpcRoleQuery : BaseQuery<NpcRole,int>, INpcRolesQuery
    {
        NpcRoleQuery(ApplicationDbContext context) : base(context) { }  
    }
}
