# SOFTWARE REQUIREMENTS SPECIFICATION — Review Module

## PhucNguyen Resort & Tour Hub
**Hệ thống Quản lý Review & Đánh giá Dịch vụ**

| Field            | Value                                |
| ---------------- | ------------------------------------ |
| Document ID      | `PHUCNGUYEN-REVIEW-SRS-001`         |
| Version          | 1.0                                  |
| Date             | 2026-06-28                           |
| Status           | Draft                                |
| Author           | Nguyễn Khắc Hiếu                   |
| Reviewed by      | truongcuong                          |
| Approved by      | Nguyễn Khắc Hiếu                   |
| Classification   | Internal                             |

## CHANGELOG

| Ngày       | Người thực hiện | Nội dung thay đổi                                       |
| ---------- | --------------- | -------------------------------------------------------- |
| 2026-06-28 | AI              | Khởi tạo tài liệu SRS cho module Review & Rating        |

## MỤC LỤC

1. Giới thiệu
2. Mô tả tổng quan
3. Các tác nhân (Actors)
4. Yêu cầu chức năng (Functional Requirements)
5. Business Rules
6. Activity Diagrams
7. Screen Specifications
8. Yêu cầu phi chức năng (Non-Functional Requirements)
9. Ràng buộc hệ thống

---

## 1. Giới thiệu

### 1.1. Mục đích

Tài liệu này mô tả chi tiết yêu cầu phần mềm (Software Requirements Specification) cho module **Review & Rating** trong hệ thống PhucNguyen Resort & Tour Hub. Module này cho phép khách hàng đánh giá dịch vụ resort (phòng, tour) và cho phép Business Manager kiểm duyệt, quản lý các đánh giá.

### 1.2. Phạm vi

Module Review & Rating bao gồm các chức năng chính:

- **Submit Review**: Khách hàng gửi đánh giá sau khi sử dụng dịch vụ
- **Rate Resort Services**: Hệ thống hỗ trợ đánh giá theo thang điểm 5 sao cho cả dịch vụ phòng (Room) và tour
- **Moderate Reviews**: Business Manager kiểm duyệt nội dung đánh giá
- **Hide/Show Reviews**: Business Manager ẩn/hiện đánh giá không phù hợp

### 1.3. Tham chiếu

| Document                          | Mô tả                                           |
| --------------------------------- | ------------------------------------------------ |
| `SWP391-HOS-02.md`               | Project Specification — UC18, UC19               |
| `SWP391_NHOM1.md`                | SRS Nhóm 1 — UC14, UC29, UC47, UC48, UC49       |
| `Review_EDS.md`                  | Engineering Documentation Standard               |
| `Review_TDD.md`                  | Test-Driven Development Specification            |

### 1.4. Thuật ngữ

| Thuật ngữ        | Định nghĩa                                                                              |
| ----------------- | ---------------------------------------------------------------------------------------- |
| Review            | Đánh giá của khách hàng bao gồm rating (1-5 sao) và bình luận văn bản                  |
| Rating            | Điểm đánh giá theo thang 1-5 sao                                                        |
| IsVisible         | Cờ boolean xác định review có hiển thị công khai hay không                               |
| Moderate          | Hành vi kiểm duyệt nội dung review bởi Business Manager                                 |
| Soft Hide         | Ẩn review bằng cách toggle flag, không xóa dữ liệu khỏi database                       |
| Eligibility       | Điều kiện đủ để khách hàng được phép submit review (dịch vụ đã hoàn thành)              |

---

## 2. Mô tả tổng quan

### 2.1. Bối cảnh hệ thống

Module Review & Rating thuộc **Module 4: Tour Booking & Feedback System** (theo SWP391-HOS-02.md). Module này phục vụ hai mục đích chính:

1. **Cho khách hàng**: Cung cấp kênh phản hồi chất lượng dịch vụ sau khi sử dụng
2. **Cho Business Manager**: Cung cấp công cụ quản lý và kiểm duyệt nội dung phản hồi

### 2.2. Mối quan hệ với các module khác

