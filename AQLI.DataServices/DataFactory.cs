using AQLI.Data.Models;
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
                    .Where(t => t.TankID == id)
                    .FirstOrDefault();
        }

        /// <summary>
        /// List tanks with loaded dependent entities
        /// </summary>
        public List<AquaticTankModel> List_Tanks()
        {            
            return Database.Tank
                    .Include(wt => wt.WaterType)
                    .Include(ct => ct.CreatureType)
                    .Include(tem => tem.Temporment)
                    .Include(env => env.Environment)
                    .Include(tt => tt.TankType)
                    .ToList();
        }

        /// <summary>
        /// Return all system medical records
        /// </summary>
        public List<MedicalRecordModel> List_MedicalRecords()
        {
            return Database.MedicalRecord
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
                    .Where(t => t.OwnerID == id)
                    .ToList();
        }

        /// <summary>
        /// List all purchases from the database
        /// </summary>
        /// <returns>List<PurchaseModel></returns>
        public List<PurchaseModel> List_Purchases()
        {
            return Database.Purchases
                .Include(pc => pc.PurchaseCategory)
                .Include(st => st.Store)
                .ToList();
        }

        /// <summary>
        /// List all purchase invoices from the database
        /// </summary>
        /// <returns>List<PurchaseInvoiceModel></returns>
        public List<PurchaseInvoiceModel> List_PurchaseInvoices()
        {
            return Database.PurchaseInvoices
                .Include(o => o.Owner)
                .Include(p => p.Purchases)
                .Include(s => s.Store)
                //.Include(t => t.Tank)
                .ToList();
        }

        /// <summary>
        /// List all purchase categories
        /// </summary>
        /// <returns>List<PurchaseCategoryModel></returns>
        public List<PurchaseCategoryModel> List_PurchaseCategories()
        {
            return Database.PurchaseCategory
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
        /// List all stores
        /// </summary>
        public List<StoreModel> List_Stores()
        {
            return Database.Stores
                .ToList();
        }

        /// <summary>
        /// List types of tanks
        /// </summary>
        public List<TankTypeModel> List_TankTypes()
        {
            return Database.TankType
                .ToList();
        }

        /// <summary>
        /// List types of water
        /// </summary>
        public List<WaterTypeModel> List_WaterTypes()
        {
            return Database.WaterType
                .ToList();
        }

        /// <summary>
        /// List temporment levels of fish
        /// </summary>
        public List<TempormentModel> List_TempormentLevels()
        {
            return Database.Temporment
                .ToList();
        }

        /// <summary>
        /// List types of tank environments
        /// </summary>
        public List<EnvironmentModel> List_Environments()
        {
            return Database.Environment
                .ToList();
        }

        /// <summary>
        /// List creature types
        /// </summary>
        public List<CreatureTypeModel> List_CreatureTypes()
        {
            return Database.CreatureType
                .ToList();
        }

        /// <summary>
        /// Add a new tank to the database
        /// </summary>
        /// <param name="_dataModel">Data Model representing the tank to add</param>
        public AquaticTankModel Add_Tank(AquaticTankModel _dataModel)
        {
            Database.Tank.Add(_dataModel);
            Database.SaveChanges();

            return _dataModel;
        }

        /// <summary>
        /// Add a new purchase to the database
        /// </summary>
        /// <param name="_dataModel">Data model representing the purchase to add</param>
        /// <returns>PurchaseModel</returns>
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

        /// <summary>
        /// Remove a Tank from the database.
        /// </summary>
        /// <param name="id">ID of the tank to remove</param>
        public void Remove_UserTank(int id)
        {
            var model = Database.Tank.Where(t => t.TankID == id).First();
            
            Database.Tank.Remove(model);
            Database.SaveChanges();
        }

    }
}
