using System.Reflection;
using AspNetCoreApi.Infrastructure.Exceptions;
using AspNetCoreApi.Infrastructure.HealthChecks;
using AspNetCoreApi.Infrastructure.Mediation;
using AspNetCoreApi.Infrastructure.ProblemDetails;
using AspNetCoreApi.Infrastructure.Swagger;
using Autofac;
using Autofac.Features.Variance;
using AutoMapper;
using EntityManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleApiWebApp.Data;

namespace SampleApiWebApp
{
    public class Startup
    {
        private const string ApiName = "Sample API";
        private const string ApiVersion = "v1";

        private readonly string[] ApiVersions = { ApiVersion };

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());

            builder.RegisterModule(new EntityManagementModule<DatabaseContext>());
            builder.RegisterModule(new MediationModule(new Assembly[] { typeof(Startup).Assembly }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // TOOD: fix this
                // endpoints.MapHealthChecks("/health", this.configuration.GetHealthCheckOptions("ApplicationSettings"));
                endpoints.MapControllers();
            });

            app.ConfigureSwagger(ApiName, this.ApiVersions);

            app.MigrationDatabase<DatabaseContext>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<DatabaseContext>(options =>
                options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddControllers(options => options.Filters.Add(new ExceptionFilter()));

            services.ConfigureProblemDetails();

            services.ConfigureSwagger(ApiVersion, this.ApiVersions);
        }
    }
}