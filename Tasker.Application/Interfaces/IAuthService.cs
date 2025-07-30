using Tasker.Application.DTOs;

namespace Tasker.Application.Interfaces
{
    public interface IAuthService
    {
       Task<AuthResponse> RegisterAsync(RegisterRequest request);

        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
