using Microsoft.EntityFrameworkCore;
using OMA_Data.Entities;
using OMA_Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute = OMA_Data.Entities.Attribute;
using Task = OMA_Data.Entities.Task;

namespace OMA_Data.Data
{
    public class OMAContext(DbContextOptions<OMAContext> options) : DbContext(options)
    {
        public DbSet<Turbine> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<OMA_Data.Entities.Attribute> Attributes { get; set; }
        public DbSet<DeviceData> DeviceData { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<DeviceAction> DeviceAction { get; set; }
        public DbSet<Turbine> Turbines { get; set; }
        public DbSet<OMA_Data.Entities.Task> Tasks { get; set; }
        public DbSet<Drone> Drones { get; set; }
        public DbSet<Island> Islands { get; set; }
        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<AlarmConfig> AlarmsConfig { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region AutoInclude
            modelBuilder.Entity<Alarm>()
                .Navigation(x => x.Turbine)
                .AutoInclude();
            modelBuilder.Entity<Alarm>()
                .Navigation(x => x.Island)
                .AutoInclude();
            modelBuilder.Entity<AlarmConfig>()
                .Navigation(x => x.Island)
                .AutoInclude();
            modelBuilder.Entity<Island>()
                .Navigation(x => x.Turbines)
                .AutoInclude();
            modelBuilder.Entity<Turbine>()
                .Navigation(x => x.Devices)
                .AutoInclude();
            modelBuilder.Entity<Turbine>()
                .Navigation(x => x.Island)
                .AutoInclude();
            modelBuilder.Entity<Entities.Task>()
                .Navigation(x => x.Owner)
                .AutoInclude();
            modelBuilder.Entity<Entities.Task>()
                .Navigation(x => x.User)
                .AutoInclude();
            modelBuilder.Entity<Entities.Task>()
                .Navigation(x => x.Turbine)
                .AutoInclude();
            modelBuilder.Entity<Log>()
                .Navigation(x => x.User)
                .AutoInclude();
            modelBuilder.Entity<Drone>()
                .Navigation(x => x.Task)
                .AutoInclude();
            modelBuilder.Entity<Device>()
                .Navigation(x => x.Turbine)
                .AutoInclude();
            modelBuilder.Entity<Device>()
                .Navigation(x => x.DeviceAction)
                .AutoInclude();
            modelBuilder.Entity<Device>()
                .Navigation(x => x.DeviceData)
                .AutoInclude();
            modelBuilder.Entity<DeviceAction>()
                .Navigation(x => x.Device)
                .AutoInclude();
            modelBuilder.Entity<DeviceData>()
                .Navigation(x => x.Device)
                .AutoInclude();
            modelBuilder.Entity<DeviceData>()
                .Navigation(x => x.Attributes)
                .AutoInclude();
            modelBuilder.Entity<Attribute>()
                .Navigation(x => x.DeviceData)
                .AutoInclude();
            #endregion

            //Need this to avoid Cascade Delete on Tasks, now it just deletes the UserFk
            //modelBuilder.Entity<Task>()
            //   .HasOne(t => t.User)
            //   .WithMany()
            //   .HasForeignKey("UserFK")
            //   .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Alarm>()
                .Property<int>("IslandFK");
            modelBuilder.Entity<Alarm>()
                .Property<int?>("TurbineFK");

            modelBuilder.Entity<AlarmConfig>()
                .Property<int>("IslandFK");

            modelBuilder.Entity<Drone>()
                .Property<int?>("TaskFK");

            modelBuilder.Entity<Task>()
                .Property<int>("TurbineFK");
            modelBuilder.Entity<Task>()
                .Property<Guid>("OwnerFK");


            modelBuilder.Entity<Turbine>()
                .Property<int>("IslandFK");

            modelBuilder.Entity<Island>().HasData(
                new { IslandID = 1, Title = "Island One", ClientID = "ClientA", Abbreviation = "IS1" },
                new { IslandID = 2, Title = "Island Two", ClientID = "ClientB", Abbreviation = "IS2" }
            );

            modelBuilder.Entity<Turbine>().HasData(
                new { TurbineID = 1, Title = "Turbine 1 - Island 1", ClientID = "ClientA", IslandFK = 1 },
                new { TurbineID = 2, Title = "Turbine 2 - Island 1", ClientID = "ClientA", IslandFK = 1 },
                new { TurbineID = 3, Title = "Turbine 3 - Island 1", ClientID = "ClientA", IslandFK = 1 },
                new { TurbineID = 4, Title = "Turbine 4 - Island 1", ClientID = "ClientA", IslandFK = 1 },
                new { TurbineID = 5, Title = "Turbine 5 - Island 1", ClientID = "ClientA", IslandFK = 1 },
                new { TurbineID = 6, Title = "Turbine 1 - Island 2", ClientID = "ClientB", IslandFK = 2 },
                new { TurbineID = 7, Title = "Turbine 2 - Island 2", ClientID = "ClientB", IslandFK = 2 },
                new { TurbineID = 8, Title = "Turbine 3 - Island 2", ClientID = "ClientB", IslandFK = 2 },
                new { TurbineID = 9, Title = "Turbine 4 - Island 2", ClientID = "ClientB", IslandFK = 2 },
                new { TurbineID = 10, Title = "Turbine 5 - Island 2", ClientID = "ClientB", IslandFK = 2 }
            );

            modelBuilder.Entity<Alarm>().HasData(
                new { AlarmID = 1, IslandFK = 1, TurbineFK = (int?)1 },
                new { AlarmID = 2, IslandFK = 2, TurbineFK = (int?)6 }
            );

            modelBuilder.Entity<AlarmConfig>().HasData(
                new { AlarmConfigID = 1, IslandFK = 1, MinTemperature = 15, MaxTemperature = 30, MinHumidity = 30, MaxHumidity = 70, MinAirPressure = 1000, MaxAirPressure = 1050 },
                new { AlarmConfigID = 2, IslandFK = 2, MinTemperature = 15, MaxTemperature = 30, MinHumidity = 30, MaxHumidity = 70, MinAirPressure = 1000, MaxAirPressure = 1050 }
            );


            modelBuilder.Entity<Drone>().HasData(
                new { DroneID = 1, Title = "Drone One", Available = true, TaskFK = 1 },
                new { DroneID = 2, Title = "Drone Two", Available = false, TaskFK = 2 }
            );


            modelBuilder.Entity<User>().HasData(
                new User { UserID = Guid.Parse("c6936336-4a10-4445-b373-60f6a37a58c4"), FullName = "Admin User", Email = "admin@example.com", Phone = "1234567890" },
                new User { UserID = Guid.Parse("cf9844c4-55aa-4eef-bba2-9b97771a8c29"), FullName = "Hotline User", Email = "hotlineuser@example.com", Phone = "0987654321" }
            );


            modelBuilder.Entity<Task>().HasData(
                new { TaskID = 1, Title = "Task One", IsCompleted = false, Type = "Type A", Description = "Description for Task One", FinishDescription = "Finish Task One", TurbineFK = 1, Level = LevelEnum.Hotline1 ,OwnerFK = Guid.Parse("c6936336-4a10-4445-b373-60f6a37a58c4"), UserFK = Guid.Parse("c6936336-4a10-4445-b373-60f6a37a58c4") },
                new { TaskID = 2, Title = "Task Two", IsCompleted = false, Type = "Type B", Description = "Description for Task Two", FinishDescription = "Finish Task Two", TurbineFK = 6, Level = LevelEnum.Hotline1 ,OwnerFK = Guid.Parse("cf9844c4-55aa-4eef-bba2-9b97771a8c29"), UserFK = Guid.Parse("cf9844c4-55aa-4eef-bba2-9b97771a8c29") }
            );


        }
    }
}

