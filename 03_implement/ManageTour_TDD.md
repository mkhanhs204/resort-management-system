# TEST-DRIVEN DEVELOPMENT SPECIFICATION

## Module: Tour Management (Quản lý Tour)

**Document ID**: SWP391-TOUR-TDD-001
**Version**: 1.0
**Date**: 2026-06-18
**Status**: Draft
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: `Nguyễn Khắc Hiếu`
**Reviewed by**: [ ] `truong cuong`
**DPO Sign-off**: [ ] N/A *(Module không trực tiếp xử lý Sensitive-PII)*
**Approved by**: [ ] Nguyễn Khắc Hiếu
**Classification**: Internal

**References**:

- `01_SRS/SWP391_NHOM1.md` — Tài liệu SRS chính (UC11, UC12, UC26)
- `01_SRS/SWP391-HOS-02.md` — Đề bài dự án (Module 4: Tour Booking & Feedback)
- `01_SRS/Manager_Tour_SRS.md` — SRS chi tiết Tour Management
- `03_implement/ManageTour_EDS.md` — Technical Specification (EDS)
- Luật Du lịch Việt Nam 2017 — Chương III (Quyền và nghĩa vụ nhà cung cấp dịch vụ du lịch)

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.
Thứ tự bắt buộc: viết test → chạy → xác nhận FAIL 🔴 → implement → PASS 🟢 → refactor 🔵.
Test data dùng dữ liệu SYNTHETIC. Không dùng PII thật.

## CHANGELOG

**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ.

| Ngày      | Người thực hiện | Nội dung thay đổi                                         |
| ---------- | ------------------- | ------------------------------------------------------------ |
| 2026-06-18 | `[Tên]`          | Khởi tạo tài liệu — TDD spec cho Tour Management Module |

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
| Feature / Gap ID      | TOUR-MGMT-001                                                                                |
| Module                | `Tour Management - Tourism & Activities`                                                   |
| Spec gốc             | `SWP391-TOUR-IMP-001` (ManageTour_EDS.md)                                                  |
| Priority              | 🟠 P1                                                                                        |
| Sprint                | S[N] (YYYY-MM-DD -> YYYY-MM-DD)                                                              |
| Milestone             | M3 Alpha - 2026-07-11                                                                        |
| Data Classification   | Internal                                                                                     |
| Compliance Scope      | Luật Du lịch Việt Nam 2017, BR-TM-01/02/03                                                |
| Upstream Dependencies | `Auth Module (Users, Roles)`, `Room Reservation (Bookings)`, `Payment Module`          |
| Downstream Consumers  | `Consolidated Billing (GuestFolios, Invoices)`, `Notification Module`, `Review Module` |

---

## 2. Logic Issues Resolved

Bắt buộc điền trước khi viết test. Liệt kê mọi sai lệch giữa spec thiết kế và thực tế.

| #  | Spec gốc (sai / thiếu)                                                                 | Thực tế (schema / policy)                                                                       | Fix áp dụng trong test                                                                                            |
| -- | ---------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- |
| L1 | SRS gốc (UC26) không mô tả rõ khi nào Customer được ChargeToRoom                | Schema yêu cầu `BookingId` (FK → Bookings) phải NOT NULL khi PaymentMethod = 'ChargeToRoom' | Test case validate: ChargeToRoom phải có BookingId active; nếu thiếu → throw `NoActiveRoomBookingException`  |
| L2 | SRS không nêu rõ cách tính `TotalPrice`                                           | EDS spec:`TotalPrice = Tour.BasePrice × NumberOfParticipants`                                  | Test case verify phép tính nhân giá, bao gồm edge case participants = 0                                        |
| L3 | SRS chỉ nêu "capacity" nhưng không mô tả `AvailableSlots` hoạt động thế nào | Schema có `AvailableSlots` trên `TourSchedules`, giảm khi booking, tăng khi cancel        | Test case verify AvailableSlots tăng/giảm đúng; verify invariant:`TotalBooked + AvailableSlots = MaxCapacity` |
| L4 | BR03 nêu "48 giờ" nhưng không nêu tính từ field nào                              | EDS spec: tính từ `TourSchedules.DepartureDate` + `DepartureTime` (nếu có)                | Test case dùng `DepartureDate` kết hợp `DepartureTime` để tính khoảng cách 48h                          |
| L5 | SRS không nêu rõ Tour Guide chỉ được xem manifest của chính mình               | EDS Authorization Matrix: Tour Guide chỉ xem "Own" manifest                                      | Test case verify Tour Guide A không xem được manifest của Tour Guide B (403)                                   |

---

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi

Mô tả phạm vi TDD spec này: 3 domain entities, 3 service layers, 3 controllers.

```
Tour Management bao gồm các layer:
├── Domain (pure logic - validation, price calculation)
│   ├── Tour (entity + validation rules)
│   ├── TourSchedule (entity + capacity logic)
│   └── TourBooking (entity + status transitions)
├── Services (business logic - mock Repository inline)
│   ├── TourCatalogService (CRUD tours)
│   ├── TourScheduleService (CRUD schedules, manifest)
│   └── TourBookingService (booking, cancellation, refund)
├── Controllers (HTTP layer - mock Services)
│   ├── TourController (tours CRUD API)
│   ├── TourBookingController (booking API)
│   └── TourGuideController (manifest API)
└── Integration (SQL Server + full stack)
    └── Full flow: Book → ChargeToRoom → Checkout billing
```

