using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");
        builder.HasKey(x => x.ReviewId);
        builder.Property(x => x.IsVisible).HasDefaultValue(true);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
        builder.HasOne(x => x.User).WithMany(x => x.Reviews).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(x => x.Tour).WithMany(x => x.Reviews).HasForeignKey(x => x.TourId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(x => x.Booking).WithMany(x => x.Reviews).HasForeignKey(x => x.BookingId).OnDelete(DeleteBehavior.SetNull);
    }
}
