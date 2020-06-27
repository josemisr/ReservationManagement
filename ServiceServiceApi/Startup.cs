using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ServiceServiceApi.IServicesApi;
using ServiceServiceApi.ServicesApi;

namespace ServiceServiceApi
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer("Key", options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters()
           {
               ValidateIssuer = true,
               ValidateLifetime = true,
               ValidateAudience = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = Configuration["JWT:Issuer"],
               ValidAudience = Configuration["JWT:Audience"],
               IssuerSigningKey = new SymmetricSecurityKey(
                   System.Text.Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"])
               )
           };
       });
            services.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).Assembly });
            services.AddTransient(typeof(IServiceService), typeof(ServiceService));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