### TDS-02 — Test Basis / Cơ sở Kiểm thử

| Source                                   | Items Derived                                                     |
| ---------------------------------------- | ----------------------------------------------------------------- |
| `Manager_Tour_SRS.md` UC-TM-01~08      | Hành vi Guest duyệt/đặt tour, Admin CRUD, Tour Guide manifest |
| `SWP391_NHOM1.md` UC11, UC12, UC26     | Guest view tours, view tour details, book tour                    |
| `SWP391-HOS-02.md` UC15~UC19, BR01~03 | Tour browsing, booking, guide manifest, review, cancellation      |
| `ManageTour_EDS.md` ADR-002            | Pessimistic Locking cho concurrency control                       |
| `ManageTour_EDS.md` ADR-003            | Refund policy 48h                                                 |
| BR-TM-01                                 | Không overbooking (capacity check)                               |
| BR-TM-02                                 | Không checkout nếu còn unpaid tour charges                     |
| BR-TM-03                                 | Refund 100% nếu > 48h; 0% nếu ≤ 48h                            |
| Luật Du lịch VN 2017                   | An toàn, bảo hiểm, hoàn tiền                                 |

### TDS-03 — Test Conditions and Coverage Items

| Condition ID | Test Condition                                    | Coverage Item                                          | Test Cases          |
| ------------ | ------------------------------------------------- | ------------------------------------------------------ | ------------------- |
| TC-COND-001  | Guest duyệt danh sách tour active               | `TourCatalogService.GetActiveTours()`                | `TOUR-TC-001`     |
| TC-COND-002  | Guest xem chi tiết tour (bao gồm weather)       | `TourCatalogService.GetTourDetail()`                 | `TOUR-TC-002`     |
| TC-COND-003  | Customer đặt tour thành công (Online payment) | `TourBookingService.CreateBooking()`                 | `TOUR-TC-003`     |
| TC-COND-004  | Customer đặt tour thành công (ChargeToRoom)   | `TourBookingService.CreateBooking()`                 | `TOUR-TC-004`     |
| TC-COND-005  | Overbooking bị chặn (BR-TM-01)                  | `TourBookingService.validateCapacity()`              | `TOUR-TC-005`     |
| TC-COND-006  | ChargeToRoom thiếu room booking → lỗi          | `TourBookingService.CreateBooking()`                 | `TOUR-TC-006`     |
| TC-COND-007  | Hủy tour > 48h → refund 100% (BR-TM-03)         | `TourCancellationService.processRefund()`            | `TOUR-TC-007`     |
| TC-COND-008  | Hủy tour ≤ 48h → không refund (BR-TM-03)      | `TourCancellationService.processRefund()`            | `TOUR-TC-008`     |
| TC-COND-009  | Admin CRUD tour catalog                           | `TourCatalogService.CreateTour()`                    | `TOUR-TC-009`     |
| TC-COND-010  | Admin tạo lịch trình + phân công guide       | `TourScheduleService.CreateSchedule()`               | `TOUR-TC-010`     |
| TC-COND-011  | Tour Guide xem manifest trong ngày               | `TourScheduleService.GetDailyManifest()`             | `TOUR-TC-011`     |
| TC-COND-012  | Tour Guide không xem manifest guide khác        | `TourGuideController` auth guard                     | `TOUR-TC-012`     |
| TC-COND-013  | Unpaid tour charges xuất hiện khi checkout      | `TourBookingService.GetUnpaidChargeToRoomBookings()` | `TOUR-TC-013`     |
| TC-COND-014  | TotalPrice = BasePrice × Participants            | `TourBookingService` domain logic                    | `TOUR-TC-014`     |
| TC-COND-015  | AvailableSlots invariant sau booking/cancel       | `TourScheduleRepository`                             | `TOUR-TC-015`     |
| TC-COND-016  | Unauthorized role bị chặn (403)                 | Controllers auth middleware                            | `TOUR-TC-SEC-001` |
| TC-COND-017  | SQL Injection bị chặn                           | Parameterized queries                                  | `TOUR-TC-SEC-002` |

### TDS-04 — Test Techniques / Kỹ thuật Kiểm thử

| Technique (ISO 29119-4)  | Applied To                                                         | Rationale                                         |
| ------------------------ | ------------------------------------------------------------------ | ------------------------------------------------- |
| Equivalence Partitioning | NumberOfParticipants: {0, 1, valid, > AvailableSlots}              | Phân vùng input hợp lệ / không hợp lệ      |
| Boundary Value Analysis  | AvailableSlots boundary: 0, 1, MaxCapacity-1, MaxCapacity          | Kiểm tra giới hạn capacity                     |
| State Transition Testing | BookingStatus: Confirmed → Cancelled / Completed                  | Verify FSM transitions hợp lệ và bất hợp lệ |
| State Transition Testing | PaymentStatus: Pending → Paid → Refunded                         | Verify payment flow                               |
| Decision Table Testing   | Refund logic: {> 48h, Paid} → Refund; {≤ 48h, Paid} → No Refund | Kết hợp điều kiện thời gian + trạng thái  |
| Error Guessing           | SQL Injection, XSS, Invalid JWT, Missing fields                    | Tấn công phổ biến OWASP Top 10                |

