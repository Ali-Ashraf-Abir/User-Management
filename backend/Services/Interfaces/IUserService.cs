using task4.Dtos;
using Task4.Models;

namespace task4.Services.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> VerifyEmail(string token);
    Task<List<UserDto>> GetUsers();
    Task BlockUsers(List<int> userIds);
    Task UnblockUsers(List<int> userIds);
    Task DeleteUsers(List<int> userIds);
    Task DeleteUnverifiedUsers();
}