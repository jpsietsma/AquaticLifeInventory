using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class TankTypeModel
    {
        [Key]
        public int TypeID { get; set; }
        public string TypeName { get; set; }
    }
}
