using Tasker.Application.DTOs;
using Tasker.Application.Interfaces;
using Tasker.Domain.Entities;

// register and login related tasks here, like registering a user, validations, etc.

namespace Tasker.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {

                var existingUser = await _userRepository.GetByUsernameAsync(request.userName);
                if (existingUser != null)
                {
                    return new AuthResponse { Success = false, message = "Username already exists." };
                }
                var newUser = new User
                {
                    userName = request.userName,
                    password = request.password,
                    CreatedAt = DateTime.UtcNow
                };

                var newUserId = await _userRepository.AddUserAsync(newUser);
                newUser.Id = newUserId;

                return new AuthResponse
                {
                    Success = true,
                    message = "Registration successful.",
                    UserId = newUser.Id,
                    userName = newUser.userName
                };
            }

            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not register user to the database.", ex);
            }
        }

        public async System.Threading.Tasks.Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {

                var user = await _userRepository.GetByUsernameAsync(request.userName);
                if (user == null || user.password != request.password)
                {
                    return new AuthResponse { Success = false, message = "Invalid username or password." };
                }

                return new AuthResponse
                {
                    Success = true,
                    message = "Login successful.",
                    UserId = user.Id,
                    userName = user.userName
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while logging in", ex);

            }
        }
    }
}
