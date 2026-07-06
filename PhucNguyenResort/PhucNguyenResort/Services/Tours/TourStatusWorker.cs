using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhucNguyenResort.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Tours
{
    public class TourStatusWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TourStatusWorker> _logger;

        public TourStatusWorker(IServiceProvider serviceProvider, ILogger<TourStatusWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Tránh nghẽn tiến trình khởi động chính của Web Server
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ResortDbContext>();

                        // --- HÀNH ĐỘNG 1: CẬP NHẬT TRẠNG THÁI TOURBOOKINGS (CHI TIẾT KHÁCH HÀNG) ---
                        // Nghiệp vụ: Nếu Tour đã hết giờ, tự động chuyển các khách ở trạng thái 'CheckedIn' sang 'Completed'
                        // Điều kiện: Đến giờ kết thúc (EndTime) của ngày hôm nay, hoặc ngày vận hành đã trôi qua (TourDate < Hôm nay)
                        await context.Database.ExecuteSqlRawAsync(@"
                            UPDATE TourBookings
                            SET BookingStatus = 'Completed'
                            FROM TourBookings b
                            INNER JOIN TourSchedules s ON b.ScheduleId = s.ScheduleId
                            INNER JOIN Tours t ON s.TourId = t.TourId
                            WHERE b.BookingStatus = 'CheckedIn'
                            AND (
                                s.TourDate < CAST(GETDATE() AS DATE) 
                                OR (s.TourDate = CAST(GETDATE() AS DATE) AND DATEADD(hour, ISNULL(t.DurationHours, 0), s.StartTime) <= CAST(GETDATE() AS TIME))
                            )
                        ");

                        // --- HÀNH ĐỘNG 2: CẬP NHẬT TRẠNG THÁI TOURSCHEULES (LỊCH TRÌNH VẬN HÀNH TỔNG) ---
                        // Bỏ qua vì TourSchedules không có cột Status trong Database (Status là NotMapped)
                        // await context.Database.ExecuteSqlRawAsync(@""
                        //     UPDATE TourSchedules
                        //     SET Status = 'Completed'
                        //     WHERE Status != 'Completed' AND Status != 'Cancelled'
                        //     AND (
                        //         TourDate < CAST(GETDATE() AS DATE) 
                        //         OR (TourDate = CAST(GETDATE() AS DATE) AND EndTime <= CAST(GETDATE() AS TIME))
                        //     )
                        // "");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[TourStatusWorker Error]: {ex.Message}");
                }

                try
                {
                    // Chờ 1 phút quét lại hệ thống 1 lần để đảm bảo độ chính xác theo thời gian thực
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // Khi ứng dụng dừng (Stop), Token kích hoạt hủy bỏ, ngắt vòng lặp êm đẹp mà không gây crash app
                    break;
                }
            }
        }
    }
}