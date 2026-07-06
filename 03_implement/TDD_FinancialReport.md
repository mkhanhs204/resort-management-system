# TEST-DRIVEN DEVELOPMENT SPECIFICATION

Đặc tả Kiểm thử Hướng Phát triển — Module Financial Report & Business Dashboard

**Document ID**: SWP391-REPORT-TDD-001
**Version**: 1.0
**Date**: 2026-07-04
**Status**: Draft
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: Cuong
**Reviewed by**: Khanh
**DPO Sign-off**: N/A (module không xử lý PII)
**Approved by**: Cuong
**Classification**: Internal - Confidential

**References**:

- `01_SRS/SWP391_NHOM1_V2.md` — Functional requirements (UC39, UC44, UC45)
- `01_SRS/SWP391-HOS-02.md` — Project Specification (UC22, UC23, UC24, Module 5, USALI)
- `03_implement/EDS_FinancialReport.md` — Engineering Design Specification
- `template/UC22_UC23_UC24_Dashboard_Report_Plan.md` — Implementation Plan

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.
Thứ tự bắt buộc: viết test → chạy → xác nhận FAIL 🔴 → implement → PASS 🟢 → refactor 🔵.
Test data dùng dữ liệu SYNTHETIC. Không dùng PII thật.

## CHANGELOG

**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ.

| Ngày       | Người thực hiện | Nội dung thay đổi                                                           |
| ---------- | --------------- | ----------------------------------------------------------------------------- |
| 2026-07-04 | Cuong           | Khởi tạo tài liệu — TDD spec cho Financial Report & Business Dashboard |

---

## MỤC LỤC

1. Thông tin Module
2. Logic Issues Resolved
3. Test Design Specification (TDS)
4. Test Case Specification
5. Red-Green-Refactor Tracker
6. Entry / Exit Criteria
7. Rollback Plan

---

## 1. Thông tin Module

| Field                 | Value                                                                                                       |
| --------------------- | ----------------------------------------------------------------------------------------------------------- |
| Feature / Gap ID      | UC22, UC23, UC24 (HOS-02) / UC39, UC44, UC45 (SRS V2)                                                      |
| Module                | `Financial Report & Business Dashboard - Consolidated Billing & Dashboards`                                |
| Spec gốc             | `SWP391-REPORT-IMP-001` (EDS_FinancialReport.md)                                                          |
| Priority              | 🟠 P1                                                                                                       |
| Sprint                | S4 (2026-07-04 → 2026-07-18)                                                                               |
| Milestone             | M3 Alpha - 2026-07-11                                                                                       |
| Data Classification   | Internal (dữ liệu tài chính tổng hợp, không chứa PII)                                                   |
| Compliance Scope      | USALI (Uniform System of Accounts for the Lodging Industry)                                                  |
| Upstream Dependencies | `ReceptionService (GuestFolios), FoodOrderService (FoodOrders), TourBookingService (TourBookings/Payments)` |
| Downstream Consumers  | `Business Manager UI (Dashboard Charts, Financial Report View, Excel/PDF Export)`                           |

## 2. Logic Issues Resolved

| #  | Spec gốc (sai / thiếu)                                                                                           | Thực tế (schema / policy)                                                                                                     | Fix áp dụng trong test                                                                              |
| -- | ------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
| L1 | UC22 (HOS-02) chỉ nói "Room Occupancy Rate (%)" không chi tiết công thức                                      | USALI: Occupancy = (Room-nights sold / Room-nights available) × 100. Phòng Maintenance không tính vào available.           | Test phải verify phòng Maintenance bị loại khỏi TotalRooms                                        |
| L2 | UC23 (HOS-02) nói "Rooms vs F&B vs Tours" nhưng không nói nguồn dữ liệu cụ thể                               | F&B có 2 nguồn: FolioTransactions (charge-to-room) VÀ FoodOrders (IsPaidAtRestaurant). Tour cũng có 2 nguồn tương tự.    | Test phải verify FnBRevenue = SUM(FolioTxn.FoodCharge) + SUM(FoodOrders.IsPaidAtRestaurant, !Charged) |
| L3 | UC24 chỉ nói "export to Excel/PDF" không nói cấu trúc file                                                      | ADR-002: ClosedXML (3 sheets: Summary, Daily Occupancy, Daily Revenue) và QuestPDF                                          | Test phải verify file Excel có đúng 3 worksheets                                                   |
| L4 | SRS không đề cập trường hợp tháng không có dữ liệu                                                             | Occupancy = 0%, Revenue = 0 khi không có giao dịch. Biểu đồ hiển thị rỗng, export vẫn tạo file hợp lệ.                 | Test phải cover edge case: tháng rỗng → 0% / 0 VND, không exception                               |
| L5 | Manager Dashboard hiện tại dùng `FnBTableReservationDashboardViewModel` — không có Occupancy/Revenue             | Cần tạo `ManagerDashboardViewModel` mới chứa Occupancy + Revenue + existing KPIs. Phải backward-compatible Table Reservations | Test phải verify ManagerDashboardViewModel có cả Occupancy, Revenue VÀ Table Reservation data       |
| L6 | Spec không nói rõ FoodOrders đã charge-to-room CÓ được tính vào F&B Revenue hay không                          | FoodOrders đã charge-to-room SẼ xuất hiện trong FolioTransactions (FoodCharge). Nếu tính thêm từ FoodOrders → double-count | Test phải verify FoodOrders filter: `IsPaidAtRestaurant = true AND IsChargedToRoom = false`          |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi

