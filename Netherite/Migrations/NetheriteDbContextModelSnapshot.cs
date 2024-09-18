﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Netherite.Data;

#nullable disable

namespace Netherite.Migrations
{
    [DbContext(typeof(NetheriteDbContext))]
    partial class NetheriteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Netherite.Domain.Entitys.CurrencyPairsEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Icon")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("InterestRate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("NameTwo")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CurrencyPairs");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.CurrencyPairsIntervalEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CurrencyPairsId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("IntervalId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyPairsId");

                    b.HasIndex("IntervalId");

                    b.ToTable("CurrencyPairsIntervals");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.FavoritesEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CurrencyPairsId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyPairsId");

                    b.HasIndex("UserId");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.IntervalEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Time")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Intervals");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.MinerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Reward")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Miners");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.OrderEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Bet")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("CurrencyPairsId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Ended")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("PurchaseDirection")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("StartPrice")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.TaskEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Reward")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Balance")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("InvitedId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPremium")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Profit")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TelegramId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TelegramName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Wallet")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.UserTaskEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.HasIndex("UserId");

                    b.ToTable("UserTasks");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.CurrencyPairsIntervalEntity", b =>
                {
                    b.HasOne("Netherite.Domain.Entitys.CurrencyPairsEntity", "CurrencyPairs")
                        .WithMany("CurrencyPairsIntervals")
                        .HasForeignKey("CurrencyPairsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Netherite.Domain.Entitys.IntervalEntity", "Interval")
                        .WithMany("CurrencyPairsIntervals")
                        .HasForeignKey("IntervalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrencyPairs");

                    b.Navigation("Interval");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.FavoritesEntity", b =>
                {
                    b.HasOne("Netherite.Domain.Entitys.CurrencyPairsEntity", "CurrencyPairs")
                        .WithMany("Favorites")
                        .HasForeignKey("CurrencyPairsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Netherite.Domain.Entitys.UserEntity", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrencyPairs");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.UserTaskEntity", b =>
                {
                    b.HasOne("Netherite.Domain.Entitys.TaskEntity", "Task")
                        .WithMany("UserTasks")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Netherite.Domain.Entitys.UserEntity", "User")
                        .WithMany("UserTasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Task");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.CurrencyPairsEntity", b =>
                {
                    b.Navigation("CurrencyPairsIntervals");

                    b.Navigation("Favorites");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.IntervalEntity", b =>
                {
                    b.Navigation("CurrencyPairsIntervals");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.TaskEntity", b =>
                {
                    b.Navigation("UserTasks");
                });

            modelBuilder.Entity("Netherite.Domain.Entitys.UserEntity", b =>
                {
                    b.Navigation("Favorites");

                    b.Navigation("UserTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
