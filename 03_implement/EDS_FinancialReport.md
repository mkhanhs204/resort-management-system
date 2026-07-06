# ENGINEERING DOCUMENTATION STANDARD (EDS) v2.0

Đặc tả Kỹ thuật Hiện thực hóa — Module Financial Report & Business Dashboard

| Field          | Value                           |
| -------------- | ------------------------------- |
| Document ID    | `SWP391-REPORT-IMP-001`       |
| Version        | 1.0                             |
| Date           | 2026-07-04                      |
| Status         | Approve                         |
| Document Owner | Cuong                           |
| Author         | Cuong                           |
| Reviewed by    | Khanh                           |
| DPO Sign-off   | N/A (module không xử lý PII) |
| Approved by    | Cuong                           |
| Last Review    | 2026-07-04                      |
| Based on EDS   | v2.0                            |

## CHANGELOG

**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ. Mọi thay đổi phải ghi vào bảng này.

| Ngày      | Người thực hiện | Nội dung thay đổi                                               |
| ---------- | ------------------- | ------------------------------------------------------------------ |
| 2026-07-04 | Cuong               | Tạo tài liệu lần đầu — EDS cho Financial Report & Dashboard |

## MỤC LỤC

1. Tổng quan Module
2. Ma trận Truy vết (Traceability Matrix)
3. Architecture Decision Records (ADR)
4. Non-Functional Requirements & SLA
5. Static Modeling (Mô hình Tĩnh)
6. Dynamic Modeling (Mô hình Động)
7. Domain Event Catalog
8. Interface Specification (Đặc tả Giao diện)
9. API Specification (MVC Routes)
10. Bảng mã lỗi (Error Codes)
11. Quy trình Triển khai (Step-by-Step)
12. Rollback & Incident Runbook
13. Kịch bản Kiểm thử Chi tiết
14. Phương pháp Xác minh
15. Mẫu thử thực tế (Verification Samples)
16. Bảng tổng hợp phân quyền (Authorization Matrix)

---

## 1. Tổng quan Module

Module **Financial Report & Business Dashboard** chịu trách nhiệm tổng hợp và hiển thị các chỉ số kinh doanh quan trọng cho Business Manager, bao gồm: Tỷ lệ lấp đầy phòng (Room Occupancy Rate), phân tích doanh thu theo nguồn (Room / F&B / Tour), và xuất báo cáo tài chính hàng tháng sang Excel/PDF.

| Field                 | Value                                                                                  |
| --------------------- | -------------------------------------------------------------------------------------- |
| Module Name           | `Financial Report & Business Dashboard`                                              |
| Bounded Context       | `Consolidated Billing & Dashboards (Module 5)`                                       |
| Data Classification   | Internal (Dữ liệu tài chính tổng hợp — không chứa PII trực tiếp)            |
| Compliance Scope      | USALI (Uniform System of Accounts for the Lodging Industry)                            |
| Upstream Dependencies | `GuestFolioService, FolioTransactions, Bookings, FoodOrders, TourBookings, Payments` |
| Downstream Consumers  | `Business Manager UI (Dashboard, FinancialReport View), Excel/PDF file export`       |

## 2. Ma trận Truy vết (Traceability Matrix)

| Requirement ID | Loại (BR/UC) | Mô tả yêu cầu                                                                                                          | Thành phần Code                                                                  | Compliance Target | ADR liên quan |
| -------------- | ------------- | -------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------- | ----------------- | -------------- |
| UC39 (SRS V2)  | Use Case      | View Business Dashboard: giám sát hiệu suất tổng thể resort bao gồm thống kê doanh thu, tỷ lệ lấp đầy phòng | `DashboardReportService.GetOccupancyDataAsync()`, `GetRevenueBreakdownAsync()` | USALI             | ADR-001        |
| UC44 (SRS V2)  | Use Case      | View Financial Report: xem báo cáo tài chính, phân tích nguồn doanh thu                                             | `DashboardReportService.GetFinancialReportAsync()`                               | USALI             | ADR-001        |
| UC45 (SRS V2)  | Use Case      | Export PDF/Excel Report: xuất báo cáo sang PDF/Excel                                                                    | `DashboardReportService.ExportToExcelAsync()`, `ExportToPdfAsync()`            | —                | ADR-002        |
| UC22 (HOS-02)  | Use Case      | View Room Occupancy Rate (%) cho tháng hiện tại                                                                         | `DashboardReportService.GetOccupancyDataAsync()`                                 | USALI             | ADR-001        |
| UC23 (HOS-02)  | Use Case      | View Revenue Dashboard chia nhỏ thu nhập: Rooms vs F&B vs Tours                                                          | `DashboardReportService.GetRevenueBreakdownAsync()`                              | USALI             | ADR-001        |
| UC24 (HOS-02)  | Use Case      | Export monthly financial report sang Excel/PDF                                                                             | `DashboardController.ExportExcel()`, `ExportPdf()`                             | —                | ADR-002        |
| Non-UI#39      | System Func   | Occupancy Rate Calculation                                                                                                 | `DashboardReportService.GetOccupancyDataAsync()`                                 | USALI             | ADR-001        |
| Non-UI#40      | System Func   | Revenue Breakdown Calculation                                                                                              | `DashboardReportService.GetRevenueBreakdownAsync()`                              | USALI             | ADR-001        |
| Non-UI#41      | System Func   | Excel/PDF Export                                                                                                           | `DashboardReportService.ExportToExcelAsync()`, `ExportToPdfAsync()`            | —                | ADR-002        |

