# Implementation Plan – UC22 / UC23 / UC24
## Business Manager Dashboard & Financial Report

> **Module 5: Consolidated Billing & Dashboards** (SWP391-HOS-02)  
> **Actor:** Business Manager  
> **Project:** PhucNguyen Resort & Tour Hub – ASP.NET Core MVC (.NET 8)

---

## 1. Tổng quan yêu cầu

| UC | Tên | Mô tả SRS |
|----|------|-----------|
| UC22 | View Room Occupancy Dashboard | Hiển thị bảng điều khiển trực quan **Tỷ lệ lấp đầy phòng (%)** cho tháng hiện tại |
| UC23 | View Revenue Dashboard | Bảng điều khiển doanh thu chia nhỏ thu nhập theo **3 nguồn**: Phòng vs F&B vs Tour |
| UC24 | Export Financial Report | Xuất báo cáo tài chính hàng tháng sang file **Excel / PDF** |

### Mapping với SRS V2 (SWP391_NHOM1_V2.md)

- **UC39** (SRS V2) = UC22 + UC23 (HOS-02): *View Business Dashboard* – giám sát hiệu suất tổng thể resort bao gồm thống kê doanh thu, tỷ lệ lấp đầy phòng
- **UC44** (SRS V2): *View Financial Report* – xem báo cáo tài chính, phân tích nguồn doanh thu
- **UC45** (SRS V2) = UC24 (HOS-02): *Export PDF/Excel Report* – xuất báo cáo sang PDF/Excel

### Non-UI Functions liên quan (SRS V2 – Section 1.3.3)

| # | Feature | System Function | Mô tả |
|---|---------|-----------------|--------|
| 39 | Dashboard | Occupancy Rate Calculation | Tính KPI tỷ lệ lấp đầy phòng |
| 40 | Dashboard | Revenue Breakdown Calculation | Tính doanh thu theo nguồn |
| 41 | Report | Excel/PDF Export | Xuất báo cáo |

---

## 2. Phân tích hiện trạng (Existing Code)

### 2.1 Cấu trúc hiện có

| Layer | File | Trạng thái |
|-------|------|-----------|
| Controller | `Controllers/DashboardController.cs` | ✅ Đã có action `Manager()` – hiện chỉ hiển thị KPI đơn giản (TotalRoomCategories, TotalTours, PendingRefunds, TableReservations) |
| View | `Views/Dashboard/Manager.cshtml` | ✅ Đã có – hiển thị 4 KPI cards + Quick Links + Bảng đặt bàn hôm nay. **CHƯA CÓ** biểu đồ Occupancy và Revenue |
| ViewModel | `ViewModels/Dashboard/ReceptionistDashboardViewModel.cs` | ✅ Mẫu ViewModel cho dashboard – có thể tham khảo pattern |
| ViewModel | ❌ **Chưa có** `ManagerDashboardViewModel` | ⚠️ Cần tạo mới |
| Service | ❌ **Chưa có** `IDashboardReportService` | ⚠️ Cần tạo mới |

### 2.2 Models & Enums liên quan

| Entity | Mục đích | Fields quan trọng |
|--------|----------|-------------------|
| `Room` | Tổng số phòng | `RoomId`, `Status` (Available/Occupied/Maintenance/NeedsCleaning/CleaningInProgress) |
| `Booking` | Tracking đặt phòng | `CheckInDate`, `CheckOutDate`, `BookingStatus` (Reserved/Arrived/CheckedIn/CheckedOut/Cancelled/NoShow/PendingPayment) |
| `BookingDetail` | Chi tiết phòng đặt | `BookingId`, `RoomCategoryId`, `RoomId`, `Price` |
| `GuestFolio` | Tổng hợp chi phí | `TotalRoomCharges`, `TotalFoodCharges`, `TotalTourCharges`, `TotalAmount`, `Status` |
| `FolioTransaction` | Chi tiết giao dịch | `TransactionType` (RoomCharge/FoodCharge/TourCharge/Payment/Refund/Adjustment), `Amount`, `CreatedAt` |
| `Invoice` | Hóa đơn | `TotalAmount`, `FinalAmount`, `InvoiceStatus` (Draft/Issued/Paid/Cancelled), `IssuedAt` |
| `Payment` | Thanh toán | `Amount`, `PaymentStatus`, `PaidAt` |
| `FoodOrder` | Đơn đặt thức ăn | `TotalAmount`, `OrderedAt`, `IsChargedToRoom`, `ChargedAt` |
| `TourBooking` | Đặt tour | `TotalAmount`, `BookingStatus`, `CreatedAt` |

