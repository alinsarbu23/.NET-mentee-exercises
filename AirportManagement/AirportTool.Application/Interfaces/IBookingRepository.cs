using AirportTool.Domain.Models;

namespace AirportTool.Application.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<Booking?> GetConfirmationCodeAsync(
            string confirmationCode,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Booking>> GetActiveByFlightScheduleAsync(
            int flightScheduleId,
            CancellationToken cancellationToken=default);
    }
}
