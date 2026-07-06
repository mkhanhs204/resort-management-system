using System;
using System.Collections.Generic;
using PhucNguyenResort.ViewModels.TableReservations;

namespace PhucNguyenResort.ViewModels.Dashboard
{
    public class ManagerDashboardViewModel
    {
        public OccupancyData Occupancy { get; set; }
        public RevenueBreakdownData Revenue { get; set; }
        public int TotalRoomCategories { get; set; }
        public int TotalTours { get; set; }
        public int PendingRefunds { get; set; }
        
        // Backward compatibility for Table Reservations
        public FnBTableReservationDashboardViewModel TableReservationDashboard { get; set; }

        public int? SelectedMonth { get; set; }
        public int? SelectedYear { get; set; }
    }

    public class OccupancyData
    {
        public int TotalRooms { get; set; }
        public int OccupiedRooms { get; set; }
        public double OccupancyRatePercent { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public List<DailyOccupancy> DailyData { get; set; } = new List<DailyOccupancy>();
    }

    public class DailyOccupancy
    {
        public DateTime Date { get; set; }
        public int TotalRooms { get; set; }
        public int OccupiedRooms { get; set; }
        public double OccupancyRatePercent { get; set; }
    }

    public class RevenueBreakdownData
    {
        public decimal TotalRevenue { get; set; }
        public decimal RoomRevenue { get; set; }
        public decimal FnBRevenue { get; set; }
        public decimal TourRevenue { get; set; }
        public double RoomPercentage { get; set; }
        public double FnBPercentage { get; set; }
        public double TourPercentage { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public List<DailyRevenue> DailyData { get; set; } = new List<DailyRevenue>();
    }

    public class DailyRevenue
    {
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal RoomRevenue { get; set; }
        public decimal FnBRevenue { get; set; }
        public decimal TourRevenue { get; set; }
    }
}
