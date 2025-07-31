using Tasker.Domain.Entities;

// interfaces for user related functions 

namespace Tasker.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<int> AddUserAsync(User user);
        Task<User?> GetByIdAsync(int id);
    }
}
