using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using System.Reflection;

[assembly: FunctionsStartup(typeof(FunctionsUser.Startup))]

namespace FunctionsUser
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).Assembly });
        }
    }
}