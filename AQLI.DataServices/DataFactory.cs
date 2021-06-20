using AQLI.Data.Models;
using AQLI.Data.Models.ListModels;
using AQLI.DataServices.context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AQLI.DataServices
{
    public class DataFactory
    {
        private readonly DatabaseContext Database;

        public DataFactory(DatabaseContext _context)
        {
            Database = _context;
        }

        #region Section: List Factory Methods
            
            /// <summary>
            /// List all the supplies belonging to a specific tank
            /// </summary>
            /// <param name="TankID">ID of the tank to filter on</param>
            public List<TankSupplyModel> List_TankSupplies(int TankID)
            {
                return Database.Tank_Supply
                    .Where(s => s.TankID == TankID)
                    .ToList();
            }

            /// <summary>
            /// Return all plant types in the database
            /// </summary>
            public List<PlantTypeModel> List_PlantTypes()
            {
                return Database.PlantTypes
                    .ToList();
            }

            /// <summary>
            /// List all user plants, along with PlantType
            /// </summary>
            /// <param name="userId">ID of the user to filter plants</param>
            public List<UserPlantModel> List_UserPlants(int userId)
            {
                return Database.UserPlants
                    .Where(o => o.OwnerID == userId)
                    .Include(pt => pt.PlantType)
                    .ToList();
            }

            /// <summary>
            /// List all tanks found in the database
            /// </summary>
            public List<AquaticTankModel> List_Tanks()
            {
                return Database.Tank
                        .Include(wt => wt.WaterType)
                        .Include(ct => ct.CreatureType)
                        .Include(tem => tem.Temporment)
                        .Include(env => env.Environment)
                        .Include(tt => tt.TankType)
                        .Include(pr => pr.Purchase)
                            .ThenInclude(st => st.Store)
                        .Include(fp => fp.UserFish)
                            .ThenInclude(ft => ft.FishType)
                        .Include(sup => sup.Supplies)
                            .ThenInclude(p => p.Purchase)
                        .Include(eq => eq.Equipment)
                        .Include(mr => mr.MaintenanceLogs)
                        .ThenInclude(tl => tl.TemperatureRecords)
                        .Include(ml => ml.MaintenanceLogs)
                        .ThenInclude(ci => ci.CreatureInventoryRecords)
                        .Include(ml => ml.MaintenanceLogs)
                        .ThenInclude(fc => fc.FilterChangeRecords)
                        .Include(ml => ml.MaintenanceLogs)
                        .ThenInclude(fr => fr.FeedingRecords)
                        .Include(ml => ml.MaintenanceLogs)
                        .ThenInclude(wc => wc.WaterChangeRecords)
                        .Include(o => o.Owner)
                        .Include(n => n.Notes)
                        .ToList();
            }

            /// <summary>
            /// List all notifications in the database
            /// </summary>
            public List<NotificationModel> List_Notifications()
            {
                return Database.Notification
                    .Include(np => np.NotificationPriorityLevel)
                    .ToList();
            }

            /// <summary>
            /// List all notifications pending acknowledgement in the database
            /// </summary>
            /// <returns></returns>
            public List<NotificationModel> List_PendingNotifications()
            {
                return Database.Notification
                    .Include(np => np.NotificationPriorityLevel)
                    .Where(n => n.AcknowledgedDate == null)
                    .ToList();
            }

            /// <summary>
            /// Return all medical records for a particular fish
            /// </summary>
            /// <param name="fishID">ID of the userfish used to retrieve medical records</param>
            public List<UserFish_MedicalRecordModel> List_MedicalRecords(int fishID)
            {
                return Database.UserFish_MedicalRecords
                    .Where(f => f.UserFishID == fishID)
                    .ToList();
            }

            /// <summary>
            /// List all tanks owned by a user
            /// </summary>
            /// <param name="id">Id of the user to retrieve tanks</param>
            public List<AquaticTankModel> List_UserTanks(int id)
            {
                return Database.Tank
                        .Include(wt => wt.WaterType)
                        .Include(ct => ct.CreatureType)
                        .Include(tem => tem.Temporment)
                        .Include(env => env.Environment)
                        .Include(tt => tt.TankType)
                        .Include(ml => ml.MaintenanceLogs)
                        .ThenInclude(tl => tl.TemperatureRecords)
                        .Include(ml => ml.MaintenanceLogs)
                        .ThenInclude(ci => ci.CreatureInventoryRecords)
                        .Include(ml => ml.MaintenanceLogs)
                        .ThenInclude(fc => fc.FilterChangeRecords)
                        .Include(ml => ml.MaintenanceLogs)
                        .ThenInclude(fr => fr.FeedingRecords)
                        .Include(ml => ml.MaintenanceLogs)
                        .ThenInclude(wc => wc.WaterChangeRecords)
                        .Where(t => t.OwnerID == id)
                        .ToList();
            }

            /// <summary>
            /// List all purchases from the database
            /// </summary>
            public List<PurchaseModel> List_Purchases()
            {
                return Database.Purchases
                    .Include(pc => pc.PurchaseCategory)
                    .ThenInclude(ct => ct.PurchaseCategoryType)
                    .Include(pct => pct.PurchaseCategoryType)
                    .Include(st => st.Store)
                    .ToList();
            }

            /// <summary>
            /// List all purchase invoices from the database
            /// </summary>
            public List<PurchaseInvoiceModel> List_PurchaseInvoices()
            {
                return Database.PurchaseInvoices
                    .Include(o => o.Owner)
                    .Include(p => p.Purchases)
                    .Include(s => s.Store)
                    .ToList();
            }

            /// <summary>
            /// List all purchase invoices from the database for a particular user
            /// </summary>
            public List<PurchaseInvoiceModel> List_PurchaseInvoices(int id)
            {
                return Database.PurchaseInvoices
                    .Include(o => o.Owner)
                    .Include(p => p.Purchases)
                    .Include(s => s.Store)
                    .Where(o => o.OwnerID == id)
                    .ToList();
            }

            /// <summary>
            /// List all purchase categories
            /// </summary>
            public List<PurchaseCategoryModel> List_PurchaseCategories()
            {
                return Database.PurchaseCategory
                    .ToList();
            }

            /// <summary>
            /// List all purchase category types
            /// </summary>
            public List<PurchaseCategoryTypeModel> List_PurchaseCategoryTypes()
            {
                return Database.PurchaseCategoryTypes
                    .ToList();
            }

            /// <summary>
            /// List all fish types
            /// </summary>
            public List<FishTypeModel> List_FishTypes()
            {
                return Database.FishTypes
                    .ToList();
            }

            /// <summary>
            /// List all website users
            /// </summary>
            public List<WebsiteUser> List_Users()
            {
                return Database.AspNetUsers
                    .ToList();
            }

            /// <summary>
            /// List all stores in the database
            /// </summary>
            public List<StoreModel> List_Stores()
            {
                return Database.Stores
                    .ToList();
            }

            /// <summary>
            /// List all tank types in the database
            /// </summary>
            public List<TankTypeModel> List_TankTypes()
            {
                return Database.TankType
                    .ToList();
            }

            /// <summary>
            /// List all the types of maintenance logs in the database
            /// </summary>
            public List<MaintenanceLogType> List_MaintenanceLogTypes()
            {
                return Database.MaintenanceLogTypes
                    .ToList();
            }

            /// <summary>
            /// List all the types of water in the database
            /// </summary>
            public List<WaterTypeModel> List_WaterTypes()
            {
                return Database.WaterType
                    .ToList();
            }

            /// <summary>
            /// List all the fish temporment levels in the database
            /// </summary>
            public List<TempormentModel> List_TempormentLevels()
            {
                return Database.Temporment
                    .ToList();
            }

            /// <summary>
            /// List all the tank environment types in the database
            /// </summary>
            public List<EnvironmentModel> List_Environments()
            {
                return Database.Environment
                    .ToList();
            }

            /// <summary>
            /// List all the types of creature in the database
            /// </summary>
            public List<CreatureTypeModel> List_CreatureTypes()
            {
                return Database.CreatureType
                    .ToList();
            }

            /// <summary>
            /// List all fish that are new purchases and pending assignment
            /// </summary>
            public List<UserFishModel> List_NewUnassignedFishPurchases()
            {
                return Database.UserFish
                    .Where(f => f.TankID == null && f.FishStatusID == 7)
                    .ToList();
            }

            /// <summary>
            /// List all of the UserFish records, or just for a particular user
            /// </summary>
            /// <param name="UserID">ID of the user to filter on</param>
            public List<UserFishModel> List_UserFish(int? UserID)
            {
                List<UserFishModel> _final = new List<UserFishModel>();

                if (UserID.HasValue)
                {
                    _final = Database.UserFish
                        .Include(p => p.Purchase)
                        .Include(t => t.Tank)
                        .Include(ft => ft.FishType)
                        .ThenInclude(t => t.Temporment)
                        .Include(t => t.FishTemporment)
                        .Include(pf => pf.ParentFish)
                        .Include(cf => cf.ChildrenFish)
                        .Include(fs => fs.FishStatus)
                        .Include(mr => mr.MedicalRecords)
                        .ThenInclude(mrt => mrt.MedicalRecordType)
                        .Where(u => u.Purchase.OwnerID == UserID)
                        .ToList();
                }
                else
                {
                    _final = Database.UserFish
                        .Include(p => p.Purchase)
                        .Include(t => t.Tank)
                        .Include(ft => ft.FishType)
                        .Include(pf => pf.ParentFish)
                        .Include(cf => cf.ChildrenFish)
                        .Include(fs => fs.FishStatus)
                        .ToList();
                }

                return _final;
            }

            /// <summary>
            /// List the types of medical records in the database
            /// </summary>
            public List<MedicalRecordTypeModel> List_MedicalRecordTypes()
            {
                return Database.MedicalRecordTypes
                    .ToList();
            }

            /// <summary>
            /// List the maintenance logs for a particular tank
            /// </summary>
            /// <param name="tankID">ID of the tank to filter on</param>
            public List<MaintenanceLogModel> List_UserTankMaintenanceLogs(int tankID)
            {
                return Database.MaintenanceLogs
                    .Include(tl => tl.TemperatureRecords)
                    .Include(ci => ci.CreatureInventoryRecords)
                    .Include(fc => fc.FilterChangeRecords)
                    .Include(fr => fr.FeedingRecords)
                    .Include(wc => wc.WaterChangeRecords)
                    .Where(l => l.TankID == tankID)
                    .ToList();
            }

        #endregion

        #region Section: Find Factory Methods

            /// <summary>
            /// Return details for a specific plant
            /// </summary>
            /// <param name="plantID">ID of the plant to filter</param>
            public UserPlantModel Find_UserPlantDetails(int plantID)
            {
                return Database.UserPlants
                    .Where(p => p.UserPlantID == plantID)
                    .FirstOrDefault();
            }

            /// <summary>
            /// Find the details for a particular tank with loaded dependents
            /// </summary>
            /// <param name="id">Id of the tank for retrieving details</param>
            public AquaticTankModel Find_TankDetails(int id)
            {
                return Database.Tank
                        .Include(wt => wt.WaterType)
                        .Include(ct => ct.CreatureType)
                        .Include(tem => tem.Temporment)
                        .Include(env => env.Environment)
                        .Include(tt => tt.TankType)
                        .Include(pur => pur.Purchase)
                            .ThenInclude(st => st.Store)
                        .Include(fp => fp.UserFish)
                            .ThenInclude(ft => ft.FishType)
                        .Include(sup => sup.Supplies)
                            .ThenInclude(p => p.Purchase)
                        .Include(eq => eq.Equipment)
                        .Include(o => o.Owner)
                        .Include(n => n.Notes)
                        .Include(m => m.MaintenanceLogs)
                        .ThenInclude(tl => tl.TemperatureRecords)
                        .Include(ml => ml.MaintenanceLogs)
                        .ThenInclude(ci => ci.CreatureInventoryRecords)
                        .Include(ml => ml.MaintenanceLogs)
                        .ThenInclude(fc => fc.FilterChangeRecords)
                        .Include(ml => ml.MaintenanceLogs)
                        .ThenInclude(fr => fr.FeedingRecords)
                        .Include(ml => ml.MaintenanceLogs)
                        .ThenInclude(wc => wc.WaterChangeRecords)
                        .Include(ci => ci.InventoryRecords)
                        .Where(t => t.TankID == id)
                        .FirstOrDefault();
            }

            /// <summary>
            /// Find the details for a particular fish
            /// </summary>
            /// <param name="id">ID of the fish for filtering</param>
            public UserFishModel Find_FishDetails(int id)
            {
                return Database.UserFish
                    .Where(f => f.UserFishID == id)
                    .FirstOrDefault();
            }

            /// <summary>
            /// List details for a particular user
            /// </summary>
            /// <param name="ID">ID of the user used to retrieve the details</param>
            public WebsiteUser Find_UserDetails(int ID)
            {
                return Database.AspNetUsers
                    .Where(u => u.UserId == ID)
                    .FirstOrDefault();
            }

        #endregion

        #region Section: Add Factory Methods

            /// <summary>
            /// Add a Medical Record entry to a fish with a data model
            /// </summary>
            /// <param name="_modelData">Data model for adding a medical record</param>
            /// <returns>return true if successful</returns>
            public bool Add_MedicalRecord(UserFish_MedicalRecordModel _modelData)
            {
                try
                {
                    Database.UserFish_MedicalRecords.Add(_modelData);
                    Database.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw new Exception(ex.Message);
                    }
                    else
                    {
                        throw new Exception(ex.InnerException.Message);
                    }
                }
            }

            /// <summary>
            /// Add a notification to the database using a data model
            /// </summary>
            /// <param name="_modelData">Data model to user when adding the notification</param>
            /// <returns>returns true if successful</returns>
            public bool Add_Notification(NotificationModel _modelData)
            {
                bool isSuccessful = false;

                try
                {
                    var finalModel = new NotificationModel
                    {
                        AcknowledgedDate = _modelData.AcknowledgedDate,
                        Message = _modelData.Message,
                        MitigatedDate = _modelData.MitigatedDate,
                        NotificationID = _modelData.NotificationID,
                        NotificationPriorityLevelID = _modelData.NotificationPriorityLevelID,
                        TankID = _modelData.TankID,
                        TriggeredDate = _modelData.TriggeredDate,
                        WebsiteUserID = _modelData.WebsiteUserID
                    };

                    Database.Notification.Add(finalModel);
                    Database.SaveChanges();

                    isSuccessful = true;
                }
                catch (Exception)
                {
                    isSuccessful = false;
                }

                return isSuccessful;
            }

            /// <summary>
            /// Add a new tank to the database
            /// </summary>
            /// <param name="_dataModel">Data Model representing the tank to add</param>
            public AquaticTankModel Add_Tank(AquaticTankModel _dataModel)
            {
                PurchaseModel purchaseModel = List_Purchases().Where(p => p.PurchaseID == _dataModel.PurchaseID).FirstOrDefault();

                Database.Tank.Add(_dataModel);
                Database.SaveChanges();

                purchaseModel.TankID = _dataModel.TankID;

                Database.Purchases.Update(purchaseModel);
                Database.SaveChanges();

                return _dataModel;
            }

            /// <summary>
            /// Add a list of fish ids to a tank
            /// </summary>
            /// <param name="_newIds">List of fish Ids to add</param>
            /// <param name="tankID">Tank to add fish to</param>
            public void Add_TankFish(IEnumerable<int> _newIds, int tankID)
            {
                AquaticTankModel tank = Database.Tank.Where(t => t.TankID == tankID).First();

                List<UserFishModel> newFish = new List<UserFishModel>();

                foreach (int id in _newIds)
                {
                    var fish = Find_FishDetails(id);

                    fish.TankID = tankID;

                    Database.UserFish.Update(fish);
                }

                Database.SaveChanges();

            }

            /// <summary>
            /// Add a new purchase to the database
            /// </summary>
            /// <param name="_dataModel">Data model representing the purchase to add</param>
            public PurchaseModel Add_Purchase(PurchaseModel _dataModel)
            {
                Database.Purchases.Add(_dataModel);
                Database.SaveChanges();

                return _dataModel;
            }

            /// <summary>
            /// Add a new purchase invoice to the database
            /// </summary>
            /// <param name="_dataModel">Data model representing the purchase invoice to add</param>
            public async Task<PurchaseInvoiceModel> Add_PurchaseInvoice(PurchaseInvoiceModel _dataModel)
            {
                Database.PurchaseInvoices.Add(_dataModel);

                await Database.SaveChangesAsync();

                return _dataModel;
            }

            /// <summary>
            /// Add a User Fish record
            /// </summary>
            /// <param name="_dataModel">data model representing UserFish record to be added</param>
            public async Task<UserFishModel> Add_UserFish(UserFishModel _dataModel)
            {
                Database.UserFish.Add(_dataModel);
                await Database.SaveChangesAsync();

                return _dataModel;
            }

        #endregion

        #region Section: Update Factory Methods

            /// <summary>
            /// Update the details for a tank, or add a new tank if model TankID doesnt exist
            /// </summary>
            /// <param name="_dataModel">Data model to update/add to the database</param>
            public AquaticTankModel Update_TankDetails(AquaticTankModel _dataModel)
            {
                var listEntry = Database.Tank.Where(t => t.TankID == _dataModel.TankID).FirstOrDefault();

                if (listEntry != null)
                {
                    listEntry.Name = _dataModel.Name;
                    listEntry.IsActive = _dataModel.IsActive;
                    listEntry.Owner = _dataModel.Owner;
                    listEntry.TankTypeID = _dataModel.TankTypeID;
                    listEntry.TempormentID = _dataModel.TempormentID;
                    listEntry.WaterTypeID = _dataModel.WaterTypeID;
                    listEntry.CreatureTypeID = _dataModel.CreatureTypeID;
                    listEntry.EnvironmentID = _dataModel.EnvironmentID;

                    Database.SaveChanges();

                    return _dataModel;
                }
                else
                {
                    return Add_Tank(_dataModel);
                }
            }

            /// <summary>
            /// Update the details for a Purchase
            /// </summary>
            /// <param name="_dataModel">Data model containing the purchase details</param>
            public PurchaseModel Update_Purchase(PurchaseModel _dataModel)
            {
                var listEntry = Database.Purchases.Where(p => p.PurchaseID == _dataModel.PurchaseID).FirstOrDefault();

                if (listEntry != null)
                {
                    listEntry.Description = _dataModel.Description;
                    listEntry.Quantity = _dataModel.Quantity;
                    listEntry.Cost = _dataModel.Cost;
                    listEntry.OwnerID = _dataModel.OwnerID;
                    listEntry.CreatureID = _dataModel.CreatureID;
                    listEntry.PlantID = _dataModel.PlantID;
                    listEntry.DecorationID = _dataModel.DecorationID;
                    listEntry.SupplyID = _dataModel.SupplyID;
                    listEntry.StoreID = _dataModel.StoreID;
                    listEntry.TankID = _dataModel.TankID;
                    listEntry.PurchaseCategoryID = _dataModel.PurchaseCategoryID;
                    listEntry.Date = _dataModel.Date;

                    Database.SaveChanges();

                    return _dataModel;
                }
                else
                {
                    return Add_Purchase(_dataModel);
                }
            }

        #endregion

        #region Section: Remove Factory Methods

            /// <summary>
            /// Unassigns a fish from a tank, making that fish homeless
            /// </summary>
            /// <param name="fishID">ID of the fish to remove</param>
            public async Task Remove_TankFish(int fishID)
            {
                var fishModel = Database.UserFish
                    .Where(f => f.UserFishID == fishID)
                    .First();

                fishModel.TankID = null;

                Database.UserFish.Update(fishModel);
                await Database.SaveChangesAsync();
            }

            /// <summary>
            /// Remove a Tank from the database.
            /// </summary>
            /// <param name="id">ID of the tank to remove</param>
            public async Task Remove_UserTank(int id)
            {
                var tankModel = Database.Tank.Where(t => t.TankID == id).First();

                var purchase = Database.Purchases.Where(t => t.TankID == id).FirstOrDefault();
                purchase.TankID = null;

                Database.Purchases.Update(purchase);
                Database.Tank.Remove(tankModel);

                await Database.SaveChangesAsync();
            }

            /// <summary>
            /// Remove a user fish record
            /// </summary>
            /// <param name="_dataModel">data model representing UserFish record to remove</param>
            public async Task Remove_UserFish(UserFishModel _dataModel)
            {
                Database.UserFish.Remove(_dataModel);

                await Database.SaveChangesAsync();
            }

        #endregion

    }
}
