using AQLI.Data.Models.ListModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class TankTemperatureRecordModel
    {
        [Key]
        public int TankTemperatureRecordID { get; set; }
        public int RecordedTemperature { get; set; }
        public string RecordNotes { get; set; }

        public int MaintenanceLogID { get; set; }
        public MaintenanceLogModel MaintenanceLog { get; set; }
    }
}
