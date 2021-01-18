using AQLI.Data.Models;
using AQLI.DataServices;
using AQLI.DataServices.context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;

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

        #region Purchase Invoice Export Methods...

            public async Task<FileContentResult> ExportPurchaseInvoice(int purchaseID, string fileType = "pdf")
            {
                var invoice = DataSource.List_PurchaseInvoices().Where(I => I.PurchaseInvoiceID == purchaseID).First();
                string filename = invoice.InvoiceFilePath.Split('/').Last().Split('.').First() + ".csv";

                switch (fileType)
                {
                    case "csv":
                    {
                        //Return CSV file of purchases associated with the invoice.  Headers included.
                        return BuildCSV(invoice.Purchases, true, filename, true, new string[] { "Store", "Tank", "Invoice", "PurchaseCategory" });                        
                    }
                    case "aqlpdf":
                    {
                        //Prompt generation and download of PDF version of AQL Invoice details window
                        return BuildPDF<PurchaseInvoiceModel>(invoice.PurchaseInvoiceID);
                    }
                    default:
                    {
                        //Prompt download of scanned receipt file in whatever format it was uploaded in
                        var bytes = await System.IO.File.ReadAllBytesAsync(invoice.InvoiceFilePath);

                        return File(bytes, "application/octet-stream", filename);
                    }                    
                }
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
            private dynamic BuildCSV<T>(List<T> dataList, bool promptDownload = false, string promptFilename = null, bool includeHeader = false, params string[] excludedColumns) where T : class
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
                            if (property.Name == "Cost" || property.Name == "ExtCost")
                            {
                                var formattedCurrency = ((decimal)(property.GetValue(item))).ToString("C");

                                itemRowData.Append(formattedCurrency + ", ");
                            }
                            else
                            {
                                itemRowData.Append(property.GetValue(item) + ", ");
                            }                            
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

            /// <summary>
            /// Generate and prompt to save a PDF document of the Invoice Details from the AQL application.  Custom AQL invoice.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns>Generated PDF document of invoice, propmpt to save</returns>
            private FileContentResult BuildPDF<T>(int invoiceID) where T: class
            {
                var invoice = DataSource.List_PurchaseInvoices().Where(I => I.PurchaseInvoiceID == invoiceID).First();
                var bytes = GeneratePDF(invoice.PurchaseInvoiceID);

                return File(bytes, "application/octet-stream");
            }

            private byte[] GeneratePDF(int invoiceID)
            {
                byte[] bytes = null;

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10, 10, 10, 10);

                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    Chunk chunk = new Chunk("This is from chunk. ");
                    document.Add(chunk);

                    Phrase phrase = new Phrase("This is from Phrase.");
                    document.Add(phrase);

                    Paragraph para = new Paragraph("This is from paragraph.");
                    document.Add(para);

                    string text = @"you are successfully created PDF file.";
                    Paragraph paragraph = new Paragraph();
                    paragraph.SpacingBefore = 10;
                    paragraph.SpacingAfter = 10;
                    paragraph.Alignment = Element.ALIGN_LEFT;
                    paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.GREEN);
                    paragraph.Add(text);
                    document.Add(paragraph);

                    document.Close();

                    bytes = memoryStream.ToArray();
                    memoryStream.Close();                   
                }

                return bytes;
            }

        #endregion
    }
}
