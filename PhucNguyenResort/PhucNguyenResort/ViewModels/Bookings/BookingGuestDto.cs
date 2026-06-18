using PhucNguyenResort.Enums;
using System;

namespace PhucNguyenResort.ViewModels.Bookings;

public class BookingGuestDto
{
    public string FullName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public GenderType? Gender { get; set; }
    public string PassportNumber { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public bool IsPrimaryBooker { get; set; }
}
