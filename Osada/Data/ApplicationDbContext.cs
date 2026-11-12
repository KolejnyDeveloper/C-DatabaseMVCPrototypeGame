using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Osada.Models;

namespace Osada.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminVillageOverview> AdminVillageOverviews { get; set; }
    public virtual DbSet<Attack> Attacks { get; set; }

    public virtual DbSet<BuildingType> BuildingTypes { get; set; }

    public virtual DbSet<ResourceTransaction> ResourceTransactions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<UnitType> UnitTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Village> Villages { get; set; }

    public virtual DbSet<VillageBuilding> VillageBuildings { get; set; }

    public virtual DbSet<VillageOwnershipHistory> VillageOwnershipHistories { get; set; }

    public virtual DbSet<VillageResource> VillageResources { get; set; }

    public virtual DbSet<VillageUnit> VillageUnits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attack>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.AttackerVillage).WithMany(p => p.AttackAttackerVillages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attacks_Attacker");

            entity.HasOne(d => d.DefenderVillage).WithMany(p => p.AttackDefenderVillages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attacks_Defender");
        });

        modelBuilder.Entity<ResourceTransaction>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Village).WithMany(p => p.ResourceTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResourceTransactions_Villages");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserRoles_Roles"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserRoles_Users"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("UserRoles");
                    });
        });

        modelBuilder.Entity<Village>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Player).WithMany(p => p.Villages).HasConstraintName("FK_Villages_Users");
        });

        modelBuilder.Entity<VillageBuilding>(entity =>
        {
            entity.Property(e => e.Level).HasDefaultValue(1);

            entity.HasOne(d => d.BuildingType).WithMany(p => p.VillageBuildings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VillageBuildings_BuildingTypes");

            entity.HasOne(d => d.Village).WithMany(p => p.VillageBuildings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VillageBuildings_Villages");
        });

        modelBuilder.Entity<VillageOwnershipHistory>(entity =>
        {
            entity.Property(e => e.ChangedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.NewPlayer).WithMany(p => p.VillageOwnershipHistoryNewPlayers).HasConstraintName("FK_OwnershipHistory_NewPlayer");

            entity.HasOne(d => d.PreviousPlayer).WithMany(p => p.VillageOwnershipHistoryPreviousPlayers).HasConstraintName("FK_OwnershipHistory_PreviousPlayer");

            entity.HasOne(d => d.Village).WithMany(p => p.VillageOwnershipHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OwnershipHistory_Villages");
        });

        modelBuilder.Entity<VillageResource>(entity =>
        {
            entity.Property(e => e.VillageId).ValueGeneratedNever();
            entity.Property(e => e.LastUpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Village).WithOne(p => p.VillageResource)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VillageResources_Villages");
        });

        modelBuilder.Entity<VillageUnit>(entity =>
        {
            entity.HasOne(d => d.UnitType).WithMany(p => p.VillageUnits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VillageUnits_UnitTypes");

            entity.HasOne(d => d.Village).WithMany(p => p.VillageUnits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VillageUnits_Villages");
        });

        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<VillageResource>()
    .ToTable(tb => tb.HasTrigger("trg_ResourceTransactions"));

        modelBuilder.Entity<AdminVillageOverview>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("vw_AdminVillageOverview");

            entity.Property(e => e.VillageName)
                .HasMaxLength(100);

            entity.Property(e => e.OwnerUsername)
                .HasMaxLength(100);
        });
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
