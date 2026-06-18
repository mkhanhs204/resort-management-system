# ENGINEERING DOCUMENTATION STANDARD (EDS) v2.0

Quy chuan Tai lieu Ky thuat va Dac ta Hien thuc hoa

| Field          | Value                               |
| ----------------| -------------------------------------|
| Document ID    | `PHUCNGUYEN-FOODORDER-IMP-001`      |
| Version        | 2.2                                 |
| Date           | 2026-06-13                          |
| Status         | Approved                            |
| Document Owner | `Ho Minh Khanh`                     |
| Author         | `Ho Minh Khanh - Backend Developer` |
| Reviewed by    | `[Truong Tran Cuong]`               |
| DPO Sign-off   | Approved                            |
| Approved by    | `[Ho Minh Khanh]`                   |
| Last Review    | 2026-06-13                          |
| Based on EDS   | v2.0                                |

> **PHAM VI TAI LIEU**: File nay CHI xu ly **UC25, UC66, UC69, UC73** (Dat do an Room Service, Quan ly trang thai don hang, Charge to Room, Quan ly KOT). UC24 (Dat ban nha hang / Table Reservation) se duoc xu ly trong file `TableReservation_EDS.md` rieng biet.

## CHANGELOG

**Policy 4.4 — Immutable History**: Khong bao gio xoa thong tin cu. Moi thay doi phai ghi vao bang nay.

| Ngay       | Nguoi thuc hien                     | Noi dung thay doi                                                                                                                 |
| ------------| -------------------------------------| -----------------------------------------------------------------------------------------------------------------------------------|
| 2026-06-13 | `Ho Minh Khanh - Backend Developer` | Tao tai lieu lan dau cho Food Order & Room Service (UC25 + UC66 + UC69 + UC73)                                                           |
| 2026-06-13 | `Ho Minh Khanh - Backend Developer` | v2.0: Sua status flow, them anti-duplicate charge, sua bao mat, sua nullable model, dung seed data that                           |
| 2026-06-13 | `Ho Minh Khanh - Backend Developer` | v2.1: Sua kieu tra ve Service (Result Object), chuan hoa Quantity validation, chuan hoa Role Constants, cap nhat policy Migration |
| 2026-06-13 | `Ho Minh Khanh - Backend Developer` | v2.2: Yeu cau truyen currentUserId vao CreateFoodOrderAsync de check quyen so huu booking |

## MUC LUC

1. Tong quan Module
2. Ma tran Truy vet (Traceability Matrix)
3. Architecture Decision Records (ADR)
4. Non-Functional Requirements & SLA
5. Static Modeling (Mo hinh Tinh)
6. Dynamic Modeling (Mo hinh Dong)
7. Domain Event Catalog
8. Interface Specification (Dac ta Giao dien)
9. API Specification
10. Bang ma loi (Error Codes)
11. Quy trinh Trien khai (Step-by-Step)
12. Rollback & Incident Runbook
13. Kich ban Kiem thu Chi tiet (tham chieu TDD Spec)
14. Phuong phap Xac minh
15. Mau thu thuc te (API Verification Samples)
16. Bang tong hop phan quyen (Authorization Matrix)

---

## 1. Tong quan Module

Module **Food Order & Room Service** quan ly toan bo luong nghiep vu dat do an trong he thong PhucNguyen Resort — tu luc khach da Check-in dat mon (Room Service), den khi bep nhan va nau (KOT Workflow), cho den khi ghi no chi phi vao tien phong (Charge to Room).

**Pham vi bao phu**: UC25, UC66, UC69, UC73 (theo SRS ver2 `SWP391_NHOM1_ver2.pdf`).

**KHONG bao gom**: UC24 (Dat ban nha hang) — se co tai lieu EDS rieng.

| Field                  | Value                                                                             |
| ------------------------| -----------------------------------------------------------------------------------| 
| Module Name            | `Food Order & Room Service`                                                       |
| Bounded Context        | `F&B Operations — Order Management`                                               |
| Data Classification    | Internal (co truy cap du lieu khach: ten, phong, booking)                         |
| Compliance Scope       | Kiem tra quyen truy cap du lieu khach hang theo role                               |
| Upstream Dependencies  | `Module 1 (MenuItems, MenuCategories — UC04 CRUD)`, `Module 2 (Bookings — Check-in status)` |
| Downstream Consumers   | `Module 5 (GuestFolio, FolioTransaction — Consolidated Invoice UC57)`             |

---

## 2. Ma tran Truy vet (Traceability Matrix)

**Policy**: Khong viet code neu khong biet code do phuc vu Rule nao.

| Requirement ID | Loai (BR/ADR/US) | Mo ta yeu cau                                                                                               | Thanh phan Code                                                        | Compliance Target | ADR lien quan |
| ----------------| -------------------| ---------------------------------------------------------------------------------------------------------------| ------------------------------------------------------------------------| -------------------| --------------|
| US-FOOD-001    | User Story        | UC25: Khach da check-in xem menu va dat do an len phong (Room Service)                                 | `FoodOrdersController.RoomService()`, `FoodOrderService.CreateFoodOrderAsync()` | —                 | ADR-001       |
| US-FOOD-002    | User Story        | UC68: F&B Staff xem danh sach don hang do an dang den                                                  | `FoodOrdersController.OrderList()`, `FoodOrderService`                 | —                 | —             |
| US-FOOD-003    | User Story        | UC66: F&B Staff cap nhat trang thai don hang (Pending -> Cooking -> Delivered)                          | `FoodOrdersController.UpdateStatus()`, `FoodOrderService`              | —                 | ADR-002       |
| US-FOOD-004    | User Story        | UC69: F&B Staff ghi chi phi bua an vao tien phong (Charge to Room)                                     | `FoodOrderService.ChargeToRoomAsync()`, `GuestFolio`, `FolioTransaction` | —                 | ADR-003       |
| US-FOOD-005    | User Story        | UC73: Kitchen Staff xem KOT va cap nhat trang thai bep (Pending -> Cooking -> Ready -> Completed)  | `KotController`, `KotService`                                          | —                 | ADR-002       |
| BR-FOOD-001   | Business Rule     | Chi Booking co trang thai `CheckedIn` moi duoc phep dat do an Room Service                                   | `FoodOrderService.CreateFoodOrderAsync()` — kiem tra `BookingStatus`   | —                 | —             |
| BR-FOOD-002   | Business Rule     | Chi cho phep dat mon an co `IsAvailable = true` va `IsDeleted = false`                                       | `FoodOrderService.CreateFoodOrderAsync()` — filter MenuItems           | —                 | —             |
| BR-FOOD-003   | Business Rule     | UnitPrice phai duoc snapshot tai thoi diem dat hang; thay doi gia menu sau do khong anh huong don da dat      | `FoodOrderService` — `UnitPrice = menuItem.Price`                      | —                 | ADR-004       |
| BR-FOOD-004   | Business Rule     | **Validation Form Menu**: Tren tung mon submit quantity >= 0. Sau khi loc quantity > 0 phai co it nhat 1 mon. Moi mon quantity <= 1000. | `FoodOrderService.CreateFoodOrderAsync()` — validation                 | —                 | —             |
| BR-FOOD-005   | Business Rule     | Moi FoodOrder tu dong tao 1 KOT tuong ung (quan he 1:1)                                                     | `FoodOrderService.CreateFoodOrderAsync()` — tao `Kot` kem theo        | —                 | —             |
| BR-FOOD-006   | Business Rule     | Khong cho phep check-out neu con FoodOrder chua thanh toan / chua charge to room (BR02 trong SRS)            | `Module 5 — CheckoutService` *(cross-module constraint)*               | —                 | —             |
| BR-FOOD-007   | Business Rule     | Khi Charge to Room, he thong phai tao `FolioTransaction` voi `TransactionType = FoodCharge` va cap nhat `GuestFolio.TotalFoodCharges` | `FoodOrderService.ChargeToRoomAsync()`                                 | —                 | ADR-003       |
| BR-FOOD-008   | Business Rule     | **CHONG CHARGE TRUNG**: Mot FoodOrder chi duoc charge to room MỘT LAN duy nhat. Enforce bang field `IsChargedToRoom` + logic kiem tra `FolioTransaction` da ton tai | `FoodOrderService.ChargeToRoomAsync()` — kiem tra truoc khi tao        | —                 | ADR-005       |
| BR-FOOD-009   | Business Rule     | **PHAN QUYEN DU LIEU**: Customer chi duoc dat mon cho booking cua chinh minh. F&B Staff/Admin co the xem nhieu order. Kitchen Staff chi xem KOT, khong xem billing | `FoodOrdersController`, `KotController` — kiem tra UserId/Role         | —                 | —             |

