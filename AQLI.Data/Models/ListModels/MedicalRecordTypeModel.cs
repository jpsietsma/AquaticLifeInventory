using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models.ListModels
{
    public class MedicalRecordTypeModel
    {
        [Key]
        public int MedicalRecordTypeID { get; set; }
        public string RecordType { get; set; }
    }
}
