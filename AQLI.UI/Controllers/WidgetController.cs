using AQLI.Data.Models;
using AQLI.Data.Models.ViewComponentModels;
using AQLI.DataServices;
using AQLI.DataServices.context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQLI.UI.Controllers
{
    public class WidgetController : Controller
    {
        private readonly DataFactory DataSource;
        private readonly DatabaseContext Database;
        private readonly WebsiteUser DataUser;

        public WidgetController(DataFactory _dataFactory, DatabaseContext _database)
        {
            DataSource = _dataFactory;
            Database = _database;

            DataUser = DataSource.List_Users().Where(u => u.UserId == 4).FirstOrDefault();
        }

        public IActionResult Index()
        {
            var ds = DataSource.List_UserFish(4);

            var db = Database.UserFish_MedicalRecords;

            return View();
        }
                
        private string DatabaseRecordWidget(dynamic DataObject)
        {
            dynamic obj = DataObject;
            Type objType = obj.GetType();

            StringBuilder widgetHtmlCode = new StringBuilder();
            widgetHtmlCode.Append("<div class='container' style='font-size: 12px;'><table class='table table-sm table-bordered col-8'><tr>");

            //Figure out primary Key property for object 
            //- concat class name and ID? 
            //- properties where name like %Id".first() ?

            string keyPropertyName = objType.GetProperties().Where(p => p.Name.EndsWith("Id") || p.Name.EndsWith("ID")).ToList().Select(p => p.Name).First();
            int pkValue = objType.GetProperty(keyPropertyName).GetValue(obj);

            //pk#1234 
            widgetHtmlCode.Append(@$"<td><b>pk#</b><i>{pkValue}</i> ");
            
            //Created on 04/15/2021 
            if (obj.GetType().GetProperty("Added") != null)
            {
                widgetHtmlCode.Append(@$"<b>Created</b> <i>{objType.GetProperty("Added").GetValue(obj)}</i> ");
            }
            //by Jimmy Sietsma (#4)
            if (obj.GetType().GetProperty("AddedBy") != null)
            {
                var recordData = DataSource.List_Users().Where(u => u.UserId == objType.GetProperty("AddedBy").GetValue(obj)).First();
                widgetHtmlCode.Append($@"<b>by</b> <i>{recordData.FirstName} {recordData.LastName}</i> ");

                //widgetHtmlCode.Append($@"<td>by {objType.GetProperty("AddedBy").GetValue(obj)}</td>");
            }
            //Last Modified 04/16/2021 
            if (obj.GetType().GetProperty("Modified") != null)
            {
                widgetHtmlCode.Append($@"<b>Last Modified</b> <i>{objType.GetProperty("Modified").GetValue(obj)}</i> ");
            }
            //by Jimmy Sietsma (#4)
            if (obj.GetType().GetProperty("ModifiedBy") != null)
            {
                var recordData = DataSource.List_Users().Where(u => u.UserId == objType.GetProperty("ModifiedBy").GetValue(obj)).First();
                widgetHtmlCode.Append($@"<b>by</b> <i>{recordData.FirstName} {recordData.LastName}</i></td>");

                //widgetHtmlCode.Append($@"<td>by {objType.GetProperty("ModifiedBy").GetValue(obj)}</td>");
            }

            widgetHtmlCode.Append("</tr></table></div>");

            //  This should return:
            //  <div class='container'><table><tr><td><b>pk#</b><i>1234</i> <b>Created</b> <i>04/15/2021</i> <b>by</b> <i>Jimmy Sietsma</i> <b>Last Modified</b> <i>04/16/2021</i> <b>by</b> <i>Jimmy Sietsma</i></td></tr></table></div>
            string finalString = widgetHtmlCode.ToString();

            return finalString;
        }

        public IActionResult PurchaseChartWidget()
        {
            var userModel = DataUser;

            return View("_Widget_UserDashboard_PurchaseChartWidget", userModel);
        }

        public IActionResult TestWidget()
        {
            return View();
        }

        //Database widget = working
        public IActionResult TestVC()
        {
            UserFishModel Model = DataSource.List_UserFish(4).First();

            return View(Model);
        }

        //Grouped Bar Graph = working
        public IActionResult TestGraphVC()
        {

            PurchaseByCategoryVCModel viewDataModel = new PurchaseByCategoryVCModel
            {
                GraphTitle = "Current Year Breakdown of Purchases By Category Type",
                AllPurchases = DataSource.List_Purchases().Where(p => p.OwnerID == 4).ToList()
            };




            return View(viewDataModel);
        }
        
        //Pie Chart = IN PROGRESS
        public IActionResult TestPieChartVC()
        {
            TotalPurchaseBreakdownVCModel viewData = new TotalPurchaseBreakdownVCModel
            {
                GraphTitle = "Yearly Total Purchase Breakdown",
                GraphTitleSubtext = "for Jimmy Sietsma"
            };


            return View(viewData);
        }

        public IActionResult TestVertBarVC()
        {
            TotalDeathBreakdownVCModel dataModel = new TotalDeathBreakdownVCModel
            {
                GraphTitle = "Total Deaths vs Living",
                GraphTitleSubtext = "for Jimmy Sietsma"
            };


            return View(dataModel);
        }

    }
}
