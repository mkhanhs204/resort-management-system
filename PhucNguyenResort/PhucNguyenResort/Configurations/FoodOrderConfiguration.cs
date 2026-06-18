using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Configurations;

public class FoodOrderConfiguration : IEntityTypeConfiguration<FoodOrder>
{
    public void Configure(EntityTypeBuilder<FoodOrder> builder)
    {
        builder.ToTable("FoodOrders");
        builder.HasKey(x => x.FoodOrderId);
        builder.Property(x => x.OrderStatus).HasConversion<string>().HasMaxLength(50).HasDefaultValue(FoodOrderStatus.Pending);
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
        builder.Property(x => x.OrderedAt).HasDefaultValueSql("GETDATE()");
        builder.Property(x => x.IsChargedToRoom).HasDefaultValue(false);
        builder.Property(x => x.ChargedAt);
        builder.HasOne(x => x.Booking).WithMany(x => x.FoodOrders).HasForeignKey(x => x.BookingId).OnDelete(DeleteBehavior.Cascade);
    }
}
