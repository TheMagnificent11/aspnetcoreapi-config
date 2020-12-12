using System.Reflection;
using AspNetCoreApi.Infrastructure.Exceptions;
using AspNetCoreApi.Infrastructure.Logging;
using AspNetCoreApi.Infrastructure.Mediation;
using AspNetCoreApi.Infrastructure.ProblemDetails;
using AspNetCoreApi.Infrastructure.Settings;
using AspNetCoreApi.Infrastructure.Swagger;
using Autofac;
using Autofac.Features.Variance;
using AutofacSerilogIntegration;
using AutoMapper;
using EntityManagement;
using FluentValidation.AspNetCore;
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

        private readonly string[] apiVersions = { ApiVersion };

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterLogger();
            builder.RegisterSource(new ContravariantRegistrationSource());
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

            app.ConfigureSwagger(ApiName, this.apiVersions);

            app.MigrationDatabase<DatabaseContext>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = this.configuration.GetSettings<ApplicationSettings>("ApplicationSettings");
            var seqSettings = this.configuration.GetSettings<SeqSettings>("SeqSettings");

            services.ConfigureLogging(appSettings, seqSettings);

            services.ConfigureDatabaseContextAndFactory<DatabaseContext>(
                this.configuration.GetConnectionString("DefaultConnection"));

            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddControllers(options => options.Filters.Add(new ExceptionFilter()))
                .AddFluentValidation(options => options.RegisterValidatorsFromAssembly(typeof(Startup).Assembly));

            services.ConfigureProblemDetails();

            services.ConfigureSwagger(ApiName, this.apiVersions);

            services.AddHealthChecks()
                .AddDbContextCheck<DatabaseContext>();
        }
    }
}