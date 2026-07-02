# TEST-DRIVEN DEVELOPMENT SPECIFICATION

Đặc tả Kiểm thử Hướng Phát triển — Reception Check-in

**Document ID**: PNR-TDD-RECEPTION-001
**Version**: 1.1
**Date**: 2026-06-16
**Status**: Approve
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: Cuong
**Reviewed by**: Khanh
**DPO Sign-off**: Khanh
**Approved by**: Cuong
**Classification**: Internal - Confidential

**References**:

- `01_SRS/SWP391_NHOM1.md` — Functional requirements (UC53, UC54, UC55, UC59)
- `01_SRS/SWP391-HOS-02.md` — Problem Statement & BR01 (Double-Booking Prevention), UC09
- `03_implement/EDS_CHECKIN.md` — Technical Specification
- `template/TDD_TEMPLATE_V1.md` — TDD Template v1.0

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.
Thứ tự bắt buộc: viết test → chạy → xác nhận FAIL 🔴 → implement → PASS 🟢 → refactor 🔵.
Test data dùng classification `SYNTHETIC`. Không dùng PII thật.

## CHANGELOG

**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ.

| Ngày      | Người thực hiện | Nội dung thay đổi                                                                                                                                                                |
| ---------- | ------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 2026-06-16 | Cuong               | Khởi tạo tài liệu — TDD cho logic Check-in                                                                                                                                     |
| 2026-06-16 | Cuong               | Chỉnh sửa toàn bộ (v1.1): Đồng bộ cấu trúc chuẩn với EDS và TDD của Room Availability, cập nhật đầy đủ Test Cases, Tracker, Data Requirements và Rollback plan. |
| 2026-06-19 | Cuong               | Cập nhật phiên bản v1.2: Bổ sung 3 test cases mới (REC-TC-004/005/006) để kiểm thử bộ lọc phòng khả dụng, validation gán phòng, và cập nhật REC-TC-001 theo cấu trúc RoomAssignments mới. |

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

| Field                 | Value                                                                                        |
| --------------------- | -------------------------------------------------------------------------------------------- |
| Feature / Gap ID      | FEAT-RECEPTION-01                                                                            |
| Module                | `Reception Check-in` (Front Office)                                                        |
| Spec gốc             | `PNR-RECEPTION-IMP-001` (`EDS_CHECKIN.md`)                                               |
| Priority              | 🟢 P0 — Core workflow để khách nhận phòng                                             |
| Sprint                | S2                                                                                           |
| Milestone             | M2 — Front Desk Operations                                                                  |
| Data Classification   | Sensitive-PII (Xử lý Passport, CCCD)                                                       |
| Compliance Scope      | Luật Cư trú 2020, Nghị định 356/2025/NĐ-CP                                            |
| Upstream Dependencies | `Room` (Model + `RoomStatus`), `Booking` (Model + `BookingStatus`), `BookingGuest` |
| Downstream Consumers  | `ReceptionController.CheckIn()`, `Housekeeping Module`                                   |

## 2. Logic Issues Resolved

Bắt buộc điền trước khi viết test. Liệt kê mọi sai lệch giữa spec thiết kế và codebase thực tế.

| #  | Spec gốc (sai / thiếu)                               | Thực tế (schema / codebase)                                                                                                            | Fix áp dụng trong test                                                                                     |
| -- | ------------------------------------------------------ | ---------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------ |
| L1 | Controller xử lý trực tiếp thay đổi DB           | Pattern mới yêu cầu `Thin Controller, Fat Service`.                                                                                 | Đưa toàn bộ logic `CheckInAsync` vào `ReceptionService` và viết test cho file Service.            |
| L2 | Cập nhật BookingStatus nhưng quên đổi RoomStatus | Trong quá trình check-in, phòng được gán phải đổi từ `Available` sang `Occupied`. Code cũ không thực hiện việc này. | Test phải Assert `Room.Status == RoomStatus.Occupied`. Buộc developer phải sửa service để test pass. |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi

Phạm vi của TDD spec này bao gồm các component liên quan đến logic check-in:

