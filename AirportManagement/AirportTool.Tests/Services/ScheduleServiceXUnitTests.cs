using AirportTool.Application.DTOs.Schedules;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Services.Schedules;
using AirportTool.Domain.Entities;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Schedules;
using AutoMapper;
using Moq;
using Xunit;

namespace AirportTool.Tests.Services
{
    public class ScheduleServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IFlightScheduleRepository> _scheduleRepo;
        private readonly Mock<IFlightRepository> _flightRepo;
        private readonly Mock<IMapper> _mapper;

        private readonly ScheduleService _service;

        public ScheduleServiceTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _scheduleRepo = new Mock<IFlightScheduleRepository>();
            _flightRepo = new Mock<IFlightRepository>();
            _mapper = new Mock<IMapper>();

            _unitOfWork.Setup(u => u.FlightSchedules).Returns(_scheduleRepo.Object);
            _unitOfWork.Setup(u => u.Flights).Returns(_flightRepo.Object);

            _service = new ScheduleService(_unitOfWork.Object, _mapper.Object);
        }

        [Fact]
        public async Task CreateAsync_GateOverlapDetected_ThrowsInvalidOperationException()
        {
            var dto = new CreateScheduleDto
            {
                GateId = 12,
                ScheduledDepartureUtc = new DateTime(2025, 12, 1, 10, 0, 0),
                ScheduledArrivalUtc = new DateTime(2025, 12, 1, 12, 0, 0),
                FlightStatusId = 1
            };

            _scheduleRepo.Setup(r =>
                r.HasGateOverlapAsync(
                    12,
                    dto.ScheduledDepartureUtc,
                    dto.ScheduledArrivalUtc,
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_ValidSchedule_CreatesAndReturnsId()
        {
            var dto = new CreateScheduleDto
            {
                FlightId = 1,
                ScheduledDepartureUtc = DateTime.UtcNow.AddHours(1),
                ScheduledArrivalUtc = DateTime.UtcNow.AddHours(2),
                GateId = null,
                FlightStatusId = 1
            };

            _mapper.Setup(m => m.Map<FlightSchedule>(dto))
                .Returns(new FlightSchedule { Id = 123, FlightId = 1 });

            _scheduleRepo.Setup(r => r.AddAsync(It.IsAny<FlightSchedule>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _service.CreateAsync(dto);

            Assert.Equal(123, result);

            _scheduleRepo.Verify(r => r.AddAsync(It.IsAny<FlightSchedule>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchAsync_WithDateRange_ReturnsMappedDtos()
        {
            var from = DateTime.UtcNow.Date;
            var to = from.AddDays(1);

            var schedules = new List<FlightSchedule>
            {
                new FlightSchedule { Id = 1, FlightId = 100 }
            };

            _scheduleRepo.Setup(r =>
                r.GetUpcomingAsync(from, to, It.IsAny<CancellationToken>()))
                .ReturnsAsync(schedules);

            _mapper.Setup(m =>
                m.Map<IReadOnlyList<GetScheduleByIdDto>>(It.IsAny<List<FlightSchedule>>()))
                .Returns(new List<GetScheduleByIdDto>
                {
                    new GetScheduleByIdDto { Id = 1 }
                });

            var result = await _service.SearchAsync(null, from, to);

            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public async Task GetUpcomingStatsAsync_GroupsByDateCorrectly()
        {
            var today = DateTime.UtcNow.Date;

            var schedules = new List<FlightSchedule>
            {
                new FlightSchedule { ScheduledDepartureUtc = today.AddHours(1) },
                new FlightSchedule { ScheduledDepartureUtc = today.AddHours(3) }
            };

            _scheduleRepo.Setup(r =>
                r.GetUpcomingAsync(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(schedules);

            var result = await _service.GetUpcomingStatsAsync();

            Assert.Single(result);
            Assert.Equal(2, result.First().FlightsCount);
        }

        [Fact]
        public async Task ImportAsync_EmptyJson_ThrowsArgumentException()
        {
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("[]"));

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.ImportAsync(stream, CancellationToken.None));
        }

        [Fact]
        public async Task ImportAsync_ValidSingleRow_ReturnsCreated()
        {
            var json = """
            [
              {
                "airlineIata": "RO",
                "flightNumber": "RO391",
                "originIata": "OTP",
                "destinationIata": "LHR",
                "scheduledDepartureUtc": "2025-12-01T10:00:00Z",
                "scheduledArrivalUtc": "2025-12-01T12:00:00Z",
                "gateCode": null
              }
            ]
            """;

            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));

            _flightRepo.Setup(r => r.GetAirlineByIataAsync("RO", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Airline { Id = 1 });

            _flightRepo.Setup(r => r.GetAirportByIataAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Airport { Id = 1 });

            _flightRepo.Setup(r => r.GetByKeyAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Flight { Id = 10 });

            _scheduleRepo.Setup(r => r.FindByFlightAndDepartureAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((FlightSchedule?)null);

            _scheduleRepo.Setup(r => r.AddAsync(It.IsAny<FlightSchedule>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _service.ImportAsync(stream, CancellationToken.None);

            Assert.Equal(1, result.Total);
            Assert.Equal(1, result.Created);
            Assert.Equal(0, result.Updated);
            Assert.Empty(result.Errors);

            _scheduleRepo.Verify(r => r.AddAsync(It.IsAny<FlightSchedule>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
