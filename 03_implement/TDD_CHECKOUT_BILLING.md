# TEST-DRIVEN DEVELOPMENT SPECIFICATION

Đặc tả Kiểm thử Hướng Phát triển — Module Check-out & Consolidated Billing

**Document ID**: SWP391-BILLING-TDD-001
**Version**: 1.0
**Date**: 2026-06-25
**Status**: Approve
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: Cuong
**Reviewed by**: Khanh
**DPO Sign-off**:Khanh
**Approved by**: Cuong
**Classification**: Internal - Confidential

**References**:

- `01_SRS/SWP391_NHOM1_V2.md` — Functional requirements (UC53, UC54, UC55, UC56)
- `01_SRS/SWP391-HOS-02.md` — Project Specification (BR02, Module 5)
- `template/EDS_CHECKOUT_BILLING.md` — Engineering Design Specification
- `template/CheckOut_TotalBill_Plan.md` — Implementation Plan

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.
Thứ tự bắt buộc: viết test → chạy → xác nhận FAIL 🔴 → implement → PASS 🟢 → refactor 🔵.
Test data dùng dữ liệu SYNTHETIC. Không dùng PII thật.

## CHANGELOG

**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ.

| Ngày      | Người thực hiện | Nội dung thay đổi                                      |
| ---------- | ------------------- | --------------------------------------------------------- |
| 2026-06-25 | Cuong               | Khởi tạo tài liệu — TDD spec cho Check-out & Billing |

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

| Field                 | Value                                                                                              |
| --------------------- | -------------------------------------------------------------------------------------------------- |
| Feature / Gap ID      | UC53, UC54, UC55                                                                                   |
| Module                | `Check-out & Consolidated Billing - Front Desk / Billing`                                        |
| Spec gốc             | `SWP391-BILLING-IMP-001` (EDS_CHECKOUT_BILLING.md)                                               |
| Priority              | 🔴 P0                                                                                              |
| Sprint                | S3 (2026-06-25 -> 2026-07-09)                                                                      |
| Milestone             | M3 Alpha - 2026-07-11                                                                              |
| Data Classification   | PII (Tên khách, CCCD trên Invoice)                                                              |
| Compliance Scope      | NĐ 356/2025 (PDPD), Luật Cư trú 2020                                                           |
| Upstream Dependencies | `ReceptionService (Check-in), FoodOrderService (F&B charges), TourBookingService (Tour charges)` |
| Downstream Consumers  | `HousekeepingService (Room → NeedsCleaning), DashboardService (Revenue)`                        |

## 2. Logic Issues Resolved

| #  | Spec gốc (sai / thiếu)                                                                       | Thực tế (schema / policy)                                                                          | Fix áp dụng trong test                                                                            |
| -- | ---------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------- |
| L1 | UC53 không đề cập Late Check-out Surcharge                                                 | Resort nghiệp vụ chuẩn: tính phụ thu trả phòng muộn                                          | Test phải verify phụ thu 30%/50%/100% theo khung giờ                                             |
| L2 | UC54 chỉ nói "sums up Room Cost, Unpaid Food Orders, Unpaid Tour Bookings"                   | GuestFolio đã có cached totals (`TotalRoomCharges`, `TotalFoodCharges`, `TotalTourCharges`) | Test đọc trực tiếp từ GuestFolio thay vì SUM(FolioTransactions)                               |
| L3 | UC53 nói "Room status updated to Needs Cleaning"                                              | Enum`RoomStatus.NeedsCleaning` (giá trị = 3)                                                     | Test assert`Room.Status == RoomStatus.NeedsCleaning` cho **tất cả** phòng thuộc Booking |
| L4 | Spec thiếu mô tả khi DepositAmount > TotalAmount (khách đặt cọc nhiều hơn tổng bill) | BalanceDue có thể = 0 hoặc âm (cần refund)                                                      | Test BalanceDue = max(0, Total - Deposit), trường hợp Deposit > Total thì BalanceDue = 0        |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi

```
Check-out & Consolidated Billing bao gồm các layer:
├── Service Layer (ReceptionService - Business Logic)
│   ├── PrepareCheckOutViewModelAsync() — Gom bill, tính surcharge
│   └── ProcessCheckOutAsync() — Tạo Invoice, Payment, update status
├── Controller Layer (ReceptionController)
│   ├── GET CheckOut(int id) — Render trang bill
│   └── POST CheckOut(CheckOutViewModel) — Xử lý checkout
├── ViewModel (CheckOutViewModel)
│   └── Data binding giữa View và Service
└── View Layer (CheckOut.cshtml, PrintInvoice.cshtml)
    └── UI rendering — Manual test
```

### TDS-02 — Test Basis / Cơ sở Kiểm thử

| Source                              | Items Derived                                                                              |
| ----------------------------------- | ------------------------------------------------------------------------------------------ |
| `SWP391_NHOM1_V2.md` UC53         | Luồng checkout: verify booking info → review charges → process payment → update status |
| `SWP391_NHOM1_V2.md` UC54         | Generate Consolidated Invoice: gom Room + Food + Tour charges                              |
| `SWP391_NHOM1_V2.md` UC55         | Process Final Payment: tạo Payment record, mark Invoice as Paid                           |
| `SWP391-HOS-02.md` BR02           | Consolidated Billing Constraint: phải gom tất cả unpaid charges trước checkout        |
| `SWP391-HOS-02.md` BR02           | Khách không thể checkout nếu có unpaid items                                          |
| `SWP391-HOS-02.md` UC09           | Check-out includes updating Room status                                                    |
| `SWP391-HOS-02.md` UC10           | Room status transitions: Occupied → NeedsCleaning                                         |
| `EDS_CHECKOUT_BILLING.md` ADR-001 | Dùng cached totals trên GuestFolio                                                       |
| `CheckOut_TotalBill_Plan.md` §2  | Late Check-out Surcharge: 30% (12-15h), 50% (15-18h), 100% (>18h)                          |

### TDS-03 — Test Conditions and Coverage Items

| Condition ID | Test Condition                                                | Coverage Item                                          | Test Cases          |
| ------------ | ------------------------------------------------------------- | ------------------------------------------------------ | ------------------- |
| TC-COND-001  | Booking phải ở trạng thái CheckedIn mới được checkout | `PrepareCheckOutViewModelAsync()`                    | `BILL-TC-001`     |
| TC-COND-002  | GuestFolio phải tồn tại cho Booking                        | `PrepareCheckOutViewModelAsync()`                    | `BILL-TC-002`     |
| TC-COND-003  | Late surcharge 30% khi checkout 12:00-15:00                   | `PrepareCheckOutViewModelAsync()` — surcharge logic | `BILL-TC-003`     |
| TC-COND-004  | Late surcharge 50% khi checkout 15:00-18:00                   | `PrepareCheckOutViewModelAsync()` — surcharge logic | `BILL-TC-004`     |
| TC-COND-005  | Late surcharge 100% khi checkout sau 18:00                    | `PrepareCheckOutViewModelAsync()` — surcharge logic | `BILL-TC-005`     |
| TC-COND-006  | Không tính surcharge khi checkout trước 12:00             | `PrepareCheckOutViewModelAsync()` — surcharge logic | `BILL-TC-006`     |
| TC-COND-007  | BalanceDue = TotalAmount - DepositAmount                      | `PrepareCheckOutViewModelAsync()`                    | `BILL-TC-007`     |
| TC-COND-008  | BalanceDue = 0 khi Deposit >= Total                           | `PrepareCheckOutViewModelAsync()`                    | `BILL-TC-008`     |
| TC-COND-009  | Invoice được tạo với InvoiceStatus = Paid                | `ProcessCheckOutAsync()`                             | `BILL-TC-009`     |
| TC-COND-010  | Payment record được tạo với PaymentStatus = Success      | `ProcessCheckOutAsync()`                             | `BILL-TC-010`     |
| TC-COND-011  | BookingStatus → CheckedOut sau checkout                      | `ProcessCheckOutAsync()`                             | `BILL-TC-011`     |
| TC-COND-012  | RoomStatus → NeedsCleaning cho tất cả phòng               | `ProcessCheckOutAsync()`                             | `BILL-TC-012`     |
| TC-COND-013  | FolioStatus → Paid sau checkout                              | `ProcessCheckOutAsync()`                             | `BILL-TC-013`     |
| TC-COND-014  | Phải chọn PaymentMethod trước khi checkout                | `ProcessCheckOutAsync()` — validation               | `BILL-TC-014`     |
| TC-COND-015  | Gom đủ FoodCharges vào bill                                | `PrepareCheckOutViewModelAsync()`                    | `BILL-TC-015`     |
| TC-COND-016  | Gom đủ TourCharges vào bill                                | `PrepareCheckOutViewModelAsync()`                    | `BILL-TC-016`     |
| TC-COND-017  | Chỉ Receptionist/Admin mới truy cập được                | Controller [Authorize]                                 | `BILL-TC-SEC-001` |