### 2.3 NuGet Packages

- **Hiện có:** EF Core 8.0, MailKit – **CHƯA CÓ** thư viện xuất Excel/PDF
- **Cần thêm:**
  - `ClosedXML` (xuất Excel .xlsx) – nhẹ, không cần license
  - `QuestPDF` (xuất PDF) – free cho open source, API fluent

---

## 3. Kiến trúc giải pháp

```
┌──────────────────────────────────────────────────────────┐
│                   DashboardController                     │
│  Manager()  │  FinancialReport()  │  ExportExcel/PDF()   │
└──────┬──────┴──────────┬──────────┴──────────┬───────────┘
       │                 │                     │
       ▼                 ▼                     ▼
┌──────────────────────────────────────────────────────────┐
│               IDashboardReportService                     │
│  GetOccupancyDataAsync()  │  GetRevenueBreakdownAsync()  │
│  GetFinancialReportAsync()│  ExportToExcelAsync()        │
│                           │  ExportToPdfAsync()          │
└──────┬───────────────────┬───────────────────────────────┘
       │                   │
       ▼                   ▼
┌──────────────────────────────────────────────────────────┐
│                    ResortDbContext                         │
│  Rooms │ Bookings │ GuestFolios │ FolioTransactions      │
│  Invoices │ Payments │ FoodOrders │ TourBookings         │
└──────────────────────────────────────────────────────────┘
```

---

## 4. Chi tiết Implementation

### Phase 1: ViewModels (Tạo mới)

#### [NEW] `ViewModels/Dashboard/ManagerDashboardViewModel.cs`

```csharp
namespace PhucNguyenResort.ViewModels.Dashboard;

public class ManagerDashboardViewModel
{
    // === UC22: Room Occupancy ===
    public OccupancyData Occupancy { get; set; } = new();
    
    // === UC23: Revenue Breakdown ===
    public RevenueBreakdownData Revenue { get; set; } = new();
    
    // === Existing KPIs (giữ lại) ===
    public int TotalRoomCategories { get; set; }
    public int TotalTours { get; set; }
    public int PendingRefunds { get; set; }
    
    // === Table Reservation (giữ lại từ FnBDashboardViewModel) ===
    public int PendingReservationsToday { get; set; }
    public int ConfirmedReservationsToday { get; set; }
    public List<TableReservationItem> TodayReservations { get; set; } = new();
}

public class OccupancyData
{
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }              // Phòng đang có khách (CheckedIn)
    public double OccupancyRatePercent { get; set; }    // % = OccupiedRooms / TotalRooms * 100
    public int Month { get; set; }
    public int Year { get; set; }
    
    // Dữ liệu biểu đồ theo ngày trong tháng
    public List<DailyOccupancy> DailyData { get; set; } = new();
}

public class DailyOccupancy
{
    public DateTime Date { get; set; }
    public int OccupiedRooms { get; set; }
    public int TotalRooms { get; set; }
    public double OccupancyRatePercent { get; set; }
}

public class RevenueBreakdownData
{
    public decimal TotalRevenue { get; set; }
    public decimal RoomRevenue { get; set; }
    public decimal FnBRevenue { get; set; }
    public decimal TourRevenue { get; set; }
    
    public double RoomPercentage { get; set; }
    public double FnBPercentage { get; set; }
    public double TourPercentage { get; set; }
    
    public int Month { get; set; }
    public int Year { get; set; }
    
    // Dữ liệu biểu đồ doanh thu theo ngày
    public List<DailyRevenue> DailyData { get; set; } = new();
}

public class DailyRevenue
{
    public DateTime Date { get; set; }
    public decimal RoomRevenue { get; set; }
    public decimal FnBRevenue { get; set; }
    public decimal TourRevenue { get; set; }
}
```

#### [NEW] `ViewModels/Dashboard/FinancialReportViewModel.cs`