```
[Reception Check-in] bao gồm các layer:
├── Models (PhucNguyenResort.Models)
│   ├── Booking (BookingStatus enum)
│   ├── Room (RoomStatus enum)
│   └── BookingGuest (PII info)
├── Services
│   ├── ReceptionService.ProcessCheckInAsync() ← ĐỐI TƯỢNG TEST CHÍNH
│   └── BookingGuestService.SaveBookingGuestsAsync()
└── Controllers
    └── ReceptionController.CheckInConfirmed()
```

### TDS-02 — Test Basis / Cơ sở Kiểm thử

| Source                   | Items Derived                                                           |
| ------------------------ | ----------------------------------------------------------------------- |
| `SWP391_NHOM1.md` UC53 | Thực hiện Check-in                                                    |
| `SWP391_NHOM1.md` UC54 | Gán phòng vật lý                                                    |
| `SWP391_NHOM1.md` UC59 | Receptionist cập nhật trạng thái phòng (từ Available -> Occupied) |
| `SWP391_NHOM1.md` UC55 | Thu thập thông tin cư trú khách hàng                              |

### TDS-03 — Test Conditions and Coverage Items

| Condition ID | Test Condition                                                                         | Coverage Item                            | Test Cases     |
| ------------ | -------------------------------------------------------------------------------------- | ---------------------------------------- | -------------- |
| TC-COND-001  | Check-in thành công với trạng thái Reserved, lưu đủ Guest, gán phòng vật lý và đổi đúng Status | `ReceptionService.ProcessCheckInAsync` | `REC-TC-001` |
| TC-COND-002  | Check-in với trạng thái không hợp lệ (Cancelled) → trả về False + Message     | `ReceptionService.ProcessCheckInAsync` | `REC-TC-002` |
| TC-COND-003  | ID Đặt phòng không tồn tại → trả về False + Message                           | `ReceptionService.ProcessCheckInAsync` | `REC-TC-003` |
| TC-COND-004  | Lọc phòng khả dụng chính xác loại phòng, loại trừ bảo trì hoặc đã gán trùng lịch lưu trú | `ReceptionService.GetAvailableRoomsForCategoryAsync` | `REC-TC-004` |
| TC-COND-005  | Check-in thất bại do thiếu gán phòng vật lý                                   | `ReceptionService.ProcessCheckInAsync` | `REC-TC-005` |
| TC-COND-006  | Check-in thất bại do danh sách gán phòng trống                                 | `ReceptionService.ProcessCheckInAsync` | `REC-TC-006` |

### TDS-04 — Test Techniques / Kỹ thuật Kiểm thử

| Technique (ISO 29119-4)  | Applied To                     | Rationale                                                          |
| ------------------------ | ------------------------------ | ------------------------------------------------------------------ |
| State Transition Testing | BookingStatus enum transitions | Verify logic khi Booking chuyển từ Reserved → CheckedIn         |
| State Transition Testing | RoomStatus enum transitions    | Verify logic khi Room chuyển từ Available → Occupied            |
| Error Guessing           | Invalid Bookings in Service    | Truyền ID ảo (999), truyền Booking có status Cancelled, NoShow |

### TDS-05 — Test Data Requirements

| Fixture ID | Type    | Value / Logic                                  | Mục đích                   |
| ---------- | ------- | ---------------------------------------------- | ----------------------------- |
| FX-001     | Mock DB | RoomId=101 (Available), BookingId=1 (Reserved), BookingDetailId=1 | Happy path — Valid check-in  |
| FX-002     | Mock DB | BookingId=2 (Cancelled)                        | Test Invalid status rejection |
| FX-003     | Mock DB | Trống (BookingId=999)                         | Test Not Found rejection      |
| FX-004     | Mock DB | Các phòng category=1 (101-Available, 102-Maint, 103-Overlapping, 104-Non-overlap) | Test Room Availability query  |
| FX-005     | Mock DB | BookingId=3 (Reserved), BookingDetailId=301 (SelectedRoomId=null) | Test Missing assignment error |

## 4. Test Case Specification

### REC-TC-001 — Check-in thành công Happy Path

