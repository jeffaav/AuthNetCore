using AuthNetCore.Models;
using AuthNetCore.Models.Identity;
using AuthNetCore.Utils.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuthNetCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
            
        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSession();

            services.AddResponseCompression();
            services.AddResponseCaching();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddMvc();
            
            services
                .AddEntityFrameworkSqlite()
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("Identity")));

            services
                .AddIdentity<ApplicationUser, IdentityRole>(options => 
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;

                    options.User.RequireUniqueEmail = true;
                    options.Cookies.ApplicationCookie.LoginPath = "/account/login";
                    options.Cookies.ApplicationCookie.LogoutPath = "/account/logout";
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();


            ConfigureModels(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDeveloperExceptionPage();
            app.UseBrowserLink();

            app.UseSession();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseResponseCaching();
            app.UseResponseCompression();

            app.UseIdentity();


            var facebookConfig = Configuration.GetSection("Facebook");
            app.UseFacebookAuthentication(new FacebookOptions
            {
                AppId = facebookConfig.GetValue<string>("AppId"),
                AppSecret = facebookConfig.GetValue<string>("AppSecret")
            });
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            var userManager = app.ApplicationServices.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = app.ApplicationServices.GetRequiredService<RoleManager<IdentityRole>>();

            app.ApplicationServices.GetRequiredService<ApplicationDbContext>().Seed(userManager, roleManager).Wait();
        }

        #region Models

        public void ConfigureModels(IServiceCollection services)
        {
            services.AddScoped<AccountModel>();
        }

        #endregion
    }
}
