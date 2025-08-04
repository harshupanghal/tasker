// Tasker.WebUI/Services/CustomAuthenticationStateProvider.cs
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication; // For HttpContext.SignInAsync
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage; // For storing user info
using System.Text.Json;

namespace Tasker.UI.Services
    {
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
        {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ProtectedLocalStorage protectedLocalStorage)
            {
            _protectedLocalStorage = protectedLocalStorage;
            }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
            {
            try
                {
                var storedUser = await _protectedLocalStorage.GetAsync<string>("currentUser");
                if (storedUser.Success && !string.IsNullOrEmpty(storedUser.Value))
                    {
                    // Deserialize claims from storage
                    var claimsJson = storedUser.Value;
                    var claims = JsonSerializer.Deserialize<List<ClaimData>>(claimsJson);

                    if (claims != null && claims.Any())
                        {
                        var identity = new ClaimsIdentity(claims.Select(c => new Claim(c.Type, c.Value)), "TaskerAuth");
                        _currentUser = new ClaimsPrincipal(identity);
                        return new AuthenticationState(_currentUser);
                        }
                    }
                }
            catch (Exception ex)
                {
                // Log the exception, e.g., if the stored data is corrupted
                Console.WriteLine($"Error retrieving user from local storage: {ex.Message}");
                // Clear potentially corrupted storage
                await _protectedLocalStorage.DeleteAsync("currentUser");
                }

            return new AuthenticationState(_currentUser); // Return unauthenticated state if no user found
            }

        // Method to call after successful login
        public async Task MarkUserAsAuthenticated(int userId, string username)
            {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                // Add any other claims you need, e.g., roles
                // new Claim(ClaimTypes.Role, "User")
            };
            var identity = new ClaimsIdentity(claims, "TaskerAuth");
            _currentUser = new ClaimsPrincipal(identity);

            // Store user claims for persistence across sessions (if browser storage is safe for your app)
            var claimsData = claims.Select(c => new ClaimData { Type = c.Type, Value = c.Value }).ToList();
            await _protectedLocalStorage.SetAsync("currentUser", JsonSerializer.Serialize(claimsData));

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
            }

        // Method to call after logout
        public async Task MarkUserAsLoggedOut()
            {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            await _protectedLocalStorage.DeleteAsync("currentUser"); // Clear stored user info
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
            }
        }

    // Helper class to serialize/deserialize claims
    public class ClaimData
        {
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        }
    }