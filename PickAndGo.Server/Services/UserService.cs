using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using PickAndGo.Server.Models;
using System.Text;
using PickAndGo.Server.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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

        public async Task<(bool IsSuccess, string ErrorMessage)> RegisterAsync(UserDto userDto)
        {
            var existingUser = await _context.Customers
                .FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
                return (false, "Email is already taken.");

            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingEmployee != null)
                return (false, "Email is already taken.");

            var passwordHash = HashPassword(userDto.Password);

            if (userDto.Role.ToLower() == "customer")
            {
                var customer = new Customer
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Phone = userDto.Phone,
                    PasswordHash = passwordHash
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
            else if (userDto.Role.ToLower() == "employee")
            {
                var employee = new Employee
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Phone = userDto.Phone,
                    PasswordHash = passwordHash
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
            }
            else
            {
                return (false, "Invalid role.");
            }

            return (true, string.Empty);
        }

        public async Task<(bool IsSuccess, string Token, string ErrorMessage)> LoginAsync(UserDto userDto)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (customer != null && VerifyPassword(userDto.Password, customer.PasswordHash))
            {
                return (true, GenerateJwtToken(customer.Email), string.Empty);
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (employee != null && VerifyPassword(userDto.Password, employee.PasswordHash))
            {
                return (true, GenerateJwtToken(employee.Email), string.Empty);
            }

            return (false, string.Empty, "Invalid email or password.");
        }

        private string GenerateJwtToken(string email)
        {
            var claims = new[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        private string HashPassword(string password)
        {
            var salt = new byte[16];
            new Random().NextBytes(salt);
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hash;
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return storedHash == HashPassword(enteredPassword);
        }

        Task IUserService.LoginAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }
    }
}