```
Financial Report & Business Dashboard bao gồm các layer:
├── Service Layer (DashboardReportService - Business Logic)
│   ├── GetOccupancyDataAsync()     — Tính Occupancy Rate theo tháng
│   ├── GetRevenueBreakdownAsync()  — Tính Revenue theo 3 nguồn
│   ├── GetFinancialReportAsync()   — Gom tất cả data cho report
│   ├── ExportToExcelAsync()        — Xuất file Excel
│   └── ExportToPdfAsync()          — Xuất file PDF
├── Controller Layer (DashboardController)
│   ├── GET Manager()               — Render dashboard với charts
│   ├── GET FinancialReport(m?, y?) — Render trang report chi tiết
│   ├── GET ExportExcel(m, y)       — Download file Excel
│   └── GET ExportPdf(m, y)         — Download file PDF
├── ViewModel Layer
│   ├── ManagerDashboardViewModel   — Data cho Dashboard
│   └── FinancialReportViewModel    — Data cho Financial Report
└── View Layer (Manager.cshtml, FinancialReport.cshtml)
    └── Chart.js charts — Manual test
```

### TDS-02 — Test Basis / Cơ sở Kiểm thử

| Source                                       | Items Derived                                                                     |
| -------------------------------------------- | --------------------------------------------------------------------------------- |
| `SWP391_NHOM1_V2.md` UC39                  | View Business Dashboard — occupancy + revenue                                     |
| `SWP391_NHOM1_V2.md` UC44                  | View Financial Report — full report data                                           |
| `SWP391_NHOM1_V2.md` UC45                  | Export PDF/Excel Report                                                             |
| `SWP391-HOS-02.md` UC22                    | Room Occupancy Rate (%)                                                             |
| `SWP391-HOS-02.md` UC23                    | Revenue Dashboard: Rooms vs F&B vs Tours                                            |
| `SWP391-HOS-02.md` UC24                    | Export monthly financial report to Excel/PDF                                        |
| `SWP391-HOS-02.md` Section D (USALI)       | Revenue categories: Rooms Revenue, F&B Revenue, Tour Revenue                       |
| `SWP391-HOS-02.md` Night Audit Process     | Calculate occupancy rates, finalize unpaid bills                                   |
| `EDS_FinancialReport.md` ADR-001           | Nguồn dữ liệu: FolioTransactions + FoodOrders + Payments                        |
| `EDS_FinancialReport.md` ADR-002           | Libraries: ClosedXML (Excel) + QuestPDF (PDF)                                     |
| `UC22_UC23_UC24_Dashboard_Report_Plan.md`  | Công thức tính toán chi tiết: Occupancy, Revenue                                 |

### TDS-03 — Test Conditions and Coverage Items

