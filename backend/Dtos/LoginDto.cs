using Microsoft.AspNetCore.Identity;

namespace task4.Dtos;
public class LoginDto
{
    required public string Email;
    required public string Password;
}