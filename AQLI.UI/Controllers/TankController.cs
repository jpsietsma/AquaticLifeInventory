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

            #region Test code area to populate user, tanks, and fish details
                //var userModel = DataSource.CreateTankModel<AquaticTankModel>();
                UserModel = DataSource.Find_UserDetails(1);
            #endregion
        }

        public IActionResult Index()
        {
            return View(UserModel);
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var userModel = DataSource.Find_UserDetails(UserModel.UserId);

            userModel.AquaticTanks.Remove(userModel.AquaticTanks.Where(t => t.TankId == id).First());
            DataSource.Remove_UserTank(id);

            return View("Index", userModel);
        }

        [HttpPost]
        public IActionResult _Details(int Id)
        {
            return View(UserModel.AquaticTanks.Where(t => t.TankId == Id).FirstOrDefault());
        }

    }
}
