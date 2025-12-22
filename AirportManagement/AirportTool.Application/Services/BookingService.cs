using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Bookings;
using AutoMapper;

namespace AirportTool.Application.Services.Bookings
{
    public class BookingService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;


        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        private string GenerateConfirmationCode()
        {
            string guidString = Guid.NewGuid().ToString("N");
            string code = guidString.Substring(0, 8).ToUpper();
            return code;
        }

        public async Task<string> CreateAsync(CreateBookingDto dto, CancellationToken ct = default)
        {
            if (dto.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be positive.");
            }

            await using var tx = await unitOfWork.BeginTransactionAsync(ct);


            var offer = await unitOfWork.Tickets.GetOfferByIdAsync(dto.TicketId, ct);
            if (offer == null)
            {
                throw new KeyNotFoundException("Ticket offer not found.");
            }


            if (offer.FlightScheduleId != dto.FlightScheduleId)
            {
                throw new ArgumentException("Ticket does not belong to the provided FlightScheduleId.");
            }
                

            if (dto.Quantity > offer.SeatInventory)
            {
                throw new ArgumentException("Not enough seats available.");
            }
                
            var soldSeats = await unitOfWork.Tickets.CountSoldSeatsForScheduleAsync(dto.FlightScheduleId, ct);
            var capacity = await unitOfWork.FlightSchedules.GetCapacityForScheduleAsync(dto.FlightScheduleId, ct);

            if (soldSeats + dto.Quantity > capacity)
            {
                throw new InvalidOperationException("Overbooking prevented: aircraft capacity exceeded.");
            }

            var confirmationCode = GenerateConfirmationCode();
            var booking = new Booking
            {
                UserId = 1,       
                BookingStatusId = 1,  
                CreatedUtc = DateTime.UtcNow,
                ConfirmationCode = confirmationCode,
                Quantity = dto.Quantity
            };

            await unitOfWork.Bookings.AddAsync(booking, ct);
            await unitOfWork.SaveChangesAsync(ct);

            var persisted = await unitOfWork.Bookings.GetConfirmationCodeAsync(confirmationCode, ct);
            if (persisted == null)
            {
                throw new InvalidOperationException("Booking insert failed.");
            }


            await unitOfWork.Tickets.DecrementOfferInventoryAsync(dto.TicketId, dto.Quantity, ct);

            var sold = new List<Ticket>(dto.Quantity);

            for (int i = 0; i < dto.Quantity; i++)
            {
                sold.Add(new Ticket
                {
                    BookingId = persisted.Id,  
                    FlightScheduleId = dto.FlightScheduleId,
                    FareClass = offer.FareClass,
                    BasePrice = offer.BasePrice,
                    Taxes = offer.Taxes,
                    TotalPrice = offer.TotalPrice,
                    Currency = offer.Currency,
                    IsRefundable = offer.IsRefundable,
                    SeatInventory = 0,
                    PassengerFullName = dto.PassengerFullName,
                    PassengerEmail = dto.PassengerEmail
                });
            }

            await unitOfWork.Tickets.AddRangeAsync(sold, ct);

            await unitOfWork.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return confirmationCode;
        }

        public async Task CancelAsync(string confirmationCode, CancellationToken cancellationToken = default)
        {
            await using var tx = await unitOfWork.BeginTransactionAsync(cancellationToken);

            var booking = await unitOfWork.Bookings.GetConfirmationCodeAsync(confirmationCode, cancellationToken);
            if (booking == null)
            {
                throw new KeyNotFoundException("Booking not found.");
            }

            await unitOfWork.Bookings.SetStatusByCodeAsync(confirmationCode, 2, cancellationToken); 

            var soldTickets = await unitOfWork.Tickets.GetSoldByBookingIdAsync(booking.Id, cancellationToken);
            if (soldTickets.Count > 0)
            {
                var scheduleId = soldTickets[0].FlightScheduleId;
                var fareClass = soldTickets[0].FareClass;

                await unitOfWork.Tickets.IncrementOfferInventoryAsync(scheduleId, fareClass, soldTickets.Count, cancellationToken);

                foreach (var t in soldTickets)
                {
                    await unitOfWork.Tickets.DeleteAsync(t.Id, cancellationToken);
                }
                    
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }

        private async Task<Ticket?> FindOfferForScheduleAsync(int flightScheduleId, string fareClass, CancellationToken cancellationToken)
        {
            var offers = await unitOfWork.Tickets.GetByFlightScheduleIdAsync(flightScheduleId, cancellationToken);
            return offers.FirstOrDefault(ticket =>
                ticket.FareClass == fareClass
                && ticket.SeatInventory > 0
                && ticket.SeatNumber == null
                && ticket.PassengerFullName == null
                && ticket.PassengerEmail == null);

        }

        public async Task<GetBookingByIdDto?> GetByCodeAsync(
            string confirmationCode,
            CancellationToken cancellationToken = default)
        {
            var booking = await unitOfWork.Bookings.GetConfirmationCodeAsync(
                confirmationCode,
                cancellationToken);

            if (booking == null)
            {
                return null;
            }

            var result = mapper.Map<GetBookingByIdDto>(booking);
            return result;
        }



    }
}
