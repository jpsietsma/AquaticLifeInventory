using AQLI.DataServices;
using AQLI.Data.Models.ViewComponentModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQLI.UI.ViewComponents
{
    public class TotalFishTypeBreakdownViewComponent : ViewComponent
    {
        private readonly DataFactory DataSource;

        public TotalFishTypeBreakdownViewComponent(DataFactory _datasource)
        {
            DataSource = _datasource;
        }

        public async Task<IViewComponentResult> InvokeAsync(string GraphTitle, string GraphSubtitle)
        {
            TotalFishTypeBreakdownVCModel viewData = new TotalFishTypeBreakdownVCModel
            {
                GraphTitle = GraphTitle,
                GraphTitleSubtext = GraphSubtitle
            };


            return View(viewData);
        }
    }
}
