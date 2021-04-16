using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class TankSupplyModel
    {
        [Key]
        public int TankSupplyID { get; set; }
        public string SupplyName { get; set; }
        public bool IsFood { get; set; }
        public bool IsConsumableSupply { get; set; }
        public bool IsMedication { get; set; }
        public bool IsDepleted { get; set; }
        public decimal ApplicationsRemaining { get; set; }
        public decimal TotalUses { get; set; }
        public decimal UsagePerApplication { get; set; }

        public int TankID { get; set; }
        public AquaticTankModel Tank { get; set; }

    }
}
