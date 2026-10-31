using lazy_pj.Server.Models;

namespace lazy_pj.Server.Interfaces
{
    public interface IAdminService
    {
        public Task<List<UserModel>> GetAllUsersAsync();
        public Task<List<AdminModel>> GetAllAdminsAsync();
        public Task<UserModel?> GetUserAsync(int id);
        public Task<AdminModel?> GetAdminAsync(int id);
        public Task<bool> SetUserAdminAsync(int id);
        public Task<bool> RemoveUserAdminAsync(int id);
        public Task<string> GetUserRoleAsync(int id);
        public Task<bool> DeleteUserAsync(int id);
    }
}
