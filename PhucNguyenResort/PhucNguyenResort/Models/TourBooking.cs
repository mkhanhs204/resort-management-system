using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class TourBooking
{
    public int TourBookingId { get; set; }
    public int? BookingId { get; set; }
    public int? UserId { get; set; }
    public int ScheduleId { get; set; }

    [Range(1, 1000)]
    public int? Participants { get; set; }

    [Range(0, 999999999)]
    public decimal? TotalAmount { get; set; }

    public TourBookingStatus BookingStatus { get; set; } = TourBookingStatus.Pending;

    [StringLength(500)]
    public string? CancellationReason { get; set; }

    [StringLength(100)]
    public string? PrimaryGuestName { get; set; }

    [DataType(DataType.Date)]
    public DateTime? PrimaryGuestDateOfBirth { get; set; }

    public GenderType? PrimaryGuestGender { get; set; }

    [StringLength(500)]
    public string? PrimaryGuestPassportNumber { get; set; }

    [StringLength(50)]
    public string? PrimaryGuestNationality { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Booking? Booking { get; set; }
    public TourSchedule? Schedule { get; set; }
    public User? User { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
