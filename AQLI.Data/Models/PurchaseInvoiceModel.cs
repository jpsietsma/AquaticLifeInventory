using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AQLI.Data.Models
{
    public class PurchaseInvoiceModel
    {     
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int PurchaseInvoiceID { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string InvoiceFilePath { get; set; }

        public int StoreID { get; set; }
        public StoreModel Store { get; set; }

        public int OwnerID { get; set; }
        public WebsiteUser Owner { get; set; }

        public int? TankID { get; set; }
        //public AquaticTankModel Tank { get; set; }

        public List<PurchaseModel> Purchases { get; set; }        

        public PurchaseInvoiceModel()
        {
            Purchases = new List<PurchaseModel>();
        }
    }
}
