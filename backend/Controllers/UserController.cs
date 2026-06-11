namespace task4.Controllers;

using Microsoft.AspNetCore.Mvc;
using task4.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("verify/{token}")]
    public async Task<IActionResult> VerifyMail(string token)
    {


        try
        {
            var result = await userService.VerifyEmail(token);
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
