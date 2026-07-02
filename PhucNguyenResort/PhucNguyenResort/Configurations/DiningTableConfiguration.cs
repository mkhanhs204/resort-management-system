using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Configurations;

public class DiningTableConfiguration : IEntityTypeConfiguration<DiningTable>
{
    public void Configure(EntityTypeBuilder<DiningTable> builder)
    {
        builder.ToTable("DiningTables");
        builder.HasKey(x => x.TableId);
        builder.Property(x => x.TableName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ImageUrl).HasMaxLength(255);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Area).HasMaxLength(50);
        builder.Property(x => x.TableType).HasMaxLength(50);
        builder.Property(x => x.LocationNote).HasMaxLength(150);
        builder.Property(x => x.IsReservable).HasDefaultValue(true);
        builder.Property(x => x.DisplayOrder).HasDefaultValue(0);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).HasDefaultValue(DiningTableStatus.Available);
    }
}
