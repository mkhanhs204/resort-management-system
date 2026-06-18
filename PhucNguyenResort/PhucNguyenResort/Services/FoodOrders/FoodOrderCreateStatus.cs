namespace PhucNguyenResort.Services.FoodOrders;

public enum FoodOrderCreateStatus
{
    Created, // Đặt món thành công.
    Forbidden, // Bị cấm. Khách hàng đang cố tình đặt món cho một Booking không thuộc sở hữu của họ.
    BookingNotFound, // Không tìm thấy thông tin Booking dựa trên BookingId được cung cấp.
    BookingNotCheckedIn, // Trạng thái Booking không hợp lệ. Chỉ phòng đã nhận (CheckedIn) mới được phép gọi đồ ăn lên phòng.
    NoItems, // Đơn hàng không có món ăn nào (khách hàng không chọn bất kỳ món gì).
    InvalidQuantity, // Số lượng món ăn không hợp lệ (ví dụ: nhỏ hơn 0 hoặc vượt quá giới hạn 1000).
    MenuItemUnavailable, // Món ăn hiện tại đang hết hàng hoặc đã ngừng kinh doanh (IsAvailable = false hoặc IsDeleted = true).
    MenuItemNotFound, // Một hoặc nhiều món ăn trong đơn hàng không tồn tại trong hệ thống.
    DuplicateMenuItem // Phát hiện các món ăn trùng lặp trong cùng một đơn hàng (ngăn chặn spam request).
}
