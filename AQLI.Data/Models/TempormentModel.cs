using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class TempormentModel
    {
        public int TempormentID { get; set; }
        public string TempormentName { get; set; }

        public List<AquaticTankModel> Tanks { get; set; }
    }
}
