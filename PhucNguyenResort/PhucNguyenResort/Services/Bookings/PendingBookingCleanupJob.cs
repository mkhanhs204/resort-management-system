using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Bookings
{
    public class PendingBookingCleanupJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PendingBookingCleanupJob> _logger;

        public PendingBookingCleanupJob(IServiceProvider serviceProvider, ILogger<PendingBookingCleanupJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Pending Booking Cleanup Background Job is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Pending Booking Cleanup Job running at: {time}", DateTimeOffset.Now);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ResortDbContext>();

                        // Expiry window: 2 minutes (Phù hợp cho lúc Demo)
                        var cutoffTime = DateTime.Now.AddMinutes(-2);

                        var expiredBookings = await context.Bookings
                            .Include(b => b.TourBookings)
                                .ThenInclude(tb => tb.Schedule)
                            .Where(b => b.BookingStatus == BookingStatus.PendingPayment && b.CreatedAt < cutoffTime)
                            .ToListAsync(stoppingToken);

                        if (expiredBookings.Any())
                        {
                            foreach (var booking in expiredBookings)
                            {
                                booking.BookingStatus = BookingStatus.Cancelled;
                                booking.DepositAmount = 0; // Đơn chưa thanh toán mà bị hủy thì tiền cọc thực tế = 0
                                _logger.LogInformation("Booking ID {BookingId} (created at {CreatedAt}) expired and was automatically cancelled.", booking.BookingId, booking.CreatedAt);

                                // Cancel associated tour bookings and restore slots
                                foreach (var tb in booking.TourBookings)
                                {
                                    if (tb.BookingStatus != TourBookingStatus.Cancelled)
                                    {
                                        tb.BookingStatus = TourBookingStatus.Cancelled;
                                        if (tb.Schedule != null && tb.Schedule.AvailableSlots.HasValue)
                                        {
                                            tb.Schedule.AvailableSlots += tb.Participants ?? 0;
                                            _logger.LogInformation("Restored {slots} slots for Tour Schedule {ScheduleId} (Booking {BookingId})", tb.Participants, tb.ScheduleId, booking.BookingId);
                                        }
                                    }
                                }
                            }

                            await context.SaveChangesAsync(stoppingToken);
                            _logger.LogInformation("Successfully cancelled {count} expired pending payment bookings.", expiredBookings.Count);
                        }
                        else
                        {
                            _logger.LogInformation("No expired pending payment bookings found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while running Pending Booking Cleanup Job.");
                }

                // Run once every 2 minutes (Phù hợp cho lúc Demo)
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }

            _logger.LogInformation("Pending Booking Cleanup Background Job is stopping.");
        }
    }
}
