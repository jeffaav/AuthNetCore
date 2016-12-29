using Microsoft.EntityFrameworkCore;

namespace AuthNetCore.Models.Database
{
    public class AuthNetCoreContext : DbContext
    {
        public AuthNetCoreContext(DbContextOptions<AuthNetCoreContext> options)
            : base(options)
        {
        }
    }
}
