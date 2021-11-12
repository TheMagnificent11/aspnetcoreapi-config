using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sample.Data.Configuration;
using Sample.Domain;

namespace Sample.Data;

public sealed class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public int AttachedRepositories { get; set; }

    public DbSet<Team> Teams { get; set; }

    public DbSet<Player> Players { get; set; }

    public Task<int> SaveChangesAsync()
    {
        return this.SaveChangesAsync(true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder == null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TeamConfiguration());
        modelBuilder.ApplyConfiguration(new PlayerConfiguration());
    }
}
