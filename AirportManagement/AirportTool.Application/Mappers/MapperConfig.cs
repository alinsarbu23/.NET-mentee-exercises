using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Bookings;
using AutoMapper;

namespace AirportTool.Application.Mappers
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Booking, GetBookingByIdDto>().ReverseMap();
            CreateMap<Booking, GetBookingsDto>().ReverseMap();
            CreateMap<Booking, CreateBookingDto>().ReverseMap();
            CreateMap<Booking, DeleteBookingDto>().ReverseMap();


        }
    }
}
