# TEST-DRIVEN DEVELOPMENT SPECIFICATION — UC66 (Update Food Order Status)

Mẫu Đặc tả Kiểm thử Hướng Phát triển cho Use Case 66 (Cập nhật trạng thái đơn hàng đồ ăn)

**Document ID**: PHUCNGUYEN-FOODORDER-TDD-002
**Version**: 1.0
**Date**: 2026-06-18
**Status**: Draft
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: `Ho Minh Khanh - Backend Developer`
**Reviewed by**: [x] `[Truong Tran Cuong]` - Approved
**DPO Sign-off**: [x] Approved
**Approved by**: [x] Approved
**Classification**: Internal - Confidential

**References**:
- `Tailieu/SWP391_NHOM1_VER2.pdf` — Functional requirements (UC66: Update Food Order Status)
- `03_implement/FoodOrder_EDS.md` (v2.2) — Technical Specification cho Food Order
- `03_implement/FoodOrder_TDD/UC25_FoodOrder_TDD.md` — TDD spec cho UC25 (phần tạo đơn)
- `02_Design/ADR/ADR-002` — Dual Status Model (FoodOrderStatus + KotStatus)

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.
Thứ tự bắt buộc: viết test (.cs) -> chạy -> xác nhận FAIL 🔴 -> implement -> PASS 🟢 -> refactor 🔵.
Không mark test là ✅ nếu chạy unit test chưa xanh.
Test data sử dụng data giả lập (EF Core In-Memory DB) cho unit tests.

## CHANGELOG
**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ.

| Ngày       | Người thực hiện | Nội dung thay đổi                                                     |
| ------------| -----------------| -----------------------------------------------------------------------|
| 2026-06-18 | `Ho Minh Khanh` | Khởi tạo tài liệu — TDD spec cho UC66 (Update Food Order Status)     |

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
| Feature / Gap ID | UC66 (Update Food Order Status) |
| Module | `Food Order & Room Service` |
| Spec gốc | `PHUCNGUYEN-FOODORDER-IMP-001` (v2.2) |
| Priority | 🔴 P0 |
| Sprint | S[N] |
| Data Classification | Internal |
| Compliance Scope | Phân quyền truy cập theo role `FnBStaff` |
| Upstream Dependencies | `UC25 (CreateFoodOrder)` — phải có đơn hàng tồn tại trước |
| Downstream Consumers | `UC69 (ChargeToRoom)` — chỉ charge khi status = Delivered |

## 2. Logic Issues Resolved

| # | Spec gốc (sai / thiếu) | Thực tế (policy trong EDS v2.2) | Fix áp dụng trong test |
| --- | --- | --- | --- |
| L1 | SRS chỉ ghi chung "Order status must follow allowed order lifecycle" mà không liệt kê cụ thể các transition hợp lệ. | EDS §6.3 định nghĩa State Machine rõ ràng: `Pending → Cooking → Delivered`. Chỉ Pending mới được Cancel. | Test cases phải cover **TẤT CẢ** các transition bất hợp lệ (vd: Delivered → Pending, Cooking → Cancelled) và xác nhận trả về `InvalidState`. |
| L2 | SRS không đề cập kiểu trả về của hàm cập nhật. | EDS §8.1 định nghĩa `FoodOrderActionResult` với enum `FoodOrderActionStatus` (Success, NotFound, InvalidState, Forbidden). | Test cases assert trên object `FoodOrderActionResult` thay vì kiểu `bool`. |
| L3 | SRS không đề cập ràng buộc giữa FoodOrderStatus và KotStatus khi chuyển trạng thái. | EDS §6.3 ghi: Cooking → Delivered chỉ được phép khi `KotStatus = Ready hoặc Completed`. | Test case cần kiểm tra ràng buộc cross-status này. |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi
TDD spec này tập trung hoàn toàn vào luồng **F&B Staff cập nhật trạng thái đơn hàng**:
- **Service Layer only**:
  - `FoodOrderService.UpdateOrderStatusAsync(int foodOrderId, FoodOrderStatus newStatus)`

