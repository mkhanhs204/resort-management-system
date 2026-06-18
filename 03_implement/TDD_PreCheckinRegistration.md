# TEST-DRIVEN DEVELOPMENT SPECIFICATION

Đặc tả Kiểm thử Hướng Phát triển — Pre-Checkin & Residence Registration

**Document ID**: SWP391-MOD2-TDD-003
**Version**: 1.0
**Date**: 2026-06-14
**Status**: Approve
**Standard**: ISO/IEC/IEEE 29119-3:2021 — Software Testing Part 3: Test Documentation
**Author**: Cuong - Lead Developer / BA
**Reviewed by**: Khanh
**DPO Sign-off**:Khanh
**Approved by**: Cuong
**Classification**: Internal - Confidential

**References**:

- `01_SRS/SWP391_NHOM1.md` — Functional requirements (UC17: Update CCCD/Passport, UC55: Submit Stay Registration)
- `01_SRS/SWP391-HOS-02.md` — Functional requirements (UC02, UC09, Law on Residence 2020)
- `03_implement/EDS_PreCheckinRegistration.md` — Technical Specification
- Nghị định 356/2025/NĐ-CP — Legal basis (Data Privacy & Right to Deletion)

**Quy ước TDD**: Tài liệu này mô tả test cases TRƯỚC khi viết production code.
Thứ tự bắt buộc: viết test → chạy → xác nhận FAIL 🔴 → implement → PASS 🟢 → refactor 🔵.
Test data dùng classification `SYNTHETIC`. Không dùng PII thật của người dùng.

## CHANGELOG

**Policy 4.4 — Immutable History**: Không bao giờ xóa thông tin cũ.

| Ngày      | Người thực hiện | Nội dung thay đổi                                                  |
| ---------- | ------------------- | --------------------------------------------------------------------- |
| 2026-06-14 | Cuong               | Khởi tạo tài liệu — TDD spec cho Pre-Checkin & Stay Registration |

---

## MỤC LỤC

1. Thông tin Module
2. Logic Issues Resolved
3. Test Design Specification (TDS)
4. Test Case Specification
5. Red-Green-Refactor Tracker
6. Entry / Exit Criteria
7. Rollback Plan

---

## 1. Thông tin Module

| Field                 | Value                                                                 |
| --------------------- | --------------------------------------------------------------------- |
| Feature / Gap ID      | FEAT-PRECHECKIN-PDPD                                                  |
| Module                | `Room Reservation & Front Desk - Pre-Checkin`                       |
| Spec gốc             | `SWP391-MOD2-IMP-003`                                               |
| Priority              | 🟢P0                                                                  |
| Sprint                | S3                                                                    |
| Milestone             | M2 - Stay Registration & Compliance                                   |
| Data Classification   | Sensitive-PII (CCCD, Passport, Ngày sinh)                            |
| Compliance Scope      | Nghị định 356/2025/NĐ-CP (PDPD), Luật Cư trú 2020              |
| Upstream Dependencies | `ResortDbContext`, `Microsoft.AspNetCore.DataProtection`          |
| Downstream Consumers  | `ReceptionistCheckinController`, `StayRegistrationReportExporter` |

---

## 2. Logic Issues Resolved

Bắt buộc điền trước khi viết test. Liệt kê các điểm sai lệch giữa tài liệu SRS, thiết kế và thực tế codebase để chuẩn bị kịch bản kiểm thử khớp.

