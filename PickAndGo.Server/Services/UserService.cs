using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using PickAndGo.Server.Models;
using System.Text;
using PickAndGo.Server.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Identity.Client;

namespace PickAndGo.Server.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> RegisterAsync(UserDto userDto, string role = "Customer")
        {
            // Asignar el rol proporcionado, por defecto "Customer"
            userDto.Role = role;

            // Verificar si el correo electrónico ya está registrado
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (existingUser != null)
                return (false, "Email is already taken.");

            // Generar el hash y el salt de la contraseña
            var (passwordHash, salt) = HashPassword(userDto.Password);

            // Crear un nuevo usuario
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Phone = userDto.Phone,
                PasswordHash = passwordHash,
                PasswordSalt = salt, // Almacenar el salt junto al hash
                Role = userDto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (true, string.Empty);
        }

        public async Task<(bool IsSuccess, string Token, string ErrorMessage)> LoginAsync(LoginDto loginDto)
        {
            // Search for the user by email in db and its contained in user value
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            // Check if the user exists and if the provided password matches the stored hash using VerifyPassword function
            if (user != null && VerifyPassword(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                // If the credentials are valid, generate a JWT token for authentication using GenerateJwtToken function and return it
                return (true, GenerateJwtToken(user), string.Empty);
            }

            return (false, string.Empty, "Invalid email or password.");
        }

        private string GenerateJwtToken(User user)
        {
            // claims (payload) is the variable which will contain the db specified user information to be handled in frontend
            var claims = new[]
            {
                // That specified user info is the email to welcome the user
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.Email),
                // and the role to handle what is the content that each user must see depending on it
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.Role)
                
                // We could also add more db fields if it's required by frontend
            };

            // JWT Token Signing Configuration (signature) to ensure the token won't be maliciosly altered after being signed
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Token creation 
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token expiration time
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private (string Hash, string Salt) HashPassword(string password)
        {
            // Generate unique salt
            var saltBytes = new byte[16];
            new Random().NextBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);

            // Generate hash using salt
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return (hash, salt);
        }

        // Function that verifies if generated hash and stored hash are equal
        private bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            // Convert stored salt to bytes
            var saltBytes = Convert.FromBase64String(storedSalt);

            // Generate new hash with the same salt
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // and return TRUE if generated hash and stored hash are equal
            return hash == storedHash;
        }
    }
}
