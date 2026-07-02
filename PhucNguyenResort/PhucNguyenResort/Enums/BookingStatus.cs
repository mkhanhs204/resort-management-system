namespace PhucNguyenResort.Enums;

public enum BookingStatus
{
    Reserved,
    Arrived, // Khách hàng đã đến nhưng chưa check-in
    CheckedIn,
    CheckedOut,
    Cancelled,
    NoShow,
    PendingPayment
}
