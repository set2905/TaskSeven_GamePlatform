﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskSeven_GamePlatform.Server.Domain;

#nullable disable

namespace TaskSeven_GamePlatform.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230817155908__winner")]
    partial class _winner
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TaskSeven_GamePlatform.Shared.Models.GameState", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Field")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GameTypeName")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDraw")
                        .HasColumnType("bit");

                    b.Property<bool>("IsGameOver")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastMove")
                        .HasColumnType("datetime2");

                    b.Property<int>("MovesLeft")
                        .HasColumnType("int");

                    b.Property<Guid?>("Player1Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Player2Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SecondsPerMove")
                        .HasColumnType("int");

                    b.Property<Guid?>("WinnerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GameTypeName");

                    b.HasIndex("Player1Id");

                    b.HasIndex("Player2Id");

                    b.HasIndex("WinnerId");

                    b.ToTable("GameStates");
                });

            modelBuilder.Entity("TaskSeven_GamePlatform.Shared.Models.GameType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("FieldSize")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GameTypes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("706c2e99-6f6c-4472-81a5-43c56e11637c"),
                            FieldSize = 9,
                            Name = ""
                        });
                });

            modelBuilder.Entity("TaskSeven_GamePlatform.Shared.Models.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConnectionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CurrentGameTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("GameSearchStarted")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPlaying")
                        .HasColumnType("bit");

                    b.Property<bool>("LookingForOpponent")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("WaitingForMove")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CurrentGameTypeId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("TaskSeven_GamePlatform.Shared.Models.GameState", b =>
                {
                    b.HasOne("TaskSeven_GamePlatform.Shared.Models.GameType", "GameType")
                        .WithMany()
                        .HasForeignKey("GameTypeName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskSeven_GamePlatform.Shared.Models.Player", "Player1")
                        .WithMany()
                        .HasForeignKey("Player1Id");

                    b.HasOne("TaskSeven_GamePlatform.Shared.Models.Player", "Player2")
                        .WithMany()
                        .HasForeignKey("Player2Id");

                    b.HasOne("TaskSeven_GamePlatform.Shared.Models.Player", "Winner")
                        .WithMany()
                        .HasForeignKey("WinnerId");

                    b.Navigation("GameType");

                    b.Navigation("Player1");

                    b.Navigation("Player2");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("TaskSeven_GamePlatform.Shared.Models.Player", b =>
                {
                    b.HasOne("TaskSeven_GamePlatform.Shared.Models.GameType", "CurrentGameType")
                        .WithMany()
                        .HasForeignKey("CurrentGameTypeId");

                    b.Navigation("CurrentGameType");
                });
#pragma warning restore 612, 618
        }
    }
}
