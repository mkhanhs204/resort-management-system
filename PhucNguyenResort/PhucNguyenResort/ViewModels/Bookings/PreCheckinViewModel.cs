using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.Bookings
{
    public class PreCheckinViewModel
    {
        public int BookingId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public bool PdpdConsentChecked { get; set; }
        
        [Display(Name = "Chỉ đăng ký khách đến (Khách chưa nhận phòng)")]
        public bool IsArrivalOnly { get; set; }

        public PhucNguyenResort.Enums.BookingStatus CurrentBookingStatus { get; set; }

        public List<BookingGuestDto> Guests { get; set; } = new List<BookingGuestDto>();

        public List<BookingDetailAssignmentViewModel> RoomAssignments { get; set; } = new();
    }

    public class BookingDetailAssignmentViewModel
    {
        public int BookingDetailId { get; set; }
        public int RoomCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        [Display(Name = "Phòng vật lý")]
        public int? SelectedRoomId { get; set; }

        public List<RoomDto> AvailableRooms { get; set; } = new();
    }

    public class RoomDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
    }
}
