using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.DAL.Entities;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Configurations;

public class DiningTableConfiguration : IEntityTypeConfiguration<DiningTable>
{
    public void Configure(EntityTypeBuilder<DiningTable> builder)
    {
        builder.ToTable("DiningTables");
        builder.HasKey(x => x.TableId);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).HasDefaultValue(DiningTableStatus.Available);
    }
}
