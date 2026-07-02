namespace PhucNguyenResort.Enums;

public enum TourBookingStatus
{
    Pending,     // Đang chờ xác nhận/thanh toán
    Confirmed,   // Đã xác nhận/Đã thanh toán thành công
    CheckedIn,   // Khách đã có mặt và bắt đầu đi tour
    Completed,   // Tour đã kết thúc tốt đẹp
    Cancelled,   // Khách hoặc hệ thống chủ động hủy trước giờ G
    NoShow       // Đến giờ đi tour nhưng khách không xuất hiện
}