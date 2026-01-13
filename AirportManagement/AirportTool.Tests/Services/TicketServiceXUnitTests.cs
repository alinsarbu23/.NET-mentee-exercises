using AirportTool.Application.Interfaces;
using AirportTool.Application.Services.Tickets;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Tickets;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AirportTool.Tests.Services
{
    public class TicketServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<ITicketRepository> _tickets = new();
        private readonly Mock<IFlightScheduleRepository> _schedules = new();
        private readonly Mock<IBookingRepository> _bookings = new();
        private readonly Mock<IMapper> _mapper = new();

        private TicketService CreateService()
        {
            _unitOfWork.Setup(u => u.Tickets).Returns(_tickets.Object);
            _unitOfWork.Setup(u => u.FlightSchedules).Returns(_schedules.Object);
            _unitOfWork.Setup(u => u.Bookings).Returns(_bookings.Object);

            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(1);

            return new TicketService(_unitOfWork.Object, _mapper.Object);
        }

        [Fact]
        public async Task CreateOfferAsync_NegativeBasePrice_ThrowsArgumentException()
        {
            var service = CreateService();

            var dto = new CreateTicketDto
            {
                BasePrice = -1,
                Taxes = 10,
                SeatInventory = 5,
                FareClass = "Y",
                Currency = "EUR",
                FlightScheduleId = 1
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateOfferAsync(dto));

            _tickets.Verify(t => t.AddRangeAsync(It.IsAny<IEnumerable<Ticket>>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateOfferAsync_NegativeTaxes_ThrowsArgumentException()
        {
            var service = CreateService();

            var dto = new CreateTicketDto
            {
                BasePrice = 10,
                Taxes = -1,
                SeatInventory = 5,
                FareClass = "Y",
                Currency = "EUR",
                FlightScheduleId = 1
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateOfferAsync(dto));

            _tickets.Verify(t => t.AddRangeAsync(It.IsAny<IEnumerable<Ticket>>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateOfferAsync_NegativeSeatInventory_ThrowsArgumentException()
        {
            var service = CreateService();

            var dto = new CreateTicketDto
            {
                BasePrice = 10,
                Taxes = 1,
                SeatInventory = -1,
                FareClass = "Y",
                Currency = "EUR",
                FlightScheduleId = 1
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateOfferAsync(dto));

            _tickets.Verify(t => t.AddRangeAsync(It.IsAny<IEnumerable<Ticket>>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateOfferAsync_MissingFareClass_ThrowsArgumentException()
        {
            var service = CreateService();

            var dto = new CreateTicketDto
            {
                BasePrice = 10,
                Taxes = 1,
                SeatInventory = 5,
                FareClass = "   ",
                Currency = "EUR",
                FlightScheduleId = 1
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateOfferAsync(dto));

            _tickets.Verify(t => t.AddRangeAsync(It.IsAny<IEnumerable<Ticket>>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateOfferAsync_ScheduleNotFound_ThrowsKeyNotFoundException()
        {
            _schedules.Setup(s => s.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                      .ReturnsAsync((FlightSchedule?)null);

            var service = CreateService();

            var dto = new CreateTicketDto
            {
                BasePrice = 10,
                Taxes = 1,
                SeatInventory = 5,
                FareClass = "Y",
                Currency = "EUR",
                FlightScheduleId = 1
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.CreateOfferAsync(dto));

            _tickets.Verify(t => t.AddRangeAsync(It.IsAny<IEnumerable<Ticket>>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateOfferAsync_Valid_CallsAddAsyncAndSaveChanges_ReturnsId()
        {
            _schedules.Setup(s => s.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new FlightSchedule { Id = 1 });

            _bookings.Setup(b => b.GetOffersBookingIdAsync(It.IsAny<CancellationToken>()))
                     .ReturnsAsync(777);

            var mapped = new Ticket
            {
                Id = 123,
                FlightScheduleId = 1,
                FareClass = "Y",
                Currency = "EUR",
                BasePrice = 10,
                Taxes = 1,
                SeatInventory = 5
            };

            _mapper.Setup(m => m.Map<Ticket>(It.IsAny<CreateTicketDto>()))
                   .Returns(mapped);

            _tickets.Setup(t => t.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

            var service = CreateService();

            var dto = new CreateTicketDto
            {
                BasePrice = 10,
                Taxes = 1,
                SeatInventory = 5,
                FareClass = "Y",
                Currency = "EUR",
                FlightScheduleId = 1
            };

            var id = await service.CreateOfferAsync(dto);

            Assert.Equal(123, id);

            _tickets.Verify(t => t.AddAsync(
                It.Is<Ticket>(x =>
                    x.FlightScheduleId == 1 &&
                    x.FareClass == "Y" &&
                    x.Currency == "EUR" &&
                    x.BasePrice == 10 &&
                    x.Taxes == 1 &&
                    x.TotalPrice == 11 &&
                    x.SeatInventory == 5 &&
                    x.BookingId == 777 &&
                    x.SeatNumber == null &&
                    x.PassengerFullName == null &&
                    x.PassengerEmail == null),
                It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInventoryAsync_NegativeInventory_ThrowsArgumentException()
        {
            var service = CreateService();

            var dto = new UpdateTicketDto
            {
                SeatInventory = -1
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateInventoryAsync(1, dto));

            _tickets.Verify(t => t.GetOfferByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateInventoryAsync_OfferNotFound_ThrowsKeyNotFoundException()
        {
            _tickets.Setup(t => t.GetOfferByIdAsync(1, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Ticket?)null);

            var service = CreateService();

            var dto = new UpdateTicketDto { SeatInventory = 10 };

            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateInventoryAsync(1, dto));

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOfferAsync_OfferNotFound_ThrowsKeyNotFoundException()
        {
            _tickets.Setup(t => t.GetOfferByIdAsync(1, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Ticket?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteOfferAsync(1));

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
