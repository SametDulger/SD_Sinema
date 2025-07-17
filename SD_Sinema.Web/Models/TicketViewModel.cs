    public class TicketViewModel
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int TicketTypeId { get; set; }
        public int SeatId { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string ReservationInfo { get; set; } = string.Empty;
        public string TicketTypeName { get; set; } = string.Empty;
        public string SeatInfo { get; set; } = string.Empty;
    } 