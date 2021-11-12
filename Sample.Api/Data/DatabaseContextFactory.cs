using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sample.Data;

namespace Sample.Api.Data;

public sealed class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    private readonly IConfiguration configuration;

    public DatabaseContextFactory()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        this.configuration = builder.Build();
    }

    public DatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection"));

        return new DatabaseContext(optionsBuilder.Options);
    }
}
