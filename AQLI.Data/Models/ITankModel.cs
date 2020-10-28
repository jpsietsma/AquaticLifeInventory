using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models
{
    public abstract class ITankModel
    {
        [Key]
        public int TankID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Capacity { get; set; }
        public DateTime Added { get; set; }
        public EnvironmentModel Environment { get; set; }

        public List<TankInventoryRecordModel> InventoryRecords { get; set; }
        public WebsiteUser Owner { get; set; }

    }

}
