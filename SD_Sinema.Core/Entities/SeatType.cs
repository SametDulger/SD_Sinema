using System.ComponentModel.DataAnnotations;

namespace SD_Sinema.Core.Entities
{
    public class SeatType : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? Description { get; set; }
        
        public decimal PriceMultiplier { get; set; } = 1.0m;
        
        public bool IsActive { get; set; } = true;
        
        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
} 