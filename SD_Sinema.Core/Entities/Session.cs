using System.ComponentModel.DataAnnotations;

namespace SD_Sinema.Core.Entities
{
    public class Session : BaseEntity
    {
        [Required]
        public int MovieId { get; set; }
        
        [Required]
        public int SalonId { get; set; }
        
        [Required]
        public DateTime SessionDate { get; set; }
        
        [Required]
        public TimeSpan StartTime { get; set; }
        
        public TimeSpan EndTime { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public bool IsSpecialSession { get; set; } = false;

        [Required]
        public decimal Price { get; set; }
        
        [StringLength(100)]
        public string? SpecialSessionName { get; set; }
        
        public virtual Movie Movie { get; set; } = null!;
        public virtual Salon Salon { get; set; } = null!;
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
} 