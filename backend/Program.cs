using Microsoft.EntityFrameworkCore;
using Npgsql;
using Task4.Data;

var builder = WebApplication.CreateBuilder(args);

var dbUrl = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(dbUrl));

using var connection = new NpgsqlConnection(dbUrl);

try
{
    connection.Open();
    Console.WriteLine("Connected to PostgreSQL");
}
catch (Exception ex)
{
    Console.WriteLine($"Connection failed: {ex.Message}");
}

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();