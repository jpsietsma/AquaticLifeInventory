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
using Microsoft.AspNetCore.Authorization;
using AQLI.DataServices.context;

namespace AQLI.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataFactory DataSource;

        public HomeController(ILogger<HomeController> logger, DataFactory _factory)
        {
            _logger = logger;
            DataSource = _factory;
        }

        [Authorize]
        public IActionResult Index()
        {
            var x = DataSource.List_PurchaseInvoices();

            return View();
        }

    }
}
