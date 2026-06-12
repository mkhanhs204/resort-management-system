# PHÂN TÍCH CHUYÊN SÂU MODULE 3: RESTAURANT POS & ROOM SERVICE

Tài liệu này cung cấp cái nhìn chi tiết về mặt nghiệp vụ thực tế, các ràng buộc hệ thống và gợi ý thiết kế cho **Module 3: Restaurant POS & Room Service** thuộc hệ thống PhucNguyen Resort & Tour Hub.

---

## 1. Bức tranh Tổng quan (Big Picture)

Module 3 đóng vai trò là "Dạ dày" và "Nguồn thu phụ" của Resort. Nó không chỉ phục vụ khách vãng lai bước vào nhà hàng ăn (Point-Of-Sale), mà còn phục vụ các Thượng đế đang ở trong phòng lười xuống sảnh (Room Service).

### Điểm mấu chốt trong thiết kế:
*   **Trải nghiệm khách hàng:** Khách phải thấy tiện lợi khi gọi món.
*   **Vận hành của Bếp (KOT - Kitchen Order Ticket):** Bếp phải nhận được order ngay lập tức, không để lọt đơn, sai món.
*   **Dòng tiền (Billing):** Tiền ăn phải thu được, không thu tiền mặt thì phải cộng đúng vào tiền phòng.

---

## 2. Phân tích Chi tiết Nghiệp vụ & Use Cases

### 2.1. UC11: Đặt đồ ăn lên phòng (Room Service)
*   **Ngữ cảnh thực tế:** Khách đang ở phòng `101`, lúc 10h đêm đói bụng, mở app lên xem Menu và bấm gọi 1 phần Bò bít tết.
*   **Logic Hệ thống:**
    1.  **Kiểm tra điều kiện tiên quyết (Pre-condition):** API nhận Request phải kiểm tra User này có thực sự đang sở hữu một phòng nào đó không? Trạng thái Booking của họ ở Module 2 phải là `CHECKED_IN`. (Khách đã checkout hoặc khách vãng lai không được dùng tính năng này).
    2.  **Xử lý Đơn:** Tạo một `Order` với loại hình `Type = "ROOM_SERVICE"`. Lưu kèm `Room_ID` của khách vào đơn.
*   **Edge Cases (Trường hợp dị thường cần xử lý):**
    *   Món khách gọi đã hết nguyên liệu (Hệ thống phải có cờ `isAvailable` cho từng món ăn).
    *   Nhà hàng đã đóng cửa (Cần check giờ hoạt động của nhà hàng trước khi cho phép đặt).

### 2.2. UC12: Đặt bàn nhà hàng (Table Reservation)
*   **Ngữ cảnh thực tế:** Khách muốn tổ chức sinh nhật cho 4 người vào lúc 19:00 tối mai tại nhà hàng của Resort.
*   **Logic Hệ thống:**
    1.  Khách chọn ngày (`Date`), giờ (`TimeSlot`), số người (`PartySize`).
    2.  Hệ thống truy vấn các bàn (`Table`) có `Capacity >= 4` và chưa có ai đặt trong khoảng thời gian đó (thường giữ bàn trong 2 tiếng: 19:00 - 21:00).
*   **Bài toán hóc búa nhất (BR01 - Strict Concurrency):**
    *   Tránh **Double-Booking**: Chuyện gì xảy ra nếu 2 khách hàng cùng lúc (chênh nhau 1 phần nghìn giây) bấm đặt Bàn số 5?
    *   **Giải pháp thiết kế:** Database của bạn phải xử lý "Khóa" (Lock). Sử dụng Transaction mức độ cao, hoặc dùng Optimistic Concurrency Control (ví dụ: dùng trường `Version` trong DB) để người bấm sau sẽ nhận thông báo lỗi: "Bàn này vừa có người đặt, vui lòng chọn bàn khác".

### 2.3. UC13: Nhân viên quản lý luồng Order (KOT Workflow)
*   **Ngữ cảnh thực tế:** Trong bếp có một màn hình iPad. Khi khách ở trên phòng bấm "Đặt món", màn hình iPad kêu "Tinh", hiện ra đơn hàng mới. Bếp trưởng bấm "Nhận làm". Nấu xong bấm "Đã xong" để nhân viên bưng lên.
*   **Sơ đồ Trạng thái (State Machine) bắt buộc:**
    *   `PENDING` (Mới đặt, chờ bếp xác nhận).
    *   `COOKING` (Bếp đang nấu - lúc này khách **không được hủy** qua app nữa).
    *   `DELIVERED` (Đã giao hàng thành công / Khách đã nhận).
    *   *(Tùy chọn)* `CANCELED` (Khách hủy khi đang Pending, hoặc Bếp từ chối do hết đồ).