| #  | Spec gốc (sai / thiếu)                                                       | Thực tế (schema / policy)                                                                                    | Fix áp dụng trong test                                                                                                                       |
| -- | ------------------------------------------------------------------------------ | -------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------- |
| L1 | Chưa mô tả cụ thể trường Giới tính (Gender) cho khai báo tạm trú.  | Cần enum `GenderType` (Male = 0, Female = 1, Other = 2) tương thích với biểu mẫu Công an.            | Test sẽ xác thực giá trị `Gender` thuộc tập enum trên.                                                                               |
| L2 | Cho phép nhập CCCD mà không ràng buộc sự đồng ý bảo mật dữ liệu. | Vi phạm Nghị định 356/2025/NĐ-CP về xử lý dữ liệu cá nhân nhạy cảm nếu không có consent.      | Ném lỗi ngoại lệ `REG-001` nếu `PdpdConsentChecked` là `false` khi lưu CCCD.                                                      |
| L3 | Thiếu cơ chế tự động xóa dữ liệu (Right to Deletion) sau chuyến đi. | Dữ liệu CCCD/Ngày sinh không được lưu trữ vô thời hạn. Phải tự động xóa (Purge) sau checkout. | Viết test cho scheduler kiểm tra các booking có `CheckOutDate < Now - 7 ngày` sẽ bị xóa cột `PassportNumber` và `DateOfBirth`. |

---

## 3. Test Design Specification (TDS)

### TDS-01 — Scope / Phạm vi

`BookingGuestRegistration` bao gồm các thành phần được kiểm thử sau:

```
├── Models (Booking, BookingGuest, GenderType)
├── Services
│   ├── BookingGuestService.SaveBookingGuestsAsync() ← ĐỐI TƯỢNG UNIT TEST CHÍNH
│   ├── BookingGuestService.GetBookingGuestsForCheckInAsync() ← ĐỐI TƯỢNG UNIT TEST CHÍNH
│   └── BookingGuestService.PurgeExpiredPiiDataAsync() ← ĐỐI TƯỢNG UNIT TEST CHÍNH
```

### TDS-02 — Test Basis / Cơ sở Kiểm thử

| Source                    | Items Derived                                                                                |
| ------------------------- | -------------------------------------------------------------------------------------------- |
| `SWP391_NHOM1.md` UC17  | Registered user uploads ID/Passport for stay verification                                    |
| `SWP391-HOS-02.md` UC02 | Personal Profile & ID/Passport upload with explicit consent                                  |
| NĐ 356/2025 Điều 9     | Explicit consent for sensitive PII data processing                                           |
| NĐ 356/2025 Điều 5     | Right to Deletion (Xóa thông tin định danh sau khi hoàn thành dịch vụ)               |
| Luật Cư trú 2020       | Đầy đủ thông tin khai báo tạm trú (Tên, Ngày sinh, Giới tính, CCCD, Quốc tịch) |

### TDS-03 — Test Conditions and Coverage Items

| Condition ID | Test Condition                                                                                        | Coverage Item                       | Test Cases     |
| ------------ | ----------------------------------------------------------------------------------------------------- | ----------------------------------- | -------------- |
| TC-COND-001  | Lưu danh sách khách đi cùng thành công khi có PDPD consent                                    | `SaveBookingGuestsAsync`          | `REG-TC-001` |
| TC-COND-002  | Trả về lỗi `REG-001` khi lưu danh sách khách chứa số CCCD nhưng không tích chọn consent | `SaveBookingGuestsAsync`          | `REG-TC-002` |
| TC-COND-003  | Trả về lỗi `REG-002` khi số lượng khách khai báo vượt quá sức chứa booking             | `SaveBookingGuestsAsync`          | `REG-TC-003` |
| TC-COND-004  | Giải mã số CCCD/Passport thành công cho Lễ tân sử dụng check-in                              | `GetBookingGuestsForCheckInAsync` | `REG-TC-004` |
| TC-COND-005  | Tự động xóa (Purge) số CCCD và Ngày sinh của các booking đã checkout > 7 ngày             | `PurgeExpiredPiiDataAsync`        | `REG-TC-005` |

### TDS-04 — Test Techniques / Kỹ thuật Kiểm thử

