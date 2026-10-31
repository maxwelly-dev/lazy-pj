using Azure.Core;
using lazy_pj.Server.Data;
using lazy_pj.Server.Interfaces;
using lazy_pj.Server.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Identity.Client;

namespace lazy_pj.Server.Services
{
    public class RequestService : IRequest
    {
        private readonly AppDbContext _context;

        public RequestService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<AdminRequestModel> CreateRequestAsync(UserModel user, string description) 
        {
            AdminRequestModel request = new AdminRequestModel {
             UserId = user.Id,
             UserEmail = user.Email,
             CreationTime = DateTime.Now,
             Description = description};
            return (request);
        }
        public async Task<bool> AcceptRequestAsync(int id) 
        {
            var currentRequest = await _context.Requests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            //ПРОВЕРКУ ОТ ШКОЛЬНИКОВ СЮДА!!!
            int Approved = 2;
            return (true);

        }
        public async Task<bool> DenieRequestAsync(int id)
        {
            var currentRequest = await _context.Requests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            //ПРОВЕРКУ ОТ ШКОЛЬНИКОВ СЮДА!!!
            int Approved = 0;
            return (true);
        }
    }
}
