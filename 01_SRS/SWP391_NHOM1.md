# Requirement & Design Specification
## PhucNguyen Resort & Tour Hub
**Version: 1.0**
– Hanoi, June 2026 –

---

**📝 GHI CHÚ TỪ LẬP TRÌNH VIÊN:**
> Dự án này đang được code bằng ngôn ngữ **C#**, theo mô hình **MVC .NET 8**, và sử dụng cơ sở dữ liệu **SQL Server**.

---

## Record of Changes
| Version | Date | A* M, D | In charge | Change Description |
|---|---|---|---|---|
| V1.0 | 15/07 | A | | |
| V1.0 | 16/7 | M | | |
| V1.0 | 16/7 | A | | |
| V1.0 | 17/7 | A | | |
| V1.0 | 18/7 | A | | |

*A - Added M - Modified D - Deleted

---

## Contents
- Record of Changes
- I. Overview
    - 1. System Context
    - 2. User Requirements
        - 2.1 Actors
        - 2.2 Diagrams
        - 2.3 Descriptions
    - 3. Activity Diagram
    - 4. System Functionalities
        - 4.1 Screens Flow
        - 4.3 Non-UI Functions
- II. Functional Requirements
- III. System Design
    - 1. Database Design
        - 1.1 Database Schema
        - Figure III.1: Entity Relationship Diagram
        - 1.2 Table Descriptions
    - 2. Code Packages
        - 2.1 Package Diagram
        - 2.2 Package Descriptions

---

## I. Overview
### 1. System Context
PhucNguyen Resort & Tour Hub is a web-based integrated resort management system that combines hotel booking, restaurant services, tour booking, billing, and operational management into a single platform. The system enables guests to conveniently access resort services while helping staff and managers efficiently manage daily operations, payments, customer information, and business performance through centralized dashboards and reports.

*(Figure I.1: Context Diagram - xem chi tiết trong file PDF)*

### 2. User Requirements
#### 2.1 Actors
| # | Actor | Description |
|---|---|---|
| 1 | Guest | An unauthenticated user who accesses the system without logging in. Guests can browse resort information, search available rooms, view restaurant menus, explore tour packages, read customer reviews, and register for an account. |
| 2 | User | The user represents all external entities, specifically human beings. Users can log in, manage their personal profile, update personal information, and access authenticated system features. |
| 3 | Customer | A registered and authenticated user who actively uses resort services. Customers can book rooms, make deposit payments, reserve restaurant tables, order food, book tours, view invoices, submit reviews, and request personal data deletion. |
| 4 | Admin | The system administrator with the highest level of authority. Responsible for managing the entire system, user accounts, role assignments, account suspension/reactivation, data backup and recovery, and monitoring system activities. |
| 5 | Business Manager | The resort operations manager responsible for managing business-related data such as room categories, restaurant menus, and tour packages. The manager can monitor revenue, occupancy rates, financial reports, customer reviews, and overall business performance. |
| 6 | Receptionist | Front desk staff responsible for managing reservations, verifying guest information, performing check-in and check-out procedures, assigning rooms, updating room statuses, processing final payments, and generating consolidated invoices. |
| 7 | Housekeeping Staff | Staff responsible for maintaining room cleanliness. They can view cleaning tasks, update room cleaning statuses, mark rooms as cleaned, and report maintenance issues when necessary. |
| 8 | Maintenance Staff | Technical staff responsible for inspecting, repairing, and maintaining resort facilities and equipment. They manage maintenance requests, perform repairs, and update maintenance task statuses. |
| 9 | F&B Staff | Restaurant and room service staff responsible for handling food orders, managing table reservations, updating order statuses, delivering food, and charging food expenses to guest room accounts. |
| 10 | Kitchen Staff | Kitchen personnel responsible for receiving Kitchen Order Tickets (KOTs), preparing food, updating food preparation statuses, and marking orders as ready for service. |

#### 2.2 Diagrams
*(Tham khảo file PDF cho các Use Case Diagram: Guest, User, Customer, Business Manager, Receptionist, Housekeeping Staff, Maintenance Staff, F&B Staff, Kitchen Staff, Admin)*

