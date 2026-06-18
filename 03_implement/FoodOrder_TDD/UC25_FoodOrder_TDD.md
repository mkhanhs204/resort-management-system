# TEST-DRIVEN DEVELOPMENT SPECIFICATION — UC25 (Room Service)

Mẫu Đặc tả Kiểm thử Hướng Phát triển cho Use Case 25 (Đặt đồ ăn tại phòng)

**Document ID**: PHUCNGUYEN-FOODORDER-TDD-001
**Version**: 1.2
**Date**: 2026-06-18
**Status**: Approved
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: `Ho Minh Khanh  - Backend Developer`
**Reviewed by**: [X] `[Truong Tran Cuong] - Approved`
**DPO Sign-off**: [X] Approved
**Approved by**: [X] Approved
**Classification**: Internal - Confidential

**References**:
- `01_SRS/SWP391_NHOM1_ver2.pdf` — Functional requirements (UC25)
- `03_implement/FoodOrder_EDS.md` (v2.2) — Technical Specification cho Food Order
- `02_Design/ADR/ADR-001` — MVC SSR
- `02_Design/ADR/ADR-002` — Dual Status Model
- `02_Design/ADR/ADR-004` — Snapshot UnitPrice

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.
Thứ tự bắt buộc: viết test (.cs) -> chạy -> xác nhận FAIL 🔴 -> implement -> PASS 🟢 -> refactor 🔵.
Không mark test là ✅ nếu chạy unit test chưa xanh.
Test data sử dụng từ `SeedData.cs` (BookingId=2,7,12,17) cho integration, và data giả lập (mock/in-memory DB) cho unit tests.

## CHANGELOG
**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ.

| Ngày       | Người thực hiện | Nội dung thay đổi                                                                                                                                           |
| ------------| -----------------| -------------------------------------------------------------------------------------------------------------------------------------------------------------|
| 2026-06-13 | `Ho Minh Khanh` | Khởi tạo tài liệu — TDD spec cho UC11 (Food Order & Room Service)                                                                                           |
| 2026-06-13 | `Ho Minh Khanh` | v1.1: Chỉnh sửa Scope thành Service Only, cập nhật FoodOrderCreateStatus, thêm tests cho InvalidQuantity, BookingNotFound, và IsDeleted. Cập nhật fixtures. |
| 2026-06-18 | `Ho Minh Khanh` | v1.2: Cập nhật ID từ UC11 thành UC25 cho khớp với SRS ver2.                                                                                                 |

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
| Feature / Gap ID | UC25 (Room Service Order) |
| Module | `Food Order & Room Service` |
| Spec gốc | `PHUCNGUYEN-FOODORDER-IMP-001` (v2.2) |
| Priority | 🔴 P0 |
| Sprint | S[N] |
| Data Classification | Internal |
| Compliance Scope | Phân quyền truy cập theo role `Customer` và check ownership booking |
| Upstream Dependencies | `Module 1 (MenuItems)`, `Module 2 (Bookings)` |
| Downstream Consumers | `Module 5 (GuestFolio)` — sau khi order Delivered |

## 2. Logic Issues Resolved

| # | Spec gốc (sai / thiếu) | Thực tế (policy trong EDS v2.2) | Fix áp dụng trong test |
| --- | --- | --- | --- |
| L1 | Validation form menu cứng nhắc (người dùng phải xóa dòng). | Chấp nhận quantity từ form là 0. Backend tự động lọc bỏ các món có quantity = 0. | Test case phải truyền list mix quantity > 0 và = 0, đảm bảo món = 0 bị bỏ qua. |
| L2 | Check sở hữu (ownership) không rõ ràng ở service. | Thêm `currentUserId` vào signature của `CreateFoodOrderAsync(request, currentUserId)` để bắt buộc verify. | Truyền `currentUserId` khớp và không khớp để test trạng thái `Forbidden`. |
| L3 | Không định nghĩa enum cho kết quả CreateFoodOrderAsync | `CreateFoodOrderAsync` sẽ trả về object `FoodOrderCreateResult` với enum `FoodOrderCreateStatus` (Created, Forbidden, BookingNotCheckedIn, NoItems, InvalidQuantity, MenuItemUnavailable, BookingNotFound, MenuItemNotFound, DuplicateMenuItem). | Các test case sử dụng enum `FoodOrderCreateStatus` để assert. |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi
TDD spec này tập trung hoàn toàn vào luồng tạo FoodOrder từ phía Customer:
- **Service Layer only**:
  - `FoodOrderService.CreateFoodOrderAsync`
  - `FoodOrderService.BuildRoomServiceOrderAsync`

