using System;
using System.Collections.Generic;
using System.Text;

namespace AQLI.Data.Models
{
    public class UserNotification
    {
        public int NotificationId { get; set; }
        public int Owner { get; set; }
        public bool Read { get; set; }
        public string Message { get; set; }
        public NotificationPriorityTypes PriorityType { get; set; }
        public int PriorityLevel { get; set; }

    }

    public enum NotificationPriorityTypes
    {
        INFO,
        NOTICE,
        WARNING,
        DANGER
    }
}
