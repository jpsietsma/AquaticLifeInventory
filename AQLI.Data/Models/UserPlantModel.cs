using AQLI.Data.Models.ListModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class UserPlantModel
    {
        [Key]
        public int UserPlantID { get; set; }

        public PlantTypeModel PlantType { get; set; }
        public int PlantTypeID { get; set; }

        public DateTime Added { get; set; }
        public int AddedBy { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public int TankID { get; set; }
        public string Description { get; set; }
        public int OwnerID { get; set; }
    }
}