**Severity**: CRITICAL
**Feature Under Test**: `ReceptionService.ProcessCheckInAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/ReceptionServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-001

**Preconditions**:

- DB có `Booking` với ID=1 trạng thái `Reserved`.
- DB có `Room` với ID=101 tồn tại trạng thái `Available`.
- Dữ liệu Request model truyền lên hợp lệ chứa ít nhất 1 `BookingGuestDto` và gán phòng vật lý hợp lệ `SelectedRoomId=101`.
- Fixture: FX-001

**Test Steps**:

1. Arrange: Khởi tạo InMemory `ResortDbContext`. Mock `IBookingGuestService`.
2. Act: Gọi `ProcessCheckInAsync(PreCheckinViewModel)`.
3. Assert: Kiểm tra boolean Result và state của Db.

**Expected Result (PASS)**:

- Kết quả Success = `true`.
- `Booking.BookingStatus` trong Db chuyển thành `CheckedIn`.
- `Room.Status` trong Db chuyển thành `Occupied`.

**Expected Result (FAIL — dấu hiệu lỗi)**:

- Trả về false.
- Hoặc trạng thái Booking / Room không được cập nhật do thiếu Entity tracking hoặc thiếu `SaveChangesAsync`.

**Current Status**: 🟢 Passing
**Implementation Note**: Lúc đầu test FAIL do L2 (code chưa cập nhật RoomStatus). Đã fix code service để cập nhật room status. ✅

---

### REC-TC-002 — Check-in thất bại do sai trạng thái

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.ProcessCheckInAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/ReceptionServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-002

**Preconditions**:

- DB có `Booking` trạng thái `Cancelled`.
- Fixture: FX-002

**Test Steps**:

1. Arrange: Tạo db chứa BookingId=2 với Status=`Cancelled`.
2. Act: Gọi `ProcessCheckInAsync(PreCheckinViewModel với ID=2)`.
3. Assert: Kiểm tra result.

**Expected Result (PASS)**:

- Hàm trả về `Success = false` và `ErrorMessage = "Đơn đặt phòng này không ở trạng thái có thể Check-in."`
- `BookingStatus` trong Db giữ nguyên là `Cancelled`.

**Expected Result (FAIL)**:

- Báo Success = true dù trạng thái bị hủy.

**Current Status**: 🟢 Passing

---

### REC-TC-003 — Check-in thất bại do không tìm thấy Booking

**Severity**: MEDIUM
**Feature Under Test**: `ReceptionService.ProcessCheckInAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/ReceptionServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-003

**Preconditions**:

- Truyền vào một `BookingId` không tồn tại trong Database (ID=999).
- Fixture: FX-003

**Test Steps**:

1. Arrange: Khởi tạo db trống.
2. Act: Gọi `ProcessCheckInAsync(PreCheckinViewModel với ID=999)`.
3. Assert: Kiểm tra result.

**Expected Result (PASS)**:

- Hàm trả về `Success = false` và `ErrorMessage` là "Không tìm thấy thông tin đặt phòng."
- Hệ thống không bị crash (NullReferenceException).

**Expected Result (FAIL)**:

- Throw exception.

**Current Status**: 🟢 Passing
**Implementation Note**: Đã bắt được Null thông qua câu lệnh `if (booking == null) return false;`

---

### REC-TC-004 — Lọc phòng khả dụng chính xác loại phòng và tính trạng thái vận hành

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.GetAvailableRoomsForCategoryAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/ReceptionServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-004

**Preconditions**:
- DB có các phòng cùng category=1, trong đó:
  - Phòng 101: `Available` (trống hoàn toàn).
  - Phòng 102: `Maintenance` (bảo trì).
  - Phòng 103: `Available` nhưng bị gán bởi một booking khác có ngày lưu trú trùng.
  - Phòng 104: `Available` nhưng được gán bởi một booking khác có ngày lưu trú không trùng.
  - Phòng 201: Khác category=2.
- Fixture: FX-004

**Test Steps**:
1. Arrange: Cấu hình DB và gán dữ liệu như Preconditions.
2. Act: Gọi `GetAvailableRoomsForCategoryAsync(categoryId: 1, checkIn, checkOut)`.
3. Assert: Kiểm tra số lượng và danh sách phòng trả về.

**Expected Result (PASS)**:
- Chỉ trả về danh sách chứa 2 phòng là 101 và 104.
- Không chứa phòng 102 (đang bảo trì), 103 (bị trùng ngày) và 201 (khác loại).

