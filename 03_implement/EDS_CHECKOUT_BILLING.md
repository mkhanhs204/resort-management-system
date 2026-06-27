# ENGINEERING DOCUMENTATION STANDARD (EDS) v2.0

Đặc tả Kỹ thuật Hiện thực hóa — Module Check-out & Consolidated Billing

| Field          | Value                      |
| -------------- | -------------------------- |
| Document ID    | `SWP391-BILLING-IMP-001` |
| Version        | 1.0                        |
| Date           | 2026-06-25                 |
| Status         | Approve                    |
| Document Owner | Cuong                      |
| Author         | Cuong                      |
| Reviewed by    | Khanh                      |
| DPO Sign-off   | Khanh                      |
| Approved by    | Cuong                      |
| Last Review    | 2026-06-25                 |
| Based on EDS   | v2.0                       |

## CHANGELOG

**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ. Mọi thay đổi phải ghi vào bảng này.

| Ngày      | Người thực hiện | Nội dung thay đổi       |
| ---------- | ------------------- | -------------------------- |
| 2026-06-25 | Cuong               | Tạo tài liệu lần đầu |

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

Module **Check-out & Consolidated Billing** chịu trách nhiệm tổng hợp toàn bộ chi phí phát sinh trong thời gian lưu trú của khách (tiền phòng, tiền ăn uống, tiền tour), tính phụ thu trả phòng muộn (Late Check-out Surcharge), sinh Hóa đơn tổng (Consolidated Invoice), xử lý thanh toán cuối cùng (Final Payment), và hoàn tất quy trình Check-out.

| Field                 | Value                                                                               |
| --------------------- | ----------------------------------------------------------------------------------- |
| Module Name           | `Check-out & Consolidated Billing`                                                |
| Bounded Context       | `Front Desk / Billing`                                                            |
| Data Classification   | PII (Tên khách, CCCD/Passport hiển thị trên Invoice)                           |
| Compliance Scope      | Luật Cư trú 2020, NĐ 356/2025 (PDPD)                                            |
| Upstream Dependencies | `ReceptionService (Check-in), FoodOrderService (F&B), TourBookingService (Tours)` |
| Downstream Consumers  | `Housekeeping (Room → NeedsCleaning), Dashboard (Revenue Reports)`               |

## 2. Ma trận Truy vết (Traceability Matrix)

| Requirement ID       | Loại (BR/UC) | Mô tả yêu cầu                                                                                                                       | Thành phần Code                                    | Compliance Target | ADR liên quan |
| -------------------- | ------------- | --------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------- | ----------------- | -------------- |
| UC53                 | Use Case      | Perform Customer Check-out: Lễ tân thực hiện trả phòng sau khi tất cả chi phí đã được xem xét và thanh toán            | `ReceptionService.ProcessCheckOutAsync()`          | —                | ADR-001        |
| UC54                 | Use Case      | Generate Consolidated Invoice: Hệ thống tự động sinh hóa đơn tổng bao gồm Room Cost, Unpaid Food Orders, Unpaid Tour Bookings | `ReceptionService.PrepareCheckOutViewModelAsync()` | —                | ADR-001        |
| UC55                 | Use Case      | Process Final Payment: Lễ tân xử lý thanh toán cuối cùng (Cash/Credit) và đánh dấu Invoice đã thanh toán                  | `ReceptionService.ProcessCheckOutAsync()`          | —                | ADR-002        |
| UC56                 | Use Case      | Update Room Operational Status: Sau checkout, phòng chuyển sang NeedsCleaning                                                         | `ReceptionService.ProcessCheckOutAsync()`          | —                | —             |
| BR-UC53-01           | Business Rule | Khách không thể checkout nếu Invoice chưa được thanh toán                                                                      | `ProcessCheckOutAsync()` — validate payment       | —                | —             |
| BR-UC53-02           | Business Rule | Tất cả phí Room, Food, Tour chưa thanh toán phải được gom vào Consolidated Invoice                                            | `PrepareCheckOutViewModelAsync()`                  | —                | —             |
| BR-UC53-03           | Business Rule | Guest Folio phải được đóng (Paid) sau khi checkout thành công                                                                   | `ProcessCheckOutAsync()`                           | —                | —             |
| BR-UC53-04           | Business Rule | Trạng thái phòng phải chuyển NeedsCleaning sau checkout                                                                            | `ProcessCheckOutAsync()`                           | —                | —             |
| BR02 (SWP391-HOS-02) | Business Rule | Consolidated Billing Constraint: Khách không được checkout nếu có Unpaid food orders hoặc Unpaid tour bookings                  | `PrepareCheckOutViewModelAsync()` — gom charge    | —                | —             |

