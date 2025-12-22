using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Bookings;
using AutoMapper;

namespace AirportTool.Application.Services.Bookings
{
    public class BookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private string GenerateConfirmationCode()
        {
            string guidString = Guid.NewGuid().ToString("N");
            string code = guidString.Substring(0, 8).ToUpper();
            return code;
        }

        public async Task<string> CreateAsync(
            CreateBookingDto dto,
            CancellationToken cancellationToken = default)
        {
            var schedule = await _unitOfWork.FlightSchedules
                .GetByIdAsync(dto.FlightScheduleId, cancellationToken);

            if (schedule == null)
            {
                throw new KeyNotFoundException("Flight schedule not found.");
            }

            if (dto.Quantity <= 0)
            {
                throw new ArgumentException("Booking quantity must be greater than zero.");
            }

            var booking = _mapper.Map<Booking>(dto);
            booking.CreatedUtc = DateTime.UtcNow;
            booking.ConfirmationCode = GenerateConfirmationCode();
            booking.BookingStatusId = 1; 

            await _unitOfWork.Bookings.AddAsync(booking, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return booking.ConfirmationCode;
        }

        public async Task<GetBookingByIdDto?> GetByCodeAsync(
            string confirmationCode,
            CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings.GetConfirmationCodeAsync(
                confirmationCode,
                cancellationToken);

            if (booking == null)
            {
                return null;
            }

            var result = _mapper.Map<GetBookingByIdDto>(booking);
            return result;
        }

        public async Task CancelAsync(
            string confirmationCode,
            CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings.GetConfirmationCodeAsync(
                confirmationCode,
                cancellationToken);

            if (booking == null)
            {
                throw new KeyNotFoundException("Booking not found.");
            }

            if (booking.BookingStatusId == 2)
            {
                throw new InvalidOperationException("Booking is already cancelled.");
            }

            booking.BookingStatusId = 2;

            await _unitOfWork.Bookings.UpdateAsync(booking, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
