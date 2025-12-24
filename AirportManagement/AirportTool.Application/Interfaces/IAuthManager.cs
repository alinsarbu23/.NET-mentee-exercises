using AirportTool.Infrastructure.DTOs.Auth;

namespace AirportTool.Application.Interfaces
{
    public interface IAuthManager
    {
        Task<IEnumerable<string>> RegisterUserAsync(RegisterDto dto, CancellationToken ct);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto, CancellationToken ct);
    }
}