## 3. Architecture Decision Records (ADR)

### ADR-001 — Nguồn dữ liệu tính toán Occupancy & Revenue

| Field      | Value                 |
| ---------- | --------------------- |
| Status     | Accepted              |
| Deciders   | `Cuong - Developer` |
| Date       | 2026-07-04            |
| Supersedes | N/A                   |

**Bối cảnh (Context)**
Cần tổng hợp dữ liệu tài chính từ nhiều bảng (Bookings, GuestFolios, FolioTransactions, FoodOrders, TourBookings, Payments) để tính toán Occupancy Rate và Revenue Breakdown theo tháng.

**Các phương án đã xem xét (Options Considered)**

| Phương án | Mô tả                                                                  | Ưu điểm                                         | Nhược điểm                                       |
| ------------ | ------------------------------------------------------------------------ | -------------------------------------------------- | ---------------------------------------------------- |
| A            | Dùng FolioTransactions làm nguồn chính tính doanh thu               | + Đã ghi nhận mọi giao dịch, + Audit-friendly | - Bỏ sót doanh thu F&B thanh toán tại nhà hàng |
| B            | Kết hợp FolioTransactions + FoodOrders (IsPaidAtRestaurant) + Payments | + Bao phủ tất cả nguồn doanh thu               | - Phức tạp hơn, cần nhiều query                 |

**Quyết định (Decision)**
Chọn Phương án B vì đảm bảo bao phủ đầy đủ doanh thu từ 3 nguồn theo chuẩn USALI:

- **Room Revenue**: SUM(FolioTransactions WHERE TransactionType = RoomCharge)
- **F&B Revenue**: SUM(FolioTransactions WHERE TransactionType = FoodCharge) + SUM(FoodOrders WHERE IsPaidAtRestaurant = true)
- **Tour Revenue**: SUM(FolioTransactions WHERE TransactionType = TourCharge) + SUM(Payments WHERE TourBookingId != null AND PaymentStatus = Success)

**Hệ quả (Consequences)**
Tích cực:

- Bao phủ toàn bộ doanh thu, bao gồm cả trường hợp khách thanh toán F&B tại nhà hàng (không charge-to-room) và tour thanh toán riêng

Tiêu cực / Trade-offs:

- Cần đảm bảo không double-count: FoodOrders đã charge-to-room sẽ xuất hiện trong FolioTransactions, nên khi tính F&B revenue từ FoodOrders phải filter `IsPaidAtRestaurant = true AND IsChargedToRoom = false`

---

### ADR-002 — Thư viện xuất Excel/PDF

| Field      | Value                 |
| ---------- | --------------------- |
| Status     | Accepted              |
| Deciders   | `Cuong - Developer` |
| Date       | 2026-07-04            |
| Supersedes | N/A                   |

**Bối cảnh (Context)**
UC24 yêu cầu xuất báo cáo sang Excel và PDF. Cần chọn thư viện phù hợp cho ASP.NET Core 8.

**Các phương án đã xem xét (Options Considered)**

| Phương án | Mô tả                      | Ưu điểm                                           | Nhược điểm                       |
| ------------ | ---------------------------- | ---------------------------------------------------- | ------------------------------------ |
| A            | ClosedXML (Excel) + QuestPDF | + Free, + API fluent, + Không cần Office installed | - QuestPDF license cho commercial    |
| B            | EPPlus + iTextSharp          | + Mature                                             | - EPPlus license $$, iTextSharp AGPL |
| C            | NPOI + PdfSharpCore          | + Free                                               | - API phức tạp, ít docs           |

**Quyết định (Decision)**
Chọn Phương án A: **ClosedXML** cho Excel và **QuestPDF** cho PDF. Cả hai đều miễn phí cho dự án open-source/educational, API trực quan.

**Hệ quả (Consequences)**
Tích cực:

- API fluent, dễ bảo trì
- Không cần cài Office trên server

Tiêu cực:

- Cần thêm 2 NuGet packages vào project

## 4. Non-Functional Requirements & SLA

### 4.1. Performance & Availability

