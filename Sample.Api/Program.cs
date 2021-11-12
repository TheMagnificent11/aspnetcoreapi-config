using Autofac.Extensions.DependencyInjection;
using Sample.Api;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
     {
         logging.ClearProviders();
         logging.AddSerilog();
     })
    .UseSerilog()
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureWebHostDefaults(webHostBuilder =>
    {
        webHostBuilder
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>();
    })
    .Build();

host.Run();
