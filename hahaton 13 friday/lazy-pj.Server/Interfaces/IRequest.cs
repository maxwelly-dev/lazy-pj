using lazy_pj.Server.Models;

namespace lazy_pj.Server.Interfaces
{
    public interface IRequest
    {
        Task<AdminRequestModel> CreateRequestAsync(UserModel user, string Description);
        Task<bool> AcceptRequestAsync(int id);
        Task<bool> DenieRequestAsync(int  id);
    }
}