| Category     | Requirement                | Target SLA | Measurement Method | Compliance Basis |
| ------------ | -------------------------- | ---------- | ------------------ | ---------------- |
| Latency      | Dashboard load time        | < 3s       | Manual test        | —               |
| Latency      | Financial Report page load | < 5s       | Manual test        | —               |
| Latency      | Export Excel generation    | < 10s      | Manual test        | —               |
| Latency      | Export PDF generation      | < 10s      | Manual test        | —               |
| Availability | Uptime hệ thống          | 99%        | Monitoring         | —               |

### 4.2. Data Integrity & Retention

| Category    | Requirement                                    | Target | Verification Method   | Compliance Basis |
| ----------- | ---------------------------------------------- | ------ | --------------------- | ---------------- |
| Accuracy    | Occupancy Rate = (Occupied / Total) × 100     | 100%   | DB query verification | USALI            |
| Accuracy    | Revenue Room + F&B + Tour = Total Revenue      | 100%   | Cross-check query     | USALI            |
| Consistency | Excel/PDF data khớp với Dashboard hiển thị | 100%   | Manual comparison     | —               |

### 4.3. Security

| Category       | Requirement                              | Target                 | Verification Method | Compliance Basis |
| -------------- | ---------------------------------------- | ---------------------- | ------------------- | ---------------- |
| Access control | Chỉ Business Manager                    | Role-based [Authorize] | Auth Matrix (§16)  | RBAC             |
| File download  | File export không chứa PII trực tiếp | Aggregate data only    | Code review         | NĐ 356/2025     |

## 5. Static Modeling (Mô hình Tĩnh)

### 5.1. Class Diagram

```
┌──────────────────────────────────┐     ┌──────────────────────────────────┐
│      DashboardController         │     │    IDashboardReportService       │
├──────────────────────────────────┤     ├──────────────────────────────────┤
│ Manager()                        │────▶│ GetOccupancyDataAsync(m, y)     │
│ FinancialReport(m?, y?)          │     │ GetRevenueBreakdownAsync(m, y)  │
│ ExportExcel(m, y)                │     │ GetFinancialReportAsync(m, y)   │
│ ExportPdf(m, y)                  │     │ ExportToExcelAsync(m, y)        │
└──────────────────────────────────┘     │ ExportToPdfAsync(m, y)          │
                                         └─────────────┬────────────────────┘
                                                       │ implements
                                                       ▼
                                         ┌──────────────────────────────────┐
                                         │   DashboardReportService         │
                                         ├──────────────────────────────────┤
                                         │ _context: ResortDbContext        │
                                         └─────────────┬────────────────────┘
                                                       │ queries
                                                       ▼
┌──────────────┐  ┌──────────────┐  ┌──────────────────┐  ┌────────────────┐
│   Bookings   │  │    Rooms     │  │ FolioTransactions │  │  FoodOrders    │
│   (status,   │  │  (status,    │  │ (type, amount,    │  │ (IsPaidAt      │
│  checkIn/Out)│  │   total)     │  │  createdAt)       │  │  Restaurant)   │
└──────────────┘  └──────────────┘  └──────────────────┘  └────────────────┘
┌──────────────┐  ┌──────────────┐
│ TourBookings │  │   Payments   │
│ (totalAmount)│  │ (tourBooking │
└──────────────┘  │  Id, amount) │
                  └──────────────┘
```

### 5.2. ViewModels

```csharp
// === ManagerDashboardViewModel.cs ===
public class ManagerDashboardViewModel
{
    public OccupancyData Occupancy { get; set; }          // UC22
    public RevenueBreakdownData Revenue { get; set; }     // UC23
    public int TotalRoomCategories { get; set; }
    public int TotalTours { get; set; }
    public int PendingRefunds { get; set; }
    public int PendingReservationsToday { get; set; }
    public int ConfirmedReservationsToday { get; set; }
}

// === OccupancyData ===
public class OccupancyData
{
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public double OccupancyRatePercent { get; set; }      // = OccupiedRooms / TotalRooms * 100
    public int Month { get; set; }
    public int Year { get; set; }
    public List<DailyOccupancy> DailyData { get; set; }   // Biểu đồ theo ngày
}

// === RevenueBreakdownData ===
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
    public List<DailyRevenue> DailyData { get; set; }     // Biểu đồ theo ngày
}

// === FinancialReportViewModel ===
public class FinancialReportViewModel
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalRoomRevenue { get; set; }
    public decimal TotalFnBRevenue { get; set; }
    public decimal TotalTourRevenue { get; set; }
    public double AverageOccupancyRate { get; set; }
    public int TotalBookings { get; set; }
    public int CompletedBookings { get; set; }
    public int CancelledBookings { get; set; }
    public int TotalInvoices { get; set; }
    public int PaidInvoices { get; set; }
    public List<DailyRevenue> DailyRevenues { get; set; }
    public List<DailyOccupancy> DailyOccupancies { get; set; }
}
```

