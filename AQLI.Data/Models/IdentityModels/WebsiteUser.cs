using System;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AQLI.Data.Models
{
    public class WebsiteUser : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<NotificationModel> Notifications { get; set; }
        public List<PurchaseInvoiceModel> PurchaseInvoices { get; set; }

        public WebsiteUser()
        {
            Notifications = new List<NotificationModel>();
        }

    }
}
