using AQLI.Data.Models;
using AQLI.DataServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQLI.UI.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly DataFactory DataSource;

        public MaintenanceController(DataFactory _dataFactory)
        {
            DataSource = _dataFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult _AddLog()
        {            
            return View();
        }

        public IActionResult _Details_FeedingLog(int ID)
        {
            TankFeedingRecordModel dataModel = DataSource.Find_TankMaintenanceLogDetails(ID)
                .FeedingRecords
                    .Where(fr => fr.MaintenanceLogID == ID)
                    .FirstOrDefault();

            return View(dataModel);
        }

        public IActionResult _Details_FilterChangeLog(int ID)
        {
            TankFilterChangeRecordModel dataModel = DataSource.Find_TankMaintenanceLogDetails(ID)
                .FilterChangeRecords
                    .Where(fr => fr.MaintenanceLogID == ID)
                    .FirstOrDefault();

            return View(dataModel);
        }

        public IActionResult _Details_TemperatureLog(int ID)
        {
            TankTemperatureRecordModel dataModel = DataSource.Find_TankMaintenanceLogDetails(ID)
                .TemperatureRecords
                    .Where(fr => fr.MaintenanceLogID == ID)
                    .FirstOrDefault();

            return View(dataModel);
        }

        public IActionResult _Details_WaterChangeLog(int ID)
        {
            TankWaterChangeRecordModel dataModel = DataSource.Find_TankMaintenanceLogDetails(ID)
                .WaterChangeRecords
                    .Where(fr => fr.MaintenanceLogID == ID)
                    .FirstOrDefault();

            return View(dataModel);
        }

        public IActionResult _Details_InventoryLog(int ID)
        {
            TankCreatureInventoryRecordModel dataModel = DataSource.Find_TankMaintenanceLogDetails(ID)
                .CreatureInventoryRecords
                    .Where(fr => fr.MaintenanceLogID == ID)
                    .FirstOrDefault();

            return View(dataModel);
        }
    }
}
