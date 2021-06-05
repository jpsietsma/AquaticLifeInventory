using AQLI.Data.Models.ViewComponentModels;
using AQLI.DataServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQLI.UI.ViewComponents
{
    public class TotalDeathBreakdownViewComponent : ViewComponent
    {
        private readonly DataFactory DataSource;

        public TotalDeathBreakdownViewComponent(DataFactory _datasource)
        {
            DataSource = _datasource;
        }

        public async Task<IViewComponentResult> InvokeAsync(string GraphTitle, string GraphSubtitle)
        {
            TotalDeathBreakdownVCModel viewData = new TotalDeathBreakdownVCModel
            {
                GraphTitle = GraphTitle,
                GraphTitleSubtext = GraphSubtitle
            };


            return View(viewData);
        }
    }
}
