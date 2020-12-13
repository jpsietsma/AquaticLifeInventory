using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AQLI.Data.Models;
using AQLI.DataServices;
using AQLI.DataServices.context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AQLI.UI.Controllers
{
    [Authorize]
    public class PurchasesController : Controller
    {
        private readonly DatabaseContext Database;
        private readonly DataFactory DataSource;
        private readonly IWebHostEnvironment Env;

        public PurchasesController(DatabaseContext _database, DataFactory _dataFactory, IWebHostEnvironment _env)
        {
            Database = _database;
            DataSource = _dataFactory;
            Env = _env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult _PurchaseDetails(int ID)
        {
            var model = new PurchaseModel();
            model.Date = DateTime.Now.ToString("yyyy-MM-dd");

            if (ID != 0)
            {
                model = Database.Purchases.Where(p => p.PurchaseID == ID).Include("PurchaseCategory").FirstOrDefault();
            }            

            return PartialView(model);
        }

        public IActionResult _PurchaseInvoiceDetails(int ID)
        {
            PurchaseInvoiceModel model;

            model = new PurchaseInvoiceModel();
            model.Purchases = new List<PurchaseModel>();

            return PartialView(model);
        }

        public IActionResult _ConfirmDeletePurchase(int ID)
        {
            var model = DataSource.List_Purchases().Where(p => p.PurchaseID == ID).First();

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> _SavePurchaseInvoice()
        {
            var _json = Request.Form["passData"].First();
            var _dataModel = JsonConvert.DeserializeObject<PurchaseInvoiceModel>(_json);
                _dataModel.Store = DataSource.List_Stores().Where(s => s.StoreID == _dataModel.StoreID).First();

            if (Request.Form.Files.Count > 0)
            {
                var data = Request.Form.Files[0];

                _dataModel.InvoiceFilePath = await UploadFiles(Request.Form.Files[0], _dataModel.Store.StoreName);
            }

            

            if (_dataModel.PurchaseInvoiceID == 0)
            {
                //Upload invoice .pdf for first time invoice creation
                if (_dataModel.InvoiceFile != null)
                {
                    string storeName = DataSource.List_Stores().Where(s => s.StoreID == _dataModel.StoreID).Select(s => s.StoreName).First();

                     _dataModel.InvoiceFilePath = await UploadFiles(_dataModel.InvoiceFile, string.Concat(storeName));
                }

                //Add purchases to the database
                foreach (PurchaseModel purchase in _dataModel.Purchases)
                {
                    DataSource.Add_Purchase(purchase);
                }
            }
            else
            {
                //Update purchases to the database
                foreach (PurchaseModel purchase in _dataModel.Purchases)
                {
                    DataSource.Update_Purchase(purchase);
                }
            }

            return RedirectToAction("Index");
        }

        private async Task<string> UploadFiles(IFormFile InvoiceFile, string title)
        {
            long size = InvoiceFile.Length;
            string filePath = string.Empty;

                if (size > 0)
                {
                    // full path to file in invoice upload location
                    filePath = Path.Combine(Env.WebRootPath, "invoices", string.Concat("Invoice_",
                        title.Replace(" ", ""),
                        "_",
                        DateTime.Now.ToString().Split(" ").First().Replace("/", "-"),
                        "." + InvoiceFile.FileName.Split(".").Last()
                        ));

                    //save the file to the path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await InvoiceFile.CopyToAsync(stream);
                    }

                // process uploaded files
                // Don't rely on or trust the FileName property without validation.
                                
            }

            return filePath;
        }
    }
}
