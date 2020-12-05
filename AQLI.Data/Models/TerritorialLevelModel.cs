using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class TerritorialLevelModel
    {
        [Key]
        public int TerritorialLevelID { get; set; }
        public string TerritorialLevel { get; set; }
    }
}
