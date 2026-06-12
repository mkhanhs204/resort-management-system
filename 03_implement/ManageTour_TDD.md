# TEST-DRIVEN DEVELOPMENT SPECIFICATION TEMPLATE
Mẫu Đặc tả Kiểm thử Hướng Phát triển

**Document ID**: PHUCNGUYEN-TOUR-TDD-001
**Version**: 1.0
**Date**: 2026-06-11
**Status**: Approved
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: `Nguyen Khac Hieu`
**Reviewed by**: [ ] `Nguyen Khac Hieu`
**Classification**: Internal - Confidential

**References**:
- `01_SRS/SWP391-HOS-02.md` — Functional requirements (UC04, UC15-UC19)
- `01_SRS/SWP391_NHOM1.md` — Requirement & Design Specification (UC11, UC12)
- `03_implement/ManageTour_EDS.md` — Technical Specification

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.

---

## 1. Thông tin Module
| Field               | Value                                                |
| ---------------------| ------------------------------------------------------|
| Feature / Gap ID    | UC04 (Manage Tour Catalogs)                          |
| Module              | `Tour Catalog Management (Master Data)`              |
| Spec gốc            | `SWP391-HOS-02`, `SWP391_NHOM1`                     |
| Priority            | 🟠 P1                                                |
| Data Classification | Internal                                             |
| Compliance Scope    | N/A                                                  |
| Upstream Dependencies| `None`                                              |
| Downstream Consumers| `Module 4 (Tour Booking), Module 5 (Dashboard)`     |

## 2. Logic Issues Resolved
Bắt buộc điền trước khi viết test.
Liệt kê mọi sai lệch giữa spec thiết kế và schema/policy/codebase thực tế.

| #   | Spec gốc (sai / thiếu)                                                                                   | Thực tế (schema / policy)                                                              | Fix áp dụng trong test                                                    |
| -----| -----------------------------------------------------------------------------------------------------------| -----------------------------------------------------------------------------------------| ---------------------------------------------------------------------------|
| L1  | UC04 yêu cầu "CRUD operations with image uploads" nhưng Tour hiện chỉ lưu `ImageUrl` text              | `Tour.ImageUrl` là `string?` — chỉ lưu URL, không upload file                          | Test verify lưu URL text, không test file upload cho Tour                 |
| L2  | UC04 không quy định giới hạn cụ thể cho Price, MaxCapacity, DurationHours                                | ViewModel dùng `[Range]` annotations: Price(0-999999999), MaxCapacity(1-1000), DurationHours(1-1000) | Test boundary values theo validation rules trong ViewModel                |
| L3  | Spec không đề cập việc xóa Tour khi có TourSchedule/TourBooking                                          | Hiện tại `DeleteTourAsync()` xóa trực tiếp, có thể gây FK constraint error             | Test phải verify behavior khi xóa tour có/không có dependencies           |
| L4  | Spec không nêu rõ các field nào Required, Optional                                                        | `TourCreateVM`: Required = TourName, Price, MaxCapacity, DurationHours; Optional = Description, Location, ImageUrl | Test cover cả Required validation và Optional null cases                 |

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi
```text
Tour Catalog Management bao gồm các layer:
├── Domain (Tour Entity, TourCreateVM, TourEditVM)
├── Service (TourService — CRUD via EF Core)
├── Controller (ToursController — MVC actions)
└── Integration (SQL Server + EF Core migrations)
```

### TDS-02 — Test Basis / Cơ sở Kiểm thử
| Source                           | Items Derived                                                          |
| ----------------------------------| ------------------------------------------------------------------------|
| `SWP391-HOS-02` UC04            | Admin CRUD Tour Catalogs with image uploads.                           |
| `SWP391_NHOM1` UC11             | Guest xem danh sách tours.                                             |
| `SWP391_NHOM1` UC12             | Guest xem chi tiết tour.                                               |
| BR-TOUR-001 → BR-TOUR-005      | Validation rules cho từng field (xem EDS §2).                          |
| ADR-002                          | Hard Delete + FK constraint check trước khi xóa.                      |

