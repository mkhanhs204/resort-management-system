using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class GuestFolio
{
    public int FolioId { get; set; }
    public int? BookingId { get; set; }

    [Range(0, 999999999)]
    public decimal TotalRoomCharges { get; set; } = 0;

    [Range(0, 999999999)]
    public decimal TotalFoodCharges { get; set; } = 0;

    [Range(0, 999999999)]
    public decimal TotalTourCharges { get; set; } = 0;

    [Range(0, 999999999)]
    public decimal TotalAmount { get; set; } = 0;

    public FolioStatus Status { get; set; } = FolioStatus.Unpaid;

    public Booking? Booking { get; set; }
    public ICollection<FolioTransaction> FolioTransactions { get; set; } = new List<FolioTransaction>();
    public Invoice? Invoice { get; set; }
}
