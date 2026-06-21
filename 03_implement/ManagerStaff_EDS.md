# ENGINEERING DOCUMENTATION STANDARD (EDS) v2.0
Quy chuẩn Tài liệu Kỹ thuật và Đặc tả Hiện thực hóa

| Field | Value |
| --- | --- |
| Document ID | `PHUCNGUYEN-MANAGERSTAFF-IMP-001` |
| Version | 1.0 |
| Date | 2026-06-18 |
| Status | Approved |
| Document Owner | `Admin Team` |
| Author | `AI Developer` |
| Reviewed by | `Tech Lead` |
| DPO Sign-off | [x] Approved |
| Approved by | `Principal Architect` |
| Based on EDS | v2.0 |

## CHANGELOG
| Ngày | Người thực hiện | Nội dung thay đổi |
| --- | --- | --- |
| 2026-06-18 | AI | Khởi tạo tài liệu EDS cho module Manager Staff |

## MỤC LỤC
1. Tổng quan Module
2. Ma trận Truy vết (Traceability Matrix)
3. Architecture Decision Records (ADR)
4. Non-Functional Requirements & SLA
5. Static Modeling (Mô hình Tĩnh)
6. Dynamic Modeling (Mô hình Động)
7. Domain Event Catalog
8. Interface Specification (Đặc tả Giao diện)
9. API Specification
10. Bảng mã lỗi (Error Codes)
11. Quy trình Triển khai (Step-by-Step)
12. Rollback & Incident Runbook
13. Kịch bản Kiểm thử Chi tiết
14. Phương pháp Xác minh
15. Mẫu thử thực tế (API Verification Samples)
16. Bảng tổng hợp phân quyền (Authorization Matrix)

---

## 1. Tổng quan Module
Module **Manager Staff** hỗ trợ ban quản trị (Admin) quản lý vòng đời tài khoản của nhân viên: khởi tạo tài khoản, phân quyền (Role Assignment), tìm kiếm và khóa (Block) các tài khoản vi phạm chính sách hoặc đã nghỉ việc.

| Field | Value |
| --- | --- |
| Module Name | `ManagerStaffModule` |
| Bounded Context | `Identity & Access Management (IAM)` |
| Data Classification | PII / Sensitive (Mật khẩu, Thông tin cá nhân) |
| Compliance Scope | Decree 356/2025 (Bảo vệ dữ liệu cá nhân) |
| Upstream Dependencies | `AuthModule` |
| Downstream Consumers | Toàn bộ hệ thống |

## 2. Ma trận Truy vết (Traceability Matrix)
| Requirement ID | Loại (BR/ADR/US) | Mô tả yêu cầu | Thành phần Code | Compliance Target | ADR liên quan |
| --- | --- | --- | --- | --- | --- |
| UC32, UC34 | User Story | Xem và tìm kiếm tài khoản | `StaffController.GetAccounts()` | N/A | - |
| UC33 | User Story | Tạo tài khoản và cấp quyền | `StaffService.CreateStaffAccount()` | N/A | ADR-001 |
| UC36 | User Story | Khóa / Mở khóa tài khoản | `StaffService.ToggleBlockStatus()` | Decree 356/2025 | - |
| BR-STAFF-02 | Business Rule | Email không trùng lặp | `UserRepository.FindByEmail()` | N/A | - |

## 3. Architecture Decision Records (ADR)

### ADR-001 — Quản lý phân quyền bằng Bảng Roles
| Field | Value |
| --- | --- |
| Status | Accepted |
| Deciders | Tech Lead |
| Date | 2026-06-18 |

**Quyết định (Decision)**
Lưu trữ thông tin phân quyền theo mô hình Role-based Access Control (RBAC). Các quyền nghiệp vụ (Admin, Receptionist, TourGuide, FnB...) được lưu tĩnh trong bảng `Roles`. Khi Admin tạo tài khoản nhân viên mới, bắt buộc phải mapping `RoleId` vào `Users`. Khách hàng tự đăng ký mặc định là `Customer`.

## 4. Non-Functional Requirements & SLA
| Category | Requirement | Target SLA | Measurement Method | Compliance Basis |
| --- | --- | --- | --- | --- |
| Security | Hash Password | BCrypt (WorkFactor=11) | Code Review | Security Standard |
| Privacy | Ẩn Hash | Không trả PasswordHash ra API | Postman / Swagger test | Decree 356/2025 |

## 5. Static Modeling (Mô hình Tĩnh)
### 5.1. Data Structure (C# Models)
```csharp
public class User
{
    public string UserId { get; set; } // UUID
    public int RoleId { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation property
    public Role Role { get; set; }
    public UserProfile Profile { get; set; }
}

public class UserProfile
{
    public int ProfileId { get; set; }
    public string UserId { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
}
```

## 8. Interface Specification (Đặc tả Giao diện)
### 8.1. Service Interface
```csharp
public interface IStaffService
{
    Task<UserDto> CreateStaffAccountAsync(CreateStaffRequest request);
    Task<bool> ToggleAccountStatusAsync(string userId, bool isBlocked);
    Task<IEnumerable<UserDto>> SearchAccountsAsync(string keyword);
}
```

## 9. API Specification
### 9.1. Endpoints Table
| Method | Path | Auth Level | Required Roles | Rate Limit | Idempotent? |
| --- | --- | --- | --- | --- | --- |
| GET | `/api/v1/admin/staff` | Bearer | `Admin` | 60/min | Yes |
| POST | `/api/v1/admin/staff` | Bearer | `Admin` | 10/min | No |
| PATCH | `/api/v1/admin/staff/{id}/toggle-status`| Bearer | `Admin` | 30/min | Yes |

### 9.2. Request / Response Schemas
**POST `/api/v1/admin/staff`**
```json
// Request
{
  "email": "reception1@resort.com",
  "password": "SecurePassword123",
  "fullName": "Nguyen Van A",
  "roleId": 3
}

// Response (201 Created)
{
  "userId": "uuid-xxx-yyy",
  "email": "reception1@resort.com",
  "role": "Receptionist",
  "isBlocked": false
}
```

## 10. Bảng mã lỗi (Error Codes)
| Code | HTTP Status | Message (EN) | Message (VI) |
| --- | --- | --- | --- |
| `STF-001` | 400 | Email already exists | Email đã tồn tại trong hệ thống |
| `STF-002` | 404 | Role not found | Không tìm thấy Role hợp lệ |
| `STF-003` | 404 | User not found | Không tìm thấy tài khoản |
| `STF-004` | 403 | Cannot block super admin | Không thể khóa tài khoản Super Admin |

## 16. Bảng tổng hợp phân quyền (Authorization Matrix)
| Endpoint | CUSTOMER | RECEPTIONIST | ADMIN |
| --- | --- | --- | --- |
| `GET /api/v1/admin/staff` | ❌ | ❌ | ✅ |
| `POST /api/v1/admin/staff` | ❌ | ❌ | ✅ |
| `PATCH /api/v1/admin/staff/{id}/toggle-status` | ❌ | ❌ | ✅ |