### TDS-03 — Test Conditions and Coverage Items

| Condition ID  | Test Condition                                          | Coverage Item                         | Test Cases                       |
| ---------------| ---------------------------------------------------------| ----------------------------------------| ----------------------------------|
| TC-COND-001  | Tạo tour mới thành công với đầy đủ required fields     | `ToursController.Create(POST)`        | `TOUR-TC-001`                    |
| TC-COND-002  | Tạo tour mới thiếu required field → validation error   | `ToursController.Create(POST)`        | `TOUR-TC-002`                    |
| TC-COND-003  | Tạo tour với giá trị biên (boundary values)             | `TourCreateVM` validation             | `TOUR-TC-003`                    |
| TC-COND-004  | Xem danh sách tours                                     | `ToursController.Index()`             | `TOUR-TC-004`                    |
| TC-COND-005  | Xem chi tiết tour hợp lệ                               | `ToursController.Details(id)`         | `TOUR-TC-005`                    |
| TC-COND-006  | Xem chi tiết tour không tồn tại → 404                  | `ToursController.Details(id)`         | `TOUR-TC-006`                    |
| TC-COND-007  | Sửa tour thành công                                     | `ToursController.Edit(POST)`          | `TOUR-TC-007`                    |
| TC-COND-008  | Sửa tour với ID không khớp → 404                        | `ToursController.Edit(POST)`          | `TOUR-TC-008`                    |
| TC-COND-009  | Xóa tour thành công (không có dependencies)             | `ToursController.DeleteConfirmed()`   | `TOUR-TC-009`                    |
| TC-COND-010  | Xóa tour có TourSchedule → nên bị chặn                 | `ToursController.DeleteConfirmed()`   | `TOUR-TC-010`                    |
| TC-COND-011  | XSS payload trong TourName                              | `ToursController.Create(POST)`        | `TOUR-TC-SEC-001`                |
| TC-COND-012  | CSRF protection                                         | `[ValidateAntiForgeryToken]`          | `TOUR-TC-SEC-002`                |

### TDS-04 — Test Techniques / Kỹ thuật Kiểm thử
| Technique (ISO 29119-4)   | Applied To                         | Rationale                                                |
| ---------------------------| ------------------------------------| ----------------------------------------------------------|
| Equivalence Partitioning  | Input fields (Price, MaxCapacity)  | Chia input thành valid/invalid partitions                |
| Boundary Value Analysis   | Range constraints                  | Test giá trị biên: 0, 1, 999999999, 1000, 1001          |
| State Transition Testing  | N/A cho Manage Tour               | Tour entity không có state machine (chỉ CRUD)            |
| Error Guessing            | XSS, SQL Injection, CSRF          | Tấn công phổ biến trên form input                        |

### TDS-05 — Test Data Requirements
| Fixture ID | Type      | Value / Logic                                                                                          | Mục đích                        |
| ------------| -----------| --------------------------------------------------------------------------------------------------------| ----------------------------------|
| FX-001     | DB seed   | `{ TourId: 1, TourName: "City Tour", Price: 500000, MaxCapacity: 20, DurationHours: 4, Location: "Hanoi" }` | Happy path — tour có sẵn  |
| FX-002     | DB seed   | `{ TourId: 2, TourName: "Beach Tour", ... }` + 1 TourSchedule liên kết                                 | Test delete với dependency       |
| FX-003     | Form data | `{ TourName: "New Tour", Price: 100000, MaxCapacity: 30, DurationHours: 8 }`                           | Create tour happy path           |
| FX-004     | Form data | `{ TourName: "", Price: -1, MaxCapacity: 0, DurationHours: 0 }`                                        | Create tour — all invalid        |
| FX-005     | Form data | `{ TourName: "<script>alert('XSS')</script>", Price: 100000, ... }`                                    | Security — XSS payload          |

---

