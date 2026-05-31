using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.DAL.Entities;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Configurations;

public class RoomCategoryConfiguration : IEntityTypeConfiguration<RoomCategory>
{
    public void Configure(EntityTypeBuilder<RoomCategory> builder)
    {
        builder.ToTable("RoomCategories");
        builder.HasKey(x => x.CategoryId);
        builder.Property(x => x.PricePerNight).HasPrecision(18, 2);
    }
}
