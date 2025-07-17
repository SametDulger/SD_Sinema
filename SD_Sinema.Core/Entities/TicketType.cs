using System.ComponentModel.DataAnnotations;

namespace SD_Sinema.Core.Entities
{
    public class TicketType : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        public decimal? DiscountPercentage { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
} 