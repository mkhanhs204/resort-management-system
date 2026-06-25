using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhucNguyenResort.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Email
{
    public class OtpCleanupJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OtpCleanupJob> _logger;

        public OtpCleanupJob(IServiceProvider serviceProvider, ILogger<OtpCleanupJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OTP Cleanup Background Job is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("OTP Cleanup running at: {time}", DateTimeOffset.Now);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ResortDbContext>();

                        // Delete OTPs that have expired more than 24 hours ago, or are used and created more than 24 hours ago
                        var cutoffTime = DateTime.Now.AddDays(-1);
                        var expiredOtps = await context.EmailOtps
                            .Where(o => o.ExpiryTime < cutoffTime || (o.IsUsed && o.CreatedAt < cutoffTime))
                            .ToListAsync(stoppingToken);

                        if (expiredOtps.Any())
                        {
                            context.EmailOtps.RemoveRange(expiredOtps);
                            int deletedCount = await context.SaveChangesAsync(stoppingToken);
                            _logger.LogInformation("Successfully deleted {count} expired or used OTP records.", deletedCount);
                        }
                        else
                        {
                            _logger.LogInformation("No expired or used OTP records found to clean up.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while cleaning up OTP data.");
                }

                // Run once every 24 hours
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }

            _logger.LogInformation("OTP Cleanup Background Job is stopping.");
        }
    }
}
