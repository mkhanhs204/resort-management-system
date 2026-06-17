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

        public List<BookingGuestDto> Guests { get; set; } = new List<BookingGuestDto>();
    }
}
