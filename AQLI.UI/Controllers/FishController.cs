using AQLI.Data.Models;
using AQLI.DataServices;
using AQLI.DataServices.context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQLI.UI.Controllers
{
    [Authorize]
    public class FishController : Controller
    {
        private readonly DataFactory DataSource;
        private readonly UserManager<WebsiteUser> UserManager;
       

        public FishController(DataFactory _dataFactory, UserManager<WebsiteUser> _userManager)
        {
            DataSource = _dataFactory;
            UserManager = _userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            var user = UserManager.GetUserAsync(User).Result;
            List<UserFishModel> _allUserFish = DataSource.Find_UserFish(user.UserId);
            
            return View(_allUserFish);
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult _AddMedicalRecord()
        {
            ViewBag.MedicalRecordTypes = DataSource.List_MedicalRecordTypes();

            return View();
        }

        [HttpPost]
        public IActionResult _saveMedicalRecord(UserFish_MedicalRecordModel _modelData)
        {
            DataSource.Add_MedicalRecord(_modelData);
                        
            return RedirectToAction("Details", new { ID = _modelData.UserFishID });
        }

        public IActionResult Details(int ID)
        {
            var user = UserManager.GetUserAsync(User).Result;

            UserFishModel _dataModel = DataSource.Find_UserFish(user.UserId).Where(id => id.UserFishID == ID).FirstOrDefault();

            return View(_dataModel);
        }

        public IActionResult Edit(int ID)
        {
            var user = UserManager.GetUserAsync(User).Result;
            List<UserFishModel> _allUserFish = DataSource.Find_UserFish(user.UserId);

            UserFishModel _dataModel = DataSource.Find_UserFish(user.UserId).Where(id => id.UserFishID == ID).FirstOrDefault();

            return View(_dataModel);
        }

        public async Task<IActionResult> Remove(int ID)
        {
            UserFishModel _dataModel = DataSource.Find_FishDetails(ID);

            await DataSource.Remove_UserFish(_dataModel);

            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> CreateUserFish(UserFishModel _dataModel)
        {
            await DataSource.Add_UserFish(_dataModel);

            return RedirectToAction("Dashboard");
        }
    }
}
