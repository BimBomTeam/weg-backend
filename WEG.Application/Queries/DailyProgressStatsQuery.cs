﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Domain.Entities;
using WEG.Infrastructure.Queries;

namespace WEG.Application.Queries
{
    internal class DailyProgressStatsQuery : BaseQuery<DailyProgressStats,int> ,IDailyProgressStatsQuery
    {
        public DailyProgressStatsQuery(ApplicationDbContext context) : base(context) { }
    }
}