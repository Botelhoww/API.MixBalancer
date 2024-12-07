using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MixBalancer.Application.Dtos.Match;
using MixBalancer.Application.DTOs;
using MixBalancer.Application.Services;
using MixBalancer.Domain.Enums;

namespace MixBalancer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        // Cria uma nova partida
        [HttpPost]
        public async Task<IActionResult> CreateMatch([FromBody] CreateMatchDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _matchService.CreateMatchAsync(model);
            return result.IsSuccess
                ? Ok(new { message = "Match created successfully" })
                : BadRequest(new { message = result.ErrorMessage });
        }

        // Obtém detalhes de uma partida por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatchById([FromRoute] Guid id)
        {
            var result = await _matchService.GetMatchByIdAsync(id);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(new { message = result.ErrorMessage });
        }

        // Atualiza status ou placar de uma partida
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatch([FromRoute] Guid id, [FromBody] UpdateMatchDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _matchService.UpdateMatchAsync(id, model);
            return result.IsSuccess
                ? Ok(new { message = "Match updated successfully" })
                : BadRequest(new { message = result.ErrorMessage });
        }

        // Lista todas as partidas
        [HttpGet]
        public async Task<IActionResult> GetAllMatches([FromQuery] MatchStatus? status, [FromQuery] DateTime? date)
        {
            var result = await _matchService.GetAllMatchesAsync(status, date);
            return Ok(result.Data);
        }

        // (Opcional) Cancela uma partida
        [HttpDelete("{id}/cancel")]
        public async Task<IActionResult> CancelMatch([FromRoute] Guid id)
        {
            var result = await _matchService.CancelMatchAsync(id);
            return result.IsSuccess
                ? Ok(new { message = "Match canceled successfully" })
                : BadRequest(new { message = result.ErrorMessage });
        }
    }
}