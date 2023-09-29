﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WEBAPI.Helpers;

#nullable disable

namespace WEBAPI.Migrations
{
    [DbContext(typeof(SqliteDataContext))]
    [Migration("20230929133643_InitialDBContextAzure")]
    partial class InitialDBContextAzure
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("WEBAPI.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT")
                        .HasColumnName("create_ts");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .HasColumnType("TEXT");

                    b.Property<string>("TokenID")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("TEXT")
                        .HasColumnName("update_ts");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WEBAPI.Entities.UserWallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT")
                        .HasColumnName("create_ts");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("TEXT")
                        .HasColumnName("update_ts");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("available_balance")
                        .HasColumnType("REAL");

                    b.Property<double?>("total_balance")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserWallet");
                });

            modelBuilder.Entity("WEBAPI.Entities.UserWallet", b =>
                {
                    b.HasOne("WEBAPI.Entities.User", null)
                        .WithMany("UWallet")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WEBAPI.Entities.User", b =>
                {
                    b.Navigation("UWallet");
                });
#pragma warning restore 612, 618
        }
    }
}
