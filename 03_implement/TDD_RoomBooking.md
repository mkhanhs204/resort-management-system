# TEST-DRIVEN DEVELOPMENT SPECIFICATION TEMPLATE
Đặc tả Kiểm thử Hướng Phát triển — Room Booking

**Document ID**: SWP391-MOD2-TDD-002
**Version**: 1.1
**Date**: 2026-06-13
**Status**: In Review
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: Cuong
**Reviewed by**: [ ] Khanh - Pending
**DPO Sign-off**: [ ] Khanh - Pending
**Approved by**: [ ] Pending
**Classification**: Internal - Confidential

**References**:
- `01_SRS/SWP391_NHOM1.md` — Functional requirements (UC22, UC28)
- `01_SRS/SWP391-HOS-02.md` — Functional requirements (UC07)
- `03_implement/EDS_RoomBooking.md` — Technical Specification (SWP391-MOD2-IMP-002)
- NĐ 356/2025 — Legal basis (Data Privacy cho BookingGuest)

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.
Thứ tự bắt buộc: viết test → chạy → xác nhận FAIL 🔴 → implement → PASS 🟢 → refactor 🔵.
Test data dùng classification `SYNTHETIC`. Không dùng PII thật.

## CHANGELOG
**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ.

| Ngày | Người thực hiện | Nội dung thay đổi |
| --- | --- | --- |
| 2026-06-12 | AI Assistant | Khởi tạo tài liệu — TDD spec cho Room Booking |
| 2026-06-13 | AI BA Review | Cập nhật v1.1: Khớp trạng thái `BookingStatus.Reserved`, sửa `TotalAmount`, xóa yêu cầu `RowVersion` sai, bổ sung Test Case cho Tour Add-ons. |

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
| Field | Value |
| --- | --- |
| Feature / Gap ID | FEAT-BOOKING-CORE |
| Module | `Room Reservation & Front Desk - Booking` |
| Spec gốc | `SWP391-MOD2-IMP-002` |
| Priority | 🔴 P0 |
| Sprint | S2 |
| Milestone | M1 - Core Booking Functions |
| Data Classification | Sensitive-PII / PII (Thông tin CCCD khách hàng lưu qua BookingGuest) |
| Compliance Scope | NĐ 356/2025 |
| Upstream Dependencies | `RoomService`, `HomeService`, `TourService` |
| Downstream Consumers | `PaymentGateway` |

## 2. Logic Issues Resolved
| # | Spec gốc (sai / thiếu) | Thực tế (schema / policy) | Fix áp dụng trong test |
| --- | --- | --- | --- |
| L1 | Chưa rõ mức cọc, trường lưu trữ | Thống nhất cọc 50%. Tên property là `TotalAmount`, `DepositAmount`. | `CreateBooking` sẽ tính `DepositAmount = TotalAmount / 2` |
| L2 | Booking Status sau khi tạo là `Pending` | Enum `BookingStatus` không có Pending, trạng thái mặc định là `Reserved` | Test kiểm tra sau khi tạo Booking thì `Status == BookingStatus.Reserved` |
| L3 | Yêu cầu `RowVersion` cho DbUpdateConcurrencyException | Model `Booking` không lưu `RowVersion`. Dùng Application-Level overlap check + default EF concurrency | Bỏ yêu cầu EF migration `RowVersion` ở mục Entry Criteria. |
| L4 | Thiếu xử lý Tour Add-ons | Codebase cho phép `SelectedTourScheduleIds` khi tạo booking | Bổ sung test case kiểm tra tạo `TourBooking` tương ứng. |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi
`RoomBooking` bao gồm các layer:
```
├── Models (Booking, BookingGuest, TourBooking)
├── Services
│   ├── BookingService.CreateBookingAsync() ← ĐỐI TƯỢNG TEST CHÍNH
│   └── BookingService.CancelBookingAsync() ← ĐỐI TƯỢNG TEST CHÍNH
└── Controllers
    └── BookingsController
```

