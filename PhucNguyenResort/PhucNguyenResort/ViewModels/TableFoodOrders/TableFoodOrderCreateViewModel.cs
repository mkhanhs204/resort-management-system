using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.TableFoodOrders;

public class TableFoodOrderCreateViewModel
{
    [Required]
    public int ReservationId { get; set; }

    public string ReservationCode { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string TableNumber { get; set; } = string.Empty;
    public int PartySize { get; set; }

    [StringLength(500)]
    public string? CustomerNote { get; set; }

    public decimal TotalAmount { get; set; }
    public bool IsChargedToRoom { get; set; }
    public bool IsPaidAtRestaurant { get; set; }
    public List<TableFoodOrderMenuItemViewModel> Items { get; set; } = new();
}
