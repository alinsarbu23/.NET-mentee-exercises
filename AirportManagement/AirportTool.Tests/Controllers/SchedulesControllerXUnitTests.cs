using AirportTool.Application.DTOs.Schedules;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Services.Schedules;
using AirportTool.Domain.Entities;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Schedules;
using AirportTool.WebApi.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AirportTool.Tests.Controllers
{
    public class SchedulesControllerTests
    {
        private sealed class TestContext
        {
            public SchedulesController Controller { get; init; }
            public Mock<IUnitOfWork> Uow { get; init; }
            public Mock<IFlightScheduleRepository> SchedulesRepo { get; init; }
            public Mock<IFlightRepository> FlightsRepo { get; init; }
            public Mock<IMapper> Mapper { get; init; }
        }

        private TestContext BuildForImport()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var schedulesRepo = new Mock<IFlightScheduleRepository>();
            var flightsRepo = new Mock<IFlightRepository>();
            var mapper = new Mock<IMapper>();

            unitOfWork.Setup(u => u.FlightSchedules).Returns(schedulesRepo.Object);
            unitOfWork.Setup(u => u.Flights).Returns(flightsRepo.Object);

            unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
               .ReturnsAsync(1);

            schedulesRepo.Setup(r => r.AddAsync(It.IsAny<FlightSchedule>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask);

            flightsRepo.Setup(r => r.AddAsync(It.IsAny<Flight>(), It.IsAny<CancellationToken>()))
                      .Returns(Task.CompletedTask);

            flightsRepo.Setup(r => r.GetAirlineByIataAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new Airline { Id = 1 });

            flightsRepo.Setup(r => r.GetAirportByIataAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new Airport { Id = 1 });

            flightsRepo.Setup(r => r.GetByKeyAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Flight)null);

            schedulesRepo.Setup(r => r.FindByFlightAndDepartureAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((FlightSchedule)null);

            schedulesRepo.Setup(r => r.GetGateByCodeAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Gate)null);

            schedulesRepo.Setup(r => r.HasGateOverlapAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<int?>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var service = new ScheduleService(unitOfWork.Object, mapper.Object);
            var controller = new SchedulesController(service);

            return new TestContext
            {
                Controller = controller,
                Uow = unitOfWork,
                SchedulesRepo = schedulesRepo,
                FlightsRepo = flightsRepo,
                Mapper = mapper
            };
        }

        [Fact]
        public async Task Import_AllValid_Returns201()
        {
            var ctx = BuildForImport();

            var json = @"[
              {
                ""flightNumber"": ""RO391"",
                ""airlineIata"": ""RO"",
                ""originIata"": ""OTP"",
                ""destinationIata"": ""LHR"",
                ""scheduledDepartureUtc"": ""2025-12-01T06:30:00Z"",
                ""scheduledArrivalUtc"": ""2025-12-01T08:25:00Z"",
                ""gateCode"": null,
                ""assignedAircraftTail"": null
              }
            ]";

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var file = new FormFile(ms, 0, ms.Length, "file", "schedules.json");

            var result = await ctx.Controller.Import(file, CancellationToken.None);

            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Import_MixedResults_Returns207()
        {
            var ctx = BuildForImport();

            ctx.FlightsRepo.Setup(r => r.GetAirlineByIataAsync("BAD", It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Airline)null);

            var json = @"[
              {
                ""flightNumber"": ""RO391"",
                ""airlineIata"": ""RO"",
                ""originIata"": ""OTP"",
                ""destinationIata"": ""LHR"",
                ""scheduledDepartureUtc"": ""2025-12-01T06:30:00Z"",
                ""scheduledArrivalUtc"": ""2025-12-01T08:25:00Z"",
                ""gateCode"": null,
                ""assignedAircraftTail"": null
              },
              {
                ""flightNumber"": ""RO999"",
                ""airlineIata"": ""BAD"",
                ""originIata"": ""OTP"",
                ""destinationIata"": ""LHR"",
                ""scheduledDepartureUtc"": ""2025-12-01T09:30:00Z"",
                ""scheduledArrivalUtc"": ""2025-12-01T11:25:00Z"",
                ""gateCode"": null,
                ""assignedAircraftTail"": null
              }
            ]";

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var file = new FormFile(ms, 0, ms.Length, "file", "schedules.json");

            var result = await ctx.Controller.Import(file, CancellationToken.None);

            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal(207, obj.StatusCode);
        }
    }
}
