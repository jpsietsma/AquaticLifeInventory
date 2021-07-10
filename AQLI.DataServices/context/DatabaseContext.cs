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

        public DbSet<UserFish_MedicalRecordModel> UserFish_MedicalRecords { get; set; }
        public DbSet<MedicalRecordTypeModel> MedicalRecordTypes { get; set; }

        public DbSet<AquaticTankModel> Tank { get; set; }
        public DbSet<FishTypeModel> FishTypes { get; set; }
        public DbSet<FishFamilyModel> FishFamilyTypes { get; set; }
        public DbSet<FishFeedingTypeModel> FishFeedingTypes { get; set; }
        public DbSet<FishStatusModel> FishStatuses { get; set; }
        public DbSet<BirthingTypeModel> BirthingTypes { get; set; }
        public DbSet<TerritorialLevelModel> TerritorialLevels { get; set; }

        public DbSet<MaintenanceLogType> MaintenanceLogTypes { get; set; }
        public DbSet<MaintenanceLogModel> MaintenanceLogs { get; set; }

        public DbSet<TankTemperatureRecordModel> TankTemperatureRecords { get; set; }
        public DbSet<TankWaterChangeRecordModel> TankWaterChangeRecords { get; set; }
        public DbSet<TankFilterChangeRecordModel> TankFilterChangeRecords { get; set; }
        public DbSet<TankFeedingRecordModel> TankFeedingRecords { get; set; }
        public DbSet<TankCreatureInventoryRecordModel> TankCreatureInventoryRecords { get; set; }
        public DbSet<UserTankInventoryRecordModel> UserTankInventoryRecords { get; set; }

        public DbSet<WebsiteUser> AspNetUsers { get; set; }
        public DbSet<IdentityUserClaim<string>> AspNetUserClaims { get; set; }
        public DbSet<IdentityUserRole<string>> AspNetUserRoles { get; set; }
        public DbSet<IdentityUserLogin<string>> AspNetUserLogins { get; set; }
        public DbSet<IdentityUserToken<string>> AspNetUserTokens { get; set; }

        public DbSet<NotificationModel> Notification { get; set; }
        public DbSet<NotificationPriorityLevelModel> NotificationPriorityLevels { get; set; }

        public DbSet<UserFishModel> UserFish { get; set; }
        public DbSet<TankSupplyModel> Tank_Supply { get; set; }
        public DbSet<TankEquipmentModel> Tank_Equipment { get; set; }
        public DbSet<TankNoteModel> TankNotes { get; set; }
        public DbSet<PurchaseCategoryTypeModel> PurchaseCategoryTypes { get; set; }

        public DbSet<PlantTypeModel> PlantTypes { get; set; }
        public DbSet<UserPlantModel> UserPlants { get; set; }

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
            modelBuilder.Entity<UserFish_MedicalRecordModel>().HasKey("UserFishMedicalRecordID");
            modelBuilder.Entity<PurchaseInvoiceModel>().HasKey("PurchaseInvoiceID");
            modelBuilder.Entity<PurchaseModel>().HasKey("PurchaseID");
            modelBuilder.Entity<PurchaseCategoryModel>().HasKey("PurchaseCategoryID");
            modelBuilder.Entity<StoreModel>().HasKey("StoreID");
            modelBuilder.Entity<NotificationPriorityLevelModel>().HasKey("NotificationPriorityLevelID");
            modelBuilder.Entity<FishTypeModel>().HasKey("CreatureTypeID");

            #region Section: NotificationModel modelBuilders
            modelBuilder.Entity<NotificationModel>()
                .HasOne(o => o.WebsiteUser)
                .WithMany(u => u.Notifications)
                .HasPrincipalKey(o => o.UserId);
            #endregion

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
                .HasMany(fp => fp.UserFish)
                .WithOne(t => t.Tank);
            modelBuilder.Entity<AquaticTankModel>()
                .HasMany(ts => ts.Supplies)
                .WithOne(t => t.Tank);
            modelBuilder.Entity<AquaticTankModel>()
                .HasMany(eq => eq.Equipment)
                .WithOne(t => t.Tank);
            modelBuilder.Entity<AquaticTankModel>()
                .HasOne(o => o.Owner)
                .WithMany()
                .HasPrincipalKey("UserId");
            modelBuilder.Entity<AquaticTankModel>()
                .HasMany(n => n.Notes)
                .WithOne(t => t.Tank);
            modelBuilder.Entity<AquaticTankModel>()
                .Property(x => x.TankID).ValueGeneratedOnAdd();
            modelBuilder.Entity<AquaticTankModel>()
                .HasMany(m => m.MaintenanceLogs)
                .WithOne(t => t.Tank);
            #endregion

            #region Section: MaintenanceLogModel modelbuilders
            modelBuilder.Entity<MaintenanceLogModel>()
                .HasOne(t => t.MaintenanceLogType)
                .WithMany();
            modelBuilder.Entity<MaintenanceLogModel>()
                .HasMany(tl => tl.TemperatureRecords)
                .WithOne(m => m.MaintenanceLog);
            modelBuilder.Entity<MaintenanceLogModel>()
                .HasMany(il => il.CreatureInventoryRecords)
                .WithOne(m => m.MaintenanceLog);
            modelBuilder.Entity<MaintenanceLogModel>()
                .HasMany(wc => wc.WaterChangeRecords)
                .WithOne(m => m.MaintenanceLog);
            modelBuilder.Entity<MaintenanceLogModel>()
                .HasMany(fc => fc.FilterChangeRecords)
                .WithOne(m => m.MaintenanceLog);
            modelBuilder.Entity<MaintenanceLogModel>()
                .HasMany(fl => fl.FeedingRecords)
                .WithOne(m => m.MaintenanceLog);
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

            #region Section: UserFishModel modelBuilders
                modelBuilder.Entity<UserFishModel>()
                    .HasOne(t => t.FishTemporment)
                    .WithMany();

                modelBuilder.Entity<UserFishModel>()
                    .HasOne(s => s.FishStatus)
                    .WithMany();

            modelBuilder.Entity<UserFishModel>()
                .HasMany(cf => cf.ChildrenFish)
                .WithOne(pf => pf.ParentFish);

            modelBuilder.Entity<UserFishModel>()
                .HasMany( mr => mr.MedicalRecords)
                .WithOne(uf => uf.UserFish);

            modelBuilder.Entity<UserFishModel>()
                .HasOne(t => t.FishTemporment)
                .WithOne();

            #endregion

            #region Section: PurchaseModel modelbuilders
            modelBuilder.Entity<PurchaseModel>()
                .HasOne(pt => pt.PurchaseCategory)
                .WithMany(p => p.Purchases)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PurchaseModel>()
                .HasOne(pc => pc.PurchaseCategoryType)
                .WithMany()
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

            #region Section: UserFish_MedicalRecord modelbuilders   



            #endregion

            #region Section: UserPlants modelbuilders
            modelBuilder.Entity<UserPlantModel>()
                .HasOne(pt => pt.PlantType)
                .WithMany();
            #endregion

            #region Section: TankNotes modelbuilders

            #endregion

            //Ignore numeric User ID identity column when updating or inserting
            modelBuilder.Entity<WebsiteUser>().Property(u => u.UserId).Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();
        }
    }
}
