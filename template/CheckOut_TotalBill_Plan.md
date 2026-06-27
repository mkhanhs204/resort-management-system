# Kế Hoạch Triển Khai Chức Năng Check-out & Quản Lý Tổng Bill (Guest Folio)

Dưới góc nhìn của một BA (Business Analyst), quá trình Check-out không chỉ đơn thuần là việc thu tiền mà là bước chạm cuối cùng để tạo trải nghiệm tốt cho khách hàng, đồng thời đảm bảo doanh thu khách sạn không bị thất thoát. 

Tôi đã nghiên cứu Database và Model hiện tại của dự án (`GuestFolio`, `FolioTransaction`, `Invoice`, `Payment`) và xin đề xuất bản thiết kế chi tiết cho luồng **Check-out & Billing** như sau:

## 1. Luồng Quy Trình (Process Flow)

1. **Truy cập:** Lễ tân bấm nút **"Check-out"** tại Dashboard cho một Booking đang có trạng thái `CheckedIn`.
2. **Khởi tạo/Cập nhật Bill (Folio):** Hệ thống sẽ truy xuất `GuestFolio` của Booking đó và tính toán lại toàn bộ các khoản phí phát sinh thực tế.
3. **Hiển thị giao diện Tổng Bill:** Màn hình chia làm 2 phần (Giống thiết kế giao diện PMS chuyên nghiệp):
   - **Bên trái (Chi tiết giao dịch):** Liệt kê tiền phòng, phụ thu nhận phòng sớm (đã có), phụ thu trả phòng muộn, tiền ăn uống (Nhà hàng/Room Service), tiền Tour/Dịch vụ.
   - **Bên phải (Tóm tắt thanh toán):** Tính `Tổng tiền (Total Amount)` - `Tiền cọc (Deposit)` = `Số tiền cần thanh toán (Balance Due)`.
4. **Thanh toán & Trả phòng:** Lễ tân chọn Phương thức thanh toán (Tiền mặt, Chuyển khoản, Thẻ) -> Bấm xác nhận.
5. **Cập nhật hệ thống:** 
   - Đổi trạng thái Booking -> `CheckedOut`.
   - Đổi trạng thái Phòng (`Room`) -> `NeedsCleaning` (Cần dọn dẹp).
   - Sinh ra `Invoice` (Hóa đơn) và Record `Payment` (Thanh toán).

## 2. Quy Tắc Tính Phí Trả Phòng Muộn (Late Check-out Surcharge)

> [!WARNING]
> **Open Question: Cần Xác Nhận Logic Phụ Thu Trả Phòng Muộn**
> Giống như khi nhận phòng sớm, trả phòng muộn cũng cần quy tắc tính phí tự động. Tôi đề xuất quy tắc chuẩn của đa số các Resort 4-5 sao như sau (Giả sử giờ out chuẩn là 12:00 trưa):
> - Từ **12:00 đến 15:00**: Tính phụ thu **30%** tiền phòng của 1 đêm.
> - Từ **15:00 đến 18:00**: Tính phụ thu **50%** tiền phòng của 1 đêm.
> - **Sau 18:00**: Tính phụ thu **100%** (coi như lưu trú thêm 1 đêm).
>
> Bạn có đồng ý áp dụng bộ quy tắc này, hoặc có muốn chỉnh sửa phần trăm nào không?

## 3. Các Thay Đổi Về Kỹ Thuật (Technical Changes)

### Tầng Services (`IReceptionService`, `ReceptionService`)
#### [MODIFY] `ReceptionService.cs`
- Thêm hàm `PrepareCheckOutViewModelAsync(int bookingId)`:
  - Tự động cộng dồn các `FolioTransaction` cho các dịch vụ ăn uống (`FoodOrder`) hoặc `TourBooking` nếu có tích hợp.
  - Gọi hàm tính toán Phụ thu trả phòng muộn và tự động sinh ra một `FolioTransaction` loại `RoomCharge` (Late Check-out).
- Thêm hàm `ProcessCheckOutAsync(CheckOutViewModel model)`:
  - Cập nhật `ActualCheckOutTime = DateTime.Now`.
  - Thay đổi `RoomStatus` của các phòng thuộc Booking thành `NeedsCleaning`.
  - Sinh ra `Invoice` và `Payment` để lưu lịch sử thanh toán.
  - Thay đổi `BookingStatus` thành `CheckedOut`.

### Tầng ViewModels
#### [NEW] `CheckOutViewModel.cs`
- Chứa các thông tin hiển thị bill: `BookingId`, `GuestName`, `RoomNumbers`, Danh sách `FolioTransactions`, `DepositAmount`, `TotalAmount`, `BalanceDue`.
- Field nhận dữ liệu: `SelectedPaymentMethod`, `AmountPaid`.

### Tầng Controllers
#### [MODIFY] `ReceptionController.cs`
- Hàm `GET CheckOut(int id)`: Trả về màn hình tổng Bill.
- Hàm `POST CheckOut(CheckOutViewModel model)`: Xử lý lưu thanh toán và Check-out.

### Tầng Giao Diện (UI)
#### [MODIFY] `Views/Reception/Index.cshtml`
- Hiển thị nút **"Check-out"** (Màu cam/đỏ) thay cho nút "Báo cáo tạm trú" nếu trạng thái đang là `CheckedIn`.
#### [NEW] `Views/Reception/CheckOut.cshtml`
- Giao diện hóa đơn thanh toán đẹp mắt, phong cách chuyên nghiệp của Resort.
- Sử dụng dạng Grid: Cột trái liệt kê chi tiết các dòng phí, cột phải ô tóm tắt thanh toán và các nút thao tác.
#### [NEW] `Views/Reception/PrintInvoice.cshtml`
- Trang mẫu in Hóa Đơn (khổ giấy A4 hoặc Bill in nhiệt) để lễ tân in ra cho khách sau khi thanh toán xong.

## 4. Kế Hoạch Xác Minh (Verification Plan)
- **Tạo Booking mới:** Tiến hành Check-in bình thường.
- **Thử nghiệm Late Check-out:** Đổi giả lập thời gian trả phòng (vượt quá 12h) để xem hệ thống có tự sinh phí trả phòng muộn đúng tỷ lệ %.
- **Gom Bill:** Kiểm tra việc gom đủ tiền phòng và tiền đặt cọc.
- **Thanh toán:** Thực hiện Check-out, kiểm tra Database xem trạng thái phòng đã đổi thành `NeedsCleaning` và Invoice đã được tạo thành công hay chưa.

---

> [!IMPORTANT]
> Xin bạn phản hồi lại cho tôi về **Quy Tắc Phụ Thu Trả Phòng Muộn** ở mục 2. Nếu bạn đồng ý với kế hoạch và quy tắc này, hãy xác nhận để tôi tiến hành tạo các task code nhé!
