# ENGINEERING DOCUMENTATION STANDARD (EDS) v2.0

Quy chuẩn Tài liệu Kỹ thuật và Đặc tả Hiện thực hóa

| Field          | Value                              |
| -------------- | ---------------------------------- |
| Document ID    | `PHUCNGUYEN-TOURBOOKING-IMP-001` |
| Version        | 1.0                                |
| Date           | 2026-06-18                         |
| Status         | Approved                           |
| Document Owner | Nguyễn Khắc Hiếu                |
| Author         | Nguyễn Khắc Hiếu                |
| Reviewed by    | truong cuong                       |
| DPO Sign-off   | [x] Approved                       |
| Approved by    | Nguyễn Khắc Hiếu                |
| Based on EDS   | v2.0                               |

## CHANGELOG

| Ngày      | Người thực hiện | Nội dung thay đổi                              |
| ---------- | ------------------- | ------------------------------------------------- |
| 2026-06-18 | AI                  | Khởi tạo tài liệu EDS cho module Tour Booking |

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

Module quản lý quá trình đặt Tour cho khách hàng (Tour Booking), hỗ trợ thanh toán trực tiếp hoặc tích hợp vào hóa đơn phòng (Charge to Room).

| Field                 | Value                                        |
| --------------------- | -------------------------------------------- |
| Module Name           | `TourBookingModule`                        |
| Bounded Context       | `Tour Management & Reservation`            |
| Data Classification   | Internal / Sensitive (Payment Info)          |
| Compliance Scope      | Vietnam Law on Tourism 2017, Decree 356/2025 |
| Upstream Dependencies | `AuthModule`, `RoomBookingModule`        |
| Downstream Consumers  | `BillingModule`, `NotificationModule`    |

## 2. Ma trận Truy vết (Traceability Matrix)

| Requirement ID | Loại (BR/ADR/US) | Mô tả yêu cầu                               | Thành phần Code                             | Compliance Target | ADR liên quan |
| -------------- | ----------------- | ----------------------------------------------- | --------------------------------------------- | ----------------- | -------------- |
| UC26           | User Story        | Đặt tour và chọn phương thức thanh toán | `TourBookingController.BookTour()`          | Law on Tourism    | ADR-001        |
| BR-TOUR-01     | Business Rule     | Tránh Overbooking                              | `TourBookingService.CreateBookingAsync()`   | N/A               | ADR-002        |
| BR-TOUR-02     | Business Rule     | Charge to Room cần BookingId                   | `TourBookingService.ValidateChargeToRoom()` | Billing Standard  | ADR-001        |

## 3. Architecture Decision Records (ADR)

### ADR-001 — Phương thức thanh toán "Charge to Room"

| Field    | Value      |
| -------- | ---------- |
| Status   | Accepted   |
| Deciders | Tech Lead  |
| Date     | 2026-06-18 |

**Bối cảnh (Context)**
Khách hàng muốn gộp chi phí tour vào hóa đơn phòng khi trả phòng thay vì thanh toán ngay.

**Quyết định (Decision)**
Lưu trường `BookingId` (Nullable) trong bảng `TourBookings`. Nếu phương thức là "Charge to Room", phải kiểm tra tính hợp lệ của `BookingId` (đang Checked-in). Chi phí sẽ được tự động tính vào lúc Check-out (Consolidated Billing).

### ADR-002 — Ngăn chặn Overbooking (Pessimistic Locking)

| Field    | Value      |
| -------- | ---------- |
| Status   | Accepted   |
| Deciders | Tech Lead  |
| Date     | 2026-06-18 |

**Quyết định (Decision)**
Sử dụng Transaction và Pessimistic Locking trong C# (hoặc khóa row mức database) khi kiểm tra và trừ `AvailableSlots` của bảng `TourSchedules` để đảm bảo tính nhất quán khi có nhiều luồng đặt tour đồng thời.

## 4. Non-Functional Requirements & SLA

| Category     | Requirement               | Target SLA | Measurement Method          |
| ------------ | ------------------------- | ---------- | --------------------------- |
| Latency      | Tour booking API response | < 500ms    | Load test                   |
| Consistency  | No double booking         | 100%       | Audit logs & DB constraints |
| Availability | System uptime             | 99.9%      | Uptime monitor              |

## 5. Static Modeling (Mô hình Tĩnh)

### 5.1. Data Structure (C# Models)

```csharp
public class TourBooking
{
    public int TourBookingId { get; set; }
    public string UserId { get; set; }
    public int ScheduleId { get; set; }
    public int? BookingId { get; set; } // Nullable for non-room bookings
    public int Participants { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentMethod { get; set; } // "Immediate", "ChargeToRoom"
    public string BookingStatus { get; set; } // "Pending", "Confirmed", "Cancelled"
    public DateTime CreatedAt { get; set; }
}
```

## 8. Interface Specification (Đặc tả Giao diện)

### 8.1. Service Interface

```csharp
public interface ITourBookingService
{
    Task<TourBookingResponse> BookTourAsync(TourBookingRequest request);
    Task<bool> CancelTourBookingAsync(int tourBookingId);
    Task<IEnumerable<TourBooking>> GetUserTourBookingsAsync(string userId);
}
```

## 9. API Specification

### 9.1. Endpoints Table

| Method | Path                                            | Auth Level | Required Roles       | Rate Limit | Idempotent? |
| ------ | ----------------------------------------------- | ---------- | -------------------- | ---------- | ----------- |
| POST   | `/api/v1/tour-bookings`                       | Bearer     | `Customer`         | 10/min     | No          |
| GET    | `/api/v1/tour-bookings/my-bookings`           | Bearer     | `Customer`         | 60/min     | Yes         |
| GET    | `/api/v1/tour-bookings/manifest/{scheduleId}` | Bearer     | `TourGuide, Admin` | 60/min     | Yes         |

### 9.2. Request / Response Schemas

**POST `/api/v1/tour-bookings`**

```json
// Request
{
  "scheduleId": 101,
  "participants": 2,
  "paymentMethod": "ChargeToRoom",
  "bookingId": 505
}

// Response (201 Created)
{
  "tourBookingId": 1001,
  "status": "Confirmed",
  "totalAmount": 1500000,
  "message": "Tour booked successfully"
}
```

## 10. Bảng mã lỗi (Error Codes)

| Code       | HTTP Status | Message (EN)                               | Message (VI)                              |
| ---------- | ----------- | ------------------------------------------ | ----------------------------------------- |
| `TB-001` | 400         | Not enough available slots                 | Lịch trình không đủ chỗ trống      |
| `TB-002` | 400         | Invalid Booking ID for Charge to Room      | Mã phòng không hợp lệ để ghi nợ   |
| `TB-003` | 404         | Tour schedule not found                    | Không tìm thấy lịch trình tour       |
| `TB-004` | 403         | Only Checked-in guests can use this method | Khách chưa Check-in không thể ghi nợ |

## 16. Bảng tổng hợp phân quyền (Authorization Matrix)

| Endpoint                       | GUEST | CUSTOMER | TOUR GUIDE | ADMIN |
| ------------------------------ | ----- | -------- | ---------- | ----- |
| GET `/api/v1/tours`          | ✅    | ✅       | ✅         | ✅    |
| POST `/api/v1/tour-bookings` | ❌    | ✅       | ❌         | ✅    |
| GET `manifest/{scheduleId}`  | ❌    | ❌       | ✅         | ✅    |
