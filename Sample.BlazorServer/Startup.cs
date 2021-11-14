using System.Reflection;
using AspNetCoreApi.Infrastructure.Logging;
using AspNetCoreApi.Infrastructure.Mediation;
using AspNetCoreApi.Infrastructure.Settings;
using Autofac;
using Autofac.Features.Variance;
using AutofacSerilogIntegration;
using EntityManagement;
using Sample.Application.Teams;
using Sample.Data;

namespace Sample.BlazorServer;

public class Startup
{
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public static void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterLogger();
        builder.RegisterSource(new ContravariantRegistrationSource());
        builder.RegisterModule(new MediationModule(new Assembly[] { typeof(TeamMappings).Assembly }));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");

            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var appSettings = this.configuration.GetSettings<ApplicationSettings>("ApplicationSettings");
        var seqSettings = this.configuration.GetSettings<SeqSettings>("SeqSettings");

        services.ConfigureLogging(appSettings, seqSettings);

        services.ConfigureDatabaseContextAndFactory<DatabaseContext>(
            this.configuration.GetConnectionString("DefaultConnection"));

        services.AddAutoMapper(typeof(TeamMappings).Assembly);

        services.AddHealthChecks()
            .AddDbContextCheck<DatabaseContext>();

        services.AddRazorPages();
        services.AddServerSideBlazor();
    }
}