```
[Module] bao gồm các layer:
├── Service (EF Core In-Memory DB)
│   └── FoodOrderService.UpdateOrderStatusAsync
└── Unit Test (xUnit + EF Core InMemory)
    └── FoodOrderServiceTests.cs (UpdateOrderStatus section)
```

### TDS-02 — Test Basis / Cơ sở Kiểm thử
| Source | Items Derived |
| --- | --- |
| `SWP391_NHOM1_VER2.pdf` UC66 | F&B Staff cập nhật trạng thái đơn hàng |
| `FoodOrder_EDS.md` §6.3 State Machine | Transition hợp lệ: Pending→Cooking, Pending→Cancelled, Cooking→Delivered |
| `FoodOrder_EDS.md` §6.3 Điều kiện | Cooking→Delivered chỉ khi KotStatus = Ready hoặc Completed |
| `FoodOrder_EDS.md` §8.1 Result Object | Trả về `FoodOrderActionResult` với các status: Success, NotFound, InvalidState |
| `FoodOrder_EDS.md` Error Code FOOD-003 | FoodOrderId không tồn tại → NotFound |
| `FoodOrder_EDS.md` Error Code FOOD-006 | Chuyển trạng thái bất hợp lệ → InvalidState |

### TDS-03 — Test Conditions and Coverage Items
| Condition ID | Test Condition | Coverage Item | Test Cases |
| --- | --- | --- | --- |
| TC-COND-001 | Happy path: Pending → Cooking | `UpdateOrderStatusAsync` | `UC66-TC-001` |
| TC-COND-002 | Happy path: Cooking → Delivered (KOT = Ready) | `UpdateOrderStatusAsync` | `UC66-TC-002` |
| TC-COND-003 | Happy path: Pending → Cancelled | `UpdateOrderStatusAsync` | `UC66-TC-003` |
| TC-COND-004 | Invalid: Pending → Delivered (nhảy cóc) | `UpdateOrderStatusAsync` | `UC66-TC-004` |
| TC-COND-005 | Invalid: Cooking → Cancelled (không cho hủy khi đang nấu) | `UpdateOrderStatusAsync` | `UC66-TC-005` |
| TC-COND-006 | Invalid: Delivered → Pending (lùi trạng thái) | `UpdateOrderStatusAsync` | `UC66-TC-006` |
| TC-COND-007 | Invalid: Delivered → Cooking (lùi trạng thái) | `UpdateOrderStatusAsync` | `UC66-TC-007` |
| TC-COND-008 | Invalid: Cancelled → Pending (hồi sinh đơn đã hủy) | `UpdateOrderStatusAsync` | `UC66-TC-008` |
| TC-COND-009 | FoodOrderId không tồn tại | `UpdateOrderStatusAsync` | `UC66-TC-009` |
| TC-COND-010 | Cooking → Delivered nhưng KOT chưa Ready | `UpdateOrderStatusAsync` | `UC66-TC-010` |

### TDS-04 — Test Techniques / Kỹ thuật Kiểm thử
| Technique (ISO 29119-4) | Applied To | Rationale |
| --- | --- | --- |
| State Transition Testing | FoodOrderStatus enum | State Machine có 4 trạng thái và 3 transition hợp lệ, cần kiểm tra đầy đủ valid + invalid |
| Equivalence Partitioning | Transition input | Chia thành 2 class: transition hợp lệ (3 cặp) và transition bất hợp lệ (tất cả cặp còn lại) |
| Boundary Value Analysis | FoodOrderId | Kiểm tra ID tồn tại vs ID không tồn tại |
| Error Guessing | Cross-status dependency | Kiểm tra ràng buộc giữa FoodOrderStatus và KotStatus |

