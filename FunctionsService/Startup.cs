﻿using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using AutoMapper;
using System.Reflection;
using AccountServiceApi;

[assembly: FunctionsStartup(typeof(FunctionsServices.Startup))]

namespace FunctionsServices
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).Assembly });
        }
    }
}