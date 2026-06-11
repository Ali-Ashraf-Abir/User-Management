using task4.Dtos;
using Task4.Models;

namespace task4.Services.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> VerifyEmail(string token);
}