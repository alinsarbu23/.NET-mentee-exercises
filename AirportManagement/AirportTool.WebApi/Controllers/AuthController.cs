using AirportTool.Application.Interfaces;
using AirportTool.Infrastructure.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirportTool.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager auth;

        public AuthController(IAuthManager auth)
        {
            this.auth = auth;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto dto, CancellationToken ct)
        {
            var errors = await auth.RegisterUserAsync(dto, ct);
            if (errors.Any())
                return BadRequest(new { errors });

            return StatusCode(201);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto, CancellationToken ct)
        {
            var result = await auth.LoginAsync(dto, ct);
            if (result == null)
                return Unauthorized();

            return Ok(result);
        }
    }
}
