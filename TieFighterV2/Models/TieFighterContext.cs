using Microsoft.EntityFrameworkCore;

namespace TieFighterV2.Models
{
    public partial class TieFighterContext : DbContext
    {
        public TieFighterContext(DbContextOptions<TieFighterContext> options) : base(options)
        { }


        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Missions> Missions { get; set; }
        public virtual DbSet<Ships> Ships { get; set; }
        public virtual DbSet<Tours> Tours { get; set; }
        public virtual DbSet<Medal> Medals { get; set; }
        public virtual DbSet<UsersToMedals> UsersToMedals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=bitsql.wctc.edu;Database=TieFighter;User Id=ahayes13;Password=000415469;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.Thumbnail).HasMaxLength(512);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<Missions>(entity =>
            {
                entity.HasKey(e => e.MissionId);

                entity.HasIndex(e => e.FkTour);

                entity.Property(e => e.MissionId)
                    .HasColumnName("MissionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FkTour).HasColumnName("FK_Tour");

                entity.Property(e => e.MissionName)
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkTourNavigation)
                    .WithMany(p => p.Missions)
                    .HasForeignKey(d => d.FkTour)
                    .HasConstraintName("FK__Missions__FK_Tou__2C3393D0");
            });

            modelBuilder.Entity<Ships>(entity =>
            {
                entity.HasKey(e => e.ShipId);

                entity.Property(e => e.ShipId)
                    .HasColumnName("ShipID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ShipFolder)
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.ShipName)
                    .HasMaxLength(120)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tours>(entity =>
            {
                entity.HasKey(e => e.TourId);

                entity.Property(e => e.TourId)
                    .HasColumnName("TourID")
                    .ValueGeneratedNever();

                entity.Property(e => e.TourName)
                    .HasMaxLength(120)
                    .IsUnicode(false);
            });
        }
    }
}
