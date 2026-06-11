# TEST-DRIVEN DEVELOPMENT SPECIFICATION TEMPLATE
Mẫu Đặc tả Kiểm thử Hướng Phát triển

**Document ID**: PHUCNGUYEN-IAM-TDD-001
**Version**: 1.0
**Date**: 2026-06-08
**Status**: Draft
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: `Student 1 - QA/Backend`
**Reviewed by**: [ ] `[Tech Lead] - Pending`
**Classification**: Internal - Confidential

**References**:
- `01_Requirements/SWP391-HOS-02.md` — Functional requirements
- `03_implement/UC01_EDS.md` — Technical Specification

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.

---

## 1. Thông tin Module
| Field               | Value　　　　　　　　　　　　　　　　|
| ---------------------| --------------------------------------|
| Feature / Gap ID    | UC01　　　　　　　　　　　　　　　　 |
| Module              | `Auth, Profiles & Master Data (IAM)` |
| Spec gốc            | `SWP391-HOS-02`　　　　　　　　　　　|
| Priority            | 🔴 P0　　　　　　　　　　　　　　　　|
| Data Classification | Sensitive-PII　　　　　　　　　　　　|
| Compliance Scope    | Decree 356/2025　　　　　　　　　　　|

## 2. Logic Issues Resolved
Bắt buộc điền trước khi viết test.

| #   | Spec gốc (sai / thiếu)                                          | Thực tế (schema / policy)                                           | Fix áp dụng trong test                                |
| -----| -----------------------------------------------------------------| ---------------------------------------------------------------------| -------------------------------------------------------|
| L1  | UC01 yêu cầu "verify my email" nhưng không quy định flow cụ thể | Thêm field `isEmailVerified` và `verificationToken` trong DB schema | Chặn login (return 403) nếu `isEmailVerified = false` |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi
```text
IAM Auth bao gồm các layer:
├── Domain (Entities & Events)
├── Application (AuthService)
├── Controller (AuthController)
└── Integration (PostgreSQL cho guests table)
```

### TDS-02 — Test Basis / Cơ sở Kiểm thử
| Source | Items Derived |
| --- | --- |
| `SWP391-HOS-02` UC01 | Guest có thể register, verify email, và login. |
| Decree 356/2025 | Passwords phải được băm (hashing). |

### TDS-03 — Test Conditions and Coverage Items
| Condition ID | Test Condition                        | Coverage Item        | Test Cases   |
| --------------| ---------------------------------------| ----------------------| --------------|
| TC-COND-001  | Đăng ký thành công tài khoản mới      | `POST /register`     | `IAM-TC-001` |
| TC-COND-002  | Đăng ký với email đã tồn tại          | `POST /register`     | `IAM-TC-002` |
| TC-COND-003  | Xác thực email thành công             | `POST /verify-email` | `IAM-TC-003` |
| TC-COND-004  | Đăng nhập đúng thông tin, đã verify   | `POST /login`        | `IAM-TC-004` |
| TC-COND-005  | Đăng nhập đúng thông tin, chưa verify | `POST /login`        | `IAM-TC-005` |

### TDS-05 — Test Data Requirements
| Fixture ID | Type    | Value / Logic                                                                      | Mục đích               |
| ------------| ---------| ------------------------------------------------------------------------------------| ------------------------|
| FX-001     | DB seed | `{ email: "test@example.com", isEmailVerified: true, passwordHash: "..." }`        | Happy path login       |
| FX-002     | DB seed | `{ email: "unverified@example.com", isEmailVerified: false, passwordHash: "..." }` | Unverified login block |

---

## 4. Test Case Specification
**TC ID format**: `IAM-TC-[NNN]`

### IAM-TC-001 — Đăng ký tài khoản mới thành công
**Severity**: CRITICAL
**Feature Under Test**: `POST /api/v1/auth/register`
**TDD Phase**: 🔴 RED

**Preconditions**:
- Email chưa tồn tại trong hệ thống.

**Test Steps**:
1. Gọi `POST /api/v1/auth/register` với email và password hợp lệ.
2. Kiểm tra Response.
3. Query database bảng `guests`.

**Expected Result (PASS)**:
- HTTP 201 Created.
- DB lưu bản ghi mới với `isEmailVerified = false`.
- Password trong DB đã được hash (không phải plaintext).

### IAM-TC-002 — Đăng ký trùng email
**Severity**: MEDIUM
**Feature Under Test**: `POST /api/v1/auth/register`
**TDD Phase**: 🔴 RED

**Test Steps**:
1. Seed DB với email `test@example.com`.
2. Gọi `POST /api/v1/auth/register` với `test@example.com`.

**Expected Result (PASS)**:
- HTTP 409 Conflict.
- Response trả về mã lỗi `IAM-002`.

### IAM-TC-003 — Xác thực email
**Severity**: HIGH
**Feature Under Test**: `POST /api/v1/auth/verify-email`
**TDD Phase**: 🔴 RED

**Test Steps**:
1. Seed DB với `unverified@example.com`, `verificationToken: "abc-123"`.
2. Gọi `POST /api/v1/auth/verify-email` với token "abc-123".

**Expected Result (PASS)**:
- HTTP 200 OK.
- DB bản ghi chuyển thành `isEmailVerified = true` và `verificationToken = null`.

### IAM-TC-004 — Đăng nhập thành công
**Severity**: CRITICAL
**Feature Under Test**: `POST /api/v1/auth/login`
**TDD Phase**: 🔴 RED

**Test Steps**:
1. Cung cấp đúng email và password của account đã verify (FX-001).

**Expected Result (PASS)**:
- HTTP 200 OK.
- Response body chứa `accessToken` hợp lệ.

### IAM-TC-005 — Đăng nhập khi chưa xác thực email
**Severity**: HIGH
**Feature Under Test**: `POST /api/v1/auth/login`
**TDD Phase**: 🔴 RED

**Test Steps**:
1. Cung cấp đúng email và password của account chưa verify (FX-002).

**Expected Result (PASS)**:
- HTTP 403 Forbidden.
- Mã lỗi `IAM-004`.

---

### SECURITY TEST CASES

#### IAM-TC-006 — SQL Injection/NoSQL Injection Payload
**Severity**: CRITICAL
**OWASP**: A03:2021 — Injection
**CWE**: CWE-89 — SQL Injection
**Feature Under Test**: `POST /api/v1/auth/login`
**TDD Phase**: 🔴 RED

**Test Steps**:
1. Gọi API login với email: `' OR 1=1 --`.
2. Password: `any`.

**Expected Result (PASS = hệ thống an toàn)**:
- HTTP 400 Validation Error hoặc 401 Invalid Credentials.

---

### INTEGRATION TEST CASES

#### IAM-TC-INT-001 — Luồng Đăng ký và Đăng nhập (E2E Auth Flow)
**Severity**: HIGH
**Feature Under Test**: Full flow Register -> Verify -> Login
**TDD Phase**: 🔴 RED

**Test Steps**:
1. Call `POST /register` với `e2e@example.com`.
2. Lấy `verificationToken` từ database trực tiếp (để mô phỏng việc user lấy token từ email).
3. Call `POST /verify-email` với token đó.
4. Call `POST /login` với credential vừa tạo.

**Expected Result (PASS)**:
- Tất cả bước đều trả về HTTP 2xx (201, 200, 200).
- Cuối cùng nhận được `accessToken`.

## 5. Entry / Exit Criteria
**Exit Criteria (DoD)**
- [ ] `npm test` pass tất cả unit và integration test.
- [ ] Mật khẩu không bao giờ in ra log.
- [ ] Code coverage > 80%.
