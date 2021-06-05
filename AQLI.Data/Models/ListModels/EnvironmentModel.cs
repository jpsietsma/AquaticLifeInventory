using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class EnvironmentModel
    {
        [Key]
        public int EnvironmentID { get; set; }
        public string EnvironmentName { get; set; }

        public List<AquaticTankModel> Tanks { get; set; }
    }
}
