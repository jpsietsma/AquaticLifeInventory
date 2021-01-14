using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AQLI.Data.Models
{
    public class PurchaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseID { get; set; }
        public string Date { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal Cost { get; set; }        
        public string Description { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal ExtCost { get; set; }
        
        public int OwnerID { get; set; }
        //public WebsiteUser Owner { get; set; }

        public int StoreID { get; set; }
        public StoreModel Store { get; set; }

        public int PurchaseCategoryID { get; set; }
        public PurchaseCategoryModel PurchaseCategory { get; set; }

        public int? CreatureID { get; set; }
        //public CreatureModel Creature { get; set; }

        public int? TankID { get; set; }
        public AquaticTankModel Tank { get; set; }

        public int? PlantID { get; set; }
        //public LivePlantModel Plant { get; set; }

        public int? SupplyID { get; set; }
        //public SupplyModel Supply { get; set; }

        public int? DecorationID { get; set; }
        //public DecorationModel Decoration { get; set; }

        public int? InvoiceID { get; set; }
        public PurchaseInvoiceModel Invoice { get; set; }

    }
}