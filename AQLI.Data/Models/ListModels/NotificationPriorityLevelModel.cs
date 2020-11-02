using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class NotificationPriorityLevelModel
    {
        [Key]
        public int PriorityLevelID { get; set; }
        public string PriorityLevelName { get; set; }

        public List<NotificationModel> Notifications { get; set; }
    }
}