### TDS-02 — Test Basis / Cơ sở Kiểm thử
| Source | Items Derived |
| --- | --- |
| `FoodOrder_EDS.md` BR-FOOD-001 | Booking phải tồn tại và CheckedIn |
| `FoodOrder_EDS.md` BR-FOOD-002 | Menu item phải IsAvailable = true, IsDeleted = false |
| `FoodOrder_EDS.md` BR-FOOD-003 | Snapshot UnitPrice |
| `FoodOrder_EDS.md` BR-FOOD-004 | Quantity validation (lọc >= 0, ít nhất 1 món sau khi lọc) |
| `FoodOrder_EDS.md` BR-FOOD-009 | Data isolation (chỉ đặt món cho booking của chính user đó) |

### TDS-03 — Test Conditions and Coverage Items
| Condition ID | Test Condition                                   | Coverage Item                | Test Cases    |
| --------------| --------------------------------------------------| ------------------------------| ---------------|
| TC-COND-001  | Customer tạo order hợp lệ (mix quantity 0 và >0) | `CreateFoodOrderAsync`       | `UC25-TC-001` |
| TC-COND-002  | Customer tạo order cho booking của người khác    | `CreateFoodOrderAsync`       | `UC25-TC-002` |
| TC-COND-003  | Booking tồn tại nhưng chưa CheckedIn             | `CreateFoodOrderAsync`       | `UC25-TC-003` |
| TC-COND-004  | Submit toàn bộ danh sách món quantity = 0        | `CreateFoodOrderAsync`       | `UC25-TC-004` |
| TC-COND-005  | Đặt món bị ngưng kinh doanh (IsAvailable=false)  | `CreateFoodOrderAsync`       | `UC25-TC-005` |
| TC-COND-006  | Submit quantity âm (<0)                          | `CreateFoodOrderAsync`       | `UC25-TC-006` |
| TC-COND-007  | Submit quantity vượt giới hạn (>1000)            | `CreateFoodOrderAsync`       | `UC25-TC-007` |
| TC-COND-008  | Đặt cho BookingId không tồn tại                  | `CreateFoodOrderAsync`       | `UC25-TC-008` |
| TC-COND-009  | Đặt món đã bị xóa (IsDeleted=true)               | `CreateFoodOrderAsync`       | `UC25-TC-009` |
| TC-COND-010  | Đặt món với MenuItemId không tồn tại             | `CreateFoodOrderAsync`       | `UC25-TC-010` |
| TC-COND-011  | Gửi nhiều items có cùng MenuItemId               | `CreateFoodOrderAsync`       | `UC25-TC-011` |
| TC-COND-012  | Load menu hợp lệ cho Room Service                | `BuildRoomServiceOrderAsync` | `UC25-TC-012` |
| TC-COND-013  | Load menu cho booking không hợp lệ               | `BuildRoomServiceOrderAsync` | `UC25-TC-013` |

### TDS-04 — Test Techniques / Kỹ thuật Kiểm thử
| Technique (ISO 29119-4) | Applied To | Rationale |
| --- | --- | --- |
| Equivalence Partitioning | Quantity input | Chia class: =0 (bỏ qua), 1..1000 (hợp lệ), <0 (lỗi), >1000 (lỗi) |
| Boundary Value Analysis | Data Isolation | Kiểm tra UserId = Booking.UserId vs UserId != Booking.UserId |

### TDS-05 — Test Data Requirements
| Fixture ID | Type | Value / Logic | Mục đích |
| --- | --- | --- | --- |
| FX-BK-001 | In-Memory Entity | `BookingId=2`, `UserId=2`, `Status=CheckedIn` | Booking chuẩn (Happy path) |
| FX-BK-002 | In-Memory Entity | `BookingId=7`, `UserId=7`, `Status=CheckedIn` | Booking khác người dùng |
| FX-BK-003 | In-Memory Entity | `BookingId=1`, `UserId=2`, `Status=Reserved` | Booking chưa check-in |
| FX-MN-001 | In-Memory Entity | `MenuItemId=1`, `Price=85000`, `IsAvailable=true`, `IsDeleted=false` | Món ăn hợp lệ 1 |
| FX-MN-002 | In-Memory Entity | `MenuItemId=8`, `Price=260000`, `IsAvailable=false`, `IsDeleted=false` | Món ăn tạm hết (Unavailable) |
| FX-MN-003 | In-Memory Entity | `MenuItemId=7`, `Price=235000`, `IsAvailable=true`, `IsDeleted=false` | Món ăn hợp lệ 2 |
| FX-MN-004 | In-Memory Entity | `MenuItemId=10`, `Price=310000`, `IsAvailable=true`, `IsDeleted=false` | Món ăn hợp lệ 3 |
| FX-MN-005 | In-Memory Entity | `MenuItemId=9`, `Price=285000`, `IsAvailable=true`, `IsDeleted=true` | Món ăn đã bị xóa vĩnh viễn |