- **Equivalence Partitioning**: Phân vùng hành vi gửi danh sách khách (Hợp lệ: `<= sức chứa đặt phòng`; Không hợp lệ: `> sức chứa đặt phòng`).
- **Boundary Value Analysis**: Số lượng khách khai báo đúng bằng `NumberOfAdults + NumberOfChildren`.
- **State Transition Testing**: Chuyển đổi trạng thái của Booking (CheckedOut -> Quá hạn 7 ngày -> Purged PII).

### TDS-05 — Test Data Requirements

- **FX-001**: Booking ID = 15, `NumberOfAdults = 2`, `NumberOfChildren = 1`. PDPD Consent = true.
- **FX-002**: Booking ID = 16, `NumberOfAdults = 1`, `NumberOfChildren = 0`. PDPD Consent = false.
- **FX-003**: Booking ID = 17, `BookingStatus = CheckedOut`, `CheckOutDate = Today - 8 days`, chứa CCCD lưu trữ.
- **FX-004**: Booking ID = 18, `BookingStatus = CheckedOut`, `CheckOutDate = Today - 3 days`, chứa CCCD lưu trữ.

---

## 4. Test Case Specification

### REG-TC-001 — Khai báo thông tin khách đi cùng thành công với PDPD Consent

**Severity**: CRITICAL
**Feature Under Test**: `BookingGuestService.SaveBookingGuestsAsync()`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-001

**Preconditions**:

- Sử dụng Fixture FX-001 (Booking ID = 15, sức chứa tối đa 3 người).
- Người dùng đồng ý PDPD (`consentChecked = true`).

**Test Steps**:

1. Tạo danh sách 3 `BookingGuestDto`:
   - Khách 1: "Nguyen Van A", DOB: 15/05/1990, Gender: Male, CCCD: "012345678901", Nationality: "Vietnam", IsPrimaryBooker: true.
   - Khách 2: "Nguyen Thi B", DOB: 20/10/1995, Gender: Female, CCCD: "012345678902", Nationality: "Vietnam", IsPrimaryBooker: false.
   - Khách 3: "Nguyen An C", DOB: 12/12/2020, Gender: Male, CCCD: "", Nationality: "Vietnam", IsPrimaryBooker: false (Trẻ em không bắt buộc CCCD).
2. Gọi `SaveBookingGuestsAsync(15, guestList, true)`.

**Expected Result (PASS)**:

- Hàm trả về `true`.
- Database lưu 3 bản ghi trong bảng `BookingGuests`.
- Trường `PdpdConsentChecked` của `Booking` đổi thành `true`.
- Trường `PassportNumber` của Khách 1 và Khách 2 trong database phải được mã hóa (không lưu plaintext).

---

### REG-TC-002 — Báo lỗi REG-001 khi lưu thông tin CCCD nhưng không tích chọn PDPD Consent

**Severity**: CRITICAL
**Feature Under Test**: `BookingGuestService.SaveBookingGuestsAsync()`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-002

**Preconditions**:

- Sử dụng Fixture FX-002 (Booking ID = 16).
- Người dùng KHÔNG đồng ý PDPD (`consentChecked = false`).

**Test Steps**:

1. Gửi danh sách khách có chứa `PassportNumber = "012345678903"`.
2. Gọi `SaveBookingGuestsAsync(16, guestList, false)`.

**Expected Result (PASS)**:

- Ném ra ngoại lệ `ValidationException` với mã lỗi `REG-001` (PDPD consent is required).
- Không có dữ liệu khách hàng nào được lưu vào database.

---

### REG-TC-003 — Báo lỗi REG-002 khi số lượng khách khai báo vượt quá sức chứa booking

**Severity**: HIGH
**Feature Under Test**: `BookingGuestService.SaveBookingGuestsAsync()`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-003

**Preconditions**:

- Sử dụng Fixture FX-002 (Booking ID = 16, sức chứa tối đa 1 người).
- Người dùng đồng ý PDPD (`consentChecked = true`).

**Test Steps**:

1. Gửi danh sách gồm 2 khách: "Nguyen Van A" và "Nguyen Van B".
2. Gọi `SaveBookingGuestsAsync(16, guestList, true)`.