### TDS-05 — Test Data Requirements
| Fixture ID | Type | Value / Logic | Mục đích |
| --- | --- | --- | --- |
| FX-ORD-001 | In-Memory Entity | `FoodOrderId=1`, `BookingId=2`, `OrderStatus=Pending`, `IsChargedToRoom=false` + KOT (`KotId=1`, `Status=Pending`) | Đơn hàng mới tạo (Happy path Pending→Cooking) |
| FX-ORD-002 | In-Memory Entity | `FoodOrderId=2`, `BookingId=2`, `OrderStatus=Cooking` + KOT (`KotId=2`, `Status=Ready`) | Đơn đang nấu, bếp xong (Happy path Cooking→Delivered) |
| FX-ORD-003 | In-Memory Entity | `FoodOrderId=3`, `BookingId=2`, `OrderStatus=Delivered` | Đơn đã giao (Terminal state - mọi transition đều invalid) |
| FX-ORD-004 | In-Memory Entity | `FoodOrderId=4`, `BookingId=2`, `OrderStatus=Cancelled` | Đơn đã hủy (Terminal state - mọi transition đều invalid) |
| FX-ORD-005 | In-Memory Entity | `FoodOrderId=5`, `BookingId=2`, `OrderStatus=Cooking` + KOT (`KotId=5`, `Status=Pending`) | Đơn đang nấu, bếp CHƯA xong (Blocked Cooking→Delivered) |
| FX-BK-001 | In-Memory Entity | `BookingId=2`, `UserId=2`, `Status=CheckedIn` | Booking chuẩn |

---

## 4. Test Case Specification

**TC ID format**: `UC66-TC-[NNN]`
**Severity**: CRITICAL / HIGH / MEDIUM / LOW

---

### UC66-TC-001 — Update Status Happy Path: Pending → Cooking
**Severity**: CRITICAL
**Feature Under Test**: `FoodOrderService.UpdateOrderStatusAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — PASS
**Condition Ref**: TC-COND-001

**Preconditions**:
- Sử dụng EF Core In-Memory Database.
- DB chứa FX-BK-001, FX-ORD-001 (FoodOrder status=Pending, KOT status=Pending).

**Test Steps**:
1. Gọi `UpdateOrderStatusAsync(foodOrderId: 1, newStatus: FoodOrderStatus.Cooking)`.
2. Truy vấn lại DB để kiểm tra trạng thái.

**Expected Result (PASS)**:
- Kết quả trả về `Status == FoodOrderActionStatus.Success`.
- Trong DB, FoodOrder có `FoodOrderId=1` phải có `OrderStatus == Cooking`.

**Expected Result (FAIL — dấu hiệu lỗi)**:
- Trả về `InvalidState` hoặc exception → State Machine chưa được implement đúng.

**Current Status**: 🟢 PASS

---

### UC66-TC-002 — Update Status Happy Path: Cooking → Delivered (KOT = Ready)
**Severity**: CRITICAL
**Feature Under Test**: `FoodOrderService.UpdateOrderStatusAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — PASS
**Condition Ref**: TC-COND-002

**Preconditions**:
- DB chứa FX-BK-001, FX-ORD-002 (FoodOrder status=Cooking, KOT status=Ready).

**Test Steps**:
1. Gọi `UpdateOrderStatusAsync(foodOrderId: 2, newStatus: FoodOrderStatus.Delivered)`.
2. Truy vấn lại DB để kiểm tra trạng thái.

**Expected Result (PASS)**:
- Kết quả trả về `Status == FoodOrderActionStatus.Success`.
- Trong DB, FoodOrder có `FoodOrderId=2` phải có `OrderStatus == Delivered`.

**Expected Result (FAIL — dấu hiệu lỗi)**:
- Trả về `InvalidState` → Thiếu logic check KotStatus hoặc State Machine sai.

**Current Status**: 🟢 PASS

---

