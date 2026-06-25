namespace PhucNguyenResort.Services.FoodOrders;

public enum FoodOrderCreateStatus
{
    Created,
    Forbidden,
    BookingNotFound,
    BookingNotCheckedIn,
    NoItems,
    InvalidQuantity,
    MenuItemUnavailable,
    MenuItemNotFound,
    DuplicateMenuItem,
    RoomNotSelected,
    BookingRoomNotFound,
    BookingRoomNotAssigned,
    InvalidDeliveryTime,
    InvalidNote
}
