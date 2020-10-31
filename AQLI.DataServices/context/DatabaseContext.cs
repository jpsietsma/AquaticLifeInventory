using AQLI.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AQLI.DataServices.context
{
    public class DatabaseContext : DbContext
    {
        public DbSet<TankTypeModel> TankType { get; set; }
        public DbSet<WaterTypeModel> WaterType { get; set; }
        public DbSet<CreatureTypeModel> CreatureType { get; set; }
        public DbSet<EnvironmentModel> Environment { get; set; }
        public DbSet<TempormentModel> Temporment { get; set; }
       
        public DbSet<MedicalRecordModel> MedicalRecord { get; set; }
        public DbSet<AquaticTankModel> Tank { get; set; }
        //public DbSet<FishCreatureModel> Fish { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<WaterTypeModel>().HasKey("WaterTypeID");
            modelBuilder.Entity<CreatureTypeModel>().HasKey("CreatureTypeID");
            modelBuilder.Entity<TempormentModel>().HasKey("TempormentID");
            modelBuilder.Entity<EnvironmentModel>().HasKey("EnvironmentID");
            modelBuilder.Entity<TankTypeModel>().HasKey("TankTypeID");
            modelBuilder.Entity<MedicalRecordModel>().HasKey("MedicalRecordID");

            modelBuilder.Entity<AquaticTankModel>()
                .HasOne(wt => wt.WaterType)
                .WithMany(t => t.Tanks);           
            modelBuilder.Entity<AquaticTankModel>()
                .HasOne(ct => ct.CreatureType)
                .WithMany(t => t.Tanks);            
            modelBuilder.Entity<AquaticTankModel>()
                .HasOne(tem => tem.Temporment)
                .WithMany(t => t.Tanks);
            modelBuilder.Entity<AquaticTankModel>()
                .HasOne(e => e.Environment)
                .WithMany(t => t.Tanks);
            modelBuilder.Entity<AquaticTankModel>()
                .HasOne(tt => tt.TankType)
                .WithMany(t => t.Tanks);
            modelBuilder.Entity<AquaticTankModel>()
                .Property(x => x.TankID).ValueGeneratedOnAdd();
        }
    }
}