### UC66-TC-003 — Update Status Happy Path: Pending → Cancelled
**Severity**: HIGH
**Feature Under Test**: `FoodOrderService.UpdateOrderStatusAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — PASS
**Condition Ref**: TC-COND-003

**Preconditions**:
- DB chứa FX-BK-001, FX-ORD-001 (FoodOrder status=Pending).

**Test Steps**:
1. Gọi `UpdateOrderStatusAsync(foodOrderId: 1, newStatus: FoodOrderStatus.Cancelled)`.
2. Truy vấn lại DB để kiểm tra.

**Expected Result (PASS)**:
- Kết quả trả về `Status == FoodOrderActionStatus.Success`.
- Trong DB, FoodOrder có `FoodOrderId=1` phải có `OrderStatus == Cancelled`.

**Expected Result (FAIL — dấu hiệu lỗi)**:
- Trả về `InvalidState` → Logic Cancel chưa bao phủ trường hợp từ Pending.

**Current Status**: 🟢 PASS
**Implementation Note**: Theo EDS §6.3, chỉ Pending mới được Cancel. Cần kiểm tra thêm KOT chưa bị bếp nhận.

---

### UC66-TC-004 — Invalid Transition: Pending → Delivered (Nhảy cóc)
**Severity**: HIGH
**Feature Under Test**: `FoodOrderService.UpdateOrderStatusAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — PASS
**Condition Ref**: TC-COND-004

**Preconditions**:
- DB chứa FX-BK-001, FX-ORD-001 (FoodOrder status=Pending).

**Test Steps**:
1. Gọi `UpdateOrderStatusAsync(foodOrderId: 1, newStatus: FoodOrderStatus.Delivered)`.

**Expected Result (PASS — hệ thống chặn đúng)**:
- Kết quả trả về `Status == FoodOrderActionStatus.InvalidState`.
- Trong DB, FoodOrder `FoodOrderId=1` **vẫn giữ nguyên** `OrderStatus == Pending` (không bị thay đổi).

**Expected Result (FAIL — lỗ hổng tồn tại)**:
- Trả về `Success` và status nhảy thẳng sang Delivered → State Machine bị bỏ qua (bypass).

**Current Status**: 🟢 PASS

---

### UC66-TC-005 — Invalid Transition: Cooking → Cancelled (Không cho hủy khi đang nấu)
**Severity**: HIGH
**Feature Under Test**: `FoodOrderService.UpdateOrderStatusAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — PASS
**Condition Ref**: TC-COND-005

**Preconditions**:
- DB chứa FX-BK-001, FX-ORD-002 (FoodOrder status=Cooking).

**Test Steps**:
1. Gọi `UpdateOrderStatusAsync(foodOrderId: 2, newStatus: FoodOrderStatus.Cancelled)`.

**Expected Result (PASS — hệ thống chặn đúng)**:
- Kết quả trả về `Status == FoodOrderActionStatus.InvalidState`.
- Trong DB, FoodOrder `FoodOrderId=2` **vẫn giữ nguyên** `OrderStatus == Cooking`.

**Expected Result (FAIL — lỗ hổng tồn tại)**:
- Trả về `Success` → Bếp đã nấu rồi mà F&B hủy đơn được → Gây mất công sức bếp, thiệt hại nguyên liệu.

**Current Status**: 🟢 PASS
**Implementation Note**: Theo EDS §6.3, chỉ Pending mới được Cancel. Khi đã Cooking trở đi thì KHÔNG được hủy.

---

### UC66-TC-006 — Invalid Transition: Delivered → Pending (Lùi trạng thái)
**Severity**: HIGH
**Feature Under Test**: `FoodOrderService.UpdateOrderStatusAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — PASS
**Condition Ref**: TC-COND-006

**Preconditions**:
- DB chứa FX-BK-001, FX-ORD-003 (FoodOrder status=Delivered).

