# TEST-DRIVEN DEVELOPMENT SPECIFICATION — UC69 (Charge to Room)

Mẫu Đặc tả Kiểm thử Hướng Phát triển cho Use Case 69 (Ghi nợ chi phí ăn uống vào tiền phòng)

**Document ID**: PHUCNGUYEN-FOODORDER-TDD-003
**Version**: 1.1
**Date**: 2026-06-18
**Status**: Approved
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: `Ho Minh Khanh - Backend Developer`
**Reviewed by**: [x] `[Truong Tran Cuong] - Approved (2026-06-18)`
**DPO Sign-off**: [x] Approved (2026-06-18)
**Approved by**: [x] Approved (2026-06-18)
**Classification**: Internal - Confidential

**References**:

- `Tailieu/SWP391_NHOM1_VER2.pdf` — Functional requirements (UC69: Charge Food Bill To Room)
- `03_implement/FoodOrder_EDS.md` (v2.2) — Technical Specification cho Food Order
- `02_Design/ADR/ADR-003` — Charge to Room qua GuestFolio & FolioTransaction
- `02_Design/ADR/ADR-005` — Anti-Duplicate Charge (Chống charge trùng)

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.
Thứ tự bắt buộc: viết test (.cs) -> chạy -> xác nhận FAIL 🔴 -> implement -> PASS 🟢 -> refactor 🔵.

## CHANGELOG

| Ngày      | Người thực hiện | Nội dung thay đổi                                                                  |
| ---------- | ------------------- | ------------------------------------------------------------------------------------- |
| 2026-06-18 | `Ho Minh Khanh`   | Khởi tạo tài liệu — TDD spec cho UC69 (Charge to Room). Đã align 1:1 với SRS. |

---

## MỤC LỤC

1. Thông tin Module
2. Logic Issues Resolved
3. Test Design Specification (TDS)
4. Test Case Specification
5. Red-Green-Refactor Tracker
6. Entry / Exit Criteria

---

## 1. Thông tin Module

| Field                 | Value                                                                                         |
| --------------------- | --------------------------------------------------------------------------------------------- |
| Feature / Gap ID      | UC69 (Charge to Room)                                                                         |
| Module                | `Food Order & Room Service`                                                                 |
| Spec gốc             | `PHUCNGUYEN-FOODORDER-IMP-001` (v2.2) & `SWP391_NHOM1_V2.pdf` (SRS cập nhật mới nhất) |
| Priority              | 🔴 P0 (Nghiệp vụ tài chính cốt lõi)                                                     |
| Upstream Dependencies | `UC66 (Update Status)` — Phải Delivered mới được Charge.                              |
| Downstream Consumers  | `UC57 (Consolidated Invoice)` — Checkout sẽ tính tổng tiền từ GuestFolio.             |

---

## 2. Logic Issues Resolved (Sự ăn khớp 1:1 SRS vs EDS)

Mọi lỗ hổng (gaps) giữa SRS gốc và mã nguồn thực tế đã được lấp đầy. UC69 hiện tại đồng nhất tuyệt đối về thiết kế nghiệp vụ:

- **BR-UC69-01**: Yêu cầu đơn hàng hoàn thành -> Map với `OrderStatus == Delivered`.
- **BR-UC69-04**: Yêu cầu chống Charge trùng -> Map với cờ bảo mật `IsChargedToRoom` và Check logic `FolioTransaction` (`ADR-005`).
- **Folio Missing Check**: Hệ thống cần có cơ chế bắt lỗi khi Booking chưa được tạo sổ GuestFolio (tránh lỗi ngầm NullReference).

---

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi

TDD spec này tập trung hoàn toàn vào luồng tài chính khi **F&B Staff ghi nợ vào phòng**:

- **Service Layer**:
  - `FoodOrderService.ChargeToRoomAsync(int foodOrderId)`

### TDS-02 — Test Conditions and Coverage Items

| Condition ID | Test Condition                                                               | Test Cases      |
| ------------ | ---------------------------------------------------------------------------- | --------------- |
| TC-COND-001  | Happy path: Charge đơn hàng hợp lệ (Delivered, chưa Charge, có Folio) | `UC69-TC-001` |
| TC-COND-002  | Invalid: Charge đơn hàng chưa giao (Pending/Cooking)                     | `UC69-TC-002` |
| TC-COND-003  | Invalid: Charge trùng lặp (Đơn hàng đã được charge trước đó)   | `UC69-TC-003` |
| TC-COND-004  | Invalid: Booking không có GuestFolio                                       | `UC69-TC-004` |
| TC-COND-005  | Invalid: OrderId không tồn tại                                            | `UC69-TC-005` |
| TC-COND-006  | Invalid: Charge đơn hàng bị hủy (Cancelled)                             | `UC69-TC-006` |

### TDS-03 — Test Data Requirements (Fixtures)