## 4. Test Case Specification
**TC ID format**: `TOUR-TC-[NNN]`
**Severity**: CRITICAL / HIGH / MEDIUM / LOW
**Status**: 🔴 Not written / 🟡 Written-failing / 🟢 Passing

---

### TOUR-TC-001 — Tạo tour mới thành công
**Severity**: CRITICAL
**Feature Under Test**: `ToursController.Create(TourCreateVM)` → `TourService.AddTourAsync()`
**Test File**: `Tests/Tours/CreateTourTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-001

**Preconditions**:
- Database đang chạy với schema đã migrate.
- Không có tour trùng tên trong DB (không ràng buộc unique, nhưng để test rõ ràng).

**Test Steps**:
1. **Arrange**: Chuẩn bị `TourCreateVM` với dữ liệu hợp lệ (FX-003).
2. **Act**: Gọi `ToursController.Create(model)`.
3. **Assert**: Kiểm tra kết quả trả về và trạng thái DB.

**Expected Result (PASS)**:
- Controller trả về `RedirectToActionResult` với action = `"Index"`.
- DB chứa record mới với `TourName = "New Tour"`, `Price = 100000`, `MaxCapacity = 30`, `DurationHours = 8`.
- `TourId` được gán auto-increment (> 0).

**Expected Result (FAIL — dấu hiệu lỗi)**:
- Trả về View thay vì Redirect (dữ liệu không được lưu).
- Exception khi `SaveChangesAsync()`.

**Current Status**: 🟢 Passing
**Implementation Note**: Đảm bảo `ModelState.IsValid` trả `true` khi input hợp lệ.

---

### TOUR-TC-002 — Tạo tour thiếu required fields → Validation Error
**Severity**: HIGH
**Feature Under Test**: `ToursController.Create(TourCreateVM)` — ModelState validation
**Test File**: `Tests/Tours/CreateTourTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-002

**Preconditions**:
- Không cần seed data.

**Test Steps**:
1. **Arrange**: Chuẩn bị `TourCreateVM` thiếu `TourName` (FX-004).
2. **Act**: Thêm validation error vào `ModelState`, gọi `Controller.Create(model)`.
3. **Assert**: Kiểm tra kết quả.

**Expected Result (PASS)**:
- Controller trả về `ViewResult` (không redirect).
- `ModelState.IsValid == false`.
- `ModelState` chứa error cho key `"TourName"` với message `"Tour Name is required"`.
- DB không có record mới được tạo.

**Expected Result (FAIL — dấu hiệu lỗi)**:
- Record được tạo dù thiếu required field.
- Redirect thay vì re-render form.

**Current Status**: 🟢 Passing

---

### TOUR-TC-003 — Validation boundary values cho Price và MaxCapacity
**Severity**: MEDIUM
**Feature Under Test**: `TourCreateVM` Data Annotation validation
**Test File**: `Tests/Tours/TourValidationTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-003

**Preconditions**:
- Không cần DB (pure validation test).

**Test Steps & Expected Results**:

| Sub-case | Input                    | Expected                    |
| ----------| --------------------------| -----------------------------|
| 3a       | Price = 0                | ✅ Valid (Range: 0-999999999) |
| 3b       | Price = -1               | ❌ Invalid                   |
| 3c       | Price = 999999999        | ✅ Valid (upper boundary)    |
| 3d       | Price = 1000000000       | ❌ Invalid (exceeds range)   |
| 3e       | MaxCapacity = 1          | ✅ Valid (lower boundary)    |
| 3f       | MaxCapacity = 0          | ❌ Invalid                   |
| 3g       | MaxCapacity = 1000       | ✅ Valid (upper boundary)    |
| 3h       | MaxCapacity = 1001       | ❌ Invalid                   |
| 3i       | DurationHours = 1        | ✅ Valid                     |
| 3j       | DurationHours = 0        | ❌ Invalid                   |
| 3k       | TourName 100 chars       | ✅ Valid (max length)        |
| 3l       | TourName 101 chars       | ❌ Invalid (exceeds length)  |

**Current Status**: 🟢 Passing
**Implementation Note**: Dùng `System.ComponentModel.DataAnnotations.Validator.TryValidateObject()` để unit test validation.

---

### TOUR-TC-004 — Xem danh sách tours
**Severity**: HIGH
**Feature Under Test**: `ToursController.Index()` → `TourService.GetAllToursAsync()`
**Test File**: `Tests/Tours/ListTourTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-004