## 6. Dynamic Modeling (Mô hình Động)

### 6.1. Sequence Diagram — UC22+UC23: View Manager Dashboard

```
Manager               DashboardController      DashboardReportService      Database
   │                         │                          │                       │
   │── GET /Dashboard/Manager ──▶│                      │                       │
   │                         │── GetOccupancyDataAsync(m,y)──▶│                │
   │                         │                          │── COUNT(Rooms) ──────▶│
   │                         │                          │◀─ totalRooms ────────│
   │                         │                          │── Query Bookings ────▶│
   │                         │                          │   (CheckedIn/Out, date range)
   │                         │                          │◀─ daily occupied ────│
   │                         │                          │── Calculate % ───────│
   │                         │◀── OccupancyData ───────│                       │
   │                         │                          │                       │
   │                         │── GetRevenueBreakdownAsync(m,y)─▶│              │
   │                         │                          │── SUM FolioTxn ─────▶│
   │                         │                          │   (RoomCharge)        │
   │                         │                          │── SUM FolioTxn ─────▶│
   │                         │                          │   (FoodCharge)        │
   │                         │                          │── SUM FoodOrders ───▶│
   │                         │                          │   (IsPaidAtRestaurant)│
   │                         │                          │── SUM FolioTxn ─────▶│
   │                         │                          │   (TourCharge)        │
   │                         │                          │── SUM Payments ─────▶│
   │                         │                          │   (TourBookingId!=null)
   │                         │◀── RevenueBreakdownData──│                      │
   │                         │                          │                       │
   │◀── Render Manager.cshtml (with Chart.js) ────────│                       │
```

### 6.2. Sequence Diagram — UC24: Export Excel

```
Manager               DashboardController      DashboardReportService      ClosedXML
   │                         │                          │                       │
   │── GET /ExportExcel?m&y ─▶│                         │                       │
   │                         │── ExportToExcelAsync(m,y)──▶│                    │
   │                         │                          │── GetFinancialReport()│
   │                         │                          │── Build Workbook ────▶│
   │                         │                          │   Sheet 1: Summary    │
   │                         │                          │   Sheet 2: Occupancy  │
   │                         │                          │   Sheet 3: Revenue    │
   │                         │                          │◀─ byte[] ────────────│
   │                         │◀── byte[] ──────────────│                       │
   │◀── File Download (.xlsx) ──────────────────────────│                       │
```

### 6.3. Sequence Diagram — UC24: Export PDF

```
Manager               DashboardController      DashboardReportService      QuestPDF
   │                         │                          │                       │
   │── GET /ExportPdf?m&y ──▶│                          │                       │
   │                         │── ExportToPdfAsync(m,y)──▶│                      │
   │                         │                          │── GetFinancialReport()│
   │                         │                          │── Build Document ────▶│
   │                         │                          │   Header + KPIs       │
   │                         │                          │   Table: Revenue      │
   │                         │                          │   Table: Occupancy    │
   │                         │                          │◀─ byte[] ────────────│
   │                         │◀── byte[] ──────────────│                       │
   │◀── File Download (.pdf) ──────────────────────────│                       │
```

## 7. Domain Event Catalog

### 7.1. Events Published (Phát ra)

Module này KHÔNG phát ra domain event — chỉ đọc và tổng hợp dữ liệu (read-only).

### 7.2. Events Consumed (Tiêu thụ)

| Event Name           | Source                 | Handler                    | Action thực hiện                             |
| -------------------- | ---------------------- | -------------------------- | ---------------------------------------------- |
| `PaymentProcessed` | `ReceptionService`   | N/A (query trực tiếp DB) | Doanh thu được phản ánh qua DB query      |
| `FoodOrderCharged` | `FoodOrderService`   | N/A (query trực tiếp DB) | Doanh thu F&B được phản ánh qua DB query  |
| `TourBookingPaid`  | `TourBookingService` | N/A (query trực tiếp DB) | Doanh thu Tour được phản ánh qua DB query |

> Module này không consume events trực tiếp. Thay vào đó, nó query dữ liệu real-time từ database mỗi khi Dashboard được load hoặc Report được xuất.

## 8. Interface Specification (Đặc tả Giao diện)

### 8.1. Service Interface