| Condition ID | Test Condition                                                          | Coverage Item                                      | Test Cases                    |
| ------------ | ----------------------------------------------------------------------- | -------------------------------------------------- | ----------------------------- |
| TC-COND-001  | Occupancy Rate tính đúng % khi có bookings                            | `GetOccupancyDataAsync()`                         | `RPT-TC-001`                |
| TC-COND-002  | Occupancy Rate = 0% khi không có booking                              | `GetOccupancyDataAsync()`                         | `RPT-TC-002`                |
| TC-COND-003  | Phòng Maintenance bị loại khỏi TotalRooms                            | `GetOccupancyDataAsync()`                         | `RPT-TC-003`                |
| TC-COND-004  | Occupancy Rate không vượt 100%                                        | `GetOccupancyDataAsync()`                         | `RPT-TC-004`                |
| TC-COND-005  | DailyOccupancy trả về data cho từng ngày trong tháng                  | `GetOccupancyDataAsync()`                         | `RPT-TC-005`                |
| TC-COND-006  | Room Revenue = SUM(FolioTxn.RoomCharge)                                 | `GetRevenueBreakdownAsync()`                      | `RPT-TC-006`                |
| TC-COND-007  | F&B Revenue = FolioTxn.FoodCharge + FoodOrders.PaidAtRestaurant         | `GetRevenueBreakdownAsync()`                      | `RPT-TC-007`                |
| TC-COND-008  | Tour Revenue = FolioTxn.TourCharge + Payments(TourBookingId)            | `GetRevenueBreakdownAsync()`                      | `RPT-TC-008`                |
| TC-COND-009  | TotalRevenue = Room + F&B + Tour                                        | `GetRevenueBreakdownAsync()`                      | `RPT-TC-009`                |
| TC-COND-010  | Revenue = 0 khi tháng không có giao dịch                              | `GetRevenueBreakdownAsync()`                      | `RPT-TC-010`                |
| TC-COND-011  | Không double-count FoodOrders đã charge-to-room                       | `GetRevenueBreakdownAsync()`                      | `RPT-TC-011`                |
| TC-COND-012  | Percentage tính đúng (Room% + F&B% + Tour% = 100%)                     | `GetRevenueBreakdownAsync()`                      | `RPT-TC-012`                |
| TC-COND-013  | Excel export có đúng 3 worksheets                                      | `ExportToExcelAsync()`                            | `RPT-TC-013`                |
| TC-COND-014  | Excel export byte[] không null/empty                                    | `ExportToExcelAsync()`                            | `RPT-TC-013`                |
| TC-COND-015  | PDF export byte[] bắt đầu bằng "%PDF"                                 | `ExportToPdfAsync()`                              | `RPT-TC-014`                |
| TC-COND-016  | Financial Report ViewModel có đầy đủ data                              | `GetFinancialReportAsync()`                       | `RPT-TC-015`                |
| TC-COND-017  | Chỉ Business Manager truy cập được Dashboard/Report/Export             | Controller [Authorize]                              | `RPT-TC-SEC-001`            |
| TC-COND-018  | Input month/year validation (1-12, hợp lệ)                             | Controller parameter validation                     | `RPT-TC-SEC-002`            |

### TDS-04 — Test Techniques / Kỹ thuật Kiểm thử

| Technique (ISO 29119-4) | Applied To                                               | Rationale                                                   |
| ------------------------ | -------------------------------------------------------- | ----------------------------------------------------------- |
| Equivalence Partitioning | Occupancy Rate: 0%, 0-100%, 100%                         | 3 partitions: empty, normal, full                           |
| Boundary Value Analysis  | Month boundaries: 0, 1, 12, 13; Year: 2019, 2020, future | Ranh giới input validation                                 |
| State Transition Testing | BookingStatus → tính vào Occupancy (CheckedIn/CheckedOut) | Chỉ certain states count                                   |
| Error Guessing           | Tháng rỗng, 0 phòng, double-count FoodOrders             | Edge cases dễ gặp                                         |

### TDS-05 — Test Data Requirements

| Fixture ID | Type      | Value / Logic                                                                                                                    | Mục đích                                 |
| ---------- | --------- | -------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------- |
| FX-001     | DB seed   | `Room[] { 10 rooms: 8 Available, 1 Occupied, 1 Maintenance }`                                                                  | Base room data — TotalRooms = 9 (excl. Maintenance) |
| FX-002     | DB seed   | `Booking { Id=1, Status=CheckedIn, CheckIn=2026-07-01, CheckOut=2026-07-05 }`                                                  | Active booking for occupancy               |
| FX-003     | DB seed   | `Booking { Id=2, Status=CheckedOut, CheckIn=2026-07-02, CheckOut=2026-07-04 }`                                                 | Completed booking for occupancy            |
| FX-004     | DB seed   | `Booking { Id=3, Status=Cancelled, CheckIn=2026-07-03, CheckOut=2026-07-06 }`                                                  | Cancelled — should NOT count               |
| FX-005     | DB seed   | `FolioTransaction { FolioId=1, Type=RoomCharge, Amount=2000000, CreatedAt=2026-07-01 }`                                        | Room revenue                                |
| FX-006     | DB seed   | `FolioTransaction { FolioId=1, Type=FoodCharge, Amount=500000, CreatedAt=2026-07-02 }`                                         | F&B revenue (charge-to-room)               |
| FX-007     | DB seed   | `FolioTransaction { FolioId=1, Type=TourCharge, Amount=300000, CreatedAt=2026-07-03 }`                                         | Tour revenue (charge-to-room)              |
| FX-008     | DB seed   | `FoodOrder { Id=1, IsPaidAtRestaurant=true, IsChargedToRoom=false, TotalAmount=200000, PaidAt=2026-07-02 }`                    | F&B restaurant revenue (NOT charge-to-room) |
| FX-009     | DB seed   | `FoodOrder { Id=2, IsChargedToRoom=true, IsPaidAtRestaurant=false, TotalAmount=500000, ChargedAt=2026-07-02 }`                 | F&B already in FolioTxn — must NOT double-count |
| FX-010     | DB seed   | `Payment { Id=1, TourBookingId=1, Amount=150000, PaymentStatus=Success, PaidAt=2026-07-03 }`                                   | Tour direct payment                         |
| FX-011     | DB seed   | `BookingDetail { BookingId=1, RoomId=1 }, BookingDetail { BookingId=2, RoomId=2 }`                                             | Room assignments for occupancy calc        |

