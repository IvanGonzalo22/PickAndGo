using PickAndGo.Server.Models;

public interface IUserService
{
    // Modificar la firma para incluir el parámetro 'role'
    Task<(bool IsSuccess, string ErrorMessage)> RegisterAsync(UserDto userDto, string role);
    Task<(bool IsSuccess, string Token, string ErrorMessage)> LoginAsync(LoginDto loginDto);
}