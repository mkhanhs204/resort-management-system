# TEST-DRIVEN DEVELOPMENT SPECIFICATION

Đặc tả Kiểm thử Hướng Phát triển — Check Room Availability

**Document ID**: PNR-TDD-ROOM-001
**Version**: 1.3
**Date**: 2026-06-19
**Status**: Approve
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: Cuong
**Reviewed by**: Khanh
**DPO Sign-off**: khanh
**Approved by**: Cuong
**Classification**: Internal - Confidential

**References**:

- `01_SRS/SWP391_NHOM1.md` — Functional requirements (UC04 Search Room, UC06 View Room Details, UC59 Update Room Status)
- `01_SRS/SWP391-HOS-02.md` — Problem Statement & BR01 (Double-Booking Prevention), UC06, UC10
- `03_implement/EDS_RoomBooking.md` (`SWP391-MOD2-IMP-002`) — Technical Specification
- `template/TDD_TEMPLATE_V1.md` — TDD Template v1.0

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.
Thứ tự bắt buộc: viết test → chạy → xác nhận FAIL 🔴 → implement → PASS 🟢 → refactor 🔵.
Test data dùng classification `SYNTHETIC`. Không dùng PII thật.

## CHANGELOG

**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ.

| Ngày      | Người thực hiện | Nội dung thay đổi                                                                                                                                                                                       |
| ---------- | ------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 2026-06-11 | Cuong               | Khởi tạo tài liệu — TDD cho logic tính toán phòng trống                                                                                                                                           |
| 2026-06-13 | Cuong               | Chỉnh sửa toàn bộ (v1.1): cập nhật đúng enum values (BookingStatus, RoomStatus), bổ sung section 5/6/7 theo TDD Template, thêm Test Conditions cho CheckedOut exclusion, cập nhật Logic Issues |
| 2026-06-19 | Cuong               | Cập nhật v1.2: phản ánh kiến trúc BookingDetails mới — sửa Logic Issues L1/L3/L4, cập nhật test cases ROOM-TC-005/006, fixtures |
| 2026-06-19 | Cuong               | Cập nhật v1.3: Thêm các test cases ROOM-TC-007/008/009 cho chức năng SearchAvailableCategoriesAsync (Search Room) |

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

| Field                 | Value                                                                                                                 |
| --------------------- | --------------------------------------------------------------------------------------------------------------------- |
| Feature / Gap ID      | FEAT-ROOM-AVAILABILITY                                                                                                |
| Module                | `Room Reservation & Front Desk` (Module 2 — CORE 1)                                                                |
| Spec gốc             | `SWP391-MOD2-IMP-002` (`EDS_RoomBooking.md`)                                                                      |
| Priority              | 🟢 P0 — Logic cốt lõi, ảnh hưởng trực tiếp đến BR01                                                        |
| Sprint                | S1                                                                                                                    |
| Milestone             | M1 — Core Booking Functions                                                                                          |
| Data Classification   | Internal (logic availability không xử lý PII trực tiếp)                                                          |
| Compliance Scope      | BR01 — Strict Concurrency (Double-Booking Prevention)                                                                |
| Upstream Dependencies | `Room` (Model + `RoomStatus` enum), `RoomCategory` (Model), `Booking` (Model + `BookingStatus` enum), `BookingDetail` (Model) |
| Downstream Consumers  | `HomeController.SearchRooms()`, `HomeController.CheckRoomAvailability()`, `BookingService.CreateBookingAsync()` |

## 2. Logic Issues Resolved

Bắt buộc điền trước khi viết test. Liệt kê mọi sai lệch giữa spec thiết kế và codebase thực tế.

