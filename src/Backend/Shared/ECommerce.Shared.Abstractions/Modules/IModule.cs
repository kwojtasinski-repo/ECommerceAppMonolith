﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Abstractions.Modules
{
    public interface IModule
    {
        string Name { get; }
        string Path { get; }
        IEnumerable<string> Policies => null;
        void Register(IServiceCollection services);
        void Use(IApplicationBuilder app);
    }
}
