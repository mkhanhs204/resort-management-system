# ENGINEERING DOCUMENTATION STANDARD (EDS) v2.0
Quy chuẩn Tài liệu Kỹ thuật và Đặc tả Hiện thực hóa

| Field          | Value                                                      |
| ----------------| ------------------------------------------------------------|
| Document ID    | `PHUCNGUYEN-TOUR-IMP-001`                                  |
| Version        | 1.0                                                        |
| Date           | 2026-06-11                                                 |
| Status         | Draft                                                      |
| Document Owner | `Student 4`                                                |
| Author         | `Student 4 - Backend Developer`                            |
| Reviewed by    | `[Tech Lead]`                                              |
| DPO Sign-off   | N/A *(Module không xử lý PII)*                             |
| Approved by    | `[Principal Architect]`                                    |
| Last Review    | 2026-06-11                                                 |
| Based on EDS   | v2.0                                                       |

## CHANGELOG
**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ. Mọi thay đổi phải ghi vào bảng này.

| Ngày       | Người thực hiện                | Nội dung thay đổi                                 |
| ----------  | --------------------------------| ----------------------------------------------------|
| 2026-06-11 | `Student 4 - Backend Developer` | Tạo tài liệu lần đầu cho Manage Tour (UC04 subset) |

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
Module **Manage Tour** thuộc phần Master Data Management (Module 1 — UC04 subset), cho phép Admin/Business Manager quản lý danh mục Tour du lịch của Resort. Đây là chức năng CRUD cơ bản cho dữ liệu Tour — cung cấp nền tảng cho Module 4 (Tour Booking & Feedback).

| Field                  | Value                                                        |
| ------------------------| --------------------------------------------------------------|
| Module Name            | `Tour Catalog Management`                                    |
| Bounded Context        | `Master Data — Tourism`                                      |
| Data Classification    | Internal                                                     |
| Compliance Scope       | N/A                                                          |
| Upstream Dependencies  | `None`                                                       |
| Downstream Consumers   | `Module 4 (Tour Booking — UC15, UC16), Module 5 (Dashboard)` |

---

## 2. Ma trận Truy vết (Traceability Matrix)

| Requirement ID | Loại (BR/ADR/US) | Mô tả yêu cầu                                                                                               | Thành phần Code                                                  | Compliance Target | ADR liên quan |
| ----------------| -------------------| ---------------------------------------------------------------------------------------------------------------| ------------------------------------------------------------------| -------------------| ---------------|
| US-TOUR-001    | User Story        | UC04 (SWP391-HOS-02): As an Admin, I want to manage Tour Catalogs (CRUD operations with image uploads).     | `ToursController`, `TourService`                                 | —                 | ADR-001       |
| US-TOUR-002    | User Story        | UC11 (SWP391_NHOM1): As a guest, I want to browse available tours so that I can plan activities.             | `ToursController.Index()`, `ToursController.Details()`           | —                 | —             |
| US-TOUR-003    | User Story        | UC12 (SWP391_NHOM1): As a guest, I want to view detailed tour information so that I can decide to book.      | `ToursController.Details()`                                      | —                 | —             |
| BR-TOUR-001   | Business Rule     | TourName là bắt buộc, tối đa 100 ký tự.                                                                      | `TourCreateVM.TourName`, `TourEditVM.TourName`                   | —                 | —             |
| BR-TOUR-002   | Business Rule     | Price phải là số dương, tối đa 999,999,999.                                                                   | `TourCreateVM.Price`, `TourEditVM.Price`                         | —                 | —             |
| BR-TOUR-003   | Business Rule     | MaxCapacity phải từ 1 đến 1000.                                                                               | `TourCreateVM.MaxCapacity`, `TourEditVM.MaxCapacity`             | —                 | —             |
| BR-TOUR-004   | Business Rule     | DurationHours phải từ 1 đến 1000 giờ.                                                                        | `TourCreateVM.DurationHours`, `TourEditVM.DurationHours`         | —                 | —             |
| BR-TOUR-005   | Business Rule     | Không được xóa Tour nếu có TourSchedule hoặc TourBooking đang hoạt động.                                     | `ToursController.DeleteConfirmed()` *(cần implement cascade check)* | —                 | ADR-002       |

