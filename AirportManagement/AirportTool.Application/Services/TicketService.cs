using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Tickets;
using AutoMapper;

namespace AirportTool.Application.Services.Tickets
{
    public class TicketService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IReadOnlyList<GetTicketByIdDto>> GetOffersByScheduleAsync(int flightScheduleId, CancellationToken cancellationToken = default)
        {
            var offers = await unitOfWork.Tickets.GetOffersByScheduleIdAsync(flightScheduleId, cancellationToken);
            return mapper.Map<IReadOnlyList<GetTicketByIdDto>>(offers);
        }

        public async Task<GetTicketByIdDto?> GetOfferByIdAsync(long id, CancellationToken ct = default)
        {
            var offer = await unitOfWork.Tickets.GetOfferByIdAsync(id, ct);
            if(offer == null)
            {
                return null;
            }
            return mapper.Map<GetTicketByIdDto>(offer);
        }

        public async Task<long> CreateOfferAsync(CreateTicketDto dto, CancellationToken ct = default)
        {
            if (dto.BasePrice < 0)
            {
                throw new ArgumentException("Base price must be non-negative.");
            }

            if (dto.Taxes < 0)
            {
                throw new ArgumentException("Taxes must be non-negative.");
            }

            if (dto.SeatInventory < 0)
            {
                throw new ArgumentException("SeatInventory must be non-negative.");
            }

            if (string.IsNullOrWhiteSpace(dto.FareClass))
            {
                throw new ArgumentException("FareClass is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Currency))
            {
                throw new ArgumentException("Currency is required.");
            }

            var schedule = await unitOfWork.FlightSchedules.GetByIdAsync(dto.FlightScheduleId, ct);
            if (schedule == null)
            {
                throw new KeyNotFoundException("Flight schedule not found.");
            }

            var offersBookingId = await unitOfWork.Bookings.GetOffersBookingIdAsync(ct);

            var offer = mapper.Map<Ticket>(dto);
            offer.BookingId = offersBookingId;
            offer.TotalPrice = dto.BasePrice + dto.Taxes;
            offer.SeatNumber = null;
            offer.PassengerFullName = null;
            offer.PassengerEmail = null;

            await unitOfWork.Tickets.AddAsync(offer, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return offer.Id;
        }

        public async Task UpdateInventoryAsync(long id, UpdateTicketDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.SeatInventory < 0)
            {
                throw new ArgumentException("SeatInventory must be non-negative.");
            }
                

            var offer = await unitOfWork.Tickets.GetOfferByIdAsync(id, cancellationToken);
            if (offer == null)
            {
                throw new KeyNotFoundException("Ticket offer not found.");
            }
                
            offer.SeatInventory = dto.SeatInventory;

            await unitOfWork.Tickets.UpdateAsync(offer, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteOfferAsync(long id, CancellationToken cancellationToken = default)
        {
            var offer = await unitOfWork.Tickets.GetOfferByIdAsync(id, cancellationToken);
            if (offer == null)
            {
                throw new KeyNotFoundException("Ticket offer not found.");
            }
                
            await unitOfWork.Tickets.DeleteAsync(id, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
