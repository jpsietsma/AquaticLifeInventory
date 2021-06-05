using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class AnimalTypeModel
    {
        [Key]
        public int AnimalTypeID { get; set; }

        public int CareGuideID { get; set; }
        public CareGuideModel CareGuide { get; set; }
    }
}
