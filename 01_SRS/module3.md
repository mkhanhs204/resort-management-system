# PHÂN TÍCH CHUYÊN SÂU MODULE 3: RESTAURANT POS & ROOM SERVICE

Tài liệu này cung cấp cái nhìn chi tiết về mặt nghiệp vụ thực tế, các ràng buộc hệ thống và gợi ý thiết kế cho **Module 3: Restaurant POS & Room Service** thuộc hệ thống PhucNguyen Resort & Tour Hub.

---

## 1. Bức tranh Tổng quan (Big Picture)

Module 3 đóng vai trò là "Dạ dày" và "Nguồn thu phụ" của Resort. Nó không chỉ phục vụ khách vãng lai bước vào nhà hàng ăn (Point-Of-Sale), mà còn phục vụ các Thượng đế đang ở trong phòng lười xuống sảnh (Room Service).

### Điểm mấu chốt trong thiết kế:

* **Trải nghiệm khách hàng:** Khách phải thấy tiện lợi khi gọi món.
* **Vận hành của Bếp (KOT - Kitchen Order Ticket):** Bếp phải nhận được order ngay lập tức, không để lọt đơn, sai món.
* **Dòng tiền (Billing):** Tiền ăn phải thu được, không thu tiền mặt thì phải cộng đúng vào tiền phòng.

---

## 2. Phân tích Chi tiết Nghiệp vụ & Use Cases

### 2.1. Đặt đồ ăn lên phòng (Room Service)

* **Ngữ cảnh thực tế:** Khách đang ở phòng `101`, lúc 10h đêm đói bụng, mở app lên xem Menu và bấm gọi 1 phần Bò bít tết.
* **Logic Hệ thống:**
  1. **Kiểm tra điều kiện tiên quyết (Pre-condition):** API nhận Request phải kiểm tra User này có thực sự đang sở hữu một phòng nào đó không? Trạng thái Booking của họ ở Module 2 phải là `CHECKED_IN`. (Khách đã checkout hoặc khách vãng lai không được dùng tính năng này).
  2. **Xử lý Đơn:** Tạo một `Order` với loại hình `Type = "ROOM_SERVICE"`. Lưu kèm `Room_ID` của khách vào đơn.
* **Edge Cases (Trường hợp dị thường cần xử lý):**
  * Món khách gọi đã hết nguyên liệu (Hệ thống phải có cờ `isAvailable` cho từng món ăn).
  * Nhà hàng đã đóng cửa (Cần check giờ hoạt động của nhà hàng trước khi cho phép đặt).

### 2.2. Đặt bàn nhà hàng (Table Reservation)

* **Ngữ cảnh thực tế:** Khách muốn tổ chức sinh nhật cho 4 người vào lúc 19:00 tối mai tại nhà hàng của Resort.
* **Logic Hệ thống:**
  1. Khách chọn ngày (`Date`), giờ (`TimeSlot`), số người (`PartySize`).
  2. Hệ thống truy vấn các bàn (`Table`) có `Capacity >= 4` và chưa có ai đặt trong khoảng thời gian đó (thường giữ bàn trong 2 tiếng: 19:00 - 21:00).
* **Bài toán hóc búa nhất (BR01 - Strict Concurrency):**
  * Tránh **Double-Booking**: Chuyện gì xảy ra nếu 2 khách hàng cùng lúc (chênh nhau 1 phần nghìn giây) bấm đặt Bàn số 5?
  * **Giải pháp thiết kế:** Database của bạn phải xử lý "Khóa" (Lock). Sử dụng Transaction mức độ cao, hoặc dùng Optimistic Concurrency Control (ví dụ: dùng trường `Version` trong DB) để người bấm sau sẽ nhận thông báo lỗi: "Bàn này vừa có người đặt, vui lòng chọn bàn khác".

### 2.3.Nhân viên quản lý luồng Order (KOT Workflow)