## 3. Architecture Decision Records (ADR)

### ADR-001 — Tổng hợp chi phí qua GuestFolio + FolioTransactions

| Field      | Value                 |
| ---------- | --------------------- |
| Status     | Accepted              |
| Deciders   | `Cuong - Developer` |
| Date       | 2026-06-25            |
| Supersedes | N/A                   |

**Bối cảnh (Context)**
Hệ thống cần tổng hợp chi phí từ nhiều nguồn (Room, F&B, Tour) vào một hóa đơn duy nhất tại thời điểm Check-out. Database đã có sẵn `GuestFolio` (1:1 với Booking) và `FolioTransactions` (append-only log ghi từng khoản phí).

**Các phương án đã xem xét (Options Considered)**

| Phương án | Mô tả                                                                                                                               | Ưu điểm                                      | Nhược điểm                       |
| ------------ | ------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------- | ------------------------------------ |
| A            | Tính toán realtime bằng cách SUM tất cả FolioTransactions tại thời điểm checkout                                            | + Luôn chính xác, + Không cần sync         | - Chậm nếu có nhiều transactions |
| B            | Dùng cached totals trên GuestFolio (TotalRoomCharges, TotalFoodCharges, TotalTourCharges) và cập nhật mỗi khi thêm transaction | + Nhanh khi đọc, + Đã có sẵn trong schema | - Cần đảm bảo consistency        |

**Quyết định (Decision)**
Chọn Phương án B vì schema `GuestFolio` đã có sẵn các field `TotalRoomCharges`, `TotalFoodCharges`, `TotalTourCharges`, `TotalAmount`. Khi checkout, hệ thống sẽ đọc trực tiếp từ GuestFolio và bổ sung thêm phụ thu Late Check-out nếu có.

**Hệ quả (Consequences)**
Tích cực:

- Tốc độ query nhanh, chỉ cần 1 bản ghi GuestFolio
- Tận dụng được cấu trúc dữ liệu hiện tại

Tiêu cực / Trade-offs:

- Cần đảm bảo mỗi khi thêm FolioTransaction đều cập nhật totals trên GuestFolio — đã được xử lý trong logic hiện tại của ProcessCheckInAsync.

---

### ADR-002 — Phương thức thanh toán tại quầy (Offline Payment)

| Field      | Value                 |
| ---------- | --------------------- |
| Status     | Accepted              |
| Deciders   | `Cuong - Developer` |
| Date       | 2026-06-25            |
| Supersedes | N/A                   |

**Bối cảnh (Context)**
UC55 yêu cầu xử lý thanh toán cuối cùng. Hệ thống hỗ trợ nhiều phương thức thanh toán (`Cash`, `CreditCard`, `DebitCard`, `VNPay`, `MoMo`, `BankTransfer`).

**Quyết định (Decision)**
Thanh toán tại quầy checkout sẽ chỉ ghi nhận phương thức thanh toán do Lễ tân chọn (offline confirmation). Tích hợp VNPay/MoMo online sẽ được xem xét ở giai đoạn sau.

**Hệ quả (Consequences)**
Tích cực:

- Đơn giản hóa luồng checkout, tránh dependency vào Payment Gateway
- Phù hợp thực tế nghiệp vụ quầy lễ tân Resort

Tiêu cực:

- Chưa hỗ trợ thanh toán online tự động — sẽ bổ sung ở sprint sau

## 4. Non-Functional Requirements & SLA

### 4.1. Performance & Availability

| Category     | Requirement               | Target SLA | Measurement Method | Compliance Basis |
| ------------ | ------------------------- | ---------- | ------------------ | ---------------- |
| Latency      | Trang Check-out load time | < 2s       | Manual test        | —               |
| Latency      | Process Check-out (POST)  | < 3s       | Manual test        | —               |
| Availability | Uptime hệ thống         | 99%        | Monitoring         | —               |

### 4.2. Data Integrity & Retention

