using System;
using System.ComponentModel.DataAnnotations;

namespace WEG.Domain.Entities
{
    public class DailyProgressStats : BaseEntity<int>
    {
        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        public int DayId { get; set; }
        public virtual GameDay Day { get; set; }

        public virtual ICollection<Word> Words { get; set; }
    }
}
