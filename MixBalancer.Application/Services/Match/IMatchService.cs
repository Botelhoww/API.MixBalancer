using MixBalancer.Application.Dtos.Match;
using MixBalancer.Application.DTOs;
using MixBalancer.Domain.Enums;

namespace MixBalancer.Application.Services
{
    public interface IMatchService
    {
        Task<ServiceResult> CreateMatchAsync(CreateMatchDto model);
        Task<ServiceResult<MatchResultDto>> GetMatchByIdAsync(Guid id);
        Task<ServiceResult> UpdateMatchAsync(Guid id, UpdateMatchDto model);
        Task<ServiceResult<IEnumerable<MatchResultDto>>> GetAllMatchesAsync(MatchStatus? status, DateTime? date);
        Task<ServiceResult> CancelMatchAsync(Guid id);
    }
}