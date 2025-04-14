using DataAccess.Postgres.Migration.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Postgres.Migration;

public partial class EquipmentInventoryDbContext : DbContext
{
    public EquipmentInventoryDbContext()
    {
    }

    public EquipmentInventoryDbContext(DbContextOptions<EquipmentInventoryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Archive> Archives { get; set; }

    public virtual DbSet<Computer> Computers { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Office> Offices { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Technique> Techniques { get; set; }

    public virtual DbSet<TypeTechnique> TypeTechniques { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Archive>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("archive_pkey");

            entity.ToTable("archive");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ComputerNumber).HasColumnName("computer_number");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.DateOfManufacture).HasColumnName("date_of_manufacture");
            entity.Property(e => e.DateOfPurchase).HasColumnName("date_of_purchase");
            entity.Property(e => e.DateOfUse).HasColumnName("date_of_use");
            entity.Property(e => e.MemberName)
                .HasMaxLength(60)
                .HasColumnName("member_name");
            entity.Property(e => e.Number)
                .HasMaxLength(30)
                .HasColumnName("number");
            entity.Property(e => e.OfficeNumber)
                .HasMaxLength(100)
                .HasColumnName("office_number");
            entity.Property(e => e.Supplier)
                .HasMaxLength(150)
                .HasColumnName("supplier");
            entity.Property(e => e.TechniqueName)
                .HasMaxLength(100)
                .HasColumnName("technique_name");
            entity.Property(e => e.TypeTechniqueName)
                .HasMaxLength(100)
                .HasColumnName("type_technique_name");
            entity.Property(e => e.UnderRepair)
                .HasDefaultValue(false)
                .HasColumnName("under_repair");
            entity.Property(e => e.WriteOffDate).HasColumnName("write_off_date");
        });

        modelBuilder.Entity<Computer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("computers_pkey1");

            entity.ToTable("computers");

            entity.HasIndex(e => e.Number, "computers_number_key1").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Cpu)
                .HasMaxLength(200)
                .HasColumnName("cpu");
            entity.Property(e => e.Motherboard)
                .HasMaxLength(200)
                .HasColumnName("motherboard");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Os)
                .HasMaxLength(200)
                .HasColumnName("os");
            entity.Property(e => e.PowerSupply)
                .HasMaxLength(200)
                .HasColumnName("power_supply");
            entity.Property(e => e.Ram)
                .HasMaxLength(200)
                .HasColumnName("ram");
            entity.Property(e => e.VideoCard)
                .HasMaxLength(200)
                .HasColumnName("video_card");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("members_pkey");

            entity.ToTable("members");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdPosition).HasColumnName("id_position");
            entity.Property(e => e.Surname)
                .HasMaxLength(60)
                .HasColumnName("surname");
            entity.Property(e => e.Username)
                .HasMaxLength(60)
                .HasColumnName("username");

            entity.HasOne(d => d.IdPositionNavigation).WithMany(p => p.Members)
                .HasForeignKey(d => d.IdPosition)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("positions");
        });

        modelBuilder.Entity<Office>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("offices_pkey");

            entity.ToTable("offices");

            entity.HasIndex(e => e.Name, "offices_office_name_key").IsUnique();

            entity.HasIndex(e => e.Number, "offices_office_number_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Floor).HasColumnName("floor");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Number).HasColumnName("number");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("positions_pkey");

            entity.ToTable("positions");

            entity.HasIndex(e => e.Name, "positions_name_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Name, "roles_name_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("suppliers_pkey");

            entity.ToTable("suppliers");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Technique>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("technique_pkey");

            entity.ToTable("technique");

            entity.HasIndex(e => e.IdComputer, "id_computer_key").IsUnique();

            entity.HasIndex(e => e.Number, "number").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.DateOfManufacture).HasColumnName("date_of_manufacture");
            entity.Property(e => e.DateOfPurchase).HasColumnName("date_of_purchase");
            entity.Property(e => e.DateOfUse).HasColumnName("date_of_use");
            entity.Property(e => e.IdComputer).HasColumnName("id_computer");
            entity.Property(e => e.IdMember).HasColumnName("id_member");
            entity.Property(e => e.IdOffice).HasColumnName("id_office");
            entity.Property(e => e.IdSupplier).HasColumnName("id_supplier");
            entity.Property(e => e.IdTypeTechnique).HasColumnName("id_type_technique");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Number)
                .HasMaxLength(30)
                .HasColumnName("number");
            entity.Property(e => e.UnderRepair)
                .HasDefaultValue(false)
                .HasColumnName("under_repair");

            entity.HasOne(d => d.IdComputerNavigation).WithOne(p => p.Technique)
                .HasForeignKey<Technique>(d => d.IdComputer)
                .HasConstraintName("computers");

            entity.HasOne(d => d.IdMemberNavigation).WithMany(p => p.Techniques)
                .HasForeignKey(d => d.IdMember)
                .HasConstraintName("members");

            entity.HasOne(d => d.IdOfficeNavigation).WithMany(p => p.Techniques)
                .HasForeignKey(d => d.IdOffice)
                .HasConstraintName("offices");

            entity.HasOne(d => d.IdSupplierNavigation).WithMany(p => p.Techniques)
                .HasForeignKey(d => d.IdSupplier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("suppliers");

            entity.HasOne(d => d.IdTypeTechniqueNavigation).WithMany(p => p.Techniques)
                .HasForeignKey(d => d.IdTypeTechnique)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("type_technique");
        });

        modelBuilder.Entity<TypeTechnique>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("type_technique_pkey");

            entity.ToTable("type_technique");

            entity.HasIndex(e => e.Name, "type_technique_name_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "users_login_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Login)
                .HasMaxLength(60)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Surname)
                .HasMaxLength(60)
                .HasColumnName("surname");
            entity.Property(e => e.Username)
                .HasMaxLength(60)
                .HasColumnName("username");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
