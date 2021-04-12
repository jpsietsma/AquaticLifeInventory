using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class NotificationPriorityLevelModel
    {
        [Key]
        public int NotificationPriorityLevelID { get; set; }
        public string NotificationPriorityLevelName { get; set; }
        public int NotificationPriority { get; set; }
    }
}
