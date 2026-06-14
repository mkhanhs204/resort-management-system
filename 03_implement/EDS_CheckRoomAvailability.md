# ENGINEERING DOCUMENTATION STANDARD (EDS) v2.0
Quy chuẩn Tài liệu Kỹ thuật và Đặc tả Hiện thực hóa

| Field          | Value              |
| ----------------| --------------------|
| Document ID    | `PNR-ROOM-IMP-001` |
| Version        | 1.0                |
| Date           | 2026-06-11         |
| Status         | Approved           |
| Document Owner | Cuong              |
| Author         | Cuong              |
| Reviewed by    | Khanh              |
| DPO Sign-off   | Khanh              |
| Approved by    | Cuong              |
| Last Review    | 2026-06-11         |
| Based on EDS   | v2.0               |

## CHANGELOG
**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ. Mọi thay đổi phải ghi vào bảng này.

| Ngày       | Người thực hiện | Nội dung thay đổi                                               |
| ------------| -----------------| -----------------------------------------------------------------|
| 2026-06-11 | System Agent    | Tạo tài liệu lần đầu - Logic kiểm tra phòng trống theo Category |

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
Mô tả ngắn gọn mục đích của module, phạm vi nghiệp vụ và lý do tồn tại.

| Field                 | Value                                      |
| -----------------------| --------------------------------------------|
| Module Name           | `Room Reservation & Front Desk` (Module 2) |
| Bounded Context       | `Booking / Availability`                   |
| Data Classification   | Internal / Public                          |
| Compliance Scope      | N/A                                        |
| Upstream Dependencies | `Room Service`, `Booking Service`          |
| Downstream Consumers  | `Home Controller`, `Booking Controller`    |

## 2. Ma trận Truy vết (Traceability Matrix)

| Requirement ID | Loại (BR/ADR/US) | Mô tả yêu cầu | Thành phần Code | Compliance Target | ADR liên quan |
| --- | --- | --- | --- | --- | --- |
| BR01 | Business Rule | Strict Concurrency - Ngăn chặn Double-Booking cho cùng ngày tháng | `HomeService.CheckAvailabilityAsync` | Consistency | ADR-001 |
| UC04 / UC06 | User Story | Tìm kiếm và xem thông tin phòng trống theo ngày | `HomeController.SearchRooms` | — | — |
| UC59 | User Story | Cập nhật tình trạng phòng (Available/Maintenance/...) | `Room.Status` | — | — |

## 3. Architecture Decision Records (ADR)

### ADR-001 — Phương pháp tính số lượng phòng trống
| Field    | Value               |
| ----------| ---------------------|
| Status   | Accepted            |
| Deciders | Principal Architect |
| Date     | 2026-06-11          |

**Bối cảnh (Context)**
Cần xác định thuật toán tính số phòng trống (Available Rooms) cho một `CategoryId` trong khoảng thời gian `[CheckInDate, CheckOutDate]`.

**Các phương án đã xem xét (Options Considered)**
| Phương án                             | Mô tả                                                                                               | Ưu điểm                                                        | Nhược điểm                                                                |
| ---------------------------------------| -----------------------------------------------------------------------------------------------------| ----------------------------------------------------------------| ---------------------------------------------------------------------------|
| A: Tính theo từng phòng cụ thể        | Truy vấn từng phòng xem phòng nào trống trong thời gian đó.                                         | Chặt chẽ, cấp phát phòng ngay từ đầu.                          | Tốn kém chi phí truy vấn, dễ gây deadlock khi có nhiều booking đồng thời. |
| B: Tính gộp theo Category (Công thức) | Lấy Tổng số phòng (trừ Maintenance) trừ đi Số phòng đã Booking (trùng lịch và không Cancel/NoShow). | Tốc độ cao, linh hoạt trong việc sắp xếp phòng vật lý sau này. | Cần phải ánh xạ lại vào Room thực tế khi Check-in.                        |

