using MixBalancer.Application.Dtos.Match;
using MixBalancer.Application.Dtos.Player;
using MixBalancer.Domain.Entities;
using MixBalancer.Domain.Interfaces;

namespace MixBalancer.Application.Services.Players
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IMatchRepository _matchRepository;

        public PlayerService(IPlayerRepository playerRepository, IMatchRepository matchRepository)
        {
            _playerRepository = playerRepository;
            _matchRepository = matchRepository;
        }

        public async Task<ServiceResult> CreatePlayerAsync(CreatePlayerDto model)
        {

            var player = new Player
            {
                Nickname = model.NickName,
                SkillLevel = model.SkillLevel
            };

            await _playerRepository.AddAsync(player);

            return new ServiceResult { IsSuccess = true };
        }

        public async Task<ServiceResult<IEnumerable<PlayerResultDto>>> GetAllPlayersAsync()
        {
            var players = await _playerRepository.GetAllAsync();

            var result = players.Select(p => new PlayerResultDto
            {
                Id = p.Id,
                NickName = p.Nickname,
                SkillLevel = p.SkillLevel
            });

            return new ServiceResult<IEnumerable<PlayerResultDto>> { IsSuccess = true, Data = result };
        }

        public async Task<ServiceResult> UpdatePlayerAsync(Guid id, UpdatePlayerDto model)
        {
            var player = await _playerRepository.GetByIdAsync(id);

            if (player == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Player not found" };

            player.Nickname = model.NickName;
            player.SkillLevel = model.SkillLevel;

            await _playerRepository.UpdateAsync(player);

            return new ServiceResult { IsSuccess = true };
        }

        public async Task<ServiceResult> DeletePlayerAsync(Guid id)
        {
            var player = await _playerRepository.GetByIdAsync(id);

            if (player == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Player not found" };

            await _playerRepository.DeleteAsync(player);

            return new ServiceResult { IsSuccess = true };
        }

        public async Task<ServiceResult> GetPlayerProfileAsync(Guid userId)
        {
            var playerId = await _playerRepository.GetPlayerIdByUserId(userId);

            var player = await _playerRepository.GetByIdAsync(playerId);
            var matchHistories = await _matchRepository.GetPlayerMatchHistory(playerId);

            if (player == null)
                return new ServiceResult { IsSuccess = false, ErrorMessage = "Player not found" };

            var playerProfile = new PlayerProfileDto
            {
                Nickname = player.Nickname,
                SkillLevel = player.SkillLevel,
                WinRate = player.WinRate,
                KDRatio = player.KDRatio,
                HeadshotPercentage = player.HeadshotPercentage,
                TotalMatches = player.TotalMatches,
                BestMap = player.BestMap,
                WorstMap = player.WorstMap,
                Aces = player.Aces,
                Clutches = player.Clutches,
                MatchHistory = matchHistories.Select(m => new MatchHistoryDto
                {
                    Date = m.Date,
                    Map = m.Map,
                    Result = m.Result,
                    KD = m.KD
                }).ToList()
            };

            return new ServiceResult<PlayerProfileDto> { IsSuccess = true, Data = playerProfile };
        }
    }
}
