# ENGINEERING DOCUMENTATION STANDARD (EDS) v2.0
Quy chuẩn Tài liệu Kỹ thuật và Đặc tả Hiện thực hóa

| Field          | Value                                                 |
| ----------------| -------------------------------------------------------|
| Document ID    | `PHUCNGUYEN-IAM-IMP-001`                              |
| Version        | 1.0                                                   |
| Date           | 2026-06-08                                            |
| Status         | Draft                                                 |
| Document Owner | `Student 1`                                           |
| Author         | `Student 1 - Backend Developer`                       |
| Reviewed by    | `[Tech Lead]`                                         |
| DPO Sign-off   | [ ] Pending - `[Tên DPO]` *(bắt buộc với module PII)* |
| Approved by    | `[Principal Architect]`                               |
| Last Review    | 2026-06-08                                            |
| Based on EDS   | v2.0                                                  |

## CHANGELOG
**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ. Mọi thay đổi phải ghi vào bảng này.

| Ngày | Người thực hiện | Nội dung thay đổi |
| --- | --- | --- |
| 2026-06-08 | `Student 1 - Backend Developer` | Tạo tài liệu lần đầu cho UC01 |

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
13. Kịch bản Kiểm thử Chi tiết (tham chiếu TDD Spec)
14. Phương pháp Xác minh
15. Mẫu thử thực tế (API Verification Samples)
16. Bảng tổng hợp phân quyền (Authorization Matrix)

---

## 1. Tổng quan Module
Mô tả ngắn gọn mục đích của module, phạm vi nghiệp vụ và lý do tồn tại. UC01 tập trung vào chức năng Guest register, verify email và login an toàn.

| Field | Value |
| --- | --- |
| Module Name | `Auth, Profiles & Master Data (IAM)` |
| Bounded Context | `Identity & Access Management` |
| Data Classification | Sensitive-PII |
| Compliance Scope | Decree 356/2025 |
| Upstream Dependencies | `None` |
| Downstream Consumers | `Module 2 (Room Reservation), Module 3 (Restaurant POS), Module 4 (Tour Booking)` |

## 2. Ma trận Truy vết (Traceability Matrix)

| Requirement ID | Loại (BR/ADR/US) | Mô tả yêu cầu | Thành phần Code | Compliance Target | ADR liên quan |
| --- | --- | --- | --- | --- | --- |
| US-IAM-001 | User Story | UC01: As a Guest, I want to register, verify my email, and securely log in. | `AuthController.register`, `AuthController.login` | Decree 356/2025 Art. 11 (Consent) | ADR-001 |
| BR-IAM-001 | Business Rule | Email đăng ký phải là duy nhất. | `AuthService.register()` | | |
| BR-IAM-002 | Business Rule | Guest không thể login nếu chưa verify email. | `AuthService.login()` | | |

## 3. Architecture Decision Records (ADR)

### ADR-001 — Sử dụng JWT cho Authentication thay vì Session-based
| Field | Value |
| --- | --- |
| Status | Proposed |
| Deciders | `Team + Principal Architect` |
| Date | 2026-06-08 |
| Supersedes | `N/A` |

**Bối cảnh (Context)**
Hệ thống được chia làm nhiều module (phục vụ frontend và có thể mobile app tương lai). Cần một phương thức xác thực phi trạng thái (stateless) để dễ scale và không phụ thuộc vào bộ nhớ server.

**Các phương án đã xem xét (Options Considered)**
| Phương án | Mô tả | Ưu điểm | Nhược điểm |
| --- | --- | --- | --- |
| Session/Cookie | Lưu session trên Redis/Database | + Dễ revoke quyền | - Yêu cầu Redis/DB lookup mỗi request, stateful |
| JWT (JSON Web Token) | Client lưu token và gửi kèm Header | + Stateless, dễ scale, tích hợp tốt với microservices | - Khó revoke token trước khi hết hạn (cần blacklist) |

**Quyết định (Decision)**
Chọn Phương án **JWT** vì hệ thống yêu cầu tính linh hoạt, các module backend độc lập có thể tự verify token thông qua Secret Key mà không cần gọi DB.

**Hệ quả (Consequences)**
Tích cực:
- Dễ dàng xác thực tại mọi module mà không bị bottleneck ở DB.
Tiêu cực / Trade-offs:
- Phải implement Token Blacklist (hoặc chấp nhận thời gian sống ngắn của Access Token) nếu user bị khóa account lập tức.

## 4. Non-Functional Requirements & SLA

### 4.1. Performance & Availability
| Category | Requirement | Target SLA | Measurement Method | Compliance Basis |
| --- | --- | --- | --- | --- |
| Latency | API response (p99) cho Login/Register | < 300ms | k6 load test | — |
| Availability | Uptime (monthly) | 99.9% | Uptime monitor | — |

### 4.2. Data Integrity & Retention
| Category | Requirement | Target | Verification Method | Compliance Basis |
| --- | --- | --- | --- | --- |
| Durability | Zero record loss for Accounts | RPO = 0 | Transaction log | — |
| Consistency | Account data sync | 100% | Constraints | — |

