using System;
using System.ComponentModel.DataAnnotations;

namespace WEG.Domain.Entities
{
    public class GameDay : BaseEntity<int>
    {
        [Required]
        public DateTime Date { get; set; }
        public virtual ICollection<NpcRole> NpcRoles { get; set; }
    }
}