using AQLI.Data.Models;
using AQLI.Data.Models.ViewComponentModels;
using AQLI.DataServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQLI.UI.ViewComponents
{
    public class PurchasesByCategoryViewComponent : ViewComponent
    {
        private readonly DataFactory DataSource;

        public PurchasesByCategoryViewComponent(DataFactory _dataSource)
        {
            DataSource = _dataSource;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<PurchaseModel> PurchaseHistory, string GraphTitle, string GraphSubtitle, string User)
        {
            PurchaseByCategoryVCModel vcDataModel = new PurchaseByCategoryVCModel
            {
                AllPurchases = PurchaseHistory,
                GraphTitle = GraphTitle,
                GraphTitleSubtext = GraphSubtitle
            };

            return View(vcDataModel);
        }

    }
}
