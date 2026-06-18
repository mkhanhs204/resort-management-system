# TEST-DRIVEN DEVELOPMENT SPECIFICATION TEMPLATE
Mẫu Đặc tả Kiểm thử Hướng Phát triển

**Document ID**: PHUCNGUYEN-TDD-MANAGERSTAFF-001
**Version**: 1.0
**Date**: 2026-06-18
**Status**: Approved
**Classification**: Internal - Confidential

## CHANGELOG
| Ngày | Người thực hiện | Nội dung thay đổi |
| --- | --- | --- |
| 2026-06-18 | AI | Khởi tạo tài liệu TDD spec cho Manager Staff |

---

## 1. Thông tin Module
| Field | Value |
| --- | --- |
| Feature / Gap ID | GAP-STAFF-001 |
| Module | `ManagerStaffModule` |
| Spec gốc | `PHUCNGUYEN-MANAGERSTAFF-IMP-001` |
| Priority | 🔴 P0 |
| Sprint | S1 |

## 2. Logic Issues Resolved
| # | Spec gốc (sai / thiếu) | Thực tế (schema / policy) | Fix áp dụng trong test |
| --- | --- | --- | --- |
| L1 | Chưa đề cập cấm tự khóa tài khoản của chính mình | Admin có thể vô tình khóa tài khoản của mình | Test case chặn Admin tự đổi IsBlocked của chính `userId` đang thao tác. |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi
`ManagerStaffModule` bao gồm các layer:
├── Domain Models (User, Role, UserProfile)
├── Services (StaffService, PasswordHasher)
└── Controllers (StaffController)

### TDS-03 — Test Conditions and Coverage Items
| Condition ID | Test Condition | Coverage Item | Test Cases |
| --- | --- | --- | --- |
| TC-COND-001 | Tạo tài khoản nhân viên thành công | `CreateStaffAccountAsync` | `MANAGERSTAFF-TC-001` |
| TC-COND-002 | Tạo tài khoản nhưng trùng Email | `CreateStaffAccountAsync` | `MANAGERSTAFF-TC-002` |
| TC-COND-003 | Khóa tài khoản thành công | `ToggleAccountStatusAsync`| `MANAGERSTAFF-TC-003` |

## 4. Test Case Specification

### MANAGERSTAFF-TC-001 — Happy path Create Staff Account
**Severity**: CRITICAL
**Feature Under Test**: `StaffService.CreateStaffAccountAsync()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:
- Chưa tồn tại Email `newstaff@resort.com` trong hệ thống.
- RoleId `3` (Receptionist) tồn tại hợp lệ.

**Test Steps**:
1. Gọi `CreateStaffAccountAsync` với dữ liệu hợp lệ (email, mật khẩu, roleId = 3).
2. Kiểm tra DB xem User và UserProfile đã được insert chưa.
3. Kiểm tra xem Password đã được Hash hay chưa (Không lưu chuỗi gốc).

**Expected Result (PASS)**:
- Method trả về thông tin User.
- Dữ liệu mật khẩu lưu ở DB là mã Hash (không phải plain text).

**Current Status**: 🟢 Passing

---

### MANAGERSTAFF-TC-002 — Prevent Duplicate Email
**Severity**: CRITICAL
**Feature Under Test**: `StaffService.CreateStaffAccountAsync()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:
- DB đã có sẵn User với Email `admin@resort.com`.

**Test Steps**:
1. Gọi `CreateStaffAccountAsync` yêu cầu tạo tài khoản với Email `admin@resort.com`.
2. Kiểm tra exception trả về.

**Expected Result (PASS)**:
- Quăng lỗi "Email already exists" (Code: STF-001).
- Không có user nào mới được chèn vào DB.

**Current Status**: 🟢 Passing

---

### MANAGERSTAFF-TC-003 — Block Account Successfully
**Severity**: HIGH
**Feature Under Test**: `StaffService.ToggleAccountStatusAsync()`
**TDD Phase**: 🟢 GREEN

**Preconditions**:
- Tồn tại User A có `IsBlocked` = false.

**Test Steps**:
1. Gọi `ToggleAccountStatusAsync` truyền vào `UserId` của User A và cờ `isBlocked = true`.
2. Truy vấn lại User A trong CSDL.

**Expected Result (PASS)**:
- Cập nhật thành công.
- `IsBlocked` của User A trong DB chuyển thành `true`.

**Current Status**: 🟢 Passing

---

## 5. Red-Green-Refactor Tracker
| TC ID | Test File | 🔴 RED confirmed | 🟢 GREEN (commit) | 🔵 REFACTOR note |
| --- | --- | --- | --- | --- |
| `MANAGERSTAFF-TC-001` | `StaffServiceTests.cs` | `[x]` | `[x]` | |
| `MANAGERSTAFF-TC-002` | `StaffServiceTests.cs` | `[x]` | `[x]` | |
| `MANAGERSTAFF-TC-003` | `StaffServiceTests.cs` | `[x]` | `[x]` | |
