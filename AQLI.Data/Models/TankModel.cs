using System;
using System.Collections.Generic;
using System.Text;

namespace AQLI.Data.Models
{
    public class TankModel : ITankModel
    {
        public int TankId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Capacity { get; set; }
        public DateTime Added { get; set; }
        public TankEnvironment? Environment { get; set; }
        public TankSubEnvironment? SubEnvironment { get; set; }
        public List<TankInventoryRecordModel> InventoryRecords { get; set; }
        public WebsiteUser Owner { get; set; }

        public TankModel()
        {

        }
    }
}
