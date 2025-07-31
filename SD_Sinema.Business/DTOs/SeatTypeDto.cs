namespace SD_Sinema.Business.DTOs
{
    public class SeatTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal PriceMultiplier { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateSeatTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal PriceMultiplier { get; set; } = 1.0m;
        public bool IsActive { get; set; } = true;
    }

    public class UpdateSeatTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal PriceMultiplier { get; set; }
        public bool IsActive { get; set; }
    }
} 