### 4.3. Security
| Category | Requirement | Target | Verification Method | Compliance Basis |
| --- | --- | --- | --- | --- |
| Encryption at rest | Passwords | Bcrypt/Argon2 | Code review | Decree 356/2025 |
| Encryption in transit | All endpoints | TLS 1.3+ | SSL Labs scan | Decree 356/2025 |
| Access control | Role-based | Least privilege | Auth Matrix (§16) | Decree 356/2025 |

## 5. Static Modeling (Mô hình Tĩnh)

### 5.2. Data Structure (Prisma Schema)
```prisma
// === IAM SCHEMA ===

model Guest {
  id                String   @id @default(uuid()) @db.Uuid
  email             String   @unique
  passwordHash      String
  isEmailVerified   Boolean  @default(false)
  verificationToken String?
  
  // --- Audit fields ---
  createdAt         DateTime @default(now())
  updatedAt         DateTime @updatedAt

  @@map("guests")
}
```

## 6. Dynamic Modeling (Mô hình Động)
Guest -> `POST /api/v1/auth/register` -> `AuthController`
`AuthController` -> Validate DTO -> `AuthService`
`AuthService` -> Check email unique -> `GuestRepository.save`
`AuthService` -> Generate `verificationToken` -> Send Email (Twilio/SendGrid)

## 7. Domain Event Catalog

### 7.1. Events Published (Phát ra)
| Event Name | Trigger | Publisher | Subscriber(s) | Payload Schema | Async? |
| --- | --- | --- | --- | --- | --- |
| `GuestRegistered` | Guest đăng ký thành công | `AuthService` | `EmailService` | `GuestRegistered.ts` | Yes |
| `GuestEmailVerified` | Guest click link verify | `AuthService` | `MarketingService` | `GuestEmailVerified.ts` | Yes |

### 7.3. Payload Schema
```typescript
// GuestRegistered.ts
export interface GuestRegistered {
  eventId: string;
  eventType: 'GuestRegistered';
  occurredAt: string;
  version: '1.0';
  payload: {
    guestId: string;
    email: string;
    verificationToken: string;
  };
  metadata: {
    correlationId: string;
  };
}
```

## 8. Interface Specification (Đặc tả Giao diện)

### 8.1. Service Interface
```typescript
// IAuthService.ts
// @version 1.0

export interface RegisterInput {
  email: string;
  password: string;
}

export interface LoginInput {
  email: string;
  password: string;
}

export interface AuthOutput {
  accessToken: string;
  guestId: string;
}

export interface IAuthService {
  register(input: RegisterInput): Promise<void>;
  verifyEmail(token: string): Promise<void>;
  login(input: LoginInput): Promise<AuthOutput>;
}
```

## 9. API Specification

### 9.1. Endpoints Table
| Method | Path | Auth Level | Required Roles | Rate Limit | Idempotent? |
| --- | --- | --- | --- | --- | --- |
| POST | `/api/v1/auth/register` | None | `None` | 10/min | No |
| POST | `/api/v1/auth/verify-email` | None | `None` | 30/min | Yes |
| POST | `/api/v1/auth/login` | None | `None` | 30/min | No |

### 9.2. Request / Response Schemas

**POST `/api/v1/auth/register`**
Request Body:
```json
{
  "email": "guest@example.com",
  "password": "SecurePassword123!"
}
```
Response — 201 Created:
```json
{
  "status": "SUCCESS",
  "message": "Registration successful. Please check your email to verify."
}
```

**POST `/api/v1/auth/login`**
Request Body:
```json
{
  "email": "guest@example.com",
  "password": "SecurePassword123!"
}
```
Response — 200 OK:
```json
{
  "accessToken": "eyJhbG...",
  "guestId": "uuid-v4",
  "expiresIn": 3600
}
```

## 10. Bảng mã lỗi (Error Codes)

| Code | HTTP Status | Message (EN) | Message (VI) | Trigger Condition |
| --- | --- | --- | --- | --- |
| `IAM-001` | 400 | Validation failed | Dữ liệu không hợp lệ | Thiếu email/password |
| `IAM-002` | 409 | Email already exists | Email đã tồn tại | Đăng ký trùng email |
| `IAM-003` | 401 | Invalid credentials | Sai thông tin đăng nhập | Sai email hoặc password |
| `IAM-004` | 403 | Email not verified | Email chưa xác thực | Login khi chưa verify |

## 11. Quy trình Triển khai (Step-by-Step)
*(Tham khảo Checklist trong template chuẩn, cần backup DB và chạy migration tạo bảng `guests`)*

## 12. Rollback & Incident Runbook
| Điều kiện | Ngưỡng | Người quyết định |
| --- | --- | --- |
| Error rate đăng nhập tăng | > 5% trong 5 phút | On-call Engineer |

## 13. Kịch bản Kiểm thử Chi tiết
*(Vui lòng xem chi tiết tại tài liệu `UC01_TDD.md`)*

## 14. Phương pháp Xác minh
```sql
-- Verify record tồn tại sau khi tạo
SELECT id, email, is_email_verified
FROM guests
WHERE email = 'guest@example.com';
```

## 16. Bảng tổng hợp phân quyền (Authorization Matrix)
| Endpoint | GUEST | USER (Verified) | ADMIN |
| --- | --- | --- | --- |
| POST `/api/v1/auth/register` | ✅ | ❌ | ❌ |
| POST `/api/v1/auth/login` | ✅ | ❌ | ❌ |