| Category    | Requirement                                      | Target     | Verification Method        | Compliance Basis   |
| ----------- | ------------------------------------------------ | ---------- | -------------------------- | ------------------ |
| Durability  | Không mất dữ liệu thanh toán                | RPO = 0    | SQL Server transaction log | —                 |
| Consistency | GuestFolio.TotalAmount == SUM(FolioTransactions) | 100%       | DB query verification      | BR-UC53-02         |
| Retention   | Invoice lưu trữ vĩnh viễn                    | Indefinite | DB backup policy           | Luật kế toán VN |

### 4.3. Security

| Category        | Requirement                      | Target                   | Verification Method | Compliance Basis |
| --------------- | -------------------------------- | ------------------------ | ------------------- | ---------------- |
| Access control  | Chỉ Receptionist, Admin         | Role-based [Authorize]   | Auth Matrix (§16)  | NĐ 356/2025     |
| CSRF Protection | AntiForgeryToken trên form POST | ValidateAntiForgeryToken | Code review         | OWASP A5         |

## 5. Static Modeling (Mô hình Tĩnh)

### 5.1. Class Diagram

```
┌──────────────────────────┐       ┌──────────────────────────┐
│      Booking             │       │      GuestFolio          │
├──────────────────────────┤       ├──────────────────────────┤
│ BookingId (PK)           │1────1 │ FolioId (PK)             │
│ UserId (FK)              │       │ BookingId (FK, Unique)    │
│ CheckInDate              │       │ TotalRoomCharges          │
│ CheckOutDate             │       │ TotalFoodCharges          │
│ BookingStatus             │       │ TotalTourCharges          │
│ DepositAmount            │       │ TotalAmount               │
│ TotalAmount              │       │ Status (FolioStatus)      │
│ ActualCheckInTime        │       └────────────┬─────────────┘
│ ActualCheckOutTime       │                    │ 1
│ RoomHandoverTime         │                    │
└──────────┬───────────────┘                    ▼ N
           │ 1                    ┌──────────────────────────┐
           ▼ N                    │   FolioTransaction       │
┌──────────────────────────┐      ├──────────────────────────┤
│   BookingDetail          │      │ TransactionId (PK)       │
├──────────────────────────┤      │ FolioId (FK)             │
│ BookingDetailId (PK)     │      │ TransactionType (Enum)   │
│ BookingId (FK)           │      │ ReferenceId              │
│ RoomId (FK, nullable)    │      │ Amount                   │
│ RoomCategoryId (FK)      │      │ CreatedAt                │
└──────────────────────────┘      └──────────────────────────┘

┌──────────────────────────┐      ┌──────────────────────────┐
│      Invoice             │      │      Payment             │
├──────────────────────────┤      ├──────────────────────────┤
│ InvoiceId (PK)           │1───N │ PaymentId (PK)           │
│ FolioId (FK, Unique)     │      │ InvoiceId (FK)           │
│ InvoiceNumber            │      │ TourBookingId (FK, null) │
│ TotalAmount              │      │ PaymentMethod (Enum)     │
│ TaxAmount                │      │ Amount                   │
│ FinalAmount              │      │ PaymentStatus (Enum)     │
│ InvoiceStatus (Enum)     │      │ TransactionCode          │
│ IssuedAt                 │      │ PaidAt                   │
└──────────────────────────┘      └──────────────────────────┘
```

### 5.2. Data Structure (Entity Framework / SQL Server)

**Enums liên quan:**

```csharp
// BookingStatus.cs
public enum BookingStatus
{
    Reserved, Arrived, CheckedIn, CheckedOut, Cancelled, NoShow, PendingPayment
}

// FolioStatus.cs
public enum FolioStatus
{
    Unpaid, PartialPaid, Paid, Refunded, Cancelled
}

// FolioTransactionType.cs
public enum FolioTransactionType
{
    RoomCharge, FoodCharge, TourCharge, Payment, Refund, Adjustment
}

// InvoiceStatus.cs
public enum InvoiceStatus
{
    Draft, Issued, Paid, Cancelled
}

// PaymentMethod.cs
public enum PaymentMethod
{
    Cash, CreditCard, DebitCard, VNPay, MoMo, BankTransfer
}

// PaymentStatus.cs
public enum PaymentStatus
{
    Pending, Success, Failed, Refunded
}
```

## 6. Dynamic Modeling (Mô hình Động)

### 6.1. Sequence Diagram — Happy Path (Check-out)

