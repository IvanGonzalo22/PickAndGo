using PickAndGo.Server.Models;

namespace PickAndGo.Server.Services
{
    public interface IUserService
    {
        Task<(bool IsSuccess, string ErrorMessage)> RegisterAsync(UserDto userDto);
        Task<(bool IsSuccess, string Token, string ErrorMessage)> LoginAsync(UserDto userDto);
        Task LoginAsync(LoginDto loginDto);
    }
}
