using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AQLI.Data.Models;
using AQLI.DataServices;
using AQLI.DataServices.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AQLI.UI.Controllers
{
    [Authorize]
    public class TankController : Controller
    {
        private readonly ILogger<TankController> _logger;
        private DataFactory DataSource;
        private WebsiteUser UserModel;

        public TankController(ILogger<TankController> logger, DataFactory _dataFactory, DatabaseContext _efContext)
        {
            _logger = logger;
            DataSource = _dataFactory;
            UserModel = new WebsiteUser { UserId = 1, FirstName = "Jimmy", LastName = "Sietsma", Email = "jpsietsma@gmail.com", UserName = "jpsietsma" };
        }

        public IActionResult Index()
        {
            return View(UserModel);
        }                

        [HttpGet]
        public IActionResult _Details(int ID)
        {
            AquaticTankModel model = DataSource.Find_TankDetails(ID);

            if (model == null)
            {
                model = new AquaticTankModel { TankID = 0 };
            }

            return PartialView(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult _Save(AquaticTankModel _dataModel)
        {
            if (_dataModel.TankID == 0)
            {
                _dataModel.Owner = UserModel;
                _dataModel.OwnerID = UserModel.UserId;
                DataSource.Add_Tank(_dataModel);
            }
            else
            {
                DataSource.Update_TankDetails(_dataModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult _Delete(int ID)
        {
            DataSource.Remove_UserTank(ID);

            return RedirectToAction("Index");
        }

        public IActionResult _ConfirmDeleteTank(int ID)
        {
            return PartialView("_ConfirmDeleteTank", DataSource.Find_TankDetails(ID));
        }

    }
}
