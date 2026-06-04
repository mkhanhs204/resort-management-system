using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Configurations;

public class RefundConfiguration : IEntityTypeConfiguration<Refund>
{
    public void Configure(EntityTypeBuilder<Refund> builder)
    {
        builder.ToTable("Refunds");
        builder.HasKey(x => x.RefundId);
        builder.Property(x => x.RefundStatus).HasConversion<string>().HasMaxLength(50);
        builder.Property(x => x.RefundAmount).HasPrecision(18, 2);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
        builder.HasOne(x => x.Booking).WithMany(x => x.Refunds).HasForeignKey(x => x.BookingId).OnDelete(DeleteBehavior.SetNull);
    }
}
