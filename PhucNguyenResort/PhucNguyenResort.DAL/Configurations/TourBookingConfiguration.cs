using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.DAL.Entities;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Configurations;

public class TourBookingConfiguration : IEntityTypeConfiguration<TourBooking>
{
    public void Configure(EntityTypeBuilder<TourBooking> builder)
    {
        builder.ToTable("TourBookings");
        builder.HasKey(x => x.TourBookingId);
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
        builder.Property(x => x.BookingStatus).HasConversion<string>().HasMaxLength(50).HasDefaultValue(TourBookingStatus.Pending);
        builder.HasOne(x => x.Booking).WithMany(x => x.TourBookings).HasForeignKey(x => x.BookingId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Schedule).WithMany(x => x.TourBookings).HasForeignKey(x => x.ScheduleId).OnDelete(DeleteBehavior.Restrict);
    }
}