### TDS-04 — Test Techniques / Kỹ thuật Kiểm thử

| Technique (ISO 29119-4)  | Applied To                                                    | Rationale                                            |
| ------------------------ | ------------------------------------------------------------- | ---------------------------------------------------- |
| Equivalence Partitioning | Late surcharge time ranges                                    | 4 khung giờ: <12h, 12-15h, 15-18h, >18h             |
| Boundary Value Analysis  | Checkout time boundaries                                      | Ranh giới: 11:59, 12:00, 14:59, 15:00, 17:59, 18:00 |
| State Transition Testing | BookingStatus, FolioStatus, InvoiceStatus, RoomStatus         | Verify FSM transitions đúng                        |
| Error Guessing           | Missing PaymentMethod, null GuestFolio, non-CheckedIn booking | Edge cases dễ gặp                                  |

### TDS-05 — Test Data Requirements

| Fixture ID | Type      | Value / Logic                                                                                                                                             | Mục đích                           |
| ---------- | --------- | --------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------- |
| FX-001     | DB seed   | `Booking { Id=1, Status=CheckedIn, CheckOutDate=2026-07-01, DepositAmount=500000 }`                                                                     | Happy path checkout                   |
| FX-002     | DB seed   | `GuestFolio { FolioId=1, BookingId=1, TotalRoomCharges=2000000, TotalFoodCharges=500000, TotalTourCharges=300000, TotalAmount=2800000, Status=Unpaid }` | Folio với đầy đủ charges         |
| FX-003     | DB seed   | `BookingDetail { BookingDetailId=1, BookingId=1, RoomId=101 }`                                                                                          | Room assignment                       |
| FX-004     | DB seed   | `Room { RoomId=101, RoomNumber="P101", Status=Occupied }`                                                                                               | Room đang occupied                   |
| FX-005     | DB seed   | `Booking { Id=2, Status=Reserved, CheckOutDate=2026-07-02 }`                                                                                            | Booking chưa CheckedIn — error case |
| FX-006     | DB seed   | `Booking { Id=3, Status=CheckedIn, DepositAmount=5000000, Folio.TotalAmount=2000000 }`                                                                  | Deposit > Total — edge case          |
| FX-007     | Time mock | `DateTime.Now = 2026-07-01 14:00`                                                                                                                       | Late surcharge 30%                    |
| FX-008     | Time mock | `DateTime.Now = 2026-07-01 16:30`                                                                                                                       | Late surcharge 50%                    |
| FX-009     | Time mock | `DateTime.Now = 2026-07-01 19:00`                                                                                                                       | Late surcharge 100%                   |
| FX-010     | Time mock | `DateTime.Now = 2026-07-01 11:30`                                                                                                                       | Không surcharge                      |

## 4. Test Case Specification

---

### BILL-TC-001 — Chỉ Booking ở trạng thái CheckedIn mới được checkout

**Severity**: CRITICAL
**Legal**: `BR-UC53-01`
**Feature Under Test**: `ReceptionService.PrepareCheckOutViewModelAsync()`
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED — chưa implement
**Condition Ref**: TC-COND-001

**Preconditions**:

- Database có Booking #2 với BookingStatus = Reserved (FX-005)

