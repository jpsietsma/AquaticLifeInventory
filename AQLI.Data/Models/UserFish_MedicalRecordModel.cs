using AQLI.Data.Models.ListModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class UserFish_MedicalRecordModel
    {
        [Key]
        public int UserFishMedicalRecordID { get; set; }
        public string RecordNote { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedByID { get; set; }

        public int UserFishID { get; set; }
        public UserFishModel UserFish { get; set; }

        public int MedicalRecordTypeID { get; set; }
        public MedicalRecordTypeModel MedicalRecordType { get; set; }

    }

}