| #  | Spec gốc (sai / thiếu)                                                                        | Thực tế (schema / codebase)                                                                                                                                                                                                                                         | Fix áp dụng trong test                                                                                                                                                                                                                                        |
| -- | ----------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| L1 | Spec ban đầu không nói rõ phải loại trừ BookingStatus nào khi đếm phòng đã đặt  | `HomeService.CheckAvailabilityAsync()` loại trừ `BookingStatus.Cancelled` và `BookingStatus.CheckedOut` — tức chỉ đếm booking `Reserved`, `CheckedIn`, `NoShow`, `PendingPayment` là "đang chiếm phòng"                                                         | Test phải assert: booking `Cancelled` và `CheckedOut` **không** trừ vào available count                                                                                                                                                              |
| L2 | Spec viết "Room.Status == Maintenance" dạng string                                            | Codebase dùng enum `RoomStatus.Maintenance` (file `Enums/RoomStatus.cs`). Các giá trị khác: `Available`, `Occupied`, `NeedsCleaning`                                                                                                                   | Test dùng `RoomStatus.Maintenance` enum thay vì string. Phòng `Maintenance` bị loại khỏi TotalRooms                                                                                                                                                   |
| L3 | Spec gốc không đề cập đến `CheckedOut`                                                 | `BookingStatus` enum gồm: `Reserved(0)`, `CheckedIn(1)`, `CheckedOut(2)`, `Cancelled(3)`, `NoShow(4)`, `PendingPayment(5)`. ✅ **RESOLVED** — Code hiện tại **ĐÃ loại trừ** `CheckedOut` và `Cancelled`. Logic nhất quán với `BookingService` | ✅ **RESOLVED** — `CheckedOut` đã được thêm vào danh sách loại trừ. Test case ROOM-TC-006 không còn áp dụng                                                                                                       |
| L4 | Spec gốc của `BookingService` loại trừ `Cancelled` VÀ `CheckedOut` khi check overlap | `BookingService.CreateBookingAsync()` line 30-31: loại trừ `Cancelled` và `CheckedOut`. Cả `HomeService` và `BookingService` đều loại trừ `Cancelled` + `CheckedOut` — logic đã nhất quán                                                             | ✅ **RESOLVED** — Cả hai service đều loại trừ Cancelled + CheckedOut. Không còn inconsistency |
| L5 | `HomeController.SearchRooms()` khi nhận ngày không hợp lệ                                | Code thực tế: nếu `checkIn`/`checkOut` rỗng thì trả toàn bộ categories (không filter). Nếu parse fail thì fallback `Today`/`Today+1`. Nếu `CheckOut <= CheckIn` thì auto sửa thành `CheckIn + 1`                                           | Test phải cover: ngày rỗng, ngày invalid string, ngày hợp lệ, ngày trong quá khứ                                                                                                                                                                      |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi

Phạm vi của TDD spec này bao gồm các component liên quan đến logic kiểm tra phòng trống:

```
[Room Reservation - Availability Check] bao gồm các layer:
├── Models (PhucNguyenResort.Models)
│   ├── Booking (BookingStatus enum)
│   ├── Room (RoomStatus enum)
│   └── RoomCategory
├── Services
│   ├── HomeService.CheckAvailabilityAsync() ← ĐỐI TƯỢNG TEST CHÍNH
│   ├── HomeService.SearchAvailableCategoriesAsync()
│   └── RoomService.UpdateRoomAsync()
└── Controllers
    ├── HomeController.SearchRooms() ← TEST CONTROLLER
    └── RoomsController.Edit() ← TEST CONTROLLER
```

### TDS-02 — Test Basis / Cơ sở Kiểm thử

| Source                        | Items Derived                                                                                                             |
| ----------------------------- | ------------------------------------------------------------------------------------------------------------------------- |
| `SWP391-HOS-02.md` BR01     | Strict Concurrency — Phòng không được đặt trùng cho cùng khoảng thời gian                                     |
| `SWP391_NHOM1.md` UC04      | Guest tìm kiếm phòng theo preferences                                                                                  |
| `SWP391_NHOM1.md` UC06      | Guest xem chi tiết phòng                                                                                                |
| `SWP391_NHOM1.md` UC59      | Receptionist cập nhật trạng thái phòng                                                                               |
| `HomeService.cs` line 39-40 | Logic overlap:`b.CheckInDate < checkOutDate && b.CheckOutDate > checkInDate`                                            |
| `HomeService.cs` line 33    | Logic loại trừ phòng Maintenance:`r.Status != RoomStatus.Maintenance`                                                |
| `HomeService.cs` line 37-38 | Logic loại trừ booking (via `BookingDetails`):`BookingStatus != Cancelled && BookingStatus != CheckedOut`                                        |
| `HomeController.cs` line 71 | Date parsing formats:`dd/MM/yyyy`, `dd MM, yyyy`, `d MMMM, yyyy`, `dd MMMM, yyyy`, `MM/dd/yyyy`, `yyyy-MM-dd` |

### TDS-03 — Test Conditions and Coverage Items

