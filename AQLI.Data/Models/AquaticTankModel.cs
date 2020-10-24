using System;
using System.Collections.Generic;
using System.Text;

namespace AQLI.Data.Models
{
    public class AquaticTankModel : ITankModel
    {
        public int TankId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Capacity { get; set; }
        public bool IsActive { get; set; }
        public TankType TankType { get; set; }
        public TankEnvironment? Environment { get; set; }
        public TankSubEnvironment? SubEnvironment { get; set; }
        public AquariumWaterType? WaterType { get; set; }
        public AquariumTemporment? Temporment { get; set; }
        public AquaticTankCreatureType CreatureType { get; set; }
        public WebsiteUser Owner { get; set; }
        public DateTime Added { get; set; }
        public List<FishCreatureModel> FishPopulation { get; set; }
        public List<TankInventoryRecordModel> InventoryRecords { get; set; }

        public AquaticTankModel(AquaticTankCreatureType _tankCreatureType = AquaticTankCreatureType.Other, WebsiteUser _tankOwner = null)
        {
            InventoryRecords = new List<TankInventoryRecordModel>();
            Environment = TankEnvironment.Aquatic;
            CreatureType = _tankCreatureType;
            IsActive = true;

            FishPopulation = new List<FishCreatureModel>();

            if (_tankOwner != null)
            {
                Owner = _tankOwner;                          
            }
        }

        public AquaticTankModel()
        {
             CreatureType = AquaticTankCreatureType.Other;

            InventoryRecords = new List<TankInventoryRecordModel>();
            Environment = TankEnvironment.Aquatic;
            IsActive = true;

            FishPopulation = new List<FishCreatureModel>();

            Owner = new WebsiteUser { UserId = 1 };
        }               

    }

}
