# UC11 – Room Service Order Testing Checklist

## Test Environment

Giả sử ứng dụng chạy tại:

```text
https://localhost:{port}
```

Thay `{port}` bằng port thực tế của dự án.

---

# Test Data Preparation

UC11 yêu cầu:

* User phải đăng nhập.
* User phải có role **Customer**.
* Booking phải thuộc user đó.
* Booking phải ở trạng thái **CheckedIn**.

## Booking phù hợp trong Seed Data

| BookingId | UserId | Role     | Status    |
| --------- | ------ | -------- | --------- |
| 12        | 12     | Customer | CheckedIn |
| 17        | 17     | Customer | CheckedIn |

> Lưu ý: Password trong seed đang ở dạng `hashed-password-xx`, có thể không đăng nhập trực tiếp được.

### Cách test đơn giản

1. Đăng ký tài khoản mới:

```text
/Account/Register
```

2. Đăng nhập bằng tài khoản vừa tạo.
3. Tạo hoặc gán một booking CheckedIn cho tài khoản đó.
4. Sử dụng BookingId tương ứng để test Room Service.

### BookingStatus

```csharp
BookingStatus.CheckedIn = 1
```

---

# Standard UI Flow

## Step 1 – Login

Truy cập:

```text
/Account/Login
```

Đăng nhập bằng tài khoản Customer.

---

## Step 2 – Open Profile

Truy cập:

```text
/Profile
```

Nếu user có booking CheckedIn, hệ thống hiển thị nút:

```text
Order Room Service
```

---

## Step 3 – Open Room Service

Sau khi bấm nút:

```text
/FoodOrders/RoomService?bookingId={bookingId}
```

---

## Step 4 – Create Order

* Chọn số lượng món ăn.
* Submit Order.

---

## Step 5 – Success Redirect

Sau khi đặt món thành công:

```text
/FoodOrders/Details/{foodOrderId}
```

---

## Expected Result

Thông tin hiển thị:

* FoodOrderId
* BookingId
* CustomerName
* RoomNumber
* OrderStatus
* KitchenStatus
* Danh sách món
* Quantity
* UnitPrice
* LineTotal
* TotalAmount

### Database Verification

Bảng `FoodOrders`

* Thêm 1 record mới.
* `OrderStatus = Pending`
* `IsChargedToRoom = false`

Bảng `FoodOrderItems`

* Tạo các record tương ứng với từng món.

---

# Direct URL Test Cases

## Case 1 – User Not Logged In

### URL

```text
/FoodOrders/RoomService?bookingId=12
```

### Expected

* Redirect Login.
* Không xem menu.
* Không tạo order.

---

## Case 2 – Logged In But Not Customer

### URL

```text
/FoodOrders/RoomService?bookingId=12
```

### Expected

* Bị chặn bởi:

```csharp
[Authorize(Roles = AppRoles.Customer)]
```

* Access Denied hoặc Redirect.

---

## Case 3 – Missing BookingId

### URL

```text
/FoodOrders/RoomService
```

### Expected

* Hiện form nhập BookingId.
* Không hiện menu.
* Không tạo order.

---

## Case 4 – Booking Does Not Exist

### URL

```text
/FoodOrders/RoomService?bookingId=999999
```

### Expected

* Hiển thị lỗi booking không tồn tại.
* Không load menu.

---

## Case 5 – Booking Belongs To Another User

### URL

```text
/FoodOrders/RoomService?bookingId=12
```

### Expected

* Không load booking.
* Hiển thị lỗi rõ ràng.
* Không cho đặt món.

---

## Case 6 – Booking Not CheckedIn

### Status Examples

* Reserved
* CheckedOut
* Cancelled
* NoShow

### URL

```text
/FoodOrders/RoomService?bookingId={bookingId}
```

### Expected

* Hiển thị thông tin booking.
* Không hiển thị form order.
* Hiển thị thông báo:

```text
Only checked-in bookings can order room service.
```

---

## Case 7 – Valid Booking And Menu Loaded

### URL

```text
/FoodOrders/RoomService?bookingId=12
```

### Preconditions

* Đăng nhập đúng Customer.
* Customer sở hữu booking.

### Expected

Hiển thị:

* Thông tin booking.
* Danh sách menu.

Chỉ hiển thị menu:

```text
IsAvailable = true
IsDeleted = false
```

Input quantity:

```html
min="0"
max="1000"
```

---

## Case 8 – Submit Without Selecting Any Item

### Action

Tất cả quantity = 0.

### Expected

Thông báo:

```text
Please select at least one menu item.
```

User vẫn ở màn hình Room Service.

---

## Case 9 – Invalid Quantity

### Action

Nhập:

* Quantity < 0
* Quantity > 1000

### Expected

Thông báo:

```text
Menu item quantity must be between 0 and 1000.
```

> Có thể dùng DevTools để bypass UI validation.

---

## Case 10 – Successful Order Creation

### Example

| Item    | Quantity |
| ------- | -------- |
| Pho Bo  | 1        |
| Bun Cha | 2        |

### Expected

Redirect:

```text
/FoodOrders/Details/{id}
```

Kiểm tra:

```text
TotalAmount
```

Được tính chính xác:

```text
Quantity × UnitPrice
```

Database:

```text
IsChargedToRoom = false
```

---

## Case 11 – View Own Order Details

### URL

```text
/FoodOrders/Details/{foodOrderId}
```

### Expected

Hiển thị:

* Order Details
* Booking Info
* Ordered Items
* Total Amount

Có nút:

```text
Back to Room Service
```

Tiền hiển thị theo định dạng VND.

---

## Case 12 – View Another User's Order

### URL

```text
/FoodOrders/Details/{foodOrderId}
```

### Expected

```text
NotFound
```

Không lộ dữ liệu user khác.

---

## Case 13 – Menu Item Unavailable

### Test Data

```text
IsAvailable = false
```

hoặc

```text
IsDeleted = true
```

### Expected

Không hiển thị trong menu.

Nếu submit bằng DevTools:

```text
One or more menu items are unavailable or discontinued.
```

---

## Case 14 – Duplicate Menu Item

### Setup

Gửi request chứa cùng một MenuItemId nhiều lần.

### Expected

```text
Duplicate menu items are not allowed in the same order.
```

---

## Case 15 – Menu Item Does Not Exist

### Setup

Sửa MenuItemId:

```text
999999
```

### Expected

```text
One or more menu items do not exist.
```

---

# Test Coverage Summary

Để xác nhận UC11 hoàn chỉnh cần test đầy đủ 5 nhóm:

1. Authorization
2. Booking Ownership
3. CheckedIn Booking Validation
4. Menu Item Validation
5. Order Creation & Order Details Verification


# UC11 – Room Service Food Order

## Overview

UC11 là luồng **Room Service Food Order**, cho phép khách đang lưu trú trong resort mở menu nhà hàng và đặt món giao lên phòng.

UC11 hiện **chưa bao gồm**:

* Quản lý bếp.
* Charge tiền vào folio.
* Thanh toán.

UC11 tập trung vào việc:

```text
Customer → Order Food → Create Food Order → View Order Details
```

---

# System Design

## Domain Models

UC11 sử dụng các entity sau:

### Booking

Xác định:

* Khách đang ở phòng nào.
* Booking có đủ điều kiện gọi món hay không.

---

### Room

Dùng để:

* Lấy số phòng.
* Hiển thị thông tin phòng trong order.

---

### User

Xác định:

* Khách đang đăng nhập.
* Chủ sở hữu booking.

---

### MenuCategory

Nhóm món ăn.

Ví dụ:

* Main Course
* Beverage
* Dessert

---

### MenuItem

Danh sách món ăn khách có thể chọn.

---

### FoodOrder

Đại diện cho một đơn gọi món.

---

### FoodOrderItem

Lưu từng món ăn trong đơn.

---

### Kot

Kitchen Order Ticket.

Được tạo khi có FoodOrder để phục vụ luồng bếp sau này.

---

### GuestFolio

Hiện tại chưa sử dụng trong UC11.

Được dùng ở:

```text
UC14 – Charge To Room
```

---

# Service Layer

## FoodOrderService

Service chịu trách nhiệm xử lý toàn bộ nghiệp vụ UC11.

### Chức năng

#### Build Room Service Screen

Từ:

```text
bookingId
```

load dữ liệu màn hình Room Service.

---

#### Validate Booking

Kiểm tra:

