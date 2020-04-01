using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace WidgetApi.Swagger
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var filePath = Path.Combine(System.AppContext.BaseDirectory, xmlFile);
            var serviceTitle = Assembly.GetExecutingAssembly().GetName().Name;
            var serviceDescription = "The description of my service goes here";
            var openApiContact = new OpenApiContact { Name = "My Team Name", Email = "my-team-email@multco.us" };

            var openApiInfoV1 = new OpenApiInfo
            {
                Title = serviceTitle,
                Version = "v1",
                Description = serviceDescription,
                Contact = openApiContact
            };

            var openApiInfoV2 = new OpenApiInfo
            {
                Title = serviceTitle,
                Version = "v2",
                Description = serviceDescription,
                Contact = openApiContact
            };

            services.AddSwaggerGen(c =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Authorization header. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.OpenIdConnect
                };

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securityScheme, Array.Empty<string>() }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(securityRequirement);

                c.SwaggerDoc("v1", openApiInfoV1);
                c.SwaggerDoc("v2", openApiInfoV2);
                c.IncludeXmlComments(filePath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerService(this IApplicationBuilder app)
        {
            var serviceTitle = Assembly.GetExecutingAssembly().GetName().Name;

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", serviceTitle + " v2");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", serviceTitle + " v1");
            });

            return app;
        }
    }
}