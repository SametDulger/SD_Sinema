namespace SD_Sinema.Web.Models.DTOs
{
    public class TicketTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateTicketTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateTicketTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public bool IsActive { get; set; }
    }
} 