using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using task4.Enums;
using Task4.Data;

namespace task4.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSection =
            configuration.GetSection("Jwt");

        var key =
            Encoding.UTF8.GetBytes(
                jwtSection["Key"]!);

        services
            .AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var db = context.HttpContext.RequestServices
                .GetRequiredService<AppDbContext>();

            var userIdClaim =
                context.Principal?.FindFirst(
                    ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                context.Fail("Invalid token");
                return;
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = await db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null ||
                user.IsBlocked == true)
            {
                context.Fail("Account is blocked");
            }
        }
    };
});
        return services;
    }
}