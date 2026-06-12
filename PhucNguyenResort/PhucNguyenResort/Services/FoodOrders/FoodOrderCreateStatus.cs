namespace PhucNguyenResort.Services.FoodOrders;

public enum FoodOrderCreateStatus
{
    Created,
    BookingNotFound,
    BookingNotCheckedIn,
    NoItems,
    InvalidQuantity,
    MenuItemUnavailable
}
