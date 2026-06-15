using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class BookingGuest
{
    public int BookingGuestId { get; set; }
    public int BookingId { get; set; }

    [StringLength(100)]
    public string? FullName { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(500)]
    public string? PassportNumber { get; set; }

    [StringLength(50)]
    public string? Nationality { get; set; }

    public GenderType? Gender { get; set; }
    public bool IsPrimaryBooker { get; set; }

    public Booking? Booking { get; set; }
}
