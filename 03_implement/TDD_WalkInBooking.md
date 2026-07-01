# TEST-DRIVEN DEVELOPMENT SPECIFICATION

Đặc tả Kiểm thử Hướng Phát triển — Đặt Phòng Tại Quầy (Walk-in Booking)

**Document ID**: SWP391-MOD2-TDD-003
**Version**: 1.0
**Date**: 2026-06-21
**Status**: Approve
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: Cuong
**DPO Sign-off**: Khanh
**Approved by**: Cuong
**Classification**: Internal - Confidential

**References**:

- `01_SRS/SWP391_NHOM1_V2.md` — Use Cases Description (UC49, UC51, UC55)
- `01_SRS/SWP391-HOS-02.md` — Problem Statement & Guest Cycle (UC08, UC09, UC21)
- `03_implement/EDS_WalkInBooking.md` — Technical Specification (SWP391-MOD2-IMP-003)
- NĐ 356/2025/NĐ-CP & Luật Cư trú 2020 — Cơ sở pháp lý cho việc khai báo tạm trú và bảo vệ dữ liệu cá nhân.

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.
Thứ tự bắt buộc: viết test → chạy → xác nhận FAIL 🔴 → implement → PASS 🟢 → refactor 🔵.
Test data dùng classification `SYNTHETIC`. Không dùng PII thật.

---

## CHANGELOG

**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ.

| Ngày      | Người thực hiện | Nội dung thay đổi                                                            |
| ---------- | ------------------- | ------------------------------------------------------------------------------- |
| 2026-06-21 | Cuong               | Khởi tạo tài liệu — TDD spec cho Walk-in Booking (Đặt phòng tại quầy) |

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

| Field                 | Value                                                                                     |
| --------------------- | ----------------------------------------------------------------------------------------- |
| Feature / Gap ID      | FEAT-WALKIN-BOOKING                                                                       |
| Module                | `Room Reservation & Front Desk - Walk-in Booking`                                       |
| Spec gốc             | `SWP391-MOD2-IMP-003`                                                                   |
| Priority              | 🟢                                                                                        |
| Sprint                | S2                                                                                        |
| Milestone             | M2 - Front Desk Operational Functions                                                     |
| Data Classification   | Sensitive-PII / PII (Thông tin định danh CCCD/Passport của các khách ở tại quầy) |
| Compliance Scope      | NĐ 356/2025 (PDPD) & Luật Cư trú 2020                                                 |
| Upstream Dependencies | `Auth Module (Users, Roles)`, `Room Management Module (Rooms, RoomCategories)`        |
| Downstream Consumers  | `Consolidated Billing Module (GuestFolio, Invoice, Payments)`                           |

---

## 2. Logic Issues Resolved

| #  | Spec gốc (sai / thiếu)                        | Thực tế (schema / policy)                                                                             | Fix áp dụng trong test                                                                                                                                                               |
| -- | ----------------------------------------------- | ------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| L1 | Khách đặt tại quầy không có tài khoản. | Model`Booking` yêu cầu `UserId` bắt buộc (`[Required]`).                                      | Hệ thống tự động tìm kiếm`User` qua SĐT/Email. Nếu chưa có, tự động tạo mới `User` với Role `Customer` và mật khẩu ngẫu nhiên để liên kết.             |
| L2 | Thanh toán cọc qua VNPay.                     | Khách tại quầy thanh toán trực tiếp qua Tiền mặt (Cash), Quẹt thẻ (POS), hoặc Bank Transfer. | `WalkInBooking` sẽ ghi nhận trực tiếp giao dịch `Payment` ngoại tuyến, bỏ qua cổng VNPay và đổi trạng thái thanh toán ngay.                                         |
| L3 | Quy trình 2 bước: tạo đơn rồi check-in.  | Gây mất thời gian cho lễ tân khi khách muốn nhận phòng ngay.                                   | Hỗ trợ checkbox`InstantCheckIn`. Khi bật, hệ thống tự động gán phòng vật lý, lưu thông tin định danh và đổi trạng thái phòng sang `Occupied` ngay lập tức. |

---

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi

`WalkInBooking` bao gồm các layer kiểm thử:

```
├── Models (Booking, BookingGuest, User, Payment)
├── Services
│   └── ReceptionService.ProcessWalkInBookingAsync() ← ĐỐI TƯỢNG TEST CHÍNH
└── Controllers
    └── ReceptionController.WalkInBooking()
```

### TDS-02 — Test Basis / Cơ sở Kiểm thử

