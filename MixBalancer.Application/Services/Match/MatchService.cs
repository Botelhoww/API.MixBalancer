using MixBalancer.Application.Dtos.Match;
using MixBalancer.Application.Dtos.Player;
using MixBalancer.Application.Dtos.Team;
using MixBalancer.Application.DTOs;
using MixBalancer.Domain.Entities;
using MixBalancer.Domain.Enums;
using MixBalancer.Domain.Interfaces;

namespace MixBalancer.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ITeamRepository _teamRepository;

        public MatchService(IMatchRepository matchRepository, ITeamRepository teamRepository)
        {
            _matchRepository = matchRepository;
            _teamRepository = teamRepository;
        }

        public async Task<ServiceResult> CreateMatchAsync(CreateMatchDto model)
        {
            // Verificar se os times existem
            var teamA = await _teamRepository.GetByIdAsync(model.TeamAId);
            var teamB = await _teamRepository.GetByIdAsync(model.TeamBId);

            if (teamA == null || teamB == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "One or both teams not found." };

            var match = new Match
            {
                Id = Guid.NewGuid(),
                TeamAId = model.TeamAId,
                TeamBId = model.TeamBId,
                Date = model.Date,
                Status = MatchStatus.Created,
                ScoreTeamA = 0,
                ScoreTeamB = 0
            };

            await _matchRepository.AddAsync(match);

            return new ServiceResult { IsSuccess = true };
        }

        public async Task<ServiceResult<MatchResultDto>> GetMatchByIdAsync(Guid id)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null)
                return new ServiceResult<MatchResultDto> { IsSuccess = false, ErrorMessage = "Match not found." };

            var matchDto = new MatchResultDto
            {
                Id = match.Id,
                Date = match.Date,
                Status = match.Status,
                ScoreTeamA = match.ScoreTeamA,
                ScoreTeamB = match.ScoreTeamB,
                TeamA = new TeamResultDto
                {
                    Id = match.TeamA.Id,
                    Name = match.TeamA.Name,
                    Players = match.TeamA.Players.Select(p => new PlayerResultDto
                    {
                        Id = p.Id,
                        NickName = p.Nickname,
                        SkillLevel = p.SkillLevel
                    }).ToList(),
                    AverageSkillLevel = match.TeamA.AverageSkillLevel,
                    ManagedByUserId = match.TeamA.ManagedByUserId
                },
                TeamB = new TeamResultDto
                {
                    Id = match.TeamB.Id,
                    Name = match.TeamB.Name,
                    Players = match.TeamB.Players.Select(p => new PlayerResultDto
                    {
                        Id = p.Id,
                        NickName = p.Nickname,
                        SkillLevel = p.SkillLevel
                    }).ToList(),
                    AverageSkillLevel = match.TeamB.AverageSkillLevel,
                    ManagedByUserId = match.TeamB.ManagedByUserId
                }
            };

            return new ServiceResult<MatchResultDto> { IsSuccess = true, Data = matchDto };
        }

        public async Task<ServiceResult> UpdateMatchAsync(Guid id, UpdateMatchDto model)
        {
            var match = await _matchRepository.GetByIdAsync(id);

            if (model.Status == MatchStatus.Finished)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Unable to update. Match is already finished." };

            if (model.Status == MatchStatus.Cancelled)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Unable to update. Match is cancelled." };

            if (match == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Match not found." };

            if (model.Status != match.Status)
                match.Status = model.Status;

            if (model.ScoreTeamA.HasValue)
                match.ScoreTeamA = model.ScoreTeamA.Value;

            if (model.ScoreTeamB.HasValue)
                match.ScoreTeamB = model.ScoreTeamB.Value;

            await _matchRepository.UpdateAsync(match);

            return new ServiceResult { IsSuccess = true };
        }

        public async Task<ServiceResult<IEnumerable<MatchResultDto>>> GetAllMatchesAsync(MatchStatus? status, DateTime? date)
        {
            var matches = await _matchRepository.GetAllAsync(status, date);

            var matchDtos = matches.Select(m => new MatchResultDto
            {
                Id = m.Id,
                Date = m.Date,
                Status = m.Status,
                ScoreTeamA = m.ScoreTeamA,
                ScoreTeamB = m.ScoreTeamB,
                TeamA = new TeamResultDto
                {
                    Id = m.TeamA.Id,
                    Name = m.TeamA.Name,
                    Players = m.TeamA.Players.Select(p => new PlayerResultDto
                    {
                        Id = p.Id,
                        NickName = p.Nickname,
                        SkillLevel = p.SkillLevel
                    }).ToList(),
                    AverageSkillLevel = m.TeamA.AverageSkillLevel,
                    ManagedByUserId = m.TeamA.ManagedByUserId
                },
                TeamB = new TeamResultDto
                {
                    Id = m.TeamB.Id,
                    Name = m.TeamB.Name,
                    Players = m.TeamB.Players.Select(p => new PlayerResultDto
                    {
                        Id = p.Id,
                        NickName = p.Nickname,
                        SkillLevel = p.SkillLevel
                    }).ToList(),
                    AverageSkillLevel = m.TeamB.AverageSkillLevel,
                    ManagedByUserId = m.TeamB.ManagedByUserId
                }
            });

            return new ServiceResult<IEnumerable<MatchResultDto>> { IsSuccess = true, Data = matchDtos };
        }

        public async Task<ServiceResult> CancelMatchAsync(Guid id)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Match not found." };

            match.Status = MatchStatus.Cancelled;

            await _matchRepository.UpdateAsync(match);

            return new ServiceResult { IsSuccess = true };
        }
    }
}