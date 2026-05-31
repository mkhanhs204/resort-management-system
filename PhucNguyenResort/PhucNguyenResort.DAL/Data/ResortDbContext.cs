using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.DAL.Entities;

namespace PhucNguyenResort.DAL.Data;

public class ResortDbContext : DbContext
{
    public ResortDbContext(DbContextOptions<ResortDbContext> options) : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<RoomCategory> RoomCategories => Set<RoomCategory>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingGuest> BookingGuests => Set<BookingGuest>();
    public DbSet<MenuCategory> MenuCategories => Set<MenuCategory>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<DiningTable> DiningTables => Set<DiningTable>();
    public DbSet<TableReservation> TableReservations => Set<TableReservation>();
    public DbSet<FoodOrder> FoodOrders => Set<FoodOrder>();
    public DbSet<FoodOrderItem> FoodOrderItems => Set<FoodOrderItem>();
    public DbSet<Kot> KOT => Set<Kot>();
    public DbSet<Tour> Tours => Set<Tour>();
    public DbSet<TourSchedule> TourSchedules => Set<TourSchedule>();
    public DbSet<TourBooking> TourBookings => Set<TourBooking>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<GuestFolio> GuestFolios => Set<GuestFolio>();
    public DbSet<FolioTransaction> FolioTransactions => Set<FolioTransaction>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Refund> Refunds => Set<Refund>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResortDbContext).Assembly);
        SeedData.Apply(modelBuilder);
    }
}
