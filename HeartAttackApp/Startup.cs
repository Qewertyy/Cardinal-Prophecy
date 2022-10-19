using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security;
using CardinalPServices.Interfaces;
using CardinalPServices.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HeartAttackApp.Filters;

namespace HeartAttackApp
{

    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMvc().AddRazorRuntimeCompilation();
            services.AddMvcCore().AddDataAnnotations();
            services.AddSingleton<IUserProfile, UserProfileService>();
            services.AddSingleton<IPatient, PatientService>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);//We set Time here 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSession();

            //app.UseMvcWithDefaultRoute();
            app.UseRouting();
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Welcome to Dotnet Core!");
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Account}/{action=Login}");

            });
        }
    }
}