* **Ngữ cảnh thực tế:** Trong bếp có một màn hình iPad. Khi khách ở trên phòng bấm "Đặt món", màn hình iPad kêu "Tinh", hiện ra đơn hàng mới. Bếp trưởng bấm "Nhận làm". Nấu xong bấm "Đã xong" để nhân viên bưng lên.
* **Sơ đồ Trạng thái (State Machine) bắt buộc:**
  * `PENDING` (Mới đặt, chờ bếp xác nhận).
  * `COOKING` (Bếp đang nấu - lúc này khách **không được hủy** qua app nữa).
  * `DELIVERED` (Đã giao hàng thành công / Khách đã nhận).
  * *(Tùy chọn)* `CANCELED` (Khách hủy khi đang Pending, hoặc Bếp từ chối do hết đồ).
* **Giải pháp thiết kế:** Cần sử dụng WebSocket (như SignalR trong .NET hoặc Socket.io trong Node.js) để push event `OrderCreated` xuống client của Bếp ngay lập tức (Real-time).

### 2.4. Ghi nợ vào tiền phòng (Charge to Room)

* **Ngữ cảnh thực tế:** Khách tên Nam ở phòng 101, xuống nhà hàng ăn hết 500k. Ăn xong khách lười lấy ví, bảo "Tính vào tiền phòng cho anh nhé". Nhân viên nhà hàng mở máy POS, chọn phòng 101, thấy đúng tên Nam, bấm "Ghi nợ".
* **Logic Hệ thống:**
  1. Nhân viên F&B không thu tiền mặt/chuyển khoản cho đơn hàng này.
  2. `Order` này được cập nhật `Payment_Method = "CHARGE_TO_ROOM"`.
  3. Khóa ngoại `Room_Booking_ID` của đơn này được trỏ vào phiên ở hiện tại của ông Nam.
* **Ràng buộc cốt lõi (BR02):** Tại thời điểm ông Nam đi Check-out ngày hôm sau, Lễ tân (Module 5) sẽ quét toàn bộ các `Order` có `Payment_Method = "CHARGE_TO_ROOM"` mà chưa thanh toán để cộng dồn vào hóa đơn cuối cùng (Consolidated Invoice).

---

## 3. Đề xuất Mô hình Thực thể - Database Entities (Draft)

Để đáp ứng các nghiệp vụ trên, Module 3 cần sở hữu các bảng dữ liệu sau trong Schema:

1. **`MenuCategory`**: Thể loại món (Đồ ăn chính, Đồ uống, Tráng miệng).
2. **`FoodItem`**: Món ăn. Thuộc tính: Tên, Giá, Hình ảnh, `isAvailable` (còn/hết). *(Nguồn dữ liệu do Module 1 Admin quản lý)*.
3. **`RestaurantTable`**: Quản lý bàn vật lý. Thuộc tính: Tên Bàn (Bàn 1, VIP 2), `Capacity` (chứa được mấy người).
4. **`TableReservation`**: Lịch sử đặt bàn.
   * `Guest_ID`, `Table_ID`.
   * `ReservedDate`, `StartTime`, `EndTime`.
   * `Status` (Reserved, Seated, Canceled).
5. **`FoodOrder`**: Đơn đặt món. Đây là BẢNG TRUNG TÂM.
   * `Order_Type`: (RoomService / DineIn).
   * `Status`: (Pending / Cooking / Delivered).
   * `TotalAmount`.
   * `Payment_Status`: (Unpaid / Paid).
   * `Payment_Method`: (Cash / Credit / **ChargeToRoom**).
   * `Room_Booking_ID`: Nullable. Có giá trị nếu khách Charge to Room hoặc Room Service.
6. **`FoodOrderDetail`**: Lưu danh sách món ăn của một Order.
   * `Order_ID`, `FoodItem_ID`, `Quantity`, `UnitPrice`.

---

## 4. Gợi ý áp dụng TDD cho Module 3

Khi bạn nhận module này để code, hãy chia nhỏ thành các phiên làm việc TDD như sau:

* **TDD Phiên 1 (Dễ nhất): Quản lý Menu (CRUD FoodItems).**
  * *Test:* Xem khách hàng có lấy được danh sách các món ăn đang "Available" không.
* **TDD Phiên 2 (Lõi): Đặt bàn chống trùng lặp (Table Reservation).**
  * *Test 1:* Khách A đặt bàn 1 lúc 19:00 -> Thành công.
  * *Test 2:* Khách B đặt bàn 1 lúc 19:30 (bị đè thời gian) -> Báo lỗi Conflict.
