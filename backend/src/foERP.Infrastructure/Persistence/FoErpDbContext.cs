using foERP.Domain.Finance;
using foERP.Domain.SupplyChain;
using Microsoft.EntityFrameworkCore;

namespace foERP.Infrastructure.Persistence;

public sealed class FoErpDbContext(DbContextOptions<FoErpDbContext> options) : DbContext(options), foERP.Application.Abstractions.IUnitOfWork
{
    public DbSet<AccountBook> AccountBooks => Set<AccountBook>();
    public DbSet<LedgerAccount> LedgerAccounts => Set<LedgerAccount>();
    public DbSet<ItemMaster> ItemMasters => Set<ItemMaster>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("foerp");

        modelBuilder.Entity<AccountBook>(entity =>
        {
            entity.HasIndex(x => x.BookCode).IsUnique();
            entity.Property(x => x.BookCode).HasMaxLength(20).IsRequired();
            entity.Property(x => x.BookName).HasMaxLength(128).IsRequired();
            entity.Property(x => x.BaseCurrency).HasMaxLength(3).IsRequired();
        });

        modelBuilder.Entity<LedgerAccount>(entity =>
        {
            entity.HasIndex(x => new { x.AccountBookId, x.AccountNumber }).IsUnique();
            entity.Property(x => x.AccountNumber).HasMaxLength(32).IsRequired();
            entity.Property(x => x.Name).HasMaxLength(256).IsRequired();

            entity.HasOne<AccountBook>()
                .WithMany()
                .HasForeignKey(x => x.AccountBookId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ItemMaster>(entity =>
        {
            entity.HasIndex(x => x.ItemId).IsUnique();
            entity.Property(x => x.ItemId).HasMaxLength(64).IsRequired();
            entity.Property(x => x.ItemName).HasMaxLength(256).IsRequired();
            entity.Property(x => x.InventoryUnit).HasMaxLength(16);
        });
    }
}
