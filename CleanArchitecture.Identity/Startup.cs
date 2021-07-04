using CleanArchitecture.Identity.Data;
using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace CleanArchitecture.Identity
{
    public class Startup
    {
        private readonly IConfiguration Configurations;

        public Startup(IConfiguration configuration) =>
            Configurations = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configurations.GetValue<string>("DbConnection");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Notes.Identity.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });

            services.AddIdentityServer()
                    .AddAspNetIdentity<AppUser>()
                    .AddInMemoryApiResources(Configuration.ApiResources)
                    .AddInMemoryIdentityResources(Configuration.IdentityResources)
                    .AddInMemoryClients(Configuration.Clients)
                    .AddInMemoryApiScopes(Configuration.ApiScopes)
                    .AddDeveloperSigningCredential();

            services.AddControllersWithViews();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Styles")),
                RequestPath = "/Styles"
            });
            

            app.UseRouting();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
