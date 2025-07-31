namespace SD_Sinema.Business.DTOs
{
    public class SessionDto
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int SalonId { get; set; }
        public DateTime SessionDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsSpecialSession { get; set; }
        public string? SpecialSessionName { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string SalonName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public decimal? Price { get; set; }
    }

    public class CreateSessionDto
    {
        public int MovieId { get; set; }
        public int SalonId { get; set; }
        public DateTime SessionDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSpecialSession { get; set; }
        public string? SpecialSessionName { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateSessionDto
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int SalonId { get; set; }
        public DateTime SessionDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsSpecialSession { get; set; }
        public string? SpecialSessionName { get; set; }
        public decimal Price { get; set; }
    }
} 