namespace task4.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using task4.Dtos;
using task4.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
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
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await userService.GetUsers();

        return Ok(users);
    }

    [Authorize]
    [HttpPost("block")]
    public async Task<IActionResult> BlockUsers(
    UserIdsDto dto)
    {
        await userService.BlockUsers(dto.UserIds);

        return Ok(new
        {
            Message = "Users blocked successfully"
        });
    }
    [Authorize]
    [HttpPost("unblock")]
    public async Task<IActionResult> UnblockUsers(
        UserIdsDto dto)
    {
        await userService.UnblockUsers(
            dto.UserIds);

        return Ok(new
        {
            Message = "Users unblocked successfully"
        });
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteUsers(
    [FromBody] UserIdsDto dto)
    {
        await userService.DeleteUsers(
            dto.UserIds);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("unverified")]
    public async Task<IActionResult>
    DeleteUnverifiedUsers()
    {
        await userService
            .DeleteUnverifiedUsers();

        return NoContent();
    }
}
