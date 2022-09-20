using System;
using System.Collections.Generic;
using CargoHubWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CargoHubWeb.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Depot> Depots { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=102.65.3.6;Initial Catalog=CargoHubWeb;Persist Security Info=True;User ID=SA;Password=,,PassnowSQL1;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Depot>(entity =>
            {
                entity.ToTable("depots");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Province)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("province");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Number);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Number)
                    .HasName("PK__orders__FD291E4050980A2F");

                entity.ToTable("orders");

                entity.Property(e => e.Number)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("number");

                entity.Property(e => e.CustomerId).HasColumnName("customerID");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.EmployeeNum).HasColumnName("employeeNum");

                entity.Property(e => e.FromDepot).HasColumnName("fromDepot");

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.ToDepot).HasColumnName("toDepot");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__orders__customer__76969D2E");

                entity.HasOne(d => d.EmployeeNumNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeNum)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__orders__employee__778AC167");

                entity.HasOne(d => d.FromDepotNavigation)
                    .WithMany(p => p.OrderFromDepotNavigations)
                    .HasForeignKey(d => d.FromDepot)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__orders__fromDepo__75A278F5");

                entity.HasOne(d => d.ToDepotNavigation)
                    .WithMany(p => p.OrderToDepotNavigations)
                    .HasForeignKey(d => d.ToDepot)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__orders__toDepot__74AE54BC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