```csharp
// IDashboardReportService.cs
// @version 1.0

namespace PhucNguyenResort.Services.Dashboard;

public interface IDashboardReportService
{
    /// <summary>
    /// UC22: Tính Tỷ lệ lấp đầy phòng theo tháng.
    /// </summary>
    /// <param name="month">Tháng (1-12)</param>
    /// <param name="year">Năm (VD: 2026)</param>
    /// <returns>OccupancyData chứa tỷ lệ tổng và dữ liệu theo ngày</returns>
    Task<OccupancyData> GetOccupancyDataAsync(int month, int year);

    /// <summary>
    /// UC23: Tính doanh thu chia theo 3 nguồn (Room, F&B, Tour).
    /// </summary>
    Task<RevenueBreakdownData> GetRevenueBreakdownAsync(int month, int year);

    /// <summary>
    /// UC44: Lấy báo cáo tài chính đầy đủ cho tháng.
    /// </summary>
    Task<FinancialReportViewModel> GetFinancialReportAsync(int month, int year);

    /// <summary>
    /// UC24: Xuất báo cáo tài chính sang Excel (.xlsx).
    /// </summary>
    Task<byte[]> ExportToExcelAsync(int month, int year);

    /// <summary>
    /// UC24: Xuất báo cáo tài chính sang PDF.
    /// </summary>
    Task<byte[]> ExportToPdfAsync(int month, int year);
}
```

## 9. API Specification (MVC Routes)

### 9.1. Endpoints Table

| Method | Route                                 | Auth Level  | Required Roles       | Description                                  |
| ------ | ------------------------------------- | ----------- | -------------------- | -------------------------------------------- |
| GET    | `/Dashboard/Manager`                | Cookie Auth | `Business Manager` | Hiển thị dashboard tổng quan + biểu đồ |
| GET    | `/Dashboard/FinancialReport?m&y`    | Cookie Auth | `Business Manager` | Hiển thị báo cáo tài chính chi tiết   |
| GET    | `/Dashboard/ExportExcel?month&year` | Cookie Auth | `Business Manager` | Tải file Excel báo cáo                    |
| GET    | `/Dashboard/ExportPdf?month&year`   | Cookie Auth | `Business Manager` | Tải file PDF báo cáo                      |

### 9.2. Request / Response

**GET `/Dashboard/Manager`**

- Input: Không (mặc định tháng/năm hiện tại)
- Output: Render View `Manager.cshtml` với `ManagerDashboardViewModel`

**GET `/Dashboard/FinancialReport?month=7&year=2026`**

- Input: `month` (int, optional), `year` (int, optional)
- Output: Render View `FinancialReport.cshtml` với `FinancialReportViewModel`

**GET `/Dashboard/ExportExcel?month=7&year=2026`**

- Input: `month` (int, required), `year` (int, required)
- Output: File download `.xlsx` — Content-Type: `application/vnd.openxmlformats-officedocument.spreadsheetml.sheet`

**GET `/Dashboard/ExportPdf?month=7&year=2026`**

- Input: `month` (int, required), `year` (int, required)
- Output: File download `.pdf` — Content-Type: `application/pdf`

## 10. Bảng mã lỗi (Error Codes)

| Code        | HTTP Status | Message (VI)                            | Trigger Condition                            |
| ----------- | ----------- | --------------------------------------- | -------------------------------------------- |
| `RPT-001` | 400         | Tháng không hợp lệ (phải từ 1-12) | month < 1 hoặc month > 12                   |
| `RPT-002` | 400         | Năm không hợp lệ                    | year < 2020 hoặc year > năm hiện tại + 1 |
| `RPT-003` | 500         | Lỗi khi tạo file Excel                | ClosedXML exception                          |
| `RPT-004` | 500         | Lỗi khi tạo file PDF                  | QuestPDF exception                           |
| `RPT-005` | 403         | Không đủ quyền truy cập            | User không có role Business Manager        |

## 11. Quy trình Triển khai (Step-by-Step)

### 11.1. Prerequisites

- [X] Models (GuestFolio, FolioTransaction, Booking, Room, FoodOrder, TourBooking, Payment, Invoice) đã tồn tại
- [X] Enums (BookingStatus, FolioTransactionType, RoomStatus, PaymentStatus) đã khai báo
- [X] Manager Dashboard View đang hoạt động cơ bản
- [ ] NuGet packages ClosedXML và QuestPDF đã được thêm

### 11.2. Implementation Steps

**Chặng 1 — Cài NuGet Packages**

```bash
cd PhucNguyenResort/PhucNguyenResort
dotnet add package ClosedXML --version 0.104.2
dotnet add package QuestPDF --version 2025.1.1
dotnet restore
```

**Chặng 2 — Tạo ViewModels**

- Tạo `ViewModels/Dashboard/ManagerDashboardViewModel.cs`
- Tạo `ViewModels/Dashboard/FinancialReportViewModel.cs`

**Chặng 3 — Tạo Service Layer**

- Tạo `Services/Dashboard/IDashboardReportService.cs`
- Tạo `Services/Dashboard/DashboardReportService.cs`:
  - `GetOccupancyDataAsync()`: Query Bookings + Rooms, tính % theo ngày
  - `GetRevenueBreakdownAsync()`: Query FolioTransactions + FoodOrders + Payments
  - `GetFinancialReportAsync()`: Gom tất cả dữ liệu vào FinancialReportViewModel
  - `ExportToExcelAsync()`: Dùng ClosedXML tạo workbook 3 sheets
  - `ExportToPdfAsync()`: Dùng QuestPDF tạo PDF document

