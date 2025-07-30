namespace SD_Sinema.Web.Models.DTOs
{
    public class TicketDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public int SeatId { get; set; }
        public int TicketTypeId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
        public bool IsVipDiscount { get; set; }
        public bool IsFreeMovie { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string MovieTitle { get; set; } = string.Empty;
        public string SalonName { get; set; } = string.Empty;
        public string SeatInfo { get; set; } = string.Empty;
        public string TicketTypeName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

    public class CreateTicketDto
    {
        public int ReservationId { get; set; }
        public string TicketNumber { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public int SeatId { get; set; }
        public int TicketTypeId { get; set; }
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
    }

    public class UpdateTicketDto
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public string TicketNumber { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public int SeatId { get; set; }
        public int TicketTypeId { get; set; }
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
    }
} 