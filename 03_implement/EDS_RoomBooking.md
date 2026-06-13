# ENGINEERING DOCUMENTATION STANDARD (EDS) v2.0
Quy chuẩn Tài liệu Kỹ thuật và Đặc tả Hiện thực hóa

| Field          | Value                 |
| ----------------| -----------------------|
| Document ID    | `SWP391-MOD2-IMP-002` |
| Version        | 1.0                   |
| Date           | 2026-06-12            |
| Status         | Draft                 |
| Document Owner | Cuong                 |
| Author         | Cuong                 |
| Reviewed by    | Khanh                 |
| DPO Sign-off   | Khanh                 |
| Approved by    | Cuong                 |
| Last Review    | 2026-06-12            |
| Based on EDS   | v2.0                  |

## CHANGELOG
| Ngày | Người thực hiện | Nội dung thay đổi |
| --- | --- | --- |
| 2026-06-12 | AI Assistant | Tạo tài liệu lần đầu - EDS cho Use Case Đặt Phòng (Room Booking) |

## MỤC LỤC
1. Tổng quan Module
2. Ma trận Truy vết (Traceability Matrix)
3. Architecture Decision Records (ADR)
4. Non-Functional Requirements & SLA
5. Static Modeling (Mô hình Tĩnh)
6. Dynamic Modeling (Mô hình Động)
7. Domain Event Catalog
8. Interface Specification (Đặc tả Giao diện)
9. API Specification
10. Bảng mã lỗi (Error Codes)
11. Quy trình Triển khai (Step-by-Step)
12. Rollback & Incident Runbook
13. Kịch bản Kiểm thử Chi tiết
14. Phương pháp Xác minh
15. Mẫu thử thực tế (API Verification Samples)
16. Bảng tổng hợp phân quyền (Authorization Matrix)

---

## 1. Tổng quan Module

| Field | Value |
| --- | --- |
| Module Name | `Room Reservation & Front Desk` |
| Bounded Context | `Room Booking / Reservation` |
| Data Classification | PII / Sensitive-PII |
| Compliance Scope | NĐ 356/2025 (Bảo vệ dữ liệu cá nhân) |
| Upstream Dependencies | `Auth Module`, `Room Management Module` |
| Downstream Consumers | `Consolidated Billing Module`, `Payment Gateway` |

## 2. Ma trận Truy vết (Traceability Matrix)

| Requirement ID | Loại (BR/ADR/US) | Mô tả yêu cầu | Thành phần Code | Compliance Target | ADR liên quan |
| --- | --- | --- | --- | --- | --- |
| UC22 / UC07 | User Story | Guest books a room and pays a deposit securely. | `BookingService.CreateBookingAsync()` | NĐ 356/2025 | ADR-001 |
| BR01 | Business Rule | Strict Concurrency (Double-Booking Prevention) | `BookingRepository` + `EF Core Concurrency` | — | ADR-002 |
| BR03 | Business Rule | Cancellation Policy (100% refund if >48h) | `BookingService.CancelBookingAsync()` | Vietnam Law on Tourism | — |

## 3. Architecture Decision Records (ADR)

### ADR-002 — Chiến lược xử lý Concurrency cho Đặt phòng
| Field | Value |
| --- | --- |
| Status | Accepted |
| Deciders | Architecture Team |
| Date | 2026-06-12 |
| Supersedes | N/A |

**Bối cảnh (Context)**
Hệ thống cần đảm bảo quy tắc BR01 (Double-Booking Prevention) để một phòng không bị đặt trùng bởi 2 khách hàng cho cùng một khoảng thời gian.

**Các phương án đã xem xét (Options Considered)**
| Phương án | Mô tả | Ưu điểm | Nhược điểm |
| --- | --- | --- | --- |
| A (Pessimistic Locking) | Lock table hoặc row bằng `UPDLOCK` SQL Server. | Ngăn chặn triệt để conflict từ DB. | Gây nghẽn hệ thống, giảm performance khi tải cao. |
| B (Optimistic Concurrency) | Dùng RowVersion (EF Core) + Catch `DbUpdateConcurrencyException`. | Không bị lock, performance cao. | Phải handle exception khi save failed và báo lại user. |

**Quyết định (Decision)**
Chọn Phương án [B] Optimistic Concurrency + Transaction Isolation Level (Serializable/Repeatable Read) khi check availability và create booking.

