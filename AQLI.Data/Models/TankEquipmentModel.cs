using AQLI.Data.Models.ListModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class TankEquipmentModel
    {
        [Key]
        public int TankEquipmentId { get; set; }
        public string EquipmentName { get; set; }

        public int PurchaseId { get; set; }
        public PurchaseModel Purchase{ get; set; }

        public int TankID { get; set; }
        public AquaticTankModel Tank { get; set; }
    }
}
