using MixBalancer.Application.Dtos.Match;
using MixBalancer.Application.DTOs;
using MixBalancer.Domain.Enums;

namespace MixBalancer.Application.Services
{
    public interface IMatchService
    {
        // Cria uma nova partida
        Task<ServiceResult> CreateMatchAsync(CreateMatchDto model);

        // Obtém detalhes de uma partida por ID
        Task<ServiceResult<MatchResultDto>> GetMatchByIdAsync(Guid id);

        // Atualiza status ou placar de uma partida
        Task<ServiceResult> UpdateMatchAsync(Guid id, UpdateMatchDto model);

        // Lista todas as partidas
        Task<ServiceResult<IEnumerable<MatchResultDto>>> GetAllMatchesAsync(MatchStatus? status, DateTime? date);

        // Cancela uma partida
        Task<ServiceResult> CancelMatchAsync(Guid id);

        // Atribui um Match ID do CS2 à partida no MixBalancer
        Task<ServiceResult> SetMatchIdCS2Async(Guid matchId, string matchIdCS2);

        // Adiciona um jogador a uma partida (caso necessário)
        Task<ServiceResult> AddPlayerToMatchAsync(Guid matchId, AddPlayerDto model);

        // Balanceia os times de uma partida
        Task<ServiceResult> BalanceTeamsAsync(Guid matchId);
    }
}