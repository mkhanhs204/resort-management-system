using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Configurations;

public class BookingGuestConfiguration : IEntityTypeConfiguration<BookingGuest>
{
    public void Configure(EntityTypeBuilder<BookingGuest> builder)
    {
        builder.ToTable("BookingGuests");
        builder.HasKey(x => x.BookingGuestId);
        builder.HasOne(x => x.Booking).WithMany(x => x.BookingGuests).HasForeignKey(x => x.BookingId).OnDelete(DeleteBehavior.Cascade);
    }
}
