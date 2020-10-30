using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class WebsiteUser
    {
        [Key]
        public int OwnerID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public List<NotificationModel> Notifications { get; set; }

        public WebsiteUser()
        {
            Notifications = new List<NotificationModel>();
        }

    }
}
