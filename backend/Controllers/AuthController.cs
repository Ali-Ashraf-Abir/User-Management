using Microsoft.AspNetCore.Mvc;
using task4.Dtos;
using task4.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterDto data)
    {

        try
        {
            var result = await authService.RegisterUser(data);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(
    LoginDto dto)
    {
        try
        {
            var result =
                await authService.LoginUser(dto);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return Unauthorized(new
            {
                message = ex.Message
            });
        }
    }
}