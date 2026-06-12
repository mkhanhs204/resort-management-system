# TEST-DRIVEN DEVELOPMENT SPECIFICATION TEMPLATE
Mẫu Đặc tả Kiểm thử Hướng Phát triển

**Document ID**: PNR-TDD-ROOM-001
**Version**: 1.0
**Date**: 2026-06-11
**Status**: Approved
**Standard**: ISO/IEC/IEEE 29119-3:2021
**Author**: System Agent
**Reviewed by**: Tech Lead
**DPO Sign-off**: N/A
**Approved by**: Principal Architect
**Classification**: Internal - Confidential

**References**:
- `01_SRS/SWP391_NHOM1.md` — Functional requirements (UC04, UC06)
- `01_SRS/SWP391-HOS-02.md` — Problem Statement & BR01 (Double Booking Prevention)

## CHANGELOG
**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ.
| Ngày       | Người thực hiện | Nội dung thay đổi                                       |
| ------------| -----------------| ---------------------------------------------------------|
| 2026-06-11 | System Agent    | Khởi tạo tài liệu — TDD cho logic tính toán phòng trống |

---

## 1. Thông tin Module
| Field                 | Value　　　　　　　　　　　　　　 |
| -----------------------| -----------------------------------|
| Feature / Gap ID      | FEAT-ROOM-AVAILABILITY　　　　　　|
| Module                | `Room Reservation`　　　　　　　　|
| Spec gốc              | `EDS_CheckRoomAvailability.md`　　|
| Priority              | 🔴 P0　　　　　　　　　　　　　　 |
| Sprint                | S1　　　　　　　　　　　　　　　　|
| Data Classification   | Internal　　　　　　　　　　　　　|
| Compliance Scope      | N/A　　　　　　　　　　　　　　　 |
| Upstream Dependencies | `RoomCategory`, `Room`, `Booking` |
| Downstream Consumers  | `HomeService`　　　　　　　　　　 |

## 2. Logic Issues Resolved
| # | Spec gốc (sai / thiếu) | Thực tế (schema / policy) | Fix áp dụng trong test |
| --- | --- | --- | --- |
| L1 | Không nói rõ loại trừ BookingStatus nào | Enum: `BookingStatus.Cancelled`, `NoShow` | Phải đảm bảo các trạng thái này không bị trừ vào Available Rooms |
| L2 | Phòng bảo trì | Trạng thái `Room.Status == "Maintenance"` | Phải loại trừ các phòng đang bảo trì khỏi Total Rooms |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi
Phạm vi của TDD spec này:
```
[Room Reservation] bao gồm các layer:
├── Domain (PhucNguyenResort.Models)
├── Services (HomeService) -> ĐỐI TƯỢNG TEST CHÍNH
└── Controller (HomeController)
```

### TDS-02 — Test Basis / Cơ sở Kiểm thử
| Source | Items Derived |
| --- | --- |
| `SWP391-HOS-02.md` BR01 | Strict Concurrency (Double-booking prevention) |
| `HomeService.cs` | Logic `b.CheckInDate < checkOutDate && b.CheckOutDate > checkInDate` |

### TDS-03 — Test Conditions and Coverage Items
| Condition ID | Test Condition | Coverage Item | Test Cases |
| --- | --- | --- | --- |
| TC-COND-001 | Tính toán cơ bản khi không có booking nào | `CheckAvailabilityAsync` | `ROOM-TC-001` |
| TC-COND-002 | Booking không trùng ngày (trước/sau) | `CheckAvailabilityAsync` | `ROOM-TC-002` |
| TC-COND-003 | Booking trùng lấp ngày | `CheckAvailabilityAsync` | `ROOM-TC-003` |
| TC-COND-004 | Loại trừ phòng Maintenance | `CheckAvailabilityAsync` | `ROOM-TC-004` |
| TC-COND-005 | Trạng thái Booking Cancelled/NoShow | `CheckAvailabilityAsync` | `ROOM-TC-005` |

### TDS-04 — Test Techniques / Kỹ thuật Kiểm thử
| Technique (ISO 29119-4) | Applied To | Rationale |
| --- | --- | --- |
| Boundary Value Analysis | Date Overlap (CheckIn, CheckOut) | Đảm bảo xử lý đúng tại các mốc ngày biên (ví dụ CheckOut của người này là CheckIn của người khác). |

### TDS-05 — Test Data Requirements
| Fixture ID | Type | Value / Logic | Mục đích |
| --- | --- | --- | --- |
| FX-001 | Mock DB | `TotalRooms = 5`, 0 Booking | Happy path (Empty) |
| FX-002 | Mock DB | `TotalRooms = 5`, 2 Bookings trùng ngày | Test Overlap logic |

## 4. Test Case Specification

