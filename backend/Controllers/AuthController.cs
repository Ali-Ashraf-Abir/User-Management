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
}