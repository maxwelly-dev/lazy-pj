using lazy_pj.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace lazy_pj.Server.Interfaces
{
    public interface IAuth
    {
        public Task<bool> RegisterUserAsync(UserModel user);
        public Task<bool> LoginUserAsync(UserModel user);
        public Task<bool> DeleteUserAsync(int Id);
        public Task<bool> EditUserAsync(UserModel user);
        
    }
}
