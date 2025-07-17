using System.ComponentModel.DataAnnotations;

namespace SD_Sinema.Core.Entities
{
    public class Ticket : BaseEntity
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int SessionId { get; set; }
        
        [Required]
        public int SeatId { get; set; }
        
        [Required]
        public int TicketTypeId { get; set; }
        
        [Required]
        public DateTime PurchaseDate { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";
        
        [StringLength(50)]
        public string? PaymentMethod { get; set; }
        
        [StringLength(100)]
        public string? TransactionId { get; set; }
        
        public bool IsVipDiscount { get; set; } = false;
        
        public bool IsFreeMovie { get; set; } = false;
        
        public virtual User User { get; set; } = null!;
        public virtual Session Session { get; set; } = null!;
        public virtual Seat Seat { get; set; } = null!;
        public virtual TicketType TicketType { get; set; } = null!;
    }
} 