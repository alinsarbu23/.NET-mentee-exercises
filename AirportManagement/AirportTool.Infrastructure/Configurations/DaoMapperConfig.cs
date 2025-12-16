using AirportTool.Domain.Entities;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.Data.Models;
using AutoMapper;

namespace AirportTool.Infrastructure.Configurations
{
    public class DaoMapperConfig : Profile
    {
        public DaoMapperConfig()
        {
            CreateMap<AddressDAO, Address>().ReverseMap();
            CreateMap<AircraftDAO, Aircraft>().ReverseMap();
            CreateMap<AirlineDAO, Airline>().ReverseMap();
            CreateMap<AirportDAO, Airport>().ReverseMap();
            CreateMap<BookingDAO, Booking>().ReverseMap();
            CreateMap<BookingStatusDAO, BookingStatus>().ReverseMap();
            CreateMap<FlightDAO, Flight>().ReverseMap();
            CreateMap<FlightScheduleDAO, FlightSchedule>().ReverseMap();
            CreateMap<FlightStatusDAO, FlightStatus>().ReverseMap();
            CreateMap<GateDAO, Gate>().ReverseMap();
            CreateMap<TicketDAO, Ticket>().ReverseMap();
            CreateMap<UserDAO, User>().ReverseMap();
        }
    }
}
