using Microsoft.EntityFrameworkCore;
using Npgsql;
using Resend;
using task4.Models;
using task4.Queue.Interfaces;
using task4.Services;
using task4.Services.Interfaces;
using Task4.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using task4.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection("Email"));
builder.Services.AddTransient<IEmailService, GmailEmailService>();


builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(
        JwtOptions.SectionName));

var dbUrl = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(dbUrl));
    
builder.Services.AddSingleton<IEmailQueue,
    EmailQueue>();

builder.Services.AddHostedService<
    EmailBackgroundService>();

builder.Services.AddJwtAuthentication(
    builder.Configuration);

builder.Services.AddTransient<
    IJwtService,
    JwtService>();
    
builder.Services.Configure<BrevoOptions>(
    builder.Configuration.GetSection("Brevo"));

builder.Services.AddHttpClient<IEmailService, BrevoEmailService>();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

using var connection = new NpgsqlConnection(dbUrl);

// try
// {
//     connection.Open();
//     Console.WriteLine("Connected to PostgreSQL");
// }
// catch (Exception ex)
// {
//     Console.WriteLine($"Connection failed: {ex.Message}");
// }

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();