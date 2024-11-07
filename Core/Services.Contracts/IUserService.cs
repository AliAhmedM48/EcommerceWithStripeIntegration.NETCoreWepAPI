using Core.Dtos.Identity;

namespace Core.Services.Contracts;
public interface IUserService
{
    Task<UserDto> LoginAsync(LoginDto loginDto);
    Task<UserDto> RegisterAsync(RegisterDto registerDto);
    Task<bool> CheckEmailExistAsync(string email);
}
