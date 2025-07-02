using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using BusinessObject;

namespace DataAccessObject;

public partial class CosmeticDbContext : DbContext
{
    public CosmeticDbContext()
    {
    }

    public CosmeticDbContext(DbContextOptions<CosmeticDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CosmeticCategory> CosmeticCategories { get; set; }

    public virtual DbSet<CosmeticInformation> CosmeticInformations { get; set; }

    public virtual DbSet<SystemAccount> SystemAccounts { get; set; }

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        return configuration["ConnectionStrings:DefaultConnectionString"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CosmeticCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Cosmetic__19093A2B4423A54E");

            entity.ToTable("CosmeticCategory");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.FormulationType).HasMaxLength(100);
            entity.Property(e => e.UsagePurpose).HasMaxLength(200);
        });

        modelBuilder.Entity<CosmeticInformation>(entity =>
        {
            entity.HasKey(e => e.CosmeticId).HasName("PK__Cosmetic__98ED527E5CC702B4");

            entity.ToTable("CosmeticInformation");

            entity.HasIndex(e => e.CategoryId, "IX_CosmeticInformation_CategoryID");

            entity.HasIndex(e => e.CosmeticName, "IX_CosmeticInformation_CosmeticName");

            entity.Property(e => e.CosmeticId).HasColumnName("CosmeticID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CosmeticName).HasMaxLength(150);
            entity.Property(e => e.CosmeticSize).HasMaxLength(50);
            entity.Property(e => e.DollarPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SkinType).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.CosmeticInformations)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_CosmeticInformation_Category");
        });

        modelBuilder.Entity<SystemAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__SystemAc__349DA5864FEF493F");

            entity.ToTable("SystemAccount");

            entity.HasIndex(e => e.EmailAddress, "IX_SystemAccount_EmailAddress");

            entity.HasIndex(e => e.EmailAddress, "UQ__SystemAc__49A14740B0868F8E").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AccountNote).HasColumnType("ntext");
            entity.Property(e => e.AccountPassword).HasMaxLength(255);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