**Hệ quả (Consequences)**
- Cần implement logic retry hoặc trả lỗi rõ ràng cho user khi xảy ra conflict.
- Performance tốt hơn trong đa số trường hợp (khi không có conflict).

## 4. Non-Functional Requirements & SLA

### 4.1. Performance & Availability
| Category     | Requirement                   | Target SLA | Measurement Method |
| --------------| -------------------------------| ------------| --------------------|
| Latency      | Booking creation API (p99)    | < 500ms    | k6 load test       |
| Availability | Uptime (monthly)              | 99.9%      | Uptime monitor     |
| Concurrency  | Simultaneous booking requests | 200 req/s  | Load test          |

### 4.2. Security
| Category | Requirement | Target | Verification Method |
| --- | --- | --- | --- |
| Data Protection| Mã hóa CCCD/Passport | AES-256 | SQL Column Encryption |
| Payment | Xử lý Deposit qua Gateway | PCI-DSS | Payment API Integration test |

## 5. Static Modeling (Mô hình Tĩnh)

### 5.2. Data Structure (EF Core / C# Models)
```csharp
public class Booking
{
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public int RoomId { get; set; }
    
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    
    public decimal TotalPrice { get; set; }
    public decimal DepositAmount { get; set; }
    
    public BookingStatus BookingStatus { get; set; } // Pending, Reserved, Cancelled, Completed, NoShow
    public PaymentStatus PaymentStatus { get; set; } // Unpaid, DepositPaid, FullyPaid, Refunded
    
    [Timestamp]
    public byte[] RowVersion { get; set; } // Cho Optimistic Concurrency
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; }
    public virtual Room Room { get; set; }
    public virtual ICollection<BookingGuest> BookingGuests { get; set; }
}
```

## 6. Dynamic Modeling (Mô hình Động)

### 6.3. State Machine (Booking Status)
`Pending` (Chờ đặt cọc) -> `Reserved` (Đã đặt cọc) -> `CheckedIn` -> `Completed` (Đã thanh toán đủ)
`Pending` -> `Cancelled` (Quá hạn cọc / Khách hủy)
`Reserved` -> `Cancelled` (Khách hủy, xử lý hoàn tiền theo BR03)
`Reserved` -> `NoShow` (Khách không đến)

## 7. Domain Event Catalog

### 7.1. Events Published (Phát ra)
| Event Name | Trigger | Publisher | Subscriber(s) | Async? |
| --- | --- | --- | --- | --- |
| `BookingCreatedEvent` | Khách tạo booking thành công | `BookingService` | `NotificationService` | Yes |
| `DepositPaidEvent` | Thanh toán cọc thành công | `PaymentService` | `BookingService` | Yes |
| `BookingCancelledEvent` | Khách hủy booking | `BookingService` | `PaymentService` | Yes |

## 8. Interface Specification (Đặc tả Giao diện)

### 8.1. Service Interface
```csharp
// IBookingService.cs
public interface IBookingService
{
    /// <summary>
    /// Tạo mới một booking (chưa thanh toán cọc).
    /// </summary>
    Task<BookingResult> CreateBookingAsync(CreateBookingRequest request);
    
    /// <summary>
    /// Xử lý hủy phòng (tính toán refund theo BR03).
    /// </summary>
    Task<CancelBookingResult> CancelBookingAsync(int bookingId, int userId);
    
    /// <summary>
    /// Xác nhận thanh toán cọc thành công.
    /// </summary>
    Task<bool> ConfirmDepositAsync(int bookingId, string transactionId);
}
```

## 9. UI & Controller Specification (MVC)

### 9.1. Routes & Actions Table
| HTTP Method | Route | Controller.Action | Required Roles | Chức năng |
| --- | --- | --- | --- | --- |
| GET | `/Bookings/Create` | `BookingsController.Create` | `Customer` | Hiển thị form đặt phòng |
| POST | `/Bookings/Create` | `BookingsController.Create` | `Customer` | Xử lý logic đặt phòng |
| GET | `/Bookings/MyBookings`| `BookingsController.MyBookings`| `Customer` | Xem danh sách phòng đã đặt |
| POST | `/Bookings/Cancel/{id}`| `BookingsController.Cancel` | `Customer`, `Receptionist`| Xử lý hủy phòng |

