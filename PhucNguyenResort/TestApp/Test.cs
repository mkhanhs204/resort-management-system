using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Services.Home;

class Program
{
    static async System.Threading.Tasks.Task Main(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ResortDbContext>();
        optionsBuilder.UseSqlServer("Server=MSI\\CUONG;Database=PhucNguyenResortDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        using var context = new ResortDbContext(optionsBuilder.Options);
        var svc = new HomeService(context);
        var checkIn = new DateTime(2026, 6, 27);
        var checkOut = new DateTime(2026, 6, 30);
        int total = await context.Rooms.CountAsync(r => r.CategoryId == 6 && r.Status != RoomStatus.Maintenance);
        int booked = await context.Bookings.CountAsync(b => b.Room!.CategoryId == 6 && b.BookingStatus != BookingStatus.Cancelled && b.BookingStatus != BookingStatus.NoShow && b.CheckInDate < checkOut && b.CheckOutDate > checkIn);
        Console.WriteLine($"total: {total}, booked: {booked}");
    }
}
