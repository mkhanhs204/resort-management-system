using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.DAL.Entities;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        builder.HasKey(x => x.NotificationId);
        builder.Property(x => x.IsRead).HasDefaultValue(false);
        builder.Property(x => x.SentAt).HasDefaultValueSql("GETDATE()");
        builder.HasOne(x => x.User).WithMany(x => x.Notifications).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
    }
}
