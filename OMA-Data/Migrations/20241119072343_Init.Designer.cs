﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OMA_Data.Data;

#nullable disable

namespace OMA_Data.Migrations
{
    [DbContext(typeof(OMAContext))]
    [Migration("20241119072343_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OMA_Data.Entities.Alarm", b =>
                {
                    b.Property<int>("AlarmID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AlarmID"));

                    b.Property<int>("IslandFK")
                        .HasColumnType("int");

                    b.Property<int?>("TurbineFK")
                        .HasColumnType("int");

                    b.HasKey("AlarmID");

                    b.HasIndex("IslandFK");

                    b.HasIndex("TurbineFK");

                    b.ToTable("Alarms");

                    b.HasData(
                        new
                        {
                            AlarmID = 1,
                            IslandFK = 1,
                            TurbineFK = 1
                        },
                        new
                        {
                            AlarmID = 2,
                            IslandFK = 2,
                            TurbineFK = 6
                        });
                });

            modelBuilder.Entity("OMA_Data.Entities.AlarmConfig", b =>
                {
                    b.Property<int>("AlarmConfigID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AlarmConfigID"));

                    b.Property<int>("IslandFK")
                        .HasColumnType("int");

                    b.Property<int>("MaxAirPressure")
                        .HasColumnType("int");

                    b.Property<int>("MaxHumidity")
                        .HasColumnType("int");

                    b.Property<int>("MaxTemperature")
                        .HasColumnType("int");

                    b.Property<int>("MinAirPressure")
                        .HasColumnType("int");

                    b.Property<int>("MinHumidity")
                        .HasColumnType("int");

                    b.Property<int>("MinTemperature")
                        .HasColumnType("int");

                    b.HasKey("AlarmConfigID");

                    b.HasIndex("IslandFK");

                    b.ToTable("AlarmsConfig");

                    b.HasData(
                        new
                        {
                            AlarmConfigID = 1,
                            IslandFK = 1,
                            MaxAirPressure = 1050,
                            MaxHumidity = 70,
                            MaxTemperature = 30,
                            MinAirPressure = 1000,
                            MinHumidity = 30,
                            MinTemperature = 15
                        },
                        new
                        {
                            AlarmConfigID = 2,
                            IslandFK = 2,
                            MaxAirPressure = 1050,
                            MaxHumidity = 70,
                            MaxTemperature = 30,
                            MinAirPressure = 1000,
                            MinHumidity = 30,
                            MinTemperature = 15
                        });
                });

            modelBuilder.Entity("OMA_Data.Entities.Device", b =>
                {
                    b.Property<int>("DeviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeviceId"));

                    b.Property<string>("ClientID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int>("TurbineID")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DeviceId");

                    b.HasIndex("TurbineID");

                    b.ToTable("Device");

                    b.HasData(
                        new
                        {
                            DeviceId = 1,
                            ClientID = "ClientA",
                            State = 1,
                            TurbineID = 1,
                            Type = "Sensor"
                        },
                        new
                        {
                            DeviceId = 2,
                            ClientID = "ClientB",
                            State = 1,
                            TurbineID = 1,
                            Type = "Actuator"
                        });
                });

            modelBuilder.Entity("OMA_Data.Entities.Drone", b =>
                {
                    b.Property<int>("DroneID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DroneID"));

                    b.Property<bool>("Available")
                        .HasColumnType("bit");

                    b.Property<int?>("TaskFK")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DroneID");

                    b.HasIndex("TaskFK");

                    b.ToTable("Drones");

                    b.HasData(
                        new
                        {
                            DroneID = 1,
                            Available = true,
                            TaskFK = 1,
                            Title = "Drone One"
                        },
                        new
                        {
                            DroneID = 2,
                            Available = false,
                            TaskFK = 2,
                            Title = "Drone Two"
                        });
                });

            modelBuilder.Entity("OMA_Data.Entities.Island", b =>
                {
                    b.Property<int>("IslandID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IslandID"));

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IslandID");

                    b.ToTable("Islands");

                    b.HasData(
                        new
                        {
                            IslandID = 1,
                            Abbreviation = "IS1",
                            ClientID = "ClientA",
                            Title = "Island One"
                        },
                        new
                        {
                            IslandID = 2,
                            Abbreviation = "IS2",
                            ClientID = "ClientB",
                            Title = "Island Two"
                        });
                });

            modelBuilder.Entity("OMA_Data.Entities.Log", b =>
                {
                    b.Property<int>("LogID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LogID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Severity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserFK")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LogID");

                    b.HasIndex("UserFK");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("OMA_Data.Entities.Task", b =>
                {
                    b.Property<int>("TaskID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaskID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FinishDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<Guid>("OwnerFK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TurbineFK")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserFk")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TaskID");

                    b.HasIndex("OwnerFK");

                    b.HasIndex("TurbineFK");

                    b.HasIndex("UserFk");

                    b.ToTable("Tasks");

                    b.HasData(
                        new
                        {
                            TaskID = 1,
                            Description = "Description for Task One",
                            FinishDescription = "Finish Task One",
                            IsCompleted = false,
                            Level = 1,
                            OwnerFK = new Guid("c6936336-4a10-4445-b373-60f6a37a58c4"),
                            Title = "Task One",
                            TurbineFK = 1,
                            Type = "Type A"
                        },
                        new
                        {
                            TaskID = 2,
                            Description = "Description for Task Two",
                            FinishDescription = "Finish Task Two",
                            IsCompleted = false,
                            Level = 1,
                            OwnerFK = new Guid("cf9844c4-55aa-4eef-bba2-9b97771a8c29"),
                            Title = "Task Two",
                            TurbineFK = 6,
                            Type = "Type B"
                        });
                });

            modelBuilder.Entity("OMA_Data.Entities.Turbine", b =>
                {
                    b.Property<int>("TurbineID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TurbineID"));

                    b.Property<string>("ClientID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IslandFK")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TurbineID");

                    b.HasIndex("IslandFK");

                    b.ToTable("Turbine");

                    b.HasData(
                        new
                        {
                            TurbineID = 1,
                            ClientID = "ClientA",
                            IslandFK = 1,
                            Title = "Turbine 1 - Island 1"
                        },
                        new
                        {
                            TurbineID = 2,
                            ClientID = "ClientA",
                            IslandFK = 1,
                            Title = "Turbine 2 - Island 1"
                        },
                        new
                        {
                            TurbineID = 3,
                            ClientID = "ClientA",
                            IslandFK = 1,
                            Title = "Turbine 3 - Island 1"
                        },
                        new
                        {
                            TurbineID = 4,
                            ClientID = "ClientA",
                            IslandFK = 1,
                            Title = "Turbine 4 - Island 1"
                        },
                        new
                        {
                            TurbineID = 5,
                            ClientID = "ClientA",
                            IslandFK = 1,
                            Title = "Turbine 5 - Island 1"
                        },
                        new
                        {
                            TurbineID = 6,
                            ClientID = "ClientB",
                            IslandFK = 2,
                            Title = "Turbine 1 - Island 2"
                        },
                        new
                        {
                            TurbineID = 7,
                            ClientID = "ClientB",
                            IslandFK = 2,
                            Title = "Turbine 2 - Island 2"
                        },
                        new
                        {
                            TurbineID = 8,
                            ClientID = "ClientB",
                            IslandFK = 2,
                            Title = "Turbine 3 - Island 2"
                        },
                        new
                        {
                            TurbineID = 9,
                            ClientID = "ClientB",
                            IslandFK = 2,
                            Title = "Turbine 4 - Island 2"
                        },
                        new
                        {
                            TurbineID = 10,
                            ClientID = "ClientB",
                            IslandFK = 2,
                            Title = "Turbine 5 - Island 2"
                        });
                });

            modelBuilder.Entity("OMA_Data.Entities.User", b =>
                {
                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            UserID = new Guid("c6936336-4a10-4445-b373-60f6a37a58c4"),
                            Email = "admin@example.com",
                            FullName = "Admin User",
                            Phone = "1234567890"
                        },
                        new
                        {
                            UserID = new Guid("cf9844c4-55aa-4eef-bba2-9b97771a8c29"),
                            Email = "hotlineuser@example.com",
                            FullName = "Hotline User",
                            Phone = "0987654321"
                        });
                });

            modelBuilder.Entity("OMA_Data.Entities.Alarm", b =>
                {
                    b.HasOne("OMA_Data.Entities.Island", "Island")
                        .WithMany()
                        .HasForeignKey("IslandFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OMA_Data.Entities.Turbine", "Turbine")
                        .WithMany()
                        .HasForeignKey("TurbineFK");

                    b.Navigation("Island");

                    b.Navigation("Turbine");
                });

            modelBuilder.Entity("OMA_Data.Entities.AlarmConfig", b =>
                {
                    b.HasOne("OMA_Data.Entities.Island", "Island")
                        .WithMany()
                        .HasForeignKey("IslandFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Island");
                });

            modelBuilder.Entity("OMA_Data.Entities.Device", b =>
                {
                    b.HasOne("OMA_Data.Entities.Turbine", "Turbine")
                        .WithMany("Devices")
                        .HasForeignKey("TurbineID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Turbine");
                });

            modelBuilder.Entity("OMA_Data.Entities.Drone", b =>
                {
                    b.HasOne("OMA_Data.Entities.Task", "Task")
                        .WithMany()
                        .HasForeignKey("TaskFK");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("OMA_Data.Entities.Log", b =>
                {
                    b.HasOne("OMA_Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("OMA_Data.Entities.Task", b =>
                {
                    b.HasOne("OMA_Data.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OMA_Data.Entities.Turbine", "Turbine")
                        .WithMany()
                        .HasForeignKey("TurbineFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OMA_Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserFk");

                    b.Navigation("Owner");

                    b.Navigation("Turbine");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OMA_Data.Entities.Turbine", b =>
                {
                    b.HasOne("OMA_Data.Entities.Island", "Island")
                        .WithMany("Turbines")
                        .HasForeignKey("IslandFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Island");
                });

            modelBuilder.Entity("OMA_Data.Entities.Island", b =>
                {
                    b.Navigation("Turbines");
                });

            modelBuilder.Entity("OMA_Data.Entities.Turbine", b =>
                {
                    b.Navigation("Devices");
                });
#pragma warning restore 612, 618
        }
    }
}