#### 2.3 Descriptions
| ID   | Use Case                            | Actor(s)           | Use Case Description                                                                                                                                                  |
| ------| -------------------------------------| --------------------| -----------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| UC01 | Login                               | Guest              | As a guest, I want to log into the system so that I can access features according to my role.                                                                         |
| UC02 | View Homepage                       | Guest              | As a guest, I want to view the homepage so that I can access resort information and available services.                                                               |
| UC03 | View Rooms                          | Guest              | As a guest, I want to browse available rooms so that I can find suitable accommodation.                                                                               |
| UC04 | Search Room                         | Guest              | As a guest, I want to search rooms based on my preferences so that I can find suitable rooms quickly.                                                                 |
| UC05 | Filter Room                         | Guest              | As a guest, I want to filter rooms by category, price, and capacity so that I can narrow down my options.                                                             |
| UC06 | View Room Details                   | Guest              | As a guest, I want to view room details so that I can make an informed booking decision.                                                                              |
| UC07 | View Restaurant Table               | Guest              | As a guest, I want to view restaurant tables so that I can reserve a table if needed.                                                                                 |
| UC08 | View Table Details                  | Guest              | As a guest, I want to view table details so that I can select a suitable table.                                                                                       |
| UC09 | View Restaurant Menu                | Guest              | As a guest, I want to browse the restaurant menu so that I can explore available dishes.                                                                              |
| UC10 | View Food Details                   | Guest              | As a guest, I want to view food details so that I can know ingredients, prices, and descriptions.                                                                     |
| UC11 | View Tours                          | Guest              | As a guest, I want to browse available tours so that I can plan activities during my stay.                                                                            |
| UC12 | View Tour Details                   | Guest              | As a guest, I want to view detailed tour information so that I can decide whether to book it.                                                                         |
| UC13 | View Resort Information             | Guest              | As a guest, I want to view resort information so that I can learn about facilities and services.                                                                      |
| UC14 | View Reviews                        | Guest              | As a guest, I want to view customer reviews so that I can evaluate service quality.                                                                                   |
| UC15 | View Profile                        | Registered User    | As a registered user, I want to view my profile information so that I can verify my account details.                                                                  |
| UC16 | Update Profile                      | Registered User    | As a registered user, I want to update my profile information so that my account information remains accurate.                                                        |
| UC17 | Update CCCD/Passport                | Registered User    | As a registered user, I want to upload or update my identification documents for verification purposes.                                                               |
| UC18 | Change Password                     | Registered User    | As a registered user, I want to change my password so that I can secure my account.                                                                                   |
| UC19 | Forgot Password                     | Registered User    | As a registered user, I want to reset my password when I forget it so that I can regain access to my account.                                                         |
| UC20 | Logout                              | Registered User    | As a registered user, I want to log out of the system so that my account remains secure.                                                                              |
| UC21 | Register Account                    | Customer           | As a customer, I want to create an account so that I can use resort services.                                                                                         |
| UC22 | Book Room                           | Customer           | As a customer, I want to book a room so that I can stay at the resort.                                                                                                |
| UC23 | Payment                             | Customer           | As a customer, I want to pay deposits and service fees online so that my bookings can be confirmed.                                                                   |
| UC24 | Reserve Restaurant Table            | Customer           | As a customer, I want to reserve a restaurant table so that I can dine at my preferred time.                                                                          |
| UC25 | Order Food To Room                  | Customer           | As a customer, I want to order food to my room so that I can enjoy room service.                                                                                      |
| UC26 | Book Tour                           | Customer           | As a customer, I want to book a tour and choose to pay immediately online OR charge to my room bill so that I can participate in resort activities.                   |
| UC27 | View Personal Invoice               | Customer           | As a customer, I want to view my invoices so that I can track my expenses.                                                                                            |
| UC28 | View Booking History                | Customer           | As a customer, I want to view my booking history so that I can review past reservations.                                                                              |
| UC29 | Submit Reviews                      | Customer           | As a customer, I want to submit reviews so that I can share my experience.                                                                                            |
| UC30 | Receive Notifications               | Customer           | As a customer, I want to receive notifications so that I can stay updated about bookings and services.                                                                |
| UC31 | View Dashboard                      | Admin              | As an administrator, I want to view the system dashboard so that I can monitor system activities.                                                                     |
| UC32 | View Account                        | Admin              | As an administrator, I want to manage user accounts so that I can control system access.                                                                              |
| UC33 | Create Account                      | Admin              | As an administrator, I want to create staff accounts so that employees can access the system.                                                                         |
| UC34 | Search Account                      | Admin              | As an administrator, I want to search user accounts so that I can manage them efficiently.                                                                            |
| UC35 | Sort Account                        | Admin              | As an administrator, I want to sort accounts so that I can find information quickly.                                                                                  |
| UC36 | Block Account                       | Admin              | As an administrator, I want to block accounts that violate system policies.                                                                                           |
| UC37 | Backup Database                     | Admin              | As an administrator, I want to back up the database so that system data is protected.                                                                                 |
| UC38 | Restore Database                    | Admin              | As an administrator, I want to restore the database so that data can be recovered when needed.                                                                        |
| UC39 | View Business Dashboard             | Business Manager   | As a business manager, I want to view business dashboards so that I can monitor resort performance.                                                                   |
| UC40 | View Revenue Overview               | Business Manager   | As a business manager, I want to view revenue summaries so that I can evaluate business income.                                                                       |
| UC41 | View Occupancy Rate                 | Business Manager   | As a business manager, I want to view occupancy rates so that I can monitor room utilization.                                                                         |
| UC42 | Monitor Resort Performance          | Business Manager   | As a business manager, I want to monitor operational performance so that I can improve efficiency.                                                                    |
| UC43 | Analyze Revenue by Service          | Business Manager   | As a business manager, I want to analyze revenue sources so that I can identify profitable services.                                                                  |
| UC44 | View Financial Report               | Business Manager   | As a business manager, I want to view financial reports so that I can evaluate business results.                                                                      |
| UC45 | Filter Report Data                  | Business Manager   | As a business manager, I want to filter report data so that I can focus on specific information.                                                                      |
| UC46 | Export PDF/Excel Report             | Business Manager   | As a business manager, I want to export reports so that I can share and archive business information.                                                                 |
| UC47 | Moderate Customer Reviews           | Business Manager   | As a business manager, I want to manage customer reviews so that inappropriate content can be controlled.                                                             |
| UC48 | View Customer Reviews               | Business Manager   | As a business manager, I want to review customer feedback so that I can improve service quality.                                                                      |
| UC49 | Approve/Hide Review                 | Business Manager   | As a business manager, I want to approve or hide reviews so that displayed feedback remains appropriate.                                                              |
| UC50 | Manage Room Bookings                | Receptionist       | As a receptionist, I want to manage room bookings so that reservations are handled properly.                                                                          |
| UC51 | View Customer & Booking Information | Receptionist       | As a receptionist, I want to view guest information so that I can verify reservations.                                                                                |
| UC52 | View Daily Arrival/Departure List   | Receptionist       | As a receptionist, I want to view daily arrivals and departures so that I can prepare operations.                                                                     |
| UC53 | Perform Customer Check-in           | Receptionist       | As a receptionist, I want to check guests in so that they can begin their stay.                                                                                       |
| UC54 | Assign Room                         | Receptionist       | As a receptionist, I want to assign rooms to guests during check-in.                                                                                                  |
| UC55 | Submit Stay Registration            | Receptionist       | As a receptionist, I want to submit guest stay registration information to comply with regulations.                                                                   |
| UC56 | Perform Customer Check-out          | Receptionist       | As a receptionist, I want to check guests out so that their stay can be completed.                                                                                    |
| UC57 | Generate Consolidated Invoice       | Receptionist       | As a receptionist, I want to generate consolidated invoices including room, food, and unpaid "charge to room" tour charges.                                           |
| UC58 | Process Final Payment               | Receptionist       | As a receptionist, I want to process final payments so that invoices can be settled.                                                                                  |
| UC59 | Update Room Operational Status      | Receptionist       | As a receptionist, I want to update room statuses so that room availability is accurate.                                                                              |
| UC60 | Create Cleaning Task                | Receptionist       | As a receptionist, I want to create cleaning tasks after guest check-out.                                                                                             |
| UC61 | Create Maintenance Task             | Receptionist       | As a receptionist, I want to create maintenance requests when room issues are reported.                                                                               |
| UC62 | View Dashboard                      | Housekeeping Staff | As housekeeping staff, I want to view my dashboard so that I can monitor assigned tasks.                                                                              |
| UC63 | View Room Cleaning Tasks            | Housekeeping Staff | As housekeeping staff, I want to view cleaning tasks so that I know which rooms require cleaning.                                                                     |
| UC64 | Update Room Cleaning Status         | Housekeeping Staff | As housekeeping staff, I want to update cleaning status so that room availability is accurate.                                                                        |
| UC65 | View Cleaning History               | Housekeeping Staff | As housekeeping staff, I want to view cleaning history so that I can track completed tasks.                                                                           |
| UC66 | Report Room Issues                  | Housekeeping Staff | As housekeeping staff, I want to report room issues so that maintenance staff can resolve them.                                                                       |
| UC67 | View Dashboard                      | F&B Staff          | As an F&B Staff, I want to view the operational dashboard so that I can monitor food orders, table reservations, and restaurant activities in real time.              |
| UC68 | View Food Orders                    | F&B Staff          | As an F&B Staff, I want to view all food orders so that I can track customer requests and manage the order workflow efficiently.                                      |
| UC69 | Update Food Order Status            | F&B Staff          | As an F&B Staff, I want to update the status of food orders (Pending → Cooking → Delivered) so that customers and other staff can track order progress.               |
| UC70 | View Table Reservations             | F&B Staff          | As an F&B Staff, I want to view table reservations so that I can prepare tables and manage restaurant seating arrangements.                                           |
| UC71 | Update Table Status                 | F&B Staff          | As an F&B Staff, I want to update table status (Available, Reserved, Occupied) so that reservation information remains accurate.                                      |
| UC72 | Charge Food Bill To Room            | F&B Staff          | As an F&B Staff, I want to charge a customer's food bill directly to their room so that the amount can be included in the final consolidated invoice.                 |
| UC73 | View Dashboard                      | Kitchen Staff      | As a Kitchen Staff member, I want to view the kitchen dashboard so that I can monitor incoming orders and kitchen workload.                                           |
| UC74 | View Kitchen Order Tickets (KOT)    | Kitchen Staff      | As a Kitchen Staff member, I want to view all Kitchen Order Tickets (KOT) so that I can prepare food according to customer requests.                                  |
| UC75 | View KOT Details                    | Kitchen Staff      | As a Kitchen Staff member, I want to view detailed information of a KOT so that I can prepare the correct dishes and quantities.                                      |
| UC76 | Update KOT Status                   | Kitchen Staff      | As a Kitchen Staff member, I want to update the status of a KOT (Pending → Preparing → Ready to Serve) so that F&B Staff can know when orders are ready for delivery. |

