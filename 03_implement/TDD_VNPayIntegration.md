# TEST-DRIVEN DEVELOPMENT SPECIFICATION

**Document ID**: SWP391-PAY-TDD-001
**Version**: 1.0
**Date**: 2026-06-13
**Status**: Draft
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: cuong
**Classification**: Internal - Confidential

**References**:

- `01_Requirements/SWP391-HOS-02.md` — UC23 (Thanh toán cọc/phí dịch vụ)
- `03_implement/EDS_VNPayIntegration.md` — Technical Specification
- Tài liệu kỹ thuật Cổng thanh toán VNPAY (Sandbox)

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

| Field                 | Value                   |
| --------------------- | ----------------------- |
| Feature / Gap ID      | GAP-PAY-001             |
| Module                | `Payment & Billing`   |
| Spec gốc             | `SWP391-PAY-IMP-001`  |
| Priority              | 🔴 P0                   |
| Sprint                | S1                      |
| Data Classification   | Financial               |
| Upstream Dependencies | `Room Booking Module` |
| Downstream Consumers  | `VNPAY Sandbox API`   |

## 2. Logic Issues Resolved

Liệt kê các quy tắc ngầm định (implicit rules) từ tài liệu VNPAY cần encode thành Test Cases:

| #  | Spec gốc (từ VNPAY)                    | Thực tế (C# / Business Logic)                            | Fix áp dụng trong test                                  |
| -- | ---------------------------------------- | ---------------------------------------------------------- | --------------------------------------------------------- |
| L1 | Amount phải là số nguyên (nhân 100) | Trong `Booking`, `DepositAmount` là `decimal` (VND) | `CreatePaymentUrl` phải `Amount * 100`               |
| L2 | Tham số hash phải sort theo alphabet   | Cần dùng thuật toán sắp xếp tham số khi hash        | Test case check sự tồn tại của query string đã sort |
| L3 | Trạng thái IPN                         | Không bao giờ trừ tiền khách lần 2 (Idempotency)     | Test gọi IPN 2 lần trên cùng 1 đơn                  |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi

`Payment & Billing` bao gồm các layer:
├── VNPayService (Logic tạo URL, Verify Checksum)
├── PaymentController (Xử lý HTTP Webhook)
└── Integration (DB Context)

### TDS-02 — Test Basis / Cơ sở Kiểm thử

| Source                      | Items Derived                                                         |
| --------------------------- | --------------------------------------------------------------------- |
| `SWP391-HOS-02` BR02      | Khách hàng phải thanh toán cọc thì mới hoàn tất đặt phòng |
| `EDS_VNPayIntegration.md` | API Specs, Error Codes (`00`, `97`, `04`)                       |
| Tài liệu VNPAY            | Thuật toán băm HMACSHA512                                          |

### TDS-03 — Test Conditions and Coverage Items

| Condition ID | Test Condition                           | Coverage Item                      | Test Cases           |
| ------------ | ---------------------------------------- | ---------------------------------- | -------------------- |
| TC-COND-001  | Tính toán Hash đúng chuẩn VNPAY     | `VNPayLibrary.CreateRequestUrl`  | `VNPAY-TC-001`     |
| TC-COND-002  | Xác thực IPN hợp lệ                  | `PaymentController.VNPayIPN`     | `VNPAY-TC-002`     |
| TC-COND-003  | Từ chối IPN giả mạo                  | `VNPayLibrary.ValidateSignature` | `VNPAY-TC-SEC-001` |
| TC-COND-004  | Xử lý Webhook trùng lặp (Idempotent) | `PaymentController.VNPayIPN`     | `VNPAY-TC-003`     |

### TDS-04 — Test Data Requirements

| Fixture ID | Type    | Value / Logic                                                                 | Mục đích           |
| ---------- | ------- | ----------------------------------------------------------------------------- | --------------------- |
| FX-001     | env     | `vnp_TmnCode=XONYMEI0`, `vnp_HashSecret=2Y3GKP2EU216H98QKZ2R9YCQ8QW3CY9K` | VNPay configs         |
| FX-002     | DB seed | Booking { Id: 10, DepositAmount: 500000, Status: PendingPayment }             | Test thanh toán cọc |
| FX-003     | DB seed | Booking { Id: 11, DepositAmount: 500000, Status: Reserved }                   | Test Idempotency      |

## 4. Test Case Specification

### VNPAY-TC-001 — Tạo Payment URL đúng format

**Severity**: HIGH
**Feature Under Test**: `VNPayService.CreatePaymentUrl()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-001

**Preconditions**:

- Load config từ `FX-001`.
- Model: `Amount = 500000`, `OrderId = 10`.

**Test Steps**:

1. Gọi `CreatePaymentUrl(model)`.
2. Phân tích chuỗi URL trả về.

**Expected Result (PASS)**:

- URL bắt đầu bằng `https://sandbox.vnpayment.vn/paymentv2/vpcpay.html`.
- Chứa `vnp_Amount=50000000` (đã nhân 100).
- Chứa `vnp_SecureHash` không null.

