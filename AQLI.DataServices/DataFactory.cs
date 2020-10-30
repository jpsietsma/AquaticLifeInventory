using AQLI.Data.Models;
using AQLI.DataServices.context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace AQLI.DataServices
{
    public class DataFactory
    {
        private readonly DatabaseContext Database;

        private List<WebsiteUser> _AllUsers;
        private List<FishCreatureModel> _AllFish;


        public DataFactory(DatabaseContext _context)
        {
            Database = _context;
            _AllUsers = new List<WebsiteUser>();
            _AllFish = new List<FishCreatureModel>();            

        }       

        public List<WebsiteUser> List_WebsiteUsers()
        {
            return _AllUsers;
        }

        public List<AquaticTankModel> List_Tanks()
        {            
            return Database.Tank.ToList();               
        }

        public List<FishCreatureModel> List_Fish()
        {
            return _AllFish;
        }

        public List<TankInventoryRecordModel> List_InventoryRecords()
        {
            return new List<TankInventoryRecordModel>();
        }

        public List<MedicalRecord> List_MedicalRecords()
        {
            return new List<MedicalRecord>();
        }

        public AquaticTankModel Find_TankDetails(int id)
        {
            return List_Tanks().Where(t => t.TankID == id).FirstOrDefault();
        }

        public List<AquaticTankModel> List_UserTanks(int id)
        {
            return List_Tanks().Where(t => t.Owner.ID == id).ToList();
        }

        public List<TankTypeModel> List_TankTypes()
        {
            return Database.TankType.ToList();
        }

        public List<WaterTypeModel> List_WaterTypes()
        {
            return Database.WaterType.ToList();
        }

        public List<TempormentModel> List_AgressionLevels()
        {
            return Database.Temporment.ToList();
        }

        public WebsiteUser Find_UserDetails(int id)
        {
            WebsiteUser FinalUserDetails = List_WebsiteUsers().Where(u => u.ID == id).FirstOrDefault();
            
            return FinalUserDetails;
        }

        public AquaticTankModel Add_Tank(AquaticTankModel _dataModel)
        {
            Database.Tank.Add(_dataModel);
            Database.SaveChanges();

            return _dataModel;
        }

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

        public void Remove_UserTank(int id)
        {
            List_Tanks().Remove(List_Tanks().Where(t => t.TankID == id).First());            
        }

    }
}