---

## 3. Architecture Decision Records (ADR)

### ADR-001 — Su dung MVC Server-Side Rendering thay vi SPA + API

| Field      | Value                           |
| ------------| ---------------------------------|
| Status     | Accepted                        |
| Deciders   | `Team + Principal Architect`    |
| Date       | 2026-06-04                      |
| Supersedes | `N/A`                           |

**Boi canh (Context)**
Du an SWP391 yeu cau xay dung he thong quan ly Resort tich hop. Team can quyet dinh giua SPA (React/Vue) + REST API hoac ASP.NET Core MVC server-side rendering.

**Cac phuong an da xem xet (Options Considered)**

| Phuong an     | Mo ta                              | Uu diem                                          | Nhuoc diem                                    |
| ---------------| -------------------------------------| ---------------------------------------------------| ------------------------------------------------|
| SPA + REST API | Frontend React/Vue, Backend Web API | + UX muot, + De scale frontend rieng               | - Phuc tap hon, can 2 project rieng biet      |
| MVC SSR        | ASP.NET Core MVC + Razor Views     | + Don gian, 1 project, + Phu hop team 5 SV        | - Reload toan trang, kho scale frontend rieng  |

**Quyet dinh (Decision)**
Chon **MVC Server-Side Rendering** vi phu hop voi quy mo team (5 sinh vien), thoi gian phat trien ngan, va yeu cau du an hoc thuat.

**He qua (Consequences)**
Tich cuc:
- Don gian hoa deploy va development workflow.
- De debug va trace loi cho sinh vien.

Tieu cuc / Trade-offs:
- Form submission reload toan trang — giam thieu bang AJAX cho mot so chuc nang.

---

### ADR-002 — Dual Status Model: FoodOrderStatus (front-of-house) + KotStatus (back-of-house)

| Field      | Value                           |
| ------------| ---------------------------------|
| Status     | Accepted                        |
| Deciders   | `Student 3 + Team`              |
| Date       | 2026-06-13                      |
| Supersedes | `N/A`                           |

**Boi canh (Context)**
Trong nghiep vu nha hang thuc te, luong trang thai nhin tu phia khach hang/F&B Staff (front-of-house) va phia bep (back-of-house) la khac nhau. Can quyet dinh dung mot trang thai chung hay tach rieng.

**Quyet dinh (Decision)**
Chon **Dual Status** vi dung voi mo hinh KOT (Kitchen Order Ticket) trong nganh F&B:

- **FoodOrderStatus** (Front-of-house — F&B Staff quan ly):
  - `Pending` → `Cooking` → `Delivered` → `Cancelled`
- **KotStatus** (Back-of-house — Kitchen Staff quan ly):
  - `Pending` → `Cooking` → `Ready` → `Completed` → `Cancelled`

> **LUU Y**: FoodOrderStatus hien tai trong code co cac gia tri `Preparing`, `Ready`, `Served` — can REFACTOR lai thanh `Pending`, `Cooking`, `Delivered`, `Cancelled` de khop voi yeu cau SRS (Pending -> Cooking -> Delivered).

---

### ADR-003 — Charge to Room qua GuestFolio & FolioTransaction

| Field      | Value                           |
| ------------| ---------------------------------|
| Status     | Accepted                        |
| Deciders   | `Student 3 + Student 5`         |
| Date       | 2026-06-13                      |
| Supersedes | `N/A`                           |

**Boi canh (Context)**
Khi F&B Staff thuc hien "Charge to Room" (UC69), chi phi bua an can duoc ghi nhan vao hoa don tong hop (Consolidated Invoice) de Receptionist thu tien khi khach Check-out.

**Quyet dinh (Decision)**
Chon **FolioTransaction** vi phu hop voi mo hinh tai chinh khach san chuan (USALI). Moi lan Charge to Room se tao 1 `FolioTransaction` voi `TransactionType = FoodCharge` va `ReferenceId = FoodOrderId`, dong thoi cap nhat `GuestFolio.TotalFoodCharges`.

---

### ADR-004 — Snapshot UnitPrice tai thoi diem dat hang

| Field      | Value                           |
| ------------| ---------------------------------|
| Status     | Accepted                        |
| Deciders   | `Student 3`                     |
| Date       | 2026-06-13                      |
| Supersedes | `N/A`                           |

**Quyet dinh (Decision)**
Khi tao `FoodOrderItem`, copy gia hien tai cua `MenuItem.Price` vao `FoodOrderItem.UnitPrice`. Tu do ve sau, `UnitPrice` la bat bien — thay doi gia menu khong anh huong don hang cu.

---

### ADR-005 — Chong charge to room trung lap (Anti-Duplicate Charge)

| Field      | Value                           |
| ------------| ---------------------------------|
| Status     | Accepted                        |
| Deciders   | `Student 3`                     |
| Date       | 2026-06-13                      |
| Supersedes | `N/A`                           |

**Boi canh (Context)**
UC69 rat de loi neu F&B Staff bam "Charge to Room" nhieu lan cho cung mot don hang. Khong co co che chong trung se dan den sai so tien trong GuestFolio.

**Cac phuong an da xem xet (Options Considered)**

| Phuong an           | Mo ta                                                                           | Uu diem                    | Nhuoc diem                        |
| ---------------------| ---------------------------------------------------------------------------------| ----------------------------| -----------------------------------|
| Field IsChargedToRoom | Them field `IsChargedToRoom` (bool) va `ChargedAt` (DateTime?) vao FoodOrder   | + Kiem tra nhanh O(1)      | - Can migration them field        |
| Query FolioTransaction | Kiem tra `FolioTransaction` da ton tai voi ReferenceId = FoodOrderId           | + Khong can them field     | - Can query DB moi lan           |

**Quyet dinh (Decision)**
Chon **Field IsChargedToRoom** ket hop **Query FolioTransaction** (defense in depth):
1. Them 2 field vao model `FoodOrder`:
   - `IsChargedToRoom` (bool, default = false)
   - `ChargedAt` (DateTime?, default = null)
2. Truoc khi charge: kiem tra `IsChargedToRoom == false` VA khong ton tai `FolioTransaction` voi cung `ReferenceId`.
3. Sau khi charge thanh cong: set `IsChargedToRoom = true`, `ChargedAt = DateTime.Now`.

**He qua (Consequences)**
Tich cuc:
- F&B Staff khong the vo tinh hoac co y charge trung.
- UI co the disable nut "Charge to Room" khi `IsChargedToRoom == true`.

Tieu cuc / Trade-offs:
- Can migration them 2 field moi vao bang `FoodOrders`.

---

## 4. Non-Functional Requirements & SLA

### 4.1. Performance & Availability

| Category     | Requirement                             | Target SLA | Measurement Method | Compliance Basis |
| --------------| -----------------------------------------| ------------| --------------------| ------------------|
| Latency      | Trang Room Service / Menu load          | < 500ms    | Manual test         | —                |
| Latency      | Tao FoodOrder (POST)                    | < 1s       | Manual test         | —                |
| Availability | Uptime (monthly)                        | 99%        | Uptime monitor      | —                |

### 4.2. Data Integrity & Retention