*   **Giải pháp thiết kế:** Cần sử dụng WebSocket (như SignalR trong .NET hoặc Socket.io trong Node.js) để push event `OrderCreated` xuống client của Bếp ngay lập tức (Real-time).

### 2.4. UC14: Ghi nợ vào tiền phòng (Charge to Room)
*   **Ngữ cảnh thực tế:** Khách tên Nam ở phòng 101, xuống nhà hàng ăn hết 500k. Ăn xong khách lười lấy ví, bảo "Tính vào tiền phòng cho anh nhé". Nhân viên nhà hàng mở máy POS, chọn phòng 101, thấy đúng tên Nam, bấm "Ghi nợ".
*   **Logic Hệ thống:**
    1.  Nhân viên F&B không thu tiền mặt/chuyển khoản cho đơn hàng này.
    2.  `Order` này được cập nhật `Payment_Method = "CHARGE_TO_ROOM"`.
    3.  Khóa ngoại `Room_Booking_ID` của đơn này được trỏ vào phiên ở hiện tại của ông Nam.
*   **Ràng buộc cốt lõi (BR02):** Tại thời điểm ông Nam đi Check-out ngày hôm sau, Lễ tân (Module 5) sẽ quét toàn bộ các `Order` có `Payment_Method = "CHARGE_TO_ROOM"` mà chưa thanh toán để cộng dồn vào hóa đơn cuối cùng (Consolidated Invoice).

---

## 3. Đề xuất Mô hình Thực thể - Database Entities (Draft)

Để đáp ứng các nghiệp vụ trên, Module 3 cần sở hữu các bảng dữ liệu sau trong Schema:

1.  **`MenuCategory`**: Thể loại món (Đồ ăn chính, Đồ uống, Tráng miệng).
2.  **`FoodItem`**: Món ăn. Thuộc tính: Tên, Giá, Hình ảnh, `isAvailable` (còn/hết). *(Nguồn dữ liệu do Module 1 Admin quản lý)*.
3.  **`RestaurantTable`**: Quản lý bàn vật lý. Thuộc tính: Tên Bàn (Bàn 1, VIP 2), `Capacity` (chứa được mấy người).
4.  **`TableReservation`**: Lịch sử đặt bàn.
    *   `Guest_ID`, `Table_ID`.
    *   `ReservedDate`, `StartTime`, `EndTime`.
    *   `Status` (Reserved, Seated, Canceled).
5.  **`FoodOrder`**: Đơn đặt món. Đây là BẢNG TRUNG TÂM.
    *   `Order_Type`: (RoomService / DineIn).
    *   `Status`: (Pending / Cooking / Delivered).
    *   `TotalAmount`.
    *   `Payment_Status`: (Unpaid / Paid).
    *   `Payment_Method`: (Cash / Credit / **ChargeToRoom**).
    *   `Room_Booking_ID`: Nullable. Có giá trị nếu khách Charge to Room hoặc Room Service.
6.  **`FoodOrderDetail`**: Lưu danh sách món ăn của một Order.
    *   `Order_ID`, `FoodItem_ID`, `Quantity`, `UnitPrice`.

---

## 4. Gợi ý áp dụng TDD cho Module 3

Khi bạn nhận module này để code, hãy chia nhỏ thành các phiên làm việc TDD như sau:

*   **TDD Phiên 1 (Dễ nhất): Quản lý Menu (CRUD FoodItems).**
    *   *Test:* Xem khách hàng có lấy được danh sách các món ăn đang "Available" không.
*   **TDD Phiên 2 (Lõi): Đặt bàn chống trùng lặp (Table Reservation).**
    *   *Test 1:* Khách A đặt bàn 1 lúc 19:00 -> Thành công.
    *   *Test 2:* Khách B đặt bàn 1 lúc 19:30 (bị đè thời gian) -> Báo lỗi Conflict.
*   **TDD Phiên 3 (Lõi): Đặt đồ ăn & Charge to Room.**
    *   *Test 1:* Khách Check-in gọi đồ ăn (Room Service) -> Thành công, đơn chuyển trạng thái PENDING.
    *   *Test 2:* Khách chưa Check-in cố gọi đồ ăn -> Báo lỗi Unauthorized/Forbidden.
    *   *Test 3:* Đơn hàng được charge to room, kiểm tra xem `Room_Booking_ID` có được gán đúng không.

Hy vọng tài liệu này giúp bạn làm rõ được con đường phía trước để thiết kế EDS và viết code chất lượng cao cho Module 3!