**Preconditions**:
- DB chứa ≥ 2 tours (seed data FX-001, FX-002).

**Test Steps**:
1. **Arrange**: Mock `ITourService.GetAllToursAsync()` trả về danh sách 2 tours.
2. **Act**: Gọi `controller.Index()`.
3. **Assert**: Kiểm tra kết quả.

**Expected Result (PASS)**:
- Trả về `ViewResult`.
- Model là `IEnumerable<Tour>` chứa đúng 2 items.
- Mỗi tour chứa đầy đủ các field: TourId, TourName, Price.

**Current Status**: 🟢 Passing

---

### TOUR-TC-005 — Xem chi tiết tour hợp lệ
**Severity**: HIGH
**Feature Under Test**: `ToursController.Details(int? id)`
**Test File**: `Tests/Tours/DetailsTourTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-005

**Preconditions**:
- DB chứa tour với `TourId = 1` (FX-001).

**Test Steps**:
1. **Arrange**: Mock `ITourService.GetTourByIdAsync(1)` trả về tour FX-001.
2. **Act**: Gọi `controller.Details(1)`.
3. **Assert**: Kiểm tra kết quả.

**Expected Result (PASS)**:
- Trả về `ViewResult`.
- Model là `Tour` với `TourId = 1`, `TourName = "City Tour"`.

**Current Status**: 🟢 Passing

---

### TOUR-TC-006 — Xem chi tiết tour không tồn tại → NotFound
**Severity**: MEDIUM
**Feature Under Test**: `ToursController.Details(int? id)`
**Test File**: `Tests/Tours/DetailsTourTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-006

**Preconditions**:
- Tour với `TourId = 999` không tồn tại.

**Test Steps**:
1. **Arrange**: Mock `ITourService.GetTourByIdAsync(999)` trả về `null`.
2. **Act**: Gọi `controller.Details(999)`.
3. **Assert**: Kiểm tra kết quả.

**Expected Result (PASS)**:
- Trả về `NotFoundResult` (HTTP 404).

**Test Steps thêm — id = null**:
1. Gọi `controller.Details(null)`.
2. **Expected**: Trả về `NotFoundResult`.

**Current Status**: 🟢 Passing

---

### TOUR-TC-007 — Sửa tour thành công
**Severity**: HIGH
**Feature Under Test**: `ToursController.Edit(int id, TourEditVM model)` → `TourService.UpdateTourAsync()`
**Test File**: `Tests/Tours/EditTourTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-007

**Preconditions**:
- Tour với `TourId = 1` tồn tại trong DB (FX-001).

**Test Steps**:
1. **Arrange**: Mock `ITourService.GetTourByIdAsync(1)` trả về tour FX-001. Chuẩn bị `TourEditVM` với `TourId = 1`, `TourName = "Updated City Tour"`, `Price = 600000`.
2. **Act**: Gọi `controller.Edit(1, model)`.
3. **Assert**: Kiểm tra kết quả.

**Expected Result (PASS)**:
- Controller trả về `RedirectToActionResult` với action = `"Index"`.
- `UpdateTourAsync()` được gọi đúng 1 lần.
- Tour entity được cập nhật: `TourName = "Updated City Tour"`, `Price = 600000`.

**Current Status**: 🟢 Passing

---

### TOUR-TC-008 — Sửa tour với ID không khớp → NotFound
**Severity**: MEDIUM
**Feature Under Test**: `ToursController.Edit(int id, TourEditVM model)`
**Test File**: `Tests/Tours/EditTourTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-008

**Preconditions**:
- Không cần seed data.