### TDS-02 — Test Basis / Cơ sở Kiểm thử
| Source | Items Derived |
| --- | --- |
| `SRS.md` UC22 | Guest books room and pays deposit securely |
| `ADR-002` | Application-level Overlap Check |
| `BR01` | Strict Concurrency (Double-Booking Prevention) |
| `BR03` | Cancellation Policy (100% refund if >48h) |

### TDS-03 — Test Conditions and Coverage Items
| Condition ID | Test Condition | Coverage Item | Test Cases |
| --- | --- | --- | --- |
| TC-COND-001 | Tạo Booking thành công khi phòng trống | `CreateBookingAsync` | `BKG-TC-001` |
| TC-COND-002 | Trả về lỗi khi phòng đã được đặt (Concurrency) | `CreateBookingAsync` | `BKG-TC-002` |
| TC-COND-003 | Ngày CheckIn không hợp lệ (nhỏ hơn hiện tại) | `CreateBookingAsync` | `BKG-TC-003` |
| TC-COND-004 | Tạo Booking kèm Tour Add-on | `CreateBookingAsync` | `BKG-TC-004` |
| TC-COND-005 | Hủy đặt phòng trên 48h (Hoàn cọc) | `CancelBookingAsync` | `BKG-TC-005` |
| TC-COND-006 | Hủy đặt phòng dưới 48h (Không hoàn cọc) | `CancelBookingAsync` | `BKG-TC-006` |

### TDS-04 — Test Techniques / Kỹ thuật Kiểm thử
| Technique (ISO 29119-4)  | Applied To                                 | Rationale                                     |
| --------------------------| --------------------------------------------| -----------------------------------------------|
| Equivalence Partitioning | `CheckInDate`, `CheckOutDate`              | Phân vùng ngày hợp lệ và không hợp lệ         |
| Boundary Value Analysis  | Thời gian hủy (48h)                        | Test đúng 48h, 47h59m, 48h01m                 |
| Error Guessing           | Overlap logic                              | Test việc check overlap loại trừ `Cancelled` và `CheckedOut` |

### TDS-05 — Test Data Requirements
| Fixture ID | Type | Value / Logic | Mục đích |
| --- | --- | --- | --- |
| FX-001 | Mock DB | RoomId = 1, Price = 500k | Happy path CreateBooking |
| FX-002 | Mock DB | RoomId = 1, Booking overlap tồn tại | Conflict/Concurrency test |
| FX-003 | Mock DB | TourScheduleId = 5, Tour.Price = 200k | Tour Add-on test |
| FX-004 | Mock DB | BookingId = 10, CheckIn = Now + 72h | Hủy phòng hoàn cọc |

## 4. Test Case Specification

### BKG-TC-001 — Khách đặt phòng thành công khi phòng trống
**Severity**: CRITICAL
**Feature Under Test**: `BookingService.CreateBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-001

**Preconditions**:
- Phòng Id = 1 (Price: 500,000) trống từ ngày 01/01/2027 đến 05/01/2027 (4 đêm).
- Fixture: FX-001

**Test Steps**:
1. Gọi `CreateBookingAsync` với request `roomId: 1, checkIn: "01/01/2027", checkOut: "05/01/2027", NumberOfAdults: 2, NumberOfChildren: 0` và `userId: 1`.

**Expected Result (PASS)**:
- `Success` = true.
- DB lưu `BookingStatus` là `Reserved`.
- Tính đúng `TotalAmount` = 4 * 500,000 = 2,000,000.
- `DepositAmount` được tính = 1,000,000 (50% TotalAmount).

### BKG-TC-002 — Báo lỗi khi phòng đã được đặt (Concurrency Conflict)
**Severity**: CRITICAL
**Feature Under Test**: `BookingService.CreateBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-002

**Preconditions**:
- Phòng Id = 1 đã có 1 Booking từ 01/01/2027 đến 05/01/2027 có trạng thái `Reserved` (FX-002).

**Test Steps**:
1. Khách hàng B gọi `CreateBookingAsync(roomId: 1, checkIn: "02/01/2027", checkOut: "04/01/2027")`.

