namespace PhucNguyenResort.ViewModels.TableFoodOrders;

public class TableFoodOrderCheckoutViewModel
{
    public int ReservationId { get; set; }
    public string ReservationCode { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string TableNumber { get; set; } = string.Empty;
    public int PartySize { get; set; }
    public int? BookingId { get; set; }
    public int? SelectedBookingId { get; set; }
    public string? LinkedBookingDisplay { get; set; }
    public bool CanChargeToRoom { get; set; }
    public bool IsChargedToRoom { get; set; }
    public bool IsPaidAtRestaurant { get; set; }
    public decimal TotalAmount { get; set; }
    public List<TableFoodOrderBookingOptionViewModel> BookingOptions { get; set; } = new();
    public List<TableFoodOrderListViewModel> Orders { get; set; } = new();
}

public class TableFoodOrderBookingOptionViewModel
{
    public int BookingId { get; set; }
    public string DisplayText { get; set; } = string.Empty;
}
