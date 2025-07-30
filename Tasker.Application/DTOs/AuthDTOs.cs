namespace Tasker.Application.DTOs;
public class RegisterRequest
{
    public string userName { get; set; }
    public string password { get; set; }
}

public class LoginRequest
{
    public string userName { get; set; }
    public string password { get; set; }
}

public class AuthResponse
{
    public int UserId { get; set; }
    public string userName { get; set; }
    public string message { get; set; }
    public bool Success { get; set; }
}
