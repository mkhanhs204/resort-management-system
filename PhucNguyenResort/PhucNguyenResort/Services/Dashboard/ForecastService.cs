using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PhucNguyenResort.ViewModels.Dashboard;

namespace PhucNguyenResort.Services.Dashboard
{
    public class ForecastService : IForecastService
    {
        private readonly IDashboardReportService _reportService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ForecastService> _logger;

        public ForecastService(
            IDashboardReportService reportService,
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ForecastService> logger)
        {
            _reportService = reportService;
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ForecastResultViewModel> GetForecastAsync()
        {
            var today = DateTime.Today;
            var forecastDate = today.AddMonths(1);

            var historicalData = new List<MonthlyDataPoint>();

            // Lấy dữ liệu 6 tháng gần nhất
            for (int i = 6; i >= 1; i--)
            {
                var targetDate = today.AddMonths(-i + 1);
                var revenue = await _reportService.GetRevenueBreakdownAsync(targetDate.Month, targetDate.Year);
                var occupancy = await _reportService.GetOccupancyDataAsync(targetDate.Month, targetDate.Year);

                historicalData.Add(new MonthlyDataPoint
                {
                    Label = $"{targetDate.Month:D2}/{targetDate.Year}",
                    TotalRevenue = revenue.TotalRevenue,
                    RoomRevenue = revenue.RoomRevenue,
                    FnBRevenue = revenue.FnBRevenue,
                    TourRevenue = revenue.TourRevenue,
                    OccupancyRate = occupancy.OccupancyRatePercent,
                    IsForecast = false
                });
            }

            var prompt = BuildForecastPrompt(historicalData, forecastDate);
            var aiResponseJson = await CallGeminiApiAsync(prompt);
            
            var result = ParseForecastResponse(aiResponseJson, forecastDate);
            
            // Tính phần trăm thay đổi so với tháng gần nhất
            var lastMonth = historicalData.LastOrDefault();
            if (lastMonth != null)
            {
                if (lastMonth.TotalRevenue > 0)
                {
                    result.RevenueChangePercent = ((result.PredictedTotalRevenue - lastMonth.TotalRevenue) / lastMonth.TotalRevenue) * 100;
                }
                
                result.OccupancyChangePercent = result.PredictedOccupancyRate - lastMonth.OccupancyRate;
            }

            // Gán data biểu đồ
            result.HistoricalData = historicalData.ToList();
            
            // Thêm tháng dự báo vào biểu đồ
            result.HistoricalData.Add(new MonthlyDataPoint
            {
                Label = $"{forecastDate.Month:D2}/{forecastDate.Year}",
                TotalRevenue = result.PredictedTotalRevenue,
                RoomRevenue = result.PredictedRoomRevenue,
                FnBRevenue = result.PredictedFnBRevenue,
                TourRevenue = result.PredictedTourRevenue,
                OccupancyRate = result.PredictedOccupancyRate,
                IsForecast = true
            });

            return result;
        }

        private string BuildForecastPrompt(List<MonthlyDataPoint> historicalData, DateTime forecastDate)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Bạn là chuyên gia phân tích tài chính khách sạn.");
            sb.AppendLine($"Dựa trên dữ liệu doanh thu các tháng dưới đây, hãy dự báo cho tháng {forecastDate.Month}/{forecastDate.Year}.");
            sb.AppendLine();
            sb.AppendLine("DỮ LIỆU LỊCH SỬ:");
            sb.AppendLine("| Tháng | Doanh Thu Phòng | Doanh Thu F&B | Doanh Thu Tour | Tổng | Tỷ lệ lấp đầy |");
            
            foreach (var data in historicalData)
            {
                sb.AppendLine($"| {data.Label} | {data.RoomRevenue} | {data.FnBRevenue} | {data.TourRevenue} | {data.TotalRevenue} | {data.OccupancyRate}% |");
            }

            sb.AppendLine();
            sb.AppendLine("Trả lời ĐÚNG định dạng JSON sau (KHÔNG thêm markdown, KHÔNG thêm giải thích ngoài JSON):");
            sb.AppendLine("{");
            sb.AppendLine("  \"predictedTotalRevenue\": 0,");
            sb.AppendLine("  \"predictedRoomRevenue\": 0,");
            sb.AppendLine("  \"predictedFnBRevenue\": 0,");
            sb.AppendLine("  \"predictedTourRevenue\": 0,");
            sb.AppendLine("  \"predictedOccupancyRate\": 0.0, // Trả về con số phần trăm (VD: 1.5 cho 1.5%), KHÔNG dùng số thập phân (như 0.015), KHÔNG kèm ký hiệu %");
            sb.AppendLine("  \"analysis\": \"Phân tích xu hướng ngắn gọn bằng tiếng Việt (tối đa 3-4 câu)\"");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private async Task<string> CallGeminiApiAsync(string prompt)
        {
            try
            {
                var apiKey = _configuration["Gemini:ApiKey"];
                var model = _configuration["Gemini:Model"] ?? "gemini-1.5-flash";
                var rawUrl = _configuration["Gemini:ApiUrl"] ?? "https://generativelanguage.googleapis.com/v1beta/models/{0}:generateContent?key={1}";
                var apiUrl = string.Format(rawUrl, model, apiKey);

                var requestPayload = new
                {
                    contents = new[]
                    {
                        new
                        {
                            role = "user",
                            parts = new[] { new { text = prompt } }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.2, // Kết quả ổn định
                        maxOutputTokens = 8000 // Tăng max token vì gemini-2.5-flash cần nhiều token để thinking
                    }
                };

                var jsonPayload = JsonSerializer.Serialize(requestPayload);
                var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(apiUrl, httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    var errBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Gemini API Error for Forecast: Status {Status}, Body: {Body}", response.StatusCode, errBody);
                    return null;
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                
                // GHI LOG RA FILE ĐỂ DEBUG
                try {
                    System.IO.File.WriteAllText(@"e:\SWP391_PROJECT\su26-swp391-se2023-g1\gemini_response_log.txt", responseBody);
                } catch { }

                using var doc = JsonDocument.Parse(responseBody);
              
                var text = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return text;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra trong quá trình gọi Gemini API dự báo");
                return null;
            }
        }

        private ForecastResultViewModel ParseForecastResponse(string json, DateTime forecastDate)
        {
            var result = new ForecastResultViewModel
            {
                ForecastMonth = forecastDate.Month,
                ForecastYear = forecastDate.Year,
                GeneratedAt = DateTime.Now
            };

            if (string.IsNullOrWhiteSpace(json))
            {
                result.AiAnalysis = "Không thể lấy dự báo từ AI lúc này.";
                return result;
            }

            try
            {
                // Tìm kiếm vị trí của JSON object trong chuỗi trả về
                int startIndex = json.IndexOf('{');
                int endIndex = json.LastIndexOf('}');
                
                if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                {
                    var jsonString = json.Substring(startIndex, endIndex - startIndex + 1);
                    using var doc = JsonDocument.Parse(jsonString);
                    var root = doc.RootElement;

                    // Hàm hỗ trợ parse số an toàn hơn
                    decimal ParseDecimal(JsonElement element)
                    {
                        if (element.ValueKind == JsonValueKind.Number) return element.GetDecimal();
                        if (element.ValueKind == JsonValueKind.String && decimal.TryParse(element.GetString()?.Replace(",", ""), out var val)) return val;
                        return 0;
                    }

                    double ParseDouble(JsonElement element)
                    {
                        if (element.ValueKind == JsonValueKind.Number) return element.GetDouble();
                        if (element.ValueKind == JsonValueKind.String && double.TryParse(element.GetString()?.Replace("%", ""), out var val)) return val;
                        return 0;
                    }

                    // Hàm hỗ trợ lấy property không phân biệt hoa thường
                    bool TryGetPropertyCaseInsensitive(JsonElement element, string propertyName, out JsonElement value)
                    {
                        if (element.ValueKind != JsonValueKind.Object)
                        {
                            value = default;
                            return false;
                        }
                        
                        foreach (var prop in element.EnumerateObject())
                        {
                            if (prop.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                            {
                                value = prop.Value;
                                return true;
                            }
                        }
                        value = default;
                        return false;
                    }

                    if (TryGetPropertyCaseInsensitive(root, "predictedTotalRevenue", out var totalRev)) result.PredictedTotalRevenue = ParseDecimal(totalRev);
                    if (TryGetPropertyCaseInsensitive(root, "predictedRoomRevenue", out var roomRev)) result.PredictedRoomRevenue = ParseDecimal(roomRev);
                    if (TryGetPropertyCaseInsensitive(root, "predictedFnBRevenue", out var fnbRev)) result.PredictedFnBRevenue = ParseDecimal(fnbRev);
                    if (TryGetPropertyCaseInsensitive(root, "predictedTourRevenue", out var tourRev)) result.PredictedTourRevenue = ParseDecimal(tourRev);
                    if (TryGetPropertyCaseInsensitive(root, "predictedOccupancyRate", out var occRate)) result.PredictedOccupancyRate = ParseDouble(occRate);
                    if (TryGetPropertyCaseInsensitive(root, "analysis", out var analysis)) result.AiAnalysis = analysis.GetString() ?? "";
                }
                else
                {
                    // Trường hợp AI không trả về JSON mà trả về chuỗi text thông thường (VD: từ chối trả lời)
                    result.AiAnalysis = json;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi parse JSON kết quả dự báo AI: {Json}", json);
                // Hiển thị cả nguyên bản phản hồi của AI nếu parse lỗi để user dễ nhận biết nguyên nhân
                result.AiAnalysis = "Lỗi khi xử lý JSON từ AI. Phản hồi gốc: " + json;
            }

            return result;
        }
    }
}
