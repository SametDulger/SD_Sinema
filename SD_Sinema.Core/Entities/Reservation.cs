using System.ComponentModel.DataAnnotations;

namespace SD_Sinema.Core.Entities
{
    public class Reservation : BaseEntity
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int SessionId { get; set; }
        
        [Required]
        public int SeatId { get; set; }
        
        [Required]
        public DateTime ReservationDate { get; set; }
        
        [Required]
        public DateTime ExpiryDate { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";
        
        [StringLength(200)]
        public string? Notes { get; set; }
        
        public decimal? Price { get; set; }
        
        public virtual User User { get; set; } = null!;
        public virtual Session Session { get; set; } = null!;
        public virtual Seat Seat { get; set; } = null!;
    }
} 