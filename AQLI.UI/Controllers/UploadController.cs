using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AQLI.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQLI.UI.Controllers
{
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment Env;

        public UploadController(IWebHostEnvironment _env)
        {
            Env = _env;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> UploadFiles(PurchaseInvoiceModel _model)
        {
            IFormFile InvoiceFile = _model.InvoiceFile;

            long size = InvoiceFile.Length;

            var filePaths = new List<string>();

                if (size > 0)
                {
                    // full path to file in invoice upload location
                    var filePath = Path.Combine(Env.WebRootPath, "invoices", string.Concat("Invoice_",
                        _model.StoreName.Replace(" ", ""),
                        "_",
                        _model.PurchaseDate.ToString().Split(" ").First().Replace("/", "-"), 
                        "." + InvoiceFile.FileName.Split(".").Last()
                        ));

                    filePaths.Add(filePath);

                    //save the file to the path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await InvoiceFile.CopyToAsync(stream);
                    }
                }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return filePaths.First();
        }
    }
}
