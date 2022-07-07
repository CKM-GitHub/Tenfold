using Models.RealEstate.r_invoice;
using Seruichi.BL.RealEstate.r_invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Models.RealEstate.r_login;
using System.IO;


namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_invoiceController : BaseController
    {
        // GET: r_invoice
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GeneratePDF(r_loginModel model)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 30, 30);

                var header = new r_Invoice_PageEvent();
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfDoc.Open();

                pdfWriter.PageEvent = header;

                //Top Heading
                Chunk chunk = new Chunk("Your Credit Card Statement Report has been Generated", FontFactory.GetFont("Arial", 20, Font.BOLDITALIC, BaseColor.MAGENTA));
                pdfDoc.Add(chunk);

                //Horizontal Line
                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                pdfDoc.Add(line);

                //Table
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                //0=Left, 1=Centre, 2=Right
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;

                //Cell no 1
                PdfPCell cell = new PdfPCell();

                //Cell no 2
                chunk = new Chunk("Name: Mrs. Salma Mukherji,\nAddress: Latham Village, Latham, New York, US, \nOccupation: Nurse, \nAge: 35 years", FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.PINK));
                cell = new PdfPCell();
                cell.Border = 0;
                cell.AddElement(chunk);
                table.AddCell(cell);

                //Add table to document
                pdfDoc.Add(table);

                //Horizontal Line
                line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                pdfDoc.Add(line);

                //Table
                table = new PdfPTable(5);
                table.WidthPercentage = 100;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;

                //Cell
                cell = new PdfPCell();
                chunk = new Chunk("This Month's Transactions of your Credit Card");
                cell.AddElement(chunk);
                cell.Colspan = 5;
                cell.BackgroundColor = BaseColor.PINK;
                table.AddCell(cell);

                table.AddCell("S.No");
                table.AddCell("NYC Junction");
                table.AddCell("Item");
                table.AddCell("Cost");
                table.AddCell("Date");

                table.AddCell("1");
                table.AddCell("David Food Store");
                table.AddCell("Fruits & Vegetables");
                table.AddCell("$100.00");
                table.AddCell("June 1");

                table.AddCell("2");
                table.AddCell("Child Store");
                table.AddCell("Diaper Pack");
                table.AddCell("$6.00");
                table.AddCell("June 9");

                table.AddCell("3");
                table.AddCell("Punjabi Restaurant");
                table.AddCell("Dinner");
                table.AddCell("$29.00");
                table.AddCell("June 15");

                table.AddCell("4");
                table.AddCell("Wallmart Albany");
                table.AddCell("Grocery");
                table.AddCell("$299.50");
                table.AddCell("June 25");

                table.AddCell("5");
                table.AddCell("Singh Drugs");
                table.AddCell("Back Pain Tablets");
                table.AddCell("$14.99");
                table.AddCell("June 28");

                pdfDoc.Add(table);

                Paragraph para = new Paragraph();
                para.Add("Hello Salma,\n\nThank you for being our valuable customer. We hope our letter finds you in the best of health and wealth.\n\nYours Sincerely, \nBank of America");
                pdfDoc.Add(para);

                //Horizontal Line
                line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                pdfDoc.Add(line);

                para = new Paragraph();
                para.Add("This PDF is generated using iTextSharp. You can read the turorial:");
                para.SpacingBefore = 20f;
                para.SpacingAfter = 20f;
                pdfDoc.Add(para);

                //Creating link
                chunk = new Chunk("How to Create a Pdf File");
                chunk.Font = FontFactory.GetFont("Arial", 25, Font.BOLD, BaseColor.RED);
                chunk.SetAnchor("https://www.yogihosting.com/create-pdf-asp-net-mvc/");
                pdfDoc.Add(chunk);

                
                pdfDoc.Close();

                pdfWriter.Close();
               
                Response.Clear();
                Response.ClearContent();
                Response.Write(Convert.ToBase64String(ms.ToArray()));
                Response.Flush();
                Response.End();
            }
            return View();
        }
        

        ////[HttpPost]
        ////public ActionResult Get_D_Billing_List(r_invoiceModel model)
        ////{
        ////    r_invoiceBL bl = new r_invoiceBL();
        ////    var validationResult = bl.ValidateAll(model);
        ////    if (validationResult.Count > 0)
        ////    {
        ////        return ErrorResult(validationResult);
        ////    }
        ////    var dt = bl.Get_D_Billing_List(model);
        ////    return OKResult(DataTableToJSON(dt));
        ////}
    }
}