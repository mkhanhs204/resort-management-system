using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class TourBooking
{
    public int TourBookingId { get; set; }
    public int BookingId { get; set; }
    public int ScheduleId { get; set; }

    [Range(1, 1000)]
    public int? Participants { get; set; }

    [Range(0, 999999999)]
    public decimal? TotalAmount { get; set; }

    public TourBookingStatus BookingStatus { get; set; } = TourBookingStatus.Pending;

    public Booking? Booking { get; set; }
    public TourSchedule? Schedule { get; set; }
}
