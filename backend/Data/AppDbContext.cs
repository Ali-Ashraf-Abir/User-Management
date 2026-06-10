

using Microsoft.EntityFrameworkCore;
using Task4.Models;

namespace Task4.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}