---

## 4. Test Case Specification

**TC ID format**: `UC25-TC-[NNN]`
**Severity**: CRITICAL / HIGH / MEDIUM / LOW

### UC25-TC-001 — Create Order Happy Path (Filtering zero quantities)
**Severity**: CRITICAL
**Feature Under Test**: `FoodOrderService.CreateFoodOrderAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-001

**Preconditions**:
- Sử dụng EF Core In-Memory Database.
- DB chứa FX-BK-001, FX-MN-001, FX-MN-003, FX-MN-004.

**Test Steps**:
1. Gọi `CreateFoodOrderAsync(request, currentUserId: 2)`.
2. Truyền vào request:
   - `BookingId`: 2
   - `Items`: `[{ MenuItemId: 1, Quantity: 2 }, { MenuItemId: 7, Quantity: 1 }, { MenuItemId: 10, Quantity: 0 }]`
3. Chờ kết quả trả về.
4. Truy vấn lại DB In-Memory để kiểm tra.

**Expected Result (PASS)**:
- Kết quả trả về `Status == FoodOrderCreateStatus.Created`.
- Trong DB xuất hiện đúng 1 `FoodOrder` có `BookingId == 2`, `OrderStatus == Pending`, `IsChargedToRoom == false`.
- `TotalAmount` tính đúng: `(85000 * 2) + (235000 * 1) = 405000`.
- Bảng `FoodOrderItems` có đúng 2 records (MenuItem 1 và 7). Món 10 bị lọc bỏ.
- `UnitPrice` của `FoodOrderItems` được lưu bằng `Price` của `MenuItem` tại thời điểm đó (Snapshot).
- Trong DB xuất hiện đúng 1 `Kot` tương ứng với FoodOrderId mới tạo, có `Status == Pending`.

**Current Status**: 🟢 Passing

---

### UC25-TC-002 — Create Order Access Denied (Not Owner)
**Severity**: HIGH
**Feature Under Test**: `FoodOrderService.CreateFoodOrderAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-002

**Preconditions**:
- DB chứa FX-BK-002 (BookingId=7, UserId=7).

**Test Steps**:
1. Gọi `CreateFoodOrderAsync(request, currentUserId: 2)` (User 2 cố tình đặt cho Booking 7).
2. Truyền vào `BookingId`: 7, `Items`: `[{ MenuItemId: 1, Quantity: 1 }]`.

**Expected Result (PASS)**:
- Kết quả trả về `Status == FoodOrderCreateStatus.Forbidden`.
- DB KHÔNG có bất kỳ FoodOrder nào được tạo ra.

**Current Status**: 🟢 Passing

---

### UC25-TC-003 — Create Order Invalid State (Not Checked In)
**Severity**: HIGH
**Feature Under Test**: `FoodOrderService.CreateFoodOrderAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-003

**Preconditions**:
- DB chứa FX-BK-003 (BookingId=1, UserId=2, Status=Reserved).

**Test Steps**:
1. Gọi `CreateFoodOrderAsync(request, currentUserId: 2)`.
2. Truyền vào `BookingId`: 1, `Items`: `[{ MenuItemId: 1, Quantity: 1 }]`.

**Expected Result (PASS)**:
- Kết quả trả về `Status == FoodOrderCreateStatus.BookingNotCheckedIn`.
- Không tạo record mới trong DB.

**Current Status**: 🟢 Passing

---

### UC25-TC-004 — Create Order Validation Error (All Quantities are 0)
**Severity**: MEDIUM
**Feature Under Test**: `FoodOrderService.CreateFoodOrderAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-004

**Preconditions**:
- DB chứa FX-BK-001 và FX-MN-001, FX-MN-003.

**Test Steps**:
1. Gọi `CreateFoodOrderAsync(request, currentUserId: 2)`.
2. Truyền vào `BookingId`: 2, `Items`: `[{ MenuItemId: 1, Quantity: 0 }, { MenuItemId: 7, Quantity: 0 }]`.

**Expected Result (PASS)**:
- Kết quả trả về `Status == FoodOrderCreateStatus.NoItems`.
- DB không có record nào mới.

