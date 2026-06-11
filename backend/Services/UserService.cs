
using task4.Dtos;
using task4.Services.Interfaces;
using Task4.Data;
using Task4.Models;

namespace task4.Services;

public class UserService(AppDbContext db) : IUserService
{
    public async Task<UserResponseDto> VerifyEmail(string token)
    {
        var user = db.Users.FirstOrDefault(u => u.VerificationToken == token);

        if (user == null)
        {
            throw new Exception("Invalid token");
        }

        user.AccountStatus =
            Enums.AccountStatus.Verified;

        user.VerificationToken = null;

        await db.SaveChangesAsync();

        return new UserResponseDto
        {
            Email = user.Email,
            Id = user.Id,
            FullName = user.FullName
        };
    }
}