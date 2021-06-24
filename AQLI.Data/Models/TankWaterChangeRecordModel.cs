using AQLI.Data.Models.ListModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class TankWaterChangeRecordModel
    {
        [Key]
        public int TankWaterChangeRecordID { get; set; }
        public int WaterChangePercentage { get; set; }
        public string RecordNotes { get; set; }

        public int MaintenanceLogID { get; set; }
        public MaintenanceLogModel MaintenanceLog { get; set; }

        public int RecordedBy { get; set; }
    }
}
