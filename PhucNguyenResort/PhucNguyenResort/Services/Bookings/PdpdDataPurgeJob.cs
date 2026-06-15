using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Bookings
{
    public class PdpdDataPurgeJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PdpdDataPurgeJob> _logger;

        public PdpdDataPurgeJob(IServiceProvider serviceProvider, ILogger<PdpdDataPurgeJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PDPD Data Purge Background Job is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("PDPD Data Purge running at: {time}", DateTimeOffset.Now);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var guestService = scope.ServiceProvider.GetRequiredService<IBookingGuestService>();
                        int purgedCount = await guestService.PurgeExpiredPiiDataAsync();
                        if (purgedCount > 0)
                        {
                            _logger.LogInformation("Successfully purged PII data for {count} expired bookings.", purgedCount);
                        }
                        else
                        {
                            _logger.LogInformation("No expired PII data found to purge.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while purging PDPD data.");
                }

                // Run once every 24 hours
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }

            _logger.LogInformation("PDPD Data Purge Background Job is stopping.");
        }
    }
}
