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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AQLI.DataServices.Extensions;
using Microsoft.AspNetCore.Http;

namespace AQLI.UI.Controllers
{
    [Authorize]
    public class TankController : Controller
    {
        private readonly ILogger<TankController> _logger;
        private readonly UserManager<WebsiteUser> UserManager;

        private DataFactory DataSource;
        private WebsiteUser UserModel;

        public TankController(ILogger<TankController> logger, DataFactory _dataFactory, DatabaseContext _efContext, UserManager<WebsiteUser> _userManager)
        {
            _logger = logger;
            DataSource = _dataFactory;
            UserManager = _userManager;                        
        }

        public async Task<IActionResult> Index()
        {
            UserModel = await UserManager.GetUserAsync(User);

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
        public async Task<IActionResult> _Save(AquaticTankModel _dataModel)
        {
            if (_dataModel.TankID == 0)
            {
                await DataSource.Add_Tank(_dataModel);
            }
            else
            {
                await DataSource.Update_TankDetails(_dataModel);
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

        public IActionResult Dashboard(int ID)
        {
            var data = DataSource.Find_TankDetails(ID);

            return View(data);
        }



        public IActionResult AddFish(int ID)
        {
            AquaticTankModel _model = DataSource.Find_TankDetails(ID);

            return View(_model);
        }

        public IActionResult _AddFish(int ID)
        {
            var tankModel = DataSource.Find_TankDetails(ID);

            return View(tankModel);
        }

        public async Task<IActionResult> _UnassignFish(int ID)
        {
            var id = DataSource.Find_FishDetails(ID).TankID;

            await DataSource.Unassign_TankFish(ID);

            return RedirectToAction("Dashboard", "Tank", new { ID = id });
        }

        public async Task<IActionResult> _saveNewFish(IFormCollection _newFishData)
        {
            var addedValues = _newFishData["addedFishList"].First().Split(',');
            var tankId = int.Parse(_newFishData["tankID"].First());

            List<int> ids = new List<int>();

            foreach (var fishID in addedValues)
            {
                ids.Add(int.Parse(fishID)); 
            }

            await DataSource.Add_TankFish(ids, tankId);

            var user = await UserManager.GetUserAsync(User);

            return RedirectToAction("Dashboard", new { ID = tankId });
        }


        public IActionResult Test()
        {
            AquaticTankModel dataTest = DataSource.Find_TankDetails(14);

            return View(dataTest);
        }

    }
}
