using Microsoft.AspNetCore.Mvc;
using MixBalancer.Application.Dtos.Team;
using MixBalancer.Application.Services;
using MixBalancer.Application.Services.Team;
using Newtonsoft.Json;

namespace MixBalancer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost("teams")]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _teamService.CreateTeamAsync(model);
            return result.IsSuccess
                ? Ok(new { message = "Team created successfully" })
                : BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("teams")]
        public async Task<IActionResult> GetAllTeams()
        {
            ServiceResult<IEnumerable<TeamResultDto>> result = await _teamService.GetAllTeamsAsync();

            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result.Players);
        }

        [HttpPut("teams/{id}")]
        public async Task<IActionResult> UpdateTeam([FromRoute] Guid id, [FromBody] UpdateTeamDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _teamService.UpdateTeamAsync(id, model);
            return result.IsSuccess
                ? Ok(new { message = "Team updated successfully" })
                : BadRequest(new { message = result.ErrorMessage });
        }

        [HttpDelete("teams/{id}")]
        public async Task<IActionResult> DeleteTeam([FromRoute] Guid id)
        {
            var result = await _teamService.DeleteTeamAsync(id);
            return result.IsSuccess
                ? Ok(new { message = "Team deleted successfully" })
                : BadRequest(new { message = result.ErrorMessage });
        }


        [HttpPost("teams/{id}/add-player")]
        public async Task<IActionResult> AddPlayerToTeam([FromRoute] Guid id, [FromBody] AddPlayerToTeamDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _teamService.AddPlayerToTeamAsync(id, model);
            return result.IsSuccess
                ? Ok(new { message = "Player added to team successfully" })
                : BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPost("teams/{id}/remove-player")]
        public async Task<IActionResult> RemovePlayerFromTeam([FromRoute] Guid id, [FromBody] RemovePlayerFromTeamDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _teamService.RemovePlayerFromTeamAsync(id, model);
            return result.IsSuccess
                ? Ok(new { message = "Player removed from team successfully" })
                : BadRequest(new { message = result.ErrorMessage });
        }
    }
}