---

## 3. Architecture Decision Records (ADR)

### ADR-001 — Sử dụng MVC Server-Side Rendering thay vì SPA + API
| Field      | Value                           |
| ------------| ---------------------------------|
| Status     | Accepted                        |
| Deciders   | `Team + Principal Architect`    |
| Date       | 2026-06-04                      |
| Supersedes | `N/A`                           |

**Bối cảnh (Context)**
Dự án SWP391 yêu cầu xây dựng hệ thống quản lý Resort tích hợp. Team cần quyết định giữa SPA (React/Vue) + REST API hoặc ASP.NET Core MVC server-side rendering.

**Các phương án đã xem xét (Options Considered)**
| Phương án     | Mô tả                              | Ưu điểm                                         | Nhược điểm                                   |
| ---------------| -------------------------------------| --------------------------------------------------| -----------------------------------------------|
| SPA + REST API | Frontend React/Vue, Backend Web API | + UX mượt, + Dễ scale frontend riêng              | - Phức tạp hơn, cần 2 project riêng biệt     |
| MVC SSR        | ASP.NET Core MVC + Razor Views     | + Đơn giản, 1 project, + Phù hợp team 5 SV      | - Reload toàn trang, khó scale frontend riêng |

**Quyết định (Decision)**
Chọn **MVC Server-Side Rendering** vì phù hợp với quy mô team (5 sinh viên), thời gian phát triển ngắn, và yêu cầu dự án học thuật.

**Hệ quả (Consequences)**
Tích cực:
- Đơn giản hóa deploy và development workflow.
- Dễ debug và trace lỗi cho sinh viên.

Tiêu cực / Trade-offs:
- Form submission reload toàn trang — giảm thiểu bằng AJAX cho một số chức năng.

---

### ADR-002 — Xử lý Delete Tour bằng Hard Delete thay vì Soft Delete
| Field      | Value                           |
| ------------| ---------------------------------|
| Status     | Accepted                        |
| Deciders   | `Team`                          |
| Date       | 2026-06-11                      |
| Supersedes | `N/A`                           |

**Bối cảnh (Context)**
Khi Admin xóa Tour, cần quyết định giữa hard delete (xóa vĩnh viễn) hoặc soft delete (đánh dấu IsDeleted). Tour có quan hệ 1-N với TourSchedule và TourBooking.

**Các phương án đã xem xét (Options Considered)**
| Phương án   | Mô tả                                  | Ưu điểm                            | Nhược điểm                                   |
| -------------| -----------------------------------------| -------------------------------------| -----------------------------------------------|
| Hard Delete | `_context.Tours.Remove(tour)`           | + Đơn giản, dữ liệu sạch           | - Mất dữ liệu lịch sử, FK constraint error  |
| Soft Delete | `tour.IsDeleted = true`                 | + Giữ dữ liệu lịch sử              | - Phải filter mọi query                      |

**Quyết định (Decision)**
Chọn **Hard Delete** cho giai đoạn hiện tại vì đơn giản. Cần kiểm tra FK constraint trước khi xóa (nếu Tour có TourSchedule/TourBooking → từ chối xóa).

**Hệ quả (Consequences)**
Tích cực:
- Code đơn giản, không cần thêm filter cho mọi query.

Tiêu cực / Trade-offs:
- Phải implement kiểm tra FK trước khi xóa để tránh exception.

---

## 4. Non-Functional Requirements & SLA

### 4.1. Performance & Availability
| Category     | Requirement                    | Target SLA | Measurement Method | Compliance Basis |
| --------------| --------------------------------| ------------| --------------------| ------------------|
| Latency      | Page load (Tour List/Details)  | < 500ms    | Manual test        | —                |
| Availability | Uptime (monthly)               | 99%        | Uptime monitor     | —                |

