using MixBalancer.Application.Dtos.Team;
using MixBalancer.Domain.Entities;

namespace MixBalancer.Application.Services.Team
{
    public interface ITeamService
    {
        Task<ServiceResult> CreateTeamAsync(CreateTeamDto model);
        Task<ServiceResult<IEnumerable<TeamResultDto>>> GetAllTeamsAsync();
        Task<ServiceResult> UpdateTeamAsync(Guid id, UpdateTeamDto model);
        Task<ServiceResult> DeleteTeamAsync(Guid id);
        Task<ServiceResult> AddPlayerToTeamAsync(Guid teamId, AddPlayerToTeamDto model);
        Task<ServiceResult> RemovePlayerFromTeamAsync(Guid teamId, RemovePlayerFromTeamDto model);
        Task<ServiceResult<IEnumerable<TeamResultDto>>> GetMyTeamsAsync(Guid userId);
    }
}