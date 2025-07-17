using System.ComponentModel.DataAnnotations;

namespace SD_Sinema.Core.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        public DateTime? BirthDate { get; set; }
        
        [StringLength(10)]
        public string? Gender { get; set; }
        
        [StringLength(50)]
        public string? Profession { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public bool IsEmailConfirmed { get; set; } = false;
        
        public bool IsVipMember { get; set; } = false;
        
        public DateTime? VipStartDate { get; set; }
        
        public DateTime? VipEndDate { get; set; }
        
        public int MonthlyFreeMovieCount { get; set; } = 0;
        
        public int UsedFreeMovieCount { get; set; } = 0;
        
        public string Role { get; set; } = "InternetKullanici";
        
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
} 