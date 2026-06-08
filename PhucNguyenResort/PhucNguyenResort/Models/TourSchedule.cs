using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class TourSchedule
{
    public int ScheduleId { get; set; }
    public int TourId { get; set; }

    [DataType(DataType.Date)]
    public DateTime? TourDate { get; set; }

    public TimeSpan? StartTime { get; set; }

    [Range(0, 1000)]
    public int? AvailableSlots { get; set; }

    public int? GuideId { get; set; }

    public Tour? Tour { get; set; }
    public User? Guide { get; set; }
    public ICollection<TourBooking> TourBookings { get; set; } = new List<TourBooking>();
}
