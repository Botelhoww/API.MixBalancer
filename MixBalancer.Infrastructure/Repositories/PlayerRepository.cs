using Microsoft.EntityFrameworkCore;
using MixBalancer.Domain.Entities;
using MixBalancer.Domain.Interfaces;
using MixBalancer.Infrastructure.Context;

namespace MixBalancer.Infrastructure.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly MixBalancerContext _context;

        public PlayerRepository(MixBalancerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task<Player> GetByIdAsync(Guid id)
        {
            return await _context.Players.FindAsync(id);
        }

        public async Task AddAsync(Player player)
        {
            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateAsync(Player player)
        {
            _context.Players.Update(player);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Player player)
        {
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
        }

        public async Task<Guid> GetPlayerIdByUserId(Guid userId)
        {
            return await _context.Players
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();
        }
    }
}