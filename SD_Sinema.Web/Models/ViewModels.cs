using System.ComponentModel.DataAnnotations;

namespace SD_Sinema.Web.Models
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Film adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Film adı 100 karakterden uzun olamaz.")]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Açıklama 500 karakterden uzun olamaz.")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Süre zorunludur.")]
        [Range(1, 300, ErrorMessage = "Süre 1-300 dakika arasında olmalıdır.")]
        public int Duration { get; set; }
        
        [Required(ErrorMessage = "Yayın yılı zorunludur.")]
        [Range(1900, 2030, ErrorMessage = "Geçerli bir yayın yılı giriniz.")]
        public int ReleaseYear { get; set; }
        
        public int? GenreId { get; set; }
        public string? GenreName { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }

    public class UserViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Ad zorunludur.")]
        [StringLength(50, ErrorMessage = "Ad 50 karakterden uzun olamaz.")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Soyad zorunludur.")]
        [StringLength(50, ErrorMessage = "Soyad 50 karakterden uzun olamaz.")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string Phone { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
        public DateTime BirthDate { get; set; }
        
        [Required(ErrorMessage = "Cinsiyet zorunludur.")]
        public string Gender { get; set; } = string.Empty;
        
        [StringLength(50, ErrorMessage = "Meslek 50 karakterden uzun olamaz.")]
        public string Profession { get; set; } = string.Empty;
        
        // Şifre alanları eklendi
        [StringLength(100, ErrorMessage = "Şifre 100 karakterden uzun olamaz.")]
        public string? Password { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }

    public class SalonViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Salon adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Salon adı 100 karakterden uzun olamaz.")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Kapasite zorunludur.")]
        [Range(1, 500, ErrorMessage = "Kapasite 1-500 arasında olmalıdır.")]
        public int Capacity { get; set; }
        
        [StringLength(200, ErrorMessage = "Açıklama 200 karakterden uzun olamaz.")]
        public string Description { get; set; } = string.Empty;
        
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class SessionViewModel
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int SalonId { get; set; }
        
        [Required(ErrorMessage = "Seans tarihi zorunludur.")]
        public DateTime SessionDate { get; set; }
        
        [Required(ErrorMessage = "Başlangıç saati zorunludur.")]
        public DateTime StartTime { get; set; }
        
        [Required(ErrorMessage = "Bitiş saati zorunludur.")]
        public DateTime EndTime { get; set; }
        
        public bool IsActive { get; set; }
        public bool IsSpecialSession { get; set; }
        public string? SpecialSessionName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public string MovieTitle { get; set; } = string.Empty;
        public string SalonName { get; set; } = string.Empty;
    }

    public class SeatViewModel
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        
        [Required(ErrorMessage = "Koltuk numarası zorunludur.")]
        [StringLength(10, ErrorMessage = "Koltuk numarası 10 karakterden uzun olamaz.")]
        public string SeatNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Sıra numarası zorunludur.")]
        public int RowNumber { get; set; }
        
        public int? SeatTypeId { get; set; }
        public string? SeatTypeName { get; set; }
        
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        
        // Navigation property
        public string SalonName { get; set; } = string.Empty;
    }

    public class ReservationViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public int SeatId { get; set; }
        
        [Required(ErrorMessage = "Rezervasyon tarihi zorunludur.")]
        public DateTime ReservationDate { get; set; }
        
        [Required(ErrorMessage = "Koltuk sayısı zorunludur.")]
        [Range(1, 10, ErrorMessage = "Koltuk sayısı 1-10 arasında olmalıdır.")]
        public int SeatCount { get; set; }
        
        [Required(ErrorMessage = "Toplam fiyat zorunludur.")]
        [Range(0, 1000, ErrorMessage = "Geçerli bir fiyat giriniz.")]
        public decimal TotalPrice { get; set; }
        
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public string UserName { get; set; } = string.Empty;
        public string SessionInfo { get; set; } = string.Empty;
    }

    public class TicketViewModel
    {
        public int Id { get; set; }
        public int? ReservationId { get; set; }
        public int TicketTypeId { get; set; }
        public int SeatId { get; set; } // Eksik property eklendi
        
        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0, 1000, ErrorMessage = "Geçerli bir fiyat giriniz.")]
        public decimal Price { get; set; }
        
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public string ReservationInfo { get; set; } = string.Empty;
        public string TicketTypeName { get; set; } = string.Empty;
        public string SeatInfo { get; set; } = string.Empty;
    }

    public class TicketTypeViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Bilet türü adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Bilet türü adı 50 karakterden uzun olamaz.")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200, ErrorMessage = "Açıklama 200 karakterden uzun olamaz.")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0, 1000, ErrorMessage = "Geçerli bir fiyat giriniz.")]
        public decimal Price { get; set; }
        
        [Range(0, 100, ErrorMessage = "İndirim yüzdesi 0-100 arasında olmalıdır.")]
        public decimal? DiscountPercentage { get; set; }
        
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Yeni ViewModel'ler
    public class HomeViewModel
    {
        public List<MovieViewModel> ActiveMovies { get; set; } = new();
        public List<SessionViewModel> UpcomingSessions { get; set; } = new();
        public StatisticsViewModel Statistics { get; set; } = new();
    }

    public class StatisticsViewModel
    {
        public int TotalMovies { get; set; }
        public int TotalSalons { get; set; }
        public int TotalUsers { get; set; }
        public int TotalSessions { get; set; }
    }
} 