namespace PhucNguyenResort.Enums;

public enum TableReservationStatus
{
    Pending,
    Confirmed,
    Seated, // Khách hàng đã được sắp xếp chỗ ngồi
    Completed,
    Cancelled,
    NoShow // Khách hàng không đến
}
