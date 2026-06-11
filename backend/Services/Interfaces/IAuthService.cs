namespace task4.Services.Interfaces;
using task4.Dtos;

public interface IAuthService
{
    Task<UserResponseDto> RegisterUser(RegisterDto data);
    Task<LoginResponseDto> LoginUser(LoginDto data);
}