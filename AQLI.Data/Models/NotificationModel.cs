using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class NotificationModel
    {
        [Key]
        public int NotificationID { get; set; }
        public string Message { get; set; }

        public int NotificationPriorityLevelID { get; set; }
        public NotificationPriorityLevelModel PriorityLevel { get; set; }

        public NotificationModel()
        {

        }
    }
}
