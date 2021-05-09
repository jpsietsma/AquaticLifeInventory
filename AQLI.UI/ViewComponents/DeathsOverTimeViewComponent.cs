using AQLI.Data.Models.ViewComponentModels;
using AQLI.DataServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQLI.UI.ViewComponents
{
    public class DeathsOverTimeViewComponent : ViewComponent
    {
        private readonly DataFactory DataSource;

        public DeathsOverTimeViewComponent(DataFactory _datasource)
        {
            DataSource = _datasource;
        }

        public async Task<IViewComponentResult> InvokeAsync(string GraphTitle, string GraphSubtitle)
        {
            DeathsOverTimeVCModel viewData = new DeathsOverTimeVCModel
            {
                GraphTitle = GraphTitle,
                GraphTitleSubtext = GraphSubtitle
            };

            return View(viewData);
        }
    }
}
