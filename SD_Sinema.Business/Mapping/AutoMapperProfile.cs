using AutoMapper;
using SD_Sinema.Core.Entities;
using SD_Sinema.Business.DTOs;

namespace SD_Sinema.Business.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();

            // Genre mappings
            CreateMap<Genre, GenreDto>().ReverseMap();
            CreateMap<Genre, CreateGenreDto>().ReverseMap();
            CreateMap<Genre, UpdateGenreDto>().ReverseMap();

            // SeatType mappings
            CreateMap<SeatType, SeatTypeDto>().ReverseMap();
            CreateMap<SeatType, CreateSeatTypeDto>().ReverseMap();
            CreateMap<SeatType, UpdateSeatTypeDto>().ReverseMap();

            // Movie mappings
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre != null ? src.Genre.Name : null));
            CreateMap<Movie, CreateMovieDto>().ReverseMap();
            CreateMap<Movie, UpdateMovieDto>().ReverseMap();

            // Salon mappings
            CreateMap<Salon, SalonDto>().ReverseMap();
            CreateMap<Salon, CreateSalonDto>().ReverseMap();
            CreateMap<Salon, UpdateSalonDto>().ReverseMap();

            // Session mappings
            CreateMap<Session, SessionDto>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Movie.Title))
                .ForMember(dest => dest.SalonName, opt => opt.MapFrom(src => src.Salon.Name));
            CreateMap<Session, CreateSessionDto>().ReverseMap();
            CreateMap<Session, UpdateSessionDto>().ReverseMap();

            // Seat mappings
            CreateMap<Seat, SeatDto>()
                .ForMember(dest => dest.SalonName, opt => opt.MapFrom(src => src.Salon.Name))
                .ForMember(dest => dest.SeatTypeName, opt => opt.MapFrom(src => src.SeatType != null ? src.SeatType.Name : null));
            CreateMap<Seat, CreateSeatDto>().ReverseMap();
            CreateMap<Seat, UpdateSeatDto>().ReverseMap();

            // TicketType mappings
            CreateMap<TicketType, TicketTypeDto>().ReverseMap();
            CreateMap<TicketType, CreateTicketTypeDto>().ReverseMap();
            CreateMap<TicketType, UpdateTicketTypeDto>().ReverseMap();

            // Reservation mappings
            CreateMap<Reservation, ReservationDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Session.Movie.Title))
                .ForMember(dest => dest.SalonName, opt => opt.MapFrom(src => src.Session.Salon.Name));
            CreateMap<Reservation, CreateReservationDto>().ReverseMap();
            CreateMap<Reservation, UpdateReservationDto>().ReverseMap();

            // Ticket mappings
            CreateMap<Ticket, TicketDto>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Session.Movie.Title))
                .ForMember(dest => dest.SalonName, opt => opt.MapFrom(src => src.Session.Salon.Name))
                .ForMember(dest => dest.TicketTypeName, opt => opt.MapFrom(src => src.TicketType.Name))
                .ForMember(dest => dest.SeatInfo, opt => opt.MapFrom(src => $"{src.Seat.RowNumber}{src.Seat.SeatNumber}"));
            CreateMap<Ticket, CreateTicketDto>().ReverseMap();
            CreateMap<Ticket, UpdateTicketDto>().ReverseMap();
        }
    }
} 