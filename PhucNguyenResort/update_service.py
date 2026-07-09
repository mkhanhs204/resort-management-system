import re

file_path = r"e:\SWP391_PROJECT\su26-swp391-se2023-g1\PhucNguyenResort\PhucNguyenResort\Services\Dashboard\DashboardReportService.cs"

with open(file_path, 'r', encoding='utf-8') as f:
    content = f.read()

# Replace method signatures
content = content.replace("public async Task<OccupancyData> GetOccupancyDataAsync(int month, int year)", "public async Task<OccupancyData> GetOccupancyDataAsync(int? month, int? year)")
content = content.replace("public async Task<RevenueBreakdownData> GetRevenueBreakdownAsync(int month, int year)", "public async Task<RevenueBreakdownData> GetRevenueBreakdownAsync(int? month, int? year)")
content = content.replace("public async Task<FinancialReportViewModel> GetFinancialReportAsync(int month, int year)", "public async Task<FinancialReportViewModel> GetFinancialReportAsync(int? month, int? year)")
content = content.replace("public async Task<byte[]> ExportToExcelAsync(int month, int year)", "public async Task<byte[]> ExportToExcelAsync(int? month, int? year)")
content = content.replace("public async Task<byte[]> ExportToPdfAsync(int month, int year)", "public async Task<byte[]> ExportToPdfAsync(int? month, int? year)")

new_occupancy = """
        public async Task<OccupancyData> GetOccupancyDataAsync(int? month, int? year)
        {
            var totalRooms = await _context.Rooms
                .Where(r => r.Status != RoomStatus.Maintenance)
                .CountAsync();

            var data = new OccupancyData
            {
                TotalRooms = totalRooms,
                Month = month,
                Year = year
            };

            if (totalRooms == 0)
            {
                return data;
            }

            bool isAllTime = !month.HasValue || !year.HasValue;
            DateTime startDate;
            DateTime endDate;

            if (isAllTime)
            {
                var firstBooking = await _context.Bookings.OrderBy(b => b.CreatedAt).FirstOrDefaultAsync();
                startDate = firstBooking != null ? new DateTime(firstBooking.CreatedAt.Year, firstBooking.CreatedAt.Month, 1) : new DateTime(DateTime.Today.Year, 1, 1);
                endDate = DateTime.Today.AddDays(1);
            }
            else
            {
                int daysInMonth = DateTime.DaysInMonth(year.Value, month.Value);
                startDate = new DateTime(year.Value, month.Value, 1);
                endDate = new DateTime(year.Value, month.Value, daysInMonth).AddDays(1);
            }

            var bookings = await _context.Bookings
                .Include(b => b.BookingDetails)
                .Where(b => (b.BookingStatus == BookingStatus.CheckedIn || b.BookingStatus == BookingStatus.CheckedOut)
                         && b.CheckInDate < endDate
                         && b.CheckOutDate >= startDate)
                .ToListAsync();

            if (isAllTime)
            {
                var currentMonth = startDate;
                int sumOccupiedRooms = 0;
                int totalMonths = 0;
                
                while (currentMonth < endDate)
                {
                    int daysInThisMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
                    int sumOccupiedDaysInMonth = 0;
                    
                    for (int i = 1; i <= daysInThisMonth; i++)
                    {
                        var loopDate = new DateTime(currentMonth.Year, currentMonth.Month, i);
                        if (loopDate >= endDate) break;

                        var activeBookings = bookings.Where(b => b.CheckInDate.Date <= loopDate.Date && b.CheckOutDate.Date > loopDate.Date).ToList();
                        int occupiedRoomsForDay = activeBookings
                            .SelectMany(b => b.BookingDetails)
                            .Where(bd => bd.RoomId.HasValue)
                            .Select(bd => bd.RoomId)
                            .Distinct()
                            .Count();
                            
                        sumOccupiedDaysInMonth += occupiedRoomsForDay;
                    }
                    
                    var avgOccupiedRoomsInMonth = sumOccupiedDaysInMonth / daysInThisMonth;
                    var monthlyOccRate = (double)avgOccupiedRoomsInMonth / totalRooms * 100;
                    if (monthlyOccRate > 100) monthlyOccRate = 100;

                    var dailyOcc = new DailyOccupancy
                    {
                        Date = new DateTime(currentMonth.Year, currentMonth.Month, 1),
                        TotalRooms = totalRooms,
                        OccupiedRooms = avgOccupiedRoomsInMonth,
                        OccupancyRatePercent = monthlyOccRate
                    };
                    
                    data.DailyData.Add(dailyOcc);
                    sumOccupiedRooms += avgOccupiedRoomsInMonth;
                    totalMonths++;
                    
                    currentMonth = currentMonth.AddMonths(1);
                }
                
                if (totalMonths > 0)
                {
                    data.OccupiedRooms = sumOccupiedRooms / totalMonths;
                    data.OccupancyRatePercent = data.DailyData.Average(d => d.OccupancyRatePercent);
                }
            }
            else
            {
                int daysInMonth = DateTime.DaysInMonth(year.Value, month.Value);
                int sumOccupied = 0;
                int countDays = 0;

                for (int i = 1; i <= daysInMonth; i++)
                {
                    var currentDate = new DateTime(year.Value, month.Value, i);
                    var activeBookings = bookings.Where(b => b.CheckInDate.Date <= currentDate.Date && b.CheckOutDate.Date > currentDate.Date).ToList();
                    var occupiedRoomsForDay = activeBookings
                        .SelectMany(b => b.BookingDetails)
                        .Where(bd => bd.RoomId.HasValue)
                        .Select(bd => bd.RoomId)
                        .Distinct()
                        .Count();

                    var dailyOcc = new DailyOccupancy
                    {
                        Date = currentDate,
                        TotalRooms = totalRooms,
                        OccupiedRooms = occupiedRoomsForDay,
                        OccupancyRatePercent = (double)occupiedRoomsForDay / totalRooms * 100
                    };
                    
                    if (dailyOcc.OccupancyRatePercent > 100) dailyOcc.OccupancyRatePercent = 100;
                    data.DailyData.Add(dailyOcc);
                    sumOccupied += occupiedRoomsForDay;
                    countDays++;
                }

                if (countDays > 0)
                {
                    data.OccupiedRooms = sumOccupied / countDays;
                    data.OccupancyRatePercent = data.DailyData.Average(d => d.OccupancyRatePercent);
                }
            }

            return data;
        }
"""

