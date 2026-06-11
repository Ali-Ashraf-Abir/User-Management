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

public class AuthService(AppDbContext db, IEmailQueue emailQueue, IJwtService jwtService, IConfiguration configuration) : IAuthService
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
        var apiUrl = configuration["Connection:Api"];
        var verificationLink =
            $"{apiUrl}/user/verify/{verificationToken}";
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

                <p>Please verify your email by copying the link and pasting in a browser</p>


                <p>{verificationLink}</p>
                
            """;
        Console.WriteLine($"Queueing email for {user.Email}");
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



    public async Task<LoginResponseDto> LoginUser(
        LoginDto data)
    {
        var user = await db.Users
            .FirstOrDefaultAsync(
                u => u.Email == data.Email);
        Console.WriteLine($"Email from request: '{data.Email}'");
        if (user == null)
        {
            throw new Exception(
                "User not found");
        }
        if (user.IsBlocked == true)
        {
            throw new Exception("Account is blocked");
        }
        var passwordHasher =
            new PasswordHasher<User>();

        var result =
            passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                data.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new Exception(
                "Password Failed");
        }

        user.LastLoginTime =
            DateTime.UtcNow;

        await db.SaveChangesAsync();

        var token =
            jwtService.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            Email = user.Email,
            FullName = user.FullName,
            AccountStatus = user.AccountStatus
        };
    }
}