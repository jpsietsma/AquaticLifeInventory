using AQLI.Data.Models.ListModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class TankFilterChangeRecordModel
    {
        [Key]
        public int TankFilterChangeRecordID { get; set; }

        public int? SupplyID { get; set; }

        public string RecordNotes { get; set; }

        public int MaintenanceLogID { get; set; }
        public MaintenanceLogModel MaintenanceLog { get; set; }
    }
}