| Category    | Requirement                                   | Target         | Verification Method | Compliance Basis |
| -------------| -----------------------------------------------| ----------------| ---------------------| ------------------|
| Durability  | Zero record loss cho FoodOrders               | RPO = 0        | Transaction log      | —                |
| Consistency | FoodOrder <-> KOT 1:1 integrity               | 100%           | DB constraints       | —                |
| Consistency | FoodOrderItem <-> MenuItem FK integrity        | 100%           | DB constraints       | —                |
| Consistency | FolioTransaction.ReferenceId <-> FoodOrderId   | 100%           | Application logic    | —                |
| Validation  | Server-side validation cho tat ca input        | 100% coverage  | Unit tests           | —                |

### 4.3. Security & Data Access

| Category              | Requirement                                                    | Target          | Verification Method | Compliance Basis |
| -----------------------| ----------------------------------------------------------------| -----------------| ---------------------| ------------------|
| CSRF                  | `[ValidateAntiForgeryToken]` tren POST                         | 100% coverage   | Code review          | OWASP            |
| Authorization         | F&B Staff / Kitchen Staff phan quyen theo role                 | Role-based       | Auth Matrix (S16)    | —                |
| Data Isolation        | Customer chi xem/dat don cua booking cua CHINH MINH            | 100%            | Integration test     | —                |
| Data Isolation        | Kitchen Staff chi xem KOT, KHONG xem thong tin billing/khach  | 100%            | Auth Matrix (S16)    | —                |
| Input Sanitize        | XSS prevention tren tat ca input                               | 100%            | Integration test     | OWASP            |

---

## 5. Static Modeling (Mo hinh Tinh)

### 5.1. Class Diagram

```
+----------------------------+       +-------------------------------+
|   FoodOrdersController     |------>|   IFoodOrderService           |
|   (Controller)             |       |   (Interface)                 |
+----------------------------+       +-------------------------------+
| + RoomService(GET)         |       | + BuildRoomServiceOrderAsync()|
| + RoomService(POST)        |       | + CreateFoodOrderAsync()      |
| + Details(id)              |       | + GetFoodOrderDetailAsync()   |
| + OrderList()              |       | + GetAllOrdersAsync()         |
| + UpdateStatus(POST)       |       | + UpdateOrderStatusAsync()    |
| + ChargeToRoom(POST)       |       | + ChargeToRoomAsync()         |
+----------------------------+       +-------------------------------+
                                                ^
                                                | implements
                                     +-------------------------------+
                                     |   FoodOrderService            |
                                     +-------------------------------+
                                     | - _context: ResortDbContext   |
                                     +-------------------------------+

+----------------------------+       +-------------------------------+
|   KotController            |------>|   IKotService                 |
|   (Controller)             |       |   (Interface)                 |
+----------------------------+       +-------------------------------+
| + Dashboard(GET)           |       | + GetPendingKotsAsync()       |
| + Details(id)              |       | + GetKotDetailAsync()         |
| + UpdateStatus(POST)       |       | + UpdateKotStatusAsync()      |
+----------------------------+       +-------------------------------+
```

### 5.2. Data Structure (EF Core Models)

**FoodOrder Entity** — `Models/FoodOrder.cs`

> **CANH BAO NULLABLE**: Hien tai `TotalAmount` dang khai bao `decimal?` (nullable). Can chuyen thanh `decimal` (required) de dam bao data integrity.

```csharp
public class FoodOrder
{
    public int FoodOrderId { get; set; }             // PK, auto-increment
    public int BookingId { get; set; }                // FK -> Bookings(BookingId) — REQUIRED
    public FoodOrderStatus OrderStatus { get; set; }  // Pending / Cooking / Delivered / Cancelled
        = FoodOrderStatus.Pending;

    [Range(0, 999999999)]
    public decimal TotalAmount { get; set; }          // SHOULD BE REQUIRED (hien tai nullable)

    public DateTime OrderedAt { get; set; }           // Thoi diem dat hang
        = DateTime.Now;

    // --- FIELDS MOI (ADR-005: Chong charge trung) ---
    public bool IsChargedToRoom { get; set; } = false;    // Da charge vao tien phong chua?
    public DateTime? ChargedAt { get; set; }               // Thoi diem charge (null neu chua charge)

    // --- Navigation Properties ---
    public Booking? Booking { get; set; }
    public ICollection<FoodOrderItem> FoodOrderItems { get; set; } = new List<FoodOrderItem>();
    public Kot? Kot { get; set; }                     // 1:1 voi KOT
}
```

**FoodOrderItem Entity** — `Models/FoodOrderItem.cs`

> **CANH BAO NULLABLE**: Hien tai `FoodOrderId` va `MenuItemId` dang khai bao `int?` (nullable). Can chuyen thanh `int` (required) de dam bao FK integrity.

```csharp
public class FoodOrderItem
{
    public int OrderItemId { get; set; }              // PK, auto-increment
    public int FoodOrderId { get; set; }              // FK -> FoodOrders — SHOULD BE REQUIRED
    public int MenuItemId { get; set; }               // FK -> MenuItems — SHOULD BE REQUIRED

    [Range(1, 1000)]
    public int Quantity { get; set; }                 // So luong (1-1000)

    [Range(0, 999999999)]
    public decimal UnitPrice { get; set; }            // Gia snapshot tai thoi diem dat (ADR-004)

    // --- Navigation Properties ---
    public FoodOrder? FoodOrder { get; set; }
    public MenuItem? MenuItem { get; set; }
}
```

**Kot Entity** — `Models/Kot.cs`

```csharp
public class Kot
{
    public int KotId { get; set; }                    // PK, auto-increment
    public int FoodOrderId { get; set; }              // FK -> FoodOrders, Unique (1:1)
    public KotStatus Status { get; set; }             // Pending / Cooking / Ready / Completed / Cancelled
        = KotStatus.Pending;
    public DateTime CreatedAt { get; set; }
        = DateTime.Now;

    public FoodOrder? FoodOrder { get; set; }
}
```

**FolioTransaction Entity** — `Models/FolioTransaction.cs`

> **CANH BAO NULLABLE**: Hien tai `TransactionType` va `Amount` dang nullable. Neu tai lieu yeu cau integrity cao thi can doi dan thanh required o model/config.

```csharp
public class FolioTransaction
{
    public int TransactionId { get; set; }
    public int FolioId { get; set; }                      // FK -> GuestFolios
    public FolioTransactionType TransactionType { get; set; }  // SHOULD BE REQUIRED
    public int? ReferenceId { get; set; }                 // FK -> FoodOrderId khi Type = FoodCharge
    public decimal Amount { get; set; }                   // SHOULD BE REQUIRED

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public GuestFolio? Folio { get; set; }
}
```

### 5.3. Enums — CHUAN MOI (sau refactor)

```csharp
// FoodOrderStatus — Front-of-house (F&B Staff)
// REFACTOR: Bo Preparing, Ready, Served. Thay bang Cooking, Delivered.
public enum FoodOrderStatus
{
    Pending,      // Moi dat, cho bep xac nhan
    Cooking,      // Bep dang nau (= dong bo tu KotStatus.Cooking)
    Delivered,    // Da giao cho khach
    Cancelled     // Da huy
}

// KotStatus — Back-of-house (Kitchen Staff)
// GIU NGUYEN — da dung chuan
public enum KotStatus
{
    Pending,      // KOT moi, cho bep nhan
    Cooking,      // Bep dang nau
    Ready,        // Nau xong, san sang bung
    Completed,    // Da hoan tat (bep da ban giao)
    Cancelled     // Bep tu choi / huy
}
```

### 5.4. Entity Relationship

```
MenuCategories (1) ---- (N) MenuItems (1) ---- (N) FoodOrderItems
                                                        |
                                                        | N
                                                        |
Users (1) -- (N) Bookings (1) ---- (N) FoodOrders (1) -+
                     |                      |
                     | 1                    | 1
                     |                      |
                GuestFolios (1)        Kot (1:1)
                     |
                     | 1
                     |
              FolioTransactions (N)
              (ReferenceId = FoodOrderId khi TransactionType = FoodCharge)
```