| Condition ID | Test Condition                                                                 | Coverage Item                          | Test Cases         |
| ------------ | ------------------------------------------------------------------------------ | -------------------------------------- | ------------------ |
| TC-COND-001  | Tính toán cơ bản: không có booking nào → trả về TotalRooms           | `HomeService.CheckAvailabilityAsync` | `ROOM-TC-001`    |
| TC-COND-002  | Booking không trùng ngày (trước/sau khoảng search) → không trừ phòng | `HomeService.CheckAvailabilityAsync` | `ROOM-TC-002`    |
| TC-COND-003  | Booking trùng lấp ngày → trừ đúng số phòng (distinct RoomId)          | `HomeService.CheckAvailabilityAsync` | `ROOM-TC-003`    |
| TC-COND-004  | Phòng Maintenance → loại khỏi TotalRooms                                   | `HomeService.CheckAvailabilityAsync` | `ROOM-TC-004`    |
| TC-COND-005  | Booking Cancelled/CheckedOut → không trừ vào available                         | `HomeService.CheckAvailabilityAsync` | `ROOM-TC-005`    |
| TC-COND-006  | ✅ RESOLVED — CheckedOut đã được loại trừ khỏi available count                       | `HomeService.CheckAvailabilityAsync` | `ROOM-TC-006`    |
| TC-COND-007  | SearchRooms với ngày rỗng/invalid                                           | `HomeController.SearchRooms`         | `UC04_06_TC_001` |
| TC-COND-008  | SearchRooms với ngày hợp lệ                                                | `HomeController.SearchRooms`         | `UC04_06_TC_002` |
| TC-COND-009  | RoomsController.Edit → cập nhật status thành công                         | `RoomsController.Edit`               | `UC59_TC_001`    |
| TC-COND-010  | RoomsController.Edit → ID không khớp Model.RoomId                           | `RoomsController.Edit`               | `UC59_TC_002`    |
| TC-COND-011  | SearchAvailableCategoriesAsync lọc ra category có MaxAdults < adultsCount hoặc MaxChildren < childrenCount | `HomeService.SearchAvailableCategoriesAsync` | `ROOM-TC-007` |
| TC-COND-012  | SearchAvailableCategoriesAsync lọc ra category có số phòng trống < roomCount | `HomeService.SearchAvailableCategoriesAsync` | `ROOM-TC-008` |
| TC-COND-013  | SearchAvailableCategoriesAsync trả về danh sách các category hợp lệ kèm số lượng phòng trống chính xác | `HomeService.SearchAvailableCategoriesAsync` | `ROOM-TC-009` |

### TDS-04 — Test Techniques / Kỹ thuật Kiểm thử

| Technique (ISO 29119-4)  | Applied To                               | Rationale                                                                                                                |
| ------------------------ | ---------------------------------------- | ------------------------------------------------------------------------------------------------------------------------ |
| Equivalence Partitioning | BookingStatus values                     | Chia thành 2 nhóm: {Reserved, CheckedIn, NoShow, PendingPayment} (chiếm phòng) vs {Cancelled, CheckedOut} (không chiếm)            |
| Boundary Value Analysis  | Date Overlap (CheckIn/CheckOut boundary) | Đảm bảo xử lý đúng tại mốc ngày biên — ví dụ CheckOut người cũ = CheckIn người mới → KHÔNG overlap |
| State Transition Testing | RoomStatus enum transitions              | Verify logic khi Room chuyển từ Available → Maintenance → Available ảnh hưởng đến TotalRooms                    |
| Error Guessing           | Date parsing in Controller               | Nhập string không hợp lệ, ngày trong quá khứ, CheckOut ≤ CheckIn                                                 |

### TDS-05 — Test Data Requirements

| Fixture ID | Type    | Value / Logic                                                           | Mục đích                                             |
| ---------- | ------- | ----------------------------------------------------------------------- | ------------------------------------------------------- |
| FX-001     | Mock DB | CategoryId=1, 5 Rooms (Status=Available), 0 Bookings                    | Happy path — empty calendar                            |
| FX-002     | Mock DB | CategoryId=1, 5 Rooms, 2 Bookings overlap (Reserved, different RoomIds) | Test overlap trừ đúng distinct RoomId                |
| FX-003     | Mock DB | CategoryId=1, 5 Rooms (2 Available + 3 Maintenance), 0 Bookings         | Test Maintenance exclusion                              |
| FX-004     | Mock DB | CategoryId=1, 5 Rooms, 2 BookingDetails overlap (Booking Cancelled + Booking CheckedOut) | Test Cancelled/CheckedOut exclusion                         |
| FX-005     | Mock DB | CategoryId=1, 5 Rooms, 1 BookingDetail overlap (Booking NoShow)              | Test NoShow vẫn bị đếm occupied (không loại trừ) |
| FX-006     | Mock DB | CategoryId=1, 5 Rooms, 1 Booking NOT overlapping (before search range)  | Test non-overlap booking                                |
| FX-007     | Mock DB | Category 1 (MaxAdults=2, MaxChildren=1), Category 2 (MaxAdults=4, MaxChildren=2), 1 available room each | Test capacity filtering of SearchAvailableCategoriesAsync |
| FX-008     | Mock DB | Category 1, 2 rooms, 1 booking overlap (1 available room) | Test roomCount threshold filtering |
| FX-009     | Mock DB | Category 1 (5 rooms: 2 available, 3 maintenance), Category 2 (3 rooms: 1 available, 2 booked) | Test multiple categories search with available counts |

## 4. Test Case Specification

### ROOM-TC-001 — Trả về tổng số phòng khi không có Booking nào

