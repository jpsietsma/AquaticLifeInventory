using System;
using System.Collections.Generic;
using System.Text;

namespace AQLI.Data.Models
{
    public class DryTankModel : ITankModel
    {
        public int TankId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Capacity { get; set; }
        public TankEnvironment? Environment { get; set; }
        public TankSubEnvironment? SubEnvironment { get; set; }
        public DryTankCreatureType CreatureType { get; set; }
        public WebsiteUser Owner { get; set; }
        public DateTime Added { get; set; }
        public List<TankInventoryRecordModel> InventoryRecords { get; set; }

        public DryTankModel(TankEnvironment _environment = TankEnvironment.Dry, DryTankCreatureType _creatureType = DryTankCreatureType.Unknown, WebsiteUser? _tankOwner = null)
        {
            if (_tankOwner != null)
            {
                Owner = _tankOwner;
                Environment = _environment;
                CreatureType = _creatureType;

                InventoryRecords = new List<TankInventoryRecordModel>();
            }
        }

    }
}