new_revenue = """
        public async Task<RevenueBreakdownData> GetRevenueBreakdownAsync(int? month, int? year)
        {
            var data = new RevenueBreakdownData { Month = month, Year = year };
            bool isAllTime = !month.HasValue || !year.HasValue;
            
            DateTime startDate;
            DateTime endDate;

            if (isAllTime)
            {
                var firstTxn = await _context.FolioTransactions.OrderBy(t => t.CreatedAt).FirstOrDefaultAsync();
                startDate = firstTxn != null ? new DateTime(firstTxn.CreatedAt.Year, firstTxn.CreatedAt.Month, 1) : new DateTime(DateTime.Today.Year, 1, 1);
                endDate = DateTime.Today.AddDays(1);
            }
            else
            {
                startDate = new DateTime(year.Value, month.Value, 1);
                endDate = new DateTime(year.Value, month.Value, DateTime.DaysInMonth(year.Value, month.Value)).AddDays(1);
            }

            var folioTxns = await _context.FolioTransactions
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt < endDate)
                .ToListAsync();

            var roomChargeRev = folioTxns.Where(t => t.TransactionType == FolioTransactionType.RoomCharge).Sum(t => t.Amount ?? 0);
            var keptDepositBookings = await _context.Bookings
                .Where(b => b.BookingStatus == BookingStatus.Cancelled 
                         && b.CreatedAt >= startDate && b.CreatedAt < endDate
                         && !_context.Refunds.Any(r => r.BookingId == b.BookingId))
                .ToListAsync();
            var roomCancelFeeRev = keptDepositBookings.Sum(b => b.DepositAmount ?? 0);
            var roomRevenue = roomChargeRev + roomCancelFeeRev;

            var fnbFolioRev = folioTxns.Where(t => t.TransactionType == FolioTransactionType.FoodCharge).Sum(t => t.Amount ?? 0);
            var tourFolioRev = folioTxns.Where(t => t.TransactionType == FolioTransactionType.TourCharge).Sum(t => t.Amount ?? 0);

            var foodOrders = await _context.FoodOrders
                .Where(f => f.IsPaidAtRestaurant && !f.IsChargedToRoom && f.PaidAt >= startDate && f.PaidAt < endDate)
                .ToListAsync();
            var fnbRestaurantRev = foodOrders.Sum(f => f.TotalAmount ?? 0);

            var tourPayments = await _context.Payments
                .Include(p => p.TourBooking)
                .Where(p => p.TourBookingId != null && p.PaymentStatus == PaymentStatus.Success && p.PaidAt >= startDate && p.PaidAt < endDate
                         && p.TourBooking != null && p.TourBooking.BookingStatus != TourBookingStatus.Cancelled)
                .ToListAsync();
            var tourDirectRev = tourPayments.Sum(p => p.Amount ?? 0);

            data.RoomRevenue = roomRevenue;
            data.FnBRevenue = fnbFolioRev + fnbRestaurantRev;
            data.TourRevenue = tourFolioRev + tourDirectRev;
            data.TotalRevenue = data.RoomRevenue + data.FnBRevenue + data.TourRevenue;

            if (data.TotalRevenue > 0)
            {
                data.RoomPercentage = (double)(data.RoomRevenue / data.TotalRevenue * 100);
                data.FnBPercentage = (double)(data.FnBRevenue / data.TotalRevenue * 100);
                data.TourPercentage = (double)(data.TourRevenue / data.TotalRevenue * 100);
            }

            if (isAllTime)
            {
                var currentMonth = startDate;
                while (currentMonth < endDate)
                {
                    var nextMonth = currentMonth.AddMonths(1);
                    
                    var monthlyRoomCharge = folioTxns.Where(t => t.TransactionType == FolioTransactionType.RoomCharge && t.CreatedAt >= currentMonth && t.CreatedAt < nextMonth).Sum(t => t.Amount ?? 0);
                    var monthlyRoomCancelFee = keptDepositBookings.Where(b => b.CreatedAt >= currentMonth && b.CreatedAt < nextMonth).Sum(b => b.DepositAmount ?? 0);
                    var monthlyRoom = monthlyRoomCharge + monthlyRoomCancelFee;
                    
                    var monthlyFnbFolio = folioTxns.Where(t => t.TransactionType == FolioTransactionType.FoodCharge && t.CreatedAt >= currentMonth && t.CreatedAt < nextMonth).Sum(t => t.Amount ?? 0);
                    var monthlyFnbRest = foodOrders.Where(f => f.PaidAt >= currentMonth && f.PaidAt < nextMonth).Sum(f => f.TotalAmount ?? 0);
                    var monthlyFnb = monthlyFnbFolio + monthlyFnbRest;

                    var monthlyTourFolio = folioTxns.Where(t => t.TransactionType == FolioTransactionType.TourCharge && t.CreatedAt >= currentMonth && t.CreatedAt < nextMonth).Sum(t => t.Amount ?? 0);
                    var monthlyTourDirect = tourPayments
                        .Where(p => p.PaidAt >= currentMonth && p.PaidAt < nextMonth)
                        .Sum(p => p.Amount ?? 0);
                    var monthlyTour = monthlyTourFolio + monthlyTourDirect;

                    data.DailyData.Add(new DailyRevenue
                    {
                        Date = new DateTime(currentMonth.Year, currentMonth.Month, 1),
                        RoomRevenue = monthlyRoom,
                        FnBRevenue = monthlyFnb,
                        TourRevenue = monthlyTour,
                        TotalRevenue = monthlyRoom + monthlyFnb + monthlyTour
                    });
                    currentMonth = nextMonth;
                }
            }
            else
            {
                int daysInMonth = DateTime.DaysInMonth(year.Value, month.Value);
                for (int i = 1; i <= daysInMonth; i++)
                {
                    var currentDate = new DateTime(year.Value, month.Value, i);
                    var nextDate = currentDate.AddDays(1);
                    
                    var dailyRoomCharge = folioTxns.Where(t => t.TransactionType == FolioTransactionType.RoomCharge && t.CreatedAt >= currentDate && t.CreatedAt < nextDate).Sum(t => t.Amount ?? 0);
                    var dailyRoomCancelFee = keptDepositBookings.Where(b => b.CreatedAt >= currentDate && b.CreatedAt < nextDate).Sum(b => b.DepositAmount ?? 0);
                    var dailyRoom = dailyRoomCharge + dailyRoomCancelFee;
                    
                    var dailyFnbFolio = folioTxns.Where(t => t.TransactionType == FolioTransactionType.FoodCharge && t.CreatedAt >= currentDate && t.CreatedAt < nextDate).Sum(t => t.Amount ?? 0);
                    var dailyFnbRest = foodOrders.Where(f => f.PaidAt >= currentDate && f.PaidAt < nextDate).Sum(f => f.TotalAmount ?? 0);
                    var dailyFnb = dailyFnbFolio + dailyFnbRest;

                    var dailyTourFolio = folioTxns.Where(t => t.TransactionType == FolioTransactionType.TourCharge && t.CreatedAt >= currentDate && t.CreatedAt < nextDate).Sum(t => t.Amount ?? 0);
                    var dailyTourDirect = tourPayments
                        .Where(p => p.PaidAt >= currentDate && p.PaidAt < nextDate)
                        .Sum(p => p.Amount ?? 0);
                    var dailyTour = dailyTourFolio + dailyTourDirect;

                    data.DailyData.Add(new DailyRevenue
                    {
                        Date = currentDate,
                        RoomRevenue = dailyRoom,
                        FnBRevenue = dailyFnb,
                        TourRevenue = dailyTour,
                        TotalRevenue = dailyRoom + dailyFnb + dailyTour
                    });
                }
            }

            return data;
        }
"""

