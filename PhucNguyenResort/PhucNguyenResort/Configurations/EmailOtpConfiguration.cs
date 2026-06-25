using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.Models;

namespace PhucNguyenResort.Configurations
{
    public class EmailOtpConfiguration : IEntityTypeConfiguration<EmailOtp>
    {
        public void Configure(EntityTypeBuilder<EmailOtp> builder)
        {
            builder.ToTable("EmailOtps");
            builder.HasKey(x => x.OtpId);
            builder.HasIndex(x => x.Email);
            builder.Property(x => x.OtpCode).HasMaxLength(6).IsUnicode(false);
            builder.Property(x => x.IsUsed).HasDefaultValue(false);
            builder.Property(x => x.Attempts).HasDefaultValue(0);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
        }
    }
}