```
Receptionist          ReceptionController       ReceptionService           Database
    │                        │                        │                       │
    │─── GET /CheckOut/5 ───>│                        │                       │
    │                        │── PrepareCheckOutVM ──>│                       │
    │                        │                        │── Load Booking ──────>│
    │                        │                        │<─ Booking (CheckedIn)─│
    │                        │                        │── Load GuestFolio ───>│
    │                        │                        │<─ Folio + Txns ──────│
    │                        │                        │── Calc Late Surcharge│
    │                        │                        │── Insert FolioTxn ──>│(nếu có)
    │                        │                        │── Update Folio totals>│
    │                        │<── CheckOutViewModel ──│                       │
    │<── Render CheckOut ────│                        │                       │
    │                        │                        │                       │
    │── POST /CheckOut ─────>│                        │                       │
    │  (PaymentMethod, etc)  │── ProcessCheckOut ────>│                       │
    │                        │                        │── Create Invoice ────>│
    │                        │                        │── Create Payment ────>│
    │                        │                        │── Folio.Status=Paid─>│
    │                        │                        │── Invoice.Status=Paid>│
    │                        │                        │── Booking.Status=     │
    │                        │                        │   CheckedOut ────────>│
    │                        │                        │── Booking.ActualCheck │
    │                        │                        │   OutTime = Now ────>│
    │                        │                        │── Room.Status=       │
    │                        │                        │   NeedsCleaning ────>│
    │                        │                        │── SaveChangesAsync ─>│
    │                        │<── (true, "") ─────────│                       │
    │<── Redirect Index ─────│                        │                       │
```

### 6.2. Sequence Diagram — Error Path (Unpaid Invoice)

```
Receptionist          ReceptionController       ReceptionService           Database
    │                        │                        │                       │
    │── POST /CheckOut ─────>│                        │                       │
    │  (PaymentMethod=null)  │── ProcessCheckOut ────>│                       │
    │                        │                        │── Validate payment ──│
    │                        │                        │── FAIL: missing      │
    │                        │<── (false, "Vui lòng   │   payment method     │
    │                        │     chọn phương thức") │                       │
    │<── Render CheckOut ────│                        │                       │
    │    (with error msg)    │                        │                       │
```

### 6.3. State Machine — Booking Status

```
    ┌──────────┐     Check-in      ┌───────────┐     Check-out     ┌────────────┐
    │ Reserved │ ──────────────>   │ CheckedIn │ ──────────────>   │ CheckedOut │
    └──────────┘                   └───────────┘                   └────────────┘
         │            (via Arrived)      │
         ▼                               │ (chưa thanh toán)
    ┌──────────┐                         ▼
    │ Arrived  │ ───(Giao phòng)──> CheckedIn
    └──────────┘
```

### 6.4. State Machine — FolioStatus

```
    ┌────────┐     Thanh toán 1 phần     ┌─────────────┐     Thanh toán đủ     ┌────────┐
    │ Unpaid │ ─────────────────────>    │ PartialPaid │ ──────────────────>   │  Paid  │
    └────────┘                           └─────────────┘                       └────────┘
         │                                                                          │
         │──────── Thanh toán đủ (1 lần) ──────────────────────────────────────────>│
```

### 6.5. State Machine — InvoiceStatus

```
    ┌───────┐     Phát hành     ┌────────┐     Thanh toán     ┌────────┐
    │ Draft │ ──────────────>   │ Issued │ ──────────────>    │  Paid  │
    └───────┘                   └────────┘                    └────────┘
```

## 7. Domain Event Catalog

### 7.1. Events Published (Phát ra)

| Event Name            | Trigger                                       | Publisher            | Subscriber(s)                                   | Payload Summary                  | Async? |
| --------------------- | --------------------------------------------- | -------------------- | ----------------------------------------------- | -------------------------------- | ------ |
| `BookingCheckedOut` | Khi checkout thành công                     | `ReceptionService` | `HousekeepingService` (tạo task dọn phòng) | BookingId, RoomIds, CheckOutTime | No     |
| `InvoiceIssued`     | Khi Invoice được tạo và đánh dấu Paid | `ReceptionService` | `NotificationService` (gửi email hóa đơn) | InvoiceId, FolioId, FinalAmount  | No     |
| `PaymentProcessed`  | Khi Payment record được tạo               | `ReceptionService` | `DashboardService` (cập nhật revenue)       | PaymentId, Amount, Method        | No     |

