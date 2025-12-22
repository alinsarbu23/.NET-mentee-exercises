using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Tickets;
using AutoMapper;

namespace AirportTool.Application.Services.Tickets
{
    public class TicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetTicketByIdDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id, cancellationToken);

            if (ticket == null)
            {
                return null;
            }

            var result = _mapper.Map<GetTicketByIdDto>(ticket);
            return result;
        }

        public async Task<IReadOnlyList<GetTicketByIdDto>> GetByFlightScheduleAsync(int flightScheduleId, CancellationToken cancellationToken = default)
        {
            var tickets = await _unitOfWork.Tickets.GetByFlightScheduleIdAsync(flightScheduleId, cancellationToken);

            var result = _mapper.Map<IReadOnlyList<GetTicketByIdDto>>(tickets);
            return result;
        }

        public async Task<int> CreateAsync(CreateTicketDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.BasePrice < 0)
            {
                throw new ArgumentException("Base price must be non-negative.");
            }

            if (dto.Taxes < 0)
            {
                throw new ArgumentException("Taxes must be non-negative.");
            }

            var ticket = _mapper.Map<Ticket>(dto);

            await _unitOfWork.Tickets.AddAsync(ticket, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return (int)ticket.Id;
        }

        public async Task UpdateAsync(int ticketId, UpdateTicketDto dto, CancellationToken cancellationToken = default)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId, cancellationToken);

            if (ticket == null)
            {
                throw new KeyNotFoundException("Ticket not found.");
            }

            ticket.SeatInventory = dto.SeatInventory;

            await _unitOfWork.Tickets.UpdateAsync(ticket, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.Tickets.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}