## 4. Test Case Specification

---

### RPT-TC-001 — Occupancy Rate tính đúng % khi có bookings

**Severity**: CRITICAL
**Legal**: `USALI Occupancy Rate Standard`
**Feature Under Test**: `DashboardReportService.GetOccupancyDataAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED — chưa implement
**Condition Ref**: TC-COND-001

**Preconditions**:

- 10 phòng trong hệ thống: 9 khả dụng, 1 Maintenance (FX-001)
- 2 Bookings active: CheckedIn + CheckedOut trong tháng 7/2026 (FX-002, FX-003)
- BookingDetails trỏ đến Room 1 và Room 2 (FX-011)

**Test Steps**:

1. Arrange: Seed 10 rooms + 2 bookings + booking details
2. Act: Gọi `GetOccupancyDataAsync(7, 2026)`
3. Assert: OccupancyRatePercent cho ngày 2026-07-02 ≈ 22.2% (2 phòng occupied / 9 phòng available)

**Expected Result (PASS — hành vi đúng)**:

- OccupancyRatePercent > 0 cho các ngày có booking
- TotalRooms = 9 (loại phòng Maintenance)
- DailyData có entries cho từng ngày trong tháng 7

**Expected Result (FAIL — dấu hiệu lỗi)**:

- OccupancyRatePercent = 0 dù có bookings → logic query sai
- TotalRooms = 10 (đếm cả Maintenance) → filter sai

**Current Status**: 🔴 Not written
**Implementation Note**: Filter Rooms WHERE Status != RoomStatus.Maintenance. Count Bookings WHERE Status IN (CheckedIn, CheckedOut).

---

### RPT-TC-002 — Occupancy Rate = 0% khi không có booking

**Severity**: HIGH
**Feature Under Test**: `DashboardReportService.GetOccupancyDataAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-002

**Preconditions**:

- 10 phòng trong hệ thống (FX-001)
- Không có Booking nào trong tháng 8/2026

**Test Steps**:

1. Arrange: Seed rooms, no bookings for month 8
2. Act: Gọi `GetOccupancyDataAsync(8, 2026)`
3. Assert: OccupancyRatePercent = 0

**Expected Result (PASS)**: Rate = 0%, DailyData có entries nhưng tất cả OccupiedRooms = 0
**Expected Result (FAIL)**: Exception (division by zero) hoặc NaN

**Current Status**: 🔴 Not written
**Implementation Note**: Guard clause: if TotalRooms = 0 → return 0%. Dùng conditional: `occupiedRooms > 0 ? (double)occupied / total * 100 : 0`

---

### RPT-TC-003 — Phòng Maintenance bị loại khỏi TotalRooms

**Severity**: HIGH
**Feature Under Test**: `DashboardReportService.GetOccupancyDataAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-003

**Preconditions**:

- 10 phòng: 9 available/occupied, 1 Maintenance (FX-001)

**Test Steps**:

1. Arrange: Seed 10 rooms, 1 in Maintenance
2. Act: Gọi `GetOccupancyDataAsync(7, 2026)`
3. Assert: result.TotalRooms = 9

**Expected Result (PASS)**: TotalRooms = 9 (không đếm Maintenance)
**Expected Result (FAIL)**: TotalRooms = 10 → phòng Maintenance bị đếm sai

**Current Status**: 🔴 Not written

---

### RPT-TC-004 — Occupancy Rate không vượt 100%

**Severity**: HIGH
**Feature Under Test**: `DashboardReportService.GetOccupancyDataAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-004

**Preconditions**:

- 5 phòng, 5 bookings (tất cả CheckedIn) cho cùng ngày

**Test Steps**:

1. Arrange: Seed 5 rooms + 5 bookings, mỗi booking 1 room
2. Act: Gọi `GetOccupancyDataAsync(7, 2026)`
3. Assert: OccupancyRatePercent <= 100

**Expected Result (PASS)**: Rate = 100%, không vượt quá
**Expected Result (FAIL)**: Rate > 100% (double-count rooms hoặc logic sai)

**Current Status**: 🔴 Not written

---

### RPT-TC-005 — DailyOccupancy trả về data cho từng ngày trong tháng