### 7.2. Events Consumed (Tiêu thụ)

| Event Name             | Source                 | Handler               | Action thực hiện                           |
| ---------------------- | ---------------------- | --------------------- | -------------------------------------------- |
| `FoodOrderCharged`   | `FoodOrderService`   | `GuestFolioHandler` | Cập nhật TotalFoodCharges trong GuestFolio |
| `TourBookingCharged` | `TourBookingService` | `GuestFolioHandler` | Cập nhật TotalTourCharges trong GuestFolio |

## 8. Interface Specification (Đặc tả Giao diện)

### 8.1. Service Interface

```csharp
// IReceptionService.cs — Bổ sung cho Check-out
// @version 1.1

public interface IReceptionService
{
    // ... (existing methods: GetBookingsAsync, PrepareCheckInViewModelAsync, ProcessCheckInAsync, ...)

    /// <summary>
    /// Chuẩn bị ViewModel cho trang Check-out: gom bill, tính late surcharge.
    /// </summary>
    /// <param name="bookingId">ID của Booking cần checkout</param>
    /// <returns>CheckOutViewModel chứa toàn bộ thông tin hóa đơn, null nếu không tìm thấy</returns>
    /// <throws>Null khi BookingId không tồn tại hoặc không ở trạng thái CheckedIn</throws>
    Task<CheckOutViewModel?> PrepareCheckOutViewModelAsync(int bookingId);

    /// <summary>
    /// Xử lý checkout: tạo Invoice, Payment, đổi status Booking/Room/Folio.
    /// </summary>
    /// <param name="model">Dữ liệu checkout bao gồm PaymentMethod</param>
    /// <returns>Tuple (Success, ErrorMessage)</returns>
    Task<(bool Success, string ErrorMessage)> ProcessCheckOutAsync(CheckOutViewModel model);
}
```

### 8.2. ViewModel (CheckOutViewModel)

