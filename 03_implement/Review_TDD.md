# TEST-DRIVEN DEVELOPMENT SPECIFICATION TEMPLATE

Mẫu Đặc tả Kiểm thử Hướng Phát triển

**Document ID**: PHUCNGUYEN-TDD-REVIEW-001
**Version**: 2.0
**Date**: 2026-06-28
**Status**: approved
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: Nguyễn Khắc Hiếu
**Reviewed by**: truongcuong
**DPO Sign-off**: truongcuong
**Approved by**: Nguyễn Khắc Hiếu
**Classification**: Internal - Confidential

**References**:

- `01_SRS/Review_SRS.md` (PHUCNGUYEN-REVIEW-SRS-001) — Review Module SRS
- `01_SRS/SWP391-HOS-02.md` — UC18, UC19 (Submit Review, Moderate Reviews)
- `01_SRS/SWP391_NHOM1.md` — UC14, UC29, UC47, UC48, UC49 (View/Submit/Moderate Reviews)
- `03_implement/Review_EDS.md` (PHUCNGUYEN-REVIEW-IMP-001) — Technical Specification
- `template/TDD_TEMPLATE_V1.md` — TDD Template v1.0

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.
Thứ tự bắt buộc: viết test → chạy → xác nhận FAIL 🔴 → implement → PASS 🟢 → refactor 🔵.
Test data dùng dữ liệu SYNTHETIC. Không dùng PII thật.

## CHANGELOG

**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ.

| Ngày      | Người thực hiện | Nội dung thay đổi                                                                             |
| ---------- | ------------------- | ------------------------------------------------------------------------------------------------ |
| 2026-06-27 | AI                  | Khởi tạo tài liệu TDD spec cho Review Module                                                 |
| 2026-06-28 | AI                  | Cập nhật theo TDD Template v1.0, bổ sung SRS Review, thêm TC-COND mới, cập nhật test data |

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

| Field                 | Value                                                        |
| --------------------- | ------------------------------------------------------------ |
| Feature / Gap ID      | GAP-REVIEW-001                                               |
| Module                | `ReviewModule`                                             |
| Spec gốc             | `PHUCNGUYEN-REVIEW-IMP-001` (Review_EDS.md)                |
| SRS gốc              | `PHUCNGUYEN-REVIEW-SRS-001` (Review_SRS.md)                |
| Priority              | 🟠 P1                                                        |
| Sprint                | S4                                                           |
| Milestone             | M3 Alpha - 2026-07-11                                        |
| Data Classification   | Internal                                                     |
| Compliance Scope      | Vietnam Law on Tourism 2017                                  |
| Upstream Dependencies | `AuthModule`, `RoomBookingModule`, `TourBookingModule` |
| Downstream Consumers  | `HomeModule` (hiển thị review)                           |

---

## 2. Logic Issues Resolved

Bắt buộc điền trước khi viết test.
Liệt kê mọi sai lệch giữa spec thiết kế và schema/policy/codebase thực tế.
Test cases sẽ encode hành vi đã sửa, không phải hành vi trong spec gốc.

| #  | Spec gốc (sai / thiếu)                                                                        | Thực tế (schema / policy)                                                                 | Fix áp dụng trong test                                                             |
| -- | ----------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------ |
| L1 | SRS (UC18) nói "after checking out or completing a tour" nhưng chưa rõ field nào kiểm tra | `Booking.BookingStatus == CheckedOut` hoặc `TourBooking.BookingStatus == Completed`    | Test phải verify chỉ chấp nhận review khi status đúng, reject khi status khác |
| L2 | SRS (UC19) nói "moderate" nhưng chưa rõ là xóa hay ẩn                                    | Model dùng`IsVisible` flag (soft hide, không delete)                                    | Test phải verify toggle`IsVisible`, không xóa record                            |
| L3 | Model`Review.Rating` là nullable (`int?`) nhưng khi submit nên bắt buộc                | Validation phải kiểm tra Rating != null trước khi lưu                                  | Test phải verify submit với Rating=null bị reject                                 |
| L4 | Model`Review` không có unique constraint cho (UserId, BookingId) hoặc (UserId, TourId)     | Cần kiểm tra duplicate ở tầng Service (application-level)                               | Test phải verify service reject duplicate, không dựa vào DB constraint           |
| L5 | Chưa rõ Customer có thể review cả Room lẫn Tour trên cùng 1 review không               | Mỗi review chỉ gắn với 1 loại: hoặc`BookingId` hoặc `TourId` (không cùng lúc) | Test phải verify reject khi cả BookingId và TourId đều có giá trị            |
| L6 | SRS (FR-REV-003) nói "Rating 1-5 sao" nhưng model cho phép int?                              | SubmitReviewVM dùng`[Range(1, 5)]` bắt buộc, service cần validate thêm               | Test phải verify boundary values: 0, 1, 5, 6, -1 theo Equivalence Partitioning      |