* Booking có tồn tại không.
* Booking có thuộc customer hiện tại không.
* Booking có đang CheckedIn không.

---

#### Load Menu

Chỉ load:

```text
IsAvailable = true
IsDeleted = false
```

---

#### Create Order

Tạo:

```text
FoodOrder
FoodOrderItem
```

---

#### Store Historical Price

Lưu:

```text
FoodOrderItem.UnitPrice
```

theo giá tại thời điểm đặt món.

---

#### Create KOT

Tạo phiếu bếp ban đầu.

---

#### Return Result

Trả kết quả bằng:

```text
FoodOrderCreateResult
FoodOrderCreateStatus
```

---

# Controller Layer

## FoodOrdersController

Kết nối UI với Service.

### GET RoomService

```csharp
GET RoomService(int? bookingId)
```

Mở màn hình gọi món.

---

### POST RoomService

```csharp
POST RoomService(RoomServiceOrderViewModel model)
```

Submit order.

---

### Details

```csharp
Details(int id)
```

Xem chi tiết order.

---

## Authorization

Controller sử dụng:

```csharp
[Authorize(Roles = AppRoles.Customer)]
```

Chỉ Customer mới được gọi món.

---

## Current User

User hiện tại được lấy từ:

```text
ClaimsPrincipal
```

Không hard-code UserId.

---

## Error Handling

Service trả lỗi.

Controller map lỗi thành:

```text
Validation Message
User Friendly Message
```

---

# View Layer

## RoomService.cshtml

```text
Views/FoodOrders/RoomService.cshtml
```

Cho phép:

* Xem menu.
* Chọn số lượng.
* Gửi order.

---

## Details.cshtml

```text
Views/FoodOrders/Details.cshtml
```

Hiển thị:

* Thông tin order.
* Danh sách món.
* Tổng tiền.

---

## Profile Integration

Nếu customer có booking CheckedIn:

```text
Profile
  └─ Order Room Service
```

Nút điều hướng sang:

```text
/FoodOrders/RoomService?bookingId={bookingId}
```

---

# Business Flow

## Main Flow

### Step 1

Customer đăng nhập.

---

### Step 2

Customer có booking:

```text
CheckedIn
```

---

### Step 3

Customer mở:

```text
/FoodOrders/RoomService?bookingId={bookingId}
```

---

### Step 4

Hệ thống xác thực:

* Booking tồn tại.
* Booking thuộc customer hiện tại.
* Booking đang CheckedIn.

---

### Step 5

Hệ thống hiển thị menu khả dụng.

---

### Step 6

Customer chọn món và số lượng.

---

### Step 7

Customer submit order.

---

### Step 8

Service validate toàn bộ dữ liệu.

---

### Step 9

Tạo:

```text
FoodOrder
FoodOrderItem
KOT
```

---

### Step 10

Redirect sang:

```text
/FoodOrders/Details/{id}
```

---

# Business Rules

## Authentication & Authorization

Điều kiện:

* User phải đăng nhập.
* User phải có role Customer.

Controller sử dụng:

```csharp
[Authorize(Roles = AppRoles.Customer)]
```

---

## Booking Validation

Booking phải:

### Exist

```text
Booking must exist.
```

---

### Ownership

```text
Booking must belong to current customer.
```

---

### CheckedIn

Chỉ cho phép:

```text
CheckedIn
```

Không cho phép:

* Reserved
* CheckedOut
* Cancelled
* NoShow

---

## Menu Validation

Menu item phải:

```text
IsAvailable = true
IsDeleted = false
```

---

Không cho phép:

* Món hết hàng.
* Món ngừng kinh doanh.

---

Không cho phép:

```text
Duplicate MenuItemId
```

trong cùng một order.

---

## Quantity Validation

Điều kiện:

### At Least One Item

Customer phải chọn ít nhất một món.

---

### Positive Quantity

```text
Quantity > 0
```

---

### Maximum Quantity

```text
Quantity <= 1000
```

---

### Quantity = 0

Được hiểu là:

```text
Item not selected.
```

---

## Price Validation

Khi tạo order:

```text
FoodOrderItem.UnitPrice
```

được copy từ:

```text
MenuItem.Price
```

tại thời điểm đặt món.

---

Không tính lại theo giá hiện tại.

Điều này đảm bảo:

```text
Historical Pricing Integrity
```

nếu giá menu thay đổi trong tương lai.

---

## Payment Rules

UC11 không charge tiền.

Khi tạo order:

```text
IsChargedToRoom = false
ChargedAt = null
```

---

Charge tiền sẽ được xử lý ở:

```text
UC14 – Charge To Room
```

---

## Order Status Rules

Order mới:

```text
Pending
```

---

KOT mới:

```text
Pending
```

---

Luồng bếp:

```text
Pending
→ Cooking
→ Delivered
```

thuộc:

```text
UC13 – Kitchen Processing
```

không nằm trong phạm vi UC11.

---

# Summary

UC11 hiện là một luồng Customer-facing hoàn chỉnh:

```text
Customer Login
        ↓
CheckedIn Booking
        ↓
Room Service Menu
        ↓
Select Food Items
        ↓
Create Food Order
        ↓
Create FoodOrderItems
        ↓
Create KOT
        ↓
View Order Details
```

Phạm vi của UC11 kết thúc sau khi order được tạo thành công và khách xem được chi tiết đơn hàng.

# UC11 – Room Service Food Order Test Scenarios

## Test Accounts

Sử dụng một trong hai tài khoản sau:

### Customer Account 1

| Field     | Value                                                               |
| --------- | ------------------------------------------------------------------- |
| Email     | [user12@phucnguyenresort.test](mailto:user12@phucnguyenresort.test) |
| Password  | Password@123                                                        |
| BookingId | 12                                                                  |
| Room      | 112                                                                 |

### Customer Account 2

| Field     | Value                                                               |
| --------- | ------------------------------------------------------------------- |
| Email     | [user17@phucnguyenresort.test](mailto:user17@phucnguyenresort.test) |
| Password  | Password@123                                                        |
| BookingId | 17                                                                  |
| Room      | 117                                                                 |

> Lưu ý: Password chỉ hoạt động sau khi database đã được seed bằng dữ liệu mới nhất.

---

# Test Case 1 – User Not Logged In

## URL

```text
/FoodOrders/RoomService?bookingId=12
```

## Expected Result

* Redirect đến trang Login.
* Không xem được menu.
* Không thể tạo order.

---

# Test Case 2 – Customer Login Successfully

## Login

```text
/Account/Login
```

### Credentials

```text
Email: user12@phucnguyenresort.test
Password: Password@123
```

## Open Profile

```text
/Profile
```

## Expected Result

* Hiển thị khu vực Room Service.
* Hiển thị Booking #12.
* Hiển thị nút Order Room Service.

## Open Room Service

```text
/FoodOrders/RoomService?bookingId=12
```

## Expected Result

* Hiển thị Booking #12.
* Hiển thị Room 112.
* Hiển thị danh sách món ăn.
* Không hiển thị món unavailable hoặc discontinued.

---

# Test Case 3 – Create Order Successfully

## Action

Tại màn Room Service:

* Chọn quantity = 1 cho một món.
* Chọn quantity = 2 cho một món khác.

Submit order.

## Expected Result

Redirect:

```text
/FoodOrders/Details/{foodOrderId}
```

### Details Page Displays

* FoodOrderId
* BookingId
* CustomerName
* RoomNumber
* OrderStatus
* KitchenStatus
* ItemName
* CategoryName
* Quantity
* UnitPrice
* LineTotal
* TotalAmount

### Business Validation

```text
OrderStatus = Pending
IsChargedToRoom = false
```

UnitPrice phải bằng giá món tại thời điểm order.

---

# Test Case 4 – No Item Selected

## URL

```text
/FoodOrders/RoomService?bookingId=12
```

## Action

Để tất cả quantity = 0.

Submit.

## Expected Result

```text
Please select at least one menu item.
```

* Vẫn ở màn Room Service.
* Menu vẫn hiển thị đầy đủ.

---

# Test Case 5 – Invalid Quantity

## Test Data

```text
-1
```

hoặc

```text
1001
```

Nếu browser chặn, sử dụng DevTools để chỉnh sửa input.

## Expected Result

```text
Menu item quantity must be between 0 and 1000.
```

---

# Test Case 6 – Booking Does Not Exist

## URL

```text
/FoodOrders/RoomService?bookingId=999999
```

## Expected Result

* Hiển thị lỗi booking không tồn tại.
* Không hiển thị form order.

---

# Test Case 7 – Booking Belongs To Another Customer

## Login

```text
user12@phucnguyenresort.test
```

## URL

```text
/FoodOrders/RoomService?bookingId=17
```

## Expected Result

* Không cho gọi món.
* Hiển thị lỗi booking không thuộc user hiện tại.
* Không lộ thông tin booking hoặc order của user khác.

---

# Test Case 8 – Booking Not Checked In

## Preparation

Tạm thời đổi Booking #12 sang trạng thái:

* Reserved
* CheckedOut
* Cancelled
* NoShow

## URL

```text
/FoodOrders/RoomService?bookingId=12
```

## Expected Result

```text
Room service orders are only available for checked-in bookings.
```

Không hiển thị form order.

Sau khi test xong, đổi trạng thái về:

```text
CheckedIn
```

---

# Test Case 9 – Non-Customer Authorization

## Login Account

### Business Manager

| Field     | Value                                                               |
| --------- | ------------------------------------------------------------------- |
| Email     | [user02@phucnguyenresort.test](mailto:user02@phucnguyenresort.test) |
| Password  | Password@123                                                        |
| BookingId | 2                                                                   |
| Role      | Business Manager                                                    |

### URL

```text
/FoodOrders/RoomService?bookingId=2
```

### Expected Result

* Authorization failed.
* Không truy cập được Room Service.

---

### Tour Guide

| Field     | Value                                                               |
| --------- | ------------------------------------------------------------------- |
| Email     | [user07@phucnguyenresort.test](mailto:user07@phucnguyenresort.test) |
| Password  | Password@123                                                        |
| BookingId | 7                                                                   |
| Role      | Tour Guide                                                          |

### URL

```text
/FoodOrders/RoomService?bookingId=7
```

### Expected Result

* Authorization failed.
* Không truy cập được Room Service.

---

# Test Case 10 – Unavailable Menu Item

## Seed Data

| MenuItemId | Availability        |
| ---------- | ------------------- |
| 8          | IsAvailable = false |
| 13         | IsAvailable = false |

## URL

```text
/FoodOrders/RoomService?bookingId=12
```

## Expected Result

Không hiển thị:

* Vegetable Curry
* Sunset Cocktail

---

## Force Submit Using DevTools

Sửa hidden MenuItemId thành:

```text
8
```

hoặc

```text
13
```

Quantity = 1

Submit.

## Expected Result

```text
One or more menu items are unavailable or discontinued.
```

---

# Test Case 11 – Non-Existing Menu Item

## Action

Dùng DevTools đổi MenuItemId:

```text
999999
```

Quantity = 1

Submit.

## Expected Result

```text
One or more menu items do not exist.
```

---

# Test Case 12 – Duplicate Menu Item

## Action

Dùng DevTools duplicate input block để gửi cùng một MenuItemId nhiều lần.

## Expected Result

```text
Duplicate menu items are not allowed in the same order.
```

---

# Test Case 13 – Access Other User's Order Details

## Login

```text
user12@phucnguyenresort.test
```

## URL

```text
/FoodOrders/Details/{foodOrderId-of-user17}
```

## Expected Result

```text
NotFound
```

* Không hiển thị dữ liệu.
* Không lộ thông tin khách khác.

---

# UC11 Acceptance Checklist

UC11 được xem là PASS khi xác nhận được toàn bộ các điều kiện sau:

## Authorization

* [x] User chưa đăng nhập bị chặn.
* [x] Non-Customer bị chặn.

## Booking Validation

* [x] Customer đúng booking truy cập được.
* [x] Booking của user khác bị chặn.
* [x] Booking không CheckedIn bị chặn.

## Order Validation

* [x] Không chọn món bị báo lỗi.
* [x] Quantity không hợp lệ bị báo lỗi.
* [x] Menu unavailable không submit được.
* [x] Menu không tồn tại bị báo lỗi.
* [x] Duplicate menu item bị báo lỗi.

## Successful Order Flow

* [x] Tạo order thành công.
* [x] Redirect sang Details.
* [x] Order mặc định Pending.
* [x] IsChargedToRoom = false.

## Data Security

* [x] Chỉ xem được order của chính mình.
* [x] Không truy cập được order của user khác.
