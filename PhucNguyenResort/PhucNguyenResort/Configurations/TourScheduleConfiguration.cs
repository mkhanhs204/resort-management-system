using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Configurations;

public class TourScheduleConfiguration : IEntityTypeConfiguration<TourSchedule>
{
    public void Configure(EntityTypeBuilder<TourSchedule> builder)
    {
        builder.ToTable("TourSchedules");
        builder.HasKey(x => x.ScheduleId);

        builder.Property(x => x.TourDate).IsRequired(false);
        builder.Property(x => x.StartTime).IsRequired(false);
        builder.Property(x => x.AvailableSlots).IsRequired(false);
        builder.Property(x => x.GuideId).IsRequired(false);

        builder.HasOne(x => x.Tour).WithMany(x => x.TourSchedules).HasForeignKey(x => x.TourId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Guide).WithMany(x => x.GuidedTourSchedules).HasForeignKey(x => x.GuideId).OnDelete(DeleteBehavior.SetNull);
    }
}