**Current Status**: 🟢 Passing

---

### REC-TC-005 — Check-in thất bại do thiếu thông tin gán phòng vật lý

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.ProcessCheckInAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/ReceptionServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-005

**Preconditions**:
- DB có booking ở trạng thái `Reserved`.
- Người dùng truyền request model lên chứa chi tiết đặt phòng nhưng có `SelectedRoomId = null`.
- Fixture: FX-005

**Test Steps**:
1. Arrange: Cấu hình DB chứa booking cần check-in.
2. Act: Gọi `ProcessCheckInAsync` với model có `SelectedRoomId = null`.
3. Assert: Kiểm tra kết quả trả về.

**Expected Result (PASS)**:
- Hàm trả về `Success = false` và `ErrorMessage = "Vui lòng gán phòng vật lý cho tất cả các chi tiết đặt phòng."`

**Current Status**: 🟢 Passing

---

### REC-TC-006 — Check-in thất bại do danh sách gán phòng rỗng

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.ProcessCheckInAsync()`
**Test File**: `PhucNguyenResort.Tests/Services/ReceptionServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-006

**Preconditions**:
- DB có booking ở trạng thái `Reserved`.
- Request model truyền lên có `RoomAssignments = empty`.

**Test Steps**:
1. Arrange: Cấu hình DB chứa booking.
2. Act: Gọi `ProcessCheckInAsync` với danh sách gán phòng trống.
3. Assert: Kiểm tra kết quả trả về.

**Expected Result (PASS)**:
- Hàm trả về `Success = false` và `ErrorMessage = "Danh sách gán phòng trống."`

**Current Status**: 🟢 Passing

---

## 5. Red-Green-Refactor Tracker

| TC ID          | Test File                    | 🔴 RED confirmed | 🟢 GREEN (commit) | 🔵 REFACTOR note                                              |
| -------------- | ---------------------------- | ---------------- | ----------------- | ------------------------------------------------------------- |
| `REC-TC-001` | `ReceptionServiceTests.cs` | `[x]`          | `[x]`           | Cập nhật test data để truyền danh sách `RoomAssignments` |
| `REC-TC-002` | `ReceptionServiceTests.cs` | `[x]`          | `[x]`           | —                                                            |
| `REC-TC-003` | `ReceptionServiceTests.cs` | `[x]`          | `[x]`           | —                                                            |
| `REC-TC-004` | `ReceptionServiceTests.cs` | `[x]`          | `[x]`           | Kiểm tra gộp các điều kiện lọc trong GetAvailableRooms      |
| `REC-TC-005` | `ReceptionServiceTests.cs` | `[x]`          | `[x]`           | Báo lỗi trực quan khi thiếu phòng vật lý                       |
| `REC-TC-006` | `ReceptionServiceTests.cs` | `[x]`          | `[x]`           | Chặn request thiếu toàn bộ danh sách gán phòng                |

## 6. Entry / Exit Criteria

**Entry Criteria (Điều kiện bắt đầu)**

- [X] Spec kỹ thuật `EDS_CHECKIN.md` đã được hoàn thiện.
- [X] Logic Issues (Section 2) đã được confirm và thảo luận.
- [X] EF Core schema cho `Bookings`, `Rooms` đã tạo.
- [X] Test fixtures đã được chuẩn bị.

**Exit Criteria (Điều kiện kết thúc — DoD)**

- [X] TDD spec được bổ sung chi tiết các test cases `REC-TC-001` -> `REC-TC-006`.
- [X] `dotnet test` — tất cả unit tests xanh (129/129 tests pass).
- [X] Môi trường lock file được xử lý.
- [X] Code review completed.

**Suspension Criteria (Điều kiện tạm dừng)**

- Blocker: Thiếu model logic hay Database context schema chưa sync với test class.
- File đang bị lock bởi `dotnet watch`.

## 7. Rollback Plan

```bash
# Revert test files (dev only)
git checkout -- PhucNguyenResort.Tests/Services/ReceptionServiceTests.cs

# Nếu test phát hiện bug trong production code và đã fix:
git revert [commit_hash_of_fix]

# Logic issues vẫn OPEN → giữ nguyên entry trong Section 2
```
