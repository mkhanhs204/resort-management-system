using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.Bookings
{
    public class CreateBookingViewModel
    {
        [Required]
        public int RoomId { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày nhận phòng")]
        public DateTime CheckInDate { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày trả phòng")]
        public DateTime CheckOutDate { get; set; }
        
        [Range(1, 20)]
        public int NumberOfAdults { get; set; }

        [Range(0, 10)]
        public int NumberOfChildren { get; set; }

        // Mảng lưu ID của các Tour Schedule mà khách chọn thêm
        public List<int>? SelectedTourScheduleIds { get; set; }

        // Primary Booker Details for Stay Registration (Law on Residence 2020 & PDPD Compliance)
        [Required(ErrorMessage = "Vui lòng nhập Họ tên người đặt phòng chính.")]
        [Display(Name = "Họ tên người đặt chính")]
        public string PrimaryGuestName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập Ngày sinh.")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime? PrimaryGuestDateOfBirth { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Giới tính.")]
        [Display(Name = "Giới tính")]
        public GenderType? PrimaryGuestGender { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số CCCD/Hộ chiếu.")]
        [Display(Name = "Số CCCD/Hộ chiếu")]
        public string PrimaryGuestPassportNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập Quốc tịch.")]
        [Display(Name = "Quốc tịch")]
        public string PrimaryGuestNationality { get; set; } = string.Empty;

        [Display(Name = "Đồng ý điều khoản PDPD")]
        public bool PdpdConsentChecked { get; set; }
    }
}
