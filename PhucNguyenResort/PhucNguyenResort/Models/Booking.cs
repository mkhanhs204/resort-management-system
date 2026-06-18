using System.ComponentModel.DataAnnotations;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class Booking
{
    public int BookingId { get; set; }
    public int UserId { get; set; }

    [Required, DataType(DataType.Date)]
    public DateTime CheckInDate { get; set; }

    [Required, DataType(DataType.Date)]
    public DateTime CheckOutDate { get; set; }

    [Range(1, 20)]
    public int? NumberOfAdults { get; set; }

    [Range(0, 10)]
    public int? NumberOfChildren { get; set; }

    public BookingStatus BookingStatus { get; set; } = BookingStatus.Reserved;

    [Range(0, 999999999)]
    public decimal? DepositAmount { get; set; }

    [Range(0, 999999999)]
    public decimal? TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public bool PdpdConsentChecked { get; set; }
    public DateTime? ConsentTimestamp { get; set; }

    public User? User { get; set; }
    public ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
    public ICollection<BookingGuest> BookingGuests { get; set; } = new List<BookingGuest>();
    public ICollection<FoodOrder> FoodOrders { get; set; } = new List<FoodOrder>();
    public ICollection<TourBooking> TourBookings { get; set; } = new List<TourBooking>();
    public GuestFolio? GuestFolio { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Refund> Refunds { get; set; } = new List<Refund>();
}
