using System;
using System.Collections.Generic;
using System.Text;
using AQLI.Data.Models;
using AQLI.Data.Models.ListModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AQLI.DataServices.context
{
    public class DatabaseContext : DbContext
    {
        public DbSet<TankTypeModel> TankType { get; set; }
        public DbSet<WaterTypeModel> WaterType { get; set; }
        public DbSet<CreatureTypeModel> CreatureType { get; set; }
        public DbSet<EnvironmentModel> Environment { get; set; }
        public DbSet<TempormentModel> Temporment { get; set; }
       
        public DbSet<PurchaseInvoiceModel> PurchaseInvoices { get; set; }
        public DbSet<PurchaseModel> Purchases { get; set; }
        public DbSet<PurchaseCategoryModel> PurchaseCategory { get; set; }
        public DbSet<StoreModel> Stores { get; set; }

        public DbSet<MedicalRecordModel> MedicalRecord { get; set; }
        public DbSet<AquaticTankModel> Tank { get; set; }
        public DbSet<FishTypeModel> FishTypes { get; set; }
        public DbSet<FishFamilyModel> FishFamilyTypes { get; set; }
        public DbSet<FishFeedingTypeModel> FishFeedingTypes { get; set; }
        public DbSet<BirthingTypeModel> BirthingTypes { get; set; }
        public DbSet<TerritorialLevelModel> TerritorialLevels { get; set; }

        public DbSet<WebsiteUser> AspNetUsers { get; set; }
        public DbSet<IdentityUserClaim<string>> AspNetUserClaims { get; set; }
        public DbSet<IdentityUserRole<string>> AspNetUserRoles { get; set; }
        public DbSet<IdentityUserLogin<string>> AspNetUserLogins { get; set; }
        public DbSet<IdentityUserToken<string>> AspNetUserTokens { get; set; }

        public DbSet<NotificationModel> Notification { get; set; }
        
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
            modelBuilder.Entity<PurchaseInvoiceModel>().HasKey("PurchaseInvoiceID");
            modelBuilder.Entity<PurchaseModel>().HasKey("PurchaseID");
            modelBuilder.Entity<PurchaseCategoryModel>().HasKey("PurchaseCategoryID");
            modelBuilder.Entity<StoreModel>().HasKey("StoreID");

            #region Section: AquaticTankModel modelbuilders
            modelBuilder.Entity<AquaticTankModel>()
                .HasOne(wt => wt.WaterType)
                .WithMany(t => t.Tanks)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<AquaticTankModel>()
                .HasOne(ct => ct.CreatureType)
                .WithMany(t => t.Tanks)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<AquaticTankModel>()
                .HasOne(tem => tem.Temporment)
                .WithMany(t => t.Tanks)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<AquaticTankModel>()
                .HasOne(e => e.Environment)
                .WithMany(t => t.Tanks)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<AquaticTankModel>()
                .HasOne(p => p.Purchase)
                .WithOne();
            modelBuilder.Entity<AquaticTankModel>()
                .HasOne(tt => tt.TankType)
                .WithMany(t => t.Tanks)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<AquaticTankModel>()
                .Property(x => x.TankID).ValueGeneratedOnAdd();
            #endregion

            #region Section: InvoicePurchaseModel modelbuilders
            modelBuilder.Entity<PurchaseInvoiceModel>()
                .HasOne(o => o.Owner)
                .WithMany(pi => pi.PurchaseInvoices)
                .HasPrincipalKey(fk => fk.UserId);

            modelBuilder.Entity<PurchaseInvoiceModel>()
                .HasMany(p => p.Purchases)
                .WithOne(p => p.Invoice);

            modelBuilder.Entity<PurchaseInvoiceModel>()
                .HasOne(p => p.Store)
                .WithMany(p => p.Invoices);            

            modelBuilder.Entity<PurchaseInvoiceModel>()
                .Property(x => x.PurchaseInvoiceID).ValueGeneratedOnAdd();

            #endregion

            #region Section: PurchaseModel modelbuilders
            modelBuilder.Entity<PurchaseModel>()
                .HasOne(pt => pt.PurchaseCategory)
                .WithMany(p => p.Purchases)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PurchaseModel>()
                .HasOne(st => st.Store)
                .WithMany(p => p.Purchases)
                .OnDelete(DeleteBehavior.SetNull);
            #endregion

            #region Section: StoreModel modelbuilders
            modelBuilder.Entity<StoreModel>()
                .HasMany(p => p.Purchases)
                .WithOne(p => p.Store);
            #endregion

            //Ignore numeric User ID identity column when updating or inserting
            modelBuilder.Entity<WebsiteUser>().Property(u => u.UserId).Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();
        }
    }
}
