using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class FishFamilyModel
    {
        [Key]
        public int FamilyID { get; set; }
        public string FishFamilyName { get; set; }
        
        public int WaterTypeID { get; set; }
        public WaterTypeModel WaterType { get; set; }

        public int AnimalTypeID { get; set; }
        public AnimalTypeModel AnimalType { get; set; }

    }
}