* **TDD Phiên 3 (Lõi): Đặt đồ ăn & Charge to Room.**
  * *Test 1:* Khách Check-in gọi đồ ăn (Room Service) -> Thành công, đơn chuyển trạng thái PENDING.
  * *Test 2:* Khách chưa Check-in cố gọi đồ ăn -> Báo lỗi Unauthorized/Forbidden.
  * *Test 3:* Đơn hàng được charge to room, kiểm tra xem `Room_Booking_ID` có được gán đúng không.

Hy vọng tài liệu này giúp bạn làm rõ được con đường phía trước để thiết kế EDS và viết code chất lượng cao cho Module 3!

---

## 5. Ánh xạ Use Case (Master Mapping chuẩn hóa theo `SWP391_NHOM1.md`)

*Lưu ý: Các số hiệu Use Case (UC11, UC12...) ở phần 2 của tài liệu này là các mã định danh tạm thời. Dưới đây là bảng ánh xạ (Mapping) chính thức vào tài liệu Master SRS `SWP391_NHOM1.md` để đồng nhất mã UC cho toàn dự án, phân tách rõ ràng thành các Feature (Workflow) phục vụ cho việc viết EDS.*

### 🌟 Feature 1: Food Ordering & KOT Workflow (Luồng Đặt món & Xử lý Bếp)

**Mô tả & Tác dụng:** Đây là luồng xương sống của nhà hàng. Khách gọi món trên phòng (Room Service), hệ thống sinh ra hóa đơn (`FoodOrders`) và phiếu báo bếp (`KOT`). Nhân viên dưới bếp xác nhận nấu, nhân viên phục vụ bưng lên phòng và cuối cùng ghi nợ chi phí vào tài khoản phòng của khách để thanh toán lúc Check-out.
**Các Use Case chuẩn tham gia vào luồng này:**

* **Góc nhìn Khách hàng (Customer/Guest):**
  * `UC09`: View Restaurant Menu (Xem thực đơn)
  * `UC10`: View Food Details (Xem chi tiết món)
  * `UC25`: Order Food To Room (Tạo đơn gọi món lên phòng)
* **Góc nhìn Nhân viên Nhà hàng (F&B Staff):**
  * `UC68`: View Food Orders (Theo dõi danh sách đơn hàng của khách)
  * `UC69`: Update Food Order Status (Cập nhật trạng thái tổng: Pending ➔ Cooking ➔ Delivered)
  * `UC72`: Charge Food Bill To Room (Ghi nợ chi phí vào hóa đơn phòng của khách)
* **Góc nhìn Dưới bếp (Kitchen Staff):**
  * `UC74`: View Kitchen Order Tickets (Xem danh sách KOT cần nấu)
  * `UC75`: View KOT Details (Xem chi tiết số lượng món cần nấu)
  * `UC76`: Update KOT Status (Cập nhật trạng thái bếp: Pending ➔ Preparing ➔ Ready)

### 🌟 Feature 2: Table Reservation Workflow (Luồng Đặt bàn & Điều phối)

**Mô tả & Tác dụng:** Giải quyết bài toán cấp phát tài nguyên vật lý (bàn ăn). Khách hàng xem sơ đồ và chọn bàn, hệ thống phải đảm bảo xử lý đồng thời (Concurrency) để không có 2 khách cùng đặt 1 bàn ở cùng 1 thời điểm. Nhân viên F&B theo dõi và đổi trạng thái bàn khi khách đến.
**Các Use Case chuẩn tham gia vào luồng này:**

* **Góc nhìn Khách hàng (Customer/Guest):**
  * `UC07`: View Restaurant Table (Xem danh sách bàn/sơ đồ)
  * `UC08`: View Table Details (Xem sức chứa của bàn)
  * `UC24`: Reserve Restaurant Table (Tạo đơn đặt bàn chống trùng lặp)
* **Góc nhìn Nhân viên Nhà hàng (F&B Staff):**
  * `UC70`: View Table Reservations (Xem danh sách khách đã đặt bàn)
  * `UC71`: Update Table Status (Cập nhật trạng thái thực tế: Available ➔ Reserved ➔ Occupied)

### 🌟 Feature 3: Restaurant Master Data Management (Quản lý Danh mục F&B)

