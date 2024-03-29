﻿// <auto-generated />
using System;
using CargoHubWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CargoHubWeb.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CargoHubWeb.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("CargoHubWeb.Models.Depot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("address");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("phone");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("province");

                    b.HasKey("Id");

                    b.ToTable("depots", (string)null);
                });

            modelBuilder.Entity("CargoHubWeb.Models.Employee", b =>
                {
                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Number"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Number");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("CargoHubWeb.Models.Order", b =>
                {
                    b.Property<string>("Number")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("number");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int")
                        .HasColumnName("customerID");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("description");

                    b.Property<int>("EmployeeNum")
                        .HasColumnType("int")
                        .HasColumnName("employeeNum");

                    b.Property<int>("FromDepot")
                        .HasColumnType("int")
                        .HasColumnName("fromDepot");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("status");

                    b.Property<int>("ToDepot")
                        .HasColumnType("int")
                        .HasColumnName("toDepot");

                    b.Property<int>("Weight")
                        .HasColumnType("int")
                        .HasColumnName("weight");

                    b.HasKey("Number")
                        .HasName("PK__orders__FD291E4050980A2F");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EmployeeNum");

                    b.HasIndex("FromDepot");

                    b.HasIndex("ToDepot");

                    b.ToTable("orders", (string)null);
                });

            modelBuilder.Entity("CargoHubWeb.Models.Order", b =>
                {
                    b.HasOne("CargoHubWeb.Models.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .IsRequired()
                        .HasConstraintName("FK__orders__customer__76969D2E");

                    b.HasOne("CargoHubWeb.Models.Employee", "EmployeeNumNavigation")
                        .WithMany("Orders")
                        .HasForeignKey("EmployeeNum")
                        .IsRequired()
                        .HasConstraintName("FK__orders__employee__778AC167");

                    b.HasOne("CargoHubWeb.Models.Depot", "FromDepotNavigation")
                        .WithMany("OrderFromDepotNavigations")
                        .HasForeignKey("FromDepot")
                        .IsRequired()
                        .HasConstraintName("FK__orders__fromDepo__75A278F5");

                    b.HasOne("CargoHubWeb.Models.Depot", "ToDepotNavigation")
                        .WithMany("OrderToDepotNavigations")
                        .HasForeignKey("ToDepot")
                        .IsRequired()
                        .HasConstraintName("FK__orders__toDepot__74AE54BC");

                    b.Navigation("Customer");

                    b.Navigation("EmployeeNumNavigation");

                    b.Navigation("FromDepotNavigation");

                    b.Navigation("ToDepotNavigation");
                });

            modelBuilder.Entity("CargoHubWeb.Models.Customer", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("CargoHubWeb.Models.Depot", b =>
                {
                    b.Navigation("OrderFromDepotNavigations");

                    b.Navigation("OrderToDepotNavigations");
                });

            modelBuilder.Entity("CargoHubWeb.Models.Employee", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
