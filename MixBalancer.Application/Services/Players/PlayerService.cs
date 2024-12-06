using MixBalancer.Application.Dtos.Player;
using MixBalancer.Domain.Entities;
using MixBalancer.Domain.Interfaces;

namespace MixBalancer.Application.Services.Players
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
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

            return new ServiceResult<IEnumerable<PlayerResultDto>> { IsSuccess = true, Players = result };
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
    }
}
