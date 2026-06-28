# ENGINEERING DOCUMENTATION STANDARD (EDS) v2.0

Quy chuẩn Tài liệu Kỹ thuật và Đặc tả Hiện thực hóa

| Field          | Value                         |
| -------------- | ----------------------------- |
| Document ID    | `PHUCNGUYEN-REVIEW-IMP-001` |
| Version        | 2.0                           |
| Date           | 2026-06-28                    |
| Status         | approved                      |
| Document Owner | Nguyễn Khắc Hiếu           |
| Author         | Nguyễn Khắc Hiếu           |
| Reviewed by    | truongcuong                   |
| DPO Sign-off   | truongcuong                   |
| Approved by    | Nguyễn Khắc Hiếu           |
| Last Review    | 2026-06-28                    |
| Based on EDS   | v2.0                          |

## CHANGELOG

**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ. Mọi thay đổi phải ghi vào bảng này.

| Ngày      | Người thực hiện | Nội dung thay đổi                                                                                           |
| ---------- | ------------------- | -------------------------------------------------------------------------------------------------------------- |
| 2026-06-27 | AI                  | Khởi tạo tài liệu EDS cho module Review & Rating                                                           |
| 2026-06-28 | AI                  | Cập nhật theo EDS Template v2.0, bổ sung SRS Review, cập nhật ADR, NFR, Scalability, Event Payload Schema |

## MỤC LỤC

1. Tổng quan Module
2. Ma trận Truy vết (Traceability Matrix)
3. Architecture Decision Records (ADR) ⭐
4. Non-Functional Requirements & SLA ⭐
5. Static Modeling (Mô hình Tĩnh)
6. Dynamic Modeling (Mô hình Động)
7. Domain Event Catalog ⭐
8. Interface Specification (Đặc tả Giao diện)
9. API Specification
10. Bảng mã lỗi (Error Codes)
11. Quy trình Triển khai (Step-by-Step)
12. Rollback & Incident Runbook ⭐
13. Kịch bản Kiểm thử Chi tiết
14. Phương pháp Xác minh
15. Mẫu thử thực tế (API Verification Samples)
16. Bảng tổng hợp phân quyền (Authorization Matrix)

---

## 1. Tổng quan Module

Module **Review & Rating** quản lý toàn bộ vòng đời đánh giá của khách hàng đối với dịch vụ resort, bao gồm:

- **Submit Review**: Khách hàng gửi đánh giá (rating 1-5 sao + bình luận) sau khi hoàn thành dịch vụ (check-out phòng hoặc hoàn thành tour).
- **Rate Resort Services**: Hỗ trợ đánh giá cả dịch vụ phòng (Room) và dịch vụ tour (Tour) theo thang điểm 5 sao.
- **Moderate Reviews**: Business Manager xem xét, kiểm duyệt nội dung đánh giá.
- **Hide/Show Reviews**: Business Manager ẩn/hiện đánh giá không phù hợp hoặc khôi phục đánh giá đã ẩn.

| Field                 | Value                                                        |
| --------------------- | ------------------------------------------------------------ |
| Module Name           | `ReviewModule`                                             |
| Bounded Context       | `Customer Feedback & Service Quality`                      |
| Data Classification   | Internal (không chứa PII nhạy cảm)                       |
| Compliance Scope      | Vietnam Law on Tourism 2017, Decree 356/2025                 |
| Upstream Dependencies | `AuthModule`, `RoomBookingModule`, `TourBookingModule` |
| Downstream Consumers  | `HomeModule` (hiển thị review trên trang chủ)          |

---

## 2. Ma trận Truy vết (Traceability Matrix)

Ánh xạ trực tiếp: [Mã yêu cầu] → [Thành phần Code] → [Mục tiêu Tuân thủ].
**Policy**: Không viết code nếu không biết code đó phục vụ Rule nào.

