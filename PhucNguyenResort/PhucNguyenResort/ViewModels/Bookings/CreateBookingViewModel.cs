using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.Bookings
{
    public class CreateBookingViewModel
    {
        public List<RoomSelectionViewModel> RoomSelections { get; set; } = new List<RoomSelectionViewModel>();

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

        // Danh sách Tour mà khách chọn thêm với số lượng slot tùy chỉnh
        public List<TourSelectionViewModel>? SelectedTours { get; set; }

        // Primary Booker Details for Stay Registration (Law on Residence 2020 & PDPD Compliance)
        [Required(ErrorMessage = "Vui lòng nhập Họ tên người đặt phòng chính.")]
        [RegularExpression(@"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂÂÊÔƠỨỪỬỮỰẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăâêôơứừửữựấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪỬỮỰỲÝỴÝỳýỵỷỹ\s]+$", ErrorMessage = "Họ tên chỉ được chứa chữ cái và khoảng trắng.")]
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
        [RegularExpression(@"^(\d{12}|[A-Z]\d{7})$", ErrorMessage = "Số CCCD phải gồm 12 chữ số hoặc Hộ chiếu gồm 1 chữ cái in hoa và 7 chữ số (VD: P1234567).")]
        [Display(Name = "Số CCCD/Hộ chiếu")]
        public string PrimaryGuestPassportNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập Quốc tịch.")]
        [Display(Name = "Quốc tịch")]
        public string PrimaryGuestNationality { get; set; } = string.Empty;

        [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn phải đồng ý với điều khoản bảo vệ dữ liệu cá nhân (PDPD) để tiếp tục.")]
        [Display(Name = "Đồng ý điều khoản PDPD")]
        public bool PdpdConsentChecked { get; set; }
    }

    public class TourSelectionViewModel
    {
        public int ScheduleId { get; set; }
        public int Participants { get; set; }
    }
}