### 4.2. Data Integrity & Retention
| Category    | Requirement                        | Target         | Verification Method | Compliance Basis |
| -------------| ------------------------------------| ----------------| ---------------------| ------------------|
| Durability  | Zero record loss cho Tours         | RPO = 0        | Transaction log     | —                |
| Consistency | Tour ↔ TourSchedule FK integrity  | 100%           | DB constraints      | —                |
| Validation  | Server-side validation cho tất cả  | 100% coverage  | Unit tests          | —                |

### 4.3. Security
| Category       | Requirement                  | Target          | Verification Method | Compliance Basis |
| ----------------| ------------------------------| -----------------| ---------------------| ------------------|
| CSRF           | AntiForgeryToken trên mọi POST | 100% coverage | Code review         | OWASP            |
| Authorization  | Chỉ Admin/Manager truy cập   | Role-based      | Auth Matrix (§16)   | —                |
| Input Sanitize | XSS prevention               | 100%            | Integration test    | OWASP            |

---

## 5. Static Modeling (Mô hình Tĩnh)

### 5.1. Class Diagram
```
┌──────────────────────┐      ┌──────────────────────────┐
│    ToursController   │──────▶    ITourService           │
│    (Controller)      │      │    (Interface)            │
├──────────────────────┤      ├──────────────────────────┤
│ + Index()            │      │ + GetAllToursAsync()     │
│ + Details(id)        │      │ + GetTourByIdAsync(id)   │
│ + Create()           │      │ + AddTourAsync(tour)     │
│ + Create(model)      │      │ + UpdateTourAsync(tour)  │
│ + Edit(id)           │      │ + DeleteTourAsync(id)    │
│ + Edit(id, model)    │      │ + TourExists(id)        │
│ + Delete(id)         │      └──────────────────────────┘
│ + DeleteConfirmed(id)│               ▲
└──────────────────────┘               │ implements
                                       │
                              ┌──────────────────────────┐
                              │    TourService            │
                              │    (Service)              │
                              ├──────────────────────────┤
                              │ - _context: ResortDbContext│
                              └──────────────────────────┘
```

### 5.2. Data Structure (EF Core Models)

**Tour Entity** — `Models/Tour.cs`
```csharp
public class Tour
{
    public int TourId { get; set; }                              // PK, auto-increment

    [StringLength(100)]
    public string? TourName { get; set; }                        // Tên tour

    public string? Description { get; set; }                     // Mô tả chi tiết

    [Range(0, 999999999)]
    public decimal? Price { get; set; }                          // Giá tour (VND)

    [Range(1, 1000)]
    public int? MaxCapacity { get; set; }                        // Số người tối đa

    [Range(1, 1000)]
    public int? DurationHours { get; set; }                      // Thời lượng (giờ)

    [StringLength(255)]
    public string? Location { get; set; }                        // Địa điểm

    [StringLength(255)]
    public string? ImageUrl { get; set; }                        // URL hình ảnh

    public ICollection<TourSchedule> TourSchedules { get; set; } // 1-N: Lịch trình
    public ICollection<Review> Reviews { get; set; }             // 1-N: Đánh giá
}
```

**TourCreateVM** — `ViewModels/Tours/TourCreateVM.cs`
```csharp
public class TourCreateVM
{
    [Required] [StringLength(100)]
    public string TourName { get; set; }         // Bắt buộc

    public string? Description { get; set; }     // Tùy chọn

    [Required] [Range(0, 999999999)]
    public decimal Price { get; set; }           // Bắt buộc

    [Required] [Range(1, 1000)]
    public int MaxCapacity { get; set; }         // Bắt buộc

    [Required] [Range(1, 1000)]
    public int DurationHours { get; set; }       // Bắt buộc

    [StringLength(255)]
    public string? Location { get; set; }        // Tùy chọn

    [StringLength(255)]
    public string? ImageUrl { get; set; }        // Tùy chọn
}
```

**TourEditVM** — `ViewModels/Tours/TourEditVM.cs`
```csharp
public class TourEditVM
{
    public int TourId { get; set; }              // Hidden field (không cho sửa)
    // Các field còn lại giống TourCreateVM
}
```

### 5.3. Entity Relationship