### TDS-05 — Test Data Requirements

| Fixture ID  | Type    | Value / Logic                                                                                                                                       | Mục đích                          |
| ----------- | ------- | --------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------ |
| FX-TOUR-001 | DB seed | `Tour { TourId=1, TourName="Ha Long Bay Tour", BasePrice=1500000, MaxCapacity=30, IsActive=true }`                                                | Tour catalog test                    |
| FX-TOUR-002 | DB seed | `Tour { TourId=2, TourName="Inactive Tour", IsActive=false }`                                                                                     | Inactive tour filter test            |
| FX-TOUR-003 | DB seed | `TourSchedule { ScheduleId=5, TourId=1, GuideId=8, DepartureDate="2026-07-01", AvailableSlots=15, Status="Scheduled" }`                           | Booking capacity test                |
| FX-TOUR-004 | DB seed | `TourSchedule { ScheduleId=6, TourId=1, DepartureDate="2026-06-19", AvailableSlots=0, Status="Scheduled" }`                                       | Full tour test                       |
| FX-TOUR-005 | DB seed | `TourBooking { TourBookingId=15, UserId=10, ScheduleId=5, Participants=2, PaymentMethod="ChargeToRoom", PaymentStatus="Pending", BookingId=102 }` | Unpaid charge test                   |
| FX-TOUR-006 | DB seed | `TourBooking { TourBookingId=16, UserId=10, ScheduleId=5, PaymentMethod="Online", PaymentStatus="Paid", BookingStatus="Confirmed" }`              | Cancellation refund test             |
| FX-TOUR-007 | DB seed | `Booking { BookingId=102, UserId=10, Status="CheckedIn" }`                                                                                        | Active room booking for ChargeToRoom |
| FX-TOUR-008 | JWT     | `{ sub: "user-10", role: "Customer" }`                                                                                                            | Customer auth context                |
| FX-TOUR-009 | JWT     | `{ sub: "admin-01", role: "Admin" }`                                                                                                              | Admin auth context                   |
| FX-TOUR-010 | JWT     | `{ sub: "guide-08", role: "TourGuide" }`                                                                                                          | Tour Guide auth context              |
| FX-TOUR-011 | JWT     | `{ sub: "recep-05", role: "Receptionist" }`                                                                                                       | Receptionist auth context            |

---

## 4. Test Case Specification

### TOUR-TC-001 — Guest duyệt danh sách tour active

**Severity**: MEDIUM
**Feature Under Test**: `TourCatalogService.GetActiveTours()`
**Test File**: `tests/TourCatalogService.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-001

**Preconditions**:

- FX-TOUR-001 (active tour) và FX-TOUR-002 (inactive tour) đã seed vào DB

**Test Steps**:

1. Arrange: Seed 2 tours — 1 active, 1 inactive
2. Act: Gọi `GetActiveTours(filter: null)`
3. Assert: Kết quả chỉ chứa tour active

**Expected Result (PASS)**:

- Trả về list chứa đúng 1 tour (TourId=1, IsActive=true)
- Tour inactive (TourId=2) KHÔNG xuất hiện
- Response chứa fields: TourId, TourName, BasePrice, ImageUrl

**Expected Result (FAIL)**:

- Trả về cả inactive tours
- Thiếu fields cần thiết

**Current Status**: 🟢 Passed
**Implementation Note**: Filter `WHERE IsActive = 1` trong repository query

---

### TOUR-TC-002 — Guest xem chi tiết tour (bao gồm weather forecast)

**Severity**: MEDIUM
**Feature Under Test**: `TourCatalogService.GetTourDetail(tourId)`
**Test File**: `tests/TourCatalogService.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-002

**Preconditions**:

- FX-TOUR-001 đã seed
- Weather API mock trả về dữ liệu dự báo

**Test Steps**:

1. Arrange: Seed tour, mock Weather API response
2. Act: Gọi `GetTourDetail(tourId=1)`
3. Assert: Response chứa đầy đủ thông tin tour + weather data

**Expected Result (PASS)**:

- Trả về TourDetailDto bao gồm: Description, Inclusions, Exclusions, Location
- Weather forecast data được attach (temperature, condition)
- Nếu Weather API fail → tour detail vẫn trả về, weather = null (graceful degradation)

**Expected Result (FAIL)**:

- Throw exception khi Weather API fail (thiếu graceful degradation)
- Thiếu fields chi tiết

**Current Status**: 🟢 Passed
**Implementation Note**: Dùng try-catch khi gọi Weather API; ADR-001: cache 30 phút

---

### TOUR-TC-003 — Customer đặt tour thành công (Online Payment)

