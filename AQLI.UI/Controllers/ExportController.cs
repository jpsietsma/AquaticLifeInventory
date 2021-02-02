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

using static AQLI.DataServices.BarcodeConfig;
using System.Drawing.Imaging;

namespace AQLI.UI.Controllers
{
    public class ExportController : Controller
    {
        private readonly DatabaseContext Database;
        private readonly DataFactory DataSource;
        private readonly BarcodeFactory BarcodeFactory;


        public ExportController(DatabaseContext _context, DataFactory _factory, BarcodeFactory _barFactory)
        {
            Database = _context;
            DataSource = _factory;
            BarcodeFactory = _barFactory;
        }

        #region Section: Purchase Invoice Export Methods...

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
                    Document document = new Document(PageSize.A4, 2, 2, 2, 2);
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                             
                    //Open document for writing to memory, and add first page to document.
                    document.OpenDocument();
                    document.NewPage();                    

                    //Add table with supplier contact information and AQL logo
                    AddInvoiceSupplierContactHeaderTable(invoice.Store, document);
                
                    //Testing barcodes
                    Image barcode = GetBarcodeImage(invoice);
                        barcode.ScaleAbsoluteHeight(35f);
                        barcode.ScaleAbsoluteWidth(250f);
                        barcode.Alignment = Element.ALIGN_RIGHT;

                    document.Add(barcode);

                    //Add Spacing Between Supplier Header and Purchase Line Item table
                    InsertDocumentLineBreak(document);
                    InsertDocumentLineBreak(document);

                    //Add table with purchase line items, headers, and summary rows
                    AddPurchasesTable(invoice, document);                                    

                    document.Close();
                    bytes = memoryStream.ToArray();
                    memoryStream.Close();                   
                }

