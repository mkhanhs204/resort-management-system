# Software Requirements Specification (SRS) - Staff Management Module
**Project:** PhucNguyen Resort & Tour Hub (SWP391-HOS-02)
**Module:** Auth, Profiles & Master Data (Module 1) - Sub-module: Staff Management

---

## 1. Overview
Module **Staff Management (Quản lý nhân sự/tài khoản)** là phân hệ dành riêng cho ban quản trị (Admin). Phân hệ này cho phép Admin kiểm soát toàn bộ người dùng trong hệ thống, đặc biệt là tạo và cấp quyền (assign roles) cho các tài khoản nhân viên (Receptionist, Housekeeping, F&B, Tour Guide, Business Manager). Module này đóng vai trò sống còn trong việc đảm bảo phân quyền bảo mật (Security & Authorization) của toàn bộ resort.

---

## 2. Actors (Đối tượng sử dụng)
| Actor | Mô tả |
| :--- | :--- |
| **Admin** | Người quản trị hệ thống với quyền hạn cao nhất. Được phép xem, tìm kiếm, phân quyền, tạo mới tài khoản nhân viên và khóa/mở khóa các tài khoản vi phạm chính sách. |

---

## 3. Use Cases (Yêu cầu chức năng)

### UC32 / UC03: View Accounts (Xem danh sách tài khoản)
- **Actor:** Admin
- **Mô tả:** Admin xem danh sách tất cả các tài khoản trong hệ thống (cả khách hàng và nhân viên). Hiển thị các thông tin cơ bản: Tên, Email, Vai trò (Role), và Trạng thái hoạt động (Active/Blocked).
- **Phân trang & Sắp xếp (UC35):** Hỗ trợ phân trang và sắp xếp danh sách theo ngày tạo, theo tên, hoặc theo vai trò để dễ dàng quản lý.

### UC33: Create Staff Account (Tạo tài khoản nhân viên)
- **Actor:** Admin
- **Mô tả:** Admin tạo tài khoản nội bộ cho nhân viên mới. Admin nhập các thông tin: Email, Mật khẩu khởi tạo, Họ tên, và **đặc biệt là phải chọn Vai trò (Role)** cho nhân viên đó.
- **Lưu ý:** Khách hàng (Customer) tự đăng ký ngoài trang chủ sẽ luôn nhận Role mặc định là Customer. Chỉ Admin mới có quyền gán các Role nghiệp vụ (Receptionist, F&B, v.v.).

### UC34: Search Account (Tìm kiếm tài khoản)
- **Actor:** Admin
- **Mô tả:** Cho phép Admin tìm kiếm nhanh một tài khoản cụ thể thông qua Email, Số điện thoại hoặc Họ tên.

### UC36: Block / Unblock Account (Khóa / Mở khóa tài khoản)
- **Actor:** Admin
- **Mô tả:** Admin có quyền vô hiệu hóa (Block) một tài khoản (nhân viên đã nghỉ việc hoặc khách hàng vi phạm chính sách). Tài khoản bị Block sẽ không thể Login vào hệ thống. Admin cũng có thể mở khóa (Unblock) lại khi cần thiết.

---

## 4. Business Rules (Quy tắc nghiệp vụ)

- **BR-STAFF-01 (Role Assignment):** Quyền (Role) chỉ được cấp phát bởi Admin. Các API tạo tài khoản nhân viên và gán quyền phải được bảo vệ nghiêm ngặt (Yêu cầu Authorization: Bearer Token + Role Admin).
- **BR-STAFF-02 (Unique Identity):** Mỗi tài khoản nhân viên (và khách hàng) phải có một địa chỉ Email duy nhất trên toàn hệ thống. Không cho phép trùng lặp Email.
- **BR-STAFF-03 (Authentication Lock):** Nếu trường trạng thái của User bị đánh dấu là "Blocked", hệ thống (Identity/Auth Layer) phải ngay lập tức từ chối mọi nỗ lực đăng nhập hoặc thu hồi token cũ.
- **BR-STAFF-04 (Data Privacy - NĐ 356/2025):** Khi Admin truy cập xem thông tin người dùng, thông tin nhạy cảm (như mật khẩu) tuyệt đối không được hiển thị (chỉ lưu trữ dạng Hash). 

---

## 5. Database Schema (Cấu trúc dữ liệu liên quan)

Phân hệ này tương tác chủ yếu với các bảng cốt lõi về Identity:

### 1. Bảng `Roles`
Lưu trữ danh sách các quyền hạn trong resort.
- `RoleId` (PK)
- `RoleName` (Unique: Admin, Customer, Receptionist, BusinessManager, TourGuide, Housekeeping, FnB, Kitchen)

### 2. Bảng `Users`
Lưu trữ thông tin đăng nhập và trạng thái tài khoản.
- `UserId` (PK)
- `RoleId` (FK) -> `Roles.RoleId` (Quyết định người này làm chức vụ gì)
- `Email` (Unique)
- `PasswordHash` (Chuỗi mã hóa mật khẩu)
- `IsBlocked` (Boolean - Cờ đánh dấu tài khoản có bị khóa hay không)
- `CreatedAt`, `UpdatedAt`

### 3. Bảng `UserProfiles`
Lưu trữ thông tin cá nhân mở rộng của nhân viên / người dùng.
- `ProfileId` (PK)
- `UserId` (FK, Unique) -> `Users.UserId`
- `FullName`, `PhoneNumber`, `Address`, `AvatarUrl`
- `IdentificationNumber` (CCCD/Passport - Lưu ý tuân thủ NĐ 356/2025 về bảo mật dữ liệu)

---
*Tài liệu được trích xuất dựa trên SWP391_NHOM1.md (UC32-36) và SWP391-HOS-02.md (Module 1).*
