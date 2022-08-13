using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ECommerce.Shared.Infrastructure.Documentations
{
    internal static class Extensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.CustomSchemaIds(s => s.FullName);
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ECommerce API",
                    Version = "v1"
                });
                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                swagger.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });

            return services;
        }

        public static WebApplication UseSwaggerUserInterface(this WebApplication app)
        {
            app.UseSwaggerUI(swaggerUI =>
            {
                swaggerUI.RoutePrefix = "swagger";
                swaggerUI.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API v1");
                swaggerUI.DocumentTitle = "ECommerce API";
            });

            return app;
        }

        public static WebApplication UseReDocDocumentation(this WebApplication app)
        {
            app.UseReDoc(reDoc =>
            {
                reDoc.RoutePrefix = "docs";
                reDoc.SpecUrl("/swagger/v1/swagger.json");
                reDoc.DocumentTitle = "ECommerce API";
            });

            return app;
        }
    }
}