**Severity**: CRITICAL
**Feature Under Test**: `HomeService.CheckAvailabilityAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/HomeServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-001

**Preconditions**:

- CategoryId = 1 có 5 phòng (Status = `RoomStatus.Available`).
- Không có bất kỳ Booking nào trong hệ thống.
- Fixture: FX-001

**Test Steps**:

1. Arrange: Mock `ResortDbContext` — 5 Rooms (CategoryId=1, Status=Available), 0 Bookings
2. Act: Gọi `CheckAvailabilityAsync(1, new DateTime(2027,1,1), new DateTime(2027,1,5))`
3. Assert: Kết quả == 5

**Expected Result (PASS)**:

- Trả về kết quả: `5`

**Expected Result (FAIL — dấu hiệu lỗi)**:

- Trả về < 5: Logic đếm TotalRooms sai (có thể filter thừa)
- Trả về > 5: Đếm phòng thuộc category khác

**Current Status**: 🟢 Passing
**Implementation Note**: Logic: `Rooms.CountAsync(r => r.CategoryId == categoryId && r.Status != RoomStatus.Maintenance)` - khi 0 booking, available = TotalRooms.

---

### ROOM-TC-002 — Trả về đúng số phòng khi Booking không bị trùng ngày

**Severity**: HIGH
**Feature Under Test**: `HomeService.CheckAvailabilityAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/HomeServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-002

**Preconditions**:

- CategoryId = 1 có 5 phòng (Status = `RoomStatus.Available`).
- Có 1 Booking cho phòng thuộc Category 1 từ `01/01/2027` đến `03/01/2027`, status = `BookingStatus.Reserved`.
- Fixture: FX-006

**Test Steps**:

1. Arrange: Mock DB — 5 Rooms, 1 Booking (CheckIn=01/01, CheckOut=03/01, Reserved)
2. Act: Gọi `CheckAvailabilityAsync(1, new DateTime(2027,1,5), new DateTime(2027,1,10))` — khoảng thời gian hoàn toàn KHÔNG giao nhau
3. Assert: Kết quả == 5

**Expected Result (PASS)**:

- Trả về kết quả: `5` (Booking cũ đã kết thúc trước khoảng search → không trừ)

**Expected Result (FAIL)**:

- Trả về < 5: Logic overlap sai, đếm cả booking không giao nhau

**Current Status**: 🟢 Passing
**Implementation Note**: Overlap condition: `b.CheckInDate < checkOutDate && b.CheckOutDate > checkInDate`. Booking (01/01→03/01) vs Search (05/01→10/01): `03/01 < 10/01 = true` nhưng `01/01 > 05/01 = false` → NOT overlap. ✅

---

### ROOM-TC-003 — Trừ đúng số phòng khi có Booking bị Overlap (Trùng ngày)

**Severity**: CRITICAL
**Feature Under Test**: `HomeService.CheckAvailabilityAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/HomeServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-003

**Preconditions**:

- CategoryId = 1 có 5 phòng (Status = `RoomStatus.Available`).
- Có 2 Bookings trên 2 phòng khác nhau (RoomId khác nhau), cùng thuộc Category 1, đều overlap khoảng search.
  - Booking A: RoomId=1, CheckIn=02/01, CheckOut=06/01, Status=Reserved
  - Booking B: RoomId=2, CheckIn=03/01, CheckOut=07/01, Status=CheckedIn
- Fixture: FX-002

**Test Steps**:

1. Arrange: Mock DB — 5 Rooms, 2 Bookings overlap trên 2 RoomId khác nhau
2. Act: Gọi `CheckAvailabilityAsync(1, new DateTime(2027,1,1), new DateTime(2027,1,5))`
3. Assert: Kết quả == 3

**Expected Result (PASS)**:

- Trả về kết quả: `3` (5 tổng - 2 phòng bị chiếm = 3)
- Giải thích: Search (01/01→05/01) giao cắt với cả Booking A (02/01→06/01) và Booking B (03/01→07/01). Mỗi booking thuộc 1 RoomId khác nhau → Distinct count = 2.

**Expected Result (FAIL)**:

- Trả về > 3: Không đếm đúng overlap
- Trả về < 3: Đếm trùng RoomId (thiếu `Distinct()`)

**Current Status**: 🟢 Passing
**Implementation Note**: Code truy vấn qua `BookingDetails`: `.Where(bd => bd.RoomCategoryId == categoryId && ...).CountAsync()` — đếm số BookingDetails có Booking overlap.

---

### ROOM-TC-004 — Phải loại bỏ phòng đang bị Maintenance khỏi TotalRooms

**Severity**: HIGH
**Feature Under Test**: `HomeService.CheckAvailabilityAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/HomeServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-004

**Preconditions**:

- CategoryId = 1 có 5 phòng: 2 phòng `RoomStatus.Available` + 3 phòng `RoomStatus.Maintenance`.
- Không có Booking.
- Fixture: FX-003

