using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models.ListModels
{
    public class FishTypeModel
    {
        [Key]
        public int CreatureTypeID { get; set; }
        public string TypeName { get; set; }
        public float TankPopulationEncumbrance { get; set; }
        public string FishTypeImagePath { get; set; }
        public int FishMinTankSize { get; set; }

        public int FishFamilyID { get; set; }
        public FishFamilyModel FishFamily { get; set; }

        public int FishFeederTypeID { get; set; }
        public FishFeedingTypeModel FishFeedingType { get; set; }

        public int BirthingTypeID { get; set; }
        public BirthingTypeModel BirthingType { get; set; }

        public int TempormentID { get; set; }
        public TempormentModel Temporment { get; set; }

        public int TerritorialLevelID { get; set; }
        public TerritorialLevelModel TerritorialLevel { get; set; }

        public int WaterTypeID { get; set; }
        public WaterTypeModel WaterType { get; set; }

        public int CareGuideID { get; set; }
        public CareGuideModel CareGuide { get; set; }

    }
}
