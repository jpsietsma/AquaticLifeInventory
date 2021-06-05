using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AQLI.Data.Models
{
    public class NotificationModel
    {
        [Key]
        public int NotificationID { get; set; }
        public string Message { get; set; }

        public DateTime TriggeredDate { get; set; }
        public DateTime? MitigatedDate { get; set; }
        public DateTime? AcknowledgedDate { get; set; }

        public int NotificationPriorityLevelID { get; set; }
        public NotificationPriorityLevelModel NotificationPriorityLevel { get; set; }

        public int WebsiteUserID { get; set; }
        public WebsiteUser WebsiteUser { get; set; }

        public int TankID { get; set; }
        public AquaticTankModel Tank { get; set; }

        public NotificationModel()
        {

        }
    }
}
