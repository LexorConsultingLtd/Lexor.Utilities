using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Utilities.Services
{
    /// <inheritdoc />
    /// <summary>
    /// This class can be used to easily consume non-singleton services and execute them within a new per-execution scope.
    /// Derived classes should implement <see cref="M:Utilities.ScopedBackgroundService.ExecuteInScopeAsync(System.IServiceProvider)" /> and create/use services as required.
    /// </summary>
    public abstract class ScopedBackgroundService : BackgroundService
    {
        private IServiceScopeFactory ServiceScopeFactory { get; }
        private ILogger<ScopedBackgroundService> Logger { get; }

        protected ScopedBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<ScopedBackgroundService> logger)
        {
            ServiceScopeFactory = serviceScopeFactory;
            Logger = logger;
        }

        protected async Task ExecuteAsync()
        {
            try
            {
                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    await ExecuteInScopeAsync(scope.ServiceProvider);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"An exception occurred in {GetType().Name}. Processing will resume on next iteration.");
            }
        }

        protected abstract Task ExecuteInScopeAsync(IServiceProvider serviceProvider);
    }
}