**Test Steps**:

1. Arrange: Seed Booking #2 (Status = Reserved)
2. Act: Gọi `PrepareCheckOutViewModelAsync(2)`
3. Assert: Kết quả trả về = `null`

**Expected Result (PASS — hành vi đúng)**:

- Method trả về `null` vì Booking không ở trạng thái CheckedIn

**Expected Result (FAIL — dấu hiệu lỗi)**:

- Method trả về ViewModel dù Booking chưa CheckedIn → lỗ hổng nghiệp vụ

**Current Status**: 🔴 Not written
**Implementation Note**: Kiểm tra `booking.BookingStatus != BookingStatus.CheckedIn` → return null

---

### BILL-TC-002 — Checkout thất bại khi không có GuestFolio

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.PrepareCheckOutViewModelAsync()`
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-002

**Preconditions**:

- Booking #1 ở trạng thái CheckedIn nhưng KHÔNG có GuestFolio

**Test Steps**:

1. Arrange: Seed Booking #1 (CheckedIn) mà không có GuestFolio
2. Act: Gọi `PrepareCheckOutViewModelAsync(1)`
3. Assert: Kết quả trả về = `null`

**Expected Result (PASS)**:

- Method trả về `null`

**Expected Result (FAIL)**:

- NullReferenceException khi truy cập GuestFolio properties

**Current Status**: 🔴 Not written

---

### BILL-TC-003 — Late surcharge 30% khi checkout 12:00 - 15:00

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.PrepareCheckOutViewModelAsync()` — surcharge logic
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-003

**Preconditions**:

- Booking #1 (FX-001): CheckedIn, CheckOutDate = 2026-07-01
- GuestFolio (FX-002): TotalRoomCharges = 2,000,000 VND
- RoomPrice per night = 1,000,000 VND
- Thời gian hiện tại (FX-007): 2026-07-01 14:00

**Test Steps**:

1. Arrange: Seed data, mock DateTime.Now = 14:00
2. Act: Gọi `PrepareCheckOutViewModelAsync(1)`
3. Assert: `model.LateCheckOutSurcharge == 300000` (30% x 1,000,000)

**Expected Result (PASS)**:

- LateCheckOutSurcharge = 300,000 VND
- FolioTransaction mới được tạo với TransactionType = RoomCharge

**Expected Result (FAIL)**:

- Surcharge = 0 hoặc sai tỷ lệ phần trăm

**Current Status**: 🔴 Not written

---

### BILL-TC-004 — Late surcharge 50% khi checkout 15:00 - 18:00

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.PrepareCheckOutViewModelAsync()` — surcharge logic
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-004

**Preconditions**:

- Booking #1 (FX-001), RoomPrice = 1,000,000 VND
- Thời gian (FX-008): 2026-07-01 16:30

**Test Steps**:

1. Arrange: Seed data, mock DateTime.Now = 16:30
2. Act: Gọi `PrepareCheckOutViewModelAsync(1)`
3. Assert: `model.LateCheckOutSurcharge == 500000` (50% x 1,000,000)

**Expected Result (PASS)**: LateCheckOutSurcharge = 500,000 VND
**Expected Result (FAIL)**: Sai tỷ lệ hoặc rơi vào khung giờ khác

**Current Status**: 🔴 Not written

---

### BILL-TC-005 — Late surcharge 100% khi checkout sau 18:00

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.PrepareCheckOutViewModelAsync()` — surcharge logic
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-005

**Preconditions**:

- Booking #1 (FX-001), RoomPrice = 1,000,000 VND
- Thời gian (FX-009): 2026-07-01 19:00

**Test Steps**:

1. Arrange: Seed data, mock DateTime.Now = 19:00
2. Act: Gọi `PrepareCheckOutViewModelAsync(1)`
3. Assert: `model.LateCheckOutSurcharge == 1000000` (100% x 1,000,000)

**Expected Result (PASS)**: LateCheckOutSurcharge = 1,000,000 VND
**Expected Result (FAIL)**: Sai tỷ lệ

**Current Status**: 🔴 Not written

---

