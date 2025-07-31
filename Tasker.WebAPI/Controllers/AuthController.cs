using Microsoft.AspNetCore.Mvc;
using Tasker.Application.DTOs;
using Tasker.Application.Interfaces;

namespace Tasker.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST /api/Auth/register
    [HttpPost("register")]
    public async System.Threading.Tasks.Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
            {

            if (!ModelState.IsValid)
                {
                return BadRequest(ModelState);
                }

            var response = await _authService.RegisterAsync(request);

            if (response.Success)
                {
                return Ok(response);
                }
            else
                {
                return BadRequest(response);
                }
            }
        catch (Exception ex) {
            throw new InvalidOperationException("Something went wrong", ex);

            }
        }

    // POST /api/Auth/login
    [HttpPost("login")]
    public async System.Threading.Tasks.Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
            {

            if (!ModelState.IsValid)
                {
                return BadRequest(ModelState);
                }

            var response = await _authService.LoginAsync(request);

            if (response.Success)
                {
                return Ok(response);
                }
            else
                {
                return Unauthorized(response);
                }
            }
        catch (Exception ex)
            {
            throw new InvalidOperationException("Something went wrong", ex);
            }
    }
}
