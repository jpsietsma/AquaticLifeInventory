using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models.ListModels
{
    public class FishStatusModel
    {
        [Key]
        public int FishStatusID { get; set; }
        public string Status { get; set; }
    }
}
