using System.ComponentModel.DataAnnotations;

namespace SD_Sinema.Core.Entities
{
    public class Genre : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
} 