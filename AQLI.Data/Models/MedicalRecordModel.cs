using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class MedicalRecordModel
    {
        [Key]
        public int MedicalRecordID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int TankID { get; set; }

    }

}