**Test Steps**:
1. **Arrange**: `TourEditVM` với `TourId = 1`, nhưng route id = `2`.
2. **Act**: Gọi `controller.Edit(2, model)` (id mismatch).
3. **Assert**: Kiểm tra kết quả.

**Expected Result (PASS)**:
- Trả về `NotFoundResult` (HTTP 404).
- `UpdateTourAsync()` KHÔNG được gọi.

**Current Status**: 🟢 Passing

---

### TOUR-TC-009 — Xóa tour thành công (không có dependencies)
**Severity**: HIGH
**Feature Under Test**: `ToursController.DeleteConfirmed(int id)` → `TourService.DeleteTourAsync()`
**Test File**: `Tests/Tours/DeleteTourTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-009

**Preconditions**:
- Tour với `TourId = 1` tồn tại, KHÔNG có TourSchedule/TourBooking liên kết.

**Test Steps**:
1. **Arrange**: Mock `ITourService.GetTourByIdAsync(1)` trả về tour. Mock `DeleteTourAsync(1)` thành công.
2. **Act**: Gọi `controller.DeleteConfirmed(1)`.
3. **Assert**: Kiểm tra kết quả.

**Expected Result (PASS)**:
- Trả về `RedirectToActionResult` với action = `"Index"`.
- `DeleteTourAsync(1)` được gọi đúng 1 lần.
- DB không còn record với `TourId = 1`.

**Current Status**: 🟢 Passing

---

### TOUR-TC-010 — Xóa tour có TourSchedule → nên bị chặn
**Severity**: HIGH
**Feature Under Test**: `TourService.DeleteTourAsync(int id)` — FK constraint handling
**Test File**: `Tests/Tours/DeleteTourTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-010

**Preconditions**:
- Tour với `TourId = 2` (FX-002) có 1 TourSchedule liên kết.

**Test Steps**:
1. **Arrange**: DB seed tour FX-002 với TourSchedule.
2. **Act**: Gọi `TourService.DeleteTourAsync(2)`.
3. **Assert**: Kiểm tra kết quả.

**Expected Result (PASS — hệ thống an toàn)**:
- `DbUpdateException` được throw (FK constraint violation).
- HOẶC service kiểm tra trước và throw custom exception `"Tour has active dependencies"`.
- DB vẫn giữ nguyên record `TourId = 2`.

**Expected Result (FAIL — lỗi logic)**:
- Tour bị xóa, TourSchedule trở thành orphan record.

**Current Status**: 🟢 Passing
**Implementation Note**: Cần implement kiểm tra `_context.TourSchedules.Any(ts => ts.TourId == tourId)` trước khi xóa.

---

### SECURITY TEST CASES

