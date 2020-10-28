using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class CreatureTypeModel
    {
        [Key]
        public int CreatureTypeID { get; set; }
        public string CreatureTypeName { get; set; }
    }
}
