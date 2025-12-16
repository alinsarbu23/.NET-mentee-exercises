using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Bookings;
using AirportTool.Infrastructure.DTOs.Flights;
using AirportTool.Infrastructure.DTOs.Schedules;
using AirportTool.Infrastructure.DTOs.Tickets;
using AutoMapper;

namespace AirportTool.Application.Mappers
{
    public class DtoMapperConfig : Profile
    {
        public DtoMapperConfig()
        {
            CreateMap<Booking, GetBookingByIdDto>().ReverseMap();
            CreateMap<CreateBookingDto, Booking>().ReverseMap();

            CreateMap<CreateFlightDto, Flight>().ReverseMap();
            CreateMap<Flight, GetFlightByIdDto>().ReverseMap();
            CreateMap<UpdateFlightDto, Flight>().ReverseMap();

            CreateMap<CreateScheduleDto, FlightSchedule>().ReverseMap();
            CreateMap<FlightSchedule, GetScheduleByIdDto>().ReverseMap();

            CreateMap<CreateTicketDto, Ticket>().ReverseMap();
            CreateMap<Ticket, GetTicketByIdDto>().ReverseMap();
        }
    }
}