```csharp
// CheckOutViewModel.cs
// @version 1.0

public class CheckOutViewModel
{
    // --- Booking Info ---
    public int BookingId { get; set; }
    public string? GuestName { get; set; }               // Tên khách chính
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public DateTime? ActualCheckInTime { get; set; }
    public string? RoomNumbers { get; set; }              // VD: "101, 102"

    // --- Folio Info ---
    public int FolioId { get; set; }
    public decimal TotalRoomCharges { get; set; }         // Tiền phòng (bao gồm early check-in surcharge)
    public decimal TotalFoodCharges { get; set; }         // Tiền F&B
    public decimal TotalTourCharges { get; set; }         // Tiền Tour
    public decimal LateCheckOutSurcharge { get; set; }    // Phụ thu trả phòng muộn
    public decimal TotalAmount { get; set; }              // Tổng cộng
    public decimal DepositAmount { get; set; }            // Tiền cọc đã đặt
    public decimal BalanceDue { get; set; }               // Số tiền cần thanh toán = Total - Deposit

    // --- Transaction Details ---
    public List<FolioTransactionDto> Transactions { get; set; } = new();

    // --- Payment Input ---
    public PaymentMethod? SelectedPaymentMethod { get; set; }
    public string? PaymentNote { get; set; }
}

public class FolioTransactionDto
{
    public int TransactionId { get; set; }
    public string? TransactionTypeName { get; set; }      // VD: "Tiền phòng", "Phụ thu sớm"
    public decimal? Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

## 9. API Specification (MVC Routes)

### 9.1. Endpoints Table

| Method | Route                            | Auth Level  | Required Roles          | Description                          |
| ------ | -------------------------------- | ----------- | ----------------------- | ------------------------------------ |
| GET    | `/Reception/CheckOut/{id}`     | Cookie Auth | `Receptionist, Admin` | Hiển thị trang tổng bill checkout |
| POST   | `/Reception/CheckOut`          | Cookie Auth | `Receptionist, Admin` | Xử lý thanh toán và checkout     |
| GET    | `/Reception/PrintInvoice/{id}` | Cookie Auth | `Receptionist, Admin` | Hiển thị trang in hóa đơn       |

### 9.2. Request / Response (MVC Form)

**GET `/Reception/CheckOut/5`**

- Input: `id` (BookingId) từ URL
- Output: Render View `CheckOut.cshtml` với `CheckOutViewModel`
- Error: Redirect về Index nếu Booking không ở trạng thái `CheckedIn`

**POST `/Reception/CheckOut`**

- Input: `CheckOutViewModel` (form data) — bao gồm `SelectedPaymentMethod`
- Output (Success): Redirect về `/Reception/Index` với `TempData["SuccessMessage"]`
- Output (Error): Re-render View `CheckOut.cshtml` với `ModelState` errors

## 10. Bảng mã lỗi (Error Codes)

| Code        | HTTP Status | Message (VI)                                                     | Trigger Condition                    |
| ----------- | ----------- | ---------------------------------------------------------------- | ------------------------------------ |
| `CHK-001` | 404         | Không tìm thấy thông tin đặt phòng                        | BookingId không tồn tại           |
| `CHK-002` | 400         | Đơn đặt phòng không ở trạng thái có thể Check-out     | BookingStatus ≠ CheckedIn           |
| `CHK-003` | 400         | Không tìm thấy hóa đơn (Guest Folio) cho đặt phòng này | GuestFolio = null                    |
| `CHK-004` | 400         | Vui lòng chọn phương thức thanh toán                       | SelectedPaymentMethod = null         |
| `CHK-005` | 500         | Lỗi hệ thống khi xử lý Check-out                            | Exception trong ProcessCheckOutAsync |

## 11. Quy trình Triển khai (Step-by-Step)

### 11.1. Prerequisites

- [ ] Các Model (`GuestFolio`, `FolioTransaction`, `Invoice`, `Payment`) đã tồn tại trong DB
- [ ] Enum (`BookingStatus.CheckedOut`, `FolioStatus`, `InvoiceStatus`, `PaymentMethod`, `PaymentStatus`) đã khai báo
- [ ] Module Check-in đã hoạt động ổn định
- [ ] Có ít nhất 1 Booking ở trạng thái `CheckedIn` để test

### 11.2. Implementation Steps

**Chặng 1 — Tạo ViewModel**

- Tạo file `ViewModels/Bookings/CheckOutViewModel.cs`

**Chặng 2 — Bổ sung Service Interface**

- Thêm 2 method signatures vào `IReceptionService.cs`

**Chặng 3 — Implement Service Logic**

- Implement `PrepareCheckOutViewModelAsync()` trong `ReceptionService.cs`:
  - Load Booking + GuestFolio + FolioTransactions
  - Tính Late Check-out Surcharge
  - Tính BalanceDue = TotalAmount - DepositAmount
- Implement `ProcessCheckOutAsync()`:
  - Tạo Invoice (InvoiceStatus = Paid)
  - Tạo Payment (PaymentStatus = Success)
  - Cập nhật GuestFolio.Status = Paid
  - Cập nhật Booking.BookingStatus = CheckedOut
  - Cập nhật Booking.ActualCheckOutTime = DateTime.Now
  - Cập nhật Room.Status = NeedsCleaning (cho tất cả phòng)

**Chặng 4 — Bổ sung Controller Actions**

- Thêm `GET CheckOut(int id)` và `POST CheckOut(CheckOutViewModel model)` vào `ReceptionController.cs`

**Chặng 5 — Tạo Views**

- Tạo `Views/Reception/CheckOut.cshtml` (giao diện tổng bill)
- Tạo `Views/Reception/PrintInvoice.cshtml` (trang in hóa đơn)

**Chặng 6 — Cập nhật Dashboard**

- Sửa `Views/Reception/Index.cshtml`: Hiển thị nút "Check-out" cho trạng thái `CheckedIn`

### 11.3. Deployment Checklist

- [ ] Build thành công (không lỗi compile)
- [ ] Trang CheckOut hiển thị đúng thông tin Bill
- [ ] Late surcharge tính đúng theo quy tắc
- [ ] Sau checkout, BookingStatus = CheckedOut
- [ ] Sau checkout, Room.Status = NeedsCleaning
- [ ] Invoice và Payment records được tạo trong DB

## 12. Rollback & Incident Runbook

### 12.1. Điều kiện kích hoạt Rollback

| Điều kiện                                    | Ngưỡng | Người quyết định |
| ----------------------------------------------- | -------- | --------------------- |
| Checkout thành công nhưng Room vẫn Occupied | 1 case   | Developer             |
| Invoice amount ≠ Folio TotalAmount             | 1 case   | Developer             |
| Payment record không được tạo              | 1 case   | Developer             |

### 12.2. Rollback Procedure

```bash
# Bước 1: Revert code changes
git checkout -- PhucNguyenResort/Services/Reception/ReceptionService.cs
git checkout -- PhucNguyenResort/Controllers/ReceptionController.cs