### BILL-TC-006 — Không tính surcharge khi checkout trước 12:00

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.PrepareCheckOutViewModelAsync()` — surcharge logic
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-006

**Preconditions**:

- Booking #1 (FX-001), RoomPrice = 1,000,000 VND
- Thời gian (FX-010): 2026-07-01 11:30

**Test Steps**:

1. Arrange: Seed data, mock DateTime.Now = 11:30
2. Act: Gọi `PrepareCheckOutViewModelAsync(1)`
3. Assert: `model.LateCheckOutSurcharge == 0`

**Expected Result (PASS)**: LateCheckOutSurcharge = 0
**Expected Result (FAIL)**: Surcharge > 0 khi checkout đúng giờ

**Current Status**: 🔴 Not written

---

### BILL-TC-007 — BalanceDue tính đúng (Total > Deposit)

**Severity**: CRITICAL
**Legal**: `BR-UC53-02`
**Feature Under Test**: `ReceptionService.PrepareCheckOutViewModelAsync()`
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-007

**Preconditions**:

- Booking #1: DepositAmount = 500,000 VND (FX-001)
- GuestFolio: TotalAmount = 2,800,000 VND (FX-002)

**Test Steps**:

1. Arrange: Seed data
2. Act: Gọi `PrepareCheckOutViewModelAsync(1)`
3. Assert: `model.BalanceDue == 2300000` (2,800,000 - 500,000)

**Expected Result (PASS)**: BalanceDue = 2,300,000 VND
**Expected Result (FAIL)**: Sai phép tính hoặc không trừ tiền cọc

**Current Status**: 🔴 Not written

---

### BILL-TC-008 — BalanceDue = 0 khi Deposit >= Total

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.PrepareCheckOutViewModelAsync()`
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-008

**Preconditions**:

- Booking #3 (FX-006): DepositAmount = 5,000,000 VND, Folio.TotalAmount = 2,000,000 VND

**Test Steps**:

1. Arrange: Seed Booking #3
2. Act: Gọi `PrepareCheckOutViewModelAsync(3)`
3. Assert: `model.BalanceDue == 0` (không bao giờ âm)

**Expected Result (PASS)**: BalanceDue = 0
**Expected Result (FAIL)**: BalanceDue < 0 (số âm) gây nhầm lẫn cho Lễ tân

**Current Status**: 🔴 Not written
**Implementation Note**: Dùng `Math.Max(0, totalAmount - depositAmount)`

---

### BILL-TC-009 — Invoice được tạo với status Paid

**Severity**: CRITICAL
**Legal**: `UC54, UC55`
**Feature Under Test**: `ReceptionService.ProcessCheckOutAsync()`
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-009

**Preconditions**:

- Booking #1 ở trạng thái CheckedIn (FX-001)
- GuestFolio tồn tại (FX-002)

**Test Steps**:

1. Arrange: Seed full data, tạo CheckOutViewModel với PaymentMethod = Cash
2. Act: Gọi `ProcessCheckOutAsync(model)`
3. Assert:
   - Result.Success == true
   - DB: Invoice record tồn tại cho FolioId=1
   - Invoice.InvoiceStatus == InvoiceStatus.Paid
   - Invoice.FinalAmount == GuestFolio.TotalAmount

**Expected Result (PASS)**: Invoice record được tạo với status Paid
**Expected Result (FAIL)**: Invoice không được tạo hoặc status sai

**Current Status**: 🔴 Not written

---

### BILL-TC-010 — Payment record được tạo đúng

**Severity**: CRITICAL
**Legal**: `UC55`
**Feature Under Test**: `ReceptionService.ProcessCheckOutAsync()`
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-010

**Preconditions**:

- Same as BILL-TC-009

**Test Steps**:

1. Arrange: CheckOutViewModel với PaymentMethod = Cash, BalanceDue = 2,300,000
2. Act: Gọi `ProcessCheckOutAsync(model)`
3. Assert:
   - Payment record tồn tại cho InvoiceId mới tạo
   - Payment.PaymentMethod == PaymentMethod.Cash
   - Payment.Amount == BalanceDue (2,300,000)
   - Payment.PaymentStatus == PaymentStatus.Success
   - Payment.PaidAt IS NOT NULL

