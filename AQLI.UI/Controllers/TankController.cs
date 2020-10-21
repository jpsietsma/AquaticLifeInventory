using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AQLI.UI.Models;
using AQLI.Data.Models;
using AQLI.DataServices;

namespace AQLI.UI.Controllers
{
    public class TankController : Controller
    {
        private readonly ILogger<TankController> _logger;
        private DataFactory DataSource;
        private WebsiteUser UserModel;

        public TankController(ILogger<TankController> logger, DataFactory _dataFactory)
        {
            _logger = logger;
            DataSource = _dataFactory;
            UserModel = DataSource.Find_UserDetails(1);

        }

        public IActionResult Index()
        {
            return View(UserModel);
        }

        [HttpPost]
        public IActionResult SaveTank(AquaticTankModel _dataModel)
        {
            _ = _dataModel.TankId == 0 ? DataSource.Add_Tank(_dataModel) : DataSource.Update_TankDetails(_dataModel);
            
            return View("Index", UserModel);
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            DataSource.Remove_UserTank(id);

            return View("Index", UserModel);
        }

        [HttpGet]
        public IActionResult _Details(int ID)
        {
            var model = new AquaticTankModel { TankType = new TankType { TankTypeID = 0, TypeName = "Unknown" } };

            if (ID != 0)
            {
                model = UserModel.AquaticTanks.Where(t => t.TankId == ID).FirstOrDefault();
            }            

            return PartialView(model);
        }
        
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult _Save(AquaticTankModel _dataModel)
        {
                DataSource.Add_Tank(_dataModel);
                UserModel.AquaticTanks.Add(_dataModel);                

            return RedirectToAction("Index");
        }

    }
}
