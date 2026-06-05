# SOFTWARE DEVELOPMENT PROJECT SPECIFICATION

**Course:** Software Development Project (SWP391)  
**Group Size:** 5 Students  
**Project Code:** SWP391-HOS-02  

# Project Title

## PhucNguyen Resort & Tour Hub
*(Integrated Hospitality Management System)*

### Domain Application
- Hospitality
- Food & Beverage (F&B)
- Tourism
- E-Commerce

---

# 1. Problem Statement

Managing a modern luxury resort involves coordinating multiple disjointed services:

- Room Accommodations
- On-site Dining (Restaurants)
- Local Tour/Activity Bookings

Traditionally, resorts use separate software for:

- Front-desk operations
- Point-of-Sale (POS) for restaurants
- Manual spreadsheets for tour bookings

This fragmentation leads to:

- Double-bookings
- Lost revenue
- Poor customer experience
- Data security vulnerabilities

PhucNguyen Resort & Tour Hub is proposed to solve this by providing an All-in-One integrated web platform.

The system allows Guests to:

- Book rooms
- Order food to rooms
- Schedule tours

while providing Resort Staff with a centralized dashboard to manage operations and generate a consolidated final invoice upon checkout.

---

# 2. Third-Party Tool Interface

The system must integrate the following external APIs:

## 2.1 Payment Gateway API (Stripe / VNPay Sandbox)

### Function
- Securely process online deposits for room bookings
- Handle final checkout payments

---

## 2.2 Email/SMS Service API (SendGrid / Twilio)

### Function
- Automatically send booking confirmations
- Send QR code tickets for tours
- Send digital invoices to guests

---

## 2.3 Weather / Map API (OpenWeather / Google Maps)

### Function
- Display real-time weather forecasts
- Display interactive maps for outdoor tour/activity packages

---

# 3. External Entity Specification (Actors)

## Guest (Customer)

The end-user who:

- Creates an account
- Books rooms
- Reserves restaurant tables
- Books tours
- Views consolidated bills

---

## Receptionist (Front Desk)

Responsible for:

- Check-in / Check-out processes
- Verifying guest identities
- Processing final payments

---

## Service Staff (F&B / Tour Guide)

Responsible for:

- Updating food order statuses
- Managing tour attendee lists

---

## System Manager (Admin)

Responsible for:

- Managing master data
  - Rooms
  - Menu Items
  - Tour Catalogs
  - Staff Accounts
- Viewing financial and occupancy dashboards

---

# 4. Use Cases / User Stories

The project is architected into 5 independent but interconnected modules.

---

# Module 1: Auth, Profiles & Master Data
*(Assigned to Student 1)*

### UC01
As a Guest, I want to register, verify my email, and securely log in.

### UC02
As a Guest, I want to manage my profile and upload my ID/Passport information.

*(Requires explicit consent per Decree 356/2025)*

### UC03
As an Admin, I want to manage staff accounts and assign roles.

### UC04
As an Admin, I want to manage:

- Room Categories
- Restaurant Menus
- Tour Catalogs

(CRUD operations with image uploads)

### UC05
As a Guest, I want to execute my "Right to Deletion" (Article 5, Decree 356/2025) to request the system to erase my personal data after my stay.

---

# Module 2: Room Reservation & Front Desk
*(CORE 1 – Assigned to Student 2)*

### UC06
As a Guest, I want to search for available rooms by:

- Date range
- Guest count
- Room type

### UC07
As a Guest, I want to book a room and pay a deposit securely.

### UC08
As a Receptionist, I want to view a daily dashboard of expected arrivals and departures.

### UC09
As a Receptionist, I want to perform Check-In and Check-Out.

(Check-In includes allocating a specific physical room number)

### UC10
As a Receptionist, I want to update physical room statuses:

- Available
- Occupied
- Needs Cleaning
- Maintenance

---

# Module 3: Restaurant POS & Room Service
*(CORE 2 – Assigned to Student 3)*

### UC11
As a checked-in Guest, I want to browse the restaurant menu and place a food order to be delivered to my room.

### UC12
As a Guest, I want to reserve a dining table for a specific date, time, and party size.

### UC13
As F&B Staff, I want to view a real-time list of incoming food orders and update their statuses.

