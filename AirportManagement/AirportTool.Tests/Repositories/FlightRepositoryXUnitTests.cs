using AirportTool.Domain.Entities;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure;
using AirportTool.Infrastructure.Data;
using AirportTool.Infrastructure.Data.Models;
using AirportTool.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AirportTool.Tests.Repositories
{
    public class FlightRepositoryTests
    {
        private static FlightRepository BuildRepository(out AirportDbContext context)
        {
            var options = new DbContextOptionsBuilder<AirportDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new AirportDbContext(options);

            var mapperConfig = new MapperConfiguration(configure =>
            {
                configure.CreateMap<FlightDAO, Flight>();
                configure.CreateMap<AirlineDAO, Airline>();
                configure.CreateMap<AirportDAO, Airport>();
            });

            var mapper = mapperConfig.CreateMapper();

            return new FlightRepository(context, mapper);
        }

        [Fact]
        public async Task SearchAsync_WithOriginAndDestinationIata_ReturnsMatchingFlights()
        {
            var repo = BuildRepository(out var context);

            var address = new AddressDAO
            {
                Id = 1,
                Country = "Romania",
                City = "Bucharest",
                Street = "Aviatorilor 1"
            };

            var airline = new AirlineDAO
            {
                Id = 1,
                IATACode = "RO",
                Name = "Tarom"
            };

            var otp = new AirportDAO
            {
                Id = 1,
                IATACode = "OTP",
                Name = "Otopeni",
                TimeZone = "Europe/Bucharest",
                AddressId = address.Id,
                Address = address
            };

            var lhr = new AirportDAO
            {
                Id = 2,
                IATACode = "LHR",
                Name = "Heathrow",
                TimeZone = "Europe/London",
                AddressId = address.Id,
                Address = address
            };

            var cdg = new AirportDAO
            {
                Id = 3,
                IATACode = "CDG",
                Name = "Charles de Gaulle",
                TimeZone = "Europe/Paris",
                AddressId = address.Id,
                Address = address
            };

            context.Addresses.Add(address);
            context.Airlines.Add(airline);
            context.Airports.AddRange(otp, lhr, cdg);

            context.Flights.AddRange(
                new FlightDAO
                {
                    Id = 1,
                    AirlineId = airline.Id,
                    Airline = airline,
                    FlightNumber = "RO101",
                    OriginAirportId = otp.Id,
                    OriginAirport = otp,
                    DestinationAirportId = lhr.Id,
                    DestinationAirport = lhr,
                    IsActive = true
                },
                new FlightDAO
                {
                    Id = 2,
                    AirlineId = airline.Id,
                    Airline = airline,
                    FlightNumber = "RO102",
                    OriginAirportId = otp.Id,
                    OriginAirport = otp,
                    DestinationAirportId = cdg.Id,
                    DestinationAirport = cdg,
                    IsActive = true
                }
            );

            await context.SaveChangesAsync();

            var result = await repo.SearchAsync(
                originIata: "OTP",
                destinationIata: "LHR",
                departureDateUtc: null,
                airlineIata: "RO");

            Assert.Single(result);

            var flight = result[0];
            Assert.Equal("RO101", flight.FlightNumber);
            Assert.Equal(otp.Id, flight.OriginAirportId);
            Assert.Equal(lhr.Id, flight.DestinationAirportId);
            Assert.Equal(airline.Id, flight.AirlineId);
        }
    }
}
