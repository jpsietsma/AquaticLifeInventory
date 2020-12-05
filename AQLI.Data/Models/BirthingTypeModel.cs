using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class BirthingTypeModel
    {
        [Key]
        public int BirthingTypeID { get; set; }
        public string BirthingType { get; set; }
        public string SpecialNotes { get; set; }
    }
}
