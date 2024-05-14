using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Domain.Entities;
using WEG.Infrastructure.Dto;

namespace WEG.Infrastructure.Services
{
    public interface IWordService
    {
        Task<IEnumerable<WordDto>> GetWordsByRoleAsync(int roleId);
        Task UpdateWordStatusAsync(int wordId, WordProgressState newState);
    }
}
