using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Configurations;

public class MenuCategoryConfiguration : IEntityTypeConfiguration<MenuCategory>
{
    public void Configure(EntityTypeBuilder<MenuCategory> builder)
    {
        builder.ToTable("MenuCategories");
        builder.HasKey(x => x.CategoryId);
        builder.Property(x => x.CategoryName).IsRequired().HasMaxLength(100); // thêm required/max length và unique index cho CategoryName
        builder.HasIndex(x => x.CategoryName).IsUnique();
    }
}
