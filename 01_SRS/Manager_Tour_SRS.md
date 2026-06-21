# Software Requirements Specification (SRS) - Tour Management Module

## 1. Overview

The Tour Management Module is a core subsystem of the PhucNguyen Resort & Tour Hub. It enables the resort to offer, manage, and track local tours and activities for guests. The module provides a seamless experience for guests to explore and book tours, allows Tour Guides to manage their daily itineraries and attendee lists, and gives System Administrators full control over the tour catalog and schedules. Furthermore, tour billing is fully integrated into the resort's consolidated billing system, allowing guests to charge tour expenses directly to their room.

## 2. Actors and Roles

| Actor                                | Description                                                                                                                                                           |
| ------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Guest / Customer**           | Browses available tours, views detailed itineraries and weather forecasts, and books tours. They can choose to pay immediately or charge the cost to their room bill. |
| **Admin (System Manager)**     | Manages the master data for tours. This includes creating, updating, and deleting tour catalogs, assigning tour guides, and scheduling tours.                         |
| **Tour Guide (Service Staff)** | Responsible for conducting the tours. They can view the daily manifest (attendee list) for the tours they are assigned to lead.                                       |
| **Receptionist**               | Handles consolidated billing upon guest checkout, ensuring any unpaid tour charges are collected.                                                                     |

## 3. Functional Requirements (Use Cases)

### 3.1. Guest / Customer Features

* **UC-TM-01: View Tours (Guest):** Guests can browse a catalog of available local tours and activities. The system displays tour summaries, pricing, and availability.
* **UC-TM-02: View Tour Details (Guest):** Guests can view in-depth information about a specific tour, including the itinerary, duration, inclusions/exclusions, and real-time weather forecasts (via OpenWeather API) or interactive maps (via Google Maps API).
* **UC-TM-03: Book Tour (Customer):** Customers can book a tour for a specific date and specify the number of participants.
* **UC-TM-04: Tour Payment Integration (Customer):** During booking, customers can choose to pay immediately (via Payment Gateway) or "Charge to Room" so that the tour cost is added to their final consolidated invoice upon check-out.

### 3.2. Admin Features (Tour Catalog Management)

* **UC-TM-05: Manage Tour Catalogs (Admin):** Admins have full CRUD (Create, Read, Update, Delete) capabilities for tour packages. They can upload images, define descriptions, set prices, and specify capacity limits.
* **UC-TM-06: Manage Tour Schedules (Admin):** Admins can schedule specific occurrences of a tour, defining the date, time, and assigning a specific Tour Guide (Service Staff) to lead the session.

### 3.3. Tour Guide Features

* **UC-TM-07: View Daily Manifest (Tour Guide):** Tour guides can log in to view the attendee list (manifest) for their assigned tours on the current day. They can see the number of participants, guest names, and room numbers.

### 3.4. Integration Features

* **UC-TM-08: Consolidated Billing (Receptionist):** When a guest checks out, the system automatically pulls any unpaid tour bookings charged to their room and adds them to the final invoice.

## 4. Database Design (Tour Module Subschema)

The Tour Management module relies on the following database tables:

| Table Name              | Description                                                                                                                    | Key Relationships                                                                                                                                 |
| ----------------------- | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Tours**         | Stores master data for tour packages (Name, Description, Base Price, Images).                                                  | Primary Key:`TourId`                                                                                                                            |
| **TourSchedules** | Stores specific instances of a tour (Date, Time, Assigned Guide).                                                              | Primary Key:`ScheduleId` `<br>` Foreign Keys: `TourId` (Tours), `GuideId` (Users/Roles)                                                   |
| **TourBookings**  | Stores guest bookings for specific tour schedules. Tracks the number of participants and payment status (Paid/Charge to Room). | Primary Key:`TourBookingId` `<br>` Foreign Keys: `UserId` (Users), `BookingId` (Room Bookings - Nullable), `ScheduleId` (TourSchedules) |

## 5. Third-Party Integrations

* **Weather / Map API:** Integrates OpenWeather or Google Maps to display interactive maps and real-time weather forecasts on the Tour Details page.
* **Email/SMS API (SendGrid / Twilio):** Automatically sends booking confirmations and digital/QR code tickets to guests upon successful tour reservation.
* **Payment Gateway (Stripe / VNPay):** Processes online payments if the guest chooses to pay for the tour immediately rather than charging it to their room.

## 6. Business Rules

* **BR-TM-01 (Capacity Limits):** A `TourSchedule` cannot be overbooked. The total number of participants from all `TourBookings` for a specific `ScheduleId` must not exceed the defined maximum capacity of the tour.
* **BR-TM-02 (Billing Constraint):** Guests cannot check out of their room if they have unpaid `TourBookings` marked as "Charge to Room". These must be settled via the consolidated invoice.
* **BR-TM-03 (Cancellation):** Tour cancellations may be subject to refund policies (e.g., 100% refund if canceled 48 hours prior, no refund within 48 hours).