**Severity**: MEDIUM
**Feature Under Test**: `DashboardReportService.GetOccupancyDataAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-005

**Preconditions**:

- Tháng 7/2026 (31 ngày), nhưng chỉ tính đến ngày hiện tại nếu là tháng hiện hành

**Test Steps**:

1. Arrange: Seed rooms + bookings
2. Act: Gọi `GetOccupancyDataAsync(7, 2026)`
3. Assert: DailyData.Count = số ngày trong tháng (hoặc đến ngày hiện tại)

**Expected Result (PASS)**: DailyData chứa entries cho mỗi ngày, mỗi entry có Date, OccupiedRooms, TotalRooms, %
**Expected Result (FAIL)**: DailyData rỗng hoặc thiếu ngày

**Current Status**: 🔴 Not written

---

### RPT-TC-006 — Room Revenue = SUM(FolioTransactions.RoomCharge)

**Severity**: CRITICAL
**Legal**: `USALI Revenue Categories`
**Feature Under Test**: `DashboardReportService.GetRevenueBreakdownAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-006

**Preconditions**:

- FolioTransaction Type=RoomCharge, Amount=2,000,000, CreatedAt trong tháng 7/2026 (FX-005)

**Test Steps**:

1. Arrange: Seed FolioTransaction (RoomCharge = 2,000,000)
2. Act: Gọi `GetRevenueBreakdownAsync(7, 2026)`
3. Assert: result.RoomRevenue = 2,000,000

**Expected Result (PASS)**: RoomRevenue = 2,000,000
**Expected Result (FAIL)**: RoomRevenue = 0 hoặc sai số

**Current Status**: 🔴 Not written

---

### RPT-TC-007 — F&B Revenue gồm cả FolioTxn và FoodOrders paid tại nhà hàng

**Severity**: CRITICAL
**Legal**: `USALI Revenue Categories, ADR-001`
**Feature Under Test**: `DashboardReportService.GetRevenueBreakdownAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-007

**Preconditions**:

- FolioTransaction Type=FoodCharge, Amount=500,000 (FX-006)
- FoodOrder IsPaidAtRestaurant=true, IsChargedToRoom=false, TotalAmount=200,000 (FX-008)

**Test Steps**:

1. Arrange: Seed FolioTransaction + FoodOrder
2. Act: Gọi `GetRevenueBreakdownAsync(7, 2026)`
3. Assert: result.FnBRevenue = 700,000 (500,000 + 200,000)

**Expected Result (PASS)**: FnBRevenue = 700,000
**Expected Result (FAIL)**: FnBRevenue = 500,000 (bỏ sót restaurant) hoặc 1,200,000 (double-count)

**Current Status**: 🔴 Not written

---

### RPT-TC-008 — Tour Revenue gồm FolioTxn và Payments trực tiếp

**Severity**: CRITICAL
**Legal**: `USALI Revenue Categories, ADR-001`
**Feature Under Test**: `DashboardReportService.GetRevenueBreakdownAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-008

**Preconditions**:

- FolioTransaction Type=TourCharge, Amount=300,000 (FX-007)
- Payment TourBookingId=1, Amount=150,000, PaymentStatus=Success (FX-010)

**Test Steps**:

1. Arrange: Seed FolioTransaction + Payment
2. Act: Gọi `GetRevenueBreakdownAsync(7, 2026)`
3. Assert: result.TourRevenue = 450,000 (300,000 + 150,000)

**Expected Result (PASS)**: TourRevenue = 450,000
**Expected Result (FAIL)**: TourRevenue = 300,000 (bỏ sót direct payment)

**Current Status**: 🔴 Not written

---

### RPT-TC-009 — TotalRevenue = Room + F&B + Tour

**Severity**: CRITICAL
**Feature Under Test**: `DashboardReportService.GetRevenueBreakdownAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-009

**Preconditions**:

- Tất cả fixtures FX-005 đến FX-010

**Test Steps**:

1. Arrange: Seed đầy đủ FolioTransactions + FoodOrders + Payments
2. Act: Gọi `GetRevenueBreakdownAsync(7, 2026)`
3. Assert: result.TotalRevenue = result.RoomRevenue + result.FnBRevenue + result.TourRevenue

**Expected Result (PASS)**: TotalRevenue = 3,150,000 (2,000,000 + 700,000 + 450,000)
**Expected Result (FAIL)**: Tổng không bằng tổng 3 nguồn

**Current Status**: 🔴 Not written

---

### RPT-TC-010 — Revenue = 0 khi tháng không có giao dịch

**Severity**: HIGH
**Feature Under Test**: `DashboardReportService.GetRevenueBreakdownAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-010

**Preconditions**:

- Không có FolioTransactions/FoodOrders/Payments trong tháng 8/2026

**Test Steps**:

1. Arrange: Seed data chỉ cho tháng 7, query tháng 8
2. Act: Gọi `GetRevenueBreakdownAsync(8, 2026)`
3. Assert: TotalRevenue = 0, RoomRevenue = 0, FnBRevenue = 0, TourRevenue = 0

**Expected Result (PASS)**: Tất cả = 0, không exception
**Expected Result (FAIL)**: Exception hoặc NaN/negative values

