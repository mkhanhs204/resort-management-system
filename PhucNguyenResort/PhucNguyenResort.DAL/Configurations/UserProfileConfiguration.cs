using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.DAL.Entities;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");
        builder.HasKey(x => x.ProfileId);
        builder.HasIndex(x => x.UserId).IsUnique();
        builder.Property(x => x.ConsentGiven).HasDefaultValue(false);
        builder.Property(x => x.DeletionRequested).HasDefaultValue(false);
        builder.HasOne(x => x.User).WithOne(x => x.UserProfile).HasForeignKey<UserProfile>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
