using AQLI.Data.Models.ListModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public class TankEquipmentModel
    {
        [Key]
        public int TankEquipmentId { get; set; }
        public string EquipmentName { get; set; }

        public int PurchaseCategoryId { get; set; }
        public PurchaseCategoryModel PurchaseCategory { get; set; }

        public int PurchaseCategoryTypeId { get; set; }
        public PurchaseCategoryTypeModel PurchaseCategoryType { get; set; }

    }
}
