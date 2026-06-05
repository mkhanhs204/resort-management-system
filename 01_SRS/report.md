# BÁO CÁO PHÂN TÍCH TÀI LIỆU

**Tài liệu gốc:** SWP391-HOS-02.md  
**Ngày báo cáo:** 2026-06-04  

---

## 📋 Tổng Quan Dự Án

**Tên dự án:** PhucNguyen Resort & Tour Hub  
**Mã dự án:** SWP391-HOS-02  
**Môn học:** Software Development Project (SWP391)  
**Loại:** Hệ thống quản lý khách sạn tích hợp (All-in-One Web Platform)  
**Nhóm:** 5 sinh viên  

---

## 🎯 Vấn Đề Cần Giải Quyết

Resort hiện đại phải quản lý **3 mảng dịch vụ riêng lẻ**: phòng, nhà hàng, tour. Việc dùng phần mềm tách biệt dẫn đến:

- Double-booking (đặt trùng)
- Thất thu doanh thu
- Trải nghiệm khách hàng kém
- Rủi ro bảo mật dữ liệu

→ **Giải pháp:** Một nền tảng web duy nhất tích hợp toàn bộ.

---

## 🔌 Tích Hợp API Bên Thứ Ba

| API | Mục đích |
|-----|----------|
| **Stripe / VNPay Sandbox** | Thanh toán online, đặt cọc |
| **SendGrid / Twilio** | Gửi email/SMS xác nhận đặt phòng, vé QR, hóa đơn |
| **OpenWeather / Google Maps** | Dự báo thời tiết, bản đồ cho tour |

---

## 👥 Các Actor (Người Dùng)

| Actor | Vai trò |
|-------|---------|
| **Guest (Khách)** | Đăng ký, đặt phòng, gọi đồ ăn, đặt tour |
| **Receptionist (Lễ tân)** | Check-in/out, xác minh danh tính, thanh toán |
| **Service Staff (F&B / Tour Guide)** | Cập nhật trạng thái đơn hàng, quản lý danh sách tour |
| **System Manager (Admin)** | Quản lý dữ liệu tổng thể, xem dashboard |

---

## 🧩 5 Module & Use Cases

### Module 1: Auth, Profiles & Master Data *(Sinh viên 1)*

| Use Case | Mô tả |
|----------|-------|
| **UC01** | Đăng ký, xác minh email, đăng nhập |
| **UC02** | Quản lý hồ sơ, upload CMND/Hộ chiếu |
| **UC03** | Admin quản lý tài khoản nhân viên & phân quyền |
| **UC04** | Admin quản lý danh mục phòng, menu nhà hàng, tour (CRUD + upload ảnh) |
| **UC05** | Khách yêu cầu xóa dữ liệu cá nhân (Right to Deletion – Nghị định 356/2025) |

---

### Module 2: Room Reservation & Front Desk *(CORE 1 – Sinh viên 2)*

| Use Case | Mô tả |
|----------|-------|
| **UC06** | Tìm kiếm phòng trống (theo ngày, số khách, loại phòng) |
| **UC07** | Đặt phòng và thanh toán đặt cọc |
| **UC08** | Lễ tân xem dashboard arrivals/departures hàng ngày |
| **UC09** | Lễ tân thực hiện Check-in / Check-out |
| **UC10** | Cập nhật trạng thái vật lý phòng (Available, Occupied, Cleaning, Maintenance) |

---

### Module 3: Restaurant POS & Room Service *(CORE 2 – Sinh viên 3)*

| Use Case | Mô tả |
|----------|-------|
| **UC11** | Khách (đã check-in) gọi đồ ăn ship lên phòng |
| **UC12** | Khách đặt bàn nhà hàng theo ngày/giờ/số người |
| **UC13** | F&B Staff xem danh sách đơn hàng thời gian thực & cập nhật trạng thái (Pending → Cooking → Delivered) |
| **UC14** | Staff gắn chi phí bữa ăn vào hóa đơn phòng (Charge to Room) |

---

### Module 4: Tour Booking & Feedback *(Sinh viên 4)*

| Use Case | Mô tả |
|----------|-------|
| **UC15** | Khách xem tour, lịch trình, dự báo thời tiết |
| **UC16** | Đặt tour theo ngày và số người tham gia |
| **UC17** | Tour Guide xem danh sách khách tham gia tour của mình |
| **UC18** | Khách đánh giá 5 sao + viết review sau khi checkout/hoàn thành tour |
| **UC19** | Admin kiểm duyệt review (ẩn/hiện) |

---

### Module 5: Consolidated Billing & Dashboards *(Sinh viên 5 – The Integrator)*

| Use Case | Mô tả |
|----------|-------|
| **UC20** | Tự động tạo hóa đơn tổng hợp khi checkout (phòng + đồ ăn + tour) |
| **UC21** | Lễ tân xử lý thanh toán cuối (tiền mặt/thẻ) và đánh dấu "Đã thanh toán" |
| **UC22** | Manager xem dashboard tỷ lệ lấp đầy phòng theo tháng |
| **UC23** | Manager xem dashboard doanh thu theo từng nguồn (Phòng / F&B / Tour) |
| **UC24** | Export báo cáo tài chính tháng ra Excel/PDF |

---

## ⚖️ Business Rules Quan Trọng

| Rule | Nội dung |
|------|----------|
| **BR01** | Không được double-booking phòng hoặc bàn ăn cùng thời điểm |
| **BR02** | Không thể check-out nếu còn đơn ăn/tour chưa thanh toán |
| **BR03** | Hoàn cọc 100% nếu hủy trước 48h; không hoàn nếu hủy trong vòng 48h |

---

## 📚 Kiến Thức Nền & Pháp Lý

| Nguồn | Phạm vi áp dụng |
|-------|----------------|
| **Nghị định 356/2025/NĐ-CP** | Bảo vệ dữ liệu cá nhân → Right to Deletion, Explicit Consent |
| **Luật Cư trú 2020** | Thu thập thông tin tạm trú (họ tên, ngày sinh, số CMND/hộ chiếu, quốc tịch) |
| **Luật Du lịch 2017** | Cảnh báo an toàn, bảo hiểm, chính sách hoàn tiền |
| **Guest Cycle (AHLEI)** | Arrival → Check-In → Occupancy → Departure |
| **TCVN 4391:2015** | Tiêu chuẩn khách sạn 4-5 sao |
| **KOT Workflow** | Quy trình phiếu bếp: Pending → Cooking → Served |
| **USALI** | Phân loại doanh thu theo Rooms / F&B / Tour |

---

## 💡 Nhận Xét Kỹ Thuật

Đây là một dự án **phức tạp và thực tế**, mô phỏng sát với nghiệp vụ resort thực tế. Mỗi sinh viên đảm nhận một module độc lập nhưng có sự liên kết chặt chẽ:

- **Module 5 (Billing)** là module phụ thuộc nhiều nhất vào các module khác — đây là "Integrator" cần tổng hợp dữ liệu từ cả 4 module còn lại.
- **BR01 (Concurrency)** yêu cầu xử lý race condition khi đặt phòng/bàn — cần dùng database transaction hoặc locking.
- **BR02** tạo ra ràng buộc nghiệp vụ chặt chẽ giữa Module 2 (Check-out) với Module 3 & 4.
- Cần tuân thủ luật pháp Việt Nam về bảo vệ dữ liệu cá nhân (Nghị định 356/2025).

---

*Báo cáo được tạo tự động từ tài liệu đặc tả SWP391-HOS-02.md*
