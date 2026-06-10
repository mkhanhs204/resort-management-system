using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Configurations;

public class FolioTransactionConfiguration : IEntityTypeConfiguration<FolioTransaction>
{
    public void Configure(EntityTypeBuilder<FolioTransaction> builder)
    {
        builder.ToTable("FolioTransactions");
        builder.HasKey(x => x.TransactionId);
        builder.Property(x => x.TransactionType).HasConversion<string>().HasMaxLength(50);
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
        builder.HasOne(x => x.Folio).WithMany(x => x.FolioTransactions).HasForeignKey(x => x.FolioId).OnDelete(DeleteBehavior.Cascade);
    }
}
