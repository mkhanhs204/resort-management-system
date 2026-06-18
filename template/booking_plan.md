# Cải tiến tính năng Đặt Phòng (Multi-Room & Multi-Type Booking)

Mục tiêu của kế hoạch này là nâng cấp hệ thống đặt phòng hiện tại từ mô hình 1 booking - 1 phòng cụ thể sang mô hình 1 booking - nhiều phòng/nhiều loại phòng, đồng thời **giao quyền gán phòng cụ thể (Room Assignment) cho Lễ tân**, thay vì tự động gán ngay lúc khách hàng đặt.

## Câu hỏi mở (Cần xác nhận lần cuối)

> [!TIP]
> 1. **Giao diện Lễ Tân (Receptionist)**: Sau khi hoàn thành phần cho khách hàng (Customer) đặt được nhiều loại phòng, chúng ta sẽ cần phải xây dựng/cập nhật thêm một giao diện cho Lễ tân để họ có thể xem danh sách các Booking chưa được gán phòng và tiến hành gán `RoomId` cụ thể trước khi khách Check-in. Bạn có muốn tôi thiết kế luôn phần việc đó trong Phase tiếp theo không?

## Proposed Changes (Các thay đổi đề xuất)

### 1. Database & Models

Để khách hàng book số lượng phòng theo loại và cho phép lễ tân gán phòng sau, bảng `BookingDetail` sẽ đại diện cho "1 slot phòng". Nếu khách đặt 2 phòng Standard, hệ thống sẽ tạo ra 2 bản ghi `BookingDetail` có `RoomId` bằng `null`.

*   **[NEW] `Models/BookingDetail.cs`**
    *   Tạo class `BookingDetail` gồm các properties: 
        *   `BookingDetailId` (PK)
        *   `BookingId` (FK)
        *   `RoomCategoryId` (FK) - Loại phòng khách đặt.
        *   `RoomId` (FK, Nullable) - Sẽ là `null` lúc đặt, và được Lễ tân gán số phòng cụ thể sau.
        *   `Price` (decimal) - Giá phòng 1 đêm tại thời điểm đặt.
*   **[MODIFY] `Models/Booking.cs`**
    *   Xóa bỏ property `RoomId` và `Room`.
    *   Thêm `public ICollection<BookingDetail> BookingDetails { get; set; }`.
*   **[MODIFY] `Models/Room.cs`**
    *   Đổi `ICollection<Booking> Bookings` thành `ICollection<BookingDetail> BookingDetails`.
*   **[MODIFY] `Data/ResortDbContext.cs`**
    *   Thêm `DbSet<BookingDetail> BookingDetails`.
    *   Cập nhật các ràng buộc quan hệ bằng Fluent API (nếu cần).

*(Lưu ý: Chạy Entity Framework Migration để cập nhật database sau khi đổi Model).*

---

### 2. ViewModels

*   **[NEW] `ViewModels/Bookings/RoomSelectionViewModel.cs`**
    *   Tạo class chứa `CategoryId` (int) và `Quantity` (int).
*   **[MODIFY] `ViewModels/Bookings/CreateBookingViewModel.cs`**
    *   Xóa `RoomId`.
    *   Thêm `List<RoomSelectionViewModel> RoomSelections`.

---

### 3. Services (Logic xử lý)

*   **[MODIFY] `Services/Bookings/BookingService.cs`**
    *   **Logic Check Trống**: Thay vì check xem phòng X có trống không, hệ thống sẽ gom nhóm và tính **tổng số phòng trống** của loại phòng Y trong ngày khách chọn. Nếu tổng phòng trống < Số lượng khách muốn đặt, thì báo lỗi.
    *   **Logic Lưu Database (`CreateBookingAsync`)**: 
        *   Tạo đối tượng `Booking`.
        *   Duyệt qua danh sách `RoomSelections`. Ví dụ khách chọn loại phòng Standard số lượng 2.
        *   Tạo ra 2 đối tượng `BookingDetail` với `RoomCategoryId = ID_Standard`, `RoomId = null`, và `Price` là giá của loại phòng Standard.
        *   Tính `TotalAmount` và `DepositAmount`.
        *   Lưu vào Database.
    *   Sửa đổi hàm `CancelBookingAsync` để lấy danh sách phòng từ `BookingDetails` thay vì trực tiếp từ Booking.

---

### 4. Controllers & Views (UI/UX)

*   **[MODIFY] `Views/Home/SearchRooms.cshtml`**
    *   Bọc danh sách các loại phòng bằng một `<form>` POST.
    *   Tại mỗi `RoomCategory` hiển thị, thêm bộ chọn Số lượng (từ 0 -> X phòng).
    *   Thêm nút "Tiến hành Đặt Phòng" ở cuối trang thay vì nút "Đặt ngay" cho từng phòng riêng lẻ.
*   **[MODIFY] `Controllers/BookingsController.cs`**
    *   Action `Create` (GET & POST) sẽ tiếp nhận danh sách các phòng khách chọn (`RoomSelections`), lấy thông tin chi tiết loại phòng (chưa quan tâm phòng cụ thể) để hiển thị.
*   **[MODIFY] `Views/Bookings/Create.cshtml`**
    *   Giao diện Checkout (Tóm tắt phòng đặt) sẽ loop qua danh sách các loại phòng và số lượng khách đã chọn, tính tổng tiền.
    *   Gỡ bỏ các thông tin liên quan đến Số phòng cụ thể (Room Number) ra khỏi giao diện khách hàng.

## Verification Plan (Kế hoạch kiểm thử)

### Manual Verification
1.  **Giao diện Đặt phòng**: User vào Search, chọn 2 phòng Standard và 1 phòng Deluxe.
2.  **Checkout**: Trang `Create` hiển thị thông tin 3 phòng, không có số phòng cụ thể.
3.  **Database Check**: Hoàn thành thanh toán. Kiểm tra DB có 1 record `Booking`, và 3 record `BookingDetail` (2 record Category Standard, 1 record Category Deluxe), và tất cả cột `RoomId` đều đang là `NULL`.
4.  **Validation**: Test thử trường hợp Resort chỉ còn 2 phòng Standard trống, nhưng khách cố tình nhập số lượng 3 -> Hệ thống báo hết phòng.
