using AQLI.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public DbSet<TankModel> Tank { get; set; }
        //public DbSet<FishCreatureModel> Fish { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }
    }
}
