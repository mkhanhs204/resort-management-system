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
        builder.Property(x => x.ReservationCode).IsRequired().HasMaxLength(30);
        builder.HasIndex(x => x.ReservationCode).IsUnique();
        builder.Property(x => x.ReservationDate).IsRequired();
        builder.Property(x => x.ReservationTime).IsRequired();
        builder.Property(x => x.DurationMinutes).IsRequired().HasDefaultValue(120);
        builder.Property(x => x.PartySize).IsRequired();
        builder.Property(x => x.SpecialRequests).HasMaxLength(500);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).HasDefaultValue(TableReservationStatus.Pending);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
        builder.Property(x => x.CancellationReason).HasMaxLength(500);
        builder.Property(x => x.IsChargedToRoom).HasDefaultValue(false);
        builder.Property(x => x.ChargedAmount).HasPrecision(18, 2).HasDefaultValue(0);
        builder.HasOne(x => x.User).WithMany(x => x.TableReservations).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Table).WithMany(x => x.TableReservations).HasForeignKey(x => x.TableId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Booking).WithMany(x => x.TableReservations).HasForeignKey(x => x.BookingId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ConfirmedByUser).WithMany().HasForeignKey(x => x.ConfirmedByUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
