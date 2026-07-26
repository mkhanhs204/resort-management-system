using PhucNguyenResort.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.Bookings;

public class BookingGuestDto
{
    public string FullName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public GenderType? Gender { get; set; }
    [RegularExpression(@"^(\d{12}|[A-Z]\d{7})$", ErrorMessage = "Số CCCD phải gồm 12 chữ số hoặc Hộ chiếu gồm 1 chữ cái in hoa và 7 chữ số (VD: P1234567).")]
    public string PassportNumber { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public bool IsPrimaryBooker { get; set; }
}
