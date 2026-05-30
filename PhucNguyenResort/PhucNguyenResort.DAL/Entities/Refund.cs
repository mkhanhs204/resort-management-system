using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Entities;

public class Refund
{
    public int RefundId { get; set; }
    public int? BookingId { get; set; }

    [Range(0, 999999999)]
    public decimal? RefundAmount { get; set; }

    public string? RefundReason { get; set; }
    public RefundStatus? RefundStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Booking? Booking { get; set; }
}
