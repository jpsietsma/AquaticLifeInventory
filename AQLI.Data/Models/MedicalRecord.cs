using System;
using System.Collections.Generic;
using System.Text;

namespace AQLI.Data.Models
{
    public abstract class MedicalRecord
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        
    }

    public enum MedicalRecordType
    {
        ROUTINE,

    }
}
