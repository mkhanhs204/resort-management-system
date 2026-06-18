# TEST-DRIVEN DEVELOPMENT SPECIFICATION TEMPLATE

Đặc tả Kiểm thử Hướng Phát triển — Refund Management

**Document ID**: SWP391-MOD3-TDD-001
**Version**: 1.0
**Date**: 2026-06-15
**Status**: Approve
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: Cuong
**Reviewed by**: Khanh
**DPO Sign-off**: Khanh
**Approved by**: Cuong
**Classification**: Internal - Confidential

**References**:
- `03_implement/EDS_Refund.md` — Technical Specification (SWP391-MOD3-IMP-001)

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.
Thứ tự bắt buộc: viết test → chạy → xác nhận FAIL 🔴 → implement → PASS 🟢 → refactor 🔵.
Test data dùng classification `SYNTHETIC`. Không dùng PII thật.

## CHANGELOG

**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ.

| Ngày      | Người thực hiện | Nội dung thay đổi                                                                                                                                          |
| ---------- | ------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 2026-06-15 | Cuong               | Khởi tạo tài liệu — TDD spec cho Refund Management                                                                                                            |

---

## MỤC LỤC

1. Thông tin Module
2. Test Design Specification (TDS)
3. Test Case Specification
4. Red-Green-Refactor Tracker
5. Entry / Exit Criteria

---

## 1. Thông tin Module

| Field                 | Value                                                                    |
| --------------------- | ------------------------------------------------------------------------ |
| Feature / Gap ID      | FEAT-REFUND-MANUAL                                                        |
| Module                | `Refund Management`                              |
| Spec gốc             | `SWP391-MOD3-IMP-001`                                                  |
| Priority              | 🟢 P0                                                                   |
| Sprint                | S2                                                                       |
| Milestone             | M1 - Core Booking Functions                                              |
| Data Classification   | Sensitive-PII (Bank Details) |

## 2. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi

`Refund` bao gồm các layer:

```
├── Models (Refund)
├── Controllers
│   └── RefundsController.ProcessRefund() ← ĐỐI TƯỢNG TEST MVC
├── Services
│   └── RefundService.ProcessRefundAsync() ← ĐỐI TƯỢNG TEST LOGIC DB
```

### TDS-02 — Test Conditions and Coverage Items

| Condition ID | Test Condition                                            | Coverage Item          | Test Cases     |
| ------------ | --------------------------------------------------------- | ---------------------- | -------------- |
| TC-COND-001  | Manager duyệt yêu cầu hoàn tiền thành công                 | `ProcessRefund`      | `REF-TC-001` |
| TC-COND-002  | Manager duyệt yêu cầu hoàn tiền thất bại do sai ID         | `ProcessRefund`      | `REF-TC-002` |
| TC-COND-003  | Manager duyệt yêu cầu đã xử lý (Not Pending)               | `ProcessRefund`      | `REF-TC-003` |

## 3. Test Case Specification

### REF-TC-001 — Manager duyệt yêu cầu hoàn tiền thành công

**Severity**: CRITICAL
**Feature Under Test**: `RefundsController.ProcessRefund()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-001

**Preconditions**:
- Có 1 bản ghi `Refund` với ID=5, `RefundStatus = Pending`.

**Test Steps**:
1. Manager gọi POST `/Refunds/ProcessRefund/5`.

**Expected Result (PASS)**:
- `RefundStatus` của bản ghi ID=5 cập nhật thành `Completed`.
- Controller redirect về `Index` kèm `TempData["SuccessMessage"]`.

### REF-TC-002 — Manager duyệt yêu cầu hoàn tiền thất bại do sai ID

**Severity**: LOW
**Feature Under Test**: `RefundsController.ProcessRefund()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-002

**Preconditions**:
- Không có bản ghi `Refund` với ID=999.

**Test Steps**:
1. Manager gọi POST `/Refunds/ProcessRefund/999`.

**Expected Result (PASS)**:
- Controller trả về kết quả `NotFound()`.

### REF-TC-003 — Manager duyệt yêu cầu đã xử lý

**Severity**: MEDIUM
**Feature Under Test**: `RefundsController.ProcessRefund()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-003

**Preconditions**:
- Có 1 bản ghi `Refund` với ID=6, `RefundStatus = Completed`.

**Test Steps**:
1. Manager gọi POST `/Refunds/ProcessRefund/6`.

**Expected Result (PASS)**:
- `RefundStatus` của bản ghi ID=6 giữ nguyên là `Completed`.
- Controller redirect về `Index` kèm `TempData["Error"]`.

## 4. Red-Green-Refactor Tracker

| TC ID          | Test File                  | 🔴 RED  | 🟢 GREEN | 🔵 REFACTOR |
| -------------- | -------------------------- | ------- | -------- | ----------- |
| `REF-TC-001` | `RefundServiceTests.cs`, `RefundsControllerTests.cs` | `[x]` | `[x]`  |             |
| `REF-TC-002` | `RefundServiceTests.cs`, `RefundsControllerTests.cs` | `[x]` | `[x]`  |             |
| `REF-TC-003` | `RefundServiceTests.cs`, `RefundsControllerTests.cs` | `[x]` | `[x]`  |             |

## 5. Entry / Exit Criteria

**Entry Criteria**
- [X] Spec kỹ thuật `EDS_Refund.md` đã được review
- [X] Các class EF Core DbContext, Model relationships đã cấu hình chuẩn.

**Exit Criteria**
- [X] Tất cả unit tests `REF-TC-*` đều PASS.