### 3. Activity Diagram
*(Tham khảo PDF cho các biểu đồ hoạt động (Activity Diagrams))*
- 3.1 Customer Room Booking Process
- 3.2 Receptionist Check-in Process
- 3.3 Food Ordering & Room Service Process
- 3.4 Tour Booking Process
- 3.5 Customer Check-out & Consolidated Invoice Process
- 3.6 Housekeeping & Maintenance Process

### 4. System Functionalities
#### 4.1 Screens Flow
*(Tham khảo PDF cho Screen Flows (Biểu đồ điều hướng màn hình))*

#### 4.2 Screen Authorization
*(Tham khảo PDF cho Bảng phân quyền màn hình (Screen Authorization Matrix))*

#### 4.3 Non-UI Functions
| # | Feature | System Function | Description |
|---|---|---|---|
| 1 | Update Cleaning Status | (Preparing -> Ready) | |
| 2 | Maintenance Dashboard | | ✓ |
| 3 | Update Cleaning Status | | ✓ |
| 4 | Report Maintenance Issue | | ✓ |
| 5 | System Settings Configuration | | ✓ |
| 6 | Global Account Security (Lock/Unlock) | | ✓ |
| 7 | Account Detail & Activity | | ✓ |
| 8 | Database Backup & Restore Page | | ✓ |
| 9 | System Logs & Audit Trail Page | | ✓ |