---

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi

Mô tả phạm vi của TDD spec này: component nào được kiểm thử, layer nào được bao phủ.

```
ReviewModule bao gồm các layer:
├── Domain Models (Review)
├── ViewModels (SubmitReviewVM, ReviewListVM, ModerateReviewVM)
├── Services (IReviewService, ReviewService)
├── Controllers (ReviewController)
└── Integration (Entity Framework In-Memory DB)
```

### TDS-02 — Test Basis / Cơ sở Kiểm thử

Điều kiện kiểm thử được derive từ các nguồn sau:

| Source             | Items Derived                                       |
| ------------------ | --------------------------------------------------- |
| `SRS` FR-REV-001 | Guest xem danh sách review công khai              |
| `SRS` FR-REV-002 | Customer submit 5-star rating + written review      |
| `SRS` FR-REV-003 | Rate Resort Services (Room + Tour, 5-star scale)    |
| `SRS` FR-REV-004 | Business Manager kiểm duyệt review                |
| `SRS` FR-REV-005 | Business Manager hide/show review                   |
| `SRS` FR-REV-006 | Business Manager xem chi tiết review               |
| `SRS` UC14       | Guest xem danh sách review công khai              |
| `SRS` UC18       | Customer submit 5-star rating + written review      |
| `SRS` UC19       | Admin/Manager moderate guest reviews (Hide/Display) |
| `SRS` UC29       | Customer submit reviews chia sẻ trải nghiệm      |
| `SRS` UC47       | Business Manager kiểm duyệt review                |
| `SRS` UC48       | Business Manager xem customer reviews               |
| `SRS` UC49       | Business Manager approve/hide review                |
| BR-REV-001         | Chỉ CheckedOut/Completed mới được review       |
| BR-REV-002         | Chống duplicate review                             |
| BR-REV-003         | Rating [1, 5]                                       |
| BR-REV-004         | IsVisible default true, Manager toggle              |
| BR-REV-005         | Mỗi review chỉ gắn 1 loại dịch vụ             |
| ADR-001            | Eligibility check trước khi submit                |
| ADR-002            | Soft hide thay vì hard delete                      |

### TDS-03 — Test Conditions and Coverage Items

Mỗi condition map sang ≥ 1 test case cụ thể.

| Condition ID | Test Condition                                                            | Coverage Item                             | Test Cases            |
| ------------ | ------------------------------------------------------------------------- | ----------------------------------------- | --------------------- |
| TC-COND-001  | Submit review thành công cho Booking đã CheckedOut                    | `ReviewService.CreateReviewAsync()`     | `REVIEW-TC-001`     |
| TC-COND-002  | Submit review thành công cho Tour đã Completed                        | `ReviewService.CreateReviewAsync()`     | `REVIEW-TC-002`     |
| TC-COND-003  | Submit review bị reject khi Booking chưa CheckedOut                     | `ReviewService.CreateReviewAsync()`     | `REVIEW-TC-003`     |
| TC-COND-004  | Submit review bị reject khi đã có review trùng (duplicate)           | `ReviewService.CreateReviewAsync()`     | `REVIEW-TC-004`     |
| TC-COND-005  | Submit review bị reject khi Rating ngoài khoảng [1, 5]                 | `ReviewService.CreateReviewAsync()`     | `REVIEW-TC-005`     |
| TC-COND-006  | Submit review bị reject khi cả BookingId và TourId đều null          | `ReviewService.CreateReviewAsync()`     | `REVIEW-TC-006`     |
| TC-COND-007  | Toggle visibility ẩn review đang hiện                                  | `ReviewService.ToggleVisibilityAsync()` | `REVIEW-TC-007`     |
| TC-COND-008  | Toggle visibility hiện review đang ẩn                                  | `ReviewService.ToggleVisibilityAsync()` | `REVIEW-TC-008`     |
| TC-COND-009  | Toggle visibility bị reject khi reviewId không tồn tại                | `ReviewService.ToggleVisibilityAsync()` | `REVIEW-TC-009`     |
| TC-COND-010  | Public list chỉ trả về review có IsVisible=true                       | `ReviewService.GetPublicReviewsAsync()` | `REVIEW-TC-010`     |
| TC-COND-011  | Manage list trả về tất cả review (cả visible lẫn hidden)            | `ReviewService.GetAllReviewsAsync()`    | `REVIEW-TC-011`     |
| TC-COND-012  | Guest không thể truy cập Manage page                                   | `ReviewController.Manage()`             | `REVIEW-TC-SEC-001` |
| TC-COND-013  | Customer không thể toggle visibility                                    | `ReviewController.ToggleVisibility()`   | `REVIEW-TC-SEC-002` |
| TC-COND-014  | XSS injection trong Comment bị encode                                    | `ReviewService` + View rendering        | `REVIEW-TC-SEC-003` |
| TC-COND-015  | Submit review bị reject khi cả BookingId và TourId đều có giá trị | `ReviewService.CreateReviewAsync()`     | `REVIEW-TC-012`     |
| TC-COND-016  | Submit review bị reject khi Tour chưa Completed                         | `ReviewService.CreateReviewAsync()`     | `REVIEW-TC-013`     |

