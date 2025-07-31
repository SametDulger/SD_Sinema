using FluentValidation;
using SD_Sinema.Business.DTOs;

namespace SD_Sinema.Business.Validation
{
    public class CreateMovieDtoValidator : AbstractValidator<CreateMovieDto>
    {
        public CreateMovieDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Film başlığı boş olamaz.")
                .MaximumLength(100).WithMessage("Film başlığı 100 karakterden uzun olamaz.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama 500 karakterden uzun olamaz.");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Süre 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(300).WithMessage("Süre 300 dakikadan uzun olamaz.");

            RuleFor(x => x.Director)
                .MaximumLength(50).WithMessage("Yönetmen adı 50 karakterden uzun olamaz.");

            RuleFor(x => x.Cast)
                .MaximumLength(100).WithMessage("Oyuncu listesi 100 karakterden uzun olamaz.");

            RuleFor(x => x.GenreId)
                .GreaterThan(0).WithMessage("Geçerli bir tür seçilmelidir.");

            RuleFor(x => x.AgeRating)
                .MaximumLength(10).WithMessage("Yaş sınırı 10 karakterden uzun olamaz.");

            RuleFor(x => x.PosterUrl)
                .MaximumLength(200).WithMessage("Poster URL'i 200 karakterden uzun olamaz.");

            RuleFor(x => x.TrailerUrl)
                .MaximumLength(200).WithMessage("Fragman URL'i 200 karakterden uzun olamaz.");

            RuleFor(x => x.ReleaseDate)
                .NotEmpty().WithMessage("Vizyon tarihi boş olamaz.")
                .GreaterThan(DateTime.Now.AddYears(-10)).WithMessage("Vizyon tarihi 10 yıldan eski olamaz.");
        }
    }

    public class UpdateMovieDtoValidator : AbstractValidator<UpdateMovieDto>
    {
        public UpdateMovieDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçersiz film ID'si.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Film başlığı boş olamaz.")
                .MaximumLength(100).WithMessage("Film başlığı 100 karakterden uzun olamaz.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama 500 karakterden uzun olamaz.");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Süre 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(300).WithMessage("Süre 300 dakikadan uzun olamaz.");

            RuleFor(x => x.Director)
                .MaximumLength(50).WithMessage("Yönetmen adı 50 karakterden uzun olamaz.");

            RuleFor(x => x.Cast)
                .MaximumLength(100).WithMessage("Oyuncu listesi 100 karakterden uzun olamaz.");

            RuleFor(x => x.GenreId)
                .GreaterThan(0).WithMessage("Geçerli bir tür seçilmelidir.");

            RuleFor(x => x.AgeRating)
                .MaximumLength(10).WithMessage("Yaş sınırı 10 karakterden uzun olamaz.");

            RuleFor(x => x.PosterUrl)
                .MaximumLength(200).WithMessage("Poster URL'i 200 karakterden uzun olamaz.");

            RuleFor(x => x.TrailerUrl)
                .MaximumLength(200).WithMessage("Fragman URL'i 200 karakterden uzun olamaz.");

            RuleFor(x => x.ReleaseDate)
                .NotEmpty().WithMessage("Vizyon tarihi boş olamaz.")
                .GreaterThan(DateTime.Now.AddYears(-10)).WithMessage("Vizyon tarihi 10 yıldan eski olamaz.");
        }
    }
} 