**Severity**: CRITICAL
**Feature Under Test**: `TourBookingService.CreateBooking()`
**Test File**: `tests/TourBookingService.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-003

**Preconditions**:

- FX-TOUR-003: TourSchedule với AvailableSlots=15
- FX-TOUR-008: Customer JWT
- Payment Service mock trả về success

**Test Steps**:

1. Arrange: Seed TourSchedule, mock PaymentService.processOnlinePayment() → success
2. Act: Gọi `CreateBooking({ scheduleId: 5, participants: 2, paymentMethod: "Online" })`
3. Assert: TourBooking được tạo, AvailableSlots giảm, PaymentStatus = "Paid"

**Expected Result (PASS)**:

- TourBooking mới với: BookingStatus = "Confirmed", PaymentStatus = "Paid"
- TotalPrice = 1500000 × 2 = 3,000,000
- AvailableSlots giảm từ 15 → 13
- Event `TourBookingCreated` được publish
- BookingId = null (không charge to room)

**Expected Result (FAIL)**:

- TotalPrice tính sai
- AvailableSlots không giảm
- PaymentStatus không phải "Paid"

**Current Status**: 🟢 Passed
**Implementation Note**: Sử dụng transaction để đảm bảo atomicity giữa insert TourBooking và update AvailableSlots

---

### TOUR-TC-004 — Customer đặt tour với ChargeToRoom

**Severity**: CRITICAL
**Feature Under Test**: `TourBookingService.CreateBooking()`
**Test File**: `tests/TourBookingService.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-004

**Preconditions**:

- FX-TOUR-003: TourSchedule có chỗ
- FX-TOUR-007: Room Booking active (BookingId=102)
- FX-TOUR-008: Customer JWT

**Test Steps**:

1. Arrange: Seed TourSchedule + active room Booking
2. Act: Gọi `CreateBooking({ scheduleId: 5, participants: 3, paymentMethod: "ChargeToRoom", bookingId: 102 })`
3. Assert: TourBooking với PaymentStatus = "Pending", BookingId = 102

**Expected Result (PASS)**:

- TourBooking mới: PaymentMethod = "ChargeToRoom", PaymentStatus = "Pending"
- BookingId = 102 (linked to room booking)
- AvailableSlots giảm 3
- Payment Service KHÔNG được gọi (chưa thanh toán)

**Expected Result (FAIL)**:

- PaymentService bị gọi khi ChargeToRoom
- BookingId = null
- PaymentStatus = "Paid" (sai — phải là "Pending")

**Current Status**: 🟢 Passed
**Implementation Note**: Logic L1 fix — validate room booking active trước khi cho ChargeToRoom

---

### TOUR-TC-005 — Overbooking bị chặn (BR-TM-01)

**Severity**: CRITICAL
**CWE**: N/A (Business Logic)
**Legal**: BR-TM-01 (Capacity Limits)
**Feature Under Test**: `TourBookingService.validateCapacity()`
**Test File**: `tests/TourBookingService.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-005

**Preconditions**:

- FX-TOUR-004: TourSchedule với AvailableSlots=0

**Test Steps**:

1. Arrange: Seed TourSchedule full (AvailableSlots=0)
2. Act: Gọi `CreateBooking({ scheduleId: 6, participants: 1 })`
3. Assert: Exception với code "TOUR-002"

**Expected Result (PASS)**:

- Throw `TourFullException` với message chứa "fully booked"
- Error code = "TOUR-002"
- AvailableSlots vẫn = 0 (không thay đổi)
- KHÔNG có TourBooking mới trong DB

**Expected Result (FAIL)**:

- Booking được tạo thành công dù AvailableSlots = 0
- AvailableSlots trở thành số âm

**Current Status**: 🟢 Passed
**Implementation Note**: ADR-002: dùng SELECT ... FOR UPDATE trên TourSchedules trước khi check

---

### TOUR-TC-006 — ChargeToRoom thiếu room booking → lỗi

**Severity**: HIGH
**Feature Under Test**: `TourBookingService.CreateBooking()`
**Test File**: `tests/TourBookingService.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-006

**Preconditions**:

- FX-TOUR-003: TourSchedule có chỗ
- KHÔNG có room booking active cho user

**Test Steps**:

1. Arrange: Seed TourSchedule, KHÔNG seed room Booking
2. Act: Gọi `CreateBooking({ paymentMethod: "ChargeToRoom", bookingId: null })`
3. Assert: Exception với code "TOUR-006"

**Expected Result (PASS)**:

- Throw `NoActiveRoomBookingException`
- Error code = "TOUR-006"
- Không có booking mới, AvailableSlots không đổi

**Expected Result (FAIL)**:

- Booking được tạo với BookingId = null và PaymentMethod = "ChargeToRoom"

**Current Status**: 🟢 Passed
**Implementation Note**: Logic L1 — validate BookingId NOT NULL khi PaymentMethod = "ChargeToRoom"

---

### TOUR-TC-007 — Hủy tour > 48h → Refund 100% (BR-TM-03)

