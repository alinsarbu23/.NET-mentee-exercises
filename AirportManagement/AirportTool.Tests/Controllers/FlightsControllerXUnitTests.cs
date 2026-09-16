using AirportTool.Application.DTOs.Flights;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Services.Flights;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Common;
using AirportTool.Infrastructure.DTOs.Flights;
using AirportTool.WebApi.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;

namespace AirportTool.Tests.Controllers
{
    public class FlightsControllerTests
    {
        private sealed class TestContext
        {
            public FlightsController Controller { get; init; }
            public Mock<IUnitOfWork> UnitOfWork { get; init; }
            public Mock<IFlightRepository> FlightsRepo { get; init; }
            public Mock<IFlightScheduleRepository> SchedulesRepo { get; init; }
            public Mock<IMapper> Mapper { get; init; }
        }

        private TestContext Build()
        {
            var uow = new Mock<IUnitOfWork>();
            var flightsRepo = new Mock<IFlightRepository>();
            var schedulesRepo = new Mock<IFlightScheduleRepository>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Flights).Returns(flightsRepo.Object);
            uow.Setup(u => u.FlightSchedules).Returns(schedulesRepo.Object);

            uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
               .ReturnsAsync(1);

            flightsRepo.Setup(r => r.AddAsync(It.IsAny<Flight>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            flightsRepo.Setup(r => r.UpdateAsync(It.IsAny<Flight>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            flightsRepo.Setup(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            mapper.Setup(m => m.Map<GetFlightByIdDto>(It.IsAny<Flight>()))
                  .Returns(new GetFlightByIdDto());

            mapper.Setup(m => m.Map<Flight>(It.IsAny<CreateFlightDto>()))
                  .Returns(new Flight
                  {
                      Id = 10,
                      OriginAirportId = 1,
                      DestinationAirportId = 2,
                      FlightNumber = "RO391",
                      AirlineId = 1,
                      IsActive = true
                  });

            mapper.Setup(m => m.Map(It.IsAny<UpdateFlightDto>(), It.IsAny<Flight>()))
                  .Callback<object, object>((src, dest) => { });

            schedulesRepo.Setup(r => r.SearchAsync(It.IsAny<GetFlightSearchDto>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new PagedResultDto<FlightSearchResultDto>());

            var service = new FlightService(uow.Object, mapper.Object);
            var controller = new FlightsController(service);

            return new TestContext
            {
                Controller = controller,
                UnitOfWork = uow,
                FlightsRepo = flightsRepo,
                SchedulesRepo = schedulesRepo,
                Mapper = mapper
            };
        }

        [Fact]
        public async Task GetById_NotFound_Returns404()
        {
            var ctx = Build();

            ctx.FlightsRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Flight)null);

            var result = await ctx.Controller.GetById(1, CancellationToken.None);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Search_WithQuery_ReturnsOk()
        {
            var ctx = Build();

            var result = await ctx.Controller.Search(new GetFlightSearchDto(), CancellationToken.None);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidRequest_Returns201()
        {
            var ctx = Build();

            var dto = new CreateFlightDto
            {
                OriginIata = "OTP",
                DestinationIata = "LHR"
            };

            var result = await ctx.Controller.Create(dto, CancellationToken.None);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetById", created.ActionName);
        }

        [Fact]
        public async Task Update_ValidRequest_Returns204()
        {
            var ctx = Build();

            ctx.FlightsRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new Flight
                           {
                               Id = 1,
                               OriginAirportId = 1,
                               DestinationAirportId = 2,
                               FlightNumber = "RO391",
                               AirlineId = 1,
                               IsActive = true
                           });

            var dto = new UpdateFlightDto
            {
                OriginIata = "OTP",
                DestinationIata = "LHR"
            };

            var result = await ctx.Controller.Update(1, dto, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ValidRequest_Returns204()
        {
            var ctx = Build();

            var result = await ctx.Controller.Delete(1, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
