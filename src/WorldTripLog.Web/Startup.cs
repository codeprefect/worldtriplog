using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorldTripLog.Web.DAL;
using WorldTripLog.Web.Data;
using WorldTripLog.Web.Models;

namespace WorldTripLog.Web
{
    public class Startup
    {
        public IConfiguration _configuartion;

        public Startup(IConfiguration configuration)
        {
            _configuartion = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WorldTripDbContext>(options =>
            {
                options.UseSqlServer(_configuartion.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<WorldTripUser, IdentityRole>()
                .AddEntityFrameworkStores<WorldTripDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IRepository, Repository<WorldTripDbContext>>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