**Severity**: CRITICAL
**Legal**: BR-TM-03, Luật Du lịch VN 2017
**Feature Under Test**: `TourCancellationService.processRefund()`
**Test File**: `tests/TourCancellationService.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-007

**Preconditions**:

- FX-TOUR-006: TourBooking (Paid, Confirmed)
- DepartureDate là ngày hiện tại + 5 ngày (> 48h)

**Test Steps**:

1. Arrange: Seed TourBooking paid, DepartureDate = Now + 5 ngày
2. Act: Gọi `CancelBooking(tourBookingId=16, userId=10)`
3. Assert: BookingStatus → "Cancelled", Refund created 100%

**Expected Result (PASS)**:

- BookingStatus = "Cancelled"
- CancelledAt != null
- Refund record tạo với amount = TotalPrice (100%)
- AvailableSlots tăng lại (+2 nếu participants = 2)
- Event `TourBookingCancelled` published

**Expected Result (FAIL)**:

- Không tạo refund
- AvailableSlots không tăng
- BookingStatus vẫn "Confirmed"

**Current Status**: 🟢 Passed
**Implementation Note**: Logic L4 — so sánh DateTime.UtcNow với DepartureDate + DepartureTime

---

### TOUR-TC-008 — Hủy tour ≤ 48h → Không refund (BR-TM-03)

**Severity**: CRITICAL
**Legal**: BR-TM-03
**Feature Under Test**: `TourCancellationService.processRefund()`
**Test File**: `tests/TourCancellationService.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-008

**Preconditions**:

- TourBooking (Paid, Confirmed)
- DepartureDate là ngày hiện tại + 1 ngày (≤ 48h)

**Test Steps**:

1. Arrange: Seed TourBooking paid, DepartureDate = Now + 1 ngày
2. Act: Gọi `CancelBooking(tourBookingId, userId)`
3. Assert: BookingStatus → "Cancelled", KHÔNG tạo Refund

**Expected Result (PASS)**:

- BookingStatus = "Cancelled"
- CancelledAt != null
- KHÔNG có Refund record mới
- AvailableSlots tăng lại
- Response có warning message "No refund — cancellation within 48 hours"

**Expected Result (FAIL)**:

- Refund được tạo (sai policy)
- Hoặc exception khiến hủy không thực hiện được

**Current Status**: 🟢 Passed
**Implementation Note**: Boundary case: đúng 48h tính là "≤ 48h" → không refund

---

### TOUR-TC-009 — Admin tạo tour catalog (CRUD)

**Severity**: HIGH
**Feature Under Test**: `TourCatalogService.CreateTour()`
**Test File**: `tests/TourCatalogService.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-009

**Preconditions**:

- FX-TOUR-009: Admin JWT
- Database sạch (hoặc mock repo)

**Test Steps**:

1. Arrange: Chuẩn bị CreateTourInput đầy đủ
2. Act: Gọi `CreateTour(input)`
3. Assert: Tour mới được tạo với IsActive = true

**Expected Result (PASS)**:

- Tour record mới trong DB
- IsActive = true (default)
- TourName, BasePrice, MaxCapacity đúng giá trị input
- CreatedAt = thời điểm hiện tại

**Expected Result (FAIL)**:

- Thiếu validation: cho phép BasePrice = 0 hoặc âm
- Thiếu validation: cho phép MaxCapacity ≤ 0

**Current Status**: 🟢 Passed
**Implementation Note**: Validate: TourName not empty, BasePrice > 0, MaxCapacity > 0

---

### TOUR-TC-010 — Admin tạo lịch trình tour + phân công guide

**Severity**: HIGH
**Feature Under Test**: `TourScheduleService.CreateSchedule()`
**Test File**: `tests/TourScheduleService.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-010

**Preconditions**:

- FX-TOUR-001: Tour tồn tại
- User (GuideId=8) tồn tại với role TourGuide

**Test Steps**:

1. Arrange: Seed Tour + TourGuide user
2. Act: Gọi `CreateSchedule({ tourId: 1, guideId: 8, departureDate: "2026-07-01", departureTime: "07:00" })`
3. Assert: TourSchedule mới, AvailableSlots = Tour.MaxCapacity

**Expected Result (PASS)**:

- TourSchedule record mới
- AvailableSlots = MaxCapacity (30, từ Tour)
- Status = "Scheduled"
- GuideId = 8

**Expected Result (FAIL)**:

- AvailableSlots ≠ MaxCapacity
- GuideId không được gán
- DepartureDate trong quá khứ nhưng vẫn được tạo

**Current Status**: 🟢 Passed
**Implementation Note**: Validate DepartureDate > today; AvailableSlots khởi tạo = Tour.MaxCapacity

---

### TOUR-TC-011 — Tour Guide xem manifest trong ngày