```
┌─────────────────┐     ┌──────────────────┐     ┌─────────────────────┐
│  AuthModule     │     │ RoomBookingModule│     │ TourBookingModule   │
│ (Xác thực user) │────▶│ (Kiểm tra status │────▶│ (Kiểm tra status    │
│                 │     │  Booking)        │     │  TourBooking)        │
└─────────────────┘     └──────────────────┘     └─────────────────────┘
         │                       │                         │
         └───────────────────────┼─────────────────────────┘
                                 ▼
                    ┌─────────────────────┐
                    │   ReviewModule      │
                    │ (Submit, Moderate,  │
                    │  Hide/Show Reviews) │
                    └─────────┬───────────┘
                              │
                              ▼
                    ┌─────────────────────┐
                    │    HomeModule       │
                    │ (Hiển thị review   │
                    │  công khai)         │
                    └─────────────────────┘
```

### 2.3. Giả định và ràng buộc

- Dự án sử dụng **C#, MVC .NET 8, SQL Server**
- Model `Review` đã tồn tại trong codebase
- Hệ thống authentication đã được implement (role-based)
- Module **không** tạo migration mới — sử dụng bảng `Reviews` đã có sẵn

---

## 3. Các tác nhân (Actors)

| #  | Actor            | Vai trò trong module Review                                                                 |
| -- | ---------------- | -------------------------------------------------------------------------------------------- |
| 1  | Guest            | Xem danh sách review công khai (không cần đăng nhập)                                        |
| 2  | Customer         | Gửi đánh giá (rating + comment) sau khi hoàn thành dịch vụ (check-out phòng / tour xong)   |
| 3  | Business Manager | Xem tất cả review, kiểm duyệt nội dung, ẩn/hiện review không phù hợp                      |
| 4  | Admin            | Có quyền tương tự Business Manager đối với chức năng review                                  |

---

## 4. Yêu cầu chức năng (Functional Requirements)

### FR-REV-001 — View Public Reviews (Xem Review Công khai)

**Use Case tham chiếu**: UC14 (SWP391_NHOM1.md)

| Field              | Value                                                                          |
| ------------------ | ------------------------------------------------------------------------------ |
| ID                 | FR-REV-001                                                                     |
| Tên                | View Public Reviews                                                            |
| Actor              | Guest, Customer, Business Manager, Admin                                       |
| Mô tả             | Người dùng (bao gồm Guest chưa đăng nhập) có thể xem danh sách review công khai |
| Điều kiện tiên quyết | Không có                                                                    |
| Luồng chính        | Xem bên dưới                                                                  |
| Kết quả            | Hiển thị danh sách review có `IsVisible = true`                               |

**Luồng chính (Main Flow)**:

1. Người dùng truy cập trang Review công khai (`/Review/PublicList`)
2. Hệ thống truy vấn tất cả review có `IsVisible = true`
3. Hệ thống hiển thị danh sách review bao gồm:
   - Tên người đánh giá (UserName)
   - Rating (số sao)
   - Bình luận (Comment)
   - Loại dịch vụ (Room / Tour)
   - Tên dịch vụ (Room Category / Tour Name)
   - Ngày đánh giá (CreatedAt)
4. Danh sách sắp xếp theo ngày đánh giá mới nhất

**Luồng thay thế**:

- 1a. Không có review nào: Hiển thị thông báo "Chưa có đánh giá nào"

---

### FR-REV-002 — Submit Review (Gửi Đánh giá)

**Use Case tham chiếu**: UC18 (SWP391-HOS-02.md), UC29 (SWP391_NHOM1.md)

| Field                  | Value                                                                          |
| ---------------------- | ------------------------------------------------------------------------------ |
| ID                     | FR-REV-002                                                                     |
| Tên                    | Submit Review                                                                  |
| Actor                  | Customer                                                                       |
| Mô tả                 | Customer gửi đánh giá (rating 1-5 sao + bình luận) sau khi hoàn thành dịch vụ |
| Điều kiện tiên quyết   | Customer đã đăng nhập; Booking đã CheckedOut HOẶC TourBooking đã Completed    |
| Luồng chính            | Xem bên dưới                                                                  |
| Kết quả                | Review được tạo mới trong DB với `IsVisible = true`                           |

**Luồng chính (Main Flow)**:

1. Customer truy cập trang Submit Review (`/Review/Submit`)
2. Hệ thống kiểm tra Customer đã đăng nhập
3. Hệ thống hiển thị form đánh giá bao gồm:
   - Rating (bắt buộc): chọn 1-5 sao
   - Comment (tùy chọn): nhập bình luận tối đa 2000 ký tự
   - Thông tin dịch vụ: BookingId hoặc TourId (ẩn, truyền qua query string)
4. Customer nhập rating và comment (nếu có)
5. Customer bấm nút "Gửi đánh giá"
6. Hệ thống validate dữ liệu:
   - Rating nằm trong khoảng [1, 5]
   - BookingId hoặc TourId phải được cung cấp (không thể cả hai đều null)
   - Comment không vượt quá 2000 ký tự
7. Hệ thống kiểm tra điều kiện đủ (Eligibility):
   - **Review phòng**: `Booking.BookingStatus == CheckedOut`
   - **Review tour**: `TourBooking.BookingStatus == Completed`
8. Hệ thống kiểm tra trùng lặp (Duplicate):
   - Chưa có review nào từ cùng UserId cho cùng BookingId/TourId
9. Hệ thống tạo record `Review` mới với `IsVisible = true`
10. Hệ thống chuyển hướng Customer đến trang Public List với thông báo thành công

**Luồng thay thế (Alternative Flows)**:

- 2a. Customer chưa đăng nhập: Chuyển hướng đến trang Login
- 6a. Rating ngoài khoảng [1, 5]: Hiển thị lỗi "Rating phải từ 1 đến 5" (REV-001)
- 6b. Thiếu cả BookingId và TourId: Hiển thị lỗi "Phải cung cấp BookingId hoặc TourId" (REV-002)
- 6c. Comment vượt quá 2000 ký tự: Hiển thị lỗi "Bình luận không vượt quá 2000 ký tự" (REV-007)
- 7a. Dịch vụ chưa hoàn thành: Hiển thị lỗi "Dịch vụ chưa hoàn thành, không thể đánh giá" (REV-003)
- 8a. Đã có review trùng: Hiển thị lỗi "Bạn đã đánh giá dịch vụ này rồi" (REV-004)

---

### FR-REV-003 — Rate Resort Services (Đánh giá Dịch vụ Resort)

**Use Case tham chiếu**: UC18 (SWP391-HOS-02.md), UC29 (SWP391_NHOM1.md)

| Field                  | Value                                                          |
| ---------------------- | -------------------------------------------------------------- |
| ID                     | FR-REV-003                                                     |
| Tên                    | Rate Resort Services                                           |
| Actor                  | Customer                                                       |
| Mô tả                 | Hệ thống hỗ trợ đánh giá theo thang 5 sao cho 2 loại dịch vụ |
| Điều kiện tiên quyết   | Tương tự FR-REV-002                                           |
| Kết quả                | Rating được lưu trong record Review                            |

**Các loại dịch vụ được đánh giá**:

| Loại dịch vụ | Điều kiện đánh giá                           | Trường liên kết   |
| ------------- | --------------------------------------------- | ------------------- |
| Room (Phòng) | `Booking.BookingStatus == CheckedOut`         | `Review.BookingId` |
| Tour          | `TourBooking.BookingStatus == Completed`      | `Review.TourId`    |

**Hệ thống Rating**:

| Số sao | Ý nghĩa    |
| ------ | ----------- |
| 1 ⭐    | Rất tệ     |
| 2 ⭐⭐  | Tệ         |
| 3 ⭐⭐⭐ | Bình thường |
| 4 ⭐⭐⭐⭐ | Tốt        |
| 5 ⭐⭐⭐⭐⭐ | Tuyệt vời |

**Ràng buộc**:

- Mỗi review chỉ gắn với **một** loại dịch vụ (hoặc BookingId hoặc TourId, không cùng lúc)
- Rating là trường bắt buộc, phải là số nguyên trong khoảng [1, 5]
- Comment là trường tùy chọn, tối đa 2000 ký tự

---

### FR-REV-004 — Moderate Reviews (Kiểm duyệt Đánh giá)

**Use Case tham chiếu**: UC47 (SWP391_NHOM1.md), UC19 (SWP391-HOS-02.md)

| Field                  | Value                                                               |
| ---------------------- | ------------------------------------------------------------------- |
| ID                     | FR-REV-004                                                          |
| Tên                    | Moderate Reviews                                                    |
| Actor                  | Business Manager, Admin                                             |
| Mô tả                 | Business Manager xem và quản lý tất cả review (bao gồm đã ẩn)     |
| Điều kiện tiên quyết   | User đã đăng nhập với role Business Manager hoặc Admin             |
| Luồng chính            | Xem bên dưới                                                       |
| Kết quả                | Hiển thị danh sách tất cả review với các hành động quản lý         |

**Luồng chính (Main Flow)**:

1. Business Manager truy cập trang Manage Reviews (`/Review/Manage`)
2. Hệ thống kiểm tra role (phải là Business Manager hoặc Admin)
3. Hệ thống hiển thị danh sách **tất cả** review (bao gồm cả `IsVisible = false`)
4. Mỗi review hiển thị:
   - Tên người đánh giá
   - Rating
   - Bình luận
   - Trạng thái hiển thị (Visible / Hidden)
   - Loại dịch vụ (Room / Tour)
   - Ngày đánh giá
   - Nút hành động: "Hide" (nếu đang Visible) hoặc "Show" (nếu đang Hidden)
5. Business Manager có thể xem chi tiết review (`/Review/Details/{id}`)

**Luồng thay thế**:

- 2a. User không có role phù hợp: Redirect đến Login hoặc trả về 403 Forbidden
- 3a. Không có review nào: Hiển thị thông báo "Chưa có đánh giá nào"

---

### FR-REV-005 — Hide/Show Reviews (Ẩn/Hiện Đánh giá)

**Use Case tham chiếu**: UC49 (SWP391_NHOM1.md), UC19 (SWP391-HOS-02.md)

| Field                  | Value                                                                    |
| ---------------------- | ------------------------------------------------------------------------ |
| ID                     | FR-REV-005                                                               |
| Tên                    | Hide/Show Reviews                                                        |
| Actor                  | Business Manager, Admin                                                  |
| Mô tả                 | Business Manager toggle trạng thái hiển thị (ẩn/hiện) của review        |
| Điều kiện tiên quyết   | User đã đăng nhập với role Business Manager hoặc Admin; Review tồn tại  |
| Luồng chính            | Xem bên dưới                                                            |
| Kết quả                | `Review.IsVisible` được toggle (true ↔ false)                           |

**Luồng chính — Hide Review**:

1. Business Manager ở trang Manage Reviews
2. Business Manager bấm nút "Hide" trên review đang hiển thị (`IsVisible = true`)
3. Hệ thống gửi request POST `/Review/ToggleVisibility` với `ReviewId`
4. Hệ thống kiểm tra role (Business Manager / Admin)
5. Hệ thống tìm review theo `ReviewId`
6. Hệ thống cập nhật `IsVisible = false` (Soft Hide — không xóa record)
7. Hệ thống chuyển hướng về trang Manage với thông báo thành công
8. Review không còn hiển thị trên trang Public List

**Luồng chính — Show Review**:

1. Business Manager ở trang Manage Reviews
2. Business Manager bấm nút "Show" trên review đang ẩn (`IsVisible = false`)
3. Hệ thống gửi request POST `/Review/ToggleVisibility` với `ReviewId`
4. Hệ thống kiểm tra role (Business Manager / Admin)
5. Hệ thống tìm review theo `ReviewId`
6. Hệ thống cập nhật `IsVisible = true`
7. Hệ thống chuyển hướng về trang Manage với thông báo thành công
8. Review xuất hiện lại trên trang Public List

**Luồng thay thế**:

- 4a. User không có role phù hợp: Redirect hoặc 403 Forbidden (REV-006)
- 5a. `ReviewId` không tồn tại: Hiển thị lỗi "Không tìm thấy đánh giá" (REV-005)

---

### FR-REV-006 — View Customer Reviews (Business Manager)

**Use Case tham chiếu**: UC48 (SWP391_NHOM1.md)

| Field                  | Value                                                         |
| ---------------------- | ------------------------------------------------------------- |
| ID                     | FR-REV-006                                                    |
| Tên                    | View Customer Reviews                                         |
| Actor                  | Business Manager                                              |
| Mô tả                 | Business Manager xem chi tiết từng review để đánh giá nội dung|
| Điều kiện tiên quyết   | User có role Business Manager hoặc Admin                     |
| Luồng chính            | Truy cập `/Review/Details/{id}` để xem chi tiết review       |
| Kết quả                | Hiển thị thông tin chi tiết của review                        |

---

## 5. Business Rules

### BR-REV-001 — Điều kiện đủ để Submit Review (Eligibility)

> Chỉ Customer đã hoàn thành dịch vụ (CheckedOut cho Room, Completed cho Tour) mới được phép gửi đánh giá.

**Chi tiết**:

| Loại dịch vụ | Trạng thái yêu cầu                       | Trường kiểm tra                  |
| ------------- | ----------------------------------------- | --------------------------------- |
| Room          | `Booking.BookingStatus == CheckedOut`     | `Bookings.BookingStatus`         |
| Tour          | `TourBooking.BookingStatus == Completed`  | `TourBookings.BookingStatus`     |

**Lý do**: Đảm bảo mọi đánh giá đều đến từ khách hàng đã thực sự sử dụng dịch vụ, ngăn chặn review spam hoặc đánh giá không chính xác.

### BR-REV-002 — Chống Review Trùng lặp (Duplicate Prevention)

> Mỗi Customer chỉ được phép gửi tối đa 1 review cho mỗi Booking hoặc TourBooking.

**Kiểm tra**: Tại tầng Service (application-level) — kiểm tra xem đã tồn tại review với cùng `(UserId, BookingId)` hoặc `(UserId, TourId)` chưa.

### BR-REV-003 — Phạm vi Rating

> Rating phải là số nguyên trong khoảng [1, 5].

**Validation**: Model validation sử dụng `[Range(1, 5)]` attribute.

### BR-REV-004 — Mặc định Hiển thị & Kiểm duyệt

> Review mặc định có `IsVisible = true` khi được tạo. Business Manager có thể toggle giá trị này.

**Chiến lược**: Sử dụng Soft Hide (toggle `IsVisible` flag) thay vì Hard Delete để:
- Giữ nguyên dữ liệu lịch sử
- Cho phép khôi phục review đã ẩn
- Hỗ trợ audit trail

### BR-REV-005 — Một Review chỉ gắn với một loại dịch vụ

> Mỗi review chỉ được liên kết với hoặc `BookingId` HOẶC `TourId`, không được cả hai cùng lúc.

**Validation**: Kiểm tra tại tầng Service — nếu cả `BookingId` và `TourId` đều null → reject (REV-002). Nếu cả hai đều có giá trị → reject.

---

## 6. Activity Diagrams

### 6.1. Submit Review Process

```
┌─────────────┐
│   START     │
└──────┬──────┘
       ▼
┌──────────────────┐
│ Customer truy cập│
│ trang Submit     │
│ Review           │
└──────┬───────────┘
       ▼
  ┌────────────┐     Không
  │ Đã đăng    │────────────► Redirect Login
  │ nhập?      │
  └────┬───────┘
       │ Có
       ▼
┌──────────────────┐
│ Hiển thị form    │
│ Rating + Comment │
└──────┬───────────┘
       ▼
┌──────────────────┐
│ Customer nhập    │
│ và bấm Submit    │
└──────┬───────────┘
       ▼
  ┌────────────┐     Không
  │ Validate   │────────────► Hiển thị lỗi
  │ input OK?  │               validation
  └────┬───────┘
       │ Có
       ▼
  ┌────────────┐     Không
  │ Dịch vụ đã│────────────► Lỗi REV-003
  │ hoàn thành?│              "Chưa hoàn thành"
  └────┬───────┘
       │ Có
       ▼
  ┌────────────┐     Có
  │ Đã review  │────────────► Lỗi REV-004
  │ rồi?       │              "Đã đánh giá rồi"
  └────┬───────┘
       │ Không
       ▼
┌──────────────────┐
│ Tạo Review mới   │
│ IsVisible = true │
└──────┬───────────┘
       ▼
┌──────────────────┐
│ Redirect đến     │
│ PublicList +      │
│ thông báo success│
└──────┬───────────┘
       ▼
┌─────────────┐
│    END      │
└─────────────┘
```

### 6.2. Moderate Reviews Process (Hide/Show)

```
┌─────────────┐
│   START     │
└──────┬──────┘
       ▼
┌──────────────────┐
│ Manager truy cập │
│ /Review/Manage   │
└──────┬───────────┘
       ▼
  ┌────────────┐     Không
  │ Có role    │────────────► 403 Forbidden
  │ Manager?   │
  └────┬───────┘
       │ Có
       ▼
┌──────────────────┐
│ Hiển thị tất cả  │
│ reviews (visible │
│ + hidden)        │
└──────┬───────────┘
       ▼
┌──────────────────┐
│ Manager chọn     │
│ Hide / Show      │
│ cho 1 review     │
└──────┬───────────┘
       ▼
  ┌────────────┐     Không
  │ Review     │────────────► Lỗi REV-005
  │ tồn tại?  │              "Không tìm thấy"
  └────┬───────┘
       │ Có
       ▼
┌──────────────────┐
│ Toggle           │
│ IsVisible        │
│ (true ↔ false)  │
└──────┬───────────┘
       ▼
┌──────────────────┐
│ Redirect về      │
│ /Review/Manage + │
│ thông báo success│
└──────┬───────────┘
       ▼
┌─────────────┐
│    END      │
└─────────────┘
```

---

## 7. Screen Specifications

### 7.1. Danh sách màn hình

| #  | Screen Name          | URL                          | Actor                          | Mô tả                              |
| -- | -------------------- | ---------------------------- | ------------------------------ | ------------------------------------ |
| 1  | Public Review List   | `/Review/PublicList`         | Guest, Customer, Manager, Admin| Xem review công khai                 |
| 2  | Submit Review        | `/Review/Submit`             | Customer                       | Form gửi đánh giá                   |
| 3  | Manage Reviews       | `/Review/Manage`             | Business Manager, Admin        | Quản lý / kiểm duyệt review        |
| 4  | Review Details       | `/Review/Details/{id}`       | Business Manager, Admin        | Xem chi tiết một review             |

### 7.2. Chi tiết từng màn hình

#### Screen 1: Public Review List

**Các thành phần giao diện**:

| #  | Element           | Type       | Mô tả                                      |
| -- | ----------------- | ---------- | -------------------------------------------- |
| 1  | Tiêu đề trang    | Text       | "Đánh giá của Khách hàng"                   |
| 2  | Danh sách review  | List/Card  | Mỗi review hiển thị: avatar, tên, sao, comment, ngày |
| 3  | Rating trung bình | Text       | Trung bình tổng số sao (tính từ visible reviews) |
| 4  | Filter loại dịch vụ | Dropdown | Lọc theo "Tất cả" / "Room" / "Tour"        |

#### Screen 2: Submit Review

**Các thành phần giao diện**:

| #  | Element           | Type       | Mô tả                                        | Validation                    |
| -- | ----------------- | ---------- | ---------------------------------------------- | ----------------------------- |
| 1  | Rating Stars      | Star Input | Chọn 1-5 sao (bắt buộc)                       | `[Range(1, 5)]`             |
| 2  | Comment           | Textarea   | Nhập bình luận (tùy chọn)                      | `[StringLength(2000)]`       |
| 3  | BookingId/TourId  | Hidden     | ID dịch vụ (truyền qua query string)           | Phải có ít nhất 1 giá trị    |
| 4  | Thông tin dịch vụ | Read-only  | Hiển thị tên Room Category / Tour Name         | —                            |
| 5  | Nút Submit        | Button     | "Gửi đánh giá"                                 | —                            |
| 6  | Error Messages    | Alert      | Hiển thị lỗi validation (nếu có)               | —                            |

#### Screen 3: Manage Reviews

**Các thành phần giao diện**:

| #  | Element           | Type       | Mô tả                                        |
| -- | ----------------- | ---------- | ---------------------------------------------- |
| 1  | Tiêu đề trang    | Text       | "Quản lý Đánh giá"                            |
| 2  | Bảng review       | Table      | Columns: #, Tên khách, Rating, Comment, Loại DV, Trạng thái, Ngày, Hành động |
| 3  | Badge trạng thái  | Badge      | "Visible" (xanh) / "Hidden" (đỏ)             |
| 4  | Nút Hide/Show     | Button     | Toggle visibility cho từng review              |
| 5  | Nút Details       | Link       | Xem chi tiết review                            |

#### Screen 4: Review Details

**Các thành phần giao diện**:

| #  | Element           | Type       | Mô tả                                        |
| -- | ----------------- | ---------- | ---------------------------------------------- |
| 1  | Thông tin review  | Card       | Tên khách, Rating, Comment đầy đủ, ngày tạo  |
| 2  | Trạng thái        | Badge      | Visible / Hidden                               |
| 3  | Nút Hide/Show     | Button     | Toggle visibility                              |
| 4  | Nút Quay lại      | Link       | Về trang Manage                                |

---

## 8. Yêu cầu phi chức năng (Non-Functional Requirements)

### 8.1. Performance

| Yêu cầu                          | Target     | Phương pháp đo |
| --------------------------------- | ---------- | --------------- |
| Submit review API response time   | < 500ms    | Load test       |
| Get reviews list response time    | < 300ms    | Load test       |
| Concurrent review submissions     | 100 req/s  | Load test       |

### 8.2. Security

| Yêu cầu                  | Target          | Phương pháp xác minh     |
| ------------------------- | --------------- | ------------------------- |
| Role-based access control | Least privilege | Authorization Matrix      |
| XSS prevention            | 100%            | HTML encode output        |
| SQL Injection prevention  | 100%            | Parameterized query (EF)  |
| CSRF protection           | 100%            | Anti-forgery token (MVC)  |

### 8.3. Data Integrity

| Yêu cầu                              | Target | Phương pháp xác minh     |
| -------------------------------------- | ------ | ------------------------- |
| 1 review per Booking/TourBooking       | 100%   | Service-level validation  |
| Rating value in [1, 5]                 | 100%   | Model + Service validation|
| No review data loss (soft hide only)   | RPO=0  | Transaction log           |

### 8.4. Usability

| Yêu cầu                              | Target       |
| -------------------------------------- | ------------ |
| Star rating input phải trực quan      | Star widget  |
| Form validation messages rõ ràng      | Tiếng Việt   |
| Responsive design                     | Mobile-first |

---

## 9. Ràng buộc hệ thống

### 9.1. Data Model — Bảng Reviews

```
┌─────────────────────────┐
│        Reviews          │
├─────────────────────────┤
│ ReviewId    : int       │ PK (Identity)
│ UserId      : int?      │ FK → Users(UserId), SET NULL on delete
│ TourId      : int?      │ FK → Tours(TourId), SET NULL on delete
│ BookingId   : int?      │ FK → Bookings(BookingId), SET NULL on delete
│ Rating      : int?      │ [Range(1, 5)]
│ Comment     : nvarchar? │ Max 2000 chars
│ IsVisible   : bit       │ Default: 1 (true)
│ CreatedAt   : datetime  │ Default: GETDATE()
└─────────────────────────┘
```

### 9.2. Authorization Matrix

| Endpoint                         | GUEST | CUSTOMER | BUSINESS MANAGER | ADMIN |
| -------------------------------- | ----- | -------- | ---------------- | ----- |
| GET `/Review/PublicList`         | ✅    | ✅       | ✅               | ✅    |
| GET `/Review/Submit`             | ❌    | ✅       | ❌               | ❌    |
| POST `/Review/Submit`            | ❌    | ✅       | ❌               | ❌    |
| GET `/Review/Manage`             | ❌    | ❌       | ✅               | ✅    |
| POST `/Review/ToggleVisibility`  | ❌    | ❌       | ✅               | ✅    |
| GET `/Review/Details/{id}`       | ❌    | ❌       | ✅               | ✅    |

### 9.3. Error Codes

| Code      | HTTP | Message (VI)                                        | Trigger                                        |
| --------- | ---- | ---------------------------------------------------- | ----------------------------------------------- |
| REV-001   | 400  | Rating phải nằm trong khoảng 1 đến 5               | Rating ngoài khoảng [1, 5]                     |
| REV-002   | 400  | Phải cung cấp BookingId hoặc TourId                | Cả BookingId và TourId đều null                |
| REV-003   | 403  | Dịch vụ chưa hoàn thành, không thể đánh giá        | Booking chưa CheckedOut / Tour chưa Completed  |
| REV-004   | 409  | Bạn đã đánh giá dịch vụ này rồi                    | Đã tồn tại review cho Booking/Tour của user    |
| REV-005   | 404  | Không tìm thấy đánh giá                             | ReviewId không tồn tại                         |
| REV-006   | 403  | Không đủ quyền thực hiện                            | User không có role Business Manager/Admin       |
| REV-007   | 400  | Bình luận không vượt quá 2000 ký tự                 | Comment.Length > 2000                           |

---

> **End of Review Module SRS** — Version 1.0