**Test Steps**:

1. Arrange: Mock DB — 5 Rooms (2 Available, 3 Maintenance), 0 Bookings
2. Act: Gọi `CheckAvailabilityAsync(1, new DateTime(2027,1,1), new DateTime(2027,1,5))`
3. Assert: Kết quả == 2

**Expected Result (PASS)**:

- Trả về kết quả: `2` (Chỉ đếm phòng `Status != RoomStatus.Maintenance`)

**Expected Result (FAIL)**:

- Trả về 5: Không filter Maintenance
- Trả về 0: Filter quá mức (có thể loại cả Available)

**Current Status**: 🟢 Passing
**Implementation Note**: Logic filter: `r.Status != PhucNguyenResort.Enums.RoomStatus.Maintenance`. Phòng `NeedsCleaning` và `Occupied` vẫn được đếm vào TotalRooms (vì status này là tạm thời, phòng vẫn bookable).

---

### ROOM-TC-005 — Bỏ qua (không trừ) các Booking đã bị Cancelled hoặc CheckedOut

**Severity**: HIGH
**Feature Under Test**: `HomeService.CheckAvailabilityAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/HomeServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-005

**Preconditions**:

- CategoryId = 1 có 5 phòng (Status = `RoomStatus.Available`).
- Có 2 Booking hoàn toàn trùng lịch (01/01→05/01) trên 2 RoomId khác nhau:
  - Booking A: RoomId=1, Status = `BookingStatus.Cancelled`
  - Booking B: RoomId=2, Status = `BookingStatus.CheckedOut`
- Fixture: FX-004

**Test Steps**:

1. Arrange: Mock DB — 5 Rooms, 2 Bookings (1 Cancelled, 1 CheckedOut), cùng overlap
2. Act: Gọi `CheckAvailabilityAsync(1, new DateTime(2027,1,1), new DateTime(2027,1,5))`
3. Assert: Kết quả == 5

**Expected Result (PASS)**:

- Trả về kết quả: `5` (Cả 2 Booking đều có status bị loại trừ → phòng được "trả lại" hệ thống)

**Expected Result (FAIL)**:

- Trả về 3: Đếm cả Cancelled/CheckedOut bookings → lỗi logic filter

**Current Status**: 🟢 Passing
**Implementation Note**: Filter condition: `b.BookingStatus != BookingStatus.Cancelled && b.BookingStatus != BookingStatus.CheckedOut`

---

### ROOM-TC-006 — ~~Booking CheckedOut vẫn bị đếm là occupied~~ ✅ RESOLVED

**Severity**: HIGH
**CWE**: N/A
**Legal**: Liên quan đến BR01 — đảm bảo tính nhất quán khi đếm phòng trống
**Feature Under Test**: `HomeService.CheckAvailabilityAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/HomeServiceTests.cs`
**TDD Phase**: ⬜ RESOLVED — CheckedOut đã được loại trừ, test case không còn áp dụng
**Condition Ref**: TC-COND-006

**Preconditions**:

- CategoryId = 1 có 5 phòng (Status = `RoomStatus.Available`).
- Có 1 Booking: RoomId=1, CheckIn=01/01, CheckOut=05/01, Status = `BookingStatus.CheckedOut`
- Fixture: FX-005

**Test Steps**:

1. Arrange: Mock DB — 5 Rooms, 1 Booking CheckedOut overlap
2. Act: Gọi `CheckAvailabilityAsync(1, new DateTime(2027,1,1), new DateTime(2027,1,5))`
3. Assert: Kết quả == 4

**Expected Result (PASS — theo behavior hiện tại của HomeService)**:

- Trả về kết quả: `4` (HomeService chỉ loại trừ Cancelled và NoShow, **KHÔNG** loại trừ CheckedOut → booking CheckedOut vẫn bị đếm)

**Expected Result (FAIL — nếu behavior thay đổi)**:

- Trả về 5: Nếu ai đó thêm `CheckedOut` vào danh sách loại trừ → cần ADR review

**Current Status**: ⬜ RESOLVED — Logic đã thay đổi, CheckedOut được loại trừ

> ✅ **Implementation Note**: Logic issue L4 (Section 2) **đã RESOLVED** (v1.2). `HomeService` hiện tại loại trừ `Cancelled` + `CheckedOut`, nhất quán với `BookingService`. Test case này không còn áp dụng — CheckedOut booking sẽ không bị đếm là occupied.

---

### ROOM-TC-007 — SearchAvailableCategoriesAsync lọc ra category có MaxAdults < adultsCount hoặc MaxChildren < childrenCount

**Severity**: HIGH
**Feature Under Test**: `HomeService.SearchAvailableCategoriesAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/HomeServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-011

