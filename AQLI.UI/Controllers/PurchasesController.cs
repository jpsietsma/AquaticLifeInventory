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

            if (ID != 0)
            {
                model = DataSource.List_PurchaseInvoices().Where(pi => pi.PurchaseInvoiceID == ID).First();
            }
            else
            {
                model = new PurchaseInvoiceModel();
                model.Purchases = new List<PurchaseModel>();
            }          

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
            var _formData = Request.Form["passData"].First();           
            var _dataModel = JsonConvert.DeserializeObject<PurchaseInvoiceModel>(_formData);

            var _purchaseData = Request.Form["purchaseData"];
            var _purchaseList = JsonConvert.DeserializeObject<List<PurchaseModel>>(_purchaseData);

            _dataModel.Store = DataSource.List_Stores().Where(s => s.StoreID == _dataModel.StoreID).First();

            //Add purchase to the database and return PK
            //int invoicePK = DataSource.Add_PurchaseInvoice(_dataModel);

            //Upload invoice .pdf if existing filepath is null and form file selected
            if (_dataModel.InvoiceFilePath == null && Request.Form.Files.Count > 0)
            {
                var data = Request.Form.Files[0];
                _dataModel.InvoiceFilePath = await UploadFiles(Request.Form.Files[0], _dataModel.Store.StoreName);
            }

            foreach (PurchaseModel purchaseModel in _purchaseList)
            {
                purchaseModel.Date = _dataModel.PurchaseDate.ToString();
                purchaseModel.StoreID = _dataModel.StoreID;
                purchaseModel.OwnerID = _dataModel.OwnerID;

                _dataModel.Purchases.Add(purchaseModel);

                switch (purchaseModel.PurchaseCategoryID)
                {
                    //Live fish purchase, add record to UserFish
                    case 1:
                    case 6:
                        {
                            break;
                        }
                    //Live plant purchase, add record to UserPlant
                    case 2:
                        {
                            break;
                        }
                    //Supply purchase, add record to UserSupply
                    case 3:
                    case 4:
                    case 7:
                    case 26:
                        {
                            break;
                        }
                    default:
                        break;
                }                
            }

            var modelList = _dataModel.Purchases;

            try
            {
                await DataSource.Add_PurchaseInvoice(_dataModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Upload purchase invoice to ~/invoices/
        /// </summary>
        /// <param name="InvoiceFile">IFormFile represents file for upload</param>
        /// <param name="storeName">String name of store which invoice applies to</param>
        /// <returns>String full filepath to uploaded invoice .pdf</returns>
        private async Task<string> UploadFiles(IFormFile InvoiceFile, string storeName)
        {
            long size = InvoiceFile.Length;
            string filePath = string.Empty;

            if (size > 0)
            {
                // full path to file in invoice upload location
                filePath = Path.Combine(Env.WebRootPath, "invoices", string.Concat("Invoice_",
                    storeName.Replace(" ", "-"),
                    "_",
                    DateTime.Now.ToString().Split(" ").First().Replace("/", "-"),
                    "." + InvoiceFile.FileName.Split(".").Last()
                    ));

                //save the file to the path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await InvoiceFile.CopyToAsync(stream);
                }                                
            }

            return filePath;
        }

    }
}