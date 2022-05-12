using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Attendence_Monitoring_System.Models
{
    public partial class Attendence_Monitoring_SystemContext : DbContext
    {
        public Attendence_Monitoring_SystemContext()
        {
        }

        public Attendence_Monitoring_SystemContext(DbContextOptions<Attendence_Monitoring_SystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AttendenceLog> AttendenceLogs { get; set; } = null!;
        public virtual DbSet<Regularization> Regularizations { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Section> Sections { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserDetail> UserDetails { get; set; } = null!;
        public virtual DbSet<UserLog> UserLogs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=SVASAGE-LAP-047\\SQLEXPRESS;Initial Catalog=Attendence_Monitoring_System;Integrated Security=SSPI");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendenceLog>(entity =>
            {
                entity.ToTable("AttendenceLog");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.TotalHours)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AttendenceLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Attendenc__UserI__440B1D61");
            });

            modelBuilder.Entity<Regularization>(entity =>
            {
                entity.ToTable("Regularization");

                entity.Property(e => e.InTime).HasColumnType("datetime");

                entity.Property(e => e.OutTime).HasColumnType("datetime");

                entity.Property(e => e.Reason)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TotalHours)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Regularizations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Regulariz__UserI__49C3F6B7");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.ToTable("Section");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Users__RoleId__38996AB5");
            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.Property(e => e.KeyName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.UserDetails)
                    .HasForeignKey(d => d.SectionId)
                    .HasConstraintName("FK__UserDetai__Secti__3E52440B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserDetails)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserDetai__UserI__3D5E1FD2");
            });

            modelBuilder.Entity<UserLog>(entity =>
            {
                entity.ToTable("UserLog");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserLog__UserId__412EB0B6");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
