using System;
using System.Collections.Generic;

namespace PhucNguyenResort.ViewModels.Dashboard
{
    public class ForecastResultViewModel
    {
        public int ForecastMonth { get; set; }       // Tháng dự báo
        public int ForecastYear { get; set; }        // Năm dự báo
    
        // Dự báo doanh thu
        public decimal PredictedTotalRevenue { get; set; }
        public decimal PredictedRoomRevenue { get; set; }
        public decimal PredictedFnBRevenue { get; set; }
        public decimal PredictedTourRevenue { get; set; }
    
        // Dự báo công suất phòng
        public double PredictedOccupancyRate { get; set; }
    
        // So sánh với tháng trước
        public decimal RevenueChangePercent { get; set; }
        public double OccupancyChangePercent { get; set; }
    
        // Nhận xét/phân tích từ AI
        public string AiAnalysis { get; set; } = "";
    
        // Dữ liệu lịch sử để vẽ biểu đồ xu hướng (6 tháng gần nhất + 1 tháng dự báo)
        public List<MonthlyDataPoint> HistoricalData { get; set; } = new();
    
        // Thời gian tạo dự báo
        public DateTime GeneratedAt { get; set; }
    }
  
    public class MonthlyDataPoint
    {
        public string Label { get; set; } = "";       // "06/2026"
        public decimal TotalRevenue { get; set; }
        public decimal RoomRevenue { get; set; }
        public decimal FnBRevenue { get; set; }
        public decimal TourRevenue { get; set; }
        public double OccupancyRate { get; set; }
        public bool IsForecast { get; set; }           // true = dự báo, false = thực tế
    }
}
