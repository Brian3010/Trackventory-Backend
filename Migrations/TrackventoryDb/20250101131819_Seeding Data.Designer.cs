﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using trackventory_backend.Data;

#nullable disable

namespace trackventory_backend.Migrations.TrackventoryDb
{
    [DbContext(typeof(TrackventoryDbContext))]
    [Migration("20250101131819_Seeding Data")]
    partial class SeedingData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("trackventory_backend.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Category");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0055638c-3362-47a7-94cd-056c7993b8b3"),
                            Name = "Retails",
                            UpdatedDate = new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("90d6a812-8b39-408e-bb30-4dd5a3d3664d"),
                            Name = "Components",
                            UpdatedDate = new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("trackventory_backend.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SKU")
                        .HasColumnType("int");

                    b.Property<string>("Site")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Warehouse")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product");

                    b.HasData(
                        new
                        {
                            Id = new Guid("5161df48-6b34-496f-9957-61077b79e56c"),
                            CategoryId = new Guid("0055638c-3362-47a7-94cd-056c7993b8b3"),
                            ProductName = "WB Ethiopia 250g CORE",
                            SKU = 102316,
                            Site = "A009",
                            UpdatedDate = new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Warehouse = "A009"
                        },
                        new
                        {
                            Id = new Guid("845ee88d-e92f-49fb-8140-18b72ac96631"),
                            CategoryId = new Guid("0055638c-3362-47a7-94cd-056c7993b8b3"),
                            ProductName = "Teavana Retail Chai CT/4 CORE",
                            SKU = 102895,
                            Site = "A009",
                            UpdatedDate = new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Warehouse = "A009"
                        },
                        new
                        {
                            Id = new Guid("869ebbb7-8457-40b8-8bba-66b900a56f32"),
                            CategoryId = new Guid("90d6a812-8b39-408e-bb30-4dd5a3d3664d"),
                            ProductName = "Cold Brew Coffee",
                            SKU = 102895,
                            Site = "A009",
                            UpdatedDate = new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Warehouse = "A009"
                        },
                        new
                        {
                            Id = new Guid("3707d0d7-643d-4c42-9ce5-ad0a32f9d23a"),
                            CategoryId = new Guid("90d6a812-8b39-408e-bb30-4dd5a3d3664d"),
                            ProductName = "Espresso Roast Decaf 1lb",
                            SKU = 102895,
                            Site = "A009",
                            UpdatedDate = new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Warehouse = "A009"
                        });
                });

            modelBuilder.Entity("trackventory_backend.Models.StockCount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Counted")
                        .HasColumnType("real");

                    b.Property<Guid>("CountedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountingReasonCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("OnHand")
                        .HasColumnType("real");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Quantity")
                        .HasColumnType("real");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("StockCount");

                    b.HasData(
                        new
                        {
                            Id = new Guid("5050a777-032c-4f9b-815d-c0ff65571f27"),
                            Counted = 0f,
                            CountedBy = new Guid("db5758d4-4a6c-43f3-957d-c04ffa1bde69"),
                            CountingReasonCode = "Stock Update",
                            OnHand = 5f,
                            ProductId = new Guid("5161df48-6b34-496f-9957-61077b79e56c"),
                            Quantity = 0f,
                            UpdatedDate = new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("ff92736d-696e-48a2-b6ad-329df9d14881"),
                            Counted = 0f,
                            CountedBy = new Guid("db5758d4-4a6c-43f3-957d-c04ffa1bde69"),
                            CountingReasonCode = "Stock Update",
                            OnHand = 5f,
                            ProductId = new Guid("845ee88d-e92f-49fb-8140-18b72ac96631"),
                            Quantity = 0f,
                            UpdatedDate = new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("6f138c1d-ee76-4f2d-8d14-49b5377fa3bd"),
                            Counted = 0f,
                            CountedBy = new Guid("db5758d4-4a6c-43f3-957d-c04ffa1bde69"),
                            CountingReasonCode = "Stock Update",
                            OnHand = 64.62f,
                            ProductId = new Guid("869ebbb7-8457-40b8-8bba-66b900a56f32"),
                            Quantity = 0f,
                            UpdatedDate = new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("67d55f8b-a78b-4f82-a970-108c5aa46739"),
                            Counted = 0f,
                            CountedBy = new Guid("db5758d4-4a6c-43f3-957d-c04ffa1bde69"),
                            CountingReasonCode = "Stock Update",
                            OnHand = 19.71f,
                            ProductId = new Guid("3707d0d7-643d-4c42-9ce5-ad0a32f9d23a"),
                            Quantity = 0f,
                            UpdatedDate = new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("trackventory_backend.Models.Product", b =>
                {
                    b.HasOne("trackventory_backend.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("trackventory_backend.Models.StockCount", b =>
                {
                    b.HasOne("trackventory_backend.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });
#pragma warning restore 612, 618
        }
    }
}
