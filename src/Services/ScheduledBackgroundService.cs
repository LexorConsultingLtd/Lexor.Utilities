using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.Services
{
    public abstract class ScheduledBackgroundService : ScopedBackgroundService
    {
        private const int ScheduleCheckDelayInMilliseconds = 2000;

        /// <summary>
        /// Implement this method to return a crontab-formatted string.
        /// See https://github.com/atifaziz/NCrontab for format specification
        /// </summary>
        protected abstract string CrontabSchedule { get; }

        private IServiceScopeFactory ServiceScopeFactory { get; }

        protected ScheduledBackgroundService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<ScheduledBackgroundService> logger
        )
            : base(serviceScopeFactory, logger)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                Initialze(scope.ServiceProvider);
            }

            var schedule = NCrontab.CrontabSchedule.Parse(CrontabSchedule);
            var nextRunTime = schedule.GetNextOccurrence(DateTime.Now);

            while (!cancellationToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                if (now > nextRunTime)
                {
                    await ExecuteAsync();
                    nextRunTime = schedule.GetNextOccurrence(now);
                }

                // Add a small processing delay
                await Task.Delay(ScheduleCheckDelayInMilliseconds, cancellationToken);
            }
        }

        /// <summary>
        /// Override to perform any initialization
        /// </summary>
        protected virtual void Initialze(IServiceProvider serviceProvider) { }
    }
}
