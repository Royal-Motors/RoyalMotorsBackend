﻿// <auto-generated />
using CarWebsiteBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarWebsiteBackend.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CarWebsiteBackend.DTOs.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("verificationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("verified")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("email")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("CarWebsiteBackend.DTOs.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("fuelconsumption")
                        .HasColumnType("real");

                    b.Property<int>("fueltankcapacity")
                        .HasColumnType("int");

                    b.Property<int>("horsepower")
                        .HasColumnType("int");

                    b.Property<string>("image_id_list")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("make")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("mileage")
                        .HasColumnType("int");

                    b.Property<string>("model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("price")
                        .HasColumnType("int");

                    b.Property<string>("transmissiontype")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("used")
                        .HasColumnType("bit");

                    b.Property<string>("video_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("name")
                        .IsUnique();

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("CarWebsiteBackend.DTOs.TestDrive", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<int>("Time")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CarId");

                    b.ToTable("TestDrives");
                });

            modelBuilder.Entity("CarWebsiteBackend.DTOs.TestDrive", b =>
                {
                    b.HasOne("CarWebsiteBackend.DTOs.Account", "Account")
                        .WithMany("TestDrives")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarWebsiteBackend.DTOs.Car", "Car")
                        .WithMany("TestDrives")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Car");
                });

            modelBuilder.Entity("CarWebsiteBackend.DTOs.Account", b =>
                {
                    b.Navigation("TestDrives");
                });

            modelBuilder.Entity("CarWebsiteBackend.DTOs.Car", b =>
                {
                    b.Navigation("TestDrives");
                });
#pragma warning restore 612, 618
        }
    }
}
