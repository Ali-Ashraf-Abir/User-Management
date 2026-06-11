namespace task4.Services;

using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using task4.Dtos;
using task4.Job;
using task4.Queue.Interfaces;
using task4.Services.Interfaces;
using Task4.Data;
using Task4.Models;

public class AuthService(AppDbContext db, IEmailQueue emailQueue) : IAuthService
{
    public async Task<UserResponseDto> RegisterUser(RegisterDto data)
    {

        var user = new User
        {
            FullName = data.FullName,
            Email = data.Email,
            AccountStatus = Enums.AccountStatus.Unverified

        };

        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, data.Password);

        var verificationToken =
            Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        user.VerificationToken = verificationToken;
        var verificationLink =
            $"https://localhost:5001/api/auth/verify?token={verificationToken}";
        try
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException pg &&
                   pg.ConstraintName == "IX_Users_Email")
        {
            throw new Exception("Email already exists");
        }

        var body = $"""
                <h1>Welcome to Task4</h1>

                <p>Thank you for registering.</p>

                <p>Please verify your email by clicking the link below:</p>

                <a href="{verificationLink}">
                    Verify My Email
                </a>
            """;

        await emailQueue.QueueEmailAsync(
            new EmailMessage
            {
                To = user.Email,
                Subject = "Verify Email",
                Body = body
            });
        return new UserResponseDto
        {
            Email = user.Email,
            Id = user.Id,
            FullName = user.FullName
        };
    }

    public Task<LoginDto> LoginUser(LoginDto data)
    {
        return Task.FromResult(data);
    }
}