#### TOUR-TC-SEC-001 — XSS Payload trong TourName
**Severity**: CRITICAL
**OWASP**: A03:2021 — Injection
**CWE**: CWE-79 — Cross-site Scripting (XSS)
**Feature Under Test**: `ToursController.Create(TourCreateVM)` + Razor View rendering
**Test File**: `Tests/Tours/TourSecurityTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-011

**Preconditions**:
- Không cần seed data.

**Test Steps (Attack Simulation)**:
1. **Arrange**: `TourCreateVM` với `TourName = "<script>alert('XSS')</script>"` (FX-005).
2. **Act**: Submit form và lưu vào DB. Render tour details page.
3. **Assert**: Kiểm tra output HTML.

**Expected Result (PASS = hệ thống an toàn)**:
- Tour được lưu vào DB thành công (dữ liệu raw).
- Khi render trên page, output được HTML-encoded: `&lt;script&gt;alert(&#x27;XSS&#x27;)&lt;/script&gt;`.
- JavaScript KHÔNG được thực thi.
- Razor mặc định encode output với `@Model.TourName` — verify behavior này.

**Expected Result (FAIL = lỗ hổng tồn tại)**:
- Script tag được render nguyên vẹn trên HTML, cho phép XSS attack.

**Current Status**: 🟢 Passing

---

#### TOUR-TC-SEC-002 — CSRF Protection trên POST actions
**Severity**: HIGH
**OWASP**: A05:2021 — Security Misconfiguration
**CWE**: CWE-352 — Cross-Site Request Forgery (CSRF)
**Feature Under Test**: `[ValidateAntiForgeryToken]` attribute trên Create/Edit/Delete POST
**Test File**: `Tests/Tours/TourSecurityTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-012

**Preconditions**:
- Controller đã apply `[ValidateAntiForgeryToken]`.

**Test Steps (Attack Simulation)**:
1. **Arrange**: Chuẩn bị POST request KHÔNG có antiforgery token.
2. **Act**: Gửi POST `/Tours/Create` với form data hợp lệ nhưng không có `__RequestVerificationToken`.
3. **Assert**: Kiểm tra response.

**Expected Result (PASS = hệ thống an toàn)**:
- HTTP 400 Bad Request (antiforgery validation failed).
- Tour KHÔNG được tạo trong DB.

**Expected Result (FAIL = lỗ hổng tồn tại)**:
- Tour được tạo mà không cần CSRF token.

**Current Status**: 🟢 Passing

---

### INTEGRATION TEST CASES

#### TOUR-TC-INT-001 — Full CRUD Flow (Create → Read → Update → Delete)
**Severity**: HIGH
**Feature Under Test**: Full flow: `Create → Index → Details → Edit → Delete`
**Test File**: `Tests/Tours/TourIntegrationTests.cs`
**TDD Phase**: 🟢 GREEN
**Condition Ref**: TC-COND-001, TC-COND-004, TC-COND-005, TC-COND-007, TC-COND-009

**Preconditions**:
- SQL Server chạy với database test.
- EF Core migration đã apply.
- Database trống hoặc seed data đã clear.

**Test Steps**:
1. **Create**: POST `/Tours/Create` với FX-003 → Verify redirect, DB có 1 record mới.
2. **Read List**: GET `/Tours` → Verify response chứa tour vừa tạo.
3. **Read Detail**: GET `/Tours/Details/{newId}` → Verify TourName, Price đúng.
4. **Update**: POST `/Tours/Edit/{newId}` với TourName = "Updated Tour" → Verify redirect, DB field đã thay đổi.
5. **Delete**: POST `/Tours/Delete/{newId}` → Verify redirect, DB không còn record.

**Expected Result (PASS)**:
- Mỗi bước trả về expected status code.
- Database state nhất quán sau mỗi operation.
- Cuối flow, `SELECT COUNT(*) FROM Tours WHERE TourId = {newId}` = 0.

**DB Assertion**:
```csharp
// Sau Create
var created = await _context.Tours.FirstOrDefaultAsync(t => t.TourName == "New Tour");
Assert.NotNull(created);
Assert.Equal(100000, created.Price);

// Sau Update
var updated = await _context.Tours.FindAsync(created.TourId);
Assert.Equal("Updated Tour", updated.TourName);

// Sau Delete
var deleted = await _context.Tours.FindAsync(created.TourId);
Assert.Null(deleted);
```

**Current Status**: 🟢 Passing

---

#### TOUR-TC-INT-002 — Concurrent Create không gây duplicate
**Severity**: MEDIUM
**Feature Under Test**: `TourService.AddTourAsync()` — concurrency safety
**Test File**: `Tests/Tours/TourIntegrationTests.cs`
**TDD Phase**: 🟢 GREEN

**Preconditions**:
- SQL Server chạy.

**Test Steps**:
1. **Arrange**: Chuẩn bị 2 `TourCreateVM` giống nhau.
2. **Act**: Gọi `AddTourAsync()` 2 lần đồng thời (parallel).
3. **Assert**: Kiểm tra DB.

**Expected Result (PASS)**:
- Cả 2 tour đều được tạo thành công (TourName không unique constraint).
- Mỗi tour có `TourId` khác nhau.
- Không có deadlock hoặc exception.

**Current Status**: 🟢 Passing

---

## 5. Red-Green-Refactor Tracker
| TC ID              | Test File                              | 🔴 RED confirmed | 🟢 GREEN (commit) | 🔵 REFACTOR note          |
| --------------------| ----------------------------------------| -------------------| --------------------| ----------------------------|
| `TOUR-TC-001`      | `Tests/Tours/CreateTourTests.cs`       | `[x]`             | `[x]`               |                            |
| `TOUR-TC-002`      | `Tests/Tours/CreateTourTests.cs`       | `[x]`             | `[x]`               |                            |
| `TOUR-TC-003`      | `Tests/Tours/TourValidationTests.cs`   | `[x]`             | `[x]`               |                            |
| `TOUR-TC-004`      | `Tests/Tours/ListTourTests.cs`         | `[x]`             | `[x]`               |                            |
| `TOUR-TC-005`      | `Tests/Tours/DetailsTourTests.cs`      | `[x]`             | `[x]`               |                            |
| `TOUR-TC-006`      | `Tests/Tours/DetailsTourTests.cs`      | `[x]`             | `[x]`               |                            |
| `TOUR-TC-007`      | `Tests/Tours/EditTourTests.cs`         | `[x]`             | `[x]`               |                            |
| `TOUR-TC-008`      | `Tests/Tours/EditTourTests.cs`         | `[x]`             | `[x]`               |                            |
| `TOUR-TC-009`      | `Tests/Tours/DeleteTourTests.cs`       | `[x]`             | `[x]`               |                            |
| `TOUR-TC-010`      | `Tests/Tours/DeleteTourTests.cs`       | `[x]`             | `[x]`               | Cần implement FK check     |
| `TOUR-TC-SEC-001`  | `Tests/Tours/TourSecurityTests.cs`     | `[x]`             | `[x]`               |                            |
| `TOUR-TC-SEC-002`  | `Tests/Tours/TourSecurityTests.cs`     | `[x]`             | `[x]`               |                            |
| `TOUR-TC-INT-001`  | `Tests/Tours/TourIntegrationTests.cs`  | `[x]`             | `[x]`               |                            |
| `TOUR-TC-INT-002`  | `Tests/Tours/TourIntegrationTests.cs`  | `[x]`             | `[x]`               |                            |

## 6. Entry / Exit Criteria

**Entry Criteria (Điều kiện bắt đầu)**
- [x] Spec kỹ thuật `ManageTour_EDS.md` đã được tạo
- [x] Logic Issues (Section 2) đã được xác nhận
- [x] EF Core migration cho bảng `Tours` đã apply thành công
- [x] Test fixtures (Section 3 TDS-05) đã được chuẩn bị
- [x] Test project đã được tạo và cấu hình (xUnit + Moq)

**Exit Criteria (Điều kiện kết thúc — DoD)**
- [x] `dotnet test` — tất cả unit tests pass (không có skip)
- [x] `dotnet test --filter Category=Integration` — tất cả integration tests pass
- [x] Test coverage ≥ 80% lines cho `ToursController.cs` và `TourService.cs`
- [x] Validation tests cover tất cả boundary values (TDS-03)
- [x] Security tests (XSS, CSRF) đều PASS
- [x] Không có plaintext sensitive data trong logs

**Suspension Criteria (Điều kiện tạm dừng)**
- Database test environment không khả dụng
- Phát hiện bug trong EF Core migration cần fix trước
- Module 1 (Auth) chưa provide Authorization middleware (TOUR-TC-SEC tests bị block)

## 7. Rollback Plan
```bash
# Revert test files
git checkout -- Tests/Tours/

# Revert implementation nếu test phát hiện bug
git checkout -- PhucNguyenResort/Controllers/ToursController.cs
git checkout -- PhucNguyenResort/Services/Tours/TourService.cs

# Clean test database
dotnet ef database update 0 --project PhucNguyenResort
dotnet ef database update --project PhucNguyenResort
```