**Current Status**: 🔴 Not written

---

### RPT-TC-011 — Không double-count FoodOrders đã charge-to-room

**Severity**: CRITICAL
**Legal**: `ADR-001 (EDS_FinancialReport.md)`
**Feature Under Test**: `DashboardReportService.GetRevenueBreakdownAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-011

**Preconditions**:

- FolioTransaction Type=FoodCharge, Amount=500,000 (FX-006) — đây là charge-to-room
- FoodOrder Id=2, IsChargedToRoom=true, TotalAmount=500,000 (FX-009) — cùng order
- FoodOrder Id=1, IsPaidAtRestaurant=true, IsChargedToRoom=false, TotalAmount=200,000 (FX-008)

**Test Steps**:

1. Arrange: Seed FolioTransaction + cả 2 FoodOrders
2. Act: Gọi `GetRevenueBreakdownAsync(7, 2026)`
3. Assert: FnBRevenue = 700,000 (500,000 folio + 200,000 restaurant) — KHÔNG phải 1,200,000

**Expected Result (PASS)**: FnBRevenue = 700,000 — FoodOrder Id=2 KHÔNG được đếm lại
**Expected Result (FAIL)**: FnBRevenue = 1,200,000 — double-count do không filter IsChargedToRoom

**Current Status**: 🔴 Not written
**Implementation Note**: Filter FoodOrders: `WHERE IsPaidAtRestaurant = true AND IsChargedToRoom = false`

---

### RPT-TC-012 — Percentage tính đúng (Room% + F&B% + Tour% ≈ 100%)

**Severity**: MEDIUM
**Feature Under Test**: `DashboardReportService.GetRevenueBreakdownAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-012

**Preconditions**:

- Có revenue từ cả 3 nguồn (FX-005 đến FX-010)

**Test Steps**:

1. Arrange: Seed data
2. Act: Gọi `GetRevenueBreakdownAsync(7, 2026)`
3. Assert: Math.Abs(RoomPercentage + FnBPercentage + TourPercentage - 100) < 1.0

**Expected Result (PASS)**: Tổng 3 tỷ lệ ≈ 100% (cho phép sai số do rounding)
**Expected Result (FAIL)**: Tổng ≠ 100%

**Current Status**: 🔴 Not written

---

### RPT-TC-013 — Excel export có đúng 3 worksheets và byte[] hợp lệ

**Severity**: HIGH
**Legal**: `UC24 / UC45`
**Feature Under Test**: `DashboardReportService.ExportToExcelAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-013, TC-COND-014

**Preconditions**:

- Dữ liệu tháng 7/2026 đã được seed (FX-001 đến FX-011)

**Test Steps**:

1. Arrange: Seed data
2. Act: Gọi `ExportToExcelAsync(7, 2026)`
3. Assert:
   - byte[] IS NOT NULL
   - byte[].Length > 0
   - Mở bằng ClosedXML: workbook có 3 worksheets: "Summary", "Daily Occupancy", "Daily Revenue"

**Expected Result (PASS)**: File Excel hợp lệ với 3 sheets
**Expected Result (FAIL)**: byte[] null, hoặc workbook corrupt, hoặc thiếu sheet

**Current Status**: 🔴 Not written

---

### RPT-TC-014 — PDF export tạo file PDF hợp lệ

**Severity**: HIGH
**Legal**: `UC24 / UC45`
**Feature Under Test**: `DashboardReportService.ExportToPdfAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-015

**Preconditions**:

- Dữ liệu tháng 7/2026 đã được seed

**Test Steps**:

1. Arrange: Seed data
2. Act: Gọi `ExportToPdfAsync(7, 2026)`
3. Assert:
   - byte[] IS NOT NULL
   - byte[].Length > 0
   - byte[0..3] = { 0x25, 0x50, 0x44, 0x46 } (%PDF — magic number)

**Expected Result (PASS)**: File PDF hợp lệ (starts with %PDF)
**Expected Result (FAIL)**: byte[] null hoặc không phải PDF format

**Current Status**: 🔴 Not written

---

### RPT-TC-015 — Financial Report ViewModel có đầy đủ data

