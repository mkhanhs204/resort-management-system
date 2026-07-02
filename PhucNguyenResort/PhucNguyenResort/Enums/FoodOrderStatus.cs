namespace PhucNguyenResort.Enums;

public enum FoodOrderStatus
{
    Pending,    // Moi dat, cho bep xac nhan
    Cooking,    // Bep dang nau (EDS: dong bo tu KotStatus.Cooking)
    Delivered,  // Da giao cho khach
    Cancelled   // Da huy
}
