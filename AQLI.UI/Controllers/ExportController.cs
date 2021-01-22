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
using System.IO;

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
                        return BuildAQLPurchaseInvoicePDF<PurchaseInvoiceModel>(invoice.PurchaseInvoiceID);
                    }
                    default:
                    {
                        //Prompt download of scanned receipt file in whatever format it was uploaded in
                        var bytes = await System.IO.File.ReadAllBytesAsync(invoice.InvoiceFilePath);

                        return File(bytes, "application/octet-stream", filename.Replace(".csv", ".pdf"));
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
            private FileContentResult BuildAQLPurchaseInvoicePDF<T>(int invoiceID) where T: class
            {
                var invoice = DataSource.List_PurchaseInvoices().Where(I => I.PurchaseInvoiceID == invoiceID).First();
                string filename = invoice.InvoiceFilePath.Split('/').Last().Split('.').First() + ".pdf";

                var bytes = GenerateAQLPurchaseInvoicePDF(invoice);

                return File(bytes, "application/octet-stream", filename);
            }

            /// <summary>
            /// Generate and return PDF invoice document as byte[]
            /// </summary>
            /// <param name="invoiceID">ID of the invoice to generate PDF document</param>
            /// <returns>byte[] contents of PDF document</returns>
            private byte[] GenerateAQLPurchaseInvoicePDF(PurchaseInvoiceModel invoice)
            {
                byte[] bytes = null;
                
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 5, 5, 5, 5);
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                             
                    //Open document for writing to memory, and add first page to document.
                    document.OpenDocument();
                    document.NewPage();                    

                    //Create a table to hold purchase line items
                    PdfPTable t = new PdfPTable(5);
                        t.SetWidths(new float[] { 150f, 20f, 40f, 40f, 75f });

                    //Add table with purchase line items and summary
                    AddPurchasesTable(t, invoice, document);                                    

                    document.Close();
                    bytes = memoryStream.ToArray();
                    memoryStream.Close();                   
                }

                return bytes;
            }

        private void AddInvoicePurchaseTableHeaderRow(PdfPTable t)
        {
            PdfPHeaderCell descriptionHeaderCell = new PdfPHeaderCell();
            descriptionHeaderCell.AddElement(new Chunk("Purchase Desctiption"));
            
            PdfPHeaderCell quantityHeaderCell = new PdfPHeaderCell();
            quantityHeaderCell.AddElement(new Chunk("Qty"));
            
            PdfPHeaderCell costHeaderCell = new PdfPHeaderCell();
            costHeaderCell.AddElement(new Chunk("Cost"));

            PdfPHeaderCell extHeaderCell = new PdfPHeaderCell();
            extHeaderCell.AddElement(new Chunk("Ext Cost"));

            PdfPHeaderCell tankHeaderCell = new PdfPHeaderCell();
            tankHeaderCell.AddElement(new Chunk("Assigned Tank"));

            t.AddCell(descriptionHeaderCell);
            t.AddCell(quantityHeaderCell);
            t.AddCell(costHeaderCell);
            t.AddCell(extHeaderCell);
            t.AddCell(tankHeaderCell);
        }

        private void AddInvoicePurchaseDataRow(PurchaseModel purchase, PdfPTable t)
        {
            PdfPCell descriptionCell = new PdfPCell();
            PdfPCell quantityCell = new PdfPCell();
            PdfPCell costCell = new PdfPCell();
            PdfPCell extCell = new PdfPCell();
            PdfPCell tankCell = new PdfPCell();

            Chunk descriptionData = new Chunk(purchase.Description);
            descriptionData.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);

            descriptionCell.AddElement(descriptionData);

            Chunk quantityData = new Chunk(purchase.Quantity.ToString());
            quantityData.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);

            quantityCell.AddElement(quantityData);

            Chunk costData = new Chunk(purchase.Cost.ToString("C"));
            costData.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);

            costCell.AddElement(costData);

            Chunk extData = new Chunk(purchase.ExtCost.ToString("C"));
            extData.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);

            extCell.AddElement(extData);

            Chunk tankData = new Chunk(purchase.Tank != null ? purchase.Tank.Name : "-unassigned-");
            tankData.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);

            tankCell.AddElement(tankData);

            t.AddCell(descriptionCell);
            t.AddCell(quantityCell);
            t.AddCell(costCell);
            t.AddCell(extCell);
            t.AddCell(tankCell);
        }

        private void AddPurchasesTable(PdfPTable t, PurchaseInvoiceModel invoice, Document document)
        {
            //Add header row to purchase table
            AddInvoicePurchaseTableHeaderRow(t);

            //Generate table rows for each purchase on invoice, Add them to the table
            invoice.Purchases.ForEach(p => AddInvoicePurchaseDataRow(p, t));

            //Add purchase table summary footer
            AddPurchaseTableSummaryFooter(t, invoice);

            //Add the table to the PDF document
            document.Add(t);
        }

        private void AddPurchaseTableSummaryFooter(PdfPTable t, PurchaseInvoiceModel invoice)
        {
            //Quantity total summary row
            Paragraph quantityHeaderText = new Paragraph("Total Purchases: ");
                quantityHeaderText.Alignment = Element.ALIGN_RIGHT;

            PdfPCell quantityTotalSummaryHeader = new PdfPCell();
                quantityTotalSummaryHeader.AddElement(quantityHeaderText);

            PdfPCell quantityTotalSummaryValue = new PdfPCell();
                quantityTotalSummaryValue.AddElement(new Chunk(invoice.Purchases.Sum(p => p.Quantity).ToString()));
                quantityTotalSummaryValue.Colspan = 4;

            t.AddCell(quantityTotalSummaryHeader);
            t.AddCell(quantityTotalSummaryValue);

            //Subtotal cost summary row
            Paragraph costHeaderText = new Paragraph("Purchase Subtotal: ");
                costHeaderText.Alignment = Element.ALIGN_RIGHT;

            PdfPCell costTotalSummaryHeader = new PdfPCell();
                costTotalSummaryHeader.AddElement(costHeaderText);
                costTotalSummaryHeader.Colspan = 3;

            PdfPCell costTotalSummaryValue = new PdfPCell();
                costTotalSummaryValue.AddElement(new Chunk(invoice.Purchases.Sum(p => p.ExtCost).ToString("C")));
                costTotalSummaryValue.Colspan = 2;

            t.AddCell(costTotalSummaryHeader);
            t.AddCell(costTotalSummaryValue);

            //Tax summary row
            Paragraph taxHeaderText = new Paragraph("Tax (8%): ");
                taxHeaderText.Alignment = Element.ALIGN_RIGHT;

            PdfPCell taxHeader = new PdfPCell();
                taxHeader.AddElement(taxHeaderText);
                taxHeader.Colspan = 3;

            PdfPCell taxValue = new PdfPCell();
            var tax = (float)invoice.Purchases.Sum(p => p.ExtCost) * .08f;

            taxValue.AddElement(new Chunk(tax.ToString("C")));
            taxValue.Colspan = 2;

            t.AddCell(taxHeader);
            t.AddCell(taxValue);

            //Total cost summary row
            Paragraph totalHeaderText = new Paragraph("Invoice Total: ");
            totalHeaderText.Alignment = Element.ALIGN_RIGHT;

            PdfPCell totalHeader = new PdfPCell();
            totalHeader.AddElement(totalHeaderText);
            totalHeader.Colspan = 3;

            PdfPCell totalValue = new PdfPCell();
            var total = (float)invoice.Purchases.Sum(p => p.ExtCost) * 1.08f;

            totalValue.AddElement(new Chunk(total.ToString("C")));
            totalValue.Colspan = 2;

            t.AddCell(totalHeader);
            t.AddCell(totalValue);

        }

        #endregion
    }
}

// ---------- These are the items available to add to an iTextSharp PDFPCell -----------
//string text = @"This is a string added to the document.";
//Chunk chunk = new Chunk("This is from chunk. ");
//Phrase phrase = new Phrase("This is from Phrase.");
//Paragraph paragraph = new Paragraph("This is from the first paragraph");
//paragraph.SpacingBefore = 10;
//paragraph.SpacingAfter = 10;
//paragraph.Alignment = Element.ALIGN_LEFT;
//paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.GREEN);
//paragraph.Add(text);