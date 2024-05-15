using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Infrastructure.Queries;
using WEG.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WEG.Application.Queries
{
    public class GameDayQuery : BaseQuery<GameDay, int>, IGameDayQuery
    {
        public GameDayQuery(ApplicationDbContext context) : base(context) { }

        public async Task<GameDay?> GetTodayGameDayAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var result = await context.GameDays.FirstOrDefaultAsync(x => x.Date.Equals(today));
            return result;
        }
    }
}
