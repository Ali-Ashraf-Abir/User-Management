
using Microsoft.EntityFrameworkCore;
using task4.Dtos;
using task4.Enums;
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
    public async Task<List<UserDto>> GetUsers()
    {
        return await db.Users
            .OrderByDescending(u => u.LastLoginTime)
            .Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                IsBlocked = u.IsBlocked,
                AccountStatus = u.AccountStatus,
                LastLoginTime = u.LastLoginTime
            })
            .ToListAsync();
    }

    public async Task BlockUsers(List<int> userIds)
    {
        var users = await db.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();

        foreach (var user in users)
        {
            user.IsBlocked = true;
        }

        await db.SaveChangesAsync();
    }

    public async Task UnblockUsers(
    List<int> userIds)
    {
        var users = await db.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();

        foreach (var user in users)
        {
            user.IsBlocked = false;
        }
        await db.SaveChangesAsync();
    }

    public async Task DeleteUsers(
    List<int> userIds)
    {
        var users = await db.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();

        db.Users.RemoveRange(users);

        await db.SaveChangesAsync();
    }

    public async Task DeleteUnverifiedUsers()
    {
        var users = await db.Users
            .Where(u =>
                u.AccountStatus ==
                AccountStatus.Unverified)
            .ToListAsync();

        db.Users.RemoveRange(users);

        await db.SaveChangesAsync();
    }

}