| Source              | Items Derived                                                              |
| ------------------- | -------------------------------------------------------------------------- |
| `SRS.md` UC49     | Receptionist quản lý và tạo đơn đặt phòng trực tiếp tại quầy. |
| `SRS.md` UC51     | Receptionist thực hiện check-in cho khách.                              |
| `BR-WK-01`        | Ràng buộc bắt buộc phải liên kết`UserId` (tự sinh nếu mới).    |
| `BR-WK-02`        | Chấp nhận hình thức thanh toán ngoại tuyến (Tiền mặt, POS).       |
| `BR-WK-04`        | Quy tắc gán số phòng vật lý trực tiếp khi đặt phòng tại quầy. |
| Luật Cư trú 2020 | Thu thập CCCD/Passport khi làm thủ tục nhận phòng trực tiếp.       |

### TDS-03 — Test Conditions and Coverage Items

| Condition ID | Test Condition                                                                               | Coverage Item                 | Test Cases     |
| ------------ | -------------------------------------------------------------------------------------------- | ----------------------------- | -------------- |
| TC-COND-001  | Đặt phòng tại quầy thành công cho khách hàng cũ                                    | `ProcessWalkInBookingAsync` | `WKB-TC-001` |
| TC-COND-002  | Đặt phòng tại quầy thành công cho khách mới (tự tạo User)                         | `ProcessWalkInBookingAsync` | `WKB-TC-002` |
| TC-COND-003  | Chặn đặt phòng tại quầy nếu số điện thoại trống hoặc không đúng format       | `ProcessWalkInBookingAsync` | `WKB-TC-003` |
| TC-COND-004  | Đặt phòng kèm nhận phòng ngay (Instant Check-in) thành công                          | `ProcessWalkInBookingAsync` | `WKB-TC-004` |
| TC-COND-005  | Đặt phòng tại quầy với thanh toán Tiền mặt (Cash)                                   | `ProcessWalkInBookingAsync` | `WKB-TC-005` |
| TC-COND-006  | Đặt phòng tại quầy với thanh toán quẹt thẻ (POS)                                    | `ProcessWalkInBookingAsync` | `WKB-TC-006` |
| TC-COND-007  | Chặn đặt phòng tại quầy nếu phòng vật lý được chọn đang bận (Double Booking) | `ProcessWalkInBookingAsync` | `WKB-TC-007` |
| TC-COND-008  | Báo lỗi khi Instant Check-in nhưng thiếu thông tin định danh của Khách chính       | `ProcessWalkInBookingAsync` | `WKB-TC-008` |

### TDS-04 — Test Techniques / Kỹ thuật Kiểm thử

| Kỹ thuật (ISO 29119-4) | Áp dụng cho                           | Lý do                                                                                                                         |
| ------------------------ | --------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------ |
| Equivalence Partitioning | SĐT khách hàng, Ngày lưu trú      | Phân vùng SĐT hợp lệ/không hợp lệ, ngày Check-in/Out hợp lệ.                                                        |
| State Transition Testing | Room Status, Booking Status             | Kiểm tra sự chuyển trạng thái phòng`Available` -> `Occupied` và trạng thái Booking `Reserved` -> `CheckedIn`. |
| Error Guessing           | Trùng phòng vật lý gán trực tiếp | Kiểm tra trường hợp 2 lễ tân gán cùng 1 phòng vật lý trống tại một thời điểm.                                 |

### TDS-05 — Test Data Requirements

| Fixture ID | Type    | Value / Logic                                             | Mục đích                |
| ---------- | ------- | --------------------------------------------------------- | -------------------------- |
| FX-WKB-001 | Mock DB | RoomId = 101, Status = Available, Price = 1,000,000       | Phòng vật lý sẵn sàng |
| FX-WKB-002 | Mock DB | User { UserId = 10, Phone = "0987654321" }                | Khách hàng cũ           |
| FX-WKB-003 | Mock DB | RoomId = 101 đã bị gán cho Booking khác trùng lịch | Conflict Test              |

---

## 4. Test Case Specification

### WKB-TC-001 — Đặt phòng tại quầy thành công cho khách hàng cũ

