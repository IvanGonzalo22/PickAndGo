namespace PickAndGo.Server.Models
{
    public class UserDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Password { get; set; }

        // El rol será asignado en el backend según el tipo de usuario (Empleado o Cliente)
        public string? Role { get; set; } // Sin valor predeterminado
    }
}