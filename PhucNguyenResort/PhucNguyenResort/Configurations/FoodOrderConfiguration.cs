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
        builder.Property(x => x.OrderType).HasConversion<string>().HasMaxLength(50).HasDefaultValue(FoodOrderType.RoomService);
        builder.Property(x => x.OrderStatus).HasConversion<string>().HasMaxLength(50).HasDefaultValue(FoodOrderStatus.Pending);
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
        builder.Property(x => x.OrderedAt).HasDefaultValueSql("GETDATE()");
        builder.Property(x => x.RequestedDeliveryAt);
        builder.Property(x => x.CustomerNote).HasMaxLength(500);
        builder.Property(x => x.IsChargedToRoom).HasDefaultValue(false);
        builder.Property(x => x.ChargedAt);
        builder.Property(x => x.IsPaidAtRestaurant).HasDefaultValue(false);
        builder.Property(x => x.PaidAt);
        builder.HasOne(x => x.Booking).WithMany(x => x.FoodOrders).HasForeignKey(x => x.BookingId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.BookingDetail).WithMany().HasForeignKey(x => x.BookingDetailId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.TableReservation).WithMany().HasForeignKey(x => x.TableReservationId).OnDelete(DeleteBehavior.Restrict);
    }
}
