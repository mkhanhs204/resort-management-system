using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhucNguyenResort.DAL.Entities;
using PhucNguyenResort.DAL.Enums;

namespace PhucNguyenResort.DAL.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");
        builder.HasKey(x => x.InvoiceId);
        builder.HasIndex(x => x.FolioId).IsUnique();
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
        builder.Property(x => x.TaxAmount).HasPrecision(18, 2);
        builder.Property(x => x.FinalAmount).HasPrecision(18, 2);
        builder.Property(x => x.InvoiceStatus).HasConversion<string>().HasMaxLength(50).HasDefaultValue(InvoiceStatus.Draft);
        builder.Property(x => x.IssuedAt).HasDefaultValueSql("GETDATE()");
        builder.HasOne(x => x.Folio).WithOne(x => x.Invoice).HasForeignKey<Invoice>(x => x.FolioId).OnDelete(DeleteBehavior.Cascade);
    }
}
