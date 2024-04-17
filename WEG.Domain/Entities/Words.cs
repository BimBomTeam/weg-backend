using System;
using System.ComponentModel.DataAnnotations;
namespace WEG.Domain.Entities
{
    public class Word : BaseEntity<int>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int DailyProgressId { get; set; }
        public virtual DailyProgressStats DailyProgress { get; set; }
        [Required]
        public State State { get; set; }
        [Required]
        public int RoleId { get; set; }
        public virtual NpcRole Role { get; set; }


    }
    public enum State
    {
        InProgress,
        Learned,
        Approved,
        Failed,
        NotApproved
    }
}

