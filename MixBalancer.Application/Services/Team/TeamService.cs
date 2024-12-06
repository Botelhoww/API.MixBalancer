using MixBalancer.Application.Dtos.Player;
using MixBalancer.Application.Dtos.Team;
using MixBalancer.Domain.Entities;
using MixBalancer.Domain.Interfaces;

namespace MixBalancer.Application.Services.Team
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;

        public TeamService(ITeamRepository teamRepository, IPlayerRepository playerRepository)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
        }

        public async Task<ServiceResult> CreateTeamAsync(CreateTeamDto model)
        {
            var team = new Domain.Entities.Team
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                ManagedByUserId = model.ManagedByUserId,
                AverageSkillLevel = 0,
                Players = new List<Player>(),
            };

            await _teamRepository.AddAsync(team);

            return new ServiceResult { IsSuccess = true };
        }

        public async Task<ServiceResult<IEnumerable<TeamResultDto>>> GetAllTeamsAsync()
        {
            var teams = await _teamRepository.GetAllAsync();
            var result = teams.Select(t => new TeamResultDto
            {
                Id = t.Id,
                Name = t.Name,
                Players = t.Players.Select(p => new PlayerResultDto
                {
                    Id = p.Id,
                    NickName = p.Nickname,
                    SkillLevel = p.SkillLevel
                }).ToList() ?? new List<PlayerResultDto>(),
                AverageSkillLevel = t.AverageSkillLevel,
                ManagedByUserId = t.ManagedByUserId
            });

            return new ServiceResult<IEnumerable<TeamResultDto>> { IsSuccess = true, Data = result };
        }

        public async Task<ServiceResult> UpdateTeamAsync(Guid id, UpdateTeamDto model)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Team not found" };

            team.Name = model.Name;
            team.ManagedByUserId = model.ManagedByUserId;

            await _teamRepository.UpdateAsync(team);

            return new ServiceResult { IsSuccess = true };
        }

        public async Task<ServiceResult> DeleteTeamAsync(Guid id)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Team not found" };

            await _teamRepository.DeleteAsync(team);

            return new ServiceResult { IsSuccess = true };
        }

        public async Task<ServiceResult> AddPlayerToTeamAsync(Guid teamId, AddPlayerToTeamDto model)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Team not found" };

            var player = await _playerRepository.GetByIdAsync(model.PlayerId);
            if (player == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Player not found" };

            team.Players.Add(player);
            await _teamRepository.UpdateAsync(team);

            return new ServiceResult { IsSuccess = true };
        }

        public async Task<ServiceResult> RemovePlayerFromTeamAsync(Guid teamId, RemovePlayerFromTeamDto model)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Team not found" };

            var player = team.Players.FirstOrDefault(p => p.Id == model.PlayerId);
            if (player == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Player not found in team" };

            team.Players.Remove(player);
            await _teamRepository.UpdateAsync(team);

            return new ServiceResult { IsSuccess = true };
        }
    }
}