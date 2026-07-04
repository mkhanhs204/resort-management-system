using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhucNguyenResort.Models;

namespace PhucNguyenResort.ViewModels.Tours
{
    /// <summary>
    /// ViewModel for displaying a single tour card in the browse list.
    /// </summary>
    public class TourCardVM
    {
        public int TourId { get; set; }
        public string TourName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int MaxCapacity { get; set; }
        public int TotalAvailableSlots { get; set; }
        public int DurationHours { get; set; }
        public string? Location { get; set; }
        public string? ImageUrl { get; set; }
        public int UpcomingScheduleCount { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }

    /// <summary>
    /// ViewModel for the tour browse page with filtering.
    /// </summary>
    public class TourListVM
    {
        public List<TourCardVM> Tours { get; set; } = new();
        public string? SearchKeyword { get; set; }
        public string? SortBy { get; set; } // "price_asc", "price_desc", "name", "duration"
        public DateTime? TourDate { get; set; }
        public int TotalCount { get; set; }

        // Pagination
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    /// <summary>
    /// ViewModel for a single schedule row in the tour detail page.
    /// </summary>
    public class ScheduleDisplayVM
    {
        public int ScheduleId { get; set; }
        public DateTime TourDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public int AvailableSlots { get; set; }
        public string? GuideName { get; set; }
        public bool IsFull => AvailableSlots <= 0;
    }

    /// <summary>
    /// ViewModel for the tour detail page.
    /// </summary>
    public class TourDetailVM
    {
        public int TourId { get; set; }
        public string TourName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int MaxCapacity { get; set; }
        public int DurationHours { get; set; }
        public string? Location { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> GalleryImages { get; set; } = new();
        public List<ScheduleDisplayVM> UpcomingSchedules { get; set; } = new();
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public List<PhucNguyenResort.ViewModels.Reviews.ReviewListVM> Reviews { get; set; } = new();
    }

    /// <summary>
    /// ViewModel for the tour booking form (POST).
    /// </summary>
    public class BookTourVM
    {
        public int ScheduleId { get; set; }

        // Display-only fields
        public string TourName { get; set; } = string.Empty;
        public string? TourImage { get; set; }
        public string? Location { get; set; }
        public int DurationHours { get; set; }
        public decimal PricePerPerson { get; set; }
        public DateTime TourDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public int AvailableSlots { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng người tham gia.")]
        [Range(1, 100, ErrorMessage = "Số lượng người phải từ 1 đến 100.")]
        [Display(Name = "Số người tham gia")]
        public int Participants { get; set; } = 1;

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; } = "VNPay"; // "VNPay" or "ChargeToRoom"

        /// <summary>
        /// BookingId for Charge to Room (nullable, only used when PaymentMethod == "ChargeToRoom").
        /// </summary>
        public int? RoomBookingId { get; set; }

        // Primary Booker Details for Stay Registration (Law on Residence 2020 & PDPD Compliance)
        [Required(ErrorMessage = "Vui lòng nhập Họ tên người đặt chính.")]
        [Display(Name = "Họ tên người đặt chính")]
        public string PrimaryGuestName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập Ngày sinh.")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime? PrimaryGuestDateOfBirth { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Giới tính.")]
        [Display(Name = "Giới tính")]
        public PhucNguyenResort.Enums.GenderType? PrimaryGuestGender { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số CCCD/Hộ chiếu.")]
        [Display(Name = "Số CCCD/Hộ chiếu")]
        public string PrimaryGuestPassportNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập Quốc tịch.")]
        [Display(Name = "Quốc tịch")]
        public string PrimaryGuestNationality { get; set; } = string.Empty;

        [Display(Name = "Đồng ý điều khoản PDPD")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn phải đồng ý với điều khoản bảo vệ dữ liệu cá nhân (PDPD).")]
        public bool PdpdConsentChecked { get; set; }

        /// <summary>
        /// Computed total = PricePerPerson * Participants
        /// </summary>
        public decimal TotalAmount => PricePerPerson * Participants;

        /// <summary>
        /// Available room bookings for Charge to Room option (display-only).
        /// </summary>
        public List<RoomBookingOptionVM> AvailableRoomBookings { get; set; } = new();
    }

    /// <summary>
    /// Represents a CheckedIn room booking option for Charge to Room.
    /// </summary>
    public class RoomBookingOptionVM
    {
        public int BookingId { get; set; }
        public string DisplayText { get; set; } = string.Empty; // e.g. "Booking #105 - Room 301, 302"
    }

    /// <summary>
    /// Result of a tour booking operation.
    /// </summary>
    public class TourBookingResultVM
    {
        public bool Success { get; set; }
        public int? TourBookingId { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? PaymentUrl { get; set; } // For VNPay redirect
        public decimal TotalAmount { get; set; }
    }

    /// <summary>
    /// ViewModel for a single row in MyTourBookings page.
    /// </summary>
    public class MyTourBookingItemVM
    {
        public int TourBookingId { get; set; }
        public int TourId { get; set; }
        public string TourName { get; set; } = string.Empty;
        public string? TourImage { get; set; }
        public string? Location { get; set; }
        public DateTime TourDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public int Participants { get; set; }
        public decimal TotalAmount { get; set; }
        public string BookingStatus { get; set; } = string.Empty;
        public bool CanCancel { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CancellationReason { get; set; }
        public bool HasReviewed { get; set; }
    }

    /// <summary>
    /// ViewModel for the BookingSuccess page.
    /// </summary>
    public class TourBookingSuccessVM
    {
        public int TourBookingId { get; set; }
        public string TourName { get; set; } = string.Empty;
        public DateTime TourDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public int Participants { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string QrCodeUrl { get; set; } = string.Empty;
    }
}