```csharp
namespace PhucNguyenResort.ViewModels.Dashboard;

public class FinancialReportViewModel
{
    public int Month { get; set; }
    public int Year { get; set; }
    
    // Tổng quan
    public decimal TotalRevenue { get; set; }
    public decimal TotalRoomRevenue { get; set; }
    public decimal TotalFnBRevenue { get; set; }
    public decimal TotalTourRevenue { get; set; }
    
    // Tỷ lệ lấp đầy
    public double AverageOccupancyRate { get; set; }
    
    // Chi tiết booking
    public int TotalBookings { get; set; }
    public int CompletedBookings { get; set; }    // CheckedOut
    public int CancelledBookings { get; set; }
    
    // Chi tiết invoice
    public int TotalInvoices { get; set; }
    public int PaidInvoices { get; set; }
    
    // Dữ liệu chi tiết theo ngày
    public List<DailyRevenue> DailyRevenues { get; set; } = new();
    public List<DailyOccupancy> DailyOccupancies { get; set; } = new();
}
```

---

### Phase 2: Service Layer

#### [NEW] `Services/Dashboard/IDashboardReportService.cs`

```csharp
namespace PhucNguyenResort.Services.Dashboard;

public interface IDashboardReportService
{
    // UC22: Room Occupancy
    Task<OccupancyData> GetOccupancyDataAsync(int month, int year);
    
    // UC23: Revenue Breakdown
    Task<RevenueBreakdownData> GetRevenueBreakdownAsync(int month, int year);
    
    // UC24: Financial Report
    Task<FinancialReportViewModel> GetFinancialReportAsync(int month, int year);
    Task<byte[]> ExportToExcelAsync(int month, int year);
    Task<byte[]> ExportToPdfAsync(int month, int year);
}
```

#### [NEW] `Services/Dashboard/DashboardReportService.cs`

**Logic tính toán chính:**

##### UC22 – Occupancy Rate Calculation

```
Công thức theo tiêu chuẩn USALI:

Occupancy Rate (%) = (Số phòng-đêm bán ra / Tổng số phòng-đêm khả dụng) × 100

Với mỗi ngày trong tháng:
  - OccupiedRooms = COUNT(Bookings WHERE BookingStatus IN (CheckedIn, CheckedOut) 
                    AND CheckInDate <= ngày AND CheckOutDate > ngày)
  - TotalRooms = COUNT(Rooms WHERE Status != Maintenance)
  - DailyOccupancyRate = OccupiedRooms / TotalRooms * 100

Tỷ lệ trung bình tháng = AVG(DailyOccupancyRate cho tất cả ngày đã qua)
```

##### UC23 – Revenue Breakdown Calculation

```
Nguồn dữ liệu: Bảng FolioTransactions (đáng tin cậy nhất vì ghi nhận mọi giao dịch)

1. Room Revenue = SUM(FolioTransactions 
     WHERE TransactionType = RoomCharge 
     AND CreatedAt trong tháng)

2. F&B Revenue = SUM(FolioTransactions 
     WHERE TransactionType = FoodCharge 
     AND CreatedAt trong tháng)
   + SUM(FoodOrders WHERE IsPaidAtRestaurant = true 
     AND PaidAt trong tháng)    // Đơn F&B trả tiền tại nhà hàng

3. Tour Revenue = SUM(FolioTransactions 
     WHERE TransactionType = TourCharge 
     AND CreatedAt trong tháng)
   + SUM(Payments WHERE TourBookingId != null 
     AND PaymentStatus = Completed 
     AND PaidAt trong tháng)    // Tour thanh toán riêng
```

##### UC24 – Export Logic

- **Excel:** Sử dụng ClosedXML tạo workbook với 3 sheet:
  1. **Summary** – Tổng quan: KPIs, tỷ lệ lấp đầy, doanh thu tổng
  2. **Daily Occupancy** – Chi tiết lấp đầy theo ngày
  3. **Daily Revenue** – Chi tiết doanh thu theo ngày chia theo 3 nguồn

- **PDF:** Sử dụng QuestPDF tạo báo cáo có:
  1. Header với logo resort + tháng/năm
  2. Bảng tổng quan KPI
  3. Bảng chi tiết doanh thu
  4. Bảng chi tiết lấp đầy phòng

---

### Phase 3: Controller Updates

#### [MODIFY] `Controllers/DashboardController.cs`

**Thay đổi cần làm:**

