using Task4.Models;

namespace task4.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}