```
Tours (1) ──── (N) TourSchedules (1) ──── (N) TourBookings
  │                     │
  │                     └── GuideId → Users(UserId)
  │
  └── (N) Reviews
```

---

## 6. Dynamic Modeling (Mô hình Động)

### 6.1. Sequence Diagram — Create Tour (Happy Path)

```
Admin Browser          ToursController              TourService            ResortDbContext
     │                        │                          │                        │
     │── GET /Tours/Create ──▶│                          │                        │
     │◀── View(form trống) ──│                          │                        │
     │                        │                          │                        │
     │── POST /Tours/Create ─▶│                          │                        │
     │   (TourCreateVM)       │── validate ModelState ──▶│                        │
     │                        │                          │                        │
     │                        │   [ModelState.IsValid]    │                        │
     │                        │── Map VM → Tour Entity ──▶│                        │
     │                        │── AddTourAsync(tour) ────▶│                        │
     │                        │                          │── Tours.Add(tour) ────▶│
     │                        │                          │── SaveChangesAsync() ─▶│
     │                        │                          │◀── Tour (with ID) ────│
     │                        │◀── return tour ──────────│                        │
     │◀── Redirect /Tours ───│                          │                        │
```

### 6.2. Sequence Diagram — Create Tour (Error Path — Validation Failed)

```
Admin Browser          ToursController              
     │                        │                     
     │── POST /Tours/Create ─▶│                     
     │   (TourCreateVM)       │── validate ModelState
     │                        │                     
     │                        │   [ModelState.IsInvalid]
     │◀── View(model + errors)│                     
     │   (hiển thị lỗi)       │                     
```

### 6.3. Sequence Diagram — Delete Tour (Happy Path)

```
Admin Browser          ToursController              TourService            ResortDbContext
     │                        │                          │                        │
     │── GET /Tours/Delete/5 ▶│                          │                        │
     │                        │── GetTourByIdAsync(5) ──▶│                        │
     │                        │                          │── FindAsync(5) ───────▶│
     │                        │◀── tour ────────────────│                        │
     │◀── View(confirm page) ─│                          │                        │
     │                        │                          │                        │
     │── POST /Tours/Delete/5 ▶│                          │                        │
     │                        │── GetTourByIdAsync(5) ──▶│                        │
     │                        │── DeleteTourAsync(5) ───▶│                        │
     │                        │                          │── Tours.Remove(tour) ─▶│
     │                        │                          │── SaveChangesAsync() ─▶│
     │◀── Redirect /Tours ───│                          │                        │
```

---

## 7. Domain Event Catalog

### 7.1. Events Published (Phát ra)
| Event Name       | Trigger                           | Publisher      | Subscriber(s)            | Async? |
| ------------------| -----------------------------------| ----------------| --------------------------| --------|
| `TourCreated`    | Admin tạo tour mới thành công     | `TourService`  | *(Future: SearchIndex)*  | No     |
| `TourUpdated`    | Admin cập nhật thông tin tour      | `TourService`  | *(Future: SearchIndex)*  | No     |
| `TourDeleted`    | Admin xóa tour                    | `TourService`  | *(Future: SearchIndex)*  | No     |

> **Lưu ý**: Giai đoạn hiện tại chưa implement event system. Các events được ghi nhận ở đây cho mục đích thiết kế tương lai.

---

## 8. Interface Specification (Đặc tả Giao diện)

### 8.1. Service Interface
```csharp
// ITourService.cs
// @version 1.0

namespace PhucNguyenResort.Services.Tours
{
    public interface ITourService
    {
        /// <summary>
        /// Lấy tất cả tour trong hệ thống.
        /// </summary>
        Task<IEnumerable<Tour>> GetAllToursAsync();

        /// <summary>
        /// Lấy tour theo ID.
        /// </summary>
        /// <returns>Tour nếu tồn tại, null nếu không.</returns>
        Task<Tour?> GetTourByIdAsync(int tourId);

        /// <summary>
        /// Thêm tour mới.
        /// </summary>
        /// <returns>Tour đã lưu với ID được gán.</returns>
        Task<Tour> AddTourAsync(Tour tour);

        /// <summary>
        /// Cập nhật thông tin tour.
        /// </summary>
        Task UpdateTourAsync(Tour tour);

        /// <summary>
        /// Xóa tour theo ID.
        /// @throws DbUpdateException nếu tour có FK dependency.
        /// </summary>
        Task DeleteTourAsync(int tourId);

        /// <summary>
        /// Kiểm tra tour có tồn tại.
        /// </summary>
        bool TourExists(int tourId);
    }
}
```