### TDS-04 — Test Techniques / Kỹ thuật Kiểm thử

| Technique (ISO 29119-4)  | Applied To                                                | Rationale                                    |
| ------------------------ | --------------------------------------------------------- | -------------------------------------------- |
| Equivalence Partitioning | Rating input domain (valid: 1-5, invalid: 0, 6, -1, null) | Giảm số lượng test case cho input rating |
| Boundary Value Analysis  | Rating = 0, 1, 5, 6                                       | Kiểm tra biên giới chấp nhận            |
| State Transition Testing | IsVisible: true ↔ false                                  | Review có 2 trạng thái visibility         |
| Decision Table           | BookingId vs TourId (null/non-null) — 4 tổ hợp         | 4 tổ hợp input cần kiểm tra              |
| Error Guessing           | XSS injection trong Comment, SQL injection                | Phòng ngừa attack vectors phổ biến       |

**Decision Table — BookingId vs TourId**:

| Case | BookingId | TourId | Expected Result          | Test Case     |
| ---- | --------- | ------ | ------------------------ | ------------- |
| 1    | null      | null   | Reject (REV-002)         | REVIEW-TC-006 |
| 2    | 100       | null   | Accept ✅ (Room)         | REVIEW-TC-001 |
| 3    | null      | 50     | Accept ✅ (Tour)         | REVIEW-TC-002 |
| 4    | 100       | 50     | Reject (cả 2 giá trị) | REVIEW-TC-012 |

### TDS-05 — Test Data Requirements

| Fixture ID | Type    | Value / Logic                                                        | Mục đích                            |
| ---------- | ------- | -------------------------------------------------------------------- | -------------------------------------- |
| FX-001     | DB seed | `User(id=1, roleId=Customer)`                                      | Customer hợp lệ                      |
| FX-002     | DB seed | `Booking(id=100, userId=1, status=CheckedOut)`                     | Booking đã checkout                  |
| FX-003     | DB seed | `Booking(id=101, userId=1, status=CheckedIn)`                      | Booking chưa checkout (test reject)   |
| FX-004     | DB seed | `Tour(id=50, tourName="Ha Long Bay")`                              | Tour để review                       |
| FX-005     | DB seed | `TourBooking(id=200, userId=1, tourId=50, status=Completed)`       | Tour đã hoàn thành                 |
| FX-006     | DB seed | `TourBooking(id=201, userId=1, tourId=50, status=Confirmed)`       | Tour chưa hoàn thành (test reject)  |
| FX-007     | DB seed | `Review(id=10, userId=1, bookingId=100, rating=5, isVisible=true)` | Review đã tồn tại (test duplicate) |
| FX-008     | DB seed | `Review(id=11, userId=2, tourId=50, rating=3, isVisible=false)`    | Review đã bị ẩn (test toggle show) |
| FX-009     | DB seed | `User(id=10, roleId=BusinessManager)`                              | Business Manager account               |
| FX-010     | Session | `{ userId: 1, role: "Customer" }`                                  | Auth context cho Customer              |
| FX-011     | Session | `{ userId: 10, role: "BusinessManager" }`                          | Auth context cho Business Manager      |
| FX-012     | DB seed | `Booking(id=102, userId=1, status=CheckedOut)`                     | Booking thứ 2 (test không duplicate) |

---

## 4. Test Case Specification

**TC ID format**: `REVIEW-TC-[NNN]`
**Severity**: CRITICAL / HIGH / MEDIUM / LOW
**Status**: 🟢 Passing

---

### REVIEW-TC-001 — Happy Path: Submit Review cho Booking đã CheckedOut

