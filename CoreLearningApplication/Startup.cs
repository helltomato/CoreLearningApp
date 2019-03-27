using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLearningApplication.Controllers;
using CoreLearningApplication.Helpers;
using CoreLearningApplication.Models;
using CoreLearningApplication.Test;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLearningApplication
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string tariffConnection = Configuration.GetConnectionString("DefaultTariffConnection");
            string userConnection = Configuration.GetConnectionString("DefaultUserConnection");
            services.AddDbContext<UserContext>(options => options.UseSqlServer(userConnection));
            services.AddDbContext<TariffContext>(options => options.UseSqlServer(tariffConnection));
            services.AddTransient<IRepository, Repository>();
            // установка конфигурации подключения
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
        }
        private static void Index(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Index");
            });
        }
    }
}
