﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TieFighter.Models;

namespace TieFighter.Data.Migrations
{
    [DbContext(typeof(TieFighterContext))]
    [Migration("20171118222032_Migration5")]
    partial class Migration5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TieFighterV2.Models.AspNetRoleClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("TieFighterV2.Models.AspNetRoles", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("TieFighterV2.Models.AspNetUserClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("TieFighterV2.Models.AspNetUserLogins", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("TieFighterV2.Models.AspNetUserRoles", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("TieFighterV2.Models.AspNetUsers", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<double>("DisplayLevel");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(120)
                        .IsUnicode(false);

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Thumbnail")
                        .HasMaxLength(512);

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("([NormalizedUserName] IS NOT NULL)");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TieFighterV2.Models.AspNetUserTokens", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TieFighterV2.Models.Medal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<string>("MedalName");

                    b.Property<int>("PointsWorth");

                    b.HasKey("Id");

                    b.ToTable("Medals");
                });

            modelBuilder.Entity("TieFighterV2.Models.Missions", b =>
                {
                    b.Property<int>("MissionId")
                        .HasColumnName("MissionID");

                    b.Property<int?>("FkTour")
                        .HasColumnName("FK_Tour");

                    b.Property<string>("MissionName")
                        .HasMaxLength(120)
                        .IsUnicode(false);

                    b.HasKey("MissionId");

                    b.HasIndex("FkTour");

                    b.ToTable("Missions");
                });

            modelBuilder.Entity("TieFighterV2.Models.Ships", b =>
                {
                    b.Property<int>("ShipId")
                        .HasColumnName("ShipID");

                    b.Property<string>("ShipFolder")
                        .HasMaxLength(120)
                        .IsUnicode(false);

                    b.Property<string>("ShipName")
                        .HasMaxLength(120)
                        .IsUnicode(false);

                    b.HasKey("ShipId");

                    b.ToTable("Ships");
                });

            modelBuilder.Entity("TieFighterV2.Models.Tours", b =>
                {
                    b.Property<int>("TourId")
                        .HasColumnName("TourID");

                    b.Property<string>("TourName")
                        .HasMaxLength(120)
                        .IsUnicode(false);

                    b.HasKey("TourId");

                    b.ToTable("Tours");
                });

            modelBuilder.Entity("TieFighterV2.Models.AspNetRoleClaims", b =>
                {
                    b.HasOne("TieFighterV2.Models.AspNetRoles", "Role")
                        .WithMany("AspNetRoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TieFighterV2.Models.AspNetUserClaims", b =>
                {
                    b.HasOne("TieFighterV2.Models.AspNetUsers", "User")
                        .WithMany("AspNetUserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TieFighterV2.Models.AspNetUserLogins", b =>
                {
                    b.HasOne("TieFighterV2.Models.AspNetUsers", "User")
                        .WithMany("AspNetUserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TieFighterV2.Models.AspNetUserRoles", b =>
                {
                    b.HasOne("TieFighterV2.Models.AspNetRoles", "Role")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TieFighterV2.Models.AspNetUsers", "User")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TieFighterV2.Models.Missions", b =>
                {
                    b.HasOne("TieFighterV2.Models.Tours", "FkTourNavigation")
                        .WithMany("Missions")
                        .HasForeignKey("FkTour")
                        .HasConstraintName("FK__Missions__FK_Tou__2C3393D0");
                });
#pragma warning restore 612, 618
        }
    }
}