**Severity**: CRITICAL
**Feature Under Test**: `ReviewService.CreateReviewAsync()`
**Test File**: `ReviewServiceTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-001

**Preconditions**:

- DB có `User(id=1)` với role Customer (FX-001)
- DB có `Booking(id=100, userId=1, status=CheckedOut)` (FX-002)
- Không có review nào cho `(userId=1, bookingId=100)`

**Test Steps**:

1. **Arrange**: Seed DB với FX-001, FX-002. Tạo `SubmitReviewVM { Rating=5, Comment="Dịch vụ tuyệt vời!", BookingId=100 }`
2. **Act**: Gọi `CreateReviewAsync(userId=1, model)`
3. **Assert**: Kiểm tra kết quả và DB state

**Expected Result (PASS — hành vi đúng)**:

- Method trả về `true`
- DB chứa 1 record `Review` mới với:
  - `UserId = 1`
  - `BookingId = 100`
  - `Rating = 5`
  - `Comment = "Dịch vụ tuyệt vời!"`
  - `IsVisible = true`
  - `CreatedAt` ≈ DateTime.Now

**Expected Result (FAIL — dấu hiệu lỗi)**:

- Method throw exception khi không nên
- Record không được tạo trong DB
- `IsVisible` không default `true`

**Current Status**: 🟢 Passing
**Implementation Note**: Service cần Include Booking khi truy vấn để check status.

---

### REVIEW-TC-002 — Happy Path: Submit Review cho Tour đã Completed

**Severity**: CRITICAL
**Feature Under Test**: `ReviewService.CreateReviewAsync()`
**Test File**: `ReviewServiceTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-002

**Preconditions**:

- DB có `User(id=1)` với role Customer (FX-001)
- DB có `Tour(id=50)` (FX-004)
- DB có `TourBooking(id=200, userId=1, scheduleId, status=Completed)` liên kết với Tour id=50 (FX-005)
- Không có review nào cho `(userId=1, tourId=50)`

**Test Steps**:

1. **Arrange**: Seed DB với FX-001, FX-004, FX-005. Tạo `SubmitReviewVM { Rating=4, Comment="Tour rất thú vị", TourId=50 }`
2. **Act**: Gọi `CreateReviewAsync(userId=1, model)`
3. **Assert**: Kiểm tra kết quả

**Expected Result (PASS)**:

- Method trả về `true`
- DB chứa `Review` mới với `TourId=50`, `Rating=4`

**Expected Result (FAIL)**:

- Method throw exception hoặc trả về false khi Tour đã Completed

**Current Status**: 🟢 Passing
**Implementation Note**: Service cần kiểm tra `TourBooking.BookingStatus == Completed` qua bảng `TourBookings` (join với Schedule → Tour).

---

### REVIEW-TC-003 — Reject: Submit Review khi Booking chưa CheckedOut

**Severity**: CRITICAL
**Feature Under Test**: `ReviewService.CreateReviewAsync()`
**Test File**: `ReviewServiceTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-003

**Preconditions**:

- DB có `Booking(id=101, userId=1, status=CheckedIn)` — chưa CheckedOut (FX-003)

**Test Steps**:

1. **Arrange**: Seed DB với FX-001, FX-003
2. **Act**: Gọi `CreateReviewAsync(userId=1, { Rating=3, BookingId=101 })`
3. **Assert**: Bắt exception

**Expected Result (PASS — hệ thống reject đúng)**:

- Throw `InvalidOperationException` với message chứa "REV-003" hoặc "chưa hoàn thành"
- Không có record mới trong bảng `Reviews`

**Expected Result (FAIL — lỗ hổng)**:

- Review được tạo thành công cho Booking chưa CheckedOut
- Không có validation check trên BookingStatus

**Current Status**: 🟢 Passing

---

### REVIEW-TC-004 — Reject: Duplicate Review Prevention

**Severity**: HIGH
**Feature Under Test**: `ReviewService.CreateReviewAsync()`
**Test File**: `ReviewServiceTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-004

**Preconditions**:

- DB đã có `Review(id=10, userId=1, bookingId=100)` (FX-007)
- Booking 100 vẫn tồn tại với status CheckedOut

**Test Steps**:

1. **Arrange**: Seed DB với FX-001, FX-002, FX-007
2. **Act**: Gọi `CreateReviewAsync(userId=1, { Rating=4, BookingId=100 })`
3. **Assert**: Bắt exception

**Expected Result (PASS)**:

- Throw `InvalidOperationException` với message chứa "REV-004" hoặc "đã đánh giá"
- DB vẫn chỉ có 1 record Review cho (userId=1, bookingId=100)

**Expected Result (FAIL)**:

- Tạo được review thứ 2 cho cùng Booking
- DB có 2+ records cho (userId=1, bookingId=100)

**Current Status**: 🟢 Passing

---

