using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AQLI.Data.Models
{
    public class WaterTypeModel
    {
        [Key]
        public int WaterTypeID { get; set; }
        public string WaterTypeName { get; set; }

        public List<AquaticTankModel> Tanks { get; set; }
    }
}