**Current Status**: 🟢 Passing

---

### UC25-TC-005 — Create Order Unavailable Menu Item (IsAvailable = false)
**Severity**: MEDIUM
**Feature Under Test**: `FoodOrderService.CreateFoodOrderAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-005

**Preconditions**:
- DB chứa FX-BK-001 và FX-MN-002 (MenuItemId=8, IsAvailable=false).

**Test Steps**:
1. Gọi `CreateFoodOrderAsync(request, currentUserId: 2)`.
2. Truyền vào `BookingId`: 2, `Items`: `[{ MenuItemId: 8, Quantity: 1 }]`.

**Expected Result (PASS)**:
- Kết quả trả về `Status == FoodOrderCreateStatus.MenuItemUnavailable`.
- DB không có record nào mới.

**Current Status**: 🟢 Passing

---

### UC25-TC-006 — Create Order Invalid Quantity (< 0)
**Severity**: MEDIUM
**Feature Under Test**: `FoodOrderService.CreateFoodOrderAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-006

**Preconditions**:
- DB chứa FX-BK-001 và FX-MN-001.

**Test Steps**:
1. Gọi `CreateFoodOrderAsync(request, currentUserId: 2)`.
2. Truyền vào `BookingId`: 2, `Items`: `[{ MenuItemId: 1, Quantity: -1 }]`.

**Expected Result (PASS)**:
- Kết quả trả về `Status == FoodOrderCreateStatus.InvalidQuantity`.
- DB không có record nào mới.

**Current Status**: 🟢 Passing

---

### UC25-TC-007 — Create Order Invalid Quantity (> 1000)
**Severity**: MEDIUM
**Feature Under Test**: `FoodOrderService.CreateFoodOrderAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-007

**Preconditions**:
- DB chứa FX-BK-001 và FX-MN-001.

**Test Steps**:
1. Gọi `CreateFoodOrderAsync(request, currentUserId: 2)`.
2. Truyền vào `BookingId`: 2, `Items`: `[{ MenuItemId: 1, Quantity: 1001 }]`.

**Expected Result (PASS)**:
- Kết quả trả về `Status == FoodOrderCreateStatus.InvalidQuantity`.
- DB không có record nào mới.

**Current Status**: 🟢 Passing

---

### UC25-TC-008 — Create Order Booking Not Found
**Severity**: HIGH
**Feature Under Test**: `FoodOrderService.CreateFoodOrderAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-008

**Preconditions**:
- DB hoàn toàn TRỐNG (không có Booking nào).
- DB chứa FX-MN-001.

**Test Steps**:
1. Gọi `CreateFoodOrderAsync(request, currentUserId: 2)`.
2. Truyền vào `BookingId`: 999, `Items`: `[{ MenuItemId: 1, Quantity: 1 }]`.

**Expected Result (PASS)**:
- Kết quả trả về `Status == FoodOrderCreateStatus.BookingNotFound`.
- DB không có record nào mới.

**Current Status**: 🟢 Passing

---

### UC25-TC-009 — Create Order Deleted Menu Item (IsDeleted = true)
**Severity**: MEDIUM
**Feature Under Test**: `FoodOrderService.CreateFoodOrderAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-009

**Preconditions**:
- DB chứa FX-BK-001 và FX-MN-005 (MenuItemId=9, IsDeleted=true).

**Test Steps**:
1. Gọi `CreateFoodOrderAsync(request, currentUserId: 2)`.
2. Truyền vào `BookingId`: 2, `Items`: `[{ MenuItemId: 9, Quantity: 1 }]`.

**Expected Result (PASS)**:
- Kết quả trả về `Status == FoodOrderCreateStatus.MenuItemUnavailable`.
- DB không có record nào mới.

**Current Status**: 🟢 Passing

---

### UC25-TC-010 — Create Order Menu Item Not Found
**Severity**: MEDIUM
**Feature Under Test**: `FoodOrderService.CreateFoodOrderAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-010

**Preconditions**:
- DB chứa FX-BK-001.

**Test Steps**:
1. Gọi `CreateFoodOrderAsync(request, currentUserId: 2)`.
2. Truyền vào `BookingId`: 2, `Items`: `[{ MenuItemId: 999, Quantity: 1 }]`.

**Expected Result (PASS)**:
- Kết quả trả về `Status == FoodOrderCreateStatus.MenuItemNotFound`.
- DB không có record nào mới.

**Current Status**: 🟢 Passing

---

