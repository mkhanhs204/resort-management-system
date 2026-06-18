using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.Users
{
    public class ProfileVM
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [StringLength(100)]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(50)]
        [Display(Name = "Quốc tịch")]
        public string? Nationality { get; set; }

        [Display(Name = "Số hộ chiếu/CMND")]
        public string? PassportNumberString { get; set; } // Dùng để hiển thị chuỗi sau khi giải mã từ byte[]

        [StringLength(255)]
        [Display(Name = "Ảnh hộ chiếu")]
        public string? PassportImage { get; set; }

        [StringLength(255)]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        // Thuộc tính bổ sung để nhận file upload từ form chỉnh sửa
        [Display(Name = "Tải lên ảnh hộ chiếu mới")]
        public IFormFile? PassportImageFile { get; set; }

        public List<ProfileBookingVM> CheckedInBookings { get; set; } = new();
    }

    public class ProfileBookingVM
    {
        public int BookingId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
