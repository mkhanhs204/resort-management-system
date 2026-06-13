using System;
using System.ComponentModel.DataAnnotations;

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
        
        [Range(1, 10)]
        public int NumberOfGuests { get; set; }
    }
}
