using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class TourSchedule
{
    public int ScheduleId { get; set; }

    [Required(ErrorMessage = "Please select a tour package.")]
    public int TourId { get; set; }

    [Required(ErrorMessage = "Departure date is required.")]
    [DataType(DataType.Date)]
    public DateTime? TourDate { get; set; }

    [Required(ErrorMessage = "Start time is required.")]
    public TimeSpan? StartTime { get; set; }

    [Required(ErrorMessage = "Available slots is required.")]
    [Range(1, 1000, ErrorMessage = "Available slots must be between 1 and 1000.")]
    public int? AvailableSlots { get; set; }

    [Required(ErrorMessage = "Please select a tour guide.")]
    public int? GuideId { get; set; }

    // Thêm thuộc tính này để fix lỗi CS1061 mà không làm thay đổi Database
    [NotMapped]
    public string Status { get; set; } = "Upcoming";

    public Tour? Tour { get; set; }
    public User? Guide { get; set; }
    public ICollection<TourBooking> TourBookings { get; set; } = new List<TourBooking>();
}