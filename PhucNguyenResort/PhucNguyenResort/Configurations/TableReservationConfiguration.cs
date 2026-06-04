using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Configurations;

public class TableReservationConfiguration : IEntityTypeConfiguration<TableReservation>
{
    public void Configure(EntityTypeBuilder<TableReservation> builder)
    {
        builder.ToTable("TableReservations");
        builder.HasKey(x => x.ReservationId);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).HasDefaultValue(TableReservationStatus.Pending);
        builder.HasOne(x => x.User).WithMany(x => x.TableReservations).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Table).WithMany(x => x.TableReservations).HasForeignKey(x => x.TableId).OnDelete(DeleteBehavior.Restrict);
    }
}
