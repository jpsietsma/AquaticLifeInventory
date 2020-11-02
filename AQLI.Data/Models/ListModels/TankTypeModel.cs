using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class TankTypeModel
    {
        [Key]
        public int TankTypeID { get; set; }
        public string TankTypeName { get; set; }

        public List<AquaticTankModel> Tanks { get; set; }
    }
}