**Expected Result (PASS)**: Payment record chính xác
**Expected Result (FAIL)**: Payment không tạo hoặc Amount sai

**Current Status**: 🔴 Not written

---

### BILL-TC-011 — BookingStatus chuyển CheckedOut

**Severity**: CRITICAL
**Legal**: `UC53 Postcondition 3`
**Feature Under Test**: `ReceptionService.ProcessCheckOutAsync()`
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-011

**Preconditions**:

- Booking #1 ở trạng thái CheckedIn (FX-001)

**Test Steps**:

1. Arrange: Seed data
2. Act: Gọi `ProcessCheckOutAsync(model)`
3. Assert:
   - Booking.BookingStatus == BookingStatus.CheckedOut
   - Booking.ActualCheckOutTime IS NOT NULL
   - Booking.ActualCheckOutTime ≈ DateTime.Now (± 5 giây)

**Expected Result (PASS)**: Status = CheckedOut, ActualCheckOutTime ghi nhận
**Expected Result (FAIL)**: Status vẫn CheckedIn hoặc ActualCheckOutTime = null

**Current Status**: 🔴 Not written

---

### BILL-TC-012 — RoomStatus chuyển NeedsCleaning cho TẤT CẢ phòng

**Severity**: CRITICAL
**Legal**: `UC53 Postcondition 4, BR-UC53-04, UC56`
**Feature Under Test**: `ReceptionService.ProcessCheckOutAsync()`
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-012

**Preconditions**:

- Booking #1 có 2 BookingDetails → Room P101 (Occupied), Room P102 (Occupied)

**Test Steps**:

1. Arrange: Seed Booking với 2 phòng, cả 2 đều Occupied
2. Act: Gọi `ProcessCheckOutAsync(model)`
3. Assert:
   - Room P101.Status == RoomStatus.NeedsCleaning
   - Room P102.Status == RoomStatus.NeedsCleaning

**Expected Result (PASS)**: Tất cả phòng chuyển NeedsCleaning
**Expected Result (FAIL)**: Chỉ 1 phòng đổi, hoặc không phòng nào đổi

**Current Status**: 🔴 Not written

---

### BILL-TC-013 — FolioStatus chuyển Paid

**Severity**: CRITICAL
**Legal**: `BR-UC53-03`
**Feature Under Test**: `ReceptionService.ProcessCheckOutAsync()`
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-013

**Preconditions**:

- GuestFolio (FX-002): Status = Unpaid

**Test Steps**:

1. Arrange: Seed data
2. Act: Gọi `ProcessCheckOutAsync(model)`
3. Assert: GuestFolio.Status == FolioStatus.Paid

**Expected Result (PASS)**: Folio status = Paid
**Expected Result (FAIL)**: Folio vẫn Unpaid sau checkout

**Current Status**: 🔴 Not written

---

### BILL-TC-014 — Checkout thất bại khi thiếu PaymentMethod

**Severity**: HIGH
**Feature Under Test**: `ReceptionService.ProcessCheckOutAsync()` — validation
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-014

**Preconditions**:

- Booking #1 ở trạng thái CheckedIn

**Test Steps**:

1. Arrange: CheckOutViewModel với SelectedPaymentMethod = null
2. Act: Gọi `ProcessCheckOutAsync(model)`
3. Assert:
   - Result.Success == false
   - Result.ErrorMessage chứa "phương thức thanh toán"

**Expected Result (PASS)**: Checkout bị reject, booking vẫn CheckedIn
**Expected Result (FAIL)**: Checkout thành công mà không có PaymentMethod → lỗi dữ liệu

**Current Status**: 🔴 Not written

---

### BILL-TC-015 — Gom đủ FoodCharges vào bill

**Severity**: HIGH
**Legal**: `BR02 (SWP391-HOS-02), UC54`
**Feature Under Test**: `ReceptionService.PrepareCheckOutViewModelAsync()`
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-015

**Preconditions**:

