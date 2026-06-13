# TEST-DRIVEN DEVELOPMENT SPECIFICATION TEMPLATE
Mẫu Đặc tả Kiểm thử Hướng Phát triển

**Document ID**: SWP391-MOD2-TDD-002
**Version**: 1.0
**Date**: 2026-06-12
**Status**: Draft
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: Cuong
**Reviewed by**: Khanh
**DPO Sign-off**: Khanh
**Approved by**: Cuong
**Classification**: Internal - Confidential

**References**:
- `01_SRS/SWP391_NHOM1.md` — Functional requirements (UC22)
- `01_SRS/SWP391-HOS-02.md` — Functional requirements (UC07)
- `03_implement/EDS_RoomBooking.md` — Technical Specification
- NĐ 356/2025 — Legal basis

## CHANGELOG
| Ngày | Người thực hiện | Nội dung thay đổi |
| --- | --- | --- |
| 2026-06-12 | AI Assistant | Khởi tạo tài liệu — TDD spec cho Room Booking |

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
| Feature / Gap ID | GAP-002 |
| Module | `Room Reservation & Front Desk - Booking` |
| Spec gốc | `SWP391-MOD2-IMP-002` |
| Priority | 🔴 P0 |
| Sprint | S2 |
| Milestone | M3 Alpha |
| Data Classification | Sensitive-PII / PII |
| Compliance Scope | NĐ 356/2025 |
| Upstream Dependencies | `RoomAvailabilityService` |
| Downstream Consumers | `PaymentGateway` |

## 2. Logic Issues Resolved
| # | Spec gốc (sai / thiếu) | Thực tế (schema / policy) | Fix áp dụng trong test |
| --- | --- | --- | --- |
| L1 | Chưa rõ mức cọc | Thống nhất cọc 50% | `CreateBooking` sẽ tính `DepositAmount = TotalPrice / 2` |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi
`RoomBooking` bao gồm các layer:
├── Domain (pure logic - no deps)
├── Application / Use Cases (mock EF Core / In-Memory DB)
├── Services
└── Controller (mock services)

### TDS-02 — Test Basis / Cơ sở Kiểm thử
| Source | Items Derived |
| --- | --- |
| `SRS.md` UC07 | Guest books room and pays deposit securely |
| `ADR-002` | Optimistic Concurrency Handle |
| `BR01` | Strict Concurrency (Double-Booking Prevention) |
| `BR03` | Cancellation Policy (100% refund if >48h) |

### TDS-03 — Test Conditions and Coverage Items
| Condition ID | Test Condition | Coverage Item | Test Cases |
| --- | --- | --- | --- |
| TC-COND-001 | Tạo Booking thành công khi phòng trống | `CreateBookingAsync` | `BKG-TC-001` |
| TC-COND-002 | Trả về lỗi khi phòng đã được đặt (Concurrency) | `CreateBookingAsync` | `BKG-TC-002` |
| TC-COND-003 | Ngày CheckIn không hợp lệ (nhỏ hơn hiện tại) | `CreateBookingAsync` | `BKG-TC-003` |
| TC-COND-004 | Hủy đặt phòng trên 48h (Hoàn cọc) | `CancelBookingAsync` | `BKG-TC-004` |
| TC-COND-005 | Hủy đặt phòng dưới 48h (Không hoàn cọc) | `CancelBookingAsync` | `BKG-TC-005` |

### TDS-04 — Test Techniques / Kỹ thuật Kiểm thử
| Technique (ISO 29119-4)  | Applied To                                 | Rationale                                     |
| --------------------------| --------------------------------------------| -----------------------------------------------|
| Equivalence Partitioning | `CheckInDate`, `CheckOutDate`              | Phân vùng ngày hợp lệ và không hợp lệ         |
| Boundary Value Analysis  | Thời gian hủy (48h)                        | Test đúng 48h, 47h59m, 48h01m                 |
| Error Guessing           | Concurrency `DbUpdateConcurrencyException` | Khách hàng bấm đúp, hoặc 2 khách đặt cùng lúc |

### TDS-05 — Test Data Requirements
| Fixture ID | Type | Value / Logic | Mục đích |
| --- | --- | --- | --- |
| FX-001 | DB seed | Room Status = Available, User = 'testuser' | Happy path |

## 4. Test Case Specification

