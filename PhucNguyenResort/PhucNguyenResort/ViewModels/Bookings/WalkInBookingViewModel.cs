using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.Bookings
{
    public class WalkInBookingViewModel
    {
        [Required(ErrorMessage = "Họ tên khách hàng không được để trống.")]
        [Display(Name = "Họ tên khách đặt")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày nhận phòng.")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày nhận phòng")]
        public DateTime CheckInDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Vui lòng chọn ngày trả phòng.")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày trả phòng")]
        public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);

        [Range(1, 20)]
        [Display(Name = "Số người lớn")]
        public int NumberOfAdults { get; set; } = 2;

        [Range(0, 10)]
        [Display(Name = "Số trẻ em")]
        public int NumberOfChildren { get; set; } = 0;

        [Required(ErrorMessage = "Vui lòng chọn ít nhất một phòng vật lý.")]
        [Display(Name = "Gán số phòng vật lý")]
        public List<int> SelectedRoomIds { get; set; } = new List<int>();

        [Display(Name = "Nhận phòng ngay (Instant Check-in)")]
        public bool InstantCheckIn { get; set; } = true;

        [Range(0, 999999999, ErrorMessage = "Số tiền thanh toán không hợp lệ.")]
        [Display(Name = "Số tiền thực nhận")]
        public decimal PaidAmount { get; set; }

        [Required]
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; } = "Cash"; // Cash, POS, BankTransfer

        [Display(Name = "Đồng ý điều khoản bảo vệ dữ liệu cá nhân (PDPD)")]
        public bool PdpdConsentChecked { get; set; }

        // Danh sách thông tin định danh dùng cho việc Check-in ngay
        public List<BookingGuestDto> Guests { get; set; } = new List<BookingGuestDto>();
    }
}