### REVIEW-TC-005 — Reject: Rating ngoài khoảng [1, 5]

**Severity**: HIGH
**Feature Under Test**: `ReviewService.CreateReviewAsync()`
**Test File**: `ReviewServiceTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-005

**Preconditions**:

- DB có `Booking(id=102, userId=1, status=CheckedOut)` (FX-012)
- Không có review trùng cho booking 102

**Test Steps (Equivalence Partitioning + Boundary Analysis)**:

| Test Input | Rating | Expected Result  |
| ---------- | ------ | ---------------- |
| Case 1     | 0      | Reject (REV-001) |
| Case 2     | -1     | Reject (REV-001) |
| Case 3     | 6      | Reject (REV-001) |
| Case 4     | 1      | Accept ✅        |
| Case 5     | 5      | Accept ✅        |
| Case 6     | 3      | Accept ✅        |

**Expected Result (PASS)**:

- Cases 1, 2, 3: Throw exception hoặc ModelState error "REV-001"
- Cases 4, 5, 6: Review tạo thành công

**Current Status**: 🟢 Passing

---

### REVIEW-TC-006 — Reject: Thiếu cả BookingId và TourId

**Severity**: HIGH
**Feature Under Test**: `ReviewService.CreateReviewAsync()`
**Test File**: `ReviewServiceTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-006

**Preconditions**:

- User hợp lệ (FX-001)

**Test Steps**:

1. **Arrange**: Tạo `SubmitReviewVM { Rating=4, Comment="Test", BookingId=null, TourId=null }`
2. **Act**: Gọi `CreateReviewAsync(userId=1, model)`
3. **Assert**: Bắt exception

**Expected Result (PASS)**:

- Throw `InvalidOperationException` với message chứa "REV-002"

**Expected Result (FAIL)**:

- Review được tạo không gắn với dịch vụ nào

**Current Status**: 🟢 Passing

---

### REVIEW-TC-007 — Toggle Visibility: Hide Review đang hiện

**Severity**: CRITICAL
**Feature Under Test**: `ReviewService.ToggleVisibilityAsync()`
**Test File**: `ReviewServiceTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-007

**Preconditions**:

- DB có `Review(id=10, isVisible=true)` (FX-007)

**Test Steps**:

1. **Arrange**: Seed DB với FX-007
2. **Act**: Gọi `ToggleVisibilityAsync(reviewId=10)`
3. **Assert**: Kiểm tra DB

**Expected Result (PASS)**:

- Method trả về `true`
- DB: `Review(id=10).IsVisible == false`
- Record **không bị xóa** khỏi DB (soft hide theo ADR-002)

**Expected Result (FAIL)**:

- `IsVisible` vẫn là `true`
- Record bị xóa khỏi DB (hard delete)

**Current Status**: 🟢 Passing

---

### REVIEW-TC-008 — Toggle Visibility: Show Review đang ẩn

**Severity**: HIGH
**Feature Under Test**: `ReviewService.ToggleVisibilityAsync()`
**Test File**: `ReviewServiceTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-008

**Preconditions**:

- DB có `Review(id=11, isVisible=false)` (FX-008)

**Test Steps**:

1. **Arrange**: Seed DB với FX-008
2. **Act**: Gọi `ToggleVisibilityAsync(reviewId=11)`
3. **Assert**: Kiểm tra DB

**Expected Result (PASS)**:

- Method trả về `true`
- DB: `Review(id=11).IsVisible == true`

**Expected Result (FAIL)**:

- `IsVisible` vẫn là `false`

**Current Status**: 🟢 Passing

---

### REVIEW-TC-009 — Reject: Toggle Visibility với ReviewId không tồn tại

**Severity**: MEDIUM
**Feature Under Test**: `ReviewService.ToggleVisibilityAsync()`
**Test File**: `ReviewServiceTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-009

**Preconditions**:

- DB không có review nào với `ReviewId=9999`

**Test Steps**:

1. **Act**: Gọi `ToggleVisibilityAsync(reviewId=9999)`
2. **Assert**: Bắt exception

**Expected Result (PASS)**:

- Throw `KeyNotFoundException` với message chứa "REV-005"

**Expected Result (FAIL)**:

- Method trả về `true` mà không có review nào bị thay đổi
- Throw exception khác (NullReferenceException)

**Current Status**: 🟢 Passing

---

### REVIEW-TC-010 — Public List chỉ trả về review Visible

**Severity**: CRITICAL
**Feature Under Test**: `ReviewService.GetPublicReviewsAsync()`
**Test File**: `ReviewServiceTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-010

**Preconditions**:

- DB có `Review(id=10, isVisible=true)` (FX-007)
- DB có `Review(id=11, isVisible=false)` (FX-008)