**Severity**: CRITICAL
**Feature Under Test**: `ReceptionService.ProcessWalkInBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-001

**Preconditions**:

- Có sẵn RoomId = 101 (Hạng phòng 1 - Price: 1,000,000/đêm).
- Có sẵn User có số điện thoại "0987654321" (UserId = 10) trong DB.
- Fixture: FX-WKB-001, FX-WKB-002

**Test Steps**:

1. Gọi `ProcessWalkInBookingAsync` với request:
   - `CustomerName`: "Nguyen Van A"
   - `PhoneNumber`: "0987654321"
   - `Email`: "nguyenvala@gmail.com"
   - `CheckInDate`: Today + 2 days
   - `CheckOutDate`: Today + 4 days (2 đêm)
   - `SelectedRoomIds`: `[101]`
   - `InstantCheckIn`: false
   - `PaidAmount`: 2,000,000
   - `PaymentMethod`: "Cash"

**Expected Result (PASS)**:

- `Success` = true.
- DB không sinh ra User mới (tổng số User giữ nguyên).
- `Booking` được tạo liên kết với `UserId = 10`.
- Trạng thái `BookingStatus` là `Reserved`.
- Tổng tiền `TotalAmount` = 2,000,000 VND.
- Đã tạo bản ghi `Payment` với `Amount = 2,000,000`, `PaymentMethod = Cash`.
- Phòng 101 vẫn ở trạng thái `Available` (vì chưa tới ngày check-in).

---

### WKB-TC-002 — Đặt phòng tại quầy thành công cho khách mới (Tự sinh tài khoản)

**Severity**: CRITICAL
**Feature Under Test**: `ReceptionService.ProcessWalkInBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-002

**Preconditions**:

- Có sẵn RoomId = 101.
- Không tồn tại số điện thoại "0911222333" trong hệ thống.

**Test Steps**:

1. Gọi `ProcessWalkInBookingAsync` với request:
   - `CustomerName`: "Tran Thi B"
   - `PhoneNumber`: "0911222333"
   - `Email`: "tranthib@gmail.com"
   - `CheckInDate`: Today + 1 day
   - `CheckOutDate`: Today + 2 days (1 đêm)
   - `SelectedRoomIds`: `[101]`
   - `InstantCheckIn`: false
   - `PaidAmount`: 1,000,000
   - `PaymentMethod`: "POS"

**Expected Result (PASS)**:

- `Success` = true.
- Một User mới được tạo trong DB với SĐT "0911222333", Role là `Customer`, `IsActive = true`.
- `Booking` được tạo liên kết với `UserId` mới này.
- `BookingStatus` là `Reserved`.
- Ghi nhận `Payment` thành công với `Method = POS`, `Amount = 1,000,000`.

---

### WKB-TC-003 — Chặn đặt phòng tại quầy nếu số điện thoại không hợp lệ

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.ProcessWalkInBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-003

**Test Steps**:

1. Gọi `ProcessWalkInBookingAsync` với `PhoneNumber` để trống hoặc chữ cái ("abcde").

**Expected Result (PASS)**:

- `Success` = false.
- `ErrorCode` = "WKB-001".
- `ErrorMessage` chứa thông tin yêu cầu số điện thoại hợp lệ.
- Không có User hay Booking nào được tạo.

---

### WKB-TC-004 — Đặt phòng kèm nhận phòng luôn (Instant Check-in) thành công

**Severity**: CRITICAL
**Feature Under Test**: `ReceptionService.ProcessWalkInBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-004

**Preconditions**:

- RoomId = 101 đang ở trạng thái `Available` và sạch sẽ.
- Khách muốn check-in hôm nay (CheckInDate = Today, CheckOutDate = Tomorrow).
- Điền đầy đủ thông tin định danh CCCD của khách lưu trú.

**Test Steps**:

1. Gọi `ProcessWalkInBookingAsync` với request:
   - `CustomerName`: "Le Van C"
   - `PhoneNumber`: "0955555555"
   - `CheckInDate`: Today
   - `CheckOutDate`: Tomorrow
   - `SelectedRoomIds`: `[101]`
   - `InstantCheckIn`: true
   - `PaidAmount`: 1,000,000
   - `PaymentMethod`: "Cash"
   - `PdpdConsentChecked`: true
   - `Guests`: Một danh sách chứa 1 khách chính:
     - `FullName`: "Le Van C", `DateOfBirth`: 1990-01-01, `Gender`: Male, `PassportNumber`: "001090123456", `Nationality`: "Vietnam", `IsPrimaryBooker`: true.

**Expected Result (PASS)**:

- `Success` = true.
- `Booking` được tạo với `BookingStatus` = `CheckedIn`.
- Trạng thái phòng vật lý 101 chuyển sang `Occupied` (Đang có khách).
- Bản ghi `BookingGuest` được tạo với thông tin CCCD "001090123456" liên kết với `BookingId`.
- Đã kích hoạt tạo `GuestFolio` nợ = 0 (vì đã thanh toán đủ 1,000,000 VND).

---

### WKB-TC-005 — Đặt phòng tại quầy với thanh toán Tiền mặt (Cash)

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.ProcessWalkInBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-005

**Test Steps**:

1. Gọi `ProcessWalkInBookingAsync` với `PaymentMethod = "Cash"` và `PaidAmount = 500,000` (đặt cọc một phần).

**Expected Result (PASS)**:

- `Success` = true.
- Bản ghi `Payment` được tạo với `PaymentMethod = Cash`, `Amount = 500,000`.
- Booking được lưu thành công. Số tiền còn thiếu được ghi nhận trên Folio.

---

### WKB-TC-006 — Đặt phòng tại quầy với thanh toán quẹt thẻ (POS)

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.ProcessWalkInBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-006

**Test Steps**:

1. Gọi `ProcessWalkInBookingAsync` với `PaymentMethod = "POS"` và `PaidAmount = 1,000,000`.

**Expected Result (PASS)**:

- `Success` = true.
- Bản ghi `Payment` được tạo với `PaymentMethod = POS`, `Amount = 1,000,000`.

---

### WKB-TC-007 — Chặn đặt phòng tại quầy nếu phòng vật lý được chọn đang bận (Double Booking)

**Severity**: CRITICAL
**Feature Under Test**: `ReceptionService.ProcessWalkInBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-007