### UC25-TC-011 — Create Order Duplicate Menu Item
**Severity**: MEDIUM
**Feature Under Test**: `FoodOrderService.CreateFoodOrderAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-011

**Preconditions**:
- DB chứa FX-BK-001 và FX-MN-001.

**Test Steps**:
1. Gọi `CreateFoodOrderAsync(request, currentUserId: 2)`.
2. Truyền vào `BookingId`: 2, `Items`: `[{ MenuItemId: 1, Quantity: 1 }, { MenuItemId: 1, Quantity: 2 }]`.

**Expected Result (PASS)**:
- Status == FoodOrderCreateStatus.DuplicateMenuItem.
- DB không có record nào mới.

**Current Status**: 🟢 Passing

---

### UC25-TC-012 — Build Menu Happy Path
**Severity**: HIGH
**Feature Under Test**: `FoodOrderService.BuildRoomServiceOrderAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-012

**Preconditions**:
- DB chứa FX-BK-001 (Booking 2), FX-MN-001, FX-MN-002, FX-MN-003.

**Test Steps**:
1. Gọi `BuildRoomServiceOrderAsync(bookingId: 2)`.

**Expected Result (PASS)**:
- Kết quả trả về khác null (kiểu `RoomServiceOrderViewModel`).
- ViewModel chứa đúng `BookingId` = 2.
- Danh sách món ăn trong ViewModel CHỈ bao gồm những món có `IsAvailable=true` và `IsDeleted=false` (loại FX-MN-002, loại FX-MN-005 nếu có).

**Current Status**: 🟢 Passing

---

### UC25-TC-013 — Build Menu Invalid Booking
**Severity**: HIGH
**Feature Under Test**: `FoodOrderService.BuildRoomServiceOrderAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-013

**Preconditions**:
- DB chứa FX-BK-003 (BookingId 1, Status=Reserved).

**Test Steps**:
1. Gọi `BuildRoomServiceOrderAsync(bookingId: 1)`.
2. Gọi `BuildRoomServiceOrderAsync(bookingId: 999)`.

**Expected Result (PASS)**:
- Cả 2 lần gọi đều trả về `null`.

**Current Status**: 🟢 Passing

---

## 5. Red-Green-Refactor Tracker

| TC ID | Test File | 🔴 RED confirmed | 🟢 GREEN (commit) | 🔵 REFACTOR note |
| --- | --- | --- | --- | --- |
| `UC25-TC-001` | `FoodOrderServiceTests.cs` | [x] | [x] | |
| `UC25-TC-002` | `FoodOrderServiceTests.cs` | [x] | [x] | |
| `UC25-TC-003` | `FoodOrderServiceTests.cs` | [x] | [x] | |
| `UC25-TC-004` | `FoodOrderServiceTests.cs` | [x] | [x] | |
| `UC25-TC-005` | `FoodOrderServiceTests.cs` | [x] | [x] | |
| `UC25-TC-006` | `FoodOrderServiceTests.cs` | [x] | [x] | |
| `UC25-TC-007` | `FoodOrderServiceTests.cs` | [x] | [x] | |
| `UC25-TC-008` | `FoodOrderServiceTests.cs` | [x] | [x] | |
| `UC25-TC-009` | `FoodOrderServiceTests.cs` | [x] | [x] | |
| `UC25-TC-010` | `FoodOrderServiceTests.cs` | [x] | [x] | |
| `UC25-TC-011` | `FoodOrderServiceTests.cs` | [x] | [x] | |
| `UC25-TC-012` | `FoodOrderServiceTests.cs` | [x] | [x] | |
| `UC25-TC-013` | `FoodOrderServiceTests.cs` | [x] | [x] | |

## 6. Entry / Exit Criteria

**Entry Criteria (Điều kiện bắt đầu)**
- [x] Spec kỹ thuật `PHUCNGUYEN-FOODORDER-IMP-001` v2.2 đã được duyệt.
- [x] Môi trường test project `PhucNguyenResort.Tests` đã cài đặt đủ các mock packages (xUnit, Moq, EntityFrameworkCore.InMemory).

**Exit Criteria (Điều kiện kết thúc — DoD)**
- [x] Tất cả unit tests trong spec này màu xanh (PASS).
- [x] Không sử dụng DB thật cho Unit Tests (phải chạy độc lập không phụ thuộc connection string).

## 7. Rollback Plan
- Revert code `FoodOrderService.cs` (nếu cần thiết).
- Báo cáo và thảo luận với Tech Lead nếu phát sinh lỗi không thể resolve ngay do vướng cấu trúc `Booking` hoặc `User`.
