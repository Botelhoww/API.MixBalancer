using Microsoft.EntityFrameworkCore;
using MixBalancer.Domain.Entities;
using MixBalancer.Domain.Interfaces;
using MixBalancer.Infrastructure.Context;

namespace MixBalancer.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MixBalancerContext _context;

        public UserRepository(MixBalancerContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(string email) =>
            await _context.Users.AnyAsync(u => u.Email == email);
    }
}