---

## 9. API Specification (MVC Routes)

### 9.1. Endpoints Table
| Method | Route                      | Auth Level    | Required Roles        | Idempotent? | Description              |
| --------| ----------------------------| ---------------| -----------------------| -------------| --------------------------|
| GET    | `/Tours`                   | Authenticated | `Admin, Manager`      | Yes         | Danh sách tất cả tours   |
| GET    | `/Tours/Details/{id}`      | Authenticated | `Admin, Manager`      | Yes         | Chi tiết tour theo ID    |
| GET    | `/Tours/Create`            | Authenticated | `Admin, Manager`      | Yes         | Form tạo tour mới        |
| POST   | `/Tours/Create`            | Authenticated | `Admin, Manager`      | No          | Submit tạo tour mới      |
| GET    | `/Tours/Edit/{id}`         | Authenticated | `Admin, Manager`      | Yes         | Form sửa tour            |
| POST   | `/Tours/Edit/{id}`         | Authenticated | `Admin, Manager`      | No          | Submit sửa tour          |
| GET    | `/Tours/Delete/{id}`       | Authenticated | `Admin, Manager`      | Yes         | Trang xác nhận xóa       |
| POST   | `/Tours/Delete/{id}`       | Authenticated | `Admin, Manager`      | No          | Xác nhận xóa tour        |

### 9.2. Form Fields & Validation

**POST `/Tours/Create`** — Tạo Tour mới

| Field         | Type     | Required | Validation Rule                      | Error Message                              |
| ---------------| ----------| ----------| --------------------------------------| --------------------------------------------|
| TourName      | string   | ✅       | MaxLength(100)                       | "Tour Name is required" / "cannot exceed 100 characters" |
| Description   | string   | ❌       | None                                 | —                                          |
| Price         | decimal  | ✅       | Range(0, 999999999)                  | "Price must be a positive number"          |
| MaxCapacity   | int      | ✅       | Range(1, 1000)                       | "Max Capacity must be between 1 and 1000"  |
| DurationHours | int      | ✅       | Range(1, 1000)                       | "Duration must be between 1 and 1000 hours"|
| Location      | string   | ❌       | MaxLength(255)                       | "Location cannot exceed 255 characters"    |
| ImageUrl      | string   | ❌       | MaxLength(255)                       | —                                          |

---

## 10. Bảng mã lỗi (Error Codes)

| Code       | HTTP Status | Message (EN)                  | Message (VI)                              | Trigger Condition                              |
| ------------| -------------| -------------------------------| -------------------------------------------| ------------------------------------------------|
| `TOUR-001` | 400         | Validation failed             | Dữ liệu không hợp lệ                    | Thiếu required field hoặc giá trị ngoài range |
| `TOUR-002` | 404         | Tour not found                | Không tìm thấy tour                      | ID không tồn tại trong DB                     |
| `TOUR-003` | 409         | Tour has active dependencies  | Tour có lịch trình/booking đang hoạt động | Xóa tour có TourSchedule/TourBooking          |
| `TOUR-004` | 403         | Insufficient permissions      | Không đủ quyền truy cập                  | User không có role Admin/Manager               |
| `TOUR-005` | 500         | Internal server error         | Lỗi hệ thống                             | Exception không xử lý được                     |

---

## 11. Quy trình Triển khai (Step-by-Step)

### 11.1. Prerequisites
- [x] Database schema cho bảng `Tours` đã có (migration `InitialCreate`)
- [x] Seed data cho Tours đã tồn tại trong `SeedData.cs`
- [ ] Authentication/Authorization phải được implement trước (Module 1)

### 11.2. Implementation Steps