- GuestFolio (FX-002): TotalFoodCharges = 500,000 VND
- 2 FolioTransactions loại FoodCharge: 300,000 + 200,000

**Test Steps**:

1. Arrange: Seed data
2. Act: Gọi `PrepareCheckOutViewModelAsync(1)`
3. Assert: `model.TotalFoodCharges == 500000`

**Expected Result (PASS)**: Food charges hiển thị đúng
**Expected Result (FAIL)**: Food charges = 0 hoặc thiếu

**Current Status**: 🔴 Not written

---

### BILL-TC-016 — Gom đủ TourCharges vào bill

**Severity**: HIGH
**Legal**: `BR02 (SWP391-HOS-02), UC54`
**Feature Under Test**: `ReceptionService.PrepareCheckOutViewModelAsync()`
**Test File**: `Tests/Services/ReceptionServiceCheckOutTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-016

**Preconditions**:

- GuestFolio (FX-002): TotalTourCharges = 300,000 VND

**Test Steps**:

1. Arrange: Seed data
2. Act: Gọi `PrepareCheckOutViewModelAsync(1)`
3. Assert: `model.TotalTourCharges == 300000`

**Expected Result (PASS)**: Tour charges hiển thị đúng
**Expected Result (FAIL)**: Tour charges = 0 hoặc thiếu

**Current Status**: 🔴 Not written

---

### SECURITY TEST CASES

#### BILL-TC-SEC-001 — Unauthorized access bị chặn

**Severity**: CRITICAL
**OWASP**: A01:2021 — Broken Access Control
**CWE**: CWE-862 — Missing Authorization
**Legal**: `NĐ 356/2025`
**Feature Under Test**: `ReceptionController` — [Authorize] attribute
**Test File**: `Tests/Controllers/ReceptionControllerCheckOutTests.cs`
**TDD Phase**: 🔴 RED

**Preconditions**:

- User KHÔNG có role Receptionist hoặc Admin (VD: Customer role)

**Test Steps (Attack Simulation)**:

1. Đăng nhập với tài khoản Customer
2. Truy cập GET `/Reception/CheckOut/1`
3. Kiểm tra response

**Expected Result (PASS = hệ thống an toàn)**:

- Response 403 Forbidden hoặc Redirect về Access Denied

**Expected Result (FAIL = lỗ hổng tồn tại)**:

- Customer truy cập được trang checkout → vi phạm least privilege

**Current Status**: 🔴 Not written

---

### INTEGRATION TEST CASES

#### BILL-TC-INT-001 — Full Check-out flow end-to-end

**Severity**: CRITICAL
**Feature Under Test**: Full flow: `GET CheckOut → POST CheckOut → Verify DB state`
**Test File**: `Tests/Integration/CheckOutIntegrationTests.cs`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-009 → TC-COND-013

**Preconditions**:

- SQL Server database running
- Seed: Booking #1 (CheckedIn), GuestFolio, BookingDetails with Rooms (Occupied), FolioTransactions

**Test Steps**:

1. Seed minimal data (FX-001 through FX-004)
2. Call `PrepareCheckOutViewModelAsync(1)` → verify ViewModel fields
3. Set `SelectedPaymentMethod = PaymentMethod.Cash`
4. Call `ProcessCheckOutAsync(model)`
5. Assert DB state

**Expected Result (PASS)**:

- Booking.BookingStatus = CheckedOut
- Booking.ActualCheckOutTime ≠ null
- Room.Status = NeedsCleaning (all rooms)
- GuestFolio.Status = Paid
- Invoice exists, InvoiceStatus = Paid
- Payment exists, PaymentStatus = Success

**Expected Result (FAIL)**:

- Any DB field not updated correctly → integration bug

**DB Assertion**:

```csharp
var booking = await _context.Bookings.FindAsync(1);
Assert.Equal(BookingStatus.CheckedOut, booking.BookingStatus);
Assert.NotNull(booking.ActualCheckOutTime);

var folio = await _context.GuestFolios.FirstAsync(f => f.BookingId == 1);
Assert.Equal(FolioStatus.Paid, folio.Status);

