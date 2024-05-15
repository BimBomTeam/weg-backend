using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Domain.Entities;
using WEG.Infrastructure.Commands;

namespace WEG.Application.Commands
{
    public class WordsCommand : BaseCommand<Word, int>, IWordsCommand
    {
        public WordsCommand(ApplicationDbContext context) : base(context) { }

        public async Task ClearWordsForRoleProgressAsync(int progressId, int roleId)
        {
            await Task.Run(() =>
            {
                var toRemove = context.Words.Where(word => word.RoleId == roleId
                    && word.DailyProgressId == progressId);

                context.RemoveRange(toRemove);
            });

        }
    }
}