### BKG-TC-001 — Khách đặt phòng thành công khi phòng trống
**Severity**: CRITICAL
**Feature Under Test**: `BookingService.CreateBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-001

**Preconditions**:
- Phòng Id = 1 trống từ ngày 01/01/2027 đến 05/01/2027.
- User đăng nhập hợp lệ.

**Test Steps**:
1. Gọi `CreateBookingAsync(roomId: 1, checkIn: "01/01/2027", checkOut: "05/01/2027", userId: 1)`

**Expected Result (PASS)**:
- Booking được tạo thành công.
- `BookingStatus` là `Pending`.
- `DepositAmount` được tính bằng 50% `TotalPrice`.

### BKG-TC-002 — Báo lỗi khi phòng đã được đặt (Concurrency Conflict)
**Severity**: CRITICAL
**Feature Under Test**: `BookingService.CreateBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-002

**Preconditions**:
- Phòng Id = 1 đã được Booking #1 giữ chỗ từ 01/01/2027 đến 05/01/2027.

**Test Steps**:
1. Khách hàng B gọi `CreateBookingAsync(roomId: 1, checkIn: "02/01/2027", checkOut: "04/01/2027", userId: 2)`

**Expected Result (PASS)**:
- Ném ra `Exception` hoặc trả về lỗi `BKG-002: Room not available`.
- KHÔNG có Booking nào được lưu thêm vào DB.

### BKG-TC-003 — Báo lỗi khi ngày Check-in/Check-out không hợp lệ
**Severity**: HIGH
**Feature Under Test**: `BookingService.CreateBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-003

**Test Steps**:
1. Gọi `CreateBookingAsync` với `CheckOutDate <= CheckInDate`.

**Expected Result (PASS)**:
- Lỗi `BKG-001: Invalid dates`.

### BKG-TC-004 — Hủy phòng > 48h trước giờ Check-in (Hoàn cọc 100%)
**Severity**: HIGH
**Feature Under Test**: `BookingService.CancelBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-004

**Preconditions**:
- Booking #1 `CheckInDate` = `05/01/2027 14:00`, `Status` = `Reserved`, `DepositAmount` = 1,000,000.
- Giờ hiện tại: `02/01/2027 10:00` (Cách > 48 tiếng).

**Test Steps**:
1. Gọi `CancelBookingAsync(1, userId)`

**Expected Result (PASS)**:
- `BookingStatus` = `Cancelled`.
- Trả về thông báo hoàn cọc số tiền 1,000,000.

### BKG-TC-005 — Hủy phòng < 48h trước giờ Check-in (Không hoàn cọc)
**Severity**: HIGH
**Feature Under Test**: `BookingService.CancelBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-005

**Preconditions**:
- Booking #2 `CheckInDate` = `05/01/2027 14:00`.
- Giờ hiện tại: `04/01/2027 10:00` (Cách < 48 tiếng).

**Test Steps**:
1. Gọi `CancelBookingAsync(2, userId)`

**Expected Result (PASS)**:
- `BookingStatus` = `Cancelled`.
- Trả về thông báo Không hoàn cọc (RefundAmount = 0).

## 5. Red-Green-Refactor Tracker
| TC ID | Test File | 🔴 RED | 🟢 GREEN | 🔵 REFACTOR |
| --- | --- | --- | --- | --- |
| `BKG-TC-001` | `BookingServiceTests.cs` | `[x]` | `[x]` | |
| `BKG-TC-002` | `BookingServiceTests.cs` | `[x]` | `[x]` | |
| `BKG-TC-003` | `BookingServiceTests.cs` | `[x]` | `[x]` | |
| `BKG-TC-004` | `BookingServiceTests.cs` | `[x]` | `[x]` | |
| `BKG-TC-005` | `BookingServiceTests.cs` | `[x]` | `[x]` | |

## 6. Entry / Exit Criteria
**Entry Criteria**
- [ ] Spec kỹ thuật `EDS_RoomBooking.md` đã được review
- [ ] Prisma/EF Core schema đã cập nhật `RowVersion`
- [ ] Các fixture test DB đã chuẩn bị

**Exit Criteria**
- [ ] Tất cả unit tests `BKG-TC-*` đều PASS.
- [ ] Coverage logic hủy phòng đạt 100%.

## 7. Rollback Plan
- Reset nhánh `git checkout main`.
- Drop in-memory/test DB tables liên quan.