1. **Inject `IDashboardReportService`** vào constructor
2. **Cập nhật action `Manager()`** – sử dụng `ManagerDashboardViewModel` thay vì dùng `ViewBag`
3. **Thêm action `FinancialReport(int? month, int? year)`** – trang xem báo cáo tài chính (UC44/SRS V2)
4. **Thêm action `ExportExcel(int month, int year)`** – xuất Excel (UC24)
5. **Thêm action `ExportPdf(int month, int year)`** – xuất PDF (UC24)

```csharp
// Action mới cho UC24
[Authorize(Roles = "Business Manager")]
public async Task<IActionResult> FinancialReport(int? month, int? year)
{
    var m = month ?? DateTime.Now.Month;
    var y = year ?? DateTime.Now.Year;
    var model = await _dashboardReportService.GetFinancialReportAsync(m, y);
    return View(model);
}

[Authorize(Roles = "Business Manager")]
public async Task<IActionResult> ExportExcel(int month, int year)
{
    var bytes = await _dashboardReportService.ExportToExcelAsync(month, year);
    return File(bytes, 
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        $"PhucNguyen_Financial_Report_{year}_{month:D2}.xlsx");
}

[Authorize(Roles = "Business Manager")]
public async Task<IActionResult> ExportPdf(int month, int year)
{
    var bytes = await _dashboardReportService.ExportToPdfAsync(month, year);
    return File(bytes, "application/pdf",
        $"PhucNguyen_Financial_Report_{year}_{month:D2}.pdf");
}
```

---

### Phase 4: Views (UI)

#### [MODIFY] `Views/Dashboard/Manager.cshtml`

Nâng cấp trang Manager Dashboard hiện tại, **bổ sung thêm** (giữ nguyên phần Quick Links và Table Reservations hiện có):

**Thêm Section 1 – UC22: Room Occupancy Rate**

- 1 card KPI lớn hiển thị **Tỷ lệ lấp đầy (%) tháng hiện tại** dạng vòng tròn (doughnut)
- 1 biểu đồ đường (line chart) hiển thị tỷ lệ lấp đầy **theo ngày trong tháng**
- Sử dụng **Chart.js** (CDN, không cần cài NuGet)
- Hiệu ứng animation khi load biểu đồ

**Thêm Section 2 – UC23: Revenue Breakdown**

- 1 biểu đồ tròn (pie/doughnut chart) chia tỷ lệ doanh thu: Room vs F&B vs Tour
- 1 biểu đồ cột xếp chồng (stacked bar chart) doanh thu theo ngày
- 3 card nhỏ hiển thị tổng doanh thu từng nguồn
- Sử dụng **Chart.js**

**Thêm Section 3 – Quick Action: Export Report (UC24)**

- Dropdown chọn tháng/năm
- 2 nút: **Xuất Excel** và **Xuất PDF**
- Link tới trang Financial Report chi tiết

**UI Design:** Giữ style consistency với Manager Dashboard hiện tại:
- Font: Playfair Display (heading) + Inter (body)
- Color palette: #805528 (primary), #dfa974 (accent), #f6f3ef (background)
- Card style: rounded-16px, subtle shadow, hover effect

#### [NEW] `Views/Dashboard/FinancialReport.cshtml`

Trang báo cáo tài chính chi tiết với:
- Header: Tiêu đề + bộ lọc tháng/năm
- Bảng tổng quan KPIs
- Bảng chi tiết doanh thu theo ngày
- Bảng chi tiết lấp đầy phòng theo ngày
- 2 nút export: Excel + PDF
- Style theo `_DashboardLayout`

---

### Phase 5: NuGet Packages & DI Registration

#### [MODIFY] `PhucNguyenResort.Web.csproj`

```xml
<PackageReference Include="ClosedXML" Version="0.104.2" />
<PackageReference Include="QuestPDF" Version="2025.1.1" />
```

#### [MODIFY] `Program.cs`

```csharp
builder.Services.AddScoped<IDashboardReportService, DashboardReportService>();
```

---

## 5. Danh sách Files cần tạo/sửa

