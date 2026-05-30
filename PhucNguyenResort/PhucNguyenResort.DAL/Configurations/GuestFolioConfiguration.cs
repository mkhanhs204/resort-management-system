using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.DAL.Entities;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Configurations;

public class GuestFolioConfiguration : IEntityTypeConfiguration<GuestFolio>
{
    public void Configure(EntityTypeBuilder<GuestFolio> builder)
    {
        builder.ToTable("GuestFolios");
        builder.HasKey(x => x.FolioId);
        builder.HasIndex(x => x.BookingId).IsUnique();
        builder.Property(x => x.TotalRoomCharges).HasPrecision(18, 2).HasDefaultValue(0);
        builder.Property(x => x.TotalFoodCharges).HasPrecision(18, 2).HasDefaultValue(0);
        builder.Property(x => x.TotalTourCharges).HasPrecision(18, 2).HasDefaultValue(0);
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2).HasDefaultValue(0);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).HasDefaultValue(FolioStatus.Unpaid);
        builder.HasOne(x => x.Booking).WithOne(x => x.GuestFolio).HasForeignKey<GuestFolio>(x => x.BookingId).OnDelete(DeleteBehavior.Cascade);
    }
}
