using Microsoft.EntityFrameworkCore;
using lazy_pj.Server.Models;
namespace lazy_pj.Server.Data
{
    public class AppDbContext : DbContext
    {
       public AppDbContext(DbContextOptions options) : base(options)
        {
        }
       public DbSet<UserModel> Users { get; set; }
       public DbSet<AdminModel> Admins { get; set; }
    }
}