### 9.2. ViewModels
**CreateBookingViewModel (Dùng cho form POST)**
```csharp
public class CreateBookingViewModel
{
    [Required]
    public int RoomId { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày nhận phòng")]
    public DateTime CheckInDate { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày trả phòng")]
    public DateTime CheckOutDate { get; set; }
    
    [Range(1, 10)]
    public int NumberOfGuests { get; set; }
}
```

## 10. Bảng thông báo lỗi (Validation & ModelState)

Trong mô hình MVC, các lỗi nghiệp vụ không trả về mã HTTP 4xx dạng JSON, mà sẽ được thêm vào `ModelState` hoặc truyền qua `TempData` để hiển thị trên View.

| Mã lỗi Nội bộ | Nơi hiển thị | Câu thông báo (VI) | Điều kiện kích hoạt |
| --- | --- | --- | --- |
| `ERR_DATE` | `ModelState` | Ngày Check-in phải trước Check-out và lớn hơn hoặc bằng hôm nay. | Validation form |
| `ERR_CONFLICT`| `ModelState` | Rất tiếc, phòng này đã có người đặt trong khoảng thời gian bạn chọn. | Bắt được `DbUpdateConcurrencyException` hoặc check logic overlap |
| `ERR_CANCEL` | `TempData["Error"]`| Không thể hủy phòng ở trạng thái hiện tại. | Khi thử hủy phòng đã CheckedIn/Completed |
| `ERR_NOTFOUND`| Lỗi 404 View | Không tìm thấy thông tin đặt phòng. | Truy cập ID không tồn tại |

## 11. Quy trình Triển khai (Step-by-Step)
1. Thêm EF Core Migration cho `Bookings` table kèm `RowVersion`.
2. Tạo các ViewModels (`CreateBookingViewModel`, `BookingDetailsViewModel`).
3. Implement `BookingService` cho xử lý logic nghiệp vụ.
4. Xây dựng `BookingsController` với các action GET/POST.
5. Thiết kế Razor Views (`Create.cshtml`, `MyBookings.cshtml`) kèm hiển thị `ModelState` errors.
6. Tích hợp màn hình thanh toán Deposit (VNPay/Stripe) sau bước tạo Booking thành công.

## 12. Rollback & Incident Runbook
| Điều kiện | Ngưỡng | Người quyết định |
| --- | --- | --- |
| Double Booking xảy ra | >= 1 case | Tech Lead |
| Lỗi khi thanh toán Deposit | > 10% giao dịch | On-call Engineer |

## 13. Kịch bản Kiểm thử Chi tiết (Bổ sung cho MVC)
- Xác nhận **Anti-Forgery Token**: Đảm bảo tất cả các form POST đều có `[ValidateAntiForgeryToken]` để phòng chống CSRF.
- Giao diện: Hiển thị đúng thông báo lỗi từ `ModelState` nếu form submit không hợp lệ.

## 14. Phương pháp Xác minh
- Chạy ứng dụng và truy cập qua trình duyệt.
- Dùng công cụ Developer Tools (F12) để kiểm tra các request POST có đính kèm cookie xác thực và RequestVerificationToken.
- Kiểm tra DB `Bookings` table để đảm bảo giao dịch hoàn tất đúng logic.

## 15. Mẫu thử thực tế (UI Verification Samples)
- **Truy cập /Bookings/Create**: Bỏ trống trường `CheckInDate` và submit. Form sẽ báo lỗi ngay lập tức nhờ Data Annotations.
- **Tạo conflict nhân tạo**: Mở 2 tab trình duyệt cho cùng 1 phòng và khoảng ngày. Tab 1 bấm Đặt phòng. Tab 2 bấm Đặt phòng. Tab 2 phải hiển thị lỗi `ERR_CONFLICT` thay vì sập trang.

## 16. Bảng tổng hợp phân quyền (Authorization Matrix - MVC Controller)
Dùng Attribute `[Authorize(Roles = "...")]` trên các Action/Controller.

| Controller / Action | GUEST | CUSTOMER | RECEPTIONIST | ADMIN |
| --- | --- | --- | --- | --- |
| GET `/Bookings/MyBookings` | Redirect to Login| ✅ Own | ❌ | ❌ |
| GET, POST `/Bookings/Create`| Redirect to Login| ✅ | ❌ | ❌ |
| GET `/Bookings/Index` | Redirect to Login| ❌ | ✅ | ✅ |
| POST `/Bookings/Cancel/{id}`| Redirect to Login| ✅ Own | ✅ | ✅ |
