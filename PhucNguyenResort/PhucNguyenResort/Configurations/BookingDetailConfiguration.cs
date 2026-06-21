using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.Models;

namespace PhucNguyenResort.Configurations;

public class BookingDetailConfiguration : IEntityTypeConfiguration<BookingDetail>
{
    public void Configure(EntityTypeBuilder<BookingDetail> builder)
    {
        builder.ToTable("BookingDetails");
        builder.HasKey(x => x.BookingDetailId);
        builder.Property(x => x.Price).HasPrecision(18, 2);
        
        builder.HasOne(x => x.Booking)
               .WithMany(x => x.BookingDetails)
               .HasForeignKey(x => x.BookingId)
               .OnDelete(DeleteBehavior.Cascade);
               
        builder.HasOne(x => x.RoomCategory)
               .WithMany()
               .HasForeignKey(x => x.RoomCategoryId)
               .OnDelete(DeleteBehavior.Restrict);
               
        builder.HasOne(x => x.Room)
               .WithMany(x => x.BookingDetails)
               .HasForeignKey(x => x.RoomId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