| Requirement ID | Loại (BR/ADR/US) | Mô tả yêu cầu                                                                         | Thành phần Code                                     | Compliance Target   | ADR liên quan |
| -------------- | ----------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------- | ------------------- | -------------- |
| FR-REV-001     | User Story        | Guest xem danh sách review công khai                                                    | `ReviewController.ViewPublicReviews()`              | —                  | —             |
| FR-REV-002     | User Story        | Customer gửi rating 5 sao và bình luận sau checkout / hoàn thành tour               | `ReviewController.SubmitReview()`                   | Law on Tourism 2017 | ADR-001        |
| FR-REV-003     | User Story        | Hệ thống hỗ trợ đánh giá theo thang 5 sao cho Room và Tour                        | `SubmitReviewVM.Rating` + `[Range(1, 5)]`         | —                  | —             |
| FR-REV-004     | User Story        | Business Manager kiểm duyệt đánh giá của khách                                     | `ReviewController.ModerateReviews()`                | —                  | ADR-002        |
| FR-REV-005     | User Story        | Business Manager phê duyệt hoặc ẩn review                                             | `ReviewService.ToggleVisibilityAsync()`             | —                  | ADR-002        |
| FR-REV-006     | User Story        | Business Manager xem chi tiết đánh giá                                                | `ReviewController.Details()`                        | —                  | —             |
| UC14           | User Story        | Guest xem danh sách review công khai                                                    | `ReviewController.ViewPublicReviews()`              | —                  | —             |
| UC18 (SRS)     | User Story        | Customer gửi rating 5 sao và bình luận sau checkout / hoàn thành tour               | `ReviewController.SubmitReview()`                   | Law on Tourism 2017 | ADR-001        |
| UC29           | User Story        | Customer gửi review chia sẻ trải nghiệm                                               | `ReviewService.CreateReviewAsync()`                 | —                  | ADR-001        |
| UC47           | User Story        | Business Manager kiểm duyệt đánh giá của khách                                     | `ReviewController.ModerateReviews()`                | —                  | ADR-002        |
| UC48           | User Story        | Business Manager xem danh sách đánh giá của khách                                   | `ReviewController.GetAllReviews()`                  | —                  | —             |
| UC49           | User Story        | Business Manager phê duyệt hoặc ẩn review                                             | `ReviewService.ToggleVisibilityAsync()`             | —                  | ADR-002        |
| BR-REV-001     | Business Rule     | Chỉ Customer đã hoàn thành dịch vụ (CheckedOut/Completed) mới được đánh giá | `ReviewService.ValidateEligibility()`               | Quality Assurance   | ADR-001        |
| BR-REV-002     | Business Rule     | Mỗi Customer chỉ được đánh giá 1 lần cho mỗi Booking hoặc TourBooking          | `ReviewService.CheckDuplicateReview()`              | Data Integrity      | ADR-001        |
| BR-REV-003     | Business Rule     | Rating phải nằm trong khoảng 1-5                                                       | `Review.Rating` + `[Range(1, 5)]` validation      | Data Validation     | —             |
| BR-REV-004     | Business Rule     | Review mặc định`IsVisible = true`, Business Manager có thể ẩn/hiện               | `ReviewConfiguration` + `ToggleVisibilityAsync()` | Content Moderation  | ADR-002        |
| BR-REV-005     | Business Rule     | Mỗi review chỉ gắn với 1 loại dịch vụ (BookingId HOẶC TourId)                     | `ReviewService.CreateReviewAsync()` validation      | Data Integrity      | ADR-001        |

---

## 3. Architecture Decision Records (ADR)

⭐ **Section mới — EDS v2.0**
Ghi lại lý do đằng sau mỗi quyết định kiến trúc quan trọng. DPO và Auditor cần section này để hiểu tại sao hệ thống được thiết kế như vậy.

### ADR-001 — Điều kiện Submit Review

| Field    | Value      |
| -------- | ---------- |
| Status   | Accepted   |
| Deciders | Tech Lead  |
| Date     | 2026-06-27 |

**Bối cảnh (Context)**
Khách hàng chỉ nên được phép đánh giá sau khi đã thực sự sử dụng dịch vụ. Cần xác định rõ điều kiện nào cho phép submit review. SRS (UC18) ghi "after checking out or completing a tour" nhưng chưa rõ field nào kiểm tra.

**Các phương án đã xem xét (Options Considered)**

| Phương án | Mô tả                                            | Ưu điểm                      | Nhược điểm                              |
| ------------ | -------------------------------------------------- | ------------------------------- | ------------------------------------------- |
| A            | Cho phép review bất cứ lúc nào sau khi đặt  | Đơn giản, dễ implement      | Review spam, đánh giá không chính xác |
| B            | Chỉ cho phép sau CheckedOut hoặc Tour Completed | Đảm bảo chất lượng review | Phức tạp hơn logic kiểm tra             |

**Quyết định (Decision)**
Chọn Phương án B — Chỉ cho phép Customer submit review khi:

- **Review phòng**: `Booking.BookingStatus == CheckedOut`
- **Review tour**: `TourBooking.BookingStatus == Completed`

Đồng thời, mỗi Customer chỉ được submit **tối đa 1 review** cho mỗi Booking hoặc TourBooking (chống duplicate). Mỗi review chỉ gắn với **1 loại dịch vụ** (BookingId HOẶC TourId, không cùng lúc).

**Hệ quả (Consequences)**

Tích cực:

- Đảm bảo mọi review đều đến từ khách hàng đã sử dụng dịch vụ thực tế.
- Ngăn chặn review spam và đánh giá không chính xác.

Tiêu cực / Trade-offs:

- Cần truy vấn thêm bảng `Bookings` / `TourBookings` để validate — giảm thiểu bằng cách include eager loading.
- Model `Review.Rating` là nullable (`int?`) trong codebase nhưng phải bắt buộc khi submit — kiểm tra tại tầng Service.

Compliance Impact:

- Tuân thủ Vietnam Law on Tourism 2017 — chỉ review thực tế từ khách hàng đã sử dụng dịch vụ.

---

### ADR-002 — Chiến lược Moderate Reviews (Soft Hide)

| Field    | Value      |
| -------- | ---------- |
| Status   | Accepted   |
| Deciders | Tech Lead  |
| Date     | 2026-06-27 |

**Bối cảnh (Context)**
Business Manager cần kiểm soát nội dung review hiển thị công khai, nhưng không nên xóa vĩnh viễn để lưu lịch sử. SRS (UC19) nói "moderate" nhưng chưa rõ là xóa hay ẩn.

**Các phương án đã xem xét (Options Considered)**

| Phương án | Mô tả                               | Ưu điểm                              | Nhược điểm                                   |
| ------------ | ------------------------------------- | --------------------------------------- | ------------------------------------------------ |
| A            | Hard Delete — xóa record khỏi DB   | Đơn giản                             | Mất dữ liệu vĩnh viễn, không audit được |
| B            | Soft Hide — toggle`IsVisible` flag | Giữ nguyên data, có thể khôi phục | Cần filter khi hiển thị                       |

