# Kế Hoạch: Dự Báo Tài Chính Tháng Tiếp Theo Bằng AI

## 1. Tổng Quan

**Mục tiêu:** Xây dựng tính năng dự báo tài chính cho tháng tiếp theo dựa trên dữ liệu báo cáo tài chính lịch sử. Tính năng này dành riêng cho **Business Manager** và sẽ được tích hợp vào trang Báo Cáo Tài Chính (`FinancialReport`).

**Phương pháp:** Sử dụng **Gemini AI API** (đã có sẵn trong dự án qua `GeminiService`) để phân tích dữ liệu doanh thu lịch sử và đưa ra dự báo. Cách này **KHÔNG yêu cầu** cài thêm thư viện ML nào, tận dụng 100% hạ tầng có sẵn.

**Luồng hoạt động:**

```
[DashboardReportService] → Lấy dữ liệu doanh thu các tháng trước
        ↓
[ForecastService] → Chuẩn hóa dữ liệu thành prompt → Gửi lên Gemini API
        ↓
[Gemini AI] → Phân tích xu hướng → Trả về JSON dự báo
        ↓
[DashboardController] → Parse JSON → Truyền vào ViewModel
        ↓
[FinancialReport.cshtml] → Hiển thị biểu đồ + bảng dự báo
```

---

## 2. Kiến Trúc Hệ Thống

### 2.1. Backend (BE)

#### A. ViewModel - `ForecastResultViewModel.cs`

**Vị trí:** `ViewModels/Dashboard/ForecastResultViewModel.cs`

```csharp
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
```

#### B. Service Interface - `IForecastService.cs`

**Vị trí:** `Services/Dashboard/IForecastService.cs`

```csharp
namespace PhucNguyenResort.Services.Dashboard
{
    public interface IForecastService
    {
        Task<ForecastResultViewModel> GetForecastAsync();
    }
}
```

#### C. Service Implementation - `ForecastService.cs`

**Vị trí:** `Services/Dashboard/ForecastService.cs`

**Logic chính:**

1. **Thu thập dữ liệu lịch sử:** Gọi `IDashboardReportService.GetRevenueBreakdownAsync()` và `GetOccupancyDataAsync()` cho **6 tháng gần nhất** (hoặc toàn bộ dữ liệu có sẵn).
2. **Xây dựng prompt cho Gemini AI:**

   ```
   Bạn là chuyên gia phân tích tài chính khách sạn.
   Dựa trên dữ liệu doanh thu các tháng dưới đây, hãy dự báo cho tháng {tháng tiếp theo}/{năm}.

   DỮ LIỆU LỊCH SỬ:
   | Tháng | Doanh Thu Phòng | Doanh Thu F&B | Doanh Thu Tour | Tổng | Tỷ lệ lấp đầy |
   | 01/2026 | 50,000,000 | 15,000,000 | 8,000,000 | 73,000,000 | 45.2% |
   | 02/2026 | 62,000,000 | 18,000,000 | 12,000,000 | 92,000,000 | 52.1% |
   ...

   Trả lời ĐÚNG định dạng JSON sau (KHÔNG thêm markdown, KHÔNG thêm giải thích ngoài JSON):
   {
     "predictedTotalRevenue": 0,
     "predictedRoomRevenue": 0,
     "predictedFnBRevenue": 0,
     "predictedTourRevenue": 0,
     "predictedOccupancyRate": 0.0,
     "analysis": "Phân tích xu hướng ngắn gọn bằng tiếng Việt (tối đa 3-4 câu)"
   }
   ```
3. **Gọi Gemini API:** Sử dụng `HttpClient` gửi request tương tự pattern trong `GeminiService.cs` (đọc `ApiKey`, `Model`, `ApiUrl` từ `IConfiguration`).
4. **Parse JSON response** và map vào `ForecastResultViewModel`.
5. **Tính toán phần trăm thay đổi** so với tháng gần nhất.
6. **Ghép dữ liệu lịch sử + dự báo** vào `HistoricalData` để vẽ biểu đồ.

