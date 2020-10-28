using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class AquaticTankModel : ITankModel
    {
        [Key]
        public int TankID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Capacity { get; set; }
        public bool IsActive { get; set; }
        public TankTypeModel TankType { get; set; }
        public EnvironmentModel Environment { get; set; }
        public WaterTypeModel WaterType { get; set; }
        public TempormentModel Temporment { get; set; }
        public CreatureTypeModel CreatureType { get; set; }
        public WebsiteUser Owner { get; set; }
        public DateTime Added { get; set; }
        public List<FishCreatureModel> FishPopulation { get; set; }
        public List<TankInventoryRecordModel> InventoryRecords { get; set; }

        public AquaticTankModel(WebsiteUser _tankOwner = null)
        {
            InventoryRecords = new List<TankInventoryRecordModel>();
            Environment = new EnvironmentModel { EnvironmentID = 1, EnvironmentName = "Freshwater Community" };
            CreatureType = new CreatureTypeModel { CreatureTypeID = 1, CreatureTypeName = "Unknown" };
            IsActive = true;

            FishPopulation = new List<FishCreatureModel>();

            if (_tankOwner != null)
            {
                Owner = _tankOwner;                          
            }
        }

        public AquaticTankModel()
        {
            CreatureType = new CreatureTypeModel { CreatureTypeID = 1, CreatureTypeName = "Unknown" };

            InventoryRecords = new List<TankInventoryRecordModel>();
            Environment = new EnvironmentModel { EnvironmentID = 1, EnvironmentName = "Freshwater Community" };
            IsActive = true;

            FishPopulation = new List<FishCreatureModel>();

            Owner = new WebsiteUser { UserId = 1 };
        }               

    }

}
