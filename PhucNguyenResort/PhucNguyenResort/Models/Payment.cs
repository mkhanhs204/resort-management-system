using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class Payment
{
    public int PaymentId { get; set; }
    public int? InvoiceId { get; set; }
    public int? TourBookingId { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }

    [Range(0, 999999999)]
    public decimal? Amount { get; set; }

    public PaymentStatus? PaymentStatus { get; set; }

    [StringLength(100)]
    public string? TransactionCode { get; set; }

    public DateTime? PaidAt { get; set; }

    public Invoice? Invoice { get; set; }
    public TourBooking? TourBooking { get; set; }
}
