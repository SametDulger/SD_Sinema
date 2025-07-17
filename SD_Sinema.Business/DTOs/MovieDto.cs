namespace SD_Sinema.Business.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Duration { get; set; }
        public string? Director { get; set; }
        public string? Cast { get; set; }
        public string? Genre { get; set; }
        public string? AgeRating { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateMovieDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Duration { get; set; }
        public string? Director { get; set; }
        public string? Cast { get; set; }
        public string? Genre { get; set; }
        public string? AgeRating { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class UpdateMovieDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Duration { get; set; }
        public string? Director { get; set; }
        public string? Cast { get; set; }
        public string? Genre { get; set; }
        public string? AgeRating { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
} 