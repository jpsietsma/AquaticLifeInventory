using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AQLI.Data.Models
{
    public class WebsiteUser : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<NotificationModel> Notifications { get; set; }

        public WebsiteUser()
        {
            Notifications = new List<NotificationModel>();
        }

    }
}
