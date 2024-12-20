﻿using MixBalancer.Application.Dtos.Player;

namespace MixBalancer.Application.Services.Players
{
    public interface IPlayerService
    {
        Task<ServiceResult> CreatePlayerAsync(CreatePlayerDto model);
        Task<ServiceResult<IEnumerable<PlayerResultDto>>> GetAllPlayersAsync();
        Task<ServiceResult> UpdatePlayerAsync(Guid id, UpdatePlayerDto model);
        Task<ServiceResult> DeletePlayerAsync(Guid id);
        Task<ServiceResult> GetPlayerProfileAsync(Guid userId);
    }
}