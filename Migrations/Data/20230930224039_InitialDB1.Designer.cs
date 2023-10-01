﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WEBAPI.Helpers;

#nullable disable

namespace WEBAPI.Migrations.Data
{
    [DbContext(typeof(DataContext))]
    [Migration("20230930224039_InitialDB1")]
    partial class InitialDB1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WEBAPI.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("create_ts");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TokenID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("update_ts");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WEBAPI.Entities.UserWallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("create_ts");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("update_ts");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<double?>("available_balance")
                        .HasColumnType("float");

                    b.Property<double?>("total_balance")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserWallet");
                });

            modelBuilder.Entity("WEBAPI.Entities.WalletTxn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("create_ts");

                    b.Property<string>("TokenID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int?>("UserWalletId")
                        .HasColumnType("int");

                    b.Property<double>("account_bal")
                        .HasColumnType("float");

                    b.Property<double>("amount")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("UserWalletId");

                    b.ToTable("WalletTxns");
                });

            modelBuilder.Entity("WEBAPI.Entities.UserWallet", b =>
                {
                    b.HasOne("WEBAPI.Entities.User", null)
                        .WithMany("UWallet")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WEBAPI.Entities.WalletTxn", b =>
                {
                    b.HasOne("WEBAPI.Entities.UserWallet", null)
                        .WithMany("WalletTrans")
                        .HasForeignKey("UserWalletId");
                });

            modelBuilder.Entity("WEBAPI.Entities.User", b =>
                {
                    b.Navigation("UWallet");
                });

            modelBuilder.Entity("WEBAPI.Entities.UserWallet", b =>
                {
                    b.Navigation("WalletTrans");
                });
#pragma warning restore 612, 618
        }
    }
}
