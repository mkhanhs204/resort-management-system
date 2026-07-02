# TEST-DRIVEN DEVELOPMENT SPECIFICATION TEMPLATE

Mẫu Đặc tả Kiểm thử Hướng Phát triển

**Document ID**: PHUCNGUYEN-TDD-TOURBOOKING-001
**Version**: 1.0
**Date**: 2026-06-18
**Status**: Approved
**Classification**: Internal - Confidential

## CHANGELOG

| Ngày      | Người thực hiện | Nội dung thay đổi                            |
| ---------- | ------------------- | ----------------------------------------------- |
| 2026-06-18 | AI                  | Khởi tạo tài liệu TDD spec cho Tour Booking |

---

## 1. Thông tin Module

| Field                | Value                              |
| -------------------- | ---------------------------------- |
| Feature / Gap ID     | GAP-TOUR-002                       |
| Module               | `TourBookingModule`              |
| Spec gốc            | `PHUCNGUYEN-TOURBOOKING-IMP-001` |
| Priority             |  P0                               |
| Sprint               | S3                                 |
| Downstream Consumers | `BillingModule`                  |

## 2. Logic Issues Resolved

| #  | Spec gốc (sai / thiếu)                               | Thực tế (schema / policy)   | Fix áp dụng trong test                                                      |
| -- | ------------------------------------------------------ | ----------------------------- | ----------------------------------------------------------------------------- |
| L1 | Chưa rõ xử lý khóa chỗ khi 2 người cùng đặt | Áp dụng Transaction locking | Phải test concurrent requests để đảm bảo AvailableSlots không bị âm. |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi

`TourBookingModule` bao gồm các layer:
├── Domain Models (Tour, TourSchedule, TourBooking)
├── Services (TourBookingService)
├── Controllers (TourBookingsController)
└── Integration (Entity Framework In-Memory / Test SQL DB)

### TDS-03 — Test Conditions and Coverage Items

| Condition ID | Test Condition                                            | Coverage Item            | Test Cases             |
| ------------ | --------------------------------------------------------- | ------------------------ | ---------------------- |
| TC-COND-001  | Đặt tour thành công với số lượng vé hợp lệ     | `BookTourAsync`        | `TOURBOOKING-TC-001` |
| TC-COND-002  | Đặt tour vượt quá số chỗ trống `AvailableSlots` | `BookTourAsync`        | `TOURBOOKING-TC-002` |
| TC-COND-003  | Charge to Room nhưng `BookingId` không tồn tại      | `ValidateChargeToRoom` | `TOURBOOKING-TC-003` |

## 4. Test Case Specification

### TOURBOOKING-TC-001 — Happy path Book Tour

**Severity**: CRITICAL
**Feature Under Test**: `TourBookingService.BookTourAsync()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:

- DB có sẵn 1 `TourSchedule` với `AvailableSlots` = 10.
- `UserId` hợp lệ.

**Test Steps**:

1. Gọi `BookTourAsync` với `Participants` = 2.
2. Kiểm tra kết quả trả về.
3. Kiểm tra DB xem `AvailableSlots` có giảm xuống 8 không.

**Expected Result (PASS)**:

- Kết quả trả về chứa `tourBookingId`.
- `AvailableSlots` của schedule = 8.

**Current Status**: 🟢 Passing

---

### TOURBOOKING-TC-002 — Prevent Overbooking (AvailableSlots Exceeded)

**Severity**: CRITICAL
**Feature Under Test**: `TourBookingService.BookTourAsync()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:

- DB có sẵn 1 `TourSchedule` với `AvailableSlots` = 3.

**Test Steps**:

1. Gọi `BookTourAsync` với `Participants` = 5.
2. Bắt exception hoặc lỗi trả về.

**Expected Result (PASS)**:

- Method throw exception hoặc trả về lỗi "Not enough available slots" (Code: TB-001).
- `AvailableSlots` vẫn giữ nguyên là 3.

**Current Status**: 🟢 Passing

---

### TOURBOOKING-TC-003 — Charge To Room validation failure

**Severity**: HIGH
**Feature Under Test**: `TourBookingService.BookTourAsync()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:

- `PaymentMethod` = "ChargeToRoom".
- Cung cấp một `BookingId` sai, hoặc `BookingId` thuộc về một phòng đã Check-out.

**Test Steps**:

1. Gọi `BookTourAsync`.
2. Kiểm tra logic xác thực `BookingId`.

**Expected Result (PASS)**:

- Lỗi "Invalid Booking ID for Charge to Room" (Code: TB-002 hoặc TB-004).
- Giao dịch không được lưu vào CSDL.

**Current Status**: 🟢 Passing

---

## 5. Red-Green-Refactor Tracker

| TC ID                  | Test File                      | 🔴 RED confirmed | 🟢 GREEN (commit) | 🔵 REFACTOR note |
| ---------------------- | ------------------------------ | ---------------- | ----------------- | ---------------- |
| `TOURBOOKING-TC-001` | `TourBookingServiceTests.cs` | `[x]`          | `[x]`           |                  |
| `TOURBOOKING-TC-002` | `TourBookingServiceTests.cs` | `[x]`          | `[x]`           |                  |
| `TOURBOOKING-TC-003` | `TourBookingServiceTests.cs` | `[x]`          | `[x]`           |                  |