### ROOM-TC-001 — Trả về tổng số phòng khi không có Booking nào
**Severity**: CRITICAL
**Feature Under Test**: `HomeService.CheckAvailabilityAsync()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:
- Category ID = 1 có 5 phòng (Status = "Available").
- Không có bất kỳ Booking nào trong hệ thống.

**Test Steps**:
1. Gọi `CheckAvailabilityAsync(1, "01/01/2027", "05/01/2027")`

**Expected Result (PASS)**:
- Trả về kết quả: `5`

### ROOM-TC-002 — Trả về đúng số phòng khi Booking không bị trùng ngày
**Severity**: HIGH
**Feature Under Test**: `HomeService.CheckAvailabilityAsync()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:
- Category ID = 1 có 5 phòng (Status = "Available").
- Có 1 Booking cho phòng thuộc Category 1 từ `01/01/2027` đến `03/01/2027`.

**Test Steps**:
1. Gọi `CheckAvailabilityAsync(1, "05/01/2027", "10/01/2027")` (Khoảng thời gian hoàn toàn không giao nhau)

**Expected Result (PASS)**:
- Trả về kết quả: `5` (Vì Booking cũ đã Check-out trước đó).

### ROOM-TC-003 — Trừ đúng số phòng khi có Booking bị Overlap (Trùng ngày)
**Severity**: CRITICAL
**Feature Under Test**: `HomeService.CheckAvailabilityAsync()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:
- Category ID = 1 có 5 phòng (Status = "Available").
- Có 2 Bookings khác nhau, đều nằm trong khoảng `02/01/2027` đến `06/01/2027`.

**Test Steps**:
1. Gọi `CheckAvailabilityAsync(1, "01/01/2027", "05/01/2027")`

**Expected Result (PASS)**:
- Trả về kết quả: `3` (5 tổng - 2 trùng = 3).
- Giải thích: Khoảng `01/01 - 05/01` giao cắt với khoảng `02/01 - 06/01`.

### ROOM-TC-004 — Phải loại bỏ phòng đang bị Maintenance
**Severity**: HIGH
**Feature Under Test**: `HomeService.CheckAvailabilityAsync()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:
- Category ID = 1 có 5 phòng (2 phòng "Available", 3 phòng "Maintenance").
- Không có Booking.

**Test Steps**:
1. Gọi `CheckAvailabilityAsync(1, "01/01/2027", "05/01/2027")`

**Expected Result (PASS)**:
- Trả về kết quả: `2` (Chỉ đếm các phòng không bị Maintenance).

### ROOM-TC-005 — Bỏ qua (không trừ) các Booking đã bị Cancelled hoặc NoShow
**Severity**: HIGH
**Feature Under Test**: `HomeService.CheckAvailabilityAsync()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:
- Category ID = 1 có 5 phòng.
- Có 2 Booking hoàn toàn trùng lịch (01/01 đến 05/01) nhưng 1 Booking có status là `Cancelled` và 1 status là `NoShow`.

**Test Steps**:
1. Gọi `CheckAvailabilityAsync(1, "01/01/2027", "05/01/2027")`

**Expected Result (PASS)**:
- Trả về kết quả: `5` (Bởi vì cả 2 Booking kia đều không còn hiệu lực nên phòng được trả lại cho hệ thống).

### UC04_06_TC_001 — Tìm kiếm với ngày không hợp lệ (SearchRooms)
**Severity**: MEDIUM
**Feature Under Test**: `HomeController.SearchRooms()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:
- `checkIn` và `checkOut` là chuỗi không hợp lệ (ví dụ: "invalid").

**Test Steps**:
1. Gọi `SearchRooms("invalid-date", "invalid-date", 2, 1)`

**Expected Result (PASS)**:
- Hệ thống tự động gán `checkInDate = Today` và `checkOutDate = Today + 1`.
- Cập nhật đúng các biến `ViewBag` (ViewData) và gọi service tìm kiếm thành công.

### UC04_06_TC_002 — Tìm kiếm với ngày hợp lệ (SearchRooms)
**Severity**: HIGH
**Feature Under Test**: `HomeController.SearchRooms()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:
- `checkIn` = "01/12/2027", `checkOut` = "05/12/2027".

**Test Steps**:
1. Gọi `SearchRooms("01/12/2027", "05/12/2027", 2, 1)`

**Expected Result (PASS)**:
- Trả về danh sách RoomCategory chính xác.
- Gọi chính xác hàm `SearchAvailableCategoriesAsync` với đúng tham số ngày tháng.

### UC59_TC_001 — Cập nhật trạng thái phòng thành công (Edit)
**Severity**: CRITICAL
**Feature Under Test**: `RoomsController.Edit()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:
- Gửi lên model `RoomEditVM` với ID = 1 và `Status = RoomStatus.Maintenance`.

**Test Steps**:
1. Gọi hàm `Edit(1, model)`

**Expected Result (PASS)**:
- Gọi hàm `UpdateRoomAsync` của `RoomService` với tham số `Status` đã được cập nhật thành `Maintenance`.
- Chuyển hướng (Redirect) thành công về trang Index.

### UC59_TC_002 — Truyền vào ID không khớp với Model ID (Edit)
**Severity**: MEDIUM
**Feature Under Test**: `RoomsController.Edit()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:
- Truyền vào ID = 1 nhưng model có `RoomId = 2`.

**Test Steps**:
1. Gọi hàm `Edit(1, model)`

**Expected Result (PASS)**:
- Trả về mã lỗi `404 NotFound`.
