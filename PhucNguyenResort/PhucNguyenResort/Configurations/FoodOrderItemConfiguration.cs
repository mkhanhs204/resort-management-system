using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Configurations;

public class FoodOrderItemConfiguration : IEntityTypeConfiguration<FoodOrderItem>
{
    public void Configure(EntityTypeBuilder<FoodOrderItem> builder)
    {
        builder.ToTable("FoodOrderItems");
        builder.HasKey(x => x.OrderItemId);
        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x => x.UnitPrice).IsRequired().HasPrecision(18, 2);
        builder.HasOne(x => x.FoodOrder).WithMany(x => x.FoodOrderItems).HasForeignKey(x => x.FoodOrderId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.MenuItem).WithMany(x => x.FoodOrderItems).HasForeignKey(x => x.MenuItemId).OnDelete(DeleteBehavior.Restrict);
    }
}