# Bước 2: Xóa files mới tạo (nếu cần)
del PhucNguyenResort/ViewModels/Bookings/CheckOutViewModel.cs
del PhucNguyenResort/Views/Reception/CheckOut.cshtml
del PhucNguyenResort/Views/Reception/PrintInvoice.cshtml

# Bước 3: Fix dữ liệu bị sai (nếu có)
# Chạy trong SSMS:
# UPDATE Bookings SET BookingStatus = 2 WHERE BookingId = [ID]; -- 2 = CheckedIn
# UPDATE Rooms SET Status = 1 WHERE RoomId = [ID]; -- 1 = Occupied
```

## 13. Kịch bản Kiểm thử Chi tiết

### 13.1. Unit Tests

**TC-UNIT-001 — Late Check-out Surcharge tính đúng 30% (12:00 - 15:00)**

```gherkin
Feature: Late Check-out Surcharge Calculation
  Background:
    Given test data classification: SYNTHETIC
    And Booking #1 có CheckOutDate = 2026-07-01, RoomPrice = 1,000,000 VND

  Scenario: Trả phòng lúc 14:00 — phụ thu 30%
    Given Thời gian checkout thực tế = 2026-07-01 14:00
    When Hệ thống tính Late Check-out Surcharge
    Then Phụ thu = 300,000 VND (30% x 1,000,000)

  Scenario: Trả phòng lúc 16:00 — phụ thu 50%
    Given Thời gian checkout thực tế = 2026-07-01 16:00
    When Hệ thống tính Late Check-out Surcharge
    Then Phụ thu = 500,000 VND (50% x 1,000,000)

  Scenario: Trả phòng lúc 19:00 — phụ thu 100%
    Given Thời gian checkout thực tế = 2026-07-01 19:00
    When Hệ thống tính Late Check-out Surcharge
    Then Phụ thu = 1,000,000 VND (100% x 1,000,000)

  Scenario: Trả phòng lúc 11:30 — không phụ thu
    Given Thời gian checkout thực tế = 2026-07-01 11:30
    When Hệ thống tính Late Check-out Surcharge
    Then Phụ thu = 0 VND
```

**TC-UNIT-002 — BalanceDue tính đúng**

```gherkin
  Scenario: Tổng bill trừ tiền cọc
    Given GuestFolio.TotalAmount = 5,000,000 VND
    And Booking.DepositAmount = 1,500,000 VND
    When PrepareCheckOutViewModelAsync được gọi
    Then BalanceDue = 3,500,000 VND
```

### 13.2. Integration Tests

**TC-INT-001 — Full Check-out flow**

```gherkin
  Scenario: Checkout thành công → DB state đúng
    Given test data classification: SYNTHETIC
    And Booking #1 ở trạng thái CheckedIn
    And GuestFolio exists với TotalAmount = 2,000,000
    And Booking.DepositAmount = 500,000
    When POST /Reception/CheckOut với PaymentMethod = Cash
    Then Booking.BookingStatus = CheckedOut
    And Booking.ActualCheckOutTime IS NOT NULL
    And Room.Status = NeedsCleaning (cho mọi phòng thuộc Booking)
    And Invoice record được tạo với InvoiceStatus = Paid
    And Payment record được tạo với PaymentStatus = Success, Amount = 1,500,000
    And GuestFolio.Status = Paid
```

## 14. Phương pháp Xác minh

### 14.1. Database Inspection

```sql
-- Verify Booking đã checkout
SELECT BookingId, BookingStatus, ActualCheckOutTime
FROM Bookings
WHERE BookingId = [ID];
-- Expected: BookingStatus = 3 (CheckedOut), ActualCheckOutTime IS NOT NULL

-- Verify Room status
SELECT r.RoomId, r.RoomNumber, r.Status
FROM Rooms r
INNER JOIN BookingDetails bd ON r.RoomId = bd.RoomId
WHERE bd.BookingId = [ID];
-- Expected: Status = 3 (NeedsCleaning)