| # | Action | File | Mô tả |
|---|--------|------|-------|
| 1 | **[NEW]** | `ViewModels/Dashboard/ManagerDashboardViewModel.cs` | ViewModel cho Manager Dashboard (UC22+UC23) |
| 2 | **[NEW]** | `ViewModels/Dashboard/FinancialReportViewModel.cs` | ViewModel cho trang Financial Report |
| 3 | **[NEW]** | `Services/Dashboard/IDashboardReportService.cs` | Interface service |
| 4 | **[NEW]** | `Services/Dashboard/DashboardReportService.cs` | Implementation: tính toán occupancy, revenue, export |
| 5 | **[MODIFY]** | `Controllers/DashboardController.cs` | Thêm inject service + cập nhật Manager() + thêm 3 actions mới |
| 6 | **[MODIFY]** | `Views/Dashboard/Manager.cshtml` | Thêm biểu đồ Occupancy + Revenue + Export buttons |
| 7 | **[NEW]** | `Views/Dashboard/FinancialReport.cshtml` | Trang báo cáo tài chính chi tiết |
| 8 | **[MODIFY]** | `PhucNguyenResort.Web.csproj` | Thêm ClosedXML + QuestPDF |
| 9 | **[MODIFY]** | `Program.cs` | Register DI cho IDashboardReportService |

---

## 6. Thứ tự triển khai (Step-by-step)

```
Step 1: Cài NuGet Packages (ClosedXML, QuestPDF)
        └── Sửa .csproj → dotnet restore

Step 2: Tạo ViewModels
        └── ManagerDashboardViewModel.cs
        └── FinancialReportViewModel.cs

Step 3: Tạo Service Layer
        └── IDashboardReportService.cs
        └── DashboardReportService.cs (logic tính toán + export)

Step 4: Register DI trong Program.cs

Step 5: Cập nhật DashboardController.cs
        └── Inject service
        └── Sửa Manager()
        └── Thêm FinancialReport(), ExportExcel(), ExportPdf()

Step 6: Cập nhật Manager.cshtml
        └── Thêm Chart.js CDN
        └── Thêm section Occupancy Chart
        └── Thêm section Revenue Chart
        └── Thêm section Export buttons

Step 7: Tạo FinancialReport.cshtml

Step 8: Build & Test
```

---

## 7. Verification Plan

### Build Test
```bash
cd PhucNguyenResort
dotnet build
```

### Manual Test Scenarios

| # | Scenario | Expected Result |
|---|----------|-----------------|
| 1 | Login as Business Manager → Dashboard | Thấy biểu đồ Occupancy Rate + Revenue Breakdown |
| 2 | Biểu đồ Occupancy hiển thị đúng % | Kiểm tra tính toán: (phòng có khách / tổng phòng) × 100 |
| 3 | Biểu đồ Revenue hiển thị 3 nguồn | Room, F&B, Tour với tổng đúng |
| 4 | Click "Xuất Excel" | Download file .xlsx với 3 sheets |
| 5 | Click "Xuất PDF" | Download file .pdf với layout báo cáo |
| 6 | Chọn tháng/năm khác | Dashboard/Report cập nhật theo tháng đã chọn |
| 7 | Tháng không có dữ liệu | Hiển thị 0% occupancy, 0 revenue, không lỗi |

---

## 8. Lưu ý quan trọng

> ⚠️ **Business Rules (theo USALI - SRS HOS-02 Section D):**
> - Doanh thu phòng = Tổng chi phí phòng đã thanh toán (qua GuestFolio/FolioTransaction)
> - Doanh thu F&B = Chi phí thức ăn charge-to-room (FolioTransaction) + thanh toán tại nhà hàng (FoodOrder.IsPaidAtRestaurant)
> - Doanh thu Tour = Chi phí tour charge-to-folio (FolioTransaction) + thanh toán tour riêng (Payment with TourBookingId)
> - Occupancy Rate chỉ tính phòng KHÔNG trong trạng thái Maintenance

> ⚠️ **Giữ backward compatibility:**
> - Manager Dashboard hiện tại sử dụng `FnBTableReservationDashboardViewModel` làm model → cần chuyển sang `ManagerDashboardViewModel` nhưng **giữ lại dữ liệu Table Reservations** để không break UI hiện có
> - Quick Links section hiện có → giữ nguyên

> ⚠️ **Chart.js sử dụng CDN:**
> - Không cần cài NuGet, chỉ thêm `<script>` tag trong View
> - Dữ liệu truyền qua `@Json.Serialize(Model.xxx)` từ Razor sang JavaScript
