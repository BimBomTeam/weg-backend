using System;
using System.ComponentModel.DataAnnotations;

namespace WEG.Domain.Entities
{
    public class NpcRole : BaseEntity<int>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int DayId { get; set; }
        public virtual GameDay Day { get; set; }

        public virtual ICollection<Word> Words { get; set; } = new List<Word>();
    }
}
