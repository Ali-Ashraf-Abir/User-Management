using task4.Dtos;

interface IAuthService
{
    Task<RegisterDto> RegiterUser();
    Task<LoginDto> LoginUser();
}