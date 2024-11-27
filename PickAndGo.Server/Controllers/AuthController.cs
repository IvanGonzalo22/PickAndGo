using Microsoft.AspNetCore.Mvc;
using PickAndGo.Server.Models;
using PickAndGo.Server.Services;

namespace PickAndGo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // Endpoint para registrar un cliente
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new
                {
                    Error = new
                    {
                        Code = "InvalidRequest",
                        Message = "Invalid user data. Please check the input and try again."
                    }
                });
            }

            // Validar campos del DTO
            if (string.IsNullOrEmpty(userDto.FirstName) || string.IsNullOrEmpty(userDto.LastName) ||
                string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Phone))
            {
                return BadRequest(new
                {
                    Error = new
                    {
                        Code = "MissingFields",
                        Message = "All fields (First Name, Last Name, Email, Phone) are required."
                    }
                });
            }

            try
            {
                // Asignamos el rol "Customer" por defecto para el cliente
                userDto.Role = "Customer";  // Aquí asignamos el rol de cliente

                var result = await _userService.RegisterAsync(userDto, "Customer");
                if (result.IsSuccess)
                {
                    return Ok(new { Message = "User registered successfully." });
                }

                // Si el registro falla, devolvemos un mensaje de error
                return BadRequest(new
                {
                    Error = new
                    {
                        Code = "RegistrationFailed",
                        Message = result.ErrorMessage
                    }
                });
            }
            catch (Exception ex)
            {
                // Capturamos cualquier excepción no controlada
                return StatusCode(500, new
                {
                    Error = new
                    {
                        Code = "InternalServerError",
                        Message = "An unexpected error occurred during registration.",
                        Details = ex.Message // Detalles del error para el equipo de desarrollo
                    }
                });
            }
        }

        // Endpoint para registrar un empleado
        [HttpPost("create-employee")]
        public async Task<IActionResult> CreateEmployee([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new
                {
                    Error = new
                    {
                        Code = "InvalidRequest",
                        Message = "Invalid user data. Please check the input and try again."
                    }
                });
            }

            // Validar campos del DTO
            if (string.IsNullOrEmpty(userDto.FirstName) || string.IsNullOrEmpty(userDto.LastName) ||
                string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Phone))
            {
                return BadRequest(new
                {
                    Error = new
                    {
                        Code = "MissingFields",
                        Message = "All fields (First Name, Last Name, Email, Phone) are required."
                    }
                });
            }

            try
            {
                // Asignamos el rol "Employee" para el empleado
                userDto.Role = "Employee";  // Aquí asignamos el rol de empleado

                var result = await _userService.RegisterAsync(userDto, "Employee");
                if (result.IsSuccess)
                {
                    return Ok(new { Message = "Employee registered successfully." });
                }

                // Si el registro falla, devolvemos un mensaje de error
                return BadRequest(new
                {
                    Error = new
                    {
                        Code = "RegistrationFailed",
                        Message = result.ErrorMessage
                    }
                });
            }
            catch (Exception ex)
            {
                // Capturamos cualquier excepción no controlada
                return StatusCode(500, new
                {
                    Error = new
                    {
                        Code = "InternalServerError",
                        Message = "An unexpected error occurred during registration.",
                        Details = ex.Message // Detalles del error para el equipo de desarrollo
                    }
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);
            if (result.IsSuccess)
            {
                return Ok(new
                {
                    Message = "Login successful",
                    Token = result.Token,
                    UserName = loginDto.Email // O cualquier otra propiedad que desees devolver
                });
            }
            return Unauthorized(new { Message = result.ErrorMessage });
        }

        // // Endpoint para generar la invitación de empleado
        // [HttpPost("generate-employee-invitation")]
        // public async Task<IActionResult> GenerateEmployeeInvitation([FromBody] EmailDto emailDto)
        // {
        //     if (string.IsNullOrEmpty(emailDto.Email))
        //     {
        //         return BadRequest("El correo electrónico es obligatorio.");
        //     }

        //     try
        //     {
        //         // Usamos el servicio para generar el token
        //         var token = await _employeeInvitationService.GenerateEmployeeInvitationTokenAsync(emailDto.Email);

        //         // Enviar el correo con el token generado
        //         await _employeeInvitationService.SendEmployeeInvitationEmailAsync(emailDto.Email, token);

        //         return Ok(new { Message = "Se ha enviado el enlace de invitación al correo." });
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, new { Error = ex.Message });
        //     }
        // }

    }
}
