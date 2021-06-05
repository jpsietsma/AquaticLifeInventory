using System;
using System.Collections.Generic;
using System.Text;

namespace AQLI.Data.Models.ViewComponentModels
{
    public abstract class BaseVCModel
    {
        public string GraphTitle { get; set; }
        public string GraphTitleSubtext { get; set; }
        public string YAxisTitle { get; set; }

    }
}