---

## 6. Dynamic Modeling (Mo hinh Dong)

### 6.1. Sequence Diagram — UC25: Dat do an Room Service (Happy Path)

```
Customer Browser      FoodOrdersController       FoodOrderService          ResortDbContext
     |                        |                          |                        |
     |-- GET /FoodOrders/     |                          |                        |
     |   RoomService?         |                          |                        |
     |   bookingId=2 ------->|                          |                        |
     |                        |-- BuildRoomServiceOrder  |                        |
     |                        |   Async(2) ------------>|                        |
     |                        |                          |-- Query Bookings ---->|
     |                        |                          |   (BookingId=2,       |
     |                        |                          |    Status=CheckedIn)  |
     |                        |                          |<-- booking found -----|
     |                        |                          |                        |
     |                        |                          |-- KIEM TRA: Booking   |
     |                        |                          |   thuoc User hien tai |
     |                        |                          |   (BR-FOOD-009)       |
     |                        |                          |                        |
     |                        |                          |-- Query MenuItems --->|
     |                        |                          |   (IsAvailable=true,  |
     |                        |                          |    IsDeleted=false)   |
     |                        |                          |<-- menuItems[] -------|
     |                        |<-- RoomServiceOrderVM ---|                        |
     |<-- View(form + menu) --|                          |                        |
     |                        |                          |                        |
     |-- POST /FoodOrders/    |                          |                        |
     |   RoomService -------->|                          |                        |
     |   (BookingId=2,        |-- CreateFoodOrderAsync() |                        |
     |    Items[{id,qty}...]) |   ------------------>   |                        |
     |                        |                          |-- Validate quantity ->|
     |                        |                          |-- Verify booking ---->|
     |                        |                          |-- Verify menuItems -->|
     |                        |                          |-- Create FoodOrder -->|
     |                        |                          |   + FoodOrderItems    |
     |                        |                          |   + Kot (Pending)     |
     |                        |                          |-- SaveChangesAsync()->|
     |                        |                          |<-- FoodOrder (with ID)|
     |                        |<-- Created(orderId) -----|                        |
     |<-- Redirect /Details/  |                          |                        |
     |    {orderId} ----------|                          |                        |
```

### 6.2. Sequence Diagram — UC69: Charge to Room (voi Anti-Duplicate Check)

```
F&B Staff Browser     FoodOrdersController       FoodOrderService          ResortDbContext
     |                        |                          |                        |
     |-- POST /FoodOrders/    |                          |                        |
     |   ChargeToRoom ------->|                          |                        |
     |   (FoodOrderId=4)      |-- ChargeToRoomAsync(4) ->|                        |
     |                        |                          |-- Get FoodOrder(4) -->|
     |                        |                          |   Include Booking     |
     |                        |                          |<-- order (Delivered) -|
     |                        |                          |                        |
     |                        |                          |-- CHECK 1: order     |
     |                        |                          |   .IsChargedToRoom    |
     |                        |                          |   == false?           |
     |                        |                          |   (ADR-005)           |
     |                        |                          |                        |
     |                        |                          |-- CHECK 2: No        |
     |                        |                          |   FolioTransaction    |
     |                        |                          |   with same RefId     |
     |                        |                          |   exists? (defense)   |
     |                        |                          |                        |
     |                        |                          |-- Get/Create          |
     |                        |                          |   GuestFolio for      |
     |                        |                          |   BookingId --------->|
     |                        |                          |<-- folio -------------|
     |                        |                          |                        |
     |                        |                          |-- Create              |
     |                        |                          |   FolioTransaction    |
     |                        |                          |   (Type=FoodCharge,   |
     |                        |                          |    RefId=4,           |
     |                        |                          |    Amount=160000) --->|
     |                        |                          |                        |
     |                        |                          |-- Update FoodOrder:   |
     |                        |                          |   IsChargedToRoom=true|
     |                        |                          |   ChargedAt=Now       |
     |                        |                          |                        |
     |                        |                          |-- Update folio        |
     |                        |                          |   TotalFoodCharges -->|
     |                        |                          |-- SaveChangesAsync()->|
     |                        |<-- Success --------------|                        |
     |<-- Redirect + Success  |                          |                        |
     |    message ------------|                          |                        |
```

### 6.3. State Machine — FoodOrderStatus (CHUAN MOI)

```
                    +------------------------------+
                    |          PENDING              | <-- Khach vua dat xong
                    +----------+-------------------+
                               |
                    +----------v-------------------+
                    |         COOKING               | <-- F&B Staff xac nhan,
                    |                               |     bep bat dau nau
                    +----------+-------------------+
                               |
                    +----------v-------------------+
                    |        DELIVERED              | <-- Da giao cho khach
                    |  (terminal — co the Charge)   |
                    +------------------------------+

                    +------------------------------+
                    |        CANCELLED              | <-- Huy don
                    |  (chi tu PENDING)             |     (Khi da Cooking tro di
                    +------------------------------+      thi KHONG duoc huy)
```

**Quy tac chuyen trang thai hop le:**

| From       | To          | Actor      | Dieu kien                                |
| ----------- | ----------- | ----------- | ----------------------------------------- |
| Pending    | Cooking     | F&B Staff  | —                                        |
| Pending    | Cancelled   | F&B Staff  | Chi khi KOT chua duoc bep nhan          |
| Cooking    | Delivered   | F&B Staff  | Khi KotStatus = Ready hoac Completed     |
| Delivered  | *(khong)*   | —          | Trang thai cuoi — chi co the Charge      |

### 6.4. State Machine — KotStatus (Back-of-house)

```
    PENDING --> COOKING --> READY --> COMPLETED
       |
       +--> CANCELLED (bep tu choi vi het nguyen lieu)
```

---

## 7. Domain Event Catalog

### 7.1. Events Published (Phat ra)

| Event Name                | Trigger                                    | Publisher          | Subscriber(s)                          | Async? |
| ---------------------------| --------------------------------------------| --------------------| ----------------------------------------| --------|
| `FoodOrderCreated`        | Khach dat mon thanh cong                   | `FoodOrderService` | *(Future: Kitchen dashboard push)*     | No     |
| `FoodOrderStatusChanged`  | F&B Staff cap nhat trang thai              | `FoodOrderService` | *(Future: Customer notification)*      | No     |
| `KotStatusChanged`        | Kitchen Staff cap nhat trang thai bep      | `KotService`       | *(Future: F&B Staff dashboard push)*   | No     |
| `FoodOrderChargedToRoom`  | F&B Staff charge don hang vao tien phong   | `FoodOrderService` | `Module 5 — GuestFolio update`         | No     |

> **Luu y**: Giai doan hien tai chua implement event system. Hien tai su dung direct method call.

---

## 8. Interface Specification (Dac ta Giao dien)

### 8.1. Result Objects (MOI)
De controller co the xu ly loi cu the ma khong phai doan qua `bool`, cac phuong thuc cap nhat trang thai va charge se tra ve `FoodOrderActionResult`.

```csharp
public enum FoodOrderActionStatus
{
    Success,
    NotFound,
    InvalidState,
    AlreadyCharged,
    NoFolio,
    Forbidden
}

public class FoodOrderActionResult
{
    public FoodOrderActionStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    
    public static FoodOrderActionResult Succeeded() => new FoodOrderActionResult { Status = FoodOrderActionStatus.Success };
    public static FoodOrderActionResult Failed(FoodOrderActionStatus status, string message) => new FoodOrderActionResult { Status = status, ErrorMessage = message };
}
```

### 8.2. Service Interface — IFoodOrderService

