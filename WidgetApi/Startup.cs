using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using WidgetApi.EFCore;
using WidgetApi.Swagger;

namespace WidgetApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddFluentValidation();

            services
                .AddMvcCore(o =>
                {
                    // decorate controller to distinguish SwaggerDoc (v1, v2, etc.)
                    o.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
                });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContextPool<WidgetContext>(o =>
            {
                o.UseSqlServer(connectionString,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 2,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null);
                    });
            });

            // register validators
            services.AddTransient<IValidator<V1.DTO.WidgetDTO>, V1.Validators.WidgetDTOValidatorCollection>();
            services.AddTransient<IValidator<V2.DTO.WidgetDTO>, V2.Validators.WidgetDTOValidatorCollection>();

            services.AddSwaggerService();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler(o =>
                {
                    o.Run(async context =>
                    {
                        var ehFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (ehFeature != null)
                        {
                                // TODO: Serilog, ApplicationInsights, etc.
                                //telemetry.TrackException(ehFeature.Error);
                                await Task.CompletedTask.ConfigureAwait(false);
                        }
                    });
                });
            }

                app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerService();
        }
    }
}