**Quyết định (Decision)**
Chọn Phương án B — Sử dụng field `IsVisible` (boolean, default = `true`). Business Manager toggle giá trị này để ẩn/hiện review. Trang public chỉ hiển thị review có `IsVisible = true`.

**Hệ quả (Consequences)**

Tích cực:

- Không mất dữ liệu, có thể khôi phục review đã ẩn bất cứ lúc nào.
- Hỗ trợ audit trail — luôn biết review nào đã bị ẩn.

Tiêu cực / Trade-offs:

- Mọi query public phải luôn filter `WHERE IsVisible = true` — giảm thiểu bằng cách encapsulate trong Service method.

Compliance Impact:

- Tuân thủ Decree 356/2025 — bảo toàn dữ liệu, không xóa vĩnh viễn.

---

## 4. Non-Functional Requirements & SLA

⭐ **Section mới — EDS v2.0**

### 4.1. Performance & Availability

| Category     | Requirement                   | Target SLA | Measurement Method | Compliance Basis |
| ------------ | ----------------------------- | ---------- | ------------------ | ---------------- |
| Latency      | Submit review API response    | < 500ms    | Load test          | —               |
| Latency      | Get reviews list API response | < 300ms    | Load test          | —               |
| Availability | System uptime                 | 99.9%      | Uptime monitor     | —               |
| Throughput   | Concurrent review submissions | 100 req/s  | Load test          | —               |

### 4.2. Data Integrity & Retention

| Category    | Requirement                      | Target    | Verification Method  | Compliance Basis |
| ----------- | -------------------------------- | --------- | -------------------- | ---------------- |
| Uniqueness  | 1 review per Booking/TourBooking | 100%      | Service-level check  | Data Integrity   |
| Consistency | Rating value 1-5                 | 100%      | Model validation     | Data Validation  |
| Durability  | No review data loss              | RPO=0     | Transaction log      | —               |
| Retention   | Review data retention            | Unlimited | Soft hide, no delete | Decree 356/2025  |

### 4.3. Security

| Category       | Requirement           | Target          | Verification Method | Compliance Basis |
| -------------- | --------------------- | --------------- | ------------------- | ---------------- |
| Access control | Role-based            | Least privilege | Auth Matrix (§16)  | OWASP A01:2021   |
| Input safety   | XSS prevention        | 100%            | HTML encode output  | OWASP A03:2021   |
| Injection      | SQL Injection blocked | 100%            | Parameterized query | OWASP A03:2021   |
| CSRF           | Anti-forgery token    | 100%            | MVC AntiForgery     | OWASP A01:2021   |

### 4.4. Scalability & Capacity Planning

Dự kiến tải trong 12 tháng tới:

- ~500 customers/tháng
- ~200 reviews/tháng (Room + Tour)
- Giải pháp scale: Vertical scaling (SQL Server) + server-side pagination cho danh sách review

---

## 5. Static Modeling (Mô hình Tĩnh)

### 5.1. Class Diagram

```
┌─────────────────────────┐
│        Review           │
├─────────────────────────┤
│ + ReviewId    : int     │ PK
│ + UserId      : int?    │ FK → Users
│ + TourId      : int?    │ FK → Tours
│ + BookingId   : int?    │ FK → Bookings
│ + Rating      : int?    │ [Range(1,5)]
│ + Comment     : string? │ Max 2000 chars
│ + IsVisible   : bool    │ Default: true
│ + CreatedAt   : DateTime│ Default: GETDATE()
├─────────────────────────┤
│ + User        : User?   │
│ + Tour        : Tour?   │
│ + Booking     : Booking?│
└─────────────────────────┘
         │
    ┌────┴────────────┬─────────────────┐
    ▼                 ▼                 ▼
┌────────┐    ┌──────────┐    ┌──────────────┐
│ User   │    │  Tour    │    │   Booking    │
│ (1:N)  │    │  (1:N)   │    │   (1:N)      │
└────────┘    └──────────┘    └──────────────┘
```

### 5.2. Data Structure (C# Model — hiện tại trong codebase)

```csharp
// File: Models/Review.cs
public class Review
{
    public int ReviewId { get; set; }
    public int? UserId { get; set; }
    public int? TourId { get; set; }
    public int? BookingId { get; set; }

    [Range(1, 5)]
    public int? Rating { get; set; }

    public string? Comment { get; set; }
    public bool IsVisible { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public User? User { get; set; }
    public Tour? Tour { get; set; }
    public Booking? Booking { get; set; }
}
```

### 5.3. Entity Configuration (EF Core)

```csharp
// File: Configurations/ReviewConfiguration.cs
public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");
        builder.HasKey(x => x.ReviewId);
        builder.Property(x => x.IsVisible).HasDefaultValue(true);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
        builder.HasOne(x => x.User)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(x => x.Tour)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.TourId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(x => x.Booking)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
```

### 5.4. ViewModel Specifications

```csharp
// File: ViewModels/Reviews/SubmitReviewVM.cs
public class SubmitReviewVM
{
    [Range(1, 5, ErrorMessage = "Rating phải từ 1 đến 5")]
    public int Rating { get; set; }

    [StringLength(2000, ErrorMessage = "Bình luận không vượt quá 2000 ký tự")]
    public string? Comment { get; set; }

    public int? BookingId { get; set; }   // Review phòng
    public int? TourId { get; set; }      // Review tour
}

// File: ViewModels/Reviews/ReviewListVM.cs
public class ReviewListVM
{
    public int ReviewId { get; set; }
    public string? UserName { get; set; }
    public int? Rating { get; set; }
    public string? Comment { get; set; }
    public bool IsVisible { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ServiceType { get; set; }  // "Room" | "Tour"
    public string? ServiceName { get; set; }  // Tên Tour hoặc Room Category
}

// File: ViewModels/Reviews/ModerateReviewVM.cs
public class ModerateReviewVM
{
    public int ReviewId { get; set; }
    public bool IsVisible { get; set; }
}
```

