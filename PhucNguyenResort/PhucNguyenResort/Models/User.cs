using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class User
{
    public int UserId { get; set; }

    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [Phone, StringLength(20)]
    public string? PhoneNumber { get; set; }

    public int RoleId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Role? Role { get; set; }
    public UserProfile? UserProfile { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<TableReservation> TableReservations { get; set; } = new List<TableReservation>();
    public ICollection<TourSchedule> GuidedTourSchedules { get; set; } = new List<TourSchedule>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<TourBooking> TourBookings { get; set; } = new List<TourBooking>();

    [InverseProperty("Manager")]
    public ICollection<Tour> ManagedTours { get; set; } = new List<Tour>();
}
