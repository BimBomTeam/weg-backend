using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEG.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public int Coins { get; set; }
        [Required]
        public UserLevel Level { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public virtual ICollection<DailyProgressStats> DailyProgresses { get; set; }
        public bool FirstLogin { get; set; }
    }


    public enum UserLevel
    {
        A1,
        A2,
        B1,
        B2,
        C1,
        C2
    }
}