---

## 6. Dynamic Modeling (Mô hình Động)

### 6.1. Sequence Diagram — Submit Review (Happy Path)

```
Customer           ReviewController         ReviewService            DB (Reviews)
   │                     │                       │                       │
   │  POST /Review/Submit│                       │                       │
   │────────────────────►│                       │                       │
   │                     │  CreateReviewAsync()  │                       │
   │                     │──────────────────────►│                       │
   │                     │                       │  Validate Eligibility │
   │                     │                       │  (Check Booking/Tour  │
   │                     │                       │   status)             │
   │                     │                       │──────────────────────►│
   │                     │                       │◄──────────────────────│
   │                     │                       │  Check Duplicate      │
   │                     │                       │──────────────────────►│
   │                     │                       │◄──────────────────────│
   │                     │                       │  INSERT Review        │
   │                     │                       │──────────────────────►│
   │                     │                       │◄──────────────────────│
   │                     │◄──────────────────────│                       │
   │  Redirect (Success) │                       │                       │
   │◄────────────────────│                       │                       │
```

### 6.2. Sequence Diagram — Toggle Visibility (Moderate)

```
BusinessManager    ReviewController         ReviewService            DB (Reviews)
   │                     │                       │                       │
   │  POST /Review/Toggle│                       │                       │
   │────────────────────►│                       │                       │
   │                     │  ToggleVisibility()   │                       │
   │                     │──────────────────────►│                       │
   │                     │                       │  FindById(reviewId)   │
   │                     │                       │──────────────────────►│
   │                     │                       │◄──────────────────────│
   │                     │                       │  UPDATE IsVisible     │
   │                     │                       │──────────────────────►│
   │                     │                       │◄──────────────────────│
   │                     │◄──────────────────────│                       │
   │  Redirect (Success) │                       │                       │
   │◄────────────────────│                       │                       │
```

### 6.3. Sequence Diagram — Error Path (Duplicate Review)

```
Customer           ReviewController         ReviewService            DB (Reviews)
   │                     │                       │                       │
   │  POST /Review/Submit│                       │                       │
   │────────────────────►│                       │                       │
   │                     │  CreateReviewAsync()  │                       │
   │                     │──────────────────────►│                       │
   │                     │                       │  Check Duplicate      │
   │                     │                       │──────────────────────►│
   │                     │                       │  ← EXISTS!            │
   │                     │                       │◄──────────────────────│
   │                     │  ← Error "Already     │                       │
   │                     │    reviewed"           │                       │
   │                     │◄──────────────────────│                       │
   │  Error Message      │                       │                       │
   │◄────────────────────│                       │                       │
```

### 6.4. Sequence Diagram — Error Path (Service Not Completed)

```
Customer           ReviewController         ReviewService            DB (Bookings)
   │                     │                       │                       │
   │  POST /Review/Submit│                       │                       │
   │────────────────────►│                       │                       │
   │                     │  CreateReviewAsync()  │                       │
   │                     │──────────────────────►│                       │
   │                     │                       │  Validate Eligibility │
   │                     │                       │  (Check Booking       │
   │                     │                       │   status)             │
   │                     │                       │──────────────────────►│
   │                     │                       │  ← Status=CheckedIn  │
   │                     │                       │◄──────────────────────│
   │                     │  ← Error "REV-003"    │                       │
   │                     │◄──────────────────────│                       │
   │  Error Message      │                       │                       │
   │  "Dịch vụ chưa      │                       │                       │
   │   hoàn thành"       │                       │                       │
   │◄────────────────────│                       │                       │
```

### 6.5. State Machine — Review Visibility

```
                    ┌──────────────────┐
                    │                  │
      Submit ──────►│  Visible (true)  │◄──────── Show (Manager)
                    │                  │
                    └────────┬─────────┘
                             │
                        Hide (Manager)
                             │
                    ┌────────▼─────────┐
                    │                  │
                    │  Hidden (false)  │
                    │                  │
                    └──────────────────┘
```

---

## 7. Domain Event Catalog

⭐ **Section mới — EDS v2.0**
Liệt kê tất cả domain events mà module này phát ra (publish) và tiêu thụ (consume). Đặt tên event theo quy tắc `[Entity][PastTenseVerb]`.

### 7.1. Events Published (Phát ra)

| Event Name          | Trigger                  | Publisher         | Subscriber(s)  | Payload Schema                                | Async? |
| ------------------- | ------------------------ | ----------------- | -------------- | --------------------------------------------- | ------ |
| `ReviewSubmitted` | Customer submit review   | `ReviewService` | `HomeModule` | `{ reviewId, userId, rating, serviceType }` | No     |
| `ReviewToggled`   | Manager hide/show review | `ReviewService` | —             | `{ reviewId, isVisible }`                   | No     |

### 7.2. Events Consumed (Tiêu thụ)

