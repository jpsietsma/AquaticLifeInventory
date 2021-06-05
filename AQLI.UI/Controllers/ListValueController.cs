using AQLI.DataServices;
using AQLI.DataServices.context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQLI.UI.Controllers
{
    public class ListValueController : Controller
    {
        private readonly DataFactory DataSource;

        public ListValueController(DataFactory _factory)
        {
            DataSource = _factory;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult TankTypes()
        {
            var tankTypes = DataSource.List_TankTypes();

            return View(tankTypes);
        }

        public IActionResult WaterTypes()
        {
            var waterTypes = DataSource.List_WaterTypes();

            return View(waterTypes);
        }

        public IActionResult TempormentLevels()
        {
            var tempormentLevels = DataSource.List_TempormentLevels();

            return View(tempormentLevels);
        }

        public IActionResult PurchaseCategories()
        {
            var purchaseCategories = DataSource.List_PurchaseCategories();

            return View(purchaseCategories);
        }

        public IActionResult Environments()
        {
            var environments = DataSource.List_Environments();

            return View(environments);
        }

        public IActionResult CreatureTypes()
        {
            var creatureTypes = DataSource.List_CreatureTypes();

            return View(creatureTypes);
        }

        public IActionResult Stores()
        {
            var stores = DataSource.List_Stores();

            return View(stores);
        }

    }
}