```csharp
// IFoodOrderService.cs
// @version 2.1

namespace PhucNguyenResort.Services.FoodOrders;

public interface IFoodOrderService
{
    // === EXISTING (v1.0) ===
    Task<RoomServiceOrderViewModel?> BuildRoomServiceOrderAsync(int bookingId);
    
    /// <summary>
    /// [UC25] Tao don hang do an.
    /// Bat buoc phai truyen currentUserId vao de service kiem tra quyen so huu booking hoac controller phai tu check (BR-FOOD-009).
    /// </summary>
    Task<FoodOrderCreateResult> CreateFoodOrderAsync(FoodOrderCreateRequest request, int currentUserId);
    
    Task<FoodOrderDetailViewModel?> GetFoodOrderDetailAsync(int foodOrderId);

    // === NEW (v2.0) ===
    Task<IEnumerable<FoodOrderDetailViewModel>> GetAllOrdersAsync(FoodOrderStatus? filterStatus = null);

    /// <summary>
    /// [UC66] F&B Staff cap nhat trang thai don hang.
    /// Tra ve FoodOrderActionResult thay vi bool de xu ly loi ro rang.
    /// @returns NotFound neu don hang khong ton tai.
    /// @returns InvalidState neu chuyen trang thai vi pham quy tac.
    /// </summary>
    Task<FoodOrderActionResult> UpdateOrderStatusAsync(int foodOrderId, FoodOrderStatus newStatus);

    /// <summary>
    /// [UC69] Ghi chi phi don hang vao tien phong.
    /// CHONG CHARGE TRUNG: Kiem tra IsChargedToRoom truoc khi tao FolioTransaction.
    /// @returns NotFound (FOOD-003) neu FoodOrderId khong ton tai.
    /// @returns InvalidState (FOOD-007) neu don hang chua Delivered.
    /// @returns NoFolio (FOOD-008) neu Booking khong co GuestFolio.
    /// @returns AlreadyCharged (FOOD-012) neu don hang DA duoc charge roi (anti-duplicate).
    /// </summary>
    Task<FoodOrderActionResult> ChargeToRoomAsync(int foodOrderId);
}
```

### 8.3. Service Interface — IKotService (MOI)

```csharp
// IKotService.cs
// @version 1.0

namespace PhucNguyenResort.Services.Kots;

public interface IKotService
{
    Task<IEnumerable<KotDetailViewModel>> GetAllKotsAsync(KotStatus? filterStatus = null);
    Task<KotDetailViewModel?> GetKotDetailAsync(int kotId);
    Task<FoodOrderActionResult> UpdateKotStatusAsync(int kotId, KotStatus newStatus);
}
```

---

## 9. API Specification (MVC Routes)

### 9.1. Endpoints Table

**FoodOrdersController:**

| Method | Route                                  | Auth Level    | Required Roles              | Idempotent? | Description                        |
| --------| ----------------------------------------| ---------------| -----------------------------| -------------| ------------------------------------|
| GET    | `/FoodOrders/RoomService?bookingId={id}` | Authenticated | `Customer` (chi booking cua minh) | Yes     | Form dat do an Room Service        |
| POST   | `/FoodOrders/RoomService`              | Authenticated | `Customer` (chi booking cua minh) | No      | Submit dat do an                   |
| GET    | `/FoodOrders/Details/{id}`             | Authenticated | `Customer (own), FnBStaff`  | Yes         | Chi tiet don hang                  |
| GET    | `/FoodOrders/OrderList`                | Authenticated | `FnBStaff`                  | Yes         | Danh sach don hang (F&B Dashboard)|
| POST   | `/FoodOrders/UpdateStatus`             | Authenticated | `FnBStaff`                  | No          | Cap nhat trang thai don hang       |
| POST   | `/FoodOrders/ChargeToRoom`             | Authenticated | `FnBStaff`                  | No          | Ghi no vao tien phong (1 lan duy nhat)|

**KotController (MOI):**

| Method | Route                              | Auth Level    | Required Roles    | Idempotent? | Description                                    |
| --------| ------------------------------------| ---------------| -------------------| -------------| ------------------------------------------------|
| GET    | `/Kot/Dashboard`                   | Authenticated | `KitchenStaff`    | Yes         | Dashboard bep — danh sach KOT (KHONG co billing)|
| GET    | `/Kot/Details/{id}`                | Authenticated | `KitchenStaff`    | Yes         | Chi tiet KOT (chi mon an, KHONG co thong tin khach)|
| POST   | `/Kot/UpdateStatus`                | Authenticated | `KitchenStaff`    | No          | Cap nhat trang thai KOT                        |

### 9.2. Form Fields & Validation

**POST `/FoodOrders/RoomService`** — Dat do an Room Service

| Field                | Type    | Required | Validation Rule          | Error Message                                  |
| ----------------------| ---------| ----------| --------------------------| ------------------------------------------------|
| BookingId             | int     | Yes      | Must exist, CheckedIn, THUOC USER HIEN TAI | "Booking not found" / "only for checked-in" / "not your booking" |
| Items[].MenuItemId    | int     | Yes      | Must exist, IsAvailable  | "Menu item unavailable or discontinued"        |
| Items[].Quantity      | int     | Yes      | **Tu 0 den 1000** (khong duoc am). Sau khi bo loc cac mon co Quantity > 0, **phai co it nhat 1 mon** duoc chon. | "Quantity phai tu 0 den 1000" / "Vui long chon it nhat 1 mon an" |

**POST `/FoodOrders/ChargeToRoom`** — Charge to Room

| Field        | Type | Required | Validation Rule                                                      | Error Message                            |
| --------------| ------| ----------| ----------------------------------------------------------------------| ------------------------------------------|
| FoodOrderId  | int  | Yes      | Must exist, OrderStatus == `Delivered`, IsChargedToRoom == `false`   | "Order must be Delivered" / "Already charged" |

---

## 10. Bang ma loi (Error Codes)

### FoodOrder Errors (Prefix: `FOOD-`)

| Code       | HTTP Status | Message (EN)                                   | Message (VI)                                        | Trigger Condition                                   |
| ------------| -------------| ------------------------------------------------| -----------------------------------------------------| -----------------------------------------------------|
| `FOOD-001` | 400         | Validation failed                              | Du lieu khong hop le                                | Thieu required field hoac gia tri ngoai range       |
| `FOOD-002` | 400         | Order must contain at least one item           | Don hang phai co it nhat mot mon                   | Items rong hoac tat ca quantity = 0                 |
| `FOOD-003` | 404         | Food order not found                           | Khong tim thay don hang                            | FoodOrderId khong ton tai trong DB                  |
| `FOOD-004` | 404         | Booking not found                              | Khong tim thay booking                             | BookingId khong ton tai trong DB                    |
| `FOOD-005` | 403         | Room service only for checked-in bookings      | Chi khach da check-in moi duoc dat mon             | Booking.BookingStatus != CheckedIn                  |
| `FOOD-006` | 409         | Invalid status transition                      | Chuyen trang thai khong hop le                     | Vi pham State Machine (vd: Delivered -> Pending)    |
| `FOOD-007` | 409         | Order must be delivered before charging        | Don hang phai duoc giao truoc khi ghi no           | Charge to Room khi OrderStatus != Delivered         |
| `FOOD-008` | 404         | Guest folio not found for this booking         | Khong tim thay so no cho booking nay               | Booking chua co GuestFolio (Module 5 chua tao)     |
| `FOOD-009` | 409         | Menu item unavailable or discontinued          | Mon an tam het hoac da ngung kinh doanh            | MenuItem.IsAvailable=false hoac IsDeleted=true      |
| `FOOD-010` | 403         | Insufficient permissions                       | Khong du quyen truy cap                            | User khong co role phu hop                          |
| `FOOD-011` | 500         | Internal server error                          | Loi he thong                                       | Exception khong xu ly duoc                          |
| `FOOD-012` | 409         | Order already charged to room                  | Don hang da duoc ghi no vao phong roi              | IsChargedToRoom == true (ADR-005: chong charge trung)|
| `FOOD-013` | 403         | Booking does not belong to current user        | Booking khong thuoc ve nguoi dung hien tai         | Customer co dat mon cho booking cua nguoi khac      |