| Event Name            | Source                | Handler           | Action thực hiện             |
| --------------------- | --------------------- | ----------------- | ------------------------------ |
| `BookingCheckedOut` | `BookingModule`     | `ReviewService` | Cho phép submit review phòng |
| `TourCompleted`     | `TourBookingModule` | `ReviewService` | Cho phép submit review tour   |

### 7.3. Payload Schema

```csharp
// ReviewSubmittedEvent
public class ReviewSubmittedEvent
{
    public int ReviewId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; }
    public string ServiceType { get; set; } // "Room" | "Tour"
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}

// ReviewToggledEvent
public class ReviewToggledEvent
{
    public int ReviewId { get; set; }
    public bool IsVisible { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}
```

---

## 8. Interface Specification (Đặc tả Giao diện)

**Policy (EDS v2.0)**: Mỗi interface phải khai báo `@version`. Mọi breaking change phải tạo ADR mới.

### 8.1. Service Interface

```csharp
// IReviewService.cs
// @version 1.0

public interface IReviewService
{
    /// <summary>
    /// Tạo review mới cho Customer.
    /// </summary>
    /// <param name="userId">ID của Customer đang đăng nhập</param>
    /// <param name="model">Dữ liệu review (rating, comment, bookingId/tourId)</param>
    /// <returns>True nếu tạo thành công</returns>
    /// <exception cref="InvalidOperationException">Khi chưa đủ điều kiện review hoặc đã review rồi</exception>
    Task<bool> CreateReviewAsync(int userId, SubmitReviewVM model);

    /// <summary>
    /// Lấy danh sách review công khai (IsVisible = true) cho Guest/Customer.
    /// </summary>
    /// <param name="tourId">Lọc theo Tour (nullable)</param>
    /// <returns>Danh sách ReviewListVM</returns>
    Task<IEnumerable<ReviewListVM>> GetPublicReviewsAsync(int? tourId = null);

    /// <summary>
    /// Lấy tất cả review (bao gồm hidden) cho Business Manager.
    /// </summary>
    /// <returns>Danh sách ReviewListVM</returns>
    Task<IEnumerable<ReviewListVM>> GetAllReviewsAsync();

    /// <summary>
    /// Toggle visibility của review (ẩn/hiện).
    /// </summary>
    /// <param name="reviewId">ID của review cần toggle</param>
    /// <returns>True nếu thành công</returns>
    /// <exception cref="KeyNotFoundException">Khi reviewId không tồn tại</exception>
    Task<bool> ToggleVisibilityAsync(int reviewId);

    /// <summary>
    /// Lấy chi tiết review theo ID.
    /// </summary>
    /// <param name="reviewId">ID của review</param>
    /// <returns>ReviewListVM hoặc null</returns>
    Task<ReviewListVM?> GetReviewByIdAsync(int reviewId);

    /// <summary>
    /// Kiểm tra Customer có đủ điều kiện submit review cho Booking/Tour không.
    /// </summary>
    /// <param name="userId">ID của Customer</param>
    /// <param name="bookingId">ID của Booking (nullable)</param>
    /// <param name="tourId">ID của Tour (nullable)</param>
    /// <returns>True nếu đủ điều kiện</returns>
    Task<bool> CanSubmitReviewAsync(int userId, int? bookingId, int? tourId);
}
```

### 8.2. Repository (sử dụng DbContext trực tiếp)

Dự án sử dụng `ResortDbContext` trực tiếp trong Service layer (không có Repository pattern riêng), consistent với các module khác.

```csharp
// Truy cập qua: _context.Reviews
// Các operations:
//   _context.Reviews.AddAsync(review)
//   _context.Reviews.FindAsync(reviewId)
//   _context.Reviews.Where(r => r.IsVisible == true)
//   _context.SaveChangesAsync()
```

---

## 9. API Specification

### 9.1. Endpoints Table (MVC Controller Actions)

Dự án sử dụng MVC pattern (không phải Web API), nên các endpoint là Controller Actions trả về View.

| Method | Path                         | Auth Level | Required Roles      | Description                      |
| ------ | ---------------------------- | ---------- | ------------------- | -------------------------------- |
| GET    | `/Review/PublicList`       | Anonymous  | `Guest, Customer` | Xem danh sách review công khai |
| GET    | `/Review/Submit`           | Session    | `Customer`        | Hiển thị form submit review    |
| POST   | `/Review/Submit`           | Session    | `Customer`        | Gửi review                      |
| GET    | `/Review/Manage`           | Session    | `BusinessManager` | Danh sách review (manage)       |
| POST   | `/Review/ToggleVisibility` | Session    | `BusinessManager` | Ẩn/hiện review                 |
| GET    | `/Review/Details/{id}`     | Session    | `BusinessManager` | Xem chi tiết review             |

### 9.2. Request / Response Schemas

**POST `/Review/Submit` — Submit Review**

Request (Form Data):

```
Rating=5
Comment=Dịch vụ rất tuyệt vời!
BookingId=123         // hoặc TourId=456
```

Response — Success: `Redirect` to `/Review/PublicList` with `TempData["SuccessMessage"]`

Response — Error: Return View with `ModelState` errors

---

**POST `/Review/ToggleVisibility` — Toggle Visibility**

Request (Form Data):

```
ReviewId=10
```

Response — Success: `Redirect` to `/Review/Manage` with `TempData["SuccessMessage"]`

Response — Error: `Redirect` with `TempData["ErrorMessage"]`

---

## 10. Bảng mã lỗi (Error Codes)

Tiền tố mã lỗi: `REV-` cho module Review.

