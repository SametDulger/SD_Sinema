namespace SD_Sinema.Web.Models.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public int SeatId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public decimal? Price { get; set; }
        public int SeatCount { get; set; }
        public decimal TotalPrice { get; set; }
        public string SessionInfo { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string MovieTitle { get; set; } = string.Empty;
        public string SalonName { get; set; } = string.Empty;
        public string SeatInfo { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

    public class CreateReservationDto
    {
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public int SeatId { get; set; }
        public int TicketTypeId { get; set; }
        public DateTime ReservationDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public decimal? Price { get; set; }
        public int SeatCount { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class UpdateReservationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public int SeatId { get; set; }
        public int TicketTypeId { get; set; }
        public DateTime ReservationDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public decimal? Price { get; set; }
        public int SeatCount { get; set; }
        public decimal TotalPrice { get; set; }
    }
} 