using AirportTool.Application.Interfaces;
using AirportTool.Application.Services.Bookings;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Bookings;
using AirportTool.WebApi.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AirportTool.Tests.Controllers
{
    public class BookingsControllerTests
    {
        private BookingService CreateRealService()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var tickets = new Mock<ITicketRepository>();
            var bookings = new Mock<IBookingRepository>();
            var schedules = new Mock<IFlightScheduleRepository>();
            var mapper = new Mock<IMapper>();
            var transaction = new Mock<IDbContextTransaction>();

            unitOfWork.Setup(u => u.Tickets).Returns(tickets.Object);
            unitOfWork.Setup(u => u.Bookings).Returns(bookings.Object);
            unitOfWork.Setup(u => u.FlightSchedules).Returns(schedules.Object);
            unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
               .ReturnsAsync(transaction.Object);

            tickets.Setup(t => t.GetOfferByIdAsync(1, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new Ticket
                   {
                       Id = 1,
                       FlightScheduleId = 10,
                       SeatInventory = 10
                   });

            tickets.Setup(t => t.CountSoldSeatsForScheduleAsync(10, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(0);

            schedules.Setup(s => s.GetCapacityForScheduleAsync(10, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(100);

            bookings.Setup(b => b.GetConfirmationCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Booking { Id = 1 });

            bookings.Setup(b => b.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

            tickets.Setup(t => t.UpdateAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
                   .Returns(Task.CompletedTask);

            tickets.Setup(t => t.AddRangeAsync(It.IsAny<List<Ticket>>(), It.IsAny<CancellationToken>()))
                   .Returns(Task.CompletedTask);

            return new BookingService(unitOfWork.Object, mapper.Object);
        }

        //[Fact]
        //public async Task Create_ValidRequest_Returns201_AndCallsServiceOnce()
        //{
        //    var bookingService = new Mock<BookingService>();

        //    bookingService
        //        .Setup(s => s.CreateAsync(It.IsAny<CreateBookingDto>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync("ABC123");

        //    var controller = new BookingsController(bookingService.Object);

        //    var dto = new CreateBookingDto
        //    {
        //        TicketId = 1,
        //        FlightScheduleId = 10,
        //        Quantity = 1,
        //        PassengerFullName = "Jane",
        //        PassengerEmail = "jane@test.com"
        //    };

        //    var result = await controller.Create(dto, CancellationToken.None);

        //    var created = Assert.IsType<CreatedAtActionResult>(result);
        //    Assert.Equal(201, created.StatusCode);

        //    bookingService.Verify(
        //        s => s.CreateAsync(It.IsAny<CreateBookingDto>(), It.IsAny<CancellationToken>()),
        //        Times.Once);
        //}


        [Fact]
        public async Task GetByCode_NotFound_Returns404()
        {
            var uow = new Mock<IUnitOfWork>();
            var mapper = new Mock<IMapper>();

            uow.Setup(u => u.Bookings.GetConfirmationCodeAsync("ABC123", It.IsAny<CancellationToken>()))
               .ReturnsAsync((Booking)null);

            var service = new BookingService(uow.Object, mapper.Object);
            var controller = new BookingsController(service);

            var result = await controller.GetByCode("ABC123", CancellationToken.None);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Cancel_ValidCode_Returns204()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var tickets = new Mock<ITicketRepository>();
            var bookings = new Mock<IBookingRepository>();
            var tx = new Mock<IDbContextTransaction>();
            var mapper = new Mock<IMapper>();

            unitOfWork.Setup(u => u.Tickets).Returns(tickets.Object);
            unitOfWork.Setup(u => u.Bookings).Returns(bookings.Object);
            unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
               .ReturnsAsync(tx.Object);

            bookings.Setup(b => b.GetConfirmationCodeAsync("ABC123", It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Booking { Id = 1 });

            tickets.Setup(t => t.GetSoldByBookingIdAsync(1, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new List<Ticket>());

            var service = new BookingService(unitOfWork.Object, mapper.Object);
            var controller = new BookingsController(service);

            var result = await controller.Cancel("ABC123", CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
