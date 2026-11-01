using lazy_pj.Server.Interfaces;
using lazy_pj.Server.Models;
using lazy_pj.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace lazy_pj.Server.Services
{
    public class AuthService : IAuth
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> RegisterUserAsync(UserModel user)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Email == user.Email || u.Name == user.Name);

            if (userExists)
            {
                throw new InvalidOperationException("User already exists!");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string?> LoginUserAsync(UserModel user)
        {
            var userFromDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (userFromDb == null || userFromDb.Password != user.Password)
            {
                throw new InvalidOperationException("Wrong Email or password!");
            }

            return GenerateJwtToken(userFromDb);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var userFromDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (userFromDb == null)
            {
                throw new InvalidOperationException("User not found!");
            }

            _context.Users.Remove(userFromDb);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditUserAsync(UserModel user)
        {
            var userFromDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (userFromDb == null)
            {
                throw new InvalidOperationException("User doesnt exist!");
            }

            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                throw new InvalidOperationException("Email or password cannot be empty!");
            }

            userFromDb.Name = user.Name;
            userFromDb.Email = user.Email;
            userFromDb.Password = user.Password;

            await _context.SaveChangesAsync();
            return true;
        }

        private string GenerateJwtToken(UserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name ?? string.Empty)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}