**Test Steps**:

1. **Arrange**: Seed DB với FX-007, FX-008
2. **Act**: Gọi `GetPublicReviewsAsync()`
3. **Assert**: Kiểm tra kết quả

**Expected Result (PASS)**:

- Kết quả chỉ chứa review id=10 (isVisible=true)
- **Không** chứa review id=11 (isVisible=false)
- Count = 1

**Expected Result (FAIL)**:

- Kết quả chứa review id=11 (bị ẩn bị leak ra public)
- Count > 1

**Current Status**: 🟢 Passing

---

### REVIEW-TC-011 — Manage List trả về tất cả reviews

**Severity**: HIGH
**Feature Under Test**: `ReviewService.GetAllReviewsAsync()`
**Test File**: `ReviewServiceTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-011

**Preconditions**:

- DB có `Review(id=10, isVisible=true)` (FX-007)
- DB có `Review(id=11, isVisible=false)` (FX-008)

**Test Steps**:

1. **Arrange**: Seed DB với FX-007, FX-008
2. **Act**: Gọi `GetAllReviewsAsync()`
3. **Assert**: Kiểm tra kết quả

**Expected Result (PASS)**:

- Kết quả chứa **cả** review id=10 và id=11
- Count = 2

**Expected Result (FAIL)**:

- Kết quả bị filter, chỉ chứa visible reviews
- Count < 2

**Current Status**: 🟢 Passing

---

### REVIEW-TC-012 — Reject: Cả BookingId và TourId đều có giá trị

**Severity**: HIGH
**Feature Under Test**: `ReviewService.CreateReviewAsync()`
**Test File**: `ReviewServiceTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-015

**Preconditions**:

- DB có `Booking(id=100, userId=1, status=CheckedOut)` (FX-002)
- DB có `Tour(id=50)` (FX-004)
- DB có `TourBooking(id=200, userId=1, tourId=50, status=Completed)` (FX-005)

**Test Steps**:

1. **Arrange**: Tạo `SubmitReviewVM { Rating=4, Comment="Test", BookingId=100, TourId=50 }`
2. **Act**: Gọi `CreateReviewAsync(userId=1, model)`
3. **Assert**: Bắt exception

**Expected Result (PASS)**:

- Throw `InvalidOperationException` — mỗi review chỉ gắn với 1 loại dịch vụ

**Expected Result (FAIL)**:

- Review được tạo với cả BookingId và TourId

**Current Status**: 🟢 Passing

---

### REVIEW-TC-013 — Reject: Submit Review khi Tour chưa Completed

**Severity**: CRITICAL
**Feature Under Test**: `ReviewService.CreateReviewAsync()`
**Test File**: `ReviewServiceTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-016

**Preconditions**:

- DB có `Tour(id=50)` (FX-004)
- DB có `TourBooking(id=201, userId=1, tourId=50, status=Confirmed)` — chưa Completed (FX-006)

**Test Steps**:

1. **Arrange**: Seed DB với FX-001, FX-004, FX-006
2. **Act**: Gọi `CreateReviewAsync(userId=1, { Rating=3, TourId=50 })`
3. **Assert**: Bắt exception

**Expected Result (PASS — hệ thống reject đúng)**:

- Throw `InvalidOperationException` với message chứa "REV-003" hoặc "chưa hoàn thành"
- Không có record mới trong bảng `Reviews`

**Expected Result (FAIL — lỗ hổng)**:

- Review được tạo thành công cho Tour chưa Completed
- Không có validation check trên TourBooking status

**Current Status**: 🟢 Passing

---

### SECURITY TEST CASES

Test cases kiểm tra attack vectors — điền thêm field OWASP và CWE.

---

#### REVIEW-TC-SEC-001 — Unauthorized Access: Guest truy cập Manage

**Severity**: CRITICAL
**OWASP**: A01:2021 — Broken Access Control
**CWE**: CWE-862 — Missing Authorization
**Feature Under Test**: `ReviewController.Manage()` Authorization
**Test File**: `ReviewControllerTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement

**Preconditions**:

- User chưa đăng nhập (Guest)

**Test Steps (Attack Simulation)**:

1. Truy cập `/Review/Manage` không có session
2. Kiểm tra response

**Expected Result (PASS = hệ thống an toàn)**:

- Redirect đến Login page (302)
- Không trả về danh sách reviews

**Expected Result (FAIL = lỗ hổng tồn tại)**:

- Trả về trang Manage với danh sách reviews
- Status 200 OK

**Current Status**: 🟢 Passing

---

#### REVIEW-TC-SEC-002 — Privilege Escalation: Customer toggle visibility

