using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using AutoMapper;
using System.Reflection;

[assembly: FunctionsStartup(typeof(FunctionsService.Startup))]

namespace FunctionsService
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).Assembly });
        }
    }
}