```csharp
public class ForecastService : IForecastService
{
    private readonly IDashboardReportService _reportService;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ForecastService> _logger;

    // Constructor inject các dependency ở trên

    public async Task<ForecastResultViewModel> GetForecastAsync()
    {
        // 1. Xác định tháng dự báo = tháng hiện tại + 1
        var today = DateTime.Today;
        var forecastDate = today.AddMonths(1);
      
        // 2. Lấy dữ liệu 6 tháng gần nhất
        var historicalData = new List<MonthlyDataPoint>();
        for (int i = 6; i >= 1; i--)
        {
            var targetDate = today.AddMonths(-i + 1); // Bao gồm cả tháng hiện tại
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
      
        // 3. Xây dựng prompt và gọi Gemini API
        var prompt = BuildForecastPrompt(historicalData, forecastDate);
        var aiResponse = await CallGeminiApiAsync(prompt);
      
        // 4. Parse response JSON và trả về ViewModel
        // ... (parse JSON, tính toán % thay đổi, ghép biểu đồ)
    }
}
```

#### D. Controller - Thêm action `Forecast` vào `DashboardController.cs`

```csharp
// Thêm vào DashboardController.cs
[Authorize(Roles = "Business Manager")]
public async Task<IActionResult> Forecast()
{
    var forecast = await _forecastService.GetForecastAsync();
    return View(forecast);
}
```

#### E. Đăng ký DI trong `Program.cs`

```csharp
builder.Services.AddScoped<IForecastService, ForecastService>();
```

#### F. Thêm menu sidebar trong `_DashboardLayout.cshtml`

Thêm mục **"Dự Báo AI"** ngay dưới mục "Business Reports" trong sidebar cho role Business Manager:

```html
<li class="@(ViewContext.RouteData.Values["Action"]?.ToString() == "Forecast" ? "active" : "")">
    <a asp-controller="Dashboard" asp-action="Forecast">
        <i class="fa fa-line-chart"></i> Dự Báo AI
    </a>
</li>
```

---

### 2.2. Frontend (FE) - `Forecast.cshtml`

**Vị trí:** `Views/Dashboard/Forecast.cshtml`

#### Thiết kế giao diện:

**Bố cục trang:**

```
┌─────────────────────────────────────────────────────────────────┐
│  ← Quay lại Bảng điều khiển                                    │
│  📊 Dự Báo Tài Chính AI                                       │
│  Dự báo doanh thu tháng {MM/YYYY} dựa trên dữ liệu lịch sử   │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐       │
│  │ TỔNG DT  │  │ DT PHÒNG │  │ DT F&B   │  │ DT TOUR  │       │
│  │ dự báo   │  │ dự báo   │  │ dự báo   │  │ dự báo   │       │
│  │ ▲/▼ x%   │  │ ▲/▼ x%   │  │ ▲/▼ x%   │  │ ▲/▼ x%   │       │
│  └──────────┘  └──────────┘  └──────────┘  └──────────┘       │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │           BIỂU ĐỒ XU HƯỚNG DOANH THU                   │   │
│  │   (Line Chart: 6 tháng thực tế + 1 tháng dự báo)       │   │
│  │   - Đường liền: dữ liệu thực tế                        │   │
│  │   - Đường nét đứt: dự báo AI                            │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
│  ┌────────────────────────┐  ┌──────────────────────────────┐  │
│  │  TỶ LỆ LẤP ĐẦY       │  │  PHÂN TÍCH CỦA AI            │  │
│  │  Dự báo: xx.x%        │  │  "Doanh thu tháng tới dự kiến│  │
│  │  Tháng trước: xx.x%   │  │  tăng xx% do..."             │  │
│  │  (Gauge Chart)         │  │                              │  │
│  └────────────────────────┘  └──────────────────────────────┘  │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  BẢNG SO SÁNH CHI TIẾT                                  │   │
│  │  | Chỉ số      | Tháng trước  | Dự báo     | Thay đổi | │   │
│  │  | Tổng DT     | xxx VNĐ      | xxx VNĐ    | +x%      | │   │
│  │  | DT Phòng    | xxx VNĐ      | xxx VNĐ    | +x%      | │   │
│  │  | DT F&B      | xxx VNĐ      | xxx VNĐ    | -x%      | │   │
│  │  | DT Tour     | xxx VNĐ      | xxx VNĐ    | +x%      | │   │
│  │  | Tỷ lệ lấp   | xx.x%       | xx.x%      | +x%      | │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
│  ⏱️ Dự báo được tạo lúc: dd/MM/yyyy HH:mm                     │
│  ⚠️ Lưu ý: Kết quả là ước tính dựa trên AI, chỉ mang tính    │
│     tham khảo và có thể không phản ánh chính xác thực tế.      │
└─────────────────────────────────────────────────────────────────┘
```

#### Thư viện biểu đồ: **Chart.js** (đã có sẵn trong project)

