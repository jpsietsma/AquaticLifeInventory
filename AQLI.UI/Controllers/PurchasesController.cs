using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AQLI.Data.Models;
using AQLI.DataServices;
using AQLI.DataServices.context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AQLI.UI.Controllers
{
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
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> _Save(PurchaseModel _dataModel)
        {

            if (_dataModel.InvoiceFilePath.Count() > 0)
            {
                _dataModel.InvoiceFilePath = await UploadFiles(_dataModel);
            }            

            if (_dataModel.TankID == 0)
            {
                DataSource.Add_Purchase(_dataModel);
            }
            else
            {
                DataSource.Update_Purchase(_dataModel);
            }

            return RedirectToAction("Index");
        }

        private async Task<string> UploadFiles(PurchaseModel _model)
        {
            List<IFormFile> InvoiceFile = _model.InvoiceUploadedFile;

            long size = InvoiceFile.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in InvoiceFile)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in invoice upload location
                    var filePath = Path.Combine(Env.WebRootPath, "invoices", string.Concat("Invoice_",
                        _model.Store.StoreName.Replace(" ", ""),
                        "_",
                        _model.Date.ToString().Split(" ").First().Replace("/", "-"),
                        "." + formFile.FileName.Split(".").Last()
                        ));

                    filePaths.Add(filePath);

                    //save the file to the path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return filePaths.First();
        }
    }
}
