using Microsoft.EntityFrameworkCore;
using Npgsql;
using Resend;
using task4.Models;
using task4.Queue.Interfaces;
using task4.Services;
using task4.Services.Interfaces;
using Task4.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection("Email"));
builder.Services.AddTransient<IEmailService, GmailEmailService>();

var dbUrl = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(dbUrl));
    
builder.Services.AddSingleton<IEmailQueue,
    EmailQueue>();

builder.Services.AddHostedService<
    EmailBackgroundService>();


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
app.MapControllers();

app.Run();