**Severity**: CRITICAL
**OWASP**: A01:2021 — Broken Access Control
**CWE**: CWE-269 — Improper Privilege Management
**Feature Under Test**: `ReviewController.ToggleVisibility()` Authorization
**Test File**: `ReviewControllerTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement

**Preconditions**:

- User đăng nhập với role Customer (không phải Business Manager)

**Test Steps (Attack Simulation)**:

1. POST `/Review/ToggleVisibility` với `ReviewId=10` bằng session Customer
2. Kiểm tra response và DB state

**Expected Result (PASS = hệ thống an toàn)**:

- 403 Forbidden hoặc Redirect với error
- DB: `Review(id=10).IsVisible` **không** thay đổi

**Expected Result (FAIL = lỗ hổng tồn tại)**:

- Review visibility bị thay đổi bởi Customer
- Status 200/302 success

**Current Status**: 🟢 Passing

---

#### REVIEW-TC-SEC-003 — XSS Prevention trong Comment

**Severity**: HIGH
**OWASP**: A03:2021 — Injection
**CWE**: CWE-79 — Cross-site Scripting (XSS)
**Feature Under Test**: `ReviewService.CreateReviewAsync()` + View rendering
**Test File**: `ReviewServiceTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement

**Preconditions**:

- Customer đủ điều kiện submit review (FX-001, FX-012)

**Test Steps (Attack Simulation)**:

1. Submit review với Comment = `<script>alert('XSS')</script>`
2. Truy cập PublicList, xem review vừa tạo
3. Kiểm tra HTML output

**Expected Result (PASS = hệ thống an toàn)**:

- Comment được HTML-encode khi hiển thị
- Không có script execution trên browser
- Output hiển thị: `&lt;script&gt;alert('XSS')&lt;/script&gt;`

**Expected Result (FAIL = lỗ hổng tồn tại)**:

- Script thực thi trên browser khi xem review
- Comment không được encode

**Current Status**: 🟢 Passing
**Implementation Note**: ASP.NET MVC mặc định HTML-encode output trong Razor Views (`@Model.Comment`), nhưng cần verify không dùng `@Html.Raw()`.

---

### INTEGRATION TEST CASES

---

#### REVIEW-TC-INT-001 — Full Flow: Submit Review → View Public → Moderate

**Severity**: HIGH
**Feature Under Test**: Full flow: `Submit → GetPublicReviews → ToggleVisibility → GetPublicReviews`
**Test File**: `ReviewIntegrationTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-001, TC-COND-007, TC-COND-010

**Preconditions**:

- In-Memory DB hoặc Test SQL DB
- Seed: FX-001, FX-012 (Booking 102 chưa có review), FX-009

**Test Steps**:

1. **Step 1**: Customer (userId=1) submit review `{ rating=5, comment="Tuyệt!", bookingId=102 }`
2. **Step 2**: Gọi `GetPublicReviewsAsync()` → verify review mới xuất hiện
3. **Step 3**: Business Manager toggle visibility cho review mới
4. **Step 4**: Gọi `GetPublicReviewsAsync()` → verify review không còn xuất hiện
5. **Step 5**: Business Manager toggle visibility lần nữa (show lại)
6. **Step 6**: Gọi `GetPublicReviewsAsync()` → verify review xuất hiện lại

**Expected Result (PASS)**:

- Step 2: List chứa review mới (count +1)
- Step 4: List không chứa review (isVisible=false)
- Step 6: List chứa review (isVisible=true lại)

**Expected Result (FAIL)**:

- Bất kỳ step nào không đúng expected
- Data inconsistency giữa các bước

**DB Assertion**:

```csharp
var review = await _context.Reviews
    .FirstOrDefaultAsync(r => r.BookingId == 102 && r.UserId == 1);
Assert.NotNull(review);
Assert.Equal(5, review.Rating);

// After toggle hide
await _reviewService.ToggleVisibilityAsync(review.ReviewId);
await _context.Entry(review).ReloadAsync();
Assert.False(review.IsVisible);

// After toggle show
await _reviewService.ToggleVisibilityAsync(review.ReviewId);
await _context.Entry(review).ReloadAsync();
Assert.True(review.IsVisible);
```

**Current Status**: 🟢 Passing

---

#### REVIEW-TC-INT-002 — Full Flow: Duplicate Prevention Across Requests

**Severity**: HIGH
**Feature Under Test**: `ReviewService.CreateReviewAsync()` uniqueness enforcement
**Test File**: `ReviewIntegrationTests.cs`
**TDD Phase**: 🟢 GREEN — đã implement
**Condition Ref**: TC-COND-001, TC-COND-004

**Preconditions**:

- In-Memory DB
- Seed: FX-001, FX-012 (Booking 102, chưa có review)

**Test Steps**:

1. Customer submit review cho Booking 102 → thành công
2. Customer submit review cho Booking 102 lần nữa → bị reject

**Expected Result (PASS)**:

- Step 1: Success, 1 review created
- Step 2: Exception "REV-004", still only 1 review in DB

**DB Assertion**:

```csharp
var count = await _context.Reviews
    .CountAsync(r => r.BookingId == 102 && r.UserId == 1);
