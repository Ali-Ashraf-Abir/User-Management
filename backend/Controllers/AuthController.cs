using Microsoft.AspNetCore.Mvc;
using task4.Dtos;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet("register")]
    public IActionResult RegisterUser(RegisterDto data) 
    {

        return Ok(data);
    }
}