**Test Steps**:
1. Gọi `UpdateOrderStatusAsync(foodOrderId: 3, newStatus: FoodOrderStatus.Pending)`.

**Expected Result (PASS — hệ thống chặn đúng)**:
- Kết quả trả về `Status == FoodOrderActionStatus.InvalidState`.
- Trong DB, FoodOrder **vẫn giữ nguyên** `OrderStatus == Delivered`.

**Expected Result (FAIL — lỗ hổng tồn tại)**:
- Trả về `Success` → Đơn đã giao bị kéo lùi về Pending → Gây rối loạn nghiệp vụ, tiềm ẩn gian lận tài chính (Charge lại lần 2).

**Current Status**: 🟢 PASS

---

### UC66-TC-007 — Invalid Transition: Delivered → Cooking (Lùi trạng thái)
**Severity**: MEDIUM
**Feature Under Test**: `FoodOrderService.UpdateOrderStatusAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — PASS
**Condition Ref**: TC-COND-007

**Preconditions**:
- DB chứa FX-BK-001, FX-ORD-003 (FoodOrder status=Delivered).

**Test Steps**:
1. Gọi `UpdateOrderStatusAsync(foodOrderId: 3, newStatus: FoodOrderStatus.Cooking)`.

**Expected Result (PASS — hệ thống chặn đúng)**:
- Kết quả trả về `Status == FoodOrderActionStatus.InvalidState`.
- Trong DB, FoodOrder **vẫn giữ nguyên** `OrderStatus == Delivered`.

**Expected Result (FAIL — lỗ hổng tồn tại)**:
- Trả về `Success` → Delivered bị kéo về Cooking → Logic sai hoàn toàn.

**Current Status**: 🟢 PASS

---

### UC66-TC-008 — Invalid Transition: Cancelled → Pending (Hồi sinh đơn đã hủy)
**Severity**: MEDIUM
**Feature Under Test**: `FoodOrderService.UpdateOrderStatusAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — PASS
**Condition Ref**: TC-COND-008

**Preconditions**:
- DB chứa FX-BK-001, FX-ORD-004 (FoodOrder status=Cancelled).

**Test Steps**:
1. Gọi `UpdateOrderStatusAsync(foodOrderId: 4, newStatus: FoodOrderStatus.Pending)`.

**Expected Result (PASS — hệ thống chặn đúng)**:
- Kết quả trả về `Status == FoodOrderActionStatus.InvalidState`.
- Trong DB, FoodOrder **vẫn giữ nguyên** `OrderStatus == Cancelled`.

**Expected Result (FAIL — lỗ hổng tồn tại)**:
- Trả về `Success` → Đơn đã hủy bị hồi sinh → Gây ra phantom order trong hệ thống.

**Current Status**: 🟢 PASS

---