**Severity**: HIGH
**Feature Under Test**: `DashboardReportService.GetFinancialReportAsync()`
**Test File**: `Tests/Services/DashboardReportServiceTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-016

**Preconditions**:

- Dữ liệu tháng 7/2026 đã được seed đầy đủ

**Test Steps**:

1. Arrange: Seed full data (FX-001 đến FX-011)
2. Act: Gọi `GetFinancialReportAsync(7, 2026)`
3. Assert:
   - Month = 7, Year = 2026
   - TotalRevenue > 0
   - TotalRoomRevenue > 0
   - TotalFnBRevenue > 0
   - TotalTourRevenue > 0
   - AverageOccupancyRate >= 0 AND <= 100
   - TotalBookings >= 0
   - DailyRevenues.Count > 0
   - DailyOccupancies.Count > 0

**Expected Result (PASS)**: ViewModel chứa đầy đủ dữ liệu hợp lệ
**Expected Result (FAIL)**: Any field = 0 khi có data hoặc null

**Current Status**: 🔴 Not written

---

### SECURITY TEST CASES

#### RPT-TC-SEC-001 — Unauthorized access bị chặn

**Severity**: CRITICAL
**OWASP**: A01:2021 — Broken Access Control
**CWE**: CWE-862 — Missing Authorization
**Feature Under Test**: `DashboardController` — [Authorize(Roles = "Business Manager")]
**Test File**: `Tests/Controllers/DashboardControllerReportTests.cs`
**TDD Phase**: 🔴 RED

**Preconditions**:

- User đăng nhập với role KHÔNG phải Business Manager (VD: Customer, Receptionist)

**Test Steps (Attack Simulation)**:

1. Đăng nhập với tài khoản Customer
2. Truy cập GET `/Dashboard/Manager`
3. Truy cập GET `/Dashboard/FinancialReport`
4. Truy cập GET `/Dashboard/ExportExcel?month=7&year=2026`
5. Kiểm tra response

**Expected Result (PASS = hệ thống an toàn)**:

- Response 403 Forbidden hoặc Redirect về Access Denied cho TẤT CẢ routes

**Expected Result (FAIL = lỗ hổng tồn tại)**:

- Non-Manager truy cập được dữ liệu tài chính → vi phạm RBAC

**Current Status**: 🔴 Not written

---

#### RPT-TC-SEC-002 — Input validation cho month/year

**Severity**: MEDIUM
**OWASP**: A03:2021 — Injection
**CWE**: CWE-20 — Improper Input Validation
**Feature Under Test**: `DashboardController.FinancialReport()`, `ExportExcel()`, `ExportPdf()`
**Test File**: `Tests/Controllers/DashboardControllerReportTests.cs`
**TDD Phase**: 🔴 RED

**Preconditions**:

- User đăng nhập với role Business Manager

**Test Steps**:

1. Gọi GET `/Dashboard/FinancialReport?month=0&year=2026` → invalid month
2. Gọi GET `/Dashboard/FinancialReport?month=13&year=2026` → invalid month
3. Gọi GET `/Dashboard/ExportExcel?month=7&year=1900` → invalid year

**Expected Result (PASS)**:

- Response trả về error message hoặc redirect, không crash

**Expected Result (FAIL)**:

- Unhandled exception hoặc server error 500

**Current Status**: 🔴 Not written

---

### INTEGRATION TEST CASES

#### RPT-TC-INT-001 — Full Dashboard + Export flow end-to-end

**Severity**: CRITICAL
**Feature Under Test**: Full flow: `GetOccupancyData → GetRevenueBreakdown → ExportToExcel → ExportToPdf`
**Test File**: `Tests/Integration/DashboardReportIntegrationTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-001 → TC-COND-015

**Preconditions**:

- SQL Server database running
- Seed: Rooms, Bookings (CheckedIn + CheckedOut), BookingDetails, GuestFolios, FolioTransactions, FoodOrders, Payments

**Test Steps**:

1. Seed full data (FX-001 through FX-011)
2. Call `GetOccupancyDataAsync(7, 2026)` → verify OccupancyRatePercent > 0
3. Call `GetRevenueBreakdownAsync(7, 2026)` → verify RoomRevenue + FnBRevenue + TourRevenue = TotalRevenue
4. Call `GetFinancialReportAsync(7, 2026)` → verify ViewModel complete
5. Call `ExportToExcelAsync(7, 2026)` → verify byte[] valid
6. Call `ExportToPdfAsync(7, 2026)` → verify byte[] valid

**Expected Result (PASS)**:

- Occupancy rate > 0% cho tháng có data
- Revenue breakdown: 3 nguồn cộng lại = total
- Excel file có 3 sheets
- PDF file starts with %PDF
- Tất cả data consistent giữa Dashboard và Export

**Expected Result (FAIL)**:

- Any inconsistency giữa Dashboard data và Export data → integration bug
- Export file corrupt

**DB Assertion**:

