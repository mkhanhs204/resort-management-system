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

                        // Chạy lệnh SQL cập nhật trạng thái tự động
                        // Loại bỏ stoppingToken tại đây để tránh ném ngoại lệ TaskCanceledException khi DB chưa phản hồi kịp lúc shutdown
                        await context.Database.ExecuteSqlRawAsync(@"
                            UPDATE TourSchedules
                            SET Status = 'Completed'
                            WHERE Status != 'Completed' AND Status != 'Cancelled'
                            AND (
                                TourDate < CAST(GETDATE() AS DATE) 
                                OR (TourDate = CAST(GETDATE() AS DATE) AND DATEADD(minute, 180, StartTime) <= CAST(GETDATE() AS TIME))
                            )
                        ");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[TourStatusWorker Error]: {ex.Message}");
                }

                try
                {
                    // Chờ 2 phút trước khi quét lại, bọc trong try-catch để bắt gọn sự kiện app tắt (Shutdown/Cancellation)
                    await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
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