var invoice = await _context.Invoices.FirstAsync(i => i.FolioId == folio.FolioId);
Assert.Equal(InvoiceStatus.Paid, invoice.InvoiceStatus);

var payment = await _context.Payments.FirstAsync(p => p.InvoiceId == invoice.InvoiceId);
Assert.Equal(PaymentStatus.Success, payment.PaymentStatus);
Assert.Equal(PaymentMethod.Cash, payment.PaymentMethod);

var rooms = await _context.BookingDetails
    .Where(bd => bd.BookingId == 1 && bd.RoomId != null)
    .Select(bd => bd.Room)
    .ToListAsync();
Assert.All(rooms, r => Assert.Equal(RoomStatus.NeedsCleaning, r.Status));
```

**Current Status**: 🔴 Not written

## 5. Red-Green-Refactor Tracker

| TC ID               | Test File                                                 | 🔴 RED confirmed | 🟢 GREEN (commit) | 🔵 REFACTOR note |
| ------------------- | --------------------------------------------------------- | ---------------- | ----------------- | ---------------- |
| `BILL-TC-001`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-002`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-003`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-004`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-005`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-006`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-007`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-008`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-009`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-010`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-011`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-012`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-013`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-014`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-015`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-016`     | `Tests/Services/ReceptionServiceCheckOutTests.cs`       | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-SEC-001` | `Tests/Controllers/ReceptionControllerCheckOutTests.cs` | `[ ]`          | `[pending]`     | —               |
| `BILL-TC-INT-001` | `Tests/Integration/CheckOutIntegrationTests.cs`         | `[ ]`          | `[pending]`     | —               |

## 6. Entry / Exit Criteria

**Entry Criteria (Điều kiện bắt đầu)**

- [X] SRS (UC53, UC54, UC55) đã được review
- [X] EDS `SWP391-BILLING-IMP-001` đã được tạo
- [X] Schema hiện tại đã có đủ Models: Booking, GuestFolio, FolioTransaction, Invoice, Payment
- [X] Enums đã khai báo: BookingStatus.CheckedOut, FolioStatus.Paid, InvoiceStatus.Paid, PaymentStatus.Success
- [X] Module Check-in đang hoạt động (upstream dependency)
- [ ] Test fixtures (Section 3 TDS-05) đã được chuẩn bị

**Exit Criteria (Điều kiện kết thúc — DoD)**

- [ ] Tất cả 18 test cases đều PASS 🟢
- [ ] Build thành công (dotnet build)
- [ ] Không có Booking bị "stuck" ở trạng thái sai sau checkout
- [ ] Room status luôn chuyển NeedsCleaning (cho tất cả phòng)
- [ ] Invoice + Payment records luôn được tạo sau checkout thành công
- [ ] GuestFolio status luôn chuyển Paid
- [ ] Late surcharge tính đúng theo 4 khung giờ
- [ ] Chỉ Receptionist/Admin mới truy cập được (Authorization verified)

**Suspension Criteria (Điều kiện tạm dừng)**

- Module Check-in bị regression (upstream dependency)
- Database migration lỗi
- F&B hoặc Tour module chưa tích hợp FolioTransaction (giảm coverage BILL-TC-015, BILL-TC-016)

## 7. Rollback Plan

```bash
# Revert implementation files
git checkout -- PhucNguyenResort/Services/Reception/ReceptionService.cs
git checkout -- PhucNguyenResort/Services/Reception/IReceptionService.cs
git checkout -- PhucNguyenResort/Controllers/ReceptionController.cs
git checkout -- PhucNguyenResort/Views/Reception/Index.cshtml

# Remove new files
del PhucNguyenResort/ViewModels/Bookings/CheckOutViewModel.cs
del PhucNguyenResort/Views/Reception/CheckOut.cshtml
del PhucNguyenResort/Views/Reception/PrintInvoice.cshtml

# Fix data nếu cần (SSMS):
# UPDATE Bookings SET BookingStatus = 2 WHERE BookingStatus = 3 AND ActualCheckOutTime > '2026-06-25';
# -- 2 = CheckedIn, 3 = CheckedOut
```