**Severity**: HIGH
**Feature Under Test**: `TourScheduleService.GetDailyManifest()`
**Test File**: `tests/TourScheduleService.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-011

**Preconditions**:

- FX-TOUR-003: TourSchedule assigned to GuideId=8
- FX-TOUR-005, FX-TOUR-006: 2 TourBookings cho ScheduleId=5

**Test Steps**:

1. Arrange: Seed schedule + 2 bookings
2. Act: Gọi `GetDailyManifest(guideId=8, date="2026-07-01")`
3. Assert: Manifest chứa 2 attendees

**Expected Result (PASS)**:

- TourManifestDto chứa: guideName, date, list tours
- Mỗi tour chứa: tourName, departureTime, totalParticipants, attendees[]
- Attendees chứa: guestName, roomNumber (nullable), numberOfParticipants
- Chỉ hiện bookings có BookingStatus != "Cancelled"

**Expected Result (FAIL)**:

- Hiện cả cancelled bookings
- Thiếu roomNumber cho ChargeToRoom guests
- totalParticipants tính sai

**Current Status**: 🟢 Passed
**Implementation Note**: JOIN TourBookings → Users → Bookings → Rooms; filter BookingStatus != 'Cancelled'

---

### TOUR-TC-013 — Unpaid tour charges xuất hiện khi checkout

**Severity**: CRITICAL
**Legal**: BR-TM-02
**Feature Under Test**: `TourBookingService.GetUnpaidChargeToRoomBookings()`
**Test File**: `tests/TourBookingService.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-013

**Preconditions**:

- FX-TOUR-005: TourBooking ChargeToRoom, PaymentStatus="Pending", BookingId=102
- FX-TOUR-007: Room Booking active

**Test Steps**:

1. Arrange: Seed 2 unpaid tour bookings for BookingId=102
2. Act: Gọi `GetUnpaidChargeToRoomBookings(bookingId=102)`
3. Assert: Trả về 2 records, tổng amount đúng

**Expected Result (PASS)**:

- List chứa 2 TourBookingDto
- Tất cả có PaymentMethod = "ChargeToRoom" và PaymentStatus = "Pending"
- Tổng TotalPrice chính xác

**Expected Result (FAIL)**:

- Trả về bookings đã Paid (lặp charge)
- Trả về bookings Cancelled
- Thiếu booking

**Current Status**: 🟢 Passed
**Implementation Note**: WHERE BookingId = @id AND PaymentMethod = 'ChargeToRoom' AND PaymentStatus = 'Pending' AND BookingStatus != 'Cancelled'

---

### TOUR-TC-014 — TotalPrice tính đúng (BasePrice × Participants)

**Severity**: HIGH
**Feature Under Test**: `TourBookingService` domain logic
**Test File**: `tests/TourBookingService.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-014

**Preconditions**:

- FX-TOUR-001: Tour BasePrice = 1,500,000
- FX-TOUR-003: TourSchedule có chỗ

**Test Steps**:

1. Arrange: Tour BasePrice = 1,500,000
2. Act: CreateBooking với participants = 3
3. Assert: TotalPrice = 4,500,000

**Expected Result (PASS)**:

- TotalPrice = 1,500,000 × 3 = 4,500,000

**Expected Result (FAIL)**:

- TotalPrice != BasePrice × Participants

**Current Status**: 🟢 Passed
**Implementation Note**: Logic L2 — phép tính đơn giản nhưng cần test edge case: participants=1, participants rất lớn

---

### TOUR-TC-015 — AvailableSlots invariant sau booking và cancel

**Severity**: CRITICAL
**Feature Under Test**: `TourScheduleRepository` + `TourBookingService`
**Test File**: `tests/TourBookingService.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-015

**Preconditions**:

- FX-TOUR-001: Tour MaxCapacity = 30
- FX-TOUR-003: TourSchedule AvailableSlots = 15

**Test Steps**:

1. Arrange: Schedule có AvailableSlots=15 (đã có 15 booked)
2. Act: Book 3 participants → AvailableSlots = 12
3. Act: Cancel booking (3 participants) → AvailableSlots = 15
4. Assert: Invariant: TotalBooked + AvailableSlots = MaxCapacity luôn đúng

**Expected Result (PASS)**:

- Sau book: AvailableSlots = 12, TotalBooked = 18, Sum = 30 ✓
- Sau cancel: AvailableSlots = 15, TotalBooked = 15, Sum = 30 ✓

**Expected Result (FAIL)**:

- AvailableSlots không cập nhật sau cancel
- Invariant bị vi phạm (Sum ≠ MaxCapacity)

**Current Status**: 🟢 Passed
**Implementation Note**: Logic L3 — cần transaction cho cả booking và cancellation

---

### SECURITY TEST CASES

#### TOUR-TC-SEC-001 — Unauthorized Role bị chặn

**Severity**: CRITICAL
**OWASP**: A01:2021 — Broken Access Control
**CWE**: CWE-862 — Missing Authorization
**Feature Under Test**: `TourController`, `TourBookingController` auth middleware
**Test File**: `tests/TourController.Security.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement

**Preconditions**:

- FX-TOUR-008: Customer JWT (role = Customer)

**Test Steps (Attack Simulation)**:

1. Customer gọi POST `/api/v1/tours` (Admin-only endpoint)
2. Customer gọi GET `/api/v1/tour-guides/manifests` (TourGuide-only endpoint)
3. Kiểm tra response

**Expected Result (PASS = hệ thống an toàn)**:

- POST /api/v1/tours → 403 Forbidden, error code "TOUR-005"
- GET /api/v1/tour-guides/manifests → 403 Forbidden

**Expected Result (FAIL = lỗ hổng tồn tại)**:

- Trả về 200/201 — Customer có thể tạo tour hoặc xem manifest

**Current Status**: 🟢 Passed

---

#### TOUR-TC-SEC-002 — SQL Injection bị chặn

**Severity**: CRITICAL
**OWASP**: A03:2021 — Injection
**CWE**: CWE-89 — SQL Injection
**Feature Under Test**: `TourController.GET /api/v1/tours?search=`
**Test File**: `tests/TourController.Security.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement

**Preconditions**:

- FX-TOUR-001 đã seed

**Test Steps (Attack Simulation)**:

1. Gọi GET `/api/v1/tours?search='; DROP TABLE Tours; --`
2. Kiểm tra response và DB state

**Expected Result (PASS = hệ thống an toàn)**:

- Response 200 OK với empty results (hoặc không match)
- Bảng Tours vẫn tồn tại, data intact

**Expected Result (FAIL = lỗ hổng tồn tại)**:

- Bảng Tours bị drop hoặc data bị corrupt

**Current Status**: 🟢 Passed

---

#### TOUR-TC-SEC-003 — Tour Guide xem manifest guide khác bị chặn

**Severity**: HIGH
**OWASP**: A01:2021 — Broken Access Control
**CWE**: CWE-639 — Authorization Bypass Through User-Controlled Key
**Legal**: Logic L5
**Feature Under Test**: `TourGuideController` authorization
**Test File**: `tests/TourGuideController.Security.spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement

**Preconditions**:

- FX-TOUR-010: TourGuide JWT (GuideId=8)
- TourSchedule assigned to GuideId=9

**Test Steps (Attack Simulation)**:

1. TourGuide (GuideId=8) gọi GET `/api/v1/tour-guides/manifests?guideId=9`
2. Kiểm tra response

**Expected Result (PASS = hệ thống an toàn)**:

- 403 Forbidden hoặc chỉ trả về manifest của GuideId=8 (ignore guideId param)

**Expected Result (FAIL = lỗ hổng tồn tại)**:

- Trả về manifest của GuideId=9

**Current Status**: 🟢 Passed

---

### INTEGRATION TEST CASES

#### TOUR-TC-INT-001 — Full flow: Book Tour → ChargeToRoom → Checkout Billing

**Severity**: CRITICAL
**Feature Under Test**: Full flow: `TourBookingService → ConsolidatedBilling`
**Test File**: `tests/integration/TourBilling.integration-spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-004, TC-COND-013

**Preconditions**:

- SQL Server running
- Seed: Tour, TourSchedule, User (Customer), Room Booking (active)

**Test Steps**:

1. Seed Tour + TourSchedule + User + Room Booking
2. POST /api/v1/tour-bookings (ChargeToRoom) → 201
3. POST /api/v1/tour-bookings (ChargeToRoom, another tour) → 201
4. GET /api/v1/tour-bookings/unpaid?bookingId=102
5. Assert DB state: 2 unpaid bookings

**Expected Result (PASS)**:

- 2 TourBookings trong DB: PaymentStatus = "Pending", PaymentMethod = "ChargeToRoom"
- GET unpaid trả về 2 records
- Tổng TotalPrice = sum of 2 bookings
- AvailableSlots giảm đúng cho cả 2 schedules

**Expected Result (FAIL)**:

- Booking không link đúng BookingId
- Unpaid endpoint trả thiếu records

**DB Assertion**:

```csharp
var unpaidBookings = await _context.TourBookings
    .Where(tb => tb.BookingId == 102
        && tb.PaymentMethod == "ChargeToRoom"
        && tb.PaymentStatus == "Pending"
        && tb.BookingStatus != "Cancelled")
    .ToListAsync();
Assert.Equal(2, unpaidBookings.Count);
Assert.True(unpaidBookings.Sum(tb => tb.TotalPrice) > 0);
```

**Current Status**: 🟢 Passed

---

#### TOUR-TC-INT-002 — Concurrent Booking (Pessimistic Lock Test)

**Severity**: CRITICAL
**Feature Under Test**: ADR-002 Pessimistic Locking
**Test File**: `tests/integration/TourConcurrency.integration-spec.cs`
**TDD Phase**: 🟢 GREEN — Đã implement
**Condition Ref**: TC-COND-005

**Preconditions**:

- TourSchedule AvailableSlots = 1

**Test Steps**:

1. Seed TourSchedule với AvailableSlots = 1
2. Chạy 2 concurrent booking requests, mỗi request participants = 1
3. Assert: chỉ 1 thành công, 1 bị reject (TOUR-002)

**Expected Result (PASS)**:

- Đúng 1 TourBooking trong DB
- AvailableSlots = 0
- Request thứ 2 nhận 409 Conflict

**Expected Result (FAIL)**:

- 2 TourBookings trong DB (overbooking!)
- AvailableSlots = -1

**DB Assertion**:

```csharp
var bookings = await _context.TourBookings
    .Where(tb => tb.ScheduleId == scheduleId && tb.BookingStatus == "Confirmed")
    .ToListAsync();
Assert.Equal(1, bookings.Count); // Chỉ 1, không phải 2

var schedule = await _context.TourSchedules.FindAsync(scheduleId);
Assert.Equal(0, schedule.AvailableSlots); // Không âm
```

