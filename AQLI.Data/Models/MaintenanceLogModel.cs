using AQLI.Data.Models.ListModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class MaintenanceLogModel
    {
        [Key]
        public int MaintenanceLogID { get; set; }

        public int MaintenanceLogTypeID { get; set; }
        public MaintenanceLogType MaintenanceLogType { get; set; }

        public DateTime LogDate { get; set; }

        public int TankID { get; set; }
        public AquaticTankModel Tank { get; set; }

        public List<TankTemperatureRecordModel> TemperatureRecords { get; set; }
        public List<TankCreatureInventoryRecordModel> CreatureInventoryRecords { get; set; }
        public List<TankWaterChangeRecordModel> WaterChangeRecords { get; set; }
        public List<TankFilterChangeRecordModel> FilterChangeRecords { get; set; }
        public List<TankFeedingRecordModel> FeedingRecords { get; set; }
    }
}
