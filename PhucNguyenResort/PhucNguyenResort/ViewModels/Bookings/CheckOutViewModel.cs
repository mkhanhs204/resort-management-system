using PhucNguyenResort.Enums;
using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.Bookings
{
    public class CheckOutViewModel
    {
        // --- Booking Info ---
        public int BookingId { get; set; }
        
        [Display(Name = "Tên khách hàng")]
        public string? GuestName { get; set; }
        
        [Display(Name = "Ngày nhận phòng")]
        public DateTime CheckInDate { get; set; }
        
        [Display(Name = "Ngày trả phòng")]
        public DateTime CheckOutDate { get; set; }
        
        [Display(Name = "Thời gian thực nhận")]
        public DateTime? ActualCheckInTime { get; set; }
        
        [Display(Name = "Danh sách phòng")]
        public string? RoomNumbers { get; set; }

        // --- Folio Info ---
        public int FolioId { get; set; }
        
        [Display(Name = "Tiền phòng")]
        public decimal TotalRoomCharges { get; set; }
        
        [Display(Name = "Tiền ăn uống")]
        public decimal TotalFoodCharges { get; set; }
        
        [Display(Name = "Tiền tour")]
        public decimal TotalTourCharges { get; set; }
        
        [Display(Name = "Phụ thu trả muộn")]
        public decimal LateCheckOutSurcharge { get; set; }
        
        [Display(Name = "Tổng cộng")]
        public decimal TotalAmount { get; set; }
        
        [Display(Name = "Tiền cọc")]
        public decimal DepositAmount { get; set; }
        
        [Display(Name = "Cần thanh toán")]
        public decimal BalanceDue { get; set; }

        // --- Transaction Details ---
        public List<FolioTransactionDto> Transactions { get; set; } = new List<FolioTransactionDto>();

        // --- Payment Input ---
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        [Display(Name = "Phương thức thanh toán")]
        public PaymentMethod? SelectedPaymentMethod { get; set; }
        
        [Display(Name = "Ghi chú")]
        public string? PaymentNote { get; set; }
    }

    public class FolioTransactionDto
    {
        public int TransactionId { get; set; }
        public string? TransactionTypeName { get; set; }
        public decimal? Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
