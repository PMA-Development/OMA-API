﻿using Microsoft.EntityFrameworkCore;
using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Data
{
    public class OMAContext(DbContextOptions<OMAContext> options) : DbContext(options)
    {
        public DbSet<Turbine> Users { get; set; }   
        public DbSet<Log> Logs { get; set; }
        public DbSet<OMA_Data.Entities.Attribute> Attributes { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Turbine> Turbines { get; set; }
        public DbSet<OMA_Data.Entities.Task> Tasks { get; set; }
        public DbSet<Drone> Drones { get; set; }
        public DbSet<Island> Islands { get; set; }
        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<AlarmConfig> AlarmsConfig { get; set; }

        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
