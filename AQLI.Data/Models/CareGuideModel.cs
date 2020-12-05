using AQLI.Data.Models.ListModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class CareGuideModel
    {
        [Key]
        public int CareGuideID { get; set; }
        public string DocumentPath { get; set; }

        public int AnimalTypeID { get; set; }
        public AnimalTypeModel AninmalType { get; set; }

        public FishTypeModel FishType { get; set; }
    }
}
