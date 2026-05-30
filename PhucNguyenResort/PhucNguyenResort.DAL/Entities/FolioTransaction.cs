using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Entities;

public class FolioTransaction
{
    public int TransactionId { get; set; }
    public int FolioId { get; set; }
    public FolioTransactionType? TransactionType { get; set; }
    public int? ReferenceId { get; set; }

    [Range(0, 999999999)]
    public decimal? Amount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public GuestFolio? Folio { get; set; }
}
