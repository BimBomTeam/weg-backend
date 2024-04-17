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
        public WordProgressState State { get; set; }
        [Required]
        public int RoleId { get; set; }
        public virtual NpcRole Role { get; set; }


    }
    public enum WordProgressState
    {
        InProgress, //Zostało utworzone, ale nic nie zrobione
        Learned, //Wyuczono, oczekuje na bossa. Po ukonczeniu dnia przechodzi na NotApproved
        Approved, //Wyuczono, potwierdzono bossem
        Failed, //ARCHIVE: Nie prawidłowo potwierdzono bossem
        NotApproved //ARCHIVE: wyuczono, ale nie zatwierdzono bossem
    }
}

