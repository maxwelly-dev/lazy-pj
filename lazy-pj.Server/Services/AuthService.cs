using lazy_pj.Server.Interfaces;
using lazy_pj.Server.Models;
using lazy_pj.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace lazy_pj.Server.Services
{
    public class AuthService : IAuth
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
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

        public async Task<bool> LoginUserAsync(UserModel user)
        {
            var userFromDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (userFromDb == null || userFromDb.Password != user.Password)
            {
                throw new InvalidOperationException("Wrong Email or password!");
            }

            return true;
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
    }
}