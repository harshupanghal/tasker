using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Tasker.Web.Services;
public class UserSessionService
    {
    private readonly ProtectedLocalStorage _storage;
    private const string Key = "UserSession";

    public int UserId { get; private set; }
    public string? Username { get; private set; }
    public bool IsLoggedIn => UserId > 0;

    public UserSessionService(ProtectedLocalStorage storage)
        {
        _storage = storage;
        }

    public async Task SetUserAsync(int userId, string username)
        {
        UserId = userId;
        Username = username;
        await _storage.SetAsync(Key, new UserSessionDto { UserId = userId, Username = username });
        }

    public async Task LoadSessionAsync()
        {
        try
            {
            var result = await _storage.GetAsync<UserSessionDto>(Key);
            if (result.Success && result.Value != null)
                {
                UserId = result.Value.UserId;
                Username = result.Value.Username;
                }
            }
        catch
            {
            // ignore and assume logged out
            }
        }

    public async Task ClearAsync()
        {
        UserId = 0;
        Username = null;
        await _storage.DeleteAsync(Key);
        }

    private class UserSessionDto
        {
        public int UserId { get; set; }
        public string? Username { get; set; }
        }
    }
