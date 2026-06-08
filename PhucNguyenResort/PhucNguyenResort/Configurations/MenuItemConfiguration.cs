using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Configurations;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable("MenuItems", table =>
        {
            table.HasCheckConstraint(
                "CK_MenuItems_AvailableRequiresCategory",
                "[IsAvailable] = 0 OR [CategoryId] IS NOT NULL");
        });

        builder.HasKey(x => x.MenuItemId);
        builder.Property(x => x.ItemName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Price).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.IsAvailable).HasDefaultValue(true);
        builder.HasOne(x => x.Category).WithMany(x => x.MenuItems).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.SetNull);
    }
}