**Preconditions**:

- Phòng 101 đã được gán cho một Booking khác từ ngày Today đến Today + 3 ngày (FX-WKB-003).

**Test Steps**:

1. Lễ tân thực hiện tạo đơn đặt phòng tại quầy chọn phòng 101 lưu trú từ ngày Today + 1 ngày đến Today + 2 ngày.

**Expected Result (PASS)**:

- `Success` = false.
- `ErrorCode` = "WKB-002".
- `ErrorMessage` báo lỗi phòng đã được đặt bởi khách khác trong khoảng thời gian được chọn.
- Không có Booking mới được tạo.

---

### WKB-TC-008 — Báo lỗi khi Instant Check-in nhưng thiếu thông tin định danh của Khách chính

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.ProcessWalkInBookingAsync()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-008

**Test Steps**:

1. Gọi `ProcessWalkInBookingAsync` với `InstantCheckIn = true` nhưng danh sách `Guests` trống hoặc Khách chính không có số CCCD/Passport.

**Expected Result (PASS)**:

- `Success` = false.
- `ErrorCode` = "WKB-003".
- `ErrorMessage` yêu cầu nhập thông tin định danh hợp lệ cho khách lưu trú khi thực hiện check-in trực tiếp.
- Không tạo Booking và không đổi trạng thái phòng.

---

## 5. Red-Green-Refactor Tracker

| TC ID          | Test File                 | 🔴 RED  | 🟢 GREEN | 🔵 REFACTOR |
| -------------- | ------------------------- | ------- | -------- | ----------- |
| `WKB-TC-001` | `WalkInBookingTests.cs` | `[x]` | `[x]`  |             |
| `WKB-TC-002` | `WalkInBookingTests.cs` | `[x]` | `[x]`  |             |
| `WKB-TC-003` | `WalkInBookingTests.cs` | `[x]` | `[x]`  |             |
| `WKB-TC-004` | `WalkInBookingTests.cs` | `[x]` | `[x]`  |             |
| `WKB-TC-005` | `WalkInBookingTests.cs` | `[x]` | `[x]`  |             |
| `WKB-TC-006` | `WalkInBookingTests.cs` | `[x]` | `[x]`  |             |
| `WKB-TC-007` | `WalkInBookingTests.cs` | `[x]` | `[x]`  |             |
| `WKB-TC-008` | `WalkInBookingTests.cs` | `[x]` | `[x]`  |             |

---

## 6. Entry / Exit Criteria

**Entry Criteria (Điều kiện bắt đầu)**

- [ ] Spec kỹ thuật `EDS_WalkInBooking.md` đã được duyệt.
- [ ] Cấu hình DbContext và các liên kết khóa ngoại User, Booking, BookingGuest đã ổn định.
- [ ] Cấu hình dự án test `PhucNguyenResort.Tests` sẵn sàng.

**Exit Criteria (Điều kiện kết thúc)**

- [ ] Tất cả unit tests `WKB-TC-*` đều PASS 🟢 (Không được phép skip).
- [ ] Code Coverage đạt tối thiểu 85% cho Service xử lý đặt phòng tại quầy.
- [ ] Tích hợp trơn tru với các API kiểm tra trùng phòng (concurrency).

---

## 7. Rollback Plan

```bash
# Revert các file test vừa tạo
git checkout -- PhucNguyenResort.Tests/Bookings/WalkInBookingTests.cs
```