### KOT Errors (Prefix: `KOT-`)

| Code      | HTTP Status | Message (EN)                     | Message (VI)                              | Trigger Condition                         |
| -----------| -------------| ----------------------------------| -------------------------------------------| -------------------------------------------|
| `KOT-001` | 400         | Validation failed                | Du lieu khong hop le                      | Thieu required field                      |
| `KOT-002` | 404         | KOT not found                   | Khong tim thay KOT                       | KotId khong ton tai                       |
| `KOT-003` | 409         | Invalid KOT status transition   | Chuyen trang thai KOT khong hop le       | Vi pham State Machine                    |
| `KOT-004` | 403         | Insufficient permissions         | Khong du quyen truy cap                  | User khong co role KitchenStaff           |

---

## 11. Quy trinh Trien khai (Step-by-Step)

> **MIGRATION POLICY**: Migration **chi duoc tao tu dong** tu model/config bang lenh `Add-Migration`. **Khong viet business logic hoac chinh tay** trong cac file migration.

### 11.1. Prerequisites

- [x] Database schema cho bang `FoodOrders`, `FoodOrderItems`, `Kot` da co (migration)
- [x] Seed data cho `MenuCategories` (20 records) va `MenuItems` (20 records) da ton tai
- [x] Seed data cho `Bookings` da co 4 booking CheckedIn: **BookingId = 2, 7, 12, 17**
- [x] `FoodOrderService` (v1.0) da implement: BuildRoomServiceOrder, CreateFoodOrder, GetFoodOrderDetail
- [ ] Authentication/Authorization phai duoc implement truoc (Module 1). **Centralize Role Constants** (vi du: `AppRoles.FnBStaff`, `AppRoles.Customer`, khong hardcode string).
- [ ] Migration them field `IsChargedToRoom`, `ChargedAt` vao bang `FoodOrders`

### 11.2. Implementation Steps

**Chang 1 — Refactor FoodOrderStatus enum** PENDING

```csharp
// Doi tu:  Pending, Preparing, Ready, Served, Cancelled
// Thanh:   Pending, Cooking, Delivered, Cancelled
```

- Cap nhat `Enums/FoodOrderStatus.cs`
- Cap nhat toan bo code references
- Cap nhat seed data
- Tao migration moi tu dong bang lenh ef tools

**Chang 2 — Them field chong charge trung** PENDING

```csharp
// Them vao Models/FoodOrder.cs:
public bool IsChargedToRoom { get; set; } = false;
public DateTime? ChargedAt { get; set; }
```

- Tao migration: `dotnet ef migrations add AddChargeToRoomFields`

**Chang 3 — Service Layer v2.1 (UC13 + UC14)** PENDING

- Them kieu `FoodOrderActionResult` va enum `FoodOrderActionStatus`
- Them `GetAllOrdersAsync()` — Lay danh sach don hang cho F&B Staff
- Them `UpdateOrderStatusAsync()` — Cap nhat trang thai (tra ve result object kem State Machine validation)
- Them `ChargeToRoomAsync()` — Tao FolioTransaction + cap nhat GuestFolio + set IsChargedToRoom=true
- Sua doi `CreateFoodOrderAsync()` — Cap nhat logic check quantity theo form menu (loc item co qty > 0, bo qua qty = 0, validate >= 0)
- Them kiem tra quyen: Bat buoc truyen `currentUserId` vao service hoac controller phai check ownership truoc khi goi service de dam bao Customer chi dat cho booking cua minh (BR-FOOD-009).

**Chang 4 — Controller v2.1 (UC13 + UC14)** PENDING

- Them `OrderList` action — F&B Staff dashboard
- Them `UpdateStatus` POST action
- Them `ChargeToRoom` POST action (xu ly theo `FoodOrderActionResult`)

**Chang 5 — KotController & KotService (UC73-76)** PENDING

- Tao `Services/Kots/IKotService.cs`
- Tao `Services/Kots/KotService.cs`
- Tao `Controllers/KotController.cs` — Dashboard (CHI hien KOT, khong hien billing)

**Chang 6 — Views** PENDING

- `Views/FoodOrders/OrderList.cshtml` — Danh sach don hang (F&B Staff)
- `Views/Kot/Dashboard.cshtml` — Dashboard bep (chi mon an, khong thong tin khach)
- `Views/Kot/Details.cshtml` — Chi tiet KOT

**Chang 7 — Authorization** PENDING

- Dung hang so Roles (e.g. `[Authorize(Roles = AppRoles.Customer)]`)
- Kiem tra UserId trong action RoomService de dam bao Customer chi xem don cua minh.

### 11.3. Deployment Checklist

- [ ] Migration them IsChargedToRoom, ChargedAt thanh cong (khong chinh tay)
- [ ] FoodOrderStatus enum da refactor dung
- [ ] Seed data da cap nhat theo enum moi
- [ ] Role constants da duoc ap dung
- [ ] Authorization middleware active
- [ ] State Machine validation hoat dong dung
- [ ] ChargeToRoom CHONG TRUNG hoat dong dung
- [ ] Customer chi dat duoc cho booking cua minh
- [ ] Kitchen Staff chi xem KOT, khong xem billing
- [ ] Smoke test toan bo flow: Dat mon -> Bep nau -> Giao -> Charge to Room (1 lan)

---

## 12. Rollback & Incident Runbook

### 12.1. Dieu kien kich hoat Rollback (Trigger Conditions)

| Dieu kien                                                  | Nguong              | Nguoi quyet dinh |
| ------------------------------------------------------------| ---------------------| ------------------|
| CRUD operation gay data corruption (FoodOrder mat du lieu) | Bat ky case nao     | Tech Lead         |
| ChargeToRoom tao SAI so tien hoac TRUNG lap               | Bat ky case nao     | Tech Lead         |
| Migration that bai                                          | Ngay lap tuc        | Developer         |
| State Machine cho phep chuyen trang thai sai               | > 3 lan trong 1h   | Developer         |

### 12.2. Rollback Procedure

```bash
# Buoc 1: Revert migration neu can (development only)
dotnet ef database update [PreviousMigration] --project PhucNguyenResort

# Buoc 2: Revert code
git checkout -- Controllers/FoodOrdersController.cs
git checkout -- Controllers/KotController.cs
git checkout -- Services/FoodOrders/
git checkout -- Services/Kots/
git checkout -- Enums/FoodOrderStatus.cs

# Buoc 3: Verify rollback
dotnet build
dotnet run
```

### 12.3. Data Recovery — Sai so tien FolioTransaction

```sql
-- Tim cac FolioTransaction sai do ChargeToRoom
SELECT ft.TransactionId, ft.FolioId, ft.Amount, ft.ReferenceId, fo.TotalAmount
FROM FolioTransactions ft
JOIN FoodOrders fo ON ft.ReferenceId = fo.FoodOrderId
WHERE ft.TransactionType = 2  -- FoodCharge
  AND ft.Amount != fo.TotalAmount;

-- Tim cac don bi charge trung (KHONG duoc co > 1 FolioTransaction cho cung FoodOrderId)
SELECT ft.ReferenceId, COUNT(*) AS ChargeCount
FROM FolioTransactions ft
WHERE ft.TransactionType = 2
GROUP BY ft.ReferenceId
HAVING COUNT(*) > 1;
```

---

## 13. Kich ban Kiem thu Chi tiet

*(Vui long xem chi tiet tai tai lieu `FoodOrder_TDD.md` — se tao sau khi EDS duoc duyet)*

### 13.1. Unit Tests — Preview

**TC-UNIT-001 — Tao FoodOrder thanh cong voi menu nhieu lua chon**

