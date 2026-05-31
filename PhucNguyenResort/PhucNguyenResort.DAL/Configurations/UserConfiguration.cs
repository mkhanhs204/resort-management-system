using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.DAL.Entities;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.UserId);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
        builder.HasOne(x => x.Role).WithMany(x => x.Users).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
    }
}
