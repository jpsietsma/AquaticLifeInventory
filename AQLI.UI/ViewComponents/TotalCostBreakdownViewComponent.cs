using AQLI.Data.Models.ViewComponentModels;
using AQLI.DataServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQLI.UI.ViewComponents
{
    public class TotalCostBreakdownViewComponent : ViewComponent
    {
        private readonly DataFactory DataSource;

        public TotalCostBreakdownViewComponent(DataFactory _datasource)
        {
            DataSource = _datasource;
        }

        public async Task<IViewComponentResult> InvokeAsync(string GraphTitle, string GraphSubtitle)
        {
            TotalCostBreakdownVCModel viewData = new TotalCostBreakdownVCModel
            {
                GraphTitle = GraphTitle,
                GraphTitleSubtext = GraphSubtitle,
                SeriesPieData = DataSource.List_Purchases()
            };

            return View(viewData);
        }
    }
}
