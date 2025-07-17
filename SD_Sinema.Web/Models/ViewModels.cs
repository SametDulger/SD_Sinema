using System.ComponentModel.DataAnnotations;

namespace SD_Sinema.Web.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad alanı zorunludur")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta alanı zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon alanı zorunludur")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Şifre")]
        public string? Password { get; set; }

        [Display(Name = "Doğum Tarihi")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Cinsiyet")]
        public string? Gender { get; set; }

        [Display(Name = "Meslek")]
        public string? Profession { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class MovieViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Film adı zorunludur")]
        [Display(Name = "Film Adı")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Süre zorunludur")]
        [Display(Name = "Süre (dakika)")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Yayın yılı zorunludur")]
        [Display(Name = "Yayın Yılı")]
        public int ReleaseYear { get; set; }

        [Required(ErrorMessage = "Tür zorunludur")]
        [Display(Name = "Tür")]
        public string Genre { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }

    public class SalonViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Salon adı zorunludur")]
        [Display(Name = "Salon Adı")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kapasite zorunludur")]
        [Display(Name = "Kapasite")]
        public int Capacity { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class SessionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Film seçimi zorunludur")]
        [Display(Name = "Film")]
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Salon seçimi zorunludur")]
        [Display(Name = "Salon")]
        public int SalonId { get; set; }

        [Required(ErrorMessage = "Başlangıç zamanı zorunludur")]
        [Display(Name = "Başlangıç Zamanı")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur")]
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties for display
        public string MovieTitle { get; set; } = string.Empty;
        public string SalonName { get; set; } = string.Empty;
    }

    public class TicketTypeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bilet türü adı zorunludur")]
        [Display(Name = "Bilet Türü Adı")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fiyat zorunludur")]
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class SeatViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Salon seçimi zorunludur")]
        [Display(Name = "Salon")]
        public int SalonId { get; set; }

        [Required(ErrorMessage = "Koltuk numarası zorunludur")]
        [Display(Name = "Koltuk Numarası")]
        public string SeatNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sıra numarası zorunludur")]
        [Display(Name = "Sıra Numarası")]
        public int RowNumber { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation property for display
        public string SalonName { get; set; } = string.Empty;
    }

    public class ReservationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı seçimi zorunludur")]
        [Display(Name = "Kullanıcı")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Seans seçimi zorunludur")]
        [Display(Name = "Seans")]
        public int SessionId { get; set; }

        [Required(ErrorMessage = "Rezervasyon tarihi zorunludur")]
        [Display(Name = "Rezervasyon Tarihi")]
        public DateTime ReservationDate { get; set; }

        [Display(Name = "Koltuk Sayısı")]
        public int SeatCount { get; set; } = 1;

        [Display(Name = "Toplam Fiyat")]
        public decimal TotalPrice { get; set; } = 0;

        [Display(Name = "Durum")]
        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        // Navigation properties for display
        public string UserName { get; set; } = string.Empty;
        public string SessionInfo { get; set; } = string.Empty;
        [Required(ErrorMessage = "Koltuk seçimi zorunludur")]
        [Display(Name = "Koltuk")]
        public int SeatId { get; set; }
    }

    public class TicketViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Rezervasyon seçimi zorunludur")]
        [Display(Name = "Rezervasyon")]
        public int ReservationId { get; set; }

        [Required(ErrorMessage = "Bilet türü seçimi zorunludur")]
        [Display(Name = "Bilet Türü")]
        public int TicketTypeId { get; set; }

        [Required(ErrorMessage = "Koltuk seçimi zorunludur")]
        [Display(Name = "Koltuk")]
        public int SeatId { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur")]
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }

        [Display(Name = "Durum")]
        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        // Navigation properties for display
        public string ReservationInfo { get; set; } = string.Empty;
        public string TicketTypeName { get; set; } = string.Empty;
        public string SeatInfo { get; set; } = string.Empty;
    }
} 