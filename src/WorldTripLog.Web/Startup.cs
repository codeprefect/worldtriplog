using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using WorldTripLog.Data.Interfaces;
using WorldTripLog.Data.Repositories;
using WorldTripLog.Web.Models;
using WorldTripLog.Web.Data;
using WorldTripLog.Web.Services;
using WorldTripLog.Domain.Entities;
using System.Net;
using Newtonsoft.Json;

namespace WorldTripLog.Web
{
    public class Startup
    {
        private IConfiguration _configuration;

        public IConfiguration Configuration
        {
            get => _configuration;
        }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WorldTripDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<WorldTripUser, IdentityRole>()
                .AddEntityFrameworkStores<WorldTripDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Identity.Application";
            })
                .ConfigureJwtAuth(_configuration);

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "AuthCookie";
                options.Cookie.Path = "/";
                options.Cookie.HttpOnly = false;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.SlidingExpiration = true;
                options.Events.OnRedirectToLogin = async ctx =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        //await ctx.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorMessage(ctx.Response.StatusCode, "unauthorized request")));
                    }
                    else
                    {
                        ctx.Response.Redirect(ctx.RedirectUri);
                    }

                    await Task.Yield();
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Authenticated", policy =>
                {
                    policy.AddAuthenticationSchemes("Identity.Application", "JwtBearer")
                        .RequireAuthenticatedUser()
                        .Build();
                });
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IDataService<,>), typeof(DataService<,>));
            services.AddTransient<GeoCoordsService>();

            services.AddMvc();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Info { Title = "WorldTripLog API", Version = "v1" });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "WorldTripLogApi.xml");
                config.IncludeXmlComments(xmlPath);
            });
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

            app.UseSwagger();

            app.UseSwaggerUI(options => options.SwaggerEndpoint(_configuration["Swagger:Url"], "WorldTripLog API v1"));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
