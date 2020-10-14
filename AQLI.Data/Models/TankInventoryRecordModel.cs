using System;
using System.Collections.Generic;
using System.Text;

namespace AQLI.Data.Models
{
    public class TankInventoryRecordModel
    {
        public int Id { get; set; }
        public int TankId { get; set; }
        public int PerformedBy { get; set; }
        public DateTime Date { get; set; }
        public List<FishCreatureModel> CreatureInventory { get; set; }
    }
}
