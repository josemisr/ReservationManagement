using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReservationManagementApp.Models;
using Microsoft.AspNetCore.Http;
using SolucionMonolitica.Filters;
using ReservationManagementApp.Authorize;
using Microsoft.AspNetCore.Authorization;
using ReservationManagementApp.Enum;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace ReservationManagementApp
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
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddDbContext<ReservationManagementDbContext>();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(VerifySession));
                // options.Filters.Add(typeof(CustomAuthFilter));
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(RolesAuthorize.Admin.ToString(), policy => policy.Requirements.Add(new RolesAuthorizeRequirement((int)RolesAuthorize.Admin)));
                options.AddPolicy(RolesAuthorize.Client.ToString(), policy => policy.Requirements.Add(new RolesAuthorizeRequirement((int)RolesAuthorize.Client)));
            });

            services.AddSingleton<IAuthorizationHandler, RoleAuthorizeHandler>();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(600);
            });
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Global/Error");
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Global/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseSession();//Orden importante, para que pueda ser usada en el Authorization (policy)
            app.UseAuthorization();

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-GB"),
                SupportedCultures = new[] { new CultureInfo("en-GB")},
                SupportedUICultures = new[] {new CultureInfo("en-GB")}
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
