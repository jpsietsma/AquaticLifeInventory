using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models.ListModels
{
    public class PurchaseCategoryTypeModel
    {
        [Key]
        public int PurchaseCategoryTypeID { get; set; }
        public string PurchaseCategoryTypeName { get; set; }
        public bool IsLiving { get; set; }
        public bool IsConsumable { get; set; }
        public bool IsEquipment { get; set; }

    }
}
