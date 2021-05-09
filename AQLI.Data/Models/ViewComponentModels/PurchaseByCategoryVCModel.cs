using System;
using System.Collections.Generic;
using System.Text;

namespace AQLI.Data.Models.ViewComponentModels
{
    public class PurchaseByCategoryVCModel : BaseVCModel
    {
        public string DataUser { get; set; }

        public List<PurchaseModel> AllPurchases { get; set; }

        public List<int> FishPurchases { get; set; }
        public List<int> EquipmentPurchases { get; set; }
        public List<int> PlantPurchases { get; set; }
        public List<int> FoodPurchases { get; set; }
        public List<int> TankPurchases { get; set; }
        public List<int> SupplyPurchases { get; set; }
    }
}
