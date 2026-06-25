using System;
using System.Collections.Generic;
using PhucNguyenResort.Services.Bookings; // Dòng này giúp nhận diện BookingGuestDto từ tầng Service

namespace PhucNguyenResort.ViewModels.Bookings
{
    public class PreCheckinViewModel
    {
        public int BookingId { get; set; }
        public string? RoomNumber { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public bool PdpdConsentChecked { get; set; }

        // Sử dụng chính xác cấu trúc dữ liệu BookingGuestDto mà Service đang trả về
        public List<BookingGuestDto> Guests { get; set; } = new List<BookingGuestDto>();
    }
}