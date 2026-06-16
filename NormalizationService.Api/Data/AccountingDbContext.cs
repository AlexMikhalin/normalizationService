using Microsoft.EntityFrameworkCore;
using NormalizationService.Api.Domain;

namespace NormalizationService.Api.Data;

public sealed class AccountingDbContext(DbContextOptions<AccountingDbContext> options) : DbContext(options)
{
    public DbSet<NormalizedAccountingEntry> NormalizedAccountingEntries => Set<NormalizedAccountingEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NormalizedAccountingEntry>(entry =>
        {
            entry.ToTable("normalized_accounting_entries");

            entry.HasKey(model => model.Id);

            entry.Property(model => model.SourceSystem).HasMaxLength(80).IsRequired();
            entry.Property(model => model.ExternalId).HasMaxLength(120).IsRequired();
            entry.Property(model => model.EntryType).HasMaxLength(60).IsRequired();
            entry.Property(model => model.AccountCode).HasMaxLength(40).IsRequired();
            entry.Property(model => model.CounterpartyName).HasMaxLength(160).IsRequired();
            entry.Property(model => model.Currency).HasMaxLength(3).IsRequired();
            entry.Property(model => model.Description).HasMaxLength(500).IsRequired();
            entry.Property(model => model.Status).HasMaxLength(60).IsRequired();
            entry.Property(model => model.ReferenceNumber).HasMaxLength(120).IsRequired();

            entry.Property(model => model.DebitAmount).HasPrecision(18, 2);
            entry.Property(model => model.CreditAmount).HasPrecision(18, 2);
            entry.Property(model => model.NetAmount).HasPrecision(18, 2);
            entry.Property(model => model.TaxAmount).HasPrecision(18, 2);
            entry.Property(model => model.GrossAmount).HasPrecision(18, 2);

            entry.HasIndex(model => new { model.SourceSystem, model.ExternalId }).IsUnique();
        });
    }
}