**Chặng 1 — Model & ViewModel** ✅ Đã hoàn thành
- `Models/Tour.cs` — Entity với Data Annotations
- `ViewModels/Tours/TourCreateVM.cs` — ViewModel cho Create
- `ViewModels/Tours/TourEditVM.cs` — ViewModel cho Edit

**Chặng 2 — Service Layer** ✅ Đã hoàn thành
- `Services/Tours/ITourService.cs` — Interface
- `Services/Tours/TourService.cs` — Implementation (CRUD via EF Core)
- DI Registration trong `Program.cs`

**Chặng 3 — Controller** ✅ Đã hoàn thành
- `Controllers/ToursController.cs` — 7 actions (Index, Details, Create GET/POST, Edit GET/POST, Delete GET/POST)
- `[ValidateAntiForgeryToken]` trên mọi POST action

**Chặng 4 — Views** ✅ Đã hoàn thành
- `Views/Tours/Index.cshtml` — Danh sách tours
- `Views/Tours/Details.cshtml` — Chi tiết tour
- `Views/Tours/Create.cshtml` — Form tạo mới
- `Views/Tours/Edit.cshtml` — Form chỉnh sửa
- `Views/Tours/Delete.cshtml` — Xác nhận xóa

**Chặng 5 — Authorization** ❌ Chưa triển khai
- Thêm `[Authorize(Roles = "Admin,Manager")]` trên `ToursController`
- Cần Module 1 (Auth) hoàn thành trước

**Chặng 6 — Delete Constraint Check** ❌ Chưa triển khai
- Kiểm tra `TourSchedules.Count > 0` trước khi xóa
- Trả về thông báo lỗi nếu có dependency

### 11.3. Deployment Checklist
- [x] Migration chạy thành công
- [x] Seed data đầy đủ
- [ ] Authorization middleware active
- [ ] Error handling cho FK constraint violation
- [ ] Smoke test CRUD flow

---

## 12. Rollback & Incident Runbook

### 12.1. Điều kiện kích hoạt Rollback (Trigger Conditions)
| Điều kiện                                    | Ngưỡng            | Người quyết định |
| ----------------------------------------------| --------------------| ------------------|
| CRUD operation gây data corruption           | Bất kỳ case nào   | Tech Lead        |
| Migration thất bại                            | Ngay lập tức       | Developer        |
| FK constraint error khi xóa Tour             | > 3 lần trong 1h  | Developer        |

### 12.2. Rollback Procedure
```bash
# Bước 1: Revert migration nếu cần (development only)
dotnet ef database update [PreviousMigration] --project PhucNguyenResort

# Bước 2: Revert code
git checkout -- Controllers/ToursController.cs
git checkout -- Services/Tours/
git checkout -- ViewModels/Tours/
git checkout -- Views/Tours/

# Bước 3: Verify rollback
dotnet build
dotnet run
```

---

## 13. Kịch bản Kiểm thử Chi tiết
*(Vui lòng xem chi tiết tại tài liệu `ManageTour_TDD.md`)*

---

## 14. Phương pháp Xác minh

### 14.1. Database Inspection
```sql
-- Verify danh sách tours
SELECT TourId, TourName, Price, MaxCapacity, DurationHours, Location
FROM Tours
ORDER BY TourId;

-- Verify tour vừa tạo
SELECT * FROM Tours WHERE TourName = 'New Tour Name';

-- Verify tour đã xóa
SELECT COUNT(*) FROM Tours WHERE TourId = [deleted_id];
-- Expected: 0

-- Verify FK integrity
SELECT t.TourId, COUNT(ts.ScheduleId) AS ScheduleCount
FROM Tours t
LEFT JOIN TourSchedules ts ON t.TourId = ts.TourId
GROUP BY t.TourId;
```

### 14.2. UI Verification
```
1. Navigate to /Tours → Verify danh sách hiển thị đúng.
2. Click "Create New" → Verify form hiển thị đầy đủ fields.
3. Submit form với dữ liệu hợp lệ → Verify redirect về /Tours và tour mới xuất hiện.
4. Submit form thiếu required field → Verify validation errors hiển thị.
5. Click "Edit" → Verify form điền sẵn dữ liệu hiện tại.
6. Click "Delete" → Verify trang xác nhận hiển thị → Confirm → Verify tour bị xóa.
```

