using AQLI.Data.Models.ListModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AQLI.Data.Models
{
    public class AquaticTankModel
    {
        [Key]
        public int TankID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Capacity { get; set; }

        public DateTime? Added { get; set; }
        public int? AddedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }

        public bool IsActive { get; set; }
        public bool IsMaintenance { get; set; }
        public bool IsQuarantine { get; set; }

        public int TankTypeID { get; set; }
        public TankTypeModel TankType { get; set; }

        public int WaterTypeID { get; set; }
        public WaterTypeModel WaterType { get; set; }

        public int TempormentID { get; set; }
        public TempormentModel Temporment { get; set; }

        public int CreatureTypeID { get; set; }
        public CreatureTypeModel CreatureType { get; set; }

        public int EnvironmentID { get; set; }
        public EnvironmentModel Environment { get; set; }

        public int PurchaseID { get; set; }
        public PurchaseModel Purchase { get; set; }

        public int OwnerID { get; set; }
        public WebsiteUser Owner { get; set; }

        public List<MaintenanceLogModel> MaintenanceLogs { get; set; }

        public List<UserFishModel> UserFish { get; set; }
        public List<TankSupplyModel> Supplies { get; set; }
        public List<TankEquipmentModel> Equipment { get; set; }
        public List<TankNoteModel> Notes { get; set; }
        public List<TankCreatureInventoryRecordModel> InventoryRecords { get; set; }        

        public AquaticTankModel(WebsiteUser _tankOwner = null)
        {
            InventoryRecords = new List<TankCreatureInventoryRecordModel>();            

            if (_tankOwner != null)
            {
                Owner = _tankOwner;                          
            }
        }

        public AquaticTankModel()
        {
            InventoryRecords = new List<TankCreatureInventoryRecordModel>();
        }               

    }

}
