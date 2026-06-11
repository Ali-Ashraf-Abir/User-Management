using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

                        ValidIssuer =
                            jwtSection["Issuer"],

                        ValidAudience =
                            jwtSection["Audience"],

                        IssuerSigningKey =
                            new SymmetricSecurityKey(key)
                    };
            });

        return services;
    }
}