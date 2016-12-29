using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AuthNetCore.Models.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().ForSqliteToTable("User");
            builder.Entity<IdentityUserClaim<string>>().ForSqliteToTable("UserClaim");
            builder.Entity<IdentityUserLogin<string>>().ForSqliteToTable("UserLogin");
            builder.Entity<IdentityUserRole<string>>().ForSqliteToTable("UserRole");
            builder.Entity<IdentityUserToken<string>>().ForSqliteToTable("UserToken");
            builder.Entity<IdentityRole>().ForSqliteToTable("Role");
            builder.Entity<IdentityRoleClaim<string>>().ForSqliteToTable("RoleClaim");

            base.OnModelCreating(builder);
        }

        public async Task Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var userStore = new UserStore<ApplicationUser>(this);
            var roleStore = new RoleStore<IdentityRole>(this);


            if ((await roleManager.FindByNameAsync("Admin")) is null)
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if ((await roleManager.FindByNameAsync("User")) is null)
                await roleManager.CreateAsync(new IdentityRole("User"));

            var user = await userManager.FindByNameAsync("Admin");
            if (user is null)
            {
                user = new ApplicationUser
                {
                    Email = "jeff.aav@gmail.com",
                    UserName = "Admin"
                };

                var resut = await userManager.CreateAsync(user, "Admin");
            }

            if (!(await userManager.IsInRoleAsync(user, "Admin")))
                await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}
