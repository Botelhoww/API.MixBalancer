using MixBalancer.Domain.Entities;

namespace MixBalancer.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<bool> EmailExistsAsync(string email);
    }
}