```gherkin
Feature: Create Food Order (Room Service)
  Background:
    Given test data classification: SYNTHETIC
    And Booking(2) exists with BookingStatus = CheckedIn, UserId = 2
    And MenuItem(1, "Pho Bo", Price=85000, IsAvailable=true)
    And MenuItem(7, "Beef Steak", Price=235000, IsAvailable=true)
    And MenuItem(9, "Cake", Price=100000, IsAvailable=true)

  Scenario: Happy path — Tao don hang voi cac mon quantity = 0 duoc bo qua
    Given BookingId = 2, Current UserId = 2
    And Items = [{MenuItemId: 1, Quantity: 2}, {MenuItemId: 7, Quantity: 1}, {MenuItemId: 9, Quantity: 0}]
    When CreateFoodOrderAsync() duoc goi
    Then result.Status == Created
    And FoodOrderItem chua chinh xac 2 mon (bo qua MenuItemId=9)
    And FoodOrder.TotalAmount == 405000

  Scenario: Error — Tat ca mon deu quantity = 0
    Given Items = [{MenuItemId: 1, Quantity: 0}, {MenuItemId: 7, Quantity: 0}]
    When CreateFoodOrderAsync() duoc goi
    Then result.Status == NoItems
```

**TC-UNIT-002 — Charge to Room (chong trung)**

```gherkin
Feature: Charge Food Order to Room
  Background:
    Given FoodOrder(4) with OrderStatus = Delivered, TotalAmount = 160000
    And Booking(4) has GuestFolio(4) with TotalFoodCharges = 0

  Scenario: Happy path — Charge lan dau
    Given FoodOrder(4).IsChargedToRoom == false
    When ChargeToRoomAsync(4) duoc goi
    Then result.Status == Success
    And FolioTransaction created with:
      | Field           | Value       |
      | FolioId         | 4           |
      | TransactionType | FoodCharge  |
      | ReferenceId     | 4           |
      | Amount          | 160000      |
    And FoodOrder(4).IsChargedToRoom == true
    And FoodOrder(4).ChargedAt != null
    And GuestFolio(4).TotalFoodCharges == 160000

  Scenario: Error — Charge lan thu 2 (CHONG TRUNG)
    Given FoodOrder(4).IsChargedToRoom == true (da charge roi)
    When ChargeToRoomAsync(4) duoc goi LAN NUA
    Then result.Status == AlreadyCharged
    And KHONG tao FolioTransaction moi
```

---

## 14. Phuong phap Xac minh

### 14.1. Database Inspection

```sql
-- Verify FoodOrder vua tao (dung seed data that: BookingId = 2, 7, 12, 17)
SELECT fo.FoodOrderId, fo.BookingId, fo.OrderStatus, fo.TotalAmount,
       fo.IsChargedToRoom, fo.ChargedAt, fo.OrderedAt,
       k.KotId, k.Status AS KotStatus
FROM FoodOrders fo
LEFT JOIN Kot k ON fo.FoodOrderId = k.FoodOrderId
WHERE fo.BookingId = 2;

-- Verify FoodOrderItems (snapshot price)
SELECT foi.OrderItemId, mi.ItemName, foi.Quantity, foi.UnitPrice,
       mi.Price AS CurrentMenuPrice,
       (foi.Quantity * foi.UnitPrice) AS LineTotal
FROM FoodOrderItems foi
JOIN MenuItems mi ON foi.MenuItemId = mi.MenuItemId
WHERE foi.FoodOrderId = [id];

-- Verify CHONG CHARGE TRUNG: khong co don nao bi charge > 1 lan
SELECT fo.FoodOrderId, fo.IsChargedToRoom, COUNT(ft.TransactionId) AS ChargeCount
FROM FoodOrders fo
LEFT JOIN FolioTransactions ft ON ft.ReferenceId = fo.FoodOrderId AND ft.TransactionType = 2
GROUP BY fo.FoodOrderId, fo.IsChargedToRoom
HAVING COUNT(ft.TransactionId) > 1;
-- Expected: 0 rows

-- Verify FK integrity: moi FoodOrder co dung 1 KOT
SELECT fo.FoodOrderId, COUNT(k.KotId) AS KotCount
FROM FoodOrders fo
LEFT JOIN Kot k ON fo.FoodOrderId = k.FoodOrderId
GROUP BY fo.FoodOrderId
HAVING COUNT(k.KotId) != 1;
-- Expected: 0 rows
```

### 14.2. UI Verification (dung seed data that)

```
1. Dang nhap voi User co BookingId = 2 (Tran Thi Binh, CheckedIn, Room 102)
   Navigate to /FoodOrders/RoomService?bookingId=2
   -> Verify: Menu hien thi day du (18 mon IsAvailable=true, bo mon 8 va 13, mac dinh cac mon co quantity = 0).

2. Submit form voi Pho Bo (qty=2) + Beef Steak (qty=1), cac mon khac de quantity = 0
   -> Verify: Redirect den /FoodOrders/Details/{newId}
   -> Verify: Don hang chi chua 2 mon duoc chon.
   -> Verify: TotalAmount = 85000*2 + 235000*1 = 405,000 VND
   -> Verify: Status = Pending, IsChargedToRoom = false

3. Dang nhap voi User KHAC, truy cap /FoodOrders/RoomService?bookingId=2
   -> Verify: Bi chan (403 hoac redirect) vi booking khong thuoc ho (BR-FOOD-009)

4. Navigate to /FoodOrders/RoomService?bookingId=1
   (BookingId 1 co BookingStatus = Reserved)
   -> Verify: Message "Room service orders are only available for checked-in bookings."

5. Dang nhap F&B Staff, vao /FoodOrders/OrderList
   -> Verify: Danh sach don hang hien thi, co filter theo status.

6. Click "Charge to Room" tren mot don Delivered
   -> Verify: Thanh cong, IsChargedToRoom = true
   -> Click "Charge to Room" LAN NUA tren cung don
   -> Verify: Bi chan, bao loi "Don hang da duoc ghi no vao phong roi" (FOOD-012)
```

---

## 15. Mau thu thuc te (Manual Verification Samples)

### 15.1. Happy Path — Dat do an Room Service (dung seed that)

```
1. GET /FoodOrders/RoomService?bookingId=2
   -> Status: 200 OK
   -> Booking: Tran Thi Binh, Room 102, CheckedIn
   -> Menu: 18 mon (bo Vegetable Curry va Sunset Cocktail vi IsAvailable=false)

2. POST /FoodOrders/RoomService
   Form data:
     BookingId = 2
     Items[0].MenuItemId = 1   (Pho Bo, 85,000 VND)
     Items[0].Quantity   = 2
     Items[1].MenuItemId = 7   (Beef Steak, 235,000 VND)
     Items[1].Quantity   = 1
     Items[2].MenuItemId = 9   (Chocolate Cake)
     Items[2].Quantity   = 0   (Se bi bo qua do quantity = 0)
   -> Status: 302 Redirect to /FoodOrders/Details/{newId}
   -> DB: FoodOrders chua record moi (OrderStatus=Pending, TotalAmount=405000)
   -> DB: Kot chua record moi (Status=Pending)
   -> DB: FoodOrderItems chua 2 records (UnitPrice = snapshot)
```

### 15.2. Happy Path — Charge to Room (dung seed FoodOrderId=4)

```
Precondition: FoodOrder(4) da duoc cap nhat thanh Delivered
1. POST /FoodOrders/ChargeToRoom
   Form data:
     FoodOrderId = 4
   -> Status: 302 Redirect (success)
   -> DB: FoodOrder(4).IsChargedToRoom = true, ChargedAt = [now]
   -> DB: FolioTransactions chua record moi:
         TransactionType = FoodCharge
         ReferenceId = 4
         Amount = 160,000 VND
   -> DB: GuestFolios.TotalFoodCharges da tang dung so tien
```

### 15.3. Error Path — Charge trung (FOOD-012)