**Preconditions**:
- Category 1: Capacity 2 adults, 1 child
- Category 2: Capacity 4 adults, 2 children
- Cả hai Category đều có 1 phòng trống (`RoomStatus.Available`)
- Fixture: FX-007

**Test Steps**:
1. Arrange: Thiết lập DB mock với Category 1 và Category 2.
2. Act: Gọi `SearchAvailableCategoriesAsync(new DateTime(2027, 1, 1), new DateTime(2027, 1, 5), adultsCount = 3, childrenCount = 1, roomCount = 1)`
3. Assert: Chỉ trả về Category 2 (vì Category 1 quá tải số người lớn tối đa)

**Expected Result (PASS)**:
- Danh sách trả về chứa duy nhất 1 category có `CategoryId = 2`

**Expected Result (FAIL)**:
- Trả về cả Category 1 (không lọc theo capacity) hoặc trả về danh sách rỗng

**Current Status**: 🟢 Passing

---

### ROOM-TC-008 — SearchAvailableCategoriesAsync lọc ra category có số phòng trống < roomCount

**Severity**: HIGH
**Feature Under Test**: `HomeService.SearchAvailableCategoriesAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/HomeServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-012

**Preconditions**:
- Category 1: Có 2 phòng, nhưng 1 phòng bị đặt trùng ngày (overlap).
- Số phòng trống thực tế = 1.
- Fixture: FX-008

**Test Steps**:
1. Arrange: Thiết lập DB mock với Category 1 và 1 booking overlap.
2. Act: Gọi `SearchAvailableCategoriesAsync(new DateTime(2027, 1, 1), new DateTime(2027, 1, 5), adultsCount = 2, childrenCount = 0, roomCount = 2)` (yêu cầu 2 phòng)
3. Assert: Trả về danh sách rỗng (vì phòng trống khả dụng chỉ có 1, nhỏ hơn roomCount yêu cầu)

**Expected Result (PASS)**:
- Kết quả trả về là một danh sách rỗng (`Empty`)

**Expected Result (FAIL)**:
- Trả về Category 1 mặc dù không đủ số phòng trống yêu cầu

**Current Status**: 🟢 Passing

---

### ROOM-TC-009 — SearchAvailableCategoriesAsync trả về danh sách các category hợp lệ kèm số lượng phòng trống chính xác

**Severity**: CRITICAL
**Feature Under Test**: `HomeService.SearchAvailableCategoriesAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/HomeServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-013

**Preconditions**:
- Category 1: Có 5 phòng (2 Available, 3 Maintenance). Số phòng trống khả dụng = 2.
- Category 2: Có 3 phòng (3 Available, nhưng 2 phòng bị booked overlap). Số phòng trống khả dụng = 1.
- Cả hai Category đều đáp ứng đủ sức chứa (adultsCount = 2, childrenCount = 1).
- Fixture: FX-009

**Test Steps**:
1. Arrange: Thiết lập DB mock với Category 1 (2 Available, 3 Maintenance) và Category 2 (3 Available, 2 Bookings overlap).
2. Act: Gọi `SearchAvailableCategoriesAsync(new DateTime(2027, 1, 1), new DateTime(2027, 1, 5), adultsCount = 2, childrenCount = 1, roomCount = 1)`
3. Assert: 
   - Trả về cả hai Category 1 và Category 2.
   - Category 1 có `AvailableRoomsCount = 2`.
   - Category 2 có `AvailableRoomsCount = 1`.

**Expected Result (PASS)**:
- Trả về đúng 2 category có `AvailableRoomsCount` lần lượt là 2 và 1.

**Expected Result (FAIL)**:
- Đếm sai số phòng trống của category hoặc loại bỏ sai một trong hai category.

**Current Status**: 🟢 Passing

---

### UC04_06_TC_001 — Tìm kiếm với ngày không hợp lệ (SearchRooms)

**Severity**: MEDIUM
**Feature Under Test**: `HomeController.SearchRooms()`
**Test File**: `PhucNguyenResort.Tests/Controllers/HomeControllerTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-007

**Preconditions**:

- `HomeController` đã inject `IHomeService` (Mock) và `ITourService` (Mock).
- `checkIn` và `checkOut` là chuỗi rỗng hoặc không hợp lệ.

**Test Steps**:

1. Arrange: Mock `IHomeService.GetRoomCategoriesForHomeAsync()` trả về danh sách categories
2. Act: Gọi `SearchRooms("", "", 2, 0, 1)` — chuỗi rỗng
3. Assert:
   - Trả về `ViewResult`
   - `ViewBag.IsFiltered = false`
   - Gọi `GetRoomCategoriesForHomeAsync()` (trả tất cả, không filter)

**Expected Result (PASS)**:

- Khi ngày rỗng: trả về tất cả categories, `ViewBag.IsFiltered = false`, không gọi `SearchAvailableCategoriesAsync`
- `ViewBag.CheckIn = ""`, `ViewBag.CheckOut = ""`

**Expected Result (FAIL)**:

- Throw exception khi parse ngày rỗng
- Gọi nhầm `SearchAvailableCategoriesAsync` với ngày mặc định

**Current Status**: 🟢 Passing
**Implementation Note**: Code check `!string.IsNullOrEmpty(checkIn) && !string.IsNullOrEmpty(checkOut)`. Nếu rỗng → trả toàn bộ categories mà không filter.

---

### UC04_06_TC_002 — Tìm kiếm với ngày hợp lệ (SearchRooms)

**Severity**: HIGH
**Feature Under Test**: `HomeController.SearchRooms()`
**Test File**: `PhucNguyenResort.Tests/Controllers/HomeControllerTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-008

