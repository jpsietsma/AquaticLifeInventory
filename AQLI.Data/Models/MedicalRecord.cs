using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public abstract class MedicalRecord
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        
    }

    public enum MedicalRecordType
    {
        ROUTINE,

    }
}
