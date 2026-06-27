using PhucNguyenResort.Models;
using System;
using System.Collections.Generic;

namespace PhucNguyenResort.ViewModels.Bookings
{
    public class InvoiceViewModel
    {
        public int BookingId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime IssuedAt { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string GuestPhone { get; set; } = string.Empty;
        public string RoomNumbers { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime? ActualCheckInTime { get; set; }
        public DateTime? ActualCheckOutTime { get; set; }
        
        public decimal TotalRoomCharges { get; set; }
        public decimal TotalFoodCharges { get; set; }
        public decimal TotalTourCharges { get; set; }
        public decimal LateCheckOutSurcharge { get; set; }
        
        public decimal TotalAmount { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal AmountPaid { get; set; }
        
        public string PaymentMethod { get; set; } = string.Empty;
        public List<FolioTransactionDto> Transactions { get; set; } = new();
    }
}
