using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System.Reflection;

[assembly: FunctionsStartup(typeof(FunctionsReservation.Startup))]

namespace FunctionsReservation
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).Assembly });
        }
    }
}