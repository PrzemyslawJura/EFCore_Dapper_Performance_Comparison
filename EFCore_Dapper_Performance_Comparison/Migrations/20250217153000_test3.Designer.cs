﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Warehouse.Infrastructure.Common;

#nullable disable

namespace EFCore_Dapper_Performance_Comparison.Migrations
{
    [DbContext(typeof(WarehouseDbContext))]
    [Migration("20250217153000_test3")]
    partial class test3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("EFCore_Dapper_Performance_Comparison.Models.Products.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("EFCore_Dapper_Performance_Comparison.Models.Transactions.Transaction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WarehouseRackId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WorkerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("WarehouseRackId");

                    b.HasIndex("WorkerId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("EFCore_Dapper_Performance_Comparison.Models.WarehouseRacks.WarehouseRack", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rack")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Sector")
                        .HasColumnType("INTEGER");

                    b.Property<string>("WarehouseSizeId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("WarehouseSizeId");

                    b.ToTable("WarehouseRacks");
                });

            modelBuilder.Entity("EFCore_Dapper_Performance_Comparison.Models.WarehousesSize.WarehouseSize", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("RackQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SectorNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("WarehousesSize");
                });

            modelBuilder.Entity("EFCore_Dapper_Performance_Comparison.Models.Workers.Worker", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("EFCore_Dapper_Performance_Comparison.Models.Transactions.Transaction", b =>
                {
                    b.HasOne("EFCore_Dapper_Performance_Comparison.Models.Products.Product", "Products")
                        .WithMany("Transactions")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFCore_Dapper_Performance_Comparison.Models.WarehouseRacks.WarehouseRack", "WarehouseRacks")
                        .WithMany("Transactions")
                        .HasForeignKey("WarehouseRackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFCore_Dapper_Performance_Comparison.Models.Workers.Worker", "Workers")
                        .WithMany("Transactions")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Products");

                    b.Navigation("WarehouseRacks");

                    b.Navigation("Workers");
                });

            modelBuilder.Entity("EFCore_Dapper_Performance_Comparison.Models.WarehouseRacks.WarehouseRack", b =>
                {
                    b.HasOne("EFCore_Dapper_Performance_Comparison.Models.WarehousesSize.WarehouseSize", "WarehousesSize")
                        .WithMany("WarehouseRacks")
                        .HasForeignKey("WarehouseSizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WarehousesSize");
                });

            modelBuilder.Entity("EFCore_Dapper_Performance_Comparison.Models.Products.Product", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("EFCore_Dapper_Performance_Comparison.Models.WarehouseRacks.WarehouseRack", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("EFCore_Dapper_Performance_Comparison.Models.WarehousesSize.WarehouseSize", b =>
                {
                    b.Navigation("WarehouseRacks");
                });

            modelBuilder.Entity("EFCore_Dapper_Performance_Comparison.Models.Workers.Worker", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
