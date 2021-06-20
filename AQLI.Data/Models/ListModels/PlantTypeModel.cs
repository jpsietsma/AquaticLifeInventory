using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQLI.Data.Models.ListModels
{
    public class PlantTypeModel
    {
        [Key]
        public int PlantTypeID { get; set; }
        public string PlantTypeName { get; set; }
        public string PlantTypeScientificName { get; set; }
        public string PlantImagePath { get; set; }

    }
}