| Fixture ID  | Type             | Value / Logic                                                                                                         | Mục đích              |
| ----------- | ---------------- | --------------------------------------------------------------------------------------------------------------------- | ------------------------ |
| FX-FOLIO-01 | In-Memory Entity | `GuestFolio` (FolioId=1, BookingId=1, TotalFoodCharges=0)                                                           | Sổ nợ chuẩn           |
| FX-ORD-01   | In-Memory Entity | `FoodOrderId=1`, `BookingId=1`, `OrderStatus=Delivered`, `IsChargedToRoom=false`, `TotalAmount=150000`      | Order đủ chuẩn charge |
| FX-ORD-02   | In-Memory Entity | `FoodOrderId=2`, `BookingId=1`, `OrderStatus=Cooking`, `IsChargedToRoom=false`                                | Order CHƯA giao xong    |
| FX-ORD-03   | In-Memory Entity | `FoodOrderId=3`, `BookingId=1`, `OrderStatus=Delivered`, `IsChargedToRoom=true`                               | Order ĐÃ bị charge    |
| FX-ORD-04   | In-Memory Entity | `FoodOrderId=4`, `BookingId=99` (Booking ko có GuestFolio), `OrderStatus=Delivered`, `IsChargedToRoom=false` | Khách chưa có Folio   |

---

## 4. Test Case Specification

### UC69-TC-001 — Charge Happy Path

**Severity**: CRITICAL
**Feature Under Test**: `FoodOrderService.ChargeToRoomAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — passed
**Preconditions**: DB có `FX-FOLIO-01` và `FX-ORD-01`.
**Test Steps**: Gọi `ChargeToRoomAsync(1)`.
**Expected Result (PASS)**:

- Status trả về `FoodOrderActionStatus.Success`.
- FoodOrder `1` có `IsChargedToRoom = true` và `ChargedAt` có giá trị ngày hiện tại.
- GuestFolio `1` có `TotalFoodCharges` bằng đúng `150000`.
- Có 1 `FolioTransaction` mới tạo với `TransactionType = FoodCharge`, `ReferenceId = 1`, `Amount = 150000`.

### UC69-TC-002 — Invalid: Charge order not delivered

**Severity**: HIGH
**Feature Under Test**: `FoodOrderService.ChargeToRoomAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — passed
**Preconditions**: DB có `FX-FOLIO-01` và `FX-ORD-02` (Cooking).
**Test Steps**: Gọi `ChargeToRoomAsync(2)`.
**Expected Result (PASS)**:

- Status trả về `FoodOrderActionStatus.InvalidState`.
- GuestFolio `1` KHÔNG bị cộng tiền. Không sinh ra `FolioTransaction` nào.

### UC69-TC-003 — Invalid: Anti-Duplicate Charge (BR-UC69-04)

**Severity**: CRITICAL
**Feature Under Test**: `FoodOrderService.ChargeToRoomAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — passed
**Preconditions**: DB có `FX-FOLIO-01` và `FX-ORD-03` (Đã charge).
**Test Steps**: Gọi `ChargeToRoomAsync(3)`.
**Expected Result (PASS)**:

- Status trả về `FoodOrderActionStatus.AlreadyCharged`.
- Tổng tiền GuestFolio không bị cộng dồn lần 2. (An toàn tài chính).

### UC69-TC-004 — Invalid: No Guest Folio

**Severity**: HIGH
**Feature Under Test**: `FoodOrderService.ChargeToRoomAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — passed
**Preconditions**: DB có `FX-ORD-04` (Delivered nhưng ko có GuestFolio map với Booking).
**Test Steps**: Gọi `ChargeToRoomAsync(4)`.
**Expected Result (PASS)**:

- Status trả về `FoodOrderActionStatus.NoFolio`.
- FoodOrder `4` vẫn giữ nguyên `IsChargedToRoom = false`.

### UC69-TC-005 — Invalid: Order Not Found

**Severity**: LOW
**Feature Under Test**: `FoodOrderService.ChargeToRoomAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — passed
**Preconditions**: DB không có Order `999`.
**Test Steps**: Gọi `ChargeToRoomAsync(999)`.
**Expected Result (PASS)**:

- Status trả về `FoodOrderActionStatus.NotFound`.

### UC69-TC-006 — Invalid: Charge Cancelled Order

**Severity**: HIGH
**Feature Under Test**: `FoodOrderService.ChargeToRoomAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — passed
**Preconditions**: DB có Order trạng thái `Cancelled`.
**Test Steps**: Gọi Charge cho đơn Cancelled.
**Expected Result (PASS)**:

- Status trả về `FoodOrderActionStatus.InvalidState`.

---

## 5. Red-Green-Refactor Tracker

| TC ID           | Test File                    | 🔴 RED confirmed | 🟢 GREEN (commit) | 🔵 REFACTOR note |
| --------------- | ---------------------------- | ---------------- | ----------------- | ---------------- |
| `UC69-TC-001` | `FoodOrderServiceTests.cs` | [x]              | [x]               |                  |
| `UC69-TC-002` | `FoodOrderServiceTests.cs` | [x]              | [x]               |                  |
| `UC69-TC-003` | `FoodOrderServiceTests.cs` | [x]              | [x]               |                  |
| `UC69-TC-004` | `FoodOrderServiceTests.cs` | [x]              | [x]               |                  |
| `UC69-TC-005` | `FoodOrderServiceTests.cs` | [x]              | [x]               |                  |
| `UC69-TC-006` | `FoodOrderServiceTests.cs` | [x]              | [x]               |                  |

## 6. Entry / Exit Criteria

**Exit Criteria (DoD)**:

- [X] Tất cả 6 unit tests đều phải vượt qua trạng thái GREEN.
- [X] Giao dịch tài chính (FolioTransaction) được map chuẩn ID đơn hàng.
- [X] Luồng test độc lập, không bị rò rỉ dữ liệu qua các test case khác (DB In-Memory riêng biệt mỗi test).
