using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.DAL.Entities;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");
        builder.HasKey(x => x.RoomId);
        builder.HasIndex(x => x.RoomNumber).IsUnique();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).HasDefaultValue(RoomStatus.Available);
        builder.HasOne(x => x.Category).WithMany(x => x.Rooms).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
    }
}