**Preconditions**:

- `HomeController` đã inject `IHomeService` (Mock).
- `checkIn` = "01/12/2027", `checkOut` = "05/12/2027" (format `dd/MM/yyyy`).

**Test Steps**:

1. Arrange: Mock `IHomeService.SearchAvailableCategoriesAsync()` trả về danh sách categories
2. Act: Gọi `SearchRooms("01/12/2027", "05/12/2027", 2, 0, 1)`
3. Assert:
   - Trả về `ViewResult`
   - `ViewBag.IsFiltered = true`
   - `ViewBag.CheckIn = "01/12/2027"`, `ViewBag.CheckOut = "05/12/2027"`
   - `ViewBag.AdultsCount = 2`, `ViewBag.ChildrenCount = 0`, `ViewBag.RoomCount = 1`
   - Mock verify: `SearchAvailableCategoriesAsync` được gọi với `checkInDate = 2027-12-01`, `checkOutDate = 2027-12-05`, `adultsCount = 2`, `childrenCount = 0`, `roomCount = 1`

**Expected Result (PASS)**:

- Trả về danh sách RoomCategory chính xác
- Gọi đúng hàm `SearchAvailableCategoriesAsync` với đúng tham số đã parse

**Expected Result (FAIL)**:

- Parse sai format ngày (vd: 01/12 thành 12/01)
- Không set `ViewBag.IsFiltered = true`

**Current Status**: 🟢 Passing
**Implementation Note**: Controller dùng `TryParseExact` với mảng formats. Format `dd/MM/yyyy` nằm đầu mảng nên được ưu tiên.

---

### UC59_TC_001 — Cập nhật trạng thái phòng thành công (Edit)

**Severity**: CRITICAL
**Feature Under Test**: `RoomsController.Edit(int id, RoomEditVM model)`
**Test File**: `PhucNguyenResort.Tests/Controllers/RoomsControllerTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-009

**Preconditions**:

- `RoomsController` inject `IRoomService` (Mock) và `IRoomCategoryService` (Mock).
- Mock `IRoomService.GetRoomByIdAsync(1)` trả về Room với RoomId=1, Status=Available.
- Gửi lên model `RoomEditVM` với RoomId=1 và Status = `RoomStatus.Maintenance`.
- `ModelState.IsValid = true`.

**Test Steps**:

1. Arrange: Setup mock services, tạo `RoomEditVM { RoomId = 1, Status = RoomStatus.Maintenance, ... }`
2. Act: Gọi `Edit(1, model)`
3. Assert:
   - Mock verify: `GetRoomByIdAsync(1)` được gọi đúng 1 lần
   - Mock verify: `UpdateRoomAsync()` được gọi với Room có `Status = RoomStatus.Maintenance`
   - Kết quả là `RedirectToActionResult` → redirect đến `Index`

**Expected Result (PASS)**:

- Gọi `UpdateRoomAsync` với tham số Room đã cập nhật `Status = RoomStatus.Maintenance`
- Chuyển hướng (Redirect) thành công về trang Index

**Expected Result (FAIL)**:

- Không gọi `UpdateRoomAsync` → status không thay đổi
- Không redirect → trả về View (có thể do ModelState invalid)

**Current Status**: 🟢 Passing
**Implementation Note**: Controller flow: verify `id == model.RoomId` → get Room from DB → update fields → `UpdateRoomAsync` → `RedirectToAction("Index")`.

---

### UC59_TC_002 — Truyền vào ID không khớp với Model ID (Edit)

**Severity**: MEDIUM
**Feature Under Test**: `RoomsController.Edit(int id, RoomEditVM model)`
**Test File**: `PhucNguyenResort.Tests/Controllers/RoomsControllerTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-010

**Preconditions**:

- Truyền vào route ID = 1 nhưng model có `RoomId = 2` (mismatch).

**Test Steps**:

1. Arrange: Tạo `RoomEditVM { RoomId = 2, ... }`
2. Act: Gọi `Edit(1, model)` — route id=1, model id=2
3. Assert: Kết quả là `NotFoundResult` (HTTP 404)

