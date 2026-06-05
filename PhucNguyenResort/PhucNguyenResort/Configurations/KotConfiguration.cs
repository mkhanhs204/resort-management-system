using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Configurations;

public class KotConfiguration : IEntityTypeConfiguration<Kot>
{
    public void Configure(EntityTypeBuilder<Kot> builder)
    {
        builder.ToTable("KOT");
        builder.HasKey(x => x.KotId);
        builder.HasIndex(x => x.FoodOrderId).IsUnique();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).HasDefaultValue(KotStatus.Pending);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
        builder.HasOne(x => x.FoodOrder).WithOne(x => x.Kot).HasForeignKey<Kot>(x => x.FoodOrderId).OnDelete(DeleteBehavior.Cascade);
    }
}