**Expected Result (PASS)**:

- Ném ra ngoại lệ `ArgumentException` hoặc Custom Exception với mã lỗi `REG-002` (Guest count exceeds booking limit).
- Không có dữ liệu nào được lưu.

---

### REG-TC-004 — Giải mã số CCCD/Passport thành công cho Lễ tân sử dụng check-in

**Severity**: HIGH
**Feature Under Test**: `BookingGuestService.GetBookingGuestsForCheckInAsync()`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-004

**Preconditions**:

- Booking ID = 15 đã lưu danh sách khách đi kèm thành công (CCCD của Khách 1 đã mã hóa dưới dạng cyphertext trong DB).

**Test Steps**:

1. Gọi `GetBookingGuestsForCheckInAsync(15)`.

**Expected Result (PASS)**:

- Trả về danh sách khách hàng.
- Trường `PassportNumber` phải được giải mã về dạng plaintext ban đầu (`"012345678901"`) để lễ tân đối chiếu.

---

### REG-TC-005 — Tự động xóa (Purge) số CCCD và Ngày sinh của các booking đã checkout quá 7 ngày

**Severity**: CRITICAL
**Feature Under Test**: `BookingGuestService.PurgeExpiredPiiDataAsync()`
**TDD Phase**: 🔴 RED
**Condition Ref**: TC-COND-005

**Preconditions**:

- Database chứa 2 booking đã checkout:
  - Booking 17 (CheckedOut cách đây 8 ngày, có CCCD). (FX-003)
  - Booking 18 (CheckedOut cách đây 3 ngày, có CCCD). (FX-004)

**Test Steps**:

1. Gọi `PurgeExpiredPiiDataAsync()`.

**Expected Result (PASS)**:

- Hàm trả về số lượng bản ghi bị xóa PII là `1` (chỉ xử lý Booking 17).
- Trường `PassportNumber` và `DateOfBirth` của khách hàng thuộc Booking 17 trong DB bị chuyển thành `null` hoặc chuỗi rỗng `""`. Họ tên và Quốc tịch vẫn được giữ lại.
- Trường `PassportNumber` và `DateOfBirth` của khách thuộc Booking 18 vẫn được giữ nguyên vẹn (do chưa quá hạn 7 ngày).

---

## 5. Red-Green-Refactor Tracker

| TC ID          | Test File                       | 🔴 RED  | 🟢 GREEN | 🔵 REFACTOR |
| -------------- | ------------------------------- | ------- | -------- | ----------- |
| `REG-TC-001` | `BookingGuestServiceTests.cs` | `[ ]` | `[ ]`  |             |
| `REG-TC-002` | `BookingGuestServiceTests.cs` | `[ ]` | `[ ]`  |             |
| `REG-TC-003` | `BookingGuestServiceTests.cs` | `[ ]` | `[ ]`  |             |
| `REG-TC-004` | `BookingGuestServiceTests.cs` | `[ ]` | `[ ]`  |             |
| `REG-TC-005` | `BookingGuestServiceTests.cs` | `[ ]` | `[ ]`  |             |

---

## 6. Entry / Exit Criteria

**Entry Criteria**

- [ ] Spec kỹ thuật `EDS_PreCheckinRegistration.md` đã được phê duyệt.
- [ ] Model `BookingGuest` đã cấu hình quan hệ Foreign Key đầy đủ.
- [ ] Test project `PhucNguyenResort.Tests` đã cài đặt thư viện Mock và In-Memory Database provider.

**Exit Criteria**

- [ ] 100% test cases trong tài liệu này được chạy và đạt kết quả `PASS` (xanh).
- [ ] Mã nguồn ứng dụng không lưu Plaintext CCCD vào nhật ký log (Audit leak prevention check).

---

## 7. Rollback Plan

```powershell
# Revert implementation files in local branch
git checkout -- PhucNguyenResort.Tests/Services/BookingGuestServiceTests.cs
```