**Chặng 4 — Register DI**

- Thêm `builder.Services.AddScoped<IDashboardReportService, DashboardReportService>();` trong `Program.cs`

**Chặng 5 — Cập nhật Controller**

- Inject `IDashboardReportService` vào `DashboardController`
- Cập nhật `Manager()` action dùng ManagerDashboardViewModel
- Thêm `FinancialReport()`, `ExportExcel()`, `ExportPdf()` actions

**Chặng 6 — Cập nhật Views**

- Sửa `Manager.cshtml`: Thêm Chart.js charts (Occupancy + Revenue)
- Tạo `FinancialReport.cshtml`: Trang báo cáo chi tiết + nút export

### 11.3. Deployment Checklist

- [ ] Build thành công (`dotnet build`)
- [ ] Dashboard hiển thị biểu đồ Occupancy Rate
- [ ] Dashboard hiển thị biểu đồ Revenue Breakdown (Room/F&B/Tour)
- [ ] Trang Financial Report hiển thị dữ liệu đúng
- [ ] Export Excel tải file .xlsx thành công với 3 sheets
- [ ] Export PDF tải file .pdf thành công với đầy đủ nội dung
- [ ] Tháng không có dữ liệu hiển thị 0% / 0 VND (không crash)

## 12. Rollback & Incident Runbook

### 12.1. Điều kiện kích hoạt Rollback

| Điều kiện                                          | Ngưỡng | Người quyết định |
| ----------------------------------------------------- | -------- | --------------------- |
| Dashboard crash khi load                              | 1 case   | Developer             |
| Occupancy Rate tính sai (> 100% hoặc < 0%)          | 1 case   | Developer             |
| Revenue total không khớp giữa Dashboard và Export | 1 case   | Developer             |
| Export file bị lỗi / corrupt                        | 1 case   | Developer             |

### 12.2. Rollback Procedure

```bash
# Bước 1: Revert code changes
git checkout -- PhucNguyenResort/Controllers/DashboardController.cs
git checkout -- PhucNguyenResort/Views/Dashboard/Manager.cshtml

# Bước 2: Xóa files mới tạo
del PhucNguyenResort/ViewModels/Dashboard/ManagerDashboardViewModel.cs
del PhucNguyenResort/ViewModels/Dashboard/FinancialReportViewModel.cs
del PhucNguyenResort/Services/Dashboard/IDashboardReportService.cs
del PhucNguyenResort/Services/Dashboard/DashboardReportService.cs
del PhucNguyenResort/Views/Dashboard/FinancialReport.cshtml

# Bước 3: Revert Program.cs (DI registration)
git checkout -- PhucNguyenResort/Program.cs

# Bước 4: Revert .csproj (NuGet packages)
git checkout -- PhucNguyenResort/PhucNguyenResort.Web.csproj
dotnet restore
```

## 13. Kịch bản Kiểm thử Chi tiết

### 13.1. Unit Tests

**TC-UNIT-001 — Occupancy Rate tính đúng %**

```gherkin
Feature: Room Occupancy Rate Calculation
  Background:
    Given test data classification: SYNTHETIC
    And Hệ thống có 10 phòng (không tính Maintenance)

  Scenario: 5/10 phòng có khách → 50%
    Given 5 Bookings ở trạng thái CheckedIn trong tháng 7/2026
    When GetOccupancyDataAsync(7, 2026) được gọi
    Then OccupancyRatePercent ≈ 50%

  Scenario: 0/10 phòng có khách → 0%
    Given Không có Booking nào trong tháng 8/2026
    When GetOccupancyDataAsync(8, 2026) được gọi
    Then OccupancyRatePercent = 0%

  Scenario: 10/10 phòng có khách → 100%
    Given 10 Bookings ở trạng thái CheckedIn
    When GetOccupancyDataAsync(7, 2026) được gọi
    Then OccupancyRatePercent = 100%
```

**TC-UNIT-002 — Revenue Breakdown tính đúng 3 nguồn**

