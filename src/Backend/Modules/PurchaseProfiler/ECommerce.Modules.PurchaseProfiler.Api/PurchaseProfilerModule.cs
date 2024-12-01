﻿using ECommerce.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.PurchaseProfiler.Api
{
    public class PurchaseProfilerModule : IModule
    {
        public const string BasePath = "purchase-profiler-module";
        public string Name => "PurchaseProfiler";

        public string Path => BasePath;

        public IEnumerable<string> Policies { get; } = [];

        public void Register(IServiceCollection services)
        {
        }

        public void Use(IApplicationBuilder app)
        {
        }
    }
}