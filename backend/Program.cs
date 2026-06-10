using Microsoft.EntityFrameworkCore;
using Task4.Data;

var builder = WebApplication.CreateBuilder(args);
var dbUrl = builder.Configuration.GetConnectionString("DefaultConnection");
var services = builder.Services;
services.AddDbContext<AppDbContext>(options =>options.UseNpgsql(dbUrl));

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
Console.WriteLine(dbUrl);
app.Run();
