using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AQLI.Data.Models
{
    public class PurchaseCategoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseCategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryColor { get; set; }

        public List<PurchaseModel> Purchases { get; set; }
    }
}