### UC66-TC-009 — Order Not Found
**Severity**: HIGH
**Feature Under Test**: `FoodOrderService.UpdateOrderStatusAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — PASS
**Condition Ref**: TC-COND-009

**Preconditions**:
- DB hoàn toàn TRỐNG (không có FoodOrder nào).

**Test Steps**:
1. Gọi `UpdateOrderStatusAsync(foodOrderId: 999, newStatus: FoodOrderStatus.Cooking)`.

**Expected Result (PASS)**:
- Kết quả trả về `Status == FoodOrderActionStatus.NotFound`.

**Expected Result (FAIL — lỗ hổng tồn tại)**:
- Ném `NullReferenceException` → Thiếu null check trong service.

**Current Status**: 🟢 PASS

---

### UC66-TC-010 — Cooking → Delivered BLOCKED khi KOT chưa Ready
**Severity**: CRITICAL
**Feature Under Test**: `FoodOrderService.UpdateOrderStatusAsync`
**Test File**: `FoodOrderServiceTests.cs`
**TDD Phase**: 🟢 GREEN — PASS
**Condition Ref**: TC-COND-010

**Preconditions**:
- DB chứa FX-BK-001, FX-ORD-005 (FoodOrder status=Cooking, KOT status=Pending — bếp chưa nấu xong).

**Test Steps**:
1. Gọi `UpdateOrderStatusAsync(foodOrderId: 5, newStatus: FoodOrderStatus.Delivered)`.

**Expected Result (PASS — hệ thống chặn đúng)**:
- Kết quả trả về `Status == FoodOrderActionStatus.InvalidState`.
- Trong DB, FoodOrder `FoodOrderId=5` **vẫn giữ nguyên** `OrderStatus == Cooking`.

**Expected Result (FAIL — lỗ hổng tồn tại)**:
- Trả về `Success` → F&B Staff đánh dấu "Đã giao" trong khi bếp chưa nấu xong → Khách nhận đĩa không có đồ ăn.

**Current Status**: 🟢 PASS
**Implementation Note**: Theo EDS §6.3, Cooking → Delivered chỉ được phép khi `KotStatus = Ready hoặc Completed`. Service phải Include navigation property `Kot` khi query FoodOrder để kiểm tra ràng buộc này.

---

## 5. Red-Green-Refactor Tracker

| TC ID | Test File | 🔴 RED confirmed | 🟢 GREEN (commit) | 🔵 REFACTOR note |
| --- | --- | --- | --- | --- |
| `UC66-TC-001` | `FoodOrderServiceTests.cs` | [x] | [x] | Done |
| `UC66-TC-002` | `FoodOrderServiceTests.cs` | [x] | [x] | Done |
| `UC66-TC-003` | `FoodOrderServiceTests.cs` | [x] | [x] | Done |
| `UC66-TC-004` | `FoodOrderServiceTests.cs` | [x] | [x] | Done |
| `UC66-TC-005` | `FoodOrderServiceTests.cs` | [x] | [x] | Done |
| `UC66-TC-006` | `FoodOrderServiceTests.cs` | [x] | [x] | Done |
| `UC66-TC-007` | `FoodOrderServiceTests.cs` | [x] | [x] | Done |
| `UC66-TC-008` | `FoodOrderServiceTests.cs` | [x] | [x] | Done |
| `UC66-TC-009` | `FoodOrderServiceTests.cs` | [x] | [x] | Done |
| `UC66-TC-010` | `FoodOrderServiceTests.cs` | [x] | [x] | Done |

## 6. Entry / Exit Criteria

**Entry Criteria (Điều kiện bắt đầu)**
- [x] Spec kỹ thuật `PHUCNGUYEN-FOODORDER-IMP-001` v2.2 đã được duyệt.
- [x] UC25 TDD đã PASS toàn bộ (đảm bảo `CreateFoodOrderAsync` hoạt động đúng để tạo data test).
- [x] Môi trường test project `PhucNguyenResort.Tests` đã cài đặt đủ các packages (xUnit, EntityFrameworkCore.InMemory).

**Exit Criteria (Điều kiện kết thúc — DoD)**
- [x] Tất cả 10 unit tests trong spec này màu xanh (PASS).
- [x] Không sử dụng DB thật cho Unit Tests.
- [x] State Machine được enforce 100% — không có transition nào bị bỏ sót.
- [x] Ràng buộc cross-status (FoodOrderStatus ↔ KotStatus) được kiểm tra.

**Suspension Criteria (Điều kiện tạm dừng)**
- Phát hiện lỗi trong UC25 (CreateFoodOrder) ảnh hưởng đến data test.
- Cần thay đổi State Machine theo yêu cầu nghiệp vụ mới từ SRS.

## 7. Rollback Plan
- Revert code `FoodOrderService.UpdateOrderStatusAsync()` (nếu cần thiết).
- Báo cáo và thảo luận với Tech Lead nếu phát sinh lỗi do State Machine conflict giữa FoodOrderStatus và KotStatus.
