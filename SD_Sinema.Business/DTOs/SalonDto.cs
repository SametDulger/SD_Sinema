namespace SD_Sinema.Business.DTOs
{
    public class SalonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateSalonDto
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateSalonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
} 