using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class CreatureTypeModel
    {
        public int CreatureTypeID { get; set; }
        public string CreatureTypeName { get; set; }

        public List<AquaticTankModel> Tanks { get; set; }
    }
}