**Expected Result (PASS)**:

- Trả về `NotFoundResult` (HTTP 404)
- **Không** gọi `UpdateRoomAsync` (không thay đổi DB)

**Expected Result (FAIL)**:

- Gọi `UpdateRoomAsync` → cập nhật sai phòng → lỗi data integrity

**Current Status**: 🟢 Passing
**Implementation Note**: Guard check đầu tiên: `if (id != model.RoomId) return NotFound();`

---

## 5. Red-Green-Refactor Tracker

| TC ID              | Test File                   | 🔴 RED confirmed | 🟢 GREEN (commit) | 🔵 REFACTOR note                                        |
| ------------------ | --------------------------- | ---------------- | ----------------- | ------------------------------------------------------- |
| `ROOM-TC-001`    | `HomeServiceTests.cs`     | `[x]`          | `[x]`           | —                                                      |
| `ROOM-TC-002`    | `HomeServiceTests.cs`     | `[x]`          | `[x]`           | —                                                      |
| `ROOM-TC-003`    | `HomeServiceTests.cs`     | `[x]`          | `[x]`           | Dùng `Distinct()` trên RoomId                       |
| `ROOM-TC-004`    | `HomeServiceTests.cs`     | `[x]`          | `[x]`           | —                                                      |
| `ROOM-TC-005`    | `HomeServiceTests.cs`     | `[x]`          | `[x]`           | —                                                      |
| `ROOM-TC-006`    | `HomeServiceTests.cs`     | `N/A`          | `N/A`           | ⬜ RESOLVED — CheckedOut đã được loại trừ (v1.2) |
| `ROOM-TC-007`    | `HomeServiceTests.cs`     | `[x]`          | `[x]`           | Lọc sức chứa (MaxAdults/MaxChildren) |
| `ROOM-TC-008`    | `HomeServiceTests.cs`     | `[x]`          | `[x]`           | Lọc số phòng khả dụng (roomCount) |
| `ROOM-TC-009`    | `HomeServiceTests.cs`     | `[x]`          | `[x]`           | Trả về AvailableRoomsCount tương ứng |
| `UC04_06_TC_001` | `HomeControllerTests.cs`  | `[x]`          | `[x]`           | —                                                      |
| `UC04_06_TC_002` | `HomeControllerTests.cs`  | `[x]`          | `[x]`           | —                                                      |
| `UC59_TC_001`    | `RoomsControllerTests.cs` | `[x]`          | `[x]`           | —                                                      |
| `UC59_TC_002`    | `RoomsControllerTests.cs` | `[x]`          | `[x]`           | —                                                      |

## 6. Entry / Exit Criteria

**Entry Criteria (Điều kiện bắt đầu)**

- [X] Spec kỹ thuật `SWP391-MOD2-IMP-002` (EDS_RoomBooking.md) đã được review
- [X] Logic Issues (Section 2) đã được confirm — đặc biệt L3, L4 cần team discussion
- [X] EF Core schema cho `Bookings`, `Rooms`, `RoomCategories` đã tạo
- [X] Test fixtures (Section 3 TDS-05) đã được chuẩn bị

**Exit Criteria (Điều kiện kết thúc — DoD)**

- [X] `dotnet test` — tất cả unit tests xanh (ROOM-TC-001 → TC-005, ROOM-TC-007 → TC-009, UC04_06_TC_001 → TC_002, UC59_TC_001 → TC_002)
- [x] `ROOM-TC-006` — ✅ RESOLVED (v1.2) — Logic issue L4 đã resolved, CheckedOut được loại trừ
- [ ] Test coverage ≥ 80% cho `HomeService.cs`, `HomeController.cs (SearchRooms)`, `RoomsController.cs (Edit)`
- [X] Không có hardcode string cho enum comparison (dùng `RoomStatus.Maintenance` thay vì `"Maintenance"`)
- [ ] Code review completed

**Suspension Criteria (Điều kiện tạm dừng)**

- ~~Logic issue L4 (`CheckedOut` exclusion inconsistency giữa `HomeService` và `BookingService`) chưa được resolve bởi Architecture team~~ ✅ RESOLVED (v1.2)
- Blocker: nếu schema `Bookings` hoặc `Rooms` thay đổi, cần cập nhật lại test data

## 7. Rollback Plan

```bash
# Revert test files (dev only)
git checkout -- PhucNguyenResort.Tests/Services/HomeServiceTests.cs
git checkout -- PhucNguyenResort.Tests/Controllers/HomeControllerTests.cs
git checkout -- PhucNguyenResort.Tests/Controllers/RoomsControllerTests.cs

# Nếu test phát hiện bug trong production code và đã fix:
git revert [commit_hash_of_fix]

# Logic issues vẫn OPEN → giữ nguyên entry trong Section 2
```