new_financial = """
        public async Task<FinancialReportViewModel> GetFinancialReportAsync(int? month, int? year)
        {
            var occupancy = await GetOccupancyDataAsync(month, year);
            var revenue = await GetRevenueBreakdownAsync(month, year);
            
            bool isAllTime = !month.HasValue || !year.HasValue;
            DateTime startDate;
            DateTime endDate;

            if (isAllTime)
            {
                var firstBooking = await _context.Bookings.OrderBy(b => b.CreatedAt).FirstOrDefaultAsync();
                startDate = firstBooking != null ? new DateTime(firstBooking.CreatedAt.Year, firstBooking.CreatedAt.Month, 1) : new DateTime(DateTime.Today.Year, 1, 1);
                endDate = DateTime.Today.AddDays(1);
            }
            else
            {
                startDate = new DateTime(year.Value, month.Value, 1);
                endDate = new DateTime(year.Value, month.Value, DateTime.DaysInMonth(year.Value, month.Value)).AddDays(1);
            }

            var totalBookings = await _context.Bookings.CountAsync(b => b.CreatedAt >= startDate && b.CreatedAt < endDate);
            var completedBookings = await _context.Bookings.CountAsync(b => b.BookingStatus == BookingStatus.CheckedOut && b.CreatedAt >= startDate && b.CreatedAt < endDate);
            var cancelledBookings = await _context.Bookings.CountAsync(b => b.BookingStatus == BookingStatus.Cancelled && b.CreatedAt >= startDate && b.CreatedAt < endDate);
            
            var invoices = await _context.Invoices.Where(i => i.IssuedAt >= startDate && i.IssuedAt < endDate).ToListAsync();

            return new FinancialReportViewModel
            {
                Month = month,
                Year = year,
                TotalRevenue = revenue.TotalRevenue,
                TotalRoomRevenue = revenue.RoomRevenue,
                TotalFnBRevenue = revenue.FnBRevenue,
                TotalTourRevenue = revenue.TourRevenue,
                AverageOccupancyRate = occupancy.OccupancyRatePercent,
                TotalBookings = totalBookings,
                CompletedBookings = completedBookings,
                CancelledBookings = cancelledBookings,
                TotalInvoices = invoices.Count,
                PaidInvoices = invoices.Count(i => i.InvoiceStatus == InvoiceStatus.Paid),
                DailyOccupancies = occupancy.DailyData,
                DailyRevenues = revenue.DailyData
            };
        }
"""

occ_pattern = r'public async Task<OccupancyData> GetOccupancyDataAsync\(int\? month, int\? year\).*?return data;\s*}'
rev_pattern = r'public async Task<RevenueBreakdownData> GetRevenueBreakdownAsync\(int\? month, int\? year\).*?return data;\s*}'
fin_pattern = r'public async Task<FinancialReportViewModel> GetFinancialReportAsync\(int\? month, int\? year\).*?return new FinancialReportViewModel\s*\{[^}]+\};\s*}'

content = re.sub(occ_pattern, new_occupancy.strip(), content, flags=re.DOTALL)
content = re.sub(rev_pattern, new_revenue.strip(), content, flags=re.DOTALL)
content = re.sub(fin_pattern, new_financial.strip(), content, flags=re.DOTALL)

with open(file_path, 'w', encoding='utf-8') as f:
    f.write(content)

print("Replaced!")
