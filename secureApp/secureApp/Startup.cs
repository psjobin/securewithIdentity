using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using secureApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace secureApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            //services.AddDefaultIdentity<IdentityUser>(options => // Adding Identity with out Roles... default identity doesn't do roles it looks like:
            // AddDefaultIdentity uses a different set of apis, but does more or less the same thing except it removes roles functionality by default.
            // Roles is the only real difference between the two, they both return IdentityBuilders which can further customize identity. https://github.com/aspnet/Identity/issues/1884
            //{
            //    options.SignIn.RequireConfirmedAccount = false;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequiredLength = 5;
            //})
            services.AddIdentity<IdentityUser, IdentityRole>(options => // Here we are adding roles
             {
                 options.SignIn.RequireConfirmedAccount = false;
                 options.Password.RequireUppercase = false;
                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequiredLength = 5;
             }).AddDefaultUI() //  This is imp! -->> OTHERWISE the urls don't work properly..  (localhost/account/login vs. localhost/identity/account/login )
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
