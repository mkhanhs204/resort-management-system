Mẫu Đặc tả Kiểm thử Hướng Phát triển cho Use Case 73 (Update KOT Status)

**Document ID**: PHUCNGUYEN-FOODORDER-TDD-004
**Version**: 1.0
**Date**: 2026-06-18
**Status**: Draft
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: `Ho Minh Khanh - Backend Developer`
**Reviewed by**: [x] `[Truong Tran Cuong] - Approved (2026-06-18)`
**DPO Sign-off**: [x] Approved (2026-06-18)
**Approved by**: [x] Approved (2026-06-18)
**Classification**: Internal - Confidential

**References**:
- `01_SRS/SWP391_NHOM1_V2.md` (UC73 Update KOT Status)
- `03_Implement/FoodOrder_EDS.md` (KOT Workflow & ADR-002 Dual Status)
- `03_Implement/FoodOrder_TDD/UC66_UpdateFoodOrderStatus_TDD.md` (Để đồng bộ phong cách)

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code. Thứ tự bắt buộc: viết test -> chạy -> xác nhận FAIL 🔴 -> implement -> PASS 🟢 -> refactor 🔵.

## CHANGELOG
| Ngày | Người thực hiện | Nội dung thay đổi |
| --- | --- | --- |
| 2026-06-18 | Antigravity | Khởi tạo tài liệu — TDD spec cho UC73 Update KOT Status |

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
| Field | Value |
| --- | --- |
| Feature | Cập nhật trạng thái phơi bếp (Kitchen Order Ticket - KOT) |
| Module | `Food Order & Room Service (Back-of-house)` |
| Spec gốc | `FoodOrder_EDS.md` (ADR-002, US-FOOD-005) |
| Priority | 🔴 P0 / CRITICAL |
| Downstream Consumers | F&B Staff (Dựa vào KOT để Giao hàng - UC66) |

## 2. Logic Issues Resolved
| # | Spec gốc (SRS) | Thực tế (EDS / Codebase) | Fix áp dụng trong test |
| --- | --- | --- | --- |
| L1 | Primary Actor là Kitchen Staff. Không đề cập trạng thái Cancelled. | Enum `KotStatus` có trạng thái `Cancelled` do hệ thống kích hoạt ngầm (ADR-002). | Đảm bảo Kitchen Staff KHÔNG THỂ chủ động cập nhật sang `Cancelled`. Bắt lỗi `KOT-003` nếu cố tình. |
| L2 | Workflow là `Pending ➔ Cooking ➔ Ready ➔ Completed`. | State Machine áp dụng cho `UpdateKotStatusAsync`. | Nếu truyền sai logic, trả về lỗi Invalid Transition. |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi
Mô tả phạm vi kiểm thử cho layer Service.
```
FoodOrder & KOT Module
├── Service: KotService
└── Integration: ResortDbContext (In-Memory cho Unit Test)
```

### TDS-02 — Test Basis / Cơ sở Kiểm thử
| Source | Items Derived |
| --- | --- |
| `SRS.md` UC73 | Workflow: Pending -> Cooking -> Ready -> Completed. |
| `ADR-002` | KOT là thực thể riêng biệt map 1:1 với FoodOrder. Bếp quản lý KOT. |
| `FoodOrder_EDS.md` | Bắt lỗi `KOT-003` (Invalid KOT status transition). |

### TDS-03 — Test Conditions and Coverage Items
| Condition ID | Test Condition | Coverage Item | Test Cases |
| --- | --- | --- | --- |
| TC-COND-001 | Cập nhật trạng thái bếp tuần tự chuẩn xác (Happy Path) | `KotService.UpdateKotStatusAsync` | `UC73-TC-001`, `TC-002`, `TC-003` |
| TC-COND-002 | Vượt cấp trạng thái (nhảy cóc) | `KotService.UpdateKotStatusAsync` | `UC73-TC-004` |
| TC-COND-003 | Đảo ngược trạng thái (lùi lại) | `KotService.UpdateKotStatusAsync` | `UC73-TC-005` |
| TC-COND-004 | Ép trạng thái Cancelled thủ công | `KotService.UpdateKotStatusAsync` | `UC73-TC-006` |

## 4. Test Case Specification

### UC73-TC-001 — Happy Path: Pending ➔ Cooking
**Severity**: CRITICAL
**Feature Under Test**: `KotService.UpdateKotStatusAsync`
**Test File**: `KotServiceTests.cs`
**TDD Phase**: 🟢 GREEN — passed
**Preconditions**: DB có một KOT trạng thái `Pending` (Id=1).
**Test Steps**:
1. Gọi `UpdateKotStatusAsync(1, KotStatus.Cooking)`.
2. Kiểm tra DB xem Kot(1) có đổi thành `Cooking` không.
**Expected Result (PASS)**:
- Method trả về `Success`.
- DB lưu lại `Status = Cooking`.

