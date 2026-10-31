using lazy_pj.Server.Data;
using lazy_pj.Server.Interfaces;
using lazy_pj.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace lazy_pj.Server.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserModel>> GetAllUsersAsync() { return await _context.Users.ToListAsync(); }
        public async Task<List<AdminModel>> GetAllAdminsAsync() { return await _context.Admins.ToListAsync(); }
        public async Task<UserModel?> GetUserAsync(int id) { return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);  }
        public async Task<AdminModel?> GetAdminAsync(int id) { return await _context.Admins.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);  }
        public async Task<bool> SetUserAdminAsync(int id)
        {
            var user = await GetUserAsync(id);
            if (user is null)
            {
                throw new InvalidOperationException("User not found!");
            }

            AdminModel admin = new AdminModel
            {
                Email = user.Email,
                Password = user.Password,
                Name = user.Name,
            };

            _context.Users.Remove(user);
            _context.Admins.Add(admin);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RemoveUserAdminAsync(int id)
        {
            var admin = await GetAdminAsync(id);
            if (admin is null)
            {
                throw new InvalidOperationException("admin not found!");
            }

            UserModel user = new UserModel
            {
                Email = admin.Email,
                Password = admin.Password,
                Name = admin.Name,
            };

            _context.Admins.Remove(admin);
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<string> GetUserRoleAsync(int id)
        {
            var admin = await GetAdminAsync(id);       
            var user = await GetUserAsync(id);
            if (user is null && admin is null)
            {
                throw new InvalidOperationException("User not found!");
            }
            if (admin is null) return "user";
            if (user is null) return "admin";
            return string.Empty;
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
    }
}
