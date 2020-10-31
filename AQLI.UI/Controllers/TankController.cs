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

namespace AQLI.UI.Controllers
{
    public class TankController : Controller
    {
        private readonly ILogger<TankController> _logger;
        private readonly DatabaseContext Database;
        private DataFactory DataSource;
        private WebsiteUser UserModel;

        public TankController(ILogger<TankController> logger, DataFactory _dataFactory, DatabaseContext _efContext)
        {
            _logger = logger;
            Database = _efContext;
            DataSource = _dataFactory;
            UserModel = new WebsiteUser { OwnerID = 1, FirstName = "Jimmy", LastName = "Sietsma", EmailAddress = "jpsietsma@gmail.com", UserName = "jpsietsma" };
        }

        public IActionResult Index()
        {
            return View(UserModel);
        }                

        [HttpGet]
        public IActionResult _Details(int ID)
        {
            AquaticTankModel model = Database.Tank
                    .Include(wt => wt.WaterType)
                    .Include(ct => ct.CreatureType)
                    .Include(tem => tem.Temporment)
                    .Include(env => env.Environment)
                    .Include(tt => tt.TankType)
                    .Where(t => t.TankID == ID)
                    .FirstOrDefault();

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
                _dataModel.OwnerID = UserModel.OwnerID;
                DataSource.Add_Tank(_dataModel);
            }
            else
            {
                DataSource.Update_TankDetails(_dataModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            DataSource.Remove_UserTank(id);

            return View("Index", UserModel);
        }

    }
}