**Current Status**: 🟢 Passed

---

## 5. Red-Green-Refactor Tracker

| TC ID               | Test File                                                 | 🔴 RED confirmed | 🟢 GREEN (commit) | 🔵 REFACTOR note |
| ------------------- | --------------------------------------------------------- | ---------------- | ----------------- | ---------------- |
| `TOUR-TC-001`     | `tests/TourCatalogService.spec.cs`                      | `[x]`          | `[x]`             |                  |
| `TOUR-TC-002`     | `tests/TourCatalogService.spec.cs`                      | `[x]`          | `[x]`             |                  |
| `TOUR-TC-003`     | `tests/TourBookingService.spec.cs`                      | `[x]`          | `[x]`             |                  |
| `TOUR-TC-004`     | `tests/TourBookingService.spec.cs`                      | `[x]`          | `[x]`             |                  |
| `TOUR-TC-005`     | `tests/TourBookingService.spec.cs`                      | `[x]`          | `[x]`             |                  |
| `TOUR-TC-006`     | `tests/TourBookingService.spec.cs`                      | `[x]`          | `[x]`             |                  |
| `TOUR-TC-007`     | `tests/TourCancellationService.spec.cs`                 | `[x]`          | `[x]`             |                  |
| `TOUR-TC-008`     | `tests/TourCancellationService.spec.cs`                 | `[x]`          | `[x]`             |                  |
| `TOUR-TC-009`     | `tests/TourCatalogService.spec.cs`                      | `[x]`          | `[x]`             |                  |
| `TOUR-TC-010`     | `tests/TourScheduleService.spec.cs`                     | `[x]`          | `[x]`             |                  |
| `TOUR-TC-011`     | `tests/TourScheduleService.spec.cs`                     | `[x]`          | `[x]`             |                  |
| `TOUR-TC-013`     | `tests/TourBookingService.spec.cs`                      | `[x]`          | `[x]`             |                  |
| `TOUR-TC-014`     | `tests/TourBookingService.spec.cs`                      | `[x]`          | `[x]`             |                  |
| `TOUR-TC-015`     | `tests/TourBookingService.spec.cs`                      | `[x]`          | `[x]`             |                  |
| `TOUR-TC-SEC-001` | `tests/TourController.Security.spec.cs`                 | `[x]`          | `[x]`             |                  |
| `TOUR-TC-SEC-002` | `tests/TourController.Security.spec.cs`                 | `[x]`          | `[x]`             |                  |
| `TOUR-TC-SEC-003` | `tests/TourGuideController.Security.spec.cs`            | `[x]`          | `[x]`             |                  |
| `TOUR-TC-INT-001` | `tests/integration/TourBilling.integration-spec.cs`     | `[x]`          | `[x]`             |                  |
| `TOUR-TC-INT-002` | `tests/integration/TourConcurrency.integration-spec.cs` | `[x]`          | `[x]`             |                  |

---

## 6. Entry / Exit Criteria

**Entry Criteria (Điều kiện bắt đầu)**

- [ ] EDS spec `SWP391-TOUR-IMP-001` (ManageTour_EDS.md) đã được review và approve
- [ ] Logic Issues (Section 2) đã được confirm với Tech Lead
- [ ] SQL Server migration cho 3 bảng Tours, TourSchedules, TourBookings đã approved
- [ ] Test fixtures (Section 3 TDS-05) đã được chuẩn bị
- [ ] Upstream modules (Auth, Room Reservation) đã stable

**Exit Criteria (Điều kiện kết thúc — DoD)**

- [ ] `dotnet test` — tất cả unit tests pass (không có skip)
- [ ] Integration tests pass trên SQL Server test instance
- [ ] Test coverage ≥ 80% lines cho service layer mới tạo
- [ ] Không có SQL injection vulnerabilities (parameterized queries only)
- [ ] BR-TM-01 (no overbooking) đã được xác minh bằng concurrent test
- [ ] BR-TM-02 (unpaid charges) đã được xác minh bằng integration test
- [ ] BR-TM-03 (refund policy 48h) đã được xác minh với boundary cases
- [ ] Authorization Matrix đã test: Customer ≠ Admin ≠ TourGuide
- [ ] AvailableSlots invariant (TotalBooked + AvailableSlots = MaxCapacity) luôn đúng

**Suspension Criteria (Điều kiện tạm dừng)**

- Auth module chưa stable (JWT validation chưa hoạt động)
- Room Reservation module chưa deploy (không test được ChargeToRoom)
- DB migration bị conflict với module khác
- CI pipeline bị broken bởi thay đổi khác

---

## 7. Rollback Plan

```bash
# Revert migration (dev only)
dotnet ef database update [PreviousMigrationName]
dotnet ef migrations remove

# Revert implementation files
git checkout -- src/Modules/TourManagement/

# Revert test files
git checkout -- tests/TourManagement/

# Gap vẫn OPEN -> giữ nguyên entry trong task tracker
```
