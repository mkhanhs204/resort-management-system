using System;
using System.Collections.Generic;

namespace PhucNguyenResort.ViewModels.Dashboard
{
    public class FinancialReportViewModel
    {
        public int? Month { get; set; }
        public int? Year { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalRoomRevenue { get; set; }
        public decimal TotalFnBRevenue { get; set; }
        public decimal TotalTourRevenue { get; set; }
        public double AverageOccupancyRate { get; set; }
        public int TotalBookings { get; set; }
        public int CompletedBookings { get; set; }
        public int CancelledBookings { get; set; }
        public int TotalInvoices { get; set; }
        public int PaidInvoices { get; set; }
        
        public List<DailyRevenue> DailyRevenues { get; set; } = new List<DailyRevenue>();
        public List<DailyOccupancy> DailyOccupancies { get; set; } = new List<DailyOccupancy>();
    }
}
