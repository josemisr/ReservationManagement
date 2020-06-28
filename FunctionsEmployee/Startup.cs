
using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System.Reflection;


[assembly: FunctionsStartup(typeof(FunctionsEmployee.Startup))]

namespace FunctionsEmployee
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).Assembly });
        }
    }
}