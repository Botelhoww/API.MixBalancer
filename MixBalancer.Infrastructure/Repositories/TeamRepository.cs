using Microsoft.EntityFrameworkCore;
using MixBalancer.Application.Dtos.Team;
using MixBalancer.Domain.Entities;
using MixBalancer.Domain.Interfaces;
using MixBalancer.Infrastructure.Context;

namespace MixBalancer.Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly MixBalancerContext _context;

        public TeamRepository(MixBalancerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _context.Teams.Include(t => t.Players).ToListAsync();
        }

        public async Task<Team> GetByIdAsync(Guid id)
        {
            return await _context.Teams.Include(t => t.Players).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Team team)
        {
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Team>> GetTeamsByUserIdAsync(Guid userId)
        {
            var myTeams = await _context.Teams.Include(t => t.Players).Where(t => t.ManagedByUserId == userId).ToListAsync();

            return myTeams;
        }
    }
}