-- Verify Invoice created
SELECT i.InvoiceId, i.FolioId, i.TotalAmount, i.FinalAmount, i.InvoiceStatus
FROM Invoices i
INNER JOIN GuestFolios gf ON i.FolioId = gf.FolioId
WHERE gf.BookingId = [ID];
-- Expected: InvoiceStatus = 2 (Paid)

-- Verify Payment created
SELECT p.PaymentId, p.InvoiceId, p.PaymentMethod, p.Amount, p.PaymentStatus
FROM Payments p
INNER JOIN Invoices i ON p.InvoiceId = i.InvoiceId
INNER JOIN GuestFolios gf ON i.FolioId = gf.FolioId
WHERE gf.BookingId = [ID];
-- Expected: PaymentStatus = 1 (Success)

-- Verify Folio closed
SELECT FolioId, TotalAmount, Status
FROM GuestFolios
WHERE BookingId = [ID];
-- Expected: Status = 2 (Paid)
```

## 15. Mẫu thử thực tế (Verification Samples)

### 15.1. Happy Path — Check-out tại quầy

1. Truy cập `/Reception/Index`
2. Tìm Booking ở trạng thái `CheckedIn`
3. Bấm nút **"Check-out"**
4. Hệ thống hiển thị trang tổng Bill với:
   - Tiền phòng: 2,000,000 VND
   - Phụ thu sớm: 300,000 VND (nếu có)
   - Tiền F&B: 500,000 VND (nếu có)
   - Tổng cộng: 2,800,000 VND
   - Tiền cọc: 1,000,000 VND
   - **Cần thanh toán: 1,800,000 VND**
5. Chọn phương thức **Tiền mặt**
6. Bấm **Xác nhận thanh toán & Check-out**
7. Expected: Redirect về Dashboard, hiển thị thông báo thành công

### 15.2. Error Path — Thiếu phương thức thanh toán

1. Tại trang Check-out, KHÔNG chọn phương thức thanh toán
2. Bấm **Xác nhận**
3. Expected: Hiển thị lỗi "Vui lòng chọn phương thức thanh toán"

## 16. Bảng tổng hợp phân quyền (Authorization Matrix)

| Route                               | Guest | Customer | Receptionist | F&B Staff | Admin |
| ----------------------------------- | ----- | -------- | ------------ | --------- | ----- |
| GET`/Reception/Index`             | ❌    | ❌       | ✅           | ❌        | ✅    |
| GET`/Reception/CheckOut/{id}`     | ❌    | ❌       | ✅           | ❌        | ✅    |
| POST`/Reception/CheckOut`         | ❌    | ❌       | ✅           | ❌        | ✅    |
| GET`/Reception/PrintInvoice/{id}` | ❌    | ❌       | ✅           | ❌        | ✅    |

Chú thích:

- ✅ = Được phép
- ❌ = Bị từ chối (Redirect Login / 403)

---

## PHỤ LỤC

### A. Glossary (Thuật ngữ)

| Thuật ngữ              | Định nghĩa                                                                                  |
| ------------------------ | ---------------------------------------------------------------------------------------------- |
| GuestFolio               | Bảng kê tổng hợp chi phí của khách trong suốt thời gian lưu trú (1:1 với Booking)  |
| FolioTransaction         | Mỗi dòng giao dịch (charge) ghi vào GuestFolio: tiền phòng, ăn uống, tour, phụ thu... |
| Consolidated Invoice     | Hóa đơn tổng được sinh ra tại thời điểm Check-out, tổng hợp toàn bộ GuestFolio  |
| Late Check-out Surcharge | Phụ thu trả phòng muộn khi khách trả phòng sau 12:00 trưa                              |
| Balance Due              | Số tiền khách cần thanh toán = TotalAmount - DepositAmount                                |
| PII                      | Personally Identifiable Information (Thông tin nhận dạng cá nhân)                         |

### B. Tài liệu tham chiếu

| Document                          | Link / Path                                |
| --------------------------------- | ------------------------------------------ |
| SRS — Use Cases UC53, UC54, UC55 | `01_SRS/SWP391_NHOM1_V2.md`              |
| Project Specification             | `01_SRS/SWP391-HOS-02.md`                |
| Check-out Plan                    | `template/CheckOut_TotalBill_Plan.md`    |
| Check-in EDS (nếu có)           | `template/CheckIn_CheckOut_Time_Plan.md` |

> *EDS v2.0 — Áp dụng cho module Check-out & Consolidated Billing.*