Assert.Equal(1, count); // Vẫn chỉ 1 record
```

**Current Status**: 🟢 Passing

---

## 5. Red-Green-Refactor Tracker

| TC ID                 | Test File                     | 🔴 RED confirmed | 🟢 GREEN (commit) | 🔵 REFACTOR note |
| --------------------- | ----------------------------- | ---------------- | ----------------- | ---------------- |
| `REVIEW-TC-001`     | `ReviewServiceTests.cs`     | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-002`     | `ReviewServiceTests.cs`     | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-003`     | `ReviewServiceTests.cs`     | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-004`     | `ReviewServiceTests.cs`     | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-005`     | `ReviewServiceTests.cs`     | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-006`     | `ReviewServiceTests.cs`     | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-007`     | `ReviewServiceTests.cs`     | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-008`     | `ReviewServiceTests.cs`     | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-009`     | `ReviewServiceTests.cs`     | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-010`     | `ReviewServiceTests.cs`     | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-011`     | `ReviewServiceTests.cs`     | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-012`     | `ReviewServiceTests.cs`     | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-013`     | `ReviewServiceTests.cs`     | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-SEC-001` | `ReviewControllerTests.cs`  | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-SEC-002` | `ReviewControllerTests.cs`  | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-SEC-003` | `ReviewServiceTests.cs`     | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-INT-001` | `ReviewIntegrationTests.cs` | `[x]`          | `[x]`             |                  |
| `REVIEW-TC-INT-002` | `ReviewIntegrationTests.cs` | `[x]`          | `[x]`             |                  |

---

## 6. Entry / Exit Criteria

**Entry Criteria (Điều kiện bắt đầu)**

- [X] Spec kỹ thuật `PHUCNGUYEN-REVIEW-IMP-001` (Review_EDS.md) đã được tạo
- [X] SRS `PHUCNGUYEN-REVIEW-SRS-001` (Review_SRS.md) đã được tạo
- [X] Logic Issues (Section 2) đã được xác nhận
- [X] Model `Review` đã tồn tại trong codebase (`Models/Review.cs`)
- [X] `ReviewConfiguration` đã tồn tại (`Configurations/ReviewConfiguration.cs`)
- [X] DbContext đã có `DbSet<Review> Reviews`
- [X] Test fixtures (Section 3 TDS-05) đã được chuẩn bị

**Exit Criteria (Điều kiện kết thúc — DoD)**

- [x] Tất cả Unit Tests (REVIEW-TC-001 → 013) — 🟢 Passing
- [x] Tất cả Security Tests (REVIEW-TC-SEC-001 → 003) — 🟢 Passing
- [x] Tất cả Integration Tests (REVIEW-TC-INT-001 → 002) — 🟢 Passing
- [x] Test coverage ≥ 80% cho các file mới tạo (ReviewService, ReviewController)
- [x] Authorization đúng theo role matrix (§16 trong EDS)
- [x] Không có XSS vulnerability trong hiển thị Comment
- [x] Review không thể submit khi Booking/Tour chưa hoàn thành
- [x] Duplicate review bị ngăn chặn 100%
- [x] Mỗi review chỉ gắn với 1 loại dịch vụ (BookingId HOẶC TourId)

**Suspension Criteria (Điều kiện tạm dừng)**

- Blocker: Model Review cần migration thay đổi schema (hiện tại không cần)
- Blocker: Auth module chưa implement role-based authorization
- CI pipeline bị broken bởi thay đổi khác

---

## 7. Rollback Plan

```bash
# Revert implementation files
git checkout -- Controllers/ReviewController.cs
git checkout -- Services/Reviews/
git checkout -- ViewModels/Reviews/
git checkout -- Views/Review/

# Revert test files
git checkout -- Tests/ReviewServiceTests.cs
git checkout -- Tests/ReviewControllerTests.cs
git checkout -- Tests/ReviewIntegrationTests.cs

# Không cần rollback migration — model Review đã tồn tại sẵn
# Module Review không tạo migration mới

# Gap vẫn OPEN → giữ nguyên entry trong backlog
```