**Quyết định (Decision)**
Chọn **Phương án B** vì kiến trúc hệ thống hiện tại quản lý việc đặt phòng theo `RoomCategory` thay vì `Room` vật lý trong giai đoạn Guest đặt (Guest booking by category). Số phòng thực tế sẽ được Receptionist gán (UC54 - Assign Room) lúc Check-in.

**Hệ quả (Consequences)**
Tích cực:
- Nâng cao hiệu suất truy vấn trên trang chủ và trang SearchRooms.
- Giảm thiểu tình trạng race condition khi khách hàng chốt phòng cùng lúc.

## 4. Non-Functional Requirements & SLA

### 4.1. Performance & Availability
| Category | Requirement | Target SLA | Measurement Method | Compliance Basis |
| --- | --- | --- | --- | --- |
| Latency | API response (p99) | < 500ms | Load test (JMeter) | — |
| Availability | Uptime (monthly) | 99.9% | Uptime monitor | — |
| Throughput | Concurrent requests | 200 req/s | Load test | — |

## 5. Static Modeling (Mô hình Tĩnh)
### 5.1. Data Structure (EF Core)
```csharp
public class RoomCategory
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public ICollection<Room> Rooms { get; set; }
}

public class Room
{
    public int RoomId { get; set; }
    public int CategoryId { get; set; }
    public string Status { get; set; } // Available, Maintenance, v.v.
}

public class Booking
{
    public int BookingId { get; set; }
    public int RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public BookingStatus BookingStatus { get; set; } // Confirmed, CheckedIn, Cancelled, NoShow
}
```

## 6. Dynamic Modeling (Mô hình Động)
### Thuật toán xác định Date Overlap (Trùng lặp ngày):
Hai khoảng thời gian `[Start1, End1]` và `[Start2, End2]` bị trùng lắp khi và chỉ khi:
`Start1 < End2 && End1 > Start2`

Tương đương trong code:
`b.CheckInDate < checkOutDate && b.CheckOutDate > checkInDate`

## 8. Interface Specification (Đặc tả Giao diện)
### 8.1. Service Interface
```csharp
// IHomeService.cs
// @version 1.0

public interface IHomeService 
{
    /// <summary>
    /// Kiểm tra số lượng phòng trống của một loại phòng trong khoảng thời gian nhất định
    /// Loại bỏ các phòng đang bảo trì (Maintenance)    /// Tìm tất cả RoomCategory có phòng trống ≥ roomCount, phù hợp số lượng người lớn và trẻ em.
    /// </summary>
    Task<IEnumerable<RoomCategory>> SearchAvailableCategoriesAsync(DateTime checkInDate, DateTime checkOutDate, int adultsCount, int childrenCount, int roomCount);
}
```

## 9. API Specification

### 9.1. Endpoints Table
| Method | Path | Auth Level | Required Roles | Rate Limit | Idempotent? |
| --- | --- | --- | --- | --- | --- |
| GET | `/Home/CheckRoomAvailability` | None (Guest) | N/A | 100/min | Yes |

### 9.2. Request / Response Schemas

**GET `/Home/CheckRoomAvailability`**

Request Params:
- `categoryId` (int)
- `checkIn` (string format: dd/MM/yyyy)
- `checkOut` (string format: dd/MM/yyyy)

Response — 200 OK:
```json
{
  "success": true,
  "availableRooms": 3,
  "message": "Còn 3 phòng trống"
}
```

## 10. Bảng mã lỗi (Error Codes)
| Code | HTTP Status | Message (EN) | Message (VI) | Trigger Condition |
| --- | --- | --- | --- | --- |
| `ROOM-001` | 400 | Invalid Dates | Ngày không hợp lệ | CheckOut <= CheckIn |
| `ROOM-002` | 404 | Category Not Found| Không tìm thấy loại phòng | CategoryId không tồn tại |
| `ROOM-003` | 500 | Internal Error | Lỗi hệ thống | Exception từ DB/EF Core |

## 13. Kịch bản Kiểm thử Chi tiết
- **Test Data Classification:** SYNTHETIC
(Xem chi tiết trong file `TDD_TEMPLATE`)
