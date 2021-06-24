using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class UserTankInventoryRecordModel
    {
        [Key]
        public int UserTankInventoryRecordID { get; set; }
        public int RecordedBy { get; set; }
    }
}
