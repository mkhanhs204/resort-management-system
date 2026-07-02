using System.Collections.Generic;
using PhucNguyenResort.Models;

namespace PhucNguyenResort.Web.Models // Bạn hãy đổi namespace cho đúng thư mục của dự án nếu cần
{
    public class TourGuideOverviewViewModel
    {
        // 3 Chỉ số tổng quan cao cấp cấp hệ thống
        public int TotalAssignedTours { get; set; }     // Tổng số tour được phân công tích lũy
        public double CompletionRate { get; set; }      // Tỷ lệ hoàn thành (%)
        public int TotalGuestsServed { get; set; }      // Tổng số hành khách thực tế đã phục vụ thành công

        // Danh sách rút gọn chứa các lịch trình thực tế (Đang chạy hoặc sắp chạy)
        public List<TourSchedule> ActiveSchedules { get; set; } = new List<TourSchedule>();
    }
}