```gherkin
Feature: Revenue Breakdown Calculation
  Background:
    Given test data classification: SYNTHETIC

  Scenario: Tính doanh thu tháng có đầy đủ 3 nguồn
    Given FolioTransactions trong tháng 7/2026:
      | TransactionType | Amount    |
      | RoomCharge      | 5,000,000 |
      | FoodCharge      | 1,500,000 |
      | TourCharge      | 800,000   |
    And FoodOrders (IsPaidAtRestaurant=true, PaidAt trong tháng 7):
      | TotalAmount |
      | 200,000     |
    When GetRevenueBreakdownAsync(7, 2026) được gọi
    Then RoomRevenue = 5,000,000
    And FnBRevenue = 1,700,000  (1,500,000 + 200,000)
    And TourRevenue = 800,000
    And TotalRevenue = 7,500,000

  Scenario: Tháng không có doanh thu
    Given Không có FolioTransactions trong tháng 8/2026
    When GetRevenueBreakdownAsync(8, 2026) được gọi
    Then TotalRevenue = 0
    And RoomRevenue = 0 And FnBRevenue = 0 And TourRevenue = 0
```

**TC-UNIT-003 — Excel export có đủ 3 sheets**

```gherkin
Feature: Excel Export
  Scenario: Xuất file Excel thành công
    Given Dữ liệu tháng 7/2026 đã được tổng hợp
    When ExportToExcelAsync(7, 2026) được gọi
    Then byte[] không null và length > 0
    And File Excel có 3 worksheets: "Summary", "Daily Occupancy", "Daily Revenue"
```

**TC-UNIT-004 — PDF export tạo file hợp lệ**

```gherkin
Feature: PDF Export
  Scenario: Xuất file PDF thành công
    Given Dữ liệu tháng 7/2026 đã được tổng hợp
    When ExportToPdfAsync(7, 2026) được gọi
    Then byte[] không null và length > 0
    And 4 bytes đầu = "%PDF" (PDF magic number)
```

### 13.2. Integration Tests

**TC-INT-001 — Full Dashboard flow**

```gherkin
  Scenario: Manager Dashboard load đầy đủ dữ liệu
    Given test data classification: SYNTHETIC
    And Database có:
      - 10 Rooms (8 Available, 1 Occupied, 1 Maintenance)
      - 3 Bookings (2 CheckedIn, 1 CheckedOut) trong tháng hiện tại
      - FolioTransactions cho mỗi Booking
    When GET /Dashboard/Manager
    Then Response status 200
    And Model chứa OccupancyData với TotalRooms > 0
    And Model chứa RevenueBreakdownData với values >= 0
```

### 13.3. E2E / Security Tests

**TC-E2E-001 — Unauthorized access bị chặn**

```gherkin
  Scenario: Customer không truy cập được Dashboard Manager
    Given User đăng nhập với role Customer
    When GET /Dashboard/Manager
    Then Response redirect về AccessDenied hoặc 403

  Scenario: Receptionist không truy cập được Financial Report
    Given User đăng nhập với role Receptionist
    When GET /Dashboard/FinancialReport
    Then Response redirect về AccessDenied hoặc 403
```

## 14. Phương pháp Xác minh

### 14.1. Database Inspection

```sql
-- Verify Occupancy calculation cho tháng 7/2026
-- Tổng số phòng khả dụng (không Maintenance)
SELECT COUNT(*) AS TotalAvailableRooms
FROM Rooms
WHERE Status != 2; -- 2 = Maintenance

-- Phòng đang occupied trong 1 ngày cụ thể (VD: 2026-07-04)
SELECT COUNT(DISTINCT bd.RoomId) AS OccupiedRooms
FROM Bookings b
INNER JOIN BookingDetails bd ON b.BookingId = bd.BookingId
WHERE b.BookingStatus IN (2, 3) -- CheckedIn, CheckedOut
  AND b.CheckInDate <= '2026-07-04'
  AND b.CheckOutDate > '2026-07-04'
  AND bd.RoomId IS NOT NULL;

-- Verify Revenue breakdown cho tháng 7/2026
SELECT TransactionType, SUM(ISNULL(Amount, 0)) AS Total
FROM FolioTransactions
WHERE CreatedAt >= '2026-07-01' AND CreatedAt < '2026-08-01'
GROUP BY TransactionType;

-- F&B paid at restaurant (không charge-to-room)
SELECT SUM(ISNULL(TotalAmount, 0)) AS FnBRestaurantRevenue
FROM FoodOrders
WHERE IsPaidAtRestaurant = 1
  AND IsChargedToRoom = 0
  AND PaidAt >= '2026-07-01' AND PaidAt < '2026-08-01';

-- Tour payments riêng
SELECT SUM(ISNULL(p.Amount, 0)) AS TourDirectPayment
FROM Payments p
WHERE p.TourBookingId IS NOT NULL
  AND p.PaymentStatus = 1 -- Success
  AND p.PaidAt >= '2026-07-01' AND p.PaidAt < '2026-08-01';
```

### 14.2. File Export Verification

```bash
# Verify Excel file không corrupt
# Mở file trong Excel hoặc LibreOffice, kiểm tra:
# 1. Sheet "Summary" có tổng doanh thu + occupancy rate
# 2. Sheet "Daily Occupancy" có dữ liệu theo ngày
# 3. Sheet "Daily Revenue" có Room/F&B/Tour theo ngày

# Verify PDF file
# Mở file PDF, kiểm tra:
# 1. Header có tên resort + tháng/năm
# 2. Bảng KPI tổng quan
# 3. Bảng chi tiết doanh thu
# 4. Bảng chi tiết lấp đầy phòng
```

