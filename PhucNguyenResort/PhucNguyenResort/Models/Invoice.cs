using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class Invoice
{
    public int InvoiceId { get; set; }
    public int FolioId { get; set; }

    [StringLength(50)]
    public string? InvoiceNumber { get; set; }

    [Range(0, 999999999)]
    public decimal? TotalAmount { get; set; }

    [Range(0, 999999999)]
    public decimal? TaxAmount { get; set; }

    [Range(0, 999999999)]
    public decimal? FinalAmount { get; set; }

    public InvoiceStatus InvoiceStatus { get; set; } = InvoiceStatus.Draft;
    public DateTime IssuedAt { get; set; } = DateTime.Now;

    public GuestFolio? Folio { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
