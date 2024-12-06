using MixBalancer.Domain.Entities;
using MixBalancer.Domain.Enums;

namespace MixBalancer.Domain.Interfaces
{
    public interface IMatchRepository
    {
        Task<IEnumerable<Match>> GetAllAsync(MatchStatus? status, DateTime? date);
        Task<Match> GetByIdAsync(Guid id);
        Task AddAsync(Match match);
        Task UpdateAsync(Match match);
        Task DeleteAsync(Match match);
    }
}