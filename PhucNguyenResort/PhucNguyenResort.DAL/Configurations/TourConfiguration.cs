using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.DAL.Entities;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Configurations;

public class TourConfiguration : IEntityTypeConfiguration<Tour>
{
    public void Configure(EntityTypeBuilder<Tour> builder)
    {
        builder.ToTable("Tours");
        builder.HasKey(x => x.TourId);
        builder.Property(x => x.Price).HasPrecision(18, 2);
    }
}