| Code        | HTTP Status | Message (EN)                          | Message (VI)                                          | Trigger Condition                                     |
| ----------- | ----------- | ------------------------------------- | ----------------------------------------------------- | ----------------------------------------------------- |
| `REV-001` | 400         | Rating must be between 1 and 5        | Rating phải nằm trong khoảng 1 đến 5             | Rating ngoài khoảng [1, 5]                          |
| `REV-002` | 400         | Must provide BookingId or TourId      | Phải cung cấp BookingId hoặc TourId                | Cả BookingId và TourId đều null                   |
| `REV-003` | 403         | Service not completed yet             | Dịch vụ chưa hoàn thành, không thể đánh giá | Booking chưa CheckedOut hoặc Tour chưa Completed   |
| `REV-004` | 409         | Review already submitted              | Bạn đã đánh giá dịch vụ này rồi             | Đã tồn tại review cho Booking/Tour của user này |
| `REV-005` | 404         | Review not found                      | Không tìm thấy đánh giá                         | ReviewId không tồn tại trong DB                    |
| `REV-006` | 403         | Insufficient permissions              | Không đủ quyền thực hiện                        | User không có role Business Manager khi moderate    |
| `REV-007` | 400         | Comment cannot exceed 2000 characters | Bình luận không vượt quá 2000 ký tự           | Comment.Length > 2000                                 |

---

## 11. Quy trình Triển khai (Step-by-Step)

### 11.1. Prerequisites

- [X] ADR đã được Accepted (xem §3)
- [X] Model `Review` đã tồn tại trong codebase (✅ đã có)
- [X] `ReviewConfiguration` đã tồn tại (✅ đã có)
- [X] DbContext đã có `DbSet<Review>` (✅ đã có)
- [ ] DPO sign-off: Không yêu cầu — module không xử lý PII nhạy cảm

### 11.2. Pre-Migration Checklist

Không cần migration mới — bảng `Reviews` đã tồn tại trong codebase.

- [X] Model `Review` đã có trong DB schema
- [X] Không cần chạy `Add-Migration` hay `Update-Database`

### 11.3. Implementation Steps

**Chặng 1 — Tạo ViewModels**

```
Tạo các file:
- ViewModels/Reviews/SubmitReviewVM.cs
- ViewModels/Reviews/ReviewListVM.cs
- ViewModels/Reviews/ModerateReviewVM.cs
```

**Chặng 2 — Tạo Service Layer**

```
Tạo các file:
- Services/Reviews/IReviewService.cs
- Services/Reviews/ReviewService.cs

Đăng ký trong Program.cs:
builder.Services.AddScoped<IReviewService, ReviewService>();
```

**Chặng 3 — Tạo Controller**

```
Tạo file:
- Controllers/ReviewController.cs

Implement các action methods:
- PublicList (GET)
- Submit (GET/POST)
- Manage (GET)
- ToggleVisibility (POST)
- Details (GET)
```

**Chặng 4 — Tạo Views**

```
Tạo các file:
- Views/Review/PublicList.cshtml    (Guest/Customer xem review)
- Views/Review/Submit.cshtml        (Form submit review)
- Views/Review/Manage.cshtml        (Business Manager quản lý)
- Views/Review/Details.cshtml       (Chi tiết review)
```

**Chặng 5 — Verification sau deploy**

```bash
# Kiểm tra nhanh sau khi deploy
dotnet build
dotnet run

# Verification steps:
1. Đăng nhập với tài khoản Customer có Booking đã CheckedOut
2. Thử submit review → kiểm tra tạo thành công
3. Đăng nhập với tài khoản Business Manager
4. Toggle visibility → kiểm tra review bị ẩn
5. Truy cập PublicList → kiểm tra review đã ẩn không hiển thị
```

### 11.4. Deployment Checklist

- [ ] Tất cả ViewModels đã tạo
- [ ] Service interface + implementation đã tạo
- [ ] Service đã đăng ký DI trong `Program.cs`
- [ ] Controller đã tạo và test thủ công
- [ ] Views đã tạo và hiển thị đúng
- [ ] Authorization hoạt động đúng theo role
- [ ] Không migration mới cần thiết (model Review đã tồn tại)
- [ ] Health check: `dotnet build` thành công, không có warning/error

---

## 12. Rollback & Incident Runbook

⭐ **Section mới — EDS v2.0**
Section này là bắt buộc. Một deploy thiếu rollback plan là deploy chưa hoàn chỉnh.

### 12.1. Điều kiện kích hoạt Rollback (Trigger Conditions)

| Điều kiện                         | Ngưỡng           | Người quyết định |
| ------------------------------------ | ------------------ | --------------------- |
| Submit review gây crash app         | Bất kỳ lần nào | Dev Lead              |
| Data không nhất quán (Rating<1)   | Bất kỳ case nào | Dev Lead              |
| Authorization bypass                 | Bất kỳ case nào | Dev Lead              |
| Hidden review bị leak ra PublicList | Bất kỳ case nào | Dev Lead + DPO        |

### 12.2. Rollback Procedure

```bash
# Bước 1: Revert implementation files
git checkout -- Controllers/ReviewController.cs
git checkout -- Services/Reviews/
git checkout -- ViewModels/Reviews/
git checkout -- Views/Review/

# Bước 2: Remove DI registration từ Program.cs
# (manual edit — xóa dòng builder.Services.AddScoped<IReviewService, ReviewService>();)

# Bước 3: Rebuild & verify
dotnet build
dotnet run

# Bước 4: Smoke test
# Truy cập trang chủ → kiểm tra hệ thống hoạt động bình thường
```

