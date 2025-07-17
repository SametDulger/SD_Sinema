using System.ComponentModel.DataAnnotations;

namespace SD_Sinema.Core.Entities
{
    public class Salon : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public int Capacity { get; set; }
        
        [StringLength(200)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
} 