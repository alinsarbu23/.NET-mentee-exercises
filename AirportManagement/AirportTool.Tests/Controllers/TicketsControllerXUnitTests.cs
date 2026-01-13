using AirportTool.Application.Interfaces;
using AirportTool.Application.Services.Tickets;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Tickets;
using AirportTool.WebApi.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AirportTool.Tests.Controllers
{
    public class TicketsControllerTests
    {
        private sealed class TestContext
        {
            public TicketsController Controller { get; init; }
            public Mock<IUnitOfWork> Uow { get; init; }
            public Mock<ITicketRepository> TicketsRepo { get; init; }
            public Mock<IFlightScheduleRepository> SchedulesRepo { get; init; }
            public Mock<IBookingRepository> BookingsRepo { get; init; }
            public Mock<IMapper> Mapper { get; init; }
        }

        private TestContext Build()
        {
            var uow = new Mock<IUnitOfWork>();
            var ticketsRepo = new Mock<ITicketRepository>();
            var schedulesRepo = new Mock<IFlightScheduleRepository>();
            var bookingsRepo = new Mock<IBookingRepository>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Tickets).Returns(ticketsRepo.Object);
            uow.Setup(u => u.FlightSchedules).Returns(schedulesRepo.Object);
            uow.Setup(u => u.Bookings).Returns(bookingsRepo.Object);

            uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
               .ReturnsAsync(1);

            ticketsRepo.Setup(r => r.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            ticketsRepo.Setup(r => r.UpdateAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            ticketsRepo.Setup(r => r.DeleteAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            ticketsRepo.Setup(r => r.GetOffersByScheduleIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new List<Ticket>());

            ticketsRepo.Setup(r => r.GetOfferByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Ticket)null);

            schedulesRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new FlightSchedule { Id = 10 });

            bookingsRepo.Setup(r => r.GetOffersBookingIdAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync(999);

            mapper.Setup(m => m.Map<IReadOnlyList<GetTicketByIdDto>>(It.IsAny<IReadOnlyList<Ticket>>()))
                  .Returns(new List<GetTicketByIdDto>());

            mapper.Setup(m => m.Map<GetTicketByIdDto>(It.IsAny<Ticket>()))
                  .Returns(new GetTicketByIdDto());

            mapper.Setup(m => m.Map<Ticket>(It.IsAny<CreateTicketDto>()))
                  .Returns(new Ticket { Id = 5 });

            var service = new TicketService(uow.Object, mapper.Object);
            var controller = new TicketsController(service);

            return new TestContext
            {
                Controller = controller,
                Uow = uow,
                TicketsRepo = ticketsRepo,
                SchedulesRepo = schedulesRepo,
                BookingsRepo = bookingsRepo,
                Mapper = mapper
            };
        }

        [Fact]
        public async Task GetBySchedule_ReturnsOk()
        {
            var ctx = Build();

            var result = await ctx.Controller.GetBySchedule(10, CancellationToken.None);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetOfferById_NotFound_Returns404()
        {
            var ctx = Build();

            var result = await ctx.Controller.GetOfferById(1, CancellationToken.None);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidRequest_Returns201()
        {
            var ctx = Build();

            var dto = new CreateTicketDto
            {
                FlightScheduleId = 10,
                FareClass = "Y",
                BasePrice = 100,
                Taxes = 10,
                Currency = "EUR",
                SeatInventory = 10
            };

            var result = await ctx.Controller.Create(dto, CancellationToken.None);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetOfferById", created.ActionName);
        }

        [Fact]
        public async Task Update_ValidRequest_Returns204()
        {
            var ctx = Build();

            ctx.TicketsRepo.Setup(r => r.GetOfferByIdAsync(1, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new Ticket { Id = 1, SeatInventory = 10 });

            var dto = new UpdateTicketDto { SeatInventory = 5 };

            var result = await ctx.Controller.Update(1, dto, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ValidRequest_Returns204()
        {
            var ctx = Build();

            ctx.TicketsRepo.Setup(r => r.GetOfferByIdAsync(1, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new Ticket { Id = 1 });

            var result = await ctx.Controller.Delete(1, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
