using System.ComponentModel.DataAnnotations;

namespace SD_Sinema.Core.Entities
{
    public class Movie : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        public int Duration { get; set; }
        
        [StringLength(50)]
        public string? Director { get; set; }
        
        [StringLength(100)]
        public string? Cast { get; set; }
        
        public int? GenreId { get; set; }
        public virtual Genre? Genre { get; set; }
        
        [StringLength(10)]
        public string? AgeRating { get; set; }
        
        [StringLength(200)]
        public string? PosterUrl { get; set; }
        
        [StringLength(200)]
        public string? TrailerUrl { get; set; }
        
        public DateTime ReleaseDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
} 