---

## II. Functional Requirements
*(Tham khảo file SRS cho Functional Requirements chi tiết)*

## III. System Design
### 1. Database Design
#### 1.1 Database Schema
*(Tham khảo biểu đồ ERD trong PDF)*

#### 1.2 Table Descriptions
| No | Table | Description |
|---|---|---|
| 1 | Roles | ● Primary keys: RoleId <br> ● Foreign keys: None <br> ● Unique keys: RoleName |
| 2 | Users | ● Primary keys: UserId <br> ● Foreign keys: RoleId → Roles(RoleId) <br> ● Unique keys: Email |
| 3 | UserProfiles | ● Primary keys: ProfileId <br> ● Foreign keys: UserId → Users(UserId) <br> ● Unique keys: UserId |
| 4 | RoomCategories | ● Primary keys: CategoryId <br> ● Foreign keys: None <br> ● Unique keys: None |
| 5 | Rooms | ● Primary keys: RoomId <br> ● Foreign keys: CategoryId → RoomCategories(CategoryId) <br> ● Unique keys: RoomNumber |
| 6 | Bookings | ● Primary keys: BookingId <br> ● Foreign keys: UserId → Users(UserId) <br> ● Unique keys: None |
| 7 | BookingDetails | ● Primary keys: BookingDetailId <br> ● Foreign keys: BookingId → Bookings(BookingId), RoomCategoryId → RoomCategories(CategoryId), RoomId (Nullable) → Rooms(RoomId) <br> ● Unique keys: None |
| 8 | BookingGuests | ● Primary keys: BookingGuestId <br> ● Foreign keys: BookingId → Bookings(BookingId) <br> ● Unique keys: None |
| 9 | MenuCategories | ● Primary keys: CategoryId <br> ● Foreign keys: None <br> ● Unique keys: None |
| 10 | MenuItems | ● Primary keys: MenuItemId <br> ● Foreign keys: CategoryId → MenuCategories(CategoryId) <br> ● Unique keys: None |
| 11 | DiningTables | ● Primary keys: TableId <br> ● Foreign keys: None <br> ● Unique keys: None |
| 12 | TableReservations | ● Primary keys: ReservationId <br> ● Foreign keys: UserId → Users(UserId), TableId → DiningTables(TableId) <br> ● Unique keys: None |
| 13 | FoodOrders | ● Primary keys: FoodOrderId <br> ● Foreign keys: BookingId → Bookings(BookingId) <br> ● Unique keys: None |
| 14 | FoodOrderItems | ● Primary keys: OrderItemId <br> ● Foreign keys: FoodOrderId → FoodOrders(FoodOrderId), MenuItemId → MenuItems(MenuItemId) <br> ● Unique keys: None |
| 15 | KOT | ● Primary keys: KotId <br> ● Foreign keys: FoodOrderId → FoodOrders(FoodOrderId) <br> ● Unique keys: FoodOrderId |
| 16 | Tours | ● Primary keys: TourId <br> ● Foreign keys: None <br> ● Unique keys: None |
| 17 | TourSchedules | ● Primary keys: ScheduleId <br> ● Foreign keys: TourId → Tours(TourId), GuideId → Users(UserId) <br> ● Unique keys: None |
| 18 | TourBookings | ● Primary keys: TourBookingId <br> ● Foreign keys: UserId → Users(UserId), BookingId (Nullable) → Bookings(BookingId), ScheduleId → TourSchedules(ScheduleId) <br> ● Unique keys: None |
| 19 | Reviews | ● Primary keys: ReviewId <br> ● Foreign keys: UserId → Users(UserId) |
| 20 | GuestFolios | ● Primary keys: FolioId <br> ● Foreign keys: BookingId → Bookings(BookingId) <br> ● Unique keys: BookingId |
| 21 | FolioTransactions | ● Primary keys: TransactionId <br> ● Foreign keys: FolioId → GuestFolios(FolioId) <br> ● Unique keys: None |
| 22 | Invoices | ● Primary keys: InvoiceId <br> ● Foreign keys: FolioId → GuestFolios(FolioId) <br> ● Unique keys: FolioId |
| 23 | Payments | ● Primary keys: PaymentId <br> ● Foreign keys: InvoiceId (Nullable) → Invoices(InvoiceId), TourBookingId (Nullable) → TourBookings(TourBookingId) <br> ● Unique keys: None |
| 24 | Refunds | ● Primary keys: RefundId <br> ● Foreign keys: BookingId → Bookings(BookingId) <br> ● Unique keys: None |
| 25 | Notifications | ● Primary keys: NotificationId <br> ● Foreign keys: UserId → Users(UserId) <br> ● Unique keys: None |

### 2. Code Packages
#### 2.1 Package Diagram
*(Tham khảo PDF)*

#### 2.2 Package Descriptions
*(Tham khảo PDF)*
