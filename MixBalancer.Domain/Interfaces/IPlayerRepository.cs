using MixBalancer.Domain.Entities;

namespace MixBalancer.Domain.Interfaces
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> GetAllAsync();
        Task<Player> GetByIdAsync(Guid id);
        Task AddAsync(Player player);
        Task UpdateAsync(Player player);
        Task DeleteAsync(Player player);
        Task<Guid> GetPlayerIdByUserId(Guid userId);
    }
}