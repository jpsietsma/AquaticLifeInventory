using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class FishFeedingTypeModel
    {
        [Key]
        public int FeedingTypeID { get; set; }
        public string FeedingType { get; set; }
    }
}
