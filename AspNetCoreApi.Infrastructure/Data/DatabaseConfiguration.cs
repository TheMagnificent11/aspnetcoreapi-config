using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreApi.Infrastructure.Data
{
    /// <summary>
    /// Database Configuration
    /// </summary>
    public static class DatabaseConfiguration
    {
        /// <summary>
        /// Migrates the databaes related to the DB context of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">DB context to migrate</typeparam>
        /// <param name="app">Application buider</param>
        public static void MigrationDatabase<T>(this IApplicationBuilder app)
            where T : DbContext
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<T>();
                dbContext.Database.Migrate();
            }
        }
    }
}
