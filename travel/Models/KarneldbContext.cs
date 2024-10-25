using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace travel.Models;

public partial class KarneldbContext : DbContext
{
    public KarneldbContext()
    {
    }

    public KarneldbContext(DbContextOptions<KarneldbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Bookingtb> Bookingtbs { get; set; }

    public virtual DbSet<Destinationtb> Destinationtbs { get; set; }

    public virtual DbSet<Guidetb> Guidetbs { get; set; }

    public virtual DbSet<Mailtb> Mailtbs { get; set; }

    public virtual DbSet<Packagetb> Packagetbs { get; set; }

    public virtual DbSet<Processtb> Processtbs { get; set; }

    public virtual DbSet<Servicestb> Servicestbs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-RDMO5QH\\SQLEXPRESS;Database=karneldb;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Bookingtb>(entity =>
        {
            entity.HasKey(e => e.BookId);

            entity.ToTable("bookingtb");

            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.Destination)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Guider)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Message)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
        });

        modelBuilder.Entity<Destinationtb>(entity =>
        {
            entity.HasKey(e => e.DestinationId);

            entity.ToTable("destinationtb");

            entity.Property(e => e.DestinationCountry)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DestinationDate1).HasColumnType("datetime");
            entity.Property(e => e.DestinationDate2).HasColumnType("datetime");
            entity.Property(e => e.DestinationDate3).HasColumnType("datetime");
            entity.Property(e => e.DestinationGuider)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DestinationImage)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Guidetb>(entity =>
        {
            entity.HasKey(e => e.GudieId);

            entity.ToTable("guidetb");

            entity.Property(e => e.GuideDestination)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GuideImage)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.GuideName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Mailtb>(entity =>
        {
            entity.HasKey(e => e.MailId);

            entity.ToTable("mailtb");

            entity.Property(e => e.MailEmail)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MailMessage)
                .HasMaxLength(3000)
                .IsUnicode(false);
            entity.Property(e => e.MailName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MailSubject)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Packagetb>(entity =>
        {
            entity.HasKey(e => e.PackageId);

            entity.ToTable("packagetb");

            entity.Property(e => e.PackageCountry)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PackageDate1).HasColumnType("datetime");
            entity.Property(e => e.PackageDate2).HasColumnType("datetime");
            entity.Property(e => e.PackageDate3).HasColumnType("datetime");
            entity.Property(e => e.PackageDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.PackageGuide)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PackageImage)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Processtb>(entity =>
        {
            entity.HasKey(e => e.ProcessId);

            entity.ToTable("processtb");

            entity.Property(e => e.ProcessDescription)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ProcessImage)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ProcessName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Servicestb>(entity =>
        {
            entity.HasKey(e => e.ServiceId);

            entity.ToTable("servicestb");

            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LogoImg)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
