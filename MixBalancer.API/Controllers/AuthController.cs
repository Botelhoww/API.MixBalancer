using Microsoft.AspNetCore.Mvc;
using MixBalancer.Application.Dtos;
using MixBalancer.Application.Dtos.User;
using MixBalancer.Application.Services;
using System.Security.Claims;

namespace MixBalancer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            return result.IsSuccess
                ? Ok(new { token = result.Token })
                : BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);

            return result.IsSuccess
                ? Ok(new { token = result.Token })
                : Unauthorized(new { message = result.ErrorMessage });
        }

        [HttpGet("me")]
        public async Task<IActionResult> MyInfos()
        {
            var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

            var result = await _authService.GetUserAsync(userEmail);

            return result.IsSuccess
                ? Ok(new { token = result })
                : Unauthorized(new { message = result.ErrorMessage });
        }
    }
}