### UC73-TC-002 — Happy Path: Cooking ➔ Ready
**Severity**: CRITICAL
**Feature Under Test**: `KotService.UpdateKotStatusAsync`
**Test File**: `KotServiceTests.cs`
**TDD Phase**: 🟢 GREEN — passed
**Preconditions**: DB có một KOT trạng thái `Cooking` (Id=2).
**Test Steps**:
1. Gọi `UpdateKotStatusAsync(2, KotStatus.Ready)`.
2. Kiểm tra DB xem Kot(2) có đổi thành `Ready` không.
**Expected Result (PASS)**:
- Method trả về `Success`.
- DB lưu lại `Status = Ready`.

### UC73-TC-003 — Happy Path: Ready ➔ Completed
**Severity**: CRITICAL
**Feature Under Test**: `KotService.UpdateKotStatusAsync`
**Test File**: `KotServiceTests.cs`
**TDD Phase**: 🟢 GREEN — passed
**Preconditions**: DB có một KOT trạng thái `Ready` (Id=3).
**Test Steps**:
1. Gọi `UpdateKotStatusAsync(3, KotStatus.Completed)`.
2. Kiểm tra DB xem Kot(3) có đổi thành `Completed` không.
**Expected Result (PASS)**:
- Method trả về `Success`.
- DB lưu lại `Status = Completed`.

### UC73-TC-004 — Invalid Transition: Nhảy cóc (Pending ➔ Ready)
**Severity**: HIGH
**Feature Under Test**: `KotService.UpdateKotStatusAsync`
**Test File**: `KotServiceTests.cs`
**TDD Phase**: 🟢 GREEN — passed
**Preconditions**: DB có một KOT trạng thái `Pending` (Id=4).
**Test Steps**: Gọi `UpdateKotStatusAsync(4, KotStatus.Ready)`.
**Expected Result (PASS)**:
- Method trả về `Failed` (Error Message chứa mã "KOT-003").
- Trạng thái KOT trong DB vẫn giữ nguyên là `Pending`.

### UC73-TC-005 — Invalid Transition: Lùi trạng thái (Completed ➔ Cooking)
**Severity**: HIGH
**Feature Under Test**: `KotService.UpdateKotStatusAsync`
**Test File**: `KotServiceTests.cs`
**TDD Phase**: 🟢 GREEN — passed
**Preconditions**: DB có một KOT trạng thái `Completed` (Id=5).
**Test Steps**: Gọi `UpdateKotStatusAsync(5, KotStatus.Cooking)`.
**Expected Result (PASS)**:
- Method trả về `Failed` (Error Message chứa mã "KOT-003").
- Trạng thái KOT trong DB vẫn giữ nguyên là `Completed`.

### UC73-TC-006 — Invalid Transition: Cố tình Cancelled thủ công
**Severity**: HIGH
**Feature Under Test**: `KotService.UpdateKotStatusAsync`
**Test File**: `KotServiceTests.cs`
**TDD Phase**: 🟢 GREEN — passed
**Preconditions**: DB có một KOT trạng thái `Pending` (Id=6).
**Test Steps**: Gọi `UpdateKotStatusAsync(6, KotStatus.Cancelled)`.
**Expected Result (PASS)**:
- Method trả về `Failed` (Error Message chứa mã "KOT-003").
- Kitchen Staff KHÔNG ĐƯỢC PHÉP tự ý đổi sang Cancelled (Đây là quyền của hệ thống / F&B Staff).

## 5. Red-Green-Refactor Tracker
| TC ID | Test File | 🔴 RED confirmed | 🟢 GREEN (commit) | 🔵 REFACTOR note |
| --- | --- | --- | --- | --- |
| `UC73-TC-001` | `KotServiceTests.cs` | [x] | [x] | |
| `UC73-TC-002` | `KotServiceTests.cs` | [x] | [x] | |
| `UC73-TC-003` | `KotServiceTests.cs` | [x] | [x] | |
| `UC73-TC-004` | `KotServiceTests.cs` | [x] | [x] | |
| `UC73-TC-005` | `KotServiceTests.cs` | [x] | [x] | |
| `UC73-TC-006` | `KotServiceTests.cs` | [x] | [x] | |

## 6. Entry / Exit Criteria
**Exit Criteria (DoD)**:
- [x] Implement `KotService.UpdateKotStatusAsync` vượt qua 6/6 test cases.
- [x] Tuân thủ chặt chẽ State Machine: `Pending ➔ Cooking ➔ Ready ➔ Completed`.
- [x] Chống lại việc cập nhật `Cancelled` bằng API của Bếp.
