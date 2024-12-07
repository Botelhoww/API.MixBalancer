using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MixBalancer.Application.Dtos.Player;
using MixBalancer.Application.Services.Players;

namespace MixBalancer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerManagementService;

        public PlayerController(IPlayerService playerManagementService)
        {
            _playerManagementService = playerManagementService;
        }

        [HttpPost("players")]
        public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _playerManagementService.CreatePlayerAsync(model);

            return result.IsSuccess
                ? Ok(new { message = "Player created successfully" })
                : BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("players")]
        public async Task<IActionResult> GetAllPlayers()
        {
            var result = await _playerManagementService.GetAllPlayersAsync();
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("players/{id}")]
        public async Task<IActionResult> UpdatePlayer([FromRoute] Guid id, [FromBody] UpdatePlayerDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _playerManagementService.UpdatePlayerAsync(id, model);
            return result.IsSuccess
                ? Ok(new { message = "Player updated successfully" })
                : BadRequest(new { message = result.ErrorMessage });
        }


        [HttpDelete("players/{id}")]
        public async Task<IActionResult> DeletePlayer([FromRoute] Guid id)
        {
            var result = await _playerManagementService.DeletePlayerAsync(id);
            return result.IsSuccess
                ? Ok(new { message = "Player deleted successfully" })
                : BadRequest(new { message = result.ErrorMessage });
        }
    }
}