Status flow:

Pending → Cooking → Delivered

### UC14
As F&B Staff, I want to attach the cost of a restaurant meal directly to a checked-in Guest’s room bill.

(Charge to Room)

---

# Module 4: Tour Booking & Feedback System
*(Assigned to Student 4)*

### UC15
As a Guest, I want to browse local tours/activities, view itineraries, and check weather forecasts.

### UC16
As a Guest, I want to book a tour for a specific date and specify the number of participants.

### UC17
As a Tour Guide, I want to view the manifest (attendee list) for my assigned tours today.

### UC18
As a Guest, after checking out or completing a tour, I want to submit a 5-star rating and written review.

### UC19
As an Admin, I want to moderate guest reviews.

(Hide / Display)

---

# Module 5: Consolidated Billing & Dashboards
*(Assigned to Student 5 – The Integrator)*

### UC20
As a Receptionist, during Check-Out, I want the system to automatically generate a Consolidated Invoice that sums up:

- Room Cost
- Unpaid Food Orders
- Unpaid Tour Bookings

### UC21
As a Receptionist, I want to process the final payment (Cash/Credit) and mark the Consolidated Invoice as Paid.

### UC22
As a Manager, I want to view a visual dashboard showing the Room Occupancy Rate (%) for the current month.

### UC23
As a Manager, I want to view a Revenue Dashboard breaking down income by source:

- Rooms
- F&B
- Tours

### UC24
As a Manager, I want to export the monthly financial report to an Excel/PDF file.

---

# 5. Business Rules

## BR01 – Strict Concurrency (Double-Booking Prevention)

The system must guarantee that:

- A specific room cannot be booked twice
- A specific restaurant table cannot be booked twice

for overlapping dates/times.

---

## BR02 – Consolidated Billing Constraint

A Guest cannot be checked out (UC09) if they have:

- Unpaid food orders (UC14)
- Unpaid tour bookings

All outstanding charges must be settled into the Final Invoice (UC20).

---

## BR03 – Cancellation Policy

- Room deposits are 100% refundable if canceled more than 48 hours before check-in.
- No refund is issued if canceled within 48 hours.

---

# 6. References & Domain Knowledge

## A. Legal Compliance & Data Protection

### Vietnam Decree 356/2025/NĐ-CP on Personal Data Protection

Focus Areas:

- Names
- ID Cards
- Passports
- Payment Information

Applications:

- Right to Deletion
- Explicit Consent

---

### Law on Residence 2020

Focus Area:

Temporary Residence Registration

Application:

Collect required fields:

- Full Name
- DOB
- Passport/ID Number
- Nationality

---

### Vietnam Law on Tourism 2017

Focus Area:

Rights and obligations of tourism service providers

Application:

- Safety warnings
- Insurance inclusions
- Refund policies

---

## B. Hotel & Front Office Operations

### Guest Cycle (AHLEI)

Stages:

1. Arrival
2. Check-In
3. Occupancy
4. Departure (Check-Out)

Room state transitions:

Vacant/Clean
→ Occupied/Clean
→ Occupied/Dirty
→ Vacant/Dirty
→ Vacant/Clean

---

### Vietnam National Standard for Hotels (TCVN 4391:2015)

Focus:

Classification and requirements for:

- 4-star hotels
- 5-star hotels

Application:

Support:

- Room Service
- Spa
- Multiple F&B outlets
- Concierge tours

---

## C. F&B and Restaurant Operations

### KOT (Kitchen Order Ticket) Workflow

Order Status:

Pending
→ Cooking
→ Served

Application:

When a guest orders food, the system must create a KOT record.

---

### Charge-to-Room Integration

Restaurant POS pushes charges into PMS (Front Desk System).

Application:

Food_Order_ID
→ linked to
Room_Booking_ID

---

## D. Hospitality Finance & Reporting

### USALI

Uniform System of Accounts for the Lodging Industry

Revenue categories:

- Rooms Revenue
- Food & Beverage Revenue
- Tour Revenue

---

### Night Audit Process

Daily closing process:

- Calculate occupancy rates
- Finalize unpaid bills
- Roll over business date

---

**End of Project Specification**