**Mô tả & Tác dụng:** Cung cấp nền teller dữ liệu cho Feature 1 và 2. Quản lý việc thêm, sửa, xóa các món ăn, thiết lập giá, cập nhật trạng thái "Còn hàng / Hết hàng", và setup sơ đồ bàn.
**Các Use Case chuẩn tham gia vào luồng này:**

* **Góc nhìn Quản trị viên (Admin / Business Manager):**
  * `UC04` (Subset): Manage Restaurant Menus (Quản lý CRUD danh mục món ăn và tải ảnh minh họa).

---

## 6. Hướng dẫn thiết kế EDS (Engineering Design Specification) theo Feature

Để đảm bảo hệ thống dễ bảo trì và không bị đụng độ code (Conflict) khi nhiều lập trình viên cùng làm việc, **tuyệt đối không tạo 1 file EDS cho từng Use Case lẻ tẻ**. Thay vào đó, hãy tạo **1 file EDS cho 1 Feature (Workflow)** theo hướng dẫn sau:

### 📄 6.1. Đối với Feature 1: Food Order & KOT Workflow
**Tên file đề xuất:** `03_implement/FoodOrder_EDS.md`
**Trọng tâm thiết kế (What to focus on):**
- **State Machine Diagram (Biểu đồ trạng thái):** Đây là phần quan trọng nhất của file này. Phải vẽ rõ sự chuyển đổi trạng thái của `FoodOrder` (Pending ➔ Cooking ➔ Delivered) và `KOT` (Pending ➔ Preparing ➔ Ready).
- **Domain Events:** Cần định nghĩa sự kiện `OrderCreatedEvent` để hệ thống bắn thông báo Real-time (qua SignalR/WebSocket) xuống màn hình iPad của Bếp.
- **Data Consistency (Tính nhất quán dữ liệu):** Khi ghi nợ (Charge to Room), file EDS phải định nghĩa rõ hàm nào sẽ lưu khóa ngoại `Room_Booking_ID` vào bảng `FoodOrders` và đảm bảo Booking đó đang ở trạng thái `CHECKED_IN`.

### 📄 6.2. Đối với Feature 2: Table Reservation Workflow
**Tên file đề xuất:** `03_implement/TableReservation_EDS.md`
**Trọng tâm thiết kế (What to focus on):**
- **Concurrency Control (Kiểm soát đồng thời):** File EDS này bắt buộc phải có mục **ADR (Architecture Decision Record)** để giải quyết bài toán chống Double-Booking (2 khách cùng đặt 1 bàn). Đề xuất sử dụng Optimistic Concurrency Control (EF Core `[Timestamp]` hoặc `RowVersion`) hoặc Database Transaction Isolation Level.
- **Validation Rules:** Xác định rõ ràng khoảng thời gian giữ bàn (Ví dụ: Một lần đặt bàn kéo dài mặc định 2 tiếng, khách đến trễ 30 phút sẽ tự động bị hủy).

### 📄 6.3. Đối với Feature 3: Master Data Management (F&B)
**Tên file đề xuất:** `03_implement/FnBMasterData_EDS.md`
**Trọng tâm thiết kế (What to focus on):**
- **File Upload:** Định nghĩa cách thức upload ảnh của món ăn (Lưu vào thư mục `wwwroot/images/menu` hay sử dụng Cloud Storage như AWS S3/Cloudinary).
- **Soft Delete (Xóa mềm):** Khi Admin xóa một món ăn (hoặc đổi trạng thái `isAvailable = false`), EDS phải quy định rõ là **KHÔNG ĐƯỢC XÓA CỨNG (Hard Delete)** trong Database để không làm hỏng lịch sử các đơn hàng (FoodOrder) cũ đã từng gọi món này.

**🔥 QUY TẮC CHUNG CHO MỌI FILE EDS:**
1. **Luôn có Ma trận truy vết (Traceability Matrix):** Liệt kê rõ các Use Case (Mã UC chuẩn từ phần 5) được bao phủ trong file EDS này.
2. **Liệt kê Error Codes:** Bắt buộc phải có bảng mã lỗi trả về (Ví dụ: `FO-001: Món ăn đã hết`, `TR-002: Bàn đã có người đặt`).
3. **Tiền đề cho TDD:** Mỗi file EDS sẽ là bản vẽ gốc để sinh ra 1 hoặc nhiều file TDD (Test-Driven Development) tương ứng.