**Expected Result (PASS)**:
- `Success` = false, `ErrorCode` = "BKG-002", `ErrorMessage` = "Room not available".
- KHÔNG có Booking mới nào được lưu.

### BKG-TC-003 — Báo lỗi khi ngày Check-in/Check-out không hợp lệ
**Severity**: HIGH
**Feature Under Test**: `BookingService.CreateBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-003

**Test Steps**:
1. Gọi `CreateBookingAsync` với `CheckOutDate <= CheckInDate` hoặc `CheckInDate < DateTime.Today`.

**Expected Result (PASS)**:
- `Success` = false, `ErrorCode` = "BKG-001", `ErrorMessage` = "Invalid dates".

### BKG-TC-004 — Khách đặt phòng kèm chọn Add-on Tour
**Severity**: HIGH
**Feature Under Test**: `BookingService.CreateBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-004

**Preconditions**:
- Phòng Id = 1 trống.
- `TourScheduleId = 5` có `Tour.Price = 200,000`. (FX-003)

**Test Steps**:
1. Gọi `CreateBookingAsync` với `SelectedTourScheduleIds = [5]` và `NumberOfAdults = 2, NumberOfChildren = 0`.

**Expected Result (PASS)**:
- Booking phòng được tạo thành công.
- Kèm theo 1 record `TourBooking` gắn liền `BookingId` vừa sinh.
- `TourBooking.TotalAmount` = 200,000 * 2 = 400,000.
- `TourBooking.BookingStatus` = `Pending`.

### BKG-TC-005 — Hủy phòng > 48h trước giờ Check-in (Hoàn cọc 100%)
**Severity**: HIGH
**Feature Under Test**: `BookingService.CancelBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-005

**Preconditions**:
- Booking ID = 10, CheckInDate cách hiện tại 72h. DepositAmount = 1,000,000. (FX-004)

**Test Steps**:
1. Gọi `CancelBookingAsync(10, userId)`

**Expected Result (PASS)**:
- `Success` = true.
- `BookingStatus` đổi thành `Cancelled`.
- Trả về `RefundAmount` = 1,000,000.

### BKG-TC-006 — Hủy phòng < 48h trước giờ Check-in (Không hoàn cọc)
**Severity**: HIGH
**Feature Under Test**: `BookingService.CancelBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-006

**Preconditions**:
- Booking ID = 11, CheckInDate cách hiện tại 24h.

**Test Steps**:
1. Gọi `CancelBookingAsync(11, userId)`

**Expected Result (PASS)**:
- `Success` = true.
- `BookingStatus` đổi thành `Cancelled`.
- Trả về `RefundAmount` = 0.

## 5. Red-Green-Refactor Tracker
| TC ID | Test File | 🔴 RED | 🟢 GREEN | 🔵 REFACTOR |
| --- | --- | --- | --- | --- |
| `BKG-TC-001` | `BookingServiceTests.cs` | `[x]` | `[x]` | |
| `BKG-TC-002` | `BookingServiceTests.cs` | `[x]` | `[x]` | |
| `BKG-TC-003` | `BookingServiceTests.cs` | `[x]` | `[x]` | |
| `BKG-TC-004` | `BookingServiceTests.cs` | `[x]` | `[x]` | |
| `BKG-TC-005` | `BookingServiceTests.cs` | `[x]` | `[x]` | |
| `BKG-TC-006` | `BookingServiceTests.cs` | `[x]` | `[x]` | |

## 6. Entry / Exit Criteria
**Entry Criteria**
- [x] Spec kỹ thuật `EDS_RoomBooking.md` đã được review
- [x] Các class EF Core DbContext, Model relationships đã cấu hình chuẩn.
- [x] Các fixture test DB đã chuẩn bị (In-Memory Database dùng cho test).

**Exit Criteria**
- [ ] Tất cả unit tests `BKG-TC-*` đều PASS.
- [ ] Logic Overlap Coverage đạt 100%.

## 7. Rollback Plan
```bash
# Revert test files (dev only)
git checkout -- PhucNguyenResort.Tests/Services/BookingServiceTests.cs
```
