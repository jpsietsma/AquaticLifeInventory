using AQLI.Data.Models.ViewComponentModels;
using AQLI.DataServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQLI.UI.ViewComponents
{
    public class TotalPurchaseBreakdownViewComponent : ViewComponent
    {
        private readonly DataFactory DataSource;

        public TotalPurchaseBreakdownViewComponent(DataFactory _datasource)
        {
            DataSource = _datasource;
        }

        public async Task<IViewComponentResult> InvokeAsync(string GraphTitle, string GraphSubtitle)
        {
            TotalPurchaseBreakdownVCModel viewDataModel = new TotalPurchaseBreakdownVCModel
            {
                GraphTitle = GraphTitle,
                GraphTitleSubtext = GraphSubtitle,
                SeriesPieData = DataSource.List_Purchases()
            };

            return View(viewDataModel);
        }
    }
}
