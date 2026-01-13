using AirportTool.Application.Interfaces;
using AirportTool.Application.Services.Bookings;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Bookings;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AirportTool.Tests.Services
{
    public class BookingServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<ITicketRepository> _tickets = new();
        private readonly Mock<IBookingRepository> _bookings = new();
        private readonly Mock<IFlightScheduleRepository> _schedules = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IDbContextTransaction> _transaction = new();

        private BookingService CreateService()
        {
            _unitOfWork.Setup(u => u.Tickets).Returns(_tickets.Object);
            _unitOfWork.Setup(u => u.Bookings).Returns(_bookings.Object);
            _unitOfWork.Setup(u => u.FlightSchedules).Returns(_schedules.Object);

            _unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(_transaction.Object);

            _transaction.Setup(t => t.CommitAsync(It.IsAny<CancellationToken>()))
                        .Returns(Task.CompletedTask);
            _transaction.Setup(t => t.RollbackAsync(It.IsAny<CancellationToken>()))
                        .Returns(Task.CompletedTask);

            return new BookingService(_unitOfWork.Object, _mapper.Object);
        }

        [Fact]
        public async Task CreateAsync_QuantityExceedsInventory_ThrowsArgumentException()
        {
            var dto = new CreateBookingDto
            {
                TicketId = 1,
                FlightScheduleId = 10,
                Quantity = 4,
                PassengerFullName = "Jane Doe",
                PassengerEmail = "jane@test.com"
            };

            var offer = new Ticket
            {
                Id = 1,
                FlightScheduleId = 10,
                SeatInventory = 3
            };

            _tickets.Setup(t => t.GetOfferByIdAsync(1, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(offer);

            var service = CreateService();

            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(dto));


            Assert.Equal(3, offer.SeatInventory);

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _bookings.Verify(b => b.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()), Times.Never);
            _tickets.Verify(t => t.DecrementOfferInventoryAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _transaction.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenCapacityExceeded_ThrowsInvalidOperationException()
        {
            var dto = new CreateBookingDto
            {
                TicketId = 1,
                FlightScheduleId = 10,
                Quantity = 2,
                PassengerFullName = "Jane Doe",
                PassengerEmail = "jane@test.com"
            };

            var offer = new Ticket
            {
                Id = 1,
                FlightScheduleId = 10,
                SeatInventory = 10
            };

            _tickets.Setup(t => t.GetOfferByIdAsync(1, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(offer);

            _tickets.Setup(t => t.CountSoldSeatsForScheduleAsync(10, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(5);

            _schedules.Setup(s => s.GetCapacityForScheduleAsync(10, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(6);

            var service = CreateService();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(dto));

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _bookings.Verify(b => b.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()), Times.Never);
            _tickets.Verify(t => t.DecrementOfferInventoryAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _transaction.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_DecrementsInventory_AddsBooking_SavesAndCommits()
        {
            var dto = new CreateBookingDto
            {
                TicketId = 1,
                FlightScheduleId = 10,
                Quantity = 2,
                PassengerFullName = "Jane Doe",
                PassengerEmail = "jane@test.com"
            };

            var offer = new Ticket
            {
                Id = 1,
                FlightScheduleId = 10,
                SeatInventory = 5,
                FareClass = "Y",
                BasePrice = 100,
                Taxes = 20,
                TotalPrice = 120,
                Currency = "EUR",
                IsRefundable = true
            };

            _tickets.Setup(t => t.GetOfferByIdAsync(dto.TicketId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(offer);

            _tickets.Setup(t => t.CountSoldSeatsForScheduleAsync(dto.FlightScheduleId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(0);

            _schedules.Setup(s => s.GetCapacityForScheduleAsync(dto.FlightScheduleId, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(200);

            _bookings.Setup(b => b.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
                     .Returns(Task.CompletedTask);

            _bookings.Setup(b => b.GetConfirmationCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new Booking { Id = 123, BookingStatusId = 1 });

            _tickets.Setup(t => t.UpdateAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

            _tickets.Setup(t => t.AddRangeAsync(It.IsAny<IEnumerable<Ticket>>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

            _unitOfWork.SetupSequence(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(1)
                       .ReturnsAsync(1);

            var service = CreateService();

            var confirmation = await service.CreateAsync(dto);

            Assert.False(string.IsNullOrWhiteSpace(confirmation));

            _bookings.Verify(b => b.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()), Times.Once);

            _tickets.Verify(t => t.UpdateAsync(
                It.Is<Ticket>(x => x.Id == offer.Id && x.SeatInventory == 3),
                It.IsAny<CancellationToken>()),
                Times.Once);

            _tickets.Verify(t => t.AddRangeAsync(
                It.Is<IEnumerable<Ticket>>(x => x.Count() == dto.Quantity),
                It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
            _transaction.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            _transaction.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CancelAsync_ActiveBooking_RestoresInventoryAndCancels()
        {
            var booking = new Booking
            {
                Id = 1,
                BookingStatusId = 1
            };

            var soldTickets = new List<Ticket>
            {
                new Ticket { FlightScheduleId = 10, FareClass = "Y" },
                new Ticket { FlightScheduleId = 10, FareClass = "Y" }
            };

            var offer = new Ticket
            {
                FareClass = "Y",
                SeatInventory = 5
            };

            _bookings.Setup(b => b.GetConfirmationCodeAsync("ABC123", It.IsAny<CancellationToken>()))
                     .ReturnsAsync(booking);

            _tickets.Setup(t => t.GetSoldByBookingIdAsync(1, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(soldTickets);

            _tickets.Setup(t => t.GetOffersByScheduleIdAsync(10, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<Ticket> { offer });

            _tickets.Setup(t => t.UpdateAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);
            _bookings.Setup(b => b.UpdateAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
                     .Returns(Task.CompletedTask);

            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(1);

            var service = CreateService();

            await service.CancelAsync("ABC123");

            Assert.Equal(7, offer.SeatInventory);
            Assert.Equal(2, booking.BookingStatusId);

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task CancelAsync_UnknownConfirmationCode_ThrowsKeyNotFoundException()
        {
            _bookings.Setup(b => b.GetConfirmationCodeAsync("NOPE", It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Booking?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.CancelAsync("NOPE"));

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
