using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Utilities.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IWebHostExtensions
    {
        public static async Task<IWebHost> MigrateDbContextAsync<TContext>(
            this IWebHost webHost,
            Func<IServiceProvider, IWebHost, Task> seederAsync
        ) where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetRequiredService<TContext>();

                try
                {
                    logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");

                    var retry = Policy.Handle<SqlException>()
                         .WaitAndRetry(new[]
                         {
                             TimeSpan.FromSeconds(5),
                             TimeSpan.FromSeconds(10),
                             TimeSpan.FromSeconds(15),
                         });

                    await retry.Execute(async () =>
                    {
                        // Migrate the database and invoke seeding
                        await context.Database.MigrateAsync();
                        await seederAsync(services, webHost);
                    });

                    logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
                }
            }

            return webHost;
        }
    }
}