```csharp
var occupancy = await _service.GetOccupancyDataAsync(7, 2026);
Assert.True(occupancy.TotalRooms > 0);
Assert.True(occupancy.OccupancyRatePercent >= 0);
Assert.True(occupancy.OccupancyRatePercent <= 100);

var revenue = await _service.GetRevenueBreakdownAsync(7, 2026);
Assert.Equal(revenue.TotalRevenue, revenue.RoomRevenue + revenue.FnBRevenue + revenue.TourRevenue);
Assert.True(revenue.TotalRevenue >= 0);

var report = await _service.GetFinancialReportAsync(7, 2026);
Assert.Equal(7, report.Month);
Assert.Equal(2026, report.Year);
Assert.Equal(revenue.TotalRevenue, report.TotalRevenue);

var excel = await _service.ExportToExcelAsync(7, 2026);
Assert.NotNull(excel);
Assert.True(excel.Length > 0);

var pdf = await _service.ExportToPdfAsync(7, 2026);
Assert.NotNull(pdf);
Assert.True(pdf.Length > 0);
Assert.Equal(0x25, pdf[0]); // '%'
Assert.Equal(0x50, pdf[1]); // 'P'
Assert.Equal(0x44, pdf[2]); // 'D'
Assert.Equal(0x46, pdf[3]); // 'F'
```

**Current Status**: 🔴 Not written

## 5. Red-Green-Refactor Tracker

| TC ID                | Test File                                                      | 🔴 RED confirmed | 🟢 GREEN (commit) | 🔵 REFACTOR note |
| -------------------- | -------------------------------------------------------------- | ---------------- | ------------------ | ----------------- |
| `RPT-TC-001`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-002`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-003`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-004`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-005`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-006`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-007`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-008`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-009`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-010`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-011`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-012`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-013`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-014`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-015`        | `Tests/Services/DashboardReportServiceTests.cs`               | `[x]`           | `[x]`       | —                |
| `RPT-TC-SEC-001`    | `Tests/Controllers/DashboardControllerReportTests.cs`         | `[x]`           | `[x]`       | —                |
| `RPT-TC-SEC-002`    | `Tests/Controllers/DashboardControllerReportTests.cs`         | `[x]`           | `[x]`       | —                |
| `RPT-TC-INT-001`    | `Tests/Integration/DashboardReportIntegrationTests.cs`        | `[x]`           | `[x]`       | —                |

## 6. Entry / Exit Criteria

**Entry Criteria (Điều kiện bắt đầu)**

- [x] SRS (UC39, UC44, UC45 / UC22, UC23, UC24) đã được review
- [x] EDS `SWP391-REPORT-IMP-001` đã được tạo
- [x] Schema hiện tại đã có đủ Models: Room, Booking, BookingDetail, GuestFolio, FolioTransaction, FoodOrder, TourBooking, Payment, Invoice
- [x] Enums đã khai báo: BookingStatus, RoomStatus, FolioTransactionType, PaymentStatus
- [x] Module Check-in/Check-out đang hoạt động (upstream dependency cho GuestFolio data)
- [ ] NuGet packages ClosedXML + QuestPDF đã được thêm vào project
- [ ] Test fixtures (Section 3 TDS-05) đã được chuẩn bị

**Exit Criteria (Điều kiện kết thúc — DoD)**

- [ ] Tất cả 18 test cases đều PASS 🟢
- [ ] Build thành công (`dotnet build`)
- [ ] Occupancy Rate luôn trong khoảng 0% - 100%
- [ ] Revenue breakdown: Room + F&B + Tour = Total (không double-count)
- [ ] Export Excel tạo file .xlsx hợp lệ với 3 sheets
- [ ] Export PDF tạo file .pdf hợp lệ
- [ ] Chỉ Business Manager mới truy cập được (Authorization verified)
- [ ] Tháng không có dữ liệu hiển thị 0% / 0 VND (không crash)

**Suspension Criteria (Điều kiện tạm dừng)**

- Module Check-out bị regression (upstream dependency cho GuestFolio/FolioTransaction data)
- NuGet packages ClosedXML / QuestPDF không tương thích với .NET 8
- CI pipeline bị broken bởi thay đổi khác

## 7. Rollback Plan

```bash
# Revert implementation files
git checkout -- PhucNguyenResort/Controllers/DashboardController.cs
git checkout -- PhucNguyenResort/Views/Dashboard/Manager.cshtml
git checkout -- PhucNguyenResort/Program.cs
git checkout -- PhucNguyenResort/PhucNguyenResort.Web.csproj

# Remove new files
del PhucNguyenResort/ViewModels/Dashboard/ManagerDashboardViewModel.cs
del PhucNguyenResort/ViewModels/Dashboard/FinancialReportViewModel.cs
del PhucNguyenResort/Services/Dashboard/IDashboardReportService.cs
del PhucNguyenResort/Services/Dashboard/DashboardReportService.cs
del PhucNguyenResort/Views/Dashboard/FinancialReport.cshtml

# Remove test files
del PhucNguyenResort.Tests/Services/DashboardReportServiceTests.cs
del PhucNguyenResort.Tests/Controllers/DashboardControllerReportTests.cs
del PhucNguyenResort.Tests/Integration/DashboardReportIntegrationTests.cs

# Restore packages
dotnet restore
```
