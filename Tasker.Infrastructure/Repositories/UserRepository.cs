using Microsoft.EntityFrameworkCore;
using Tasker.Application.Interfaces;
using Tasker.Domain.Entities;
using Tasker.Infrastructure.Database;
using Tasker.Infrastructure.Persistence;

namespace Tasker.Infrastructure.Repositories
    {
    public class UserRepository : IUserRepository
        {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
            {
            _dbContext = dbContext;
            }

        public async Task<int> AddUserAsync(User user)
            {
            try
                {
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return user.Id;
                }
            catch (Exception ex)
                {
                throw new InvalidOperationException("Could not add user to the database.", ex);
                }
            }

        public async Task<User?> GetByUsernameAsync(string username)
            {
            try
                {
                return await _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.userName == username);
                }
            catch (Exception ex)
                {
                throw new InvalidOperationException("Could not retrieve user by username.", ex);
                }
            }

        public async Task<User?> GetByIdAsync(int id)
            {
            try
                {
                return await _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);
                }
            catch (Exception ex)
                {
                throw new InvalidOperationException("Could not retrieve user by ID.", ex);
                }
            }
        }
    }
