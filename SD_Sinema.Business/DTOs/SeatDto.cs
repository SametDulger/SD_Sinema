namespace SD_Sinema.Business.DTOs
{
    public class SeatDto
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        public string RowNumber { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
        public bool IsActive { get; set; }
        public string SalonName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

    public class CreateSeatDto
    {
        public int SalonId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public int RowNumber { get; set; }
        public string SeatType { get; set; } = string.Empty;
    }

    public class UpdateSeatDto
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public int RowNumber { get; set; }
        public string SeatType { get; set; } = string.Empty;
    }
} 