```
Precondition: FoodOrder(4).IsChargedToRoom = true (da charge o buoc tren)
2. POST /FoodOrders/ChargeToRoom
   Form data:
     FoodOrderId = 4
   -> Status: 200 (re-render voi loi) hoac JSON error message
   -> Message: "Don hang da duoc ghi no vao phong roi" (Status: AlreadyCharged)
   -> DB: KHONG co FolioTransaction moi nao duoc tao
```

### 15.4. Error Path — Khach chua Check-in

```
GET /FoodOrders/RoomService?bookingId=1
  (BookingId 1: Nguyen Van An, BookingStatus = Reserved)
-> Status: 200
-> Message: "Room service orders are only available for checked-in bookings."
-> Form bi disable, khong cho submit
```

---

## 16. Bang tong hop phan quyen (Authorization Matrix)

**NOTE**: `Role` mapping phai duoc lay tu Constants trung tam cua he thong, vi du `AppRoles.Customer`, `AppRoles.FnBStaff`, `AppRoles.KitchenStaff` chu khong hardcode string `"Customer"`.

| Route                                       | GUEST | CUSTOMER      | ADMIN | MANAGER | RECEPTIONIST | F&B STAFF | KITCHEN STAFF |
| ---------------------------------------------| -------| ---------------| -------| ---------| --------------| -----------| --------------|
| GET `/FoodOrders/RoomService?bookingId=`    | X     | OK (own only) | X     | X       | X            | X         | X            |
| POST `/FoodOrders/RoomService`              | X     | OK (own only) | X     | X       | X            | X         | X            |
| GET `/FoodOrders/Details/{id}`              | X     | OK (own only) | OK    | OK      | X            | OK        | X            |
| GET `/FoodOrders/OrderList`                 | X     | X             | OK    | OK      | X            | OK        | X            |
| POST `/FoodOrders/UpdateStatus`             | X     | X             | X     | X       | X            | OK        | X            |
| POST `/FoodOrders/ChargeToRoom`             | X     | X             | X     | X       | X            | OK        | X            |
| GET `/Kot/Dashboard`                        | X     | X             | X     | X       | X            | X         | OK           |
| GET `/Kot/Details/{id}`                     | X     | X             | X     | X       | X            | X         | OK           |
| POST `/Kot/UpdateStatus`                    | X     | X             | X     | X       | X            | X         | OK           |

Chu thich:
- OK = Duoc phep
- OK (own only) = Chi duoc phep voi don hang/booking cua chinh minh
- X = Bi tu choi (403 hoac redirect login)

**Luu y Data Isolation:**
- Customer: Chi xem/dat don cho booking cua CHINH MINH (kiem tra UserId)
- Kitchen Staff: Chi xem KOT va mon an. KHONG duoc xem: ten khach, so phong, so tien, billing info
- F&B Staff: Xem duoc don hang + thong tin phong de phuc vu, nhung KHONG xem du lieu ca nhan (CCCD, passport)

---

## PHU LUC

### A. Glossary (Thuat ngu)

| Thuat ngu          | Dinh nghia                                                                                     |
| --------------------| -------------------------------------------------------------------------------------------------|
| KOT                | Kitchen Order Ticket — Phieu yeu cau bep, chua danh sach mon an can nau                      |
| Room Service       | Dich vu phuc vu do an tai phong cho khach da Check-in                                         |
| Charge to Room     | Ghi no chi phi an uong vao tien phong, thanh toan khi Check-out                               |
| Front-of-house     | Phia phuc vu khach (F&B Staff, Receptionist)                                                   |
| Back-of-house      | Phia bep (Kitchen Staff)                                                                        |
| GuestFolio         | So ghi no tong hop cua khach trong suot thoi gian luu tru                                    |
| FolioTransaction   | Mot giao dich ghi no/ghi co trong GuestFolio                                                 |
| Snapshot Price     | Gia ban duoc sao chep co dinh tai thoi diem dat hang                                          |
| USALI              | Uniform System of Accounts for the Lodging Industry                                            |
| Anti-Duplicate     | Co che chong thuc hien trung lap (vd: chong charge 2 lan)                                     |
| Result Object      | Kieu du lieu tra ve tu Service (`FoodOrderActionResult`), chua trang thai (Enum) va Message    |

### B. Tai lieu tham chieu

| Document                                | Path                                           |
| -----------------------------------------| -------------------------------------------------|
| SRS — Project Specification             | `01_SRS/SWP391-HOS-02.md`                       |
| SRS — Requirement & Design             | `01_SRS/SWP391_NHOM1.md`                        |
| Module 3 Analysis                        | `module3.md`                                     |
| EDS Template v2.0                        | `template/EDS_TEMPLATE_V2.0.md`                  |
| TDD Spec — Food Order *(upcoming)*      | `03_implement/FoodOrder_TDD.md`                  |
| FoodOrder Entity Model                   | `PhucNguyenResort/Models/FoodOrder.cs`           |
| FoodOrderItem Entity Model              | `PhucNguyenResort/Models/FoodOrderItem.cs`       |
| KOT Entity Model                        | `PhucNguyenResort/Models/Kot.cs`                 |
| FoodOrder Service Interface             | `PhucNguyenResort/Services/FoodOrders/IFoodOrderService.cs` |
| FoodOrder Service Implementation        | `PhucNguyenResort/Services/FoodOrders/FoodOrderService.cs`  |
| FoodOrder Controller                     | `PhucNguyenResort/Controllers/FoodOrdersController.cs`      |
| GuestFolio Entity (Module 5)            | `PhucNguyenResort/Models/GuestFolio.cs`          |
| FolioTransaction Entity (Module 5)      | `PhucNguyenResort/Models/FolioTransaction.cs`    |

### C. Seed Data Reference (Bookings CheckedIn)

| BookingId | UserId | User Name        | RoomId | Room Number | BookingStatus |
| -----------| --------| ------------------| --------| -------------| ---------------|
| 2         | 2      | Tran Thi Binh    | 2      | 102         | CheckedIn     |
| 7         | 7      | Dang Ngoc Linh   | 7      | 107         | CheckedIn     |
| 12        | 12     | Phan My Quyen    | 12     | 112         | CheckedIn     |
| 17        | 17     | Dinh Nhat Vy     | 17     | 117         | CheckedIn     |

### D. Seed Data Reference (MenuItems Available)

| MenuItemId | ItemName            | CategoryName      | Price (VND)  | IsAvailable |
| ------------| ---------------------| --------------------| -------------- | -------------|
| 1          | Pho Bo              | Breakfast          | 85,000       | true        |
| 2          | Bun Cha             | Appetizer          | 110,000      | true        |
| 3          | Spring Rolls        | Soup               | 135,000      | true        |
| 4          | Seafood Soup        | Salad              | 160,000      | true        |
| 5          | Caesar Salad        | Seafood            | 185,000      | true        |
| 6          | Grilled Prawns      | Vietnamese Food    | 210,000      | true        |
| 7          | Beef Steak          | Western Food       | 235,000      | true        |
| 8          | Vegetable Curry     | Vegetarian         | 260,000      | **false**   |
| 9          | Chocolate Cake      | Dessert            | 285,000      | true        |
| 10         | Vietnamese Coffee   | Coffee             | 310,000      | true        |
| 11         | Lotus Tea           | Tea                | 335,000      | true        |
| 12         | Mango Juice         | Juice              | 360,000      | true        |
| 13         | Sunset Cocktail     | Cocktail           | 385,000      | **false**   |
| 14         | Blue Lagoon Mocktail| Mocktail           | 410,000      | true        |
| 15         | Kids Pasta          | Kids Meal          | 435,000      | true        |
| 16         | Club Sandwich       | Room Service Set   | 460,000      | true        |
| 17         | BBQ Platter         | BBQ                | 485,000      | true        |
| 18         | Seafood Noodle      | Noodle             | 510,000      | true        |
| 19         | Chicken Fried Rice  | Rice Dish          | 535,000      | true        |
| 20         | Resort Combo        | Special Combo      | 560,000      | true        |