**Current Status**: 🟢 Passed

### VNPAY-TC-002 — Xác minh mã IPN hợp lệ (Happy Path)

**Severity**: CRITICAL
**Feature Under Test**: `PaymentController.VNPayIPN()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-002

**Preconditions**:

- DB có Booking `FX-002`.

**Test Steps**:

1. Giả lập GET Request đến `/Payment/VNPayIPN` với `vnp_Amount=50000000`, `vnp_TxnRef=10`.
2. Tạo `vnp_SecureHash` hợp lệ bằng `HashSecret`.
3. Kiểm tra response.
4. Kiểm tra BookingStatus trong DB.

**Expected Result (PASS)**:

- Response trả về JSON `{"RspCode": "00", "Message": "Confirm Success"}`.
- DB: Booking `10` có trạng thái đổi thành `Reserved` (Đã thanh toán cọc).
- Bảng `Payments` có 1 record mới được ghi lại.

**Current Status**: 🟢 Mock Passed (Integration DB test pending)

### VNPAY-TC-003 — Webhook Idempotency (Chống lặp)

**Severity**: HIGH
**Feature Under Test**: `PaymentController.VNPayIPN()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-004

**Preconditions**:

- DB có Booking `FX-003` (Đã được thanh toán trước đó).

**Test Steps**:

1. Gọi IPN hợp lệ như `VNPAY-TC-002` đối với Booking `11`.

**Expected Result (PASS)**:

- Response trả về JSON `{"RspCode": "02", "Message": "Order already confirmed"}`.
- DB không có record trùng lặp nào được tạo thêm.

**Current Status**: 🟢 Mock Passed

---

### SECURITY TEST CASES

#### VNPAY-TC-SEC-001 — Tấn công giả mạo IPN (Invalid Checksum)

**Severity**: CRITICAL
**OWASP**: A01:2021 — Broken Access Control
**CWE**: CWE-347 — Improper Verification of Cryptographic Signature
**Legal**: Vi phạm tính toàn vẹn dữ liệu tài chính
**Feature Under Test**: `PaymentController.VNPayIPN()`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-003

**Preconditions**:

- Hacker biết URL IPN và BookingId `10` đang chờ thanh toán.

**Test Steps (Attack Simulation)**:

1. Gọi `/Payment/VNPayIPN?vnp_TxnRef=10&vnp_Amount=50000000&vnp_SecureHash=fakehash123`
2. Kẻ tấn công cố gắng đánh lừa hệ thống cập nhật trạng thái đơn hàng.

**Expected Result (PASS = hệ thống an toàn)**:

- Response trả về JSON `{"RspCode": "97", "Message": "Invalid Checksum"}`.
- DB: Booking `10` VẪN ở trạng thái `PendingPayment`. Không có record ảo nào được ghi vào DB.

**Expected Result (FAIL = lỗ hổng tồn tại)**:

- Hàm bỏ qua bước check signature, hoặc check lỗi -> Đơn hàng chuyển sang `Reserved` dù khách chưa thanh toán.

**Current Status**: 🟢 Passed

## 5. Red-Green-Refactor Tracker

| TC ID                | Test File                      | 🔴 RED confirmed | 🟢 GREEN (commit) | 🔵 REFACTOR note |
| -------------------- | ------------------------------ | ---------------- | ----------------- | ---------------- |
| `VNPAY-TC-001`     | `VnPayIntegrationTests.cs`      | `[x]`          | `[x]`           |                  |
| `VNPAY-TC-002`     | `VnPayIntegrationTests.cs` | `[ ]`          | `[x]`           | Pending DB mock  |
| `VNPAY-TC-003`     | `VnPayIntegrationTests.cs` | `[ ]`          | `[x]`           | Pending DB mock  |
| `VNPAY-TC-SEC-001` | `VnPayIntegrationTests.cs` | `[x]`          | `[x]`           | Checksum solid   |

## 6. Entry / Exit Criteria

**Entry Criteria (Điều kiện bắt đầu)**

- [X] Spec kỹ thuật `EDS_VNPayIntegration` đã được approve.
- [X] Config mã `TmnCode`, `HashSecret` đã được cấp quyền truy cập.

**Exit Criteria (Điều kiện kết thúc — DoD)**

- [ ] Chữ ký bảo mật phải được check kỹ càng với HMACSHA512.
- [ ] Hàm IPN phải đạt tiêu chuẩn Idempotent.
- [ ] Source code không hardcode mã Secret.

## 7. Rollback Plan

Nếu tích hợp gặp sự cố nghiêm trọng (vd: sai công thức HMAC do VNPAY đổi rule):

```bash
# Revert routing (Tắt chuyển hướng VNPAY trong BookingsController)
git revert [hash_commit_vnpay_integration]

# Hủy các bảng DB không cần thiết (Nếu có migration cho Payment)
dotnet ef database update [previous_migration]
```
