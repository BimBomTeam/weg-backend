using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Infrastructure.Queries;
using WEG.Domain.Entities;

namespace WEG.Application.Queries
{
    public class GameDayQuery : BaseQuery<GameDay,int>, IGameDayQuery
    {
        public GameDayQuery(ApplicationDbContext context) : base(context) { }       
    }
}