### 12.3. Notification Protocol

| Thời điểm         | Người nhận | Kênh      | Template                                    |
| -------------------- | ------------- | ---------- | ------------------------------------------- |
| Ngay khi phát hiện | Dev team      | Group chat | "🚨 ReviewModule incident: [mô tả]"       |
| Trong 30 phút       | Tech Lead     | Direct     | Báo cáo chi tiết và kế hoạch rollback |

### 12.4. Post-Incident Review (PIR)

Bắt buộc hoàn thành PIR document trong vòng 48 giờ sau khi incident được resolve.

PIR Template:

- **Timeline**: Diễn biến từng bước theo thứ tự thời gian
- **Root Cause**: Nguyên nhân gốc rễ (5 Whys)
- **Impact**: Số users ảnh hưởng, thời gian downtime
- **Remediation**: Các bước đã thực hiện để khắc phục
- **Prevention**: Action items để tránh tái diễn

---

## 13. Kịch bản Kiểm thử Chi tiết

**Policy (EDS v2.0 — Test Data)**: Mọi test scenario phải khai báo `Test Data Classification`.

- `SYNTHETIC` (bắt buộc mặc định) — dữ liệu giả hoàn toàn
- `ANONYMIZED` — nếu cần realistic data, phải anonymize trước
- ❌ **TUYỆT ĐỐI KHÔNG** dùng Production PII trong test cases

### 13.1. Unit Tests

**TC-UNIT-001 — Submit Review Happy Path**

```gherkin
Feature: Submit Review
  Background:
    Given test data classification: SYNTHETIC
    And Customer userId=1 có Booking bookingId=100 với status CheckedOut
    And Chưa có review nào cho bookingId=100 của userId=1

  Scenario: Submit review thành công
    Given Rating = 5, Comment = "Rất tuyệt vời"
    When CreateReviewAsync(userId=1, model) được gọi
    Then Kết quả trả về = true
    And DB chứa 1 record Review mới với Rating=5, IsVisible=true

  Scenario: Submit review thiếu Rating
    Given Rating = null
    When CreateReviewAsync được gọi
    Then Throw InvalidOperationException "REV-001"
```

**TC-UNIT-002 — Duplicate Review Prevention**

```gherkin
Feature: Prevent Duplicate Review
  Background:
    Given test data classification: SYNTHETIC

  Scenario: Customer đã review Booking này rồi
    Given Customer userId=1 đã có review cho bookingId=100
    When CreateReviewAsync(userId=1, bookingId=100) được gọi
    Then Throw InvalidOperationException "REV-004"
    And Không có record mới trong DB
```

**TC-UNIT-003 — Toggle Visibility**

```gherkin
Feature: Toggle Review Visibility
  Background:
    Given test data classification: SYNTHETIC

  Scenario: Ẩn review đang hiện
    Given Review reviewId=10 có IsVisible=true
    When ToggleVisibilityAsync(10) được gọi
    Then Review.IsVisible = false

  Scenario: Hiện review đang ẩn
    Given Review reviewId=10 có IsVisible=false
    When ToggleVisibilityAsync(10) được gọi
    Then Review.IsVisible = true
```

### 13.2. Integration Tests

**TC-INT-001 — Full flow Submit + Moderate**

```gherkin
Feature: End-to-End Review Flow
  Background:
    Given test data classification: SYNTHETIC

  Scenario: Customer submit review, Manager hide it
    Given DB có Booking(id=100, userId=1, status=CheckedOut)
    When Customer(userId=1) submit review(rating=4, comment="Tốt")
    Then DB chứa Review(bookingId=100, isVisible=true)
    When BusinessManager toggle visibility cho review mới
    Then DB Review.IsVisible = false
    When Guest truy cập PublicList
    Then Không thấy review vừa bị ẩn
```

### 13.3. E2E / Security Tests

**TC-E2E-001 — Unauthorized Access**

```gherkin
Feature: Security — Unauthorized Access
  Background:
    Given test data classification: SYNTHETIC

  Scenario: Guest truy cập Manage page
    Given user chưa đăng nhập (Guest)
    When GET /Review/Manage được gọi không có session
    Then response redirect đến Login page
    And danh sách reviews KHÔNG được trả về

  Scenario: Customer toggle visibility (Privilege Escalation)
    Given user đăng nhập với role Customer
    When POST /Review/ToggleVisibility với ReviewId=10
    Then response 403 Forbidden hoặc redirect
    And Review(id=10).IsVisible KHÔNG thay đổi

  Scenario: XSS injection trong Comment
    Given Customer đủ điều kiện submit review
    When submit review với Comment = "<script>alert('XSS')</script>"
    And Guest truy cập PublicList
    Then Comment được HTML-encode khi hiển thị
    And Không có script execution trên browser
```

---

## 14. Phương pháp Xác minh

### 14.1. Database Inspection

```sql
-- Verify review đã tạo thành công
SELECT ReviewId, UserId, Rating, Comment, IsVisible, CreatedAt
FROM Reviews
WHERE BookingId = 100 AND UserId = 1;

-- Verify toggle visibility
SELECT ReviewId, IsVisible
FROM Reviews
WHERE ReviewId = 10;

-- Verify không có duplicate
SELECT COUNT(*)
FROM Reviews
WHERE BookingId = 100 AND UserId = 1;
-- Expected: 1 (chỉ 1 record)

-- Verify public reviews chỉ trả về visible
SELECT COUNT(*)
FROM Reviews
WHERE IsVisible = 1;
```

