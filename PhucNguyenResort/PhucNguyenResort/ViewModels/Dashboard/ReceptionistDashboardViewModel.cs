using System.Collections.Generic;
using PhucNguyenResort.Models;

namespace PhucNguyenResort.ViewModels.Dashboard
{
    public class ReceptionistDashboardViewModel
    {
        public int TodaysArrivals { get; set; }
        public int ActiveStayGuests { get; set; }
        public int TodaysDepartures { get; set; }
        public int AvailableRooms { get; set; }
        public List<Booking> RecentBookings { get; set; } = new List<Booking>();
    }
}