                return bytes;
            }
            
            #region Section: Invoice Supplier Table  And Logo Generation Methods...

                private void AddInvoiceSupplierContactHeaderTable(StoreModel supplier, Document document)
                {
                    //Create container table to hold supplier contact table and logo table
                    PdfPTable containerTable = new PdfPTable(2);
                        containerTable.SetWidths(new float[] { 150f, 150f });
                        containerTable.DefaultCell.Border = 0;

                    //Create a table to hold supplier contact information
                    PdfPTable supplierContactTable = new PdfPTable(2);
                        supplierContactTable.SetWidths(new float[] { 60, 40 });
                        supplierContactTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        supplierContactTable.WidthPercentage = 50;

                    PdfPCell supplierNameCell = new PdfPCell();
                        supplierNameCell.Colspan = 2;

                    Chunk supplierNameData = new Chunk(supplier.StoreName);
                        supplierNameData.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);

                        supplierNameCell.AddElement(supplierNameData);                  

                    PdfPCell supplierStreetAddressCell = new PdfPCell();

                    Chunk supplierStreetAddressData = new Chunk("123 Street Address Ave.");
                        supplierStreetAddressData.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                        
                        supplierStreetAddressCell.AddElement(supplierStreetAddressData);

                    PdfPCell supplierCityStateZipCell = new PdfPCell();
                    
                     Chunk supplierCityStateZipData = new Chunk("Walton, NY 13856");
                        supplierCityStateZipData.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);

                        supplierCityStateZipCell.AddElement(supplierCityStateZipData);

                    PdfPCell supplierPhoneEmailCell = new PdfPCell();
                        supplierPhoneEmailCell.Rowspan = 2;
            
                        Chunk supplierPhoneEmailData = new Chunk("(607) 865-1234 sagahors@gmail.com");
                            supplierPhoneEmailData.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);            

                        supplierPhoneEmailCell.AddElement(supplierPhoneEmailData);
            
                    PdfPCell supplierWebsiteCell = new PdfPCell();
                        supplierWebsiteCell.Colspan = 2;
                        supplierWebsiteCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        supplierWebsiteCell.Border = 0;
                        
                        Paragraph supplierWebsiteData = new Paragraph("Http://www.sagamorepets.com");
                            supplierWebsiteData.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);

                        supplierWebsiteCell.AddElement(supplierWebsiteData);
                                                                               
                    supplierContactTable.AddCell(supplierNameCell);
                    supplierContactTable.AddCell(supplierStreetAddressCell);
                    supplierContactTable.AddCell(supplierPhoneEmailCell);
                    supplierContactTable.AddCell(supplierCityStateZipCell);
                    supplierContactTable.AddCell(supplierWebsiteCell);

                    containerTable.AddCell(supplierContactTable);

                    PdfPCell logoCell = new PdfPCell();
                        logoCell.Border = 0;

                    AddAQLInvoiceLogo(logoCell);

                    containerTable.AddCell(logoCell);

                    document.Add(containerTable);
                    //document.Add(supplierContactTable);
                }

                private void AddAQLInvoiceLogo(PdfPCell cell)
                {                    
                    Image logoImage = Image.GetInstance("wwwroot\\images\\templates\\invoice\\AquaticLife_Logo.png");
                        logoImage.Alignment = Element.ALIGN_RIGHT;
                        logoImage.ScaleAbsoluteWidth(150f);
                        logoImage.ScaleAbsoluteHeight(150f);

                    cell.AddElement(logoImage);
                }

                private Image GetBarcodeImage(PurchaseInvoiceModel invoice)
                {
                    return Image.GetInstance(BarcodeFactory.GenerateBarcode(invoice.PurchaseInvoiceID, BarcodeTypes.Invoice), ImageFormat.Png);
                }

            #endregion

            #region Section: Invoice Purchase Table Generation Methods...

                /// <summary>
            /// Generate a table with headers, line item data, and footer summary rows
            /// </summary>
            /// <param name="purchaseTable">Table to which elements are added</param>
            /// <param name="invoice">Invoice used to render purchase line item data rows</param>
            /// <param name="document">Document to which table is added</param>
                private void AddPurchasesTable(PurchaseInvoiceModel invoice, Document document)
                {
                    //Create a table to hold purchases associated with invoices
                    PdfPTable purchaseTable = new PdfPTable(5);
                        purchaseTable.SetWidths(new float[] { 150f, 20f, 40f, 40f, 75f });

                    //Add header row to purchase table
                    AddInvoicePurchaseTableHeaderRow(purchaseTable);

                    //Generate table rows for each purchase on invoice, Add them to the table
                    invoice.Purchases.ForEach(p => AddInvoicePurchaseDataRow(p, purchaseTable));

                    //Add purchase table summary footer
                    AddPurchaseTableSummaryFooter(purchaseTable, invoice);

                    //Add the table to the PDF document
                    document.Add(purchaseTable);
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
                var tax = (float)invoice.Purchases.Sum(p => p.ExtCost) * .08f;

                Paragraph taxHeaderText = new Paragraph("Tax (8%): ");
                    taxHeaderText.Alignment = Element.ALIGN_RIGHT;

                PdfPCell taxHeader = new PdfPCell();
                    taxHeader.AddElement(taxHeaderText);
                    taxHeader.Colspan = 3;            

                PdfPCell taxValue = new PdfPCell();
                    taxValue.AddElement(new Chunk(tax.ToString("C")));
                    taxValue.Colspan = 2;

                t.AddCell(taxHeader);
                t.AddCell(taxValue);
                        
                var total = ((float)invoice.Purchases.Sum(p => p.ExtCost)) * 1.08f;

                //If invoice purchases were ordered online and shipped, show shipping cost summary row
                if (invoice.IsOnlineOrder)
                {
                    var shipping = (float)invoice.ShippingCost;

                    Paragraph shippingHeaderText = new Paragraph("Shipping: ");
                    shippingHeaderText.Alignment = Element.ALIGN_RIGHT;

                    PdfPCell shippingHeader = new PdfPCell();
                    shippingHeader.AddElement(shippingHeaderText);
                    shippingHeader.Colspan = 3;

                    PdfPCell shippingValue = new PdfPCell();
                    shippingValue.AddElement(new Chunk(shipping.ToString("C")));
                    shippingValue.Colspan = 2;

                    t.AddCell(shippingHeader);
                    t.AddCell(shippingValue);

                    total = total + shipping;
                }                        

                //Total cost summary row           
                Paragraph totalHeaderText = new Paragraph("Invoice Total: ");
                    totalHeaderText.Alignment = Element.ALIGN_RIGHT;

                PdfPCell totalHeader = new PdfPCell();
                    totalHeader.AddElement(totalHeaderText);
                    totalHeader.Colspan = 3;
                        
                PdfPCell totalValue = new PdfPCell();
            
                totalValue.AddElement(new Chunk(total.ToString("C")));
                totalValue.Colspan = 2;

                t.AddCell(totalHeader);
                t.AddCell(totalValue);

            }

                private void InsertDocumentLineBreak(Document document, bool spaceAfter = true, bool spaceBefore = true)
                {
                    Paragraph paragraph = new Paragraph();
                    
                    if (spaceBefore)
                    {
                        paragraph.SpacingBefore = 10;
                    }

                    if (spaceBefore)
                    {
                        paragraph.SpacingAfter = 10;
                    }
                                                    
                    document.Add(paragraph);
                }

            #endregion                    

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