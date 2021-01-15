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
    public class ExportController : Controller
    {
        private readonly DatabaseContext Database;
        private readonly DataFactory DataSource;


        public ExportController(DatabaseContext _context, DataFactory _factory)
        {
            Database = _context;
            DataSource = _factory;
        }

        public async Task<FileContentResult> ExportPurchaseInvoice(string fileType, int purchaseID)
        {
            switch (fileType)
            {
                case "csv":
                    {
                        return ExportAsExcel(purchaseID);                        
                    }
                case "pdf":
                    {
                        return await ExportAsPDF(purchaseID);
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        private FileContentResult ExportAsExcel(int purchaseID)
        {
            var list = Database.PurchaseInvoices.Where(I => I.PurchaseInvoiceID == purchaseID).First().Purchases;
            
            //var excludedColumns = new string[] { "ContactId", "Title", "Suffix", "MiddleName", "EmailAddress", "Active", "StreetAddress2", "FarmAssociation" };
            //return BuildCSV(list.ListRecipients, true, list.ListName, true, excludedColumns);

            return null;
        }

        private async Task<FileContentResult> ExportAsPDF(int purchaseID)
        {
            string invoicePath = Database.PurchaseInvoices.Where(I => I.PurchaseInvoiceID == purchaseID).First().InvoiceFilePath;

            var bytes = await System.IO.File.ReadAllBytesAsync(invoicePath);

            return File(bytes, "application/pdf");
        }

        /// <summary>
        /// Build a CSV string of a list of any class of objects, with option to directly prompt user to download.
        /// </summary>
        /// <typeparam name="T">Class of the view models contained by the passed in list</typeparam>
        /// <param name="dataList">List of view models</param>
        /// <param name="promptDownload">prompt user to download file?</param>
        /// <param name="promptFilename">Filename to be defaulted in download prompt</param>
        /// <param name="includeHeader">Write headers to the csv file?</param>
        /// <param name="excludedColumns">string array of properties to be excluded</param>
        /// <returns>Returns csv string of list if promptDownload is set to false, otherwise returns FileContentResult which results in download of csv</returns>
        public dynamic BuildCSV<T>(List<T> dataList, bool promptDownload = false, string promptFilename = null, bool includeHeader = false, params string[] excludedColumns) where T : class
        {
            StringBuilder csvData = new StringBuilder();

            Type viewModelClass = typeof(T);

            if (includeHeader)
            {
                StringBuilder header = new StringBuilder();
                var vmProperties = viewModelClass.GetProperties();

                //Exclude headers provided by user in excludedParameters string[]
                //If no headers specifically excluded, use all property names as headers from view model class
                foreach (var property in vmProperties)
                {
                    if (!excludedColumns.Contains(property.Name))
                    {
                        header.Append($@"{property.Name}, ");
                    }
                }

                csvData.AppendLine(header.ToString());
            }

            //Build each item row and convert to csv, then append to csvData stringbuilder
            foreach (var item in dataList)
            {
                StringBuilder itemRowData = new StringBuilder();

                var itemProperties = item.GetType().GetProperties();
                foreach (var property in itemProperties)
                {
                    if (!excludedColumns.Contains(property.Name))
                    {
                        itemRowData.Append(property.GetValue(item) + ", ");
                    }
                }

                //Add our row data to the overall csvData, then clear our itemRowData object for next iteration
                csvData.AppendLine(itemRowData.ToString());
                itemRowData.Clear();
            }

            if (promptDownload)
            {
                //if prompt filename is null, 
                //   set it to "MailingListDownload.csv" 
                //otherwise check if prompt filename contains file extension, 
                //    if no: add it and then set as the filename
                //    if yes: set the provided name as the filename
                promptFilename = promptFilename == null ? "WITExportedCSV.csv" : promptFilename.Contains(".csv") ? promptFilename : promptFilename + ".csv";

                return File(new UTF8Encoding().GetBytes(csvData.ToString()), "text/csv", promptFilename);
            }

            return csvData.ToString();
        }

    }
}