## 15. Mẫu thử thực tế (Verification Samples)

### 15.1. Happy Path — Xem Dashboard

1. Đăng nhập với tài khoản Business Manager
2. Truy cập `/Dashboard/Manager`
3. Expected:
   - Biểu đồ tròn (Doughnut) hiển thị Occupancy Rate %
   - Biểu đồ đường hiển thị Occupancy theo ngày trong tháng
   - Biểu đồ tròn (Pie) hiển thị Revenue Breakdown: Room / F&B / Tour
   - 3 KPI cards hiển thị tổng doanh thu từng nguồn
   - Nút "Xuất Excel" và "Xuất PDF"

### 15.2. Happy Path — Xuất Excel

1. Tại Dashboard, chọn tháng 7/2026
2. Bấm nút **"Xuất Excel"**
3. Expected: Tải file `PhucNguyen_Financial_Report_2026_07.xlsx`
4. Mở file: 3 sheets với dữ liệu chính xác

### 15.3. Happy Path — Xuất PDF

1. Tại Dashboard, bấm nút **"Xuất PDF"**
2. Expected: Tải file `PhucNguyen_Financial_Report_2026_07.pdf`
3. Mở file: Layout báo cáo chuyên nghiệp

### 15.4. Edge Case — Tháng không có dữ liệu

1. Chọn tháng 12/2025 (trước khi hệ thống hoạt động)
2. Expected:
   - Occupancy Rate = 0%
   - Revenue = 0 VND cho cả 3 nguồn
   - Biểu đồ hiển thị rỗng nhưng KHÔNG crash
   - Export vẫn hoạt động (file rỗng data nhưng valid format)

## 16. Bảng tổng hợp phân quyền (Authorization Matrix)

| Route                             | Guest | Customer | Receptionist | F&B Staff | Kitchen Staff | Business Manager | Admin |
| --------------------------------- | ----- | -------- | ------------ | --------- | ------------- | ---------------- | ----- |
| GET`/Dashboard/Manager`         | ❌    | ❌       | ❌           | ❌        | ❌            | ✅               | ❌    |
| GET`/Dashboard/FinancialReport` | ❌    | ❌       | ❌           | ❌        | ❌            | ✅               | ❌    |
| GET`/Dashboard/ExportExcel`     | ❌    | ❌       | ❌           | ❌        | ❌            | ✅               | ❌    |
| GET`/Dashboard/ExportPdf`       | ❌    | ❌       | ❌           | ❌        | ❌            | ✅               | ❌    |

Chú thích:

- ✅ = Được phép
- ❌ = Bị từ chối (Redirect Login / 403)

---

## PHỤ LỤC

### A. Glossary (Thuật ngữ)

| Thuật ngữ       | Định nghĩa                                                                               |
| ----------------- | ------------------------------------------------------------------------------------------- |
| USALI             | Uniform System of Accounts for the Lodging Industry — chuẩn kế toán ngành khách sạn  |
| Occupancy Rate    | Tỷ lệ lấp đầy phòng = (Phòng có khách / Tổng phòng khả dụng) × 100%           |
| Revenue Breakdown | Phân tích doanh thu theo nguồn: Room (phòng), F&B (ẩm thực), Tour (chuyến tham quan) |
| GuestFolio        | Bảng kê tổng hợp chi phí khách hàng trong suốt thời gian lưu trú                 |
| FolioTransaction  | Dòng giao dịch chi tiết trong GuestFolio: tiền phòng, ăn uống, tour, phụ thu...     |
| Charge-to-Room    | Tính chi phí vào phòng khách — sẽ thanh toán khi checkout                           |
| Chart.js          | Thư viện JavaScript vẽ biểu đồ, dùng qua CDN                                         |
| ClosedXML         | Thư viện .NET xuất file Excel .xlsx                                                      |
| QuestPDF          | Thư viện .NET xuất file PDF với API fluent                                              |

### B. Tài liệu tham chiếu

| Document                | Link / Path                                          |
| ----------------------- | ---------------------------------------------------- |
| SRS — UC39, UC44, UC45 | `01_SRS/SWP391_NHOM1_V2.md`                        |
| Project Specification   | `01_SRS/SWP391-HOS-02.md`                          |
| Implementation Plan     | `template/UC22_UC23_UC24_Dashboard_Report_Plan.md` |
| Check-out Billing EDS   | `03_implement/EDS_CHECKOUT_BILLING.md`             |

> *EDS v2.0 — Áp dụng cho module Financial Report & Business Dashboard.*
