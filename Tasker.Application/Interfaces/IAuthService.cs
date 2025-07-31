using Tasker.Application.DTOs;

// here interfaces are defined for auth services : register and login.
// we also imported the register and login dtos to define their structure in parameters

namespace Tasker.Application.Interfaces
{
    public interface IAuthService
    {
       Task<AuthResponse> RegisterAsync(RegisterRequest request);

        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