---

## 15. Mẫu thử thực tế (Manual Verification Samples)

### 15.1. Happy Path — Tạo Tour mới
```
1. GET /Tours/Create
   → Status: 200 OK, Form hiển thị trống

2. POST /Tours/Create
   Form data:
     TourName     = "Hạ Long Bay Cruise"
     Description  = "Luxury cruise with seafood dinner"
     Price        = 2500000
     MaxCapacity  = 30
     DurationHours = 8
     Location     = "Hạ Long Bay, Quảng Ninh"
     ImageUrl     = "/images/tours/halong.jpg"
   → Status: 302 Redirect to /Tours
   → DB: Tours chứa record mới
```

### 15.2. Error Path — Validation Error
```
POST /Tours/Create
Form data:
  TourName = ""          ← thiếu
  Price    = -100        ← ngoài range
  MaxCapacity = 0        ← ngoài range
→ Status: 200 (re-render form)
→ Page hiển thị:
  - "Tour Name is required"
  - "Price must be a positive number"
  - "Max Capacity must be between 1 and 1000"
```

---

## 16. Bảng tổng hợp phân quyền (Authorization Matrix)

| Route                    | GUEST | CUSTOMER | ADMIN | MANAGER | RECEPTIONIST | F&B STAFF |
| --------------------------| -------| ----------| -------| ---------| --------------| -----------|
| GET `/Tours`             | ❌    | ❌       | ✅    | ✅      | ❌           | ❌        |
| GET `/Tours/Details/{id}`| ❌    | ❌       | ✅    | ✅      | ❌           | ❌        |
| GET `/Tours/Create`      | ❌    | ❌       | ✅    | ✅      | ❌           | ❌        |
| POST `/Tours/Create`     | ❌    | ❌       | ✅    | ✅      | ❌           | ❌        |
| GET `/Tours/Edit/{id}`   | ❌    | ❌       | ✅    | ✅      | ❌           | ❌        |
| POST `/Tours/Edit/{id}`  | ❌    | ❌       | ✅    | ✅      | ❌           | ❌        |
| GET `/Tours/Delete/{id}` | ❌    | ❌       | ✅    | ❌      | ❌           | ❌        |
| POST `/Tours/Delete/{id}`| ❌    | ❌       | ✅    | ❌      | ❌           | ❌        |

Chú thích:
- ✅ = Được phép
- ❌ = Bị từ chối (403 hoặc redirect login)
- Delete chỉ Admin có quyền (Manager không được xóa dữ liệu master)

---

## PHỤ LỤC

### A. Glossary (Thuật ngữ)
| Thuật ngữ      | Định nghĩa                                                                |
| ----------------| ----------------------------------------------------------------------------|
| Tour           | Gói hoạt động du lịch / giải trí do Resort tổ chức                        |
| TourSchedule   | Lịch cụ thể cho một Tour (ngày, giờ bắt đầu, số slot, hướng dẫn viên)   |
| TourBooking    | Bản ghi đặt tour của khách hàng, liên kết với Booking (Room Reservation) |
| ViewModel (VM) | Object dùng để truyền dữ liệu giữa Controller và View, không lưu vào DB |
| CSRF           | Cross-Site Request Forgery — tấn công giả mạo yêu cầu                   |

### B. Tài liệu tham chiếu
| Document                            | Path                                          |
| --------------------------------------| ------------------------------------------------|
| SRS — Project Specification          | `01_SRS/SWP391-HOS-02.md`                     |
| SRS — Requirement & Design          | `01_SRS/SWP391_NHOM1.md`                      |
| TDD Spec — Manage Tour              | `03_implement/ManageTour_TDD.md`               |
| Tour Entity Model                    | `PhucNguyenResort/Models/Tour.cs`              |
| Tour Service Interface              | `PhucNguyenResort/Services/Tours/ITourService.cs` |
| Tour Controller                      | `PhucNguyenResort/Controllers/ToursController.cs` |
