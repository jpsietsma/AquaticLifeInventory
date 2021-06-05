using System;
using System.Collections.Generic;
using System.Text;

namespace AQLI.Data.Models.ViewComponentModels
{
    public class DatabaseRecordInfoVCModel
    {
        public int? PKID { get; set; }
        public DateTime? Added { get; set; }
        public string AddedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
