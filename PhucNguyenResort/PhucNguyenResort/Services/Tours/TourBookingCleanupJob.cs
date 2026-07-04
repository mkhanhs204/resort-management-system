using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Services.Tours;

public class TourBookingCleanupJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TourBookingCleanupJob> _logger;

    public TourBookingCleanupJob(IServiceProvider serviceProvider, ILogger<TourBookingCleanupJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TourBookingCleanupJob is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupPendingBookingsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing TourBookingCleanupJob.");
            }

            // Chạy mỗi 2 phút
            await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
        }

        _logger.LogInformation("TourBookingCleanupJob is stopping.");
    }

    private async Task CleanupPendingBookingsAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ResortDbContext>();

        var expirationTime = DateTime.Now.AddMinutes(-2);

        // Tìm các booking đang chờ thanh toán (Pending) và đã tạo quá 2 phút
        var expiredBookings = await context.TourBookings
            .Include(tb => tb.Schedule)
            .Where(tb => tb.BookingStatus == TourBookingStatus.Pending && tb.CreatedAt < expirationTime)
            .ToListAsync();

        if (expiredBookings.Any())
        {
            _logger.LogInformation($"Found {expiredBookings.Count} expired pending tour bookings.");

            foreach (var booking in expiredBookings)
            {
                // Cập nhật trạng thái thành Cancelled
                booking.BookingStatus = TourBookingStatus.Cancelled;

                // Hoàn lại số chỗ trống (AvailableSlots)
                if (booking.Schedule != null && booking.Schedule.AvailableSlots.HasValue)
                {
                    booking.Schedule.AvailableSlots += booking.Participants ?? 0;
                }
                _logger.LogInformation($"Auto-cancelled TourBookingId: {booking.TourBookingId} because it was pending for > 2 minutes.");
            }

            await context.SaveChangesAsync();
        }
    }
}