- **Line Chart:** Hiển thị xu hướng doanh thu 6 tháng + 1 tháng dự báo (nét đứt).
- **Gauge/Doughnut Chart:** Hiển thị tỷ lệ lấp đầy dự báo.

#### Styling:

- Tuân theo design system hiện tại (`Playfair Display` + `Inter`, color palette `#805528`, `#dfa974`, `#fcf9f5`...).
- KPI Cards sử dụng class `.kpi-card` có sẵn.
- Phần dự báo đánh dấu bằng border gradient hoặc badge "AI Forecast" để phân biệt.

---

## 3. Danh Sách File Cần Thay Đổi

### File MỚI tạo:

| # | File                                                | Mô tả                                    |
| - | --------------------------------------------------- | ------------------------------------------ |
| 1 | `ViewModels/Dashboard/ForecastResultViewModel.cs` | ViewModel chứa dữ liệu dự báo         |
| 2 | `Services/Dashboard/IForecastService.cs`          | Interface cho ForecastService              |
| 3 | `Services/Dashboard/ForecastService.cs`           | Logic gọi Gemini API và xử lý dự báo |
| 4 | `Views/Dashboard/Forecast.cshtml`                 | Giao diện trang dự báo AI               |

### File CẦN CHỈNH SỬA:

| # | File                                     | Nội dung chỉnh sửa                                     |
| - | ---------------------------------------- | --------------------------------------------------------- |
| 5 | `Controllers/DashboardController.cs`   | Thêm action`Forecast()`, inject `IForecastService`   |
| 6 | `Program.cs`                           | Đăng ký DI:`IForecastService` → `ForecastService` |
| 7 | `Views/Shared/_DashboardLayout.cshtml` | Thêm menu "Dự Báo AI" cho Business Manager             |

---

## 4. Trình Tự Triển Khai

### Bước 1: Tạo ViewModel (BE)

- Tạo file `ForecastResultViewModel.cs`

### Bước 2: Tạo Service (BE)

- Tạo `IForecastService.cs`
- Tạo `ForecastService.cs` với logic:
  - Thu thập dữ liệu 6 tháng gần nhất qua `IDashboardReportService`
  - Xây dựng prompt chuẩn cho Gemini AI
  - Gọi Gemini API (tái sử dụng config `Gemini:ApiKey`, `Gemini:Model`)
  - Parse JSON response → map vào ViewModel
  - Tính % thay đổi so với tháng hiện tại

### Bước 3: Cập nhật Controller (BE)

- Inject `IForecastService` vào `DashboardController`
- Thêm action `Forecast()`

### Bước 4: Đăng ký DI (BE)

- Thêm dòng đăng ký trong `Program.cs`

### Bước 5: Tạo View (FE)

- Tạo `Forecast.cshtml` với:
  - 4 KPI Cards (Tổng DT, DT Phòng, DT F&B, DT Tour) + badge phần trăm thay đổi
  - Line Chart xu hướng doanh thu (Chart.js)
  - Gauge chart tỷ lệ lấp đầy
  - Bảng so sánh chi tiết
  - Card phân tích AI
  - Disclaimer (lưu ý kết quả AI chỉ mang tính tham khảo)

### Bước 6: Cập nhật Sidebar (FE)

- Thêm menu item "Dự Báo AI" vào `_DashboardLayout.cshtml`

### Bước 7: Testing

- Build và chạy thử
- Kiểm tra với các trường hợp:
  - Có đủ 6 tháng dữ liệu
  - Dữ liệu ít hơn 6 tháng
  - Không có dữ liệu lịch sử (hiển thị thông báo phù hợp)

---

## 5. Lưu Ý Quan Trọng

1. **Không cần cài thêm NuGet package nào** - tận dụng `HttpClient` + `System.Text.Json` sẵn có.
2. **Tái sử dụng config Gemini** đã cấu hình trong `appsettings.json` (`Gemini:ApiKey`, `Gemini:Model`, `Gemini:ApiUrl`).
3. **Prompt Engineering:** Yêu cầu Gemini trả về **đúng định dạng JSON** để dễ parse. Thêm `temperature: 0.2` để kết quả ổn định.
4. **Error Handling:** Xử lý các trường hợp Gemini API timeout, trả sai format, hoặc không có dữ liệu lịch sử.
5. **Disclaimer:** Luôn hiển thị dòng cảnh báo rằng kết quả AI chỉ mang tính **tham khảo**.
6. **Authorize:** Chỉ role `Business Manager` mới truy cập được trang dự báo.