### 14.2. Manual Verification Checklist

```
1. [ ] Đăng nhập Customer → Booking đã CheckedOut → Submit review → OK
2. [ ] Đăng nhập Customer → Booking chưa CheckedOut → Submit review → ERROR
3. [ ] Đăng nhập Customer → Booking đã review → Submit lại → ERROR (duplicate)
4. [ ] Guest truy cập PublicList → Chỉ thấy review IsVisible=true
5. [ ] Đăng nhập Business Manager → Manage → Toggle Hide → OK
6. [ ] Đăng nhập Business Manager → Manage → Toggle Show → OK
7. [ ] Đăng nhập Customer (không phải Manager) → Truy cập Manage → 403
8. [ ] Submit review với Comment chứa <script> → Kiểm tra XSS encode
```

---

## 15. Mẫu thử thực tế (API Verification Samples)

### 15.1. Happy Path — Submit Review

```
1. Đăng nhập tài khoản Customer (email: customer@test.com)
2. Truy cập: /Review/Submit?bookingId=100
3. Nhập Rating = 5, Comment = "Dịch vụ hoàn hảo!"
4. Bấm Submit
5. Expected: Redirect đến PublicList, hiển thị review mới
```

### 15.2. Happy Path — Moderate Review

```
1. Đăng nhập tài khoản Business Manager
2. Truy cập: /Review/Manage
3. Tìm review cần ẩn → Bấm "Hide"
4. Expected: Review biến mất khỏi PublicList
5. Bấm "Show" để khôi phục
6. Expected: Review xuất hiện lại trên PublicList
```

### 15.3. Error Paths

```
1. Submit review khi Booking chưa CheckedOut:
   → Expected: Error "Dịch vụ chưa hoàn thành" (REV-003)

2. Submit review khi đã review rồi:
   → Expected: Error "Bạn đã đánh giá dịch vụ này rồi" (REV-004)

3. Guest (chưa đăng nhập) truy cập /Review/Submit:
   → Expected: Redirect đến Login page

4. Customer truy cập /Review/Manage:
   → Expected: 403 Forbidden hoặc Redirect

5. Submit review với Rating=0:
   → Expected: Error "Rating phải từ 1 đến 5" (REV-001)

6. Submit review thiếu BookingId và TourId:
   → Expected: Error "Phải cung cấp BookingId hoặc TourId" (REV-002)
```

---

## 16. Bảng tổng hợp phân quyền (Authorization Matrix)

**Nguyên tắc Least Privilege**: Mỗi Role chỉ có quyền tối thiểu cần thiết để thực hiện nhiệm vụ của mình.

| Endpoint                         | GUEST | CUSTOMER | BUSINESS MANAGER | ADMIN |
| -------------------------------- | ----- | -------- | ---------------- | ----- |
| GET`/Review/PublicList`        | ✅    | ✅       | ✅               | ✅    |
| GET`/Review/Submit`            | ❌    | ✅       | ❌               | ❌    |
| POST`/Review/Submit`           | ❌    | ✅       | ❌               | ❌    |
| GET`/Review/Manage`            | ❌    | ❌       | ✅               | ✅    |
| POST`/Review/ToggleVisibility` | ❌    | ❌       | ✅               | ✅    |
| GET`/Review/Details/{id}`      | ❌    | ❌       | ✅               | ✅    |

Chú thích:

- ✅ = Được phép
- ❌ = Bị từ chối (Redirect to Login hoặc 403)

---

## PHỤ LỤC

### A. Glossary (Thuật ngữ)

| Thuật ngữ      | Định nghĩa                                                                                 |
| ---------------- | --------------------------------------------------------------------------------------------- |
| Review           | Đánh giá của khách hàng bao gồm rating (1-5 sao) và bình luận văn bản             |
| Rating           | Điểm đánh giá theo thang 1-5 sao                                                         |
| IsVisible        | Cờ boolean xác định review có hiển thị công khai hay không                           |
| Moderate         | Hành vi kiểm duyệt nội dung review bởi Business Manager                                  |
| Soft Hide        | Ẩn review bằng cách toggle flag, không xóa dữ liệu khỏi database                      |
| Eligibility      | Điều kiện đủ để khách hàng được phép submit review (dịch vụ đã hoàn thành) |
| Duplicate Review | Review trùng lặp — cùng user, cùng Booking/Tour                                          |
| PII              | Personally Identifiable Information                                                           |
| DPO              | Data Protection Officer                                                                       |

### B. Tài liệu tham chiếu

| Document                     | Link / Path                               |
| ---------------------------- | ----------------------------------------- |
| SRS — Project Specification | `01_SRS/SWP391-HOS-02.md`               |
| SRS — Nhóm 1               | `01_SRS/SWP391_NHOM1.md`                |
| SRS — Review Module         | `01_SRS/Review_SRS.md`                  |
| Tour Booking EDS             | `03_implement/TourBooking_EDS.md`       |
| EDS Template v2.0            | `template/EDS_TEMPLATE_V2.0.md`         |
| TDD Template v1.0            | `template/TDD_TEMPLATE_V1.md`           |
| Review Model                 | `Models/Review.cs`                      |
| Review Configuration         | `Configurations/ReviewConfiguration.cs` |

> *EDS v2.0 — Áp dụng cho module Review & Rating.*
> *Các sections đánh dấu ⭐ là bổ sung mới so với EDS v1.0.*
