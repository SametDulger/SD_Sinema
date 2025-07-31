using System.ComponentModel.DataAnnotations;

namespace SD_Sinema.Core.Entities
{
    public class Seat : BaseEntity
    {
        [Required]
        public int SalonId { get; set; }
        
        [Required]
        [StringLength(10)]
        public string RowNumber { get; set; } = string.Empty;
        
        [Required]
        public int SeatNumber { get; set; }
        
        public int? SeatTypeId { get; set; }
        public virtual SeatType? SeatType { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public bool IsAvailable { get; set; } = true;
        
        public virtual Salon Salon { get; set; } = null!;
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
} 