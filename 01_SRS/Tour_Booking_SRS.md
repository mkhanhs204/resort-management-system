# Software Requirements Specification (SRS) - Tour Booking Module
**Project:** PhucNguyen Resort & Tour Hub (SWP391-HOS-02)
**Module:** Tour Booking System (Module 4)

---

## 1. Overview
Module **Tour Booking** cho phép khách hàng (Guest/Customer) tìm kiếm, xem chi tiết và đặt các tour du lịch/hoạt động giải trí ngay trong khuôn viên hoặc các địa điểm lân cận resort. Đồng thời, module này hỗ trợ nhân viên (Tour Guide, Admin) quản lý danh sách người tham gia (manifest) và tích hợp liền mạch với hệ thống thanh toán và hóa đơn tổng hợp (Consolidated Billing) của resort.

---

## 2. Actors (Đối tượng sử dụng)
| Actor | Mô tả |
| :--- | :--- |
| **Guest** | Người dùng chưa đăng nhập. Có thể xem danh sách tour, chi tiết hành trình, lịch trình dự kiến và thời tiết. |
| **Customer** | Khách hàng đã đăng nhập và đang lưu trú. Có thể đặt tour, chọn số lượng người tham gia, chọn phương thức thanh toán (thanh toán ngay hoặc Charge to Room). |
| **Tour Guide** | Hướng dẫn viên phụ trách tour. Có thể xem danh sách khách hàng tham gia (Manifest) của các tour được phân công trong ngày. |
| **Receptionist** | Lễ tân kiểm tra các khoản nợ từ Tour Booking để gộp vào hóa đơn tổng hợp (Consolidated Invoice) khi khách check-out. |
| **Admin / Manager** | Quản lý danh mục tour, lên lịch trình (Schedule) cho các tour, và xem thống kê doanh thu. |

---

## 3. Use Cases (Yêu cầu chức năng)

### UC11 / UC15: View Tours (Xem danh sách Tour)
- **Actor:** Guest, Customer
- **Mô tả:** Người dùng xem danh sách các tour hiện có của resort. Thông tin hiển thị bao gồm hình ảnh, tên tour, mô tả ngắn, giá cả, và sức chứa tối đa. 
- **Tính năng mở rộng:** Hiển thị bản đồ (Map API) và dự báo thời tiết (Weather API) cho các tour hoạt động ngoài trời.

### UC12: View Tour Details (Xem chi tiết Tour)
- **Actor:** Guest, Customer
- **Mô tả:** Người dùng click vào một tour cụ thể để xem chi tiết bao gồm: Hành trình (Itinerary), thời lượng (DurationHours), địa điểm (Location), và các lịch khởi hành sắp tới (TourSchedules).

### UC16 / UC26: Book Tour (Đặt Tour)
- **Actor:** Customer
- **Mô tả:** Khách hàng chọn một lịch trình (Tour Schedule), nhập số lượng người tham gia và tiến hành đặt tour.
- **Quy trình thanh toán:** Khách hàng có 2 lựa chọn:
  1. **Pay Immediately:** Thanh toán ngay qua cổng thanh toán (VNPay / Stripe).
  2. **Charge to Room:** Ghi nợ vào hóa đơn phòng (Folio). Lựa chọn này chỉ khả dụng nếu khách hàng đã có một Room Booking đang ở trạng thái Checked-In.
- **Ràng buộc:** Số lượng `AvailableSlots` của lịch trình đó phải lớn hơn hoặc bằng số lượng người tham gia. Hệ thống phải cập nhật lại `AvailableSlots` sau khi đặt thành công.

### UC17: View Tour Manifest (Xem danh sách người tham gia)
- **Actor:** Tour Guide
- **Mô tả:** Hướng dẫn viên đăng nhập và xem danh sách chi tiết (Tên khách hàng, số lượng người, thông tin liên hệ, trạng thái thanh toán) của những khách hàng đã đặt tour mà họ được phân công trong ngày.

---

## 4. Business Rules (Quy tắc nghiệp vụ)

- **BR-TOUR-01 (Capacity Limits):** Không cho phép đặt tour nếu số lượng người tham gia vượt quá `AvailableSlots` của lịch trình (`TourSchedule`). Cơ chế Pessimistic Locking hoặc Optimistic Locking phải được áp dụng để tránh Overbooking.
- **BR-TOUR-02 (Charge to Room):** Để sử dụng tính năng "Charge to Room", tài khoản khách hàng phải được liên kết với một `BookingId` hợp lệ (phòng đang được sử dụng).
- **BR-TOUR-03 (Checkout Constraint):** Khách hàng không thể Check-Out (UC09) nếu còn các khoản nợ Tour Booking chưa thanh toán. Các khoản này phải được đẩy vào Consolidated Invoice (UC20).
- **BR-TOUR-04 (Cancellation Policy):** Việc hủy đặt tour phải tuân thủ chính sách hoàn tiền của resort (ví dụ: hủy trước 48h được hoàn tiền, hủy trong vòng 48h không được hoàn tiền) dựa trên Vietnam Law on Tourism 2017.

---

## 5. Database Schema (Cấu trúc dữ liệu liên quan)

Module này giao tiếp trực tiếp với 3 bảng chính trong Database:

### 1. Bảng `Tours`
Lưu trữ danh mục gốc của các tour.
- `TourId` (PK)
- `TourName`, `Description`, `Price`, `MaxCapacity`, `DurationHours`, `Location`, `ImageUrl`

### 2. Bảng `TourSchedules`
Lưu trữ lịch khởi hành cụ thể của từng tour.
- `ScheduleId` (PK)
- `TourId` (FK) -> `Tours.TourId`
- `GuideId` (FK) -> `Users.UserId` (Người hướng dẫn)
- `TourDate` (Ngày khởi hành)
- `StartTime` (Giờ bắt đầu)
- `AvailableSlots` (Số chỗ còn trống hiện tại)

### 3. Bảng `TourBookings`
Lưu trữ giao dịch đặt tour của khách hàng.
- `TourBookingId` (PK)
- `UserId` (FK) -> `Users.UserId` (Người đặt)
- `ScheduleId` (FK) -> `TourSchedules.ScheduleId`
- `BookingId` (FK, Nullable) -> `Bookings.BookingId` (Dùng cho "Charge to Room")
- `Participants` (Số lượng người tham gia)
- `TotalAmount` (Tổng tiền = Participants * Tour.Price)
- `BookingStatus` (Enum: Pending, Confirmed, Cancelled, Completed)

---
*Tài liệu được trích xuất dựa trên SWP391_NHOM1.md và SWP391-HOS-02.md.*
