
using JobBoard.BLL.Interfaces;
using JobBoard.Common.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST: api/Auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            var result = await _authService.RegisterAsync(dto);

            return Ok(new
            {
                message = "Registration successful",
                data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var result = await _authService.LoginAsync(dto);

            return Ok(new
            {
                message = "Login successful",
                data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}
