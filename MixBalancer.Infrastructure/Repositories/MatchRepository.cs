using Microsoft.EntityFrameworkCore;
using MixBalancer.Domain.Entities;
using MixBalancer.Domain.Enums;
using MixBalancer.Domain.Interfaces;
using MixBalancer.Infrastructure.Context;

namespace MixBalancer.Infrastructure.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly MixBalancerContext _context;

        public MatchRepository(MixBalancerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Match>> GetAllAsync(MatchStatus? status, DateTime? date)
        {
            var query = _context.Matches
                .Include(m => m.TeamA)
                .ThenInclude(t => t.Players)
                .Include(m => m.TeamB)
                .ThenInclude(t => t.Players)
                .AsQueryable();

            if (status != null)
                query = query.Where(m => m.Status == status);

            if (date.HasValue)
                query = query.Where(m => m.Date.Date == date.Value.Date);

            return await query.ToListAsync();
        }

        public async Task<Match> GetByIdAsync(Guid id)
        {
            return await _context.Matches
                .Include(m => m.TeamA)
                .ThenInclude(t => t.Players)
                .Include(m => m.TeamB)
                .ThenInclude(t => t.Players)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Match match)
        {
            await _context.Matches.AddAsync(match);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Match match)
        {
            _context.Matches.Update(match);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Match match)
        {
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
        }
    }
}