using Models.RealEstate.r_invoice;
using Models.RealEstate.r_login;
using Seruichi.BL.RealEstate.r_invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using Seruichi.Common;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_invoiceController : BaseController
    {
        // GET: r_invoice
        public ActionResult Index()
        {
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            if (!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "r_login");
            }
            ViewBag.IsInvoice = user.PermissionInvoice.ToString();
            ViewBag.IsAdmin   = user.REStaffCD;
            return View();
        }
        [HttpPost]
        public ActionResult GeneratePDF(r_loginModel model)
        {
            r_invoiceBL bl = new r_invoiceBL();

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();

            PDF_Font font_Class = new PDF_Font();
            string font_folder = Server.MapPath("~/Content/fonts/");
            Font normal_font_10 = font_Class.Normal_Font_10(font_folder);
            Font bold_font_16_white = font_Class.bold_font_16_white(font_folder);
            Font normal_font_9 = font_Class.Normal_Font_10(font_folder);
            Font bold_font_9 = font_Class.Bold_Font_9(font_folder);
            Font bold_font_9_white = font_Class.Bold_Font_9_White(font_folder);
            Font bold_font_8 = font_Class.Bold_Font_8(font_folder);
            Font normal_font_8 = font_Class.Normal_Font_8(font_folder);

            DataTable dt_M_RealEstate_pdfheader = bl.Get_M_RealEstate_For_PDFHeader(model.RealECD);
            DataTable dt_D_Billing_For_PDFHeader = bl.Get_D_Billing_For_PDFHeader(model.UserName);
            DataTable dt_M_Control_For_PDFHeader = bl.Get_M_Control_For_PDFHeader();
            DataTable dt_M_Image_For_PDFHeader = bl.Get_M_Image_For_PDFHeader();
            float header_total_height = 0;

            DataTable dt_Service_Registration_Block_A = bl.Get_Service_Registration_Block_A(model.RealECD, dt_D_Billing_For_PDFHeader.Rows[0]["P年月"].ToString());
            DataTable dt_Contract_Area_Block_B = bl.Get_Contract_Area_Block_B(model.RealECD, dt_D_Billing_For_PDFHeader.Rows[0]["P年月"].ToString());
            DataTable dt_Contract_Apartments_Block_C = bl.Get_Contract_Apartments_Block_C(model.RealECD, dt_D_Billing_For_PDFHeader.Rows[0]["P年月"].ToString());
            DataTable dt_Customer_Send_Record_Block_D = bl.Get_Customer_Send_Record_Block_D(model.RealECD, dt_D_Billing_For_PDFHeader.Rows[0]["P年月"].ToString());
            DataTable dt_Customer_Transfer_Record_Block_E = bl.Get_Customer_Transfer_Record_Block_E(model.RealECD, dt_D_Billing_For_PDFHeader.Rows[0]["P年月"].ToString());
            DataTable dt_Get_Customer_Cancel_Record_Block_F = bl.Get_Customer_Cancel_Record_Block_F(model.RealECD, dt_D_Billing_For_PDFHeader.Rows[0]["P年月"].ToString());

            DataTable dt_Get_M_MultPurpose_for_Footer = bl.Get_M_MultPurpose_for_Footer();

            using (MemoryStream ms = new MemoryStream())
            {
               
                using (Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 10))
                {
                   
                    var header = new r_Invoice_PageEvent();
                    // pdfDoc.SetMargins(25, 25, 217, 85);
                    pdfDoc.SetMargins(25, 25, 240, 100); 

                    using (PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms))
                    {
                        pdfDoc.Open();
                        pdfWriter.PageEvent = header;

                        header.normal_font_10 = normal_font_10;
                        header.bold_font_16_white = bold_font_16_white;
                        header.normal_font_9 = normal_font_9;
                        header.bold_font_9 = bold_font_9;
                        header.bold_font_8 = bold_font_8;
                        header.normal_font_8 = normal_font_8;
                        header.bold_font_9_white = bold_font_9_white;

                        header.dt_M_RealEstate_pdfheader = dt_M_RealEstate_pdfheader;
                        header.dt_D_Billing_For_PDFHeader = dt_D_Billing_For_PDFHeader;
                        header.dt_M_Control_For_PDFHeader = dt_M_Control_For_PDFHeader;
                        header.dt_M_Image_For_PDFHeader = dt_M_Image_For_PDFHeader;

                        header.dt_Get_M_MultPurpose_for_Footer = dt_Get_M_MultPurpose_for_Footer;

                        header_total_height = header.header_total_height;

                        int row_count = 0;
                        header.block_all_total = 0;                        

                        PdfPTable table = new PdfPTable(5);
                        table.HorizontalAlignment = 1;
                        table.TotalWidth = 545f;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 90f, 210f, 65f, 90f, 90f };
                        table.SetWidths(widths);
                       // table.HeaderRows = 1;

                        PdfPCell pdfCell = new PdfPCell();

                       
                        //start Service_Registration_Block_A
                        if (dt_Service_Registration_Block_A.Rows.Count > 0)
                        {
                            // Title 
                            row_count += Title_Block(table, dt_Service_Registration_Block_A.Rows[0]["ServiceRegistration"].ToString(), bold_font_9);

                            // Body
                            string title = dt_Service_Registration_Block_A.Rows[0]["ContractName"].ToString();
                            string times = dt_Service_Registration_Block_A.Rows[0]["times"].ToString();
                            string unitPrice = dt_Service_Registration_Block_A.Rows[0]["UnitPrice"].ToString();
                            string Amount = dt_Service_Registration_Block_A.Rows[0]["Amount"].ToString();
                            row_count += Body_Block(table, title, times, unitPrice, Amount, normal_font_9);

                            //sub Total
                            string sub_total = dt_Service_Registration_Block_A.Rows[0]["subTotal"].ToString();
                            row_count += Sub_Total(table, sub_total, bold_font_9, normal_font_9);

                            header.block_all_total += Convert.ToInt32(sub_total);

                        }
                        //enf Service_Registration_Block_A

                        //start Contract_Area_Block_B
                        if (dt_Contract_Area_Block_B.Rows.Count > 0)
                        {

                            // Title 
                            row_count += Title_Block(table, dt_Contract_Area_Block_B.Rows[0]["ContractArea"].ToString(), bold_font_9);

                            int sub_amt = 0;
                            // Body
                            for (int i = 0; i < dt_Contract_Area_Block_B.Rows.Count; i++)
                            {
                                string title = dt_Contract_Area_Block_B.Rows[i]["ContractName"].ToString();
                                string times = dt_Contract_Area_Block_B.Rows[i]["times"].ToString();
                                string unitPrice = dt_Contract_Area_Block_B.Rows[i]["UnitPrice"].ToString();
                                string Amount = dt_Contract_Area_Block_B.Rows[i]["Amount"].ToString();
                                sub_amt += Convert.ToInt32(Amount.Replace(",", string.Empty));
                                row_count += Body_Block(table, title, times, unitPrice, Amount, normal_font_9);
                            }


                            //sub Total
                            int sub_total = sub_amt;
                            row_count += Sub_Total(table, String.Format("{0:n0}", sub_total), bold_font_9, normal_font_9);
                            header.block_all_total += sub_total;
                        }
                        // enf Contract_Area_Block_B

                        //start Contract_Apartments_Block_C
                        if (dt_Contract_Apartments_Block_C.Rows.Count > 0)
                        {

                            // Title 
                            row_count += Title_Block(table, dt_Contract_Apartments_Block_C.Rows[0]["Apartments_Num"].ToString(), bold_font_9);

                            int sub_amt = 0;
                            // Body
                            for (int i = 0; i < dt_Contract_Apartments_Block_C.Rows.Count; i++)
                            {
                                string title = dt_Contract_Apartments_Block_C.Rows[i]["ContractName"].ToString();
                                string times = dt_Contract_Apartments_Block_C.Rows[i]["times"].ToString();
                                string unitPrice = dt_Contract_Apartments_Block_C.Rows[i]["UnitPrice"].ToString();
                                string Amount = dt_Contract_Apartments_Block_C.Rows[i]["Amount"].ToString();
                                sub_amt += Convert.ToInt32(Amount.Replace(",", string.Empty));
                                row_count += Body_Block(table, title, times, unitPrice, Amount, normal_font_9);
                            }


                            //sub Total
                            int sub_total = sub_amt;
                            row_count += Sub_Total(table, String.Format("{0:n0}", sub_total), bold_font_9, normal_font_9);

                            header.block_all_total += sub_total;
                        }
                        // enf Contract_Apartments_Block_C

                        int block_D_E_F_Sub_Total = 0;

                        //start Customer_Send_Record_Block_D
                        if (dt_Customer_Send_Record_Block_D.Rows.Count > 0)
                        {

                            // Title 
                            row_count += Title_Block(table, dt_Customer_Send_Record_Block_D.Rows[0]["SendRecord"].ToString(), bold_font_9);

                            // Title 
                            row_count += Title_Block(table, dt_Customer_Send_Record_Block_D.Rows[0]["SendFrame"].ToString(), bold_font_9, "10f");


                            // Body
                            for (int i = 0; i < dt_Customer_Send_Record_Block_D.Rows.Count; i++)
                            {
                                string date = Beginning_Data(dt_Customer_Send_Record_Block_D.Rows[i]["Date"].ToString(), 10);
                                string SellerName = Beginning_Data(crypt.DecryptFromBase64(dt_Customer_Send_Record_Block_D.Rows[i]["SellerName"].ToString(), decryptionKey), 10);
                                string mansionName = Beginning_Data(dt_Customer_Send_Record_Block_D.Rows[i]["MansionName"].ToString(), 40);
                                string title = date + " " + SellerName + " " + mansionName;
                                string times = dt_Customer_Send_Record_Block_D.Rows[i]["times"].ToString();
                                string unitPrice = dt_Customer_Send_Record_Block_D.Rows[i]["UnitPrice"].ToString();
                                string Amount = dt_Customer_Send_Record_Block_D.Rows[i]["Amount"].ToString();

                                block_D_E_F_Sub_Total += Convert.ToInt32(Amount.Replace(",", string.Empty));

                                row_count += Body_Block(table, title, times, unitPrice, Amount, normal_font_9, "10f");
                            }
                        }
                        // enf Customer_Send_Record_Block_D

                        //start Customer_Transfer_Record_Block_E
                        if (dt_Customer_Transfer_Record_Block_E.Rows.Count > 0)
                        {

                            // Title 
                            if (dt_Customer_Send_Record_Block_D.Rows.Count == 0)
                                row_count += Title_Block(table, dt_Customer_Transfer_Record_Block_E.Rows[0]["DeliveryRecord"].ToString(), bold_font_9);

                            // Title 
                            row_count += Title_Block(table, dt_Customer_Transfer_Record_Block_E.Rows[0]["SendAdditional"].ToString(), bold_font_9, "10f");


                            // Body
                            for (int i = 0; i < dt_Customer_Transfer_Record_Block_E.Rows.Count; i++)
                            {
                                string date = Beginning_Data(dt_Customer_Transfer_Record_Block_E.Rows[i]["Date"].ToString(), 10);
                                string SellerName = Beginning_Data(crypt.DecryptFromBase64(dt_Customer_Transfer_Record_Block_E.Rows[i]["SellerName"].ToString(), decryptionKey), 10);
                                string mansionName = Beginning_Data(dt_Customer_Transfer_Record_Block_E.Rows[i]["MansionName"].ToString(), 40);
                                string title = date + " " + SellerName + " " + mansionName;
                                string times = dt_Customer_Transfer_Record_Block_E.Rows[i]["times"].ToString();
                                string unitPrice = dt_Customer_Transfer_Record_Block_E.Rows[i]["UnitPrice"].ToString();
                                string Amount = dt_Customer_Transfer_Record_Block_E.Rows[i]["Amount"].ToString();

                                block_D_E_F_Sub_Total += Convert.ToInt32(Amount.Replace(",", string.Empty));

                                row_count += Body_Block(table, title, times, unitPrice, Amount, normal_font_9,"10f");
                            }
                        }
                        // enf Customer_Transfer_Record_Block_E

                        //start Customer_Cancel_Record_Block_F
                        if (dt_Get_Customer_Cancel_Record_Block_F.Rows.Count > 0)
                        {

                            // Title 
                            if (dt_Customer_Send_Record_Block_D.Rows.Count == 0 && dt_Customer_Transfer_Record_Block_E.Rows.Count == 0)
                                row_count += Title_Block(table, dt_Get_Customer_Cancel_Record_Block_F.Rows[0]["DeliveryRecord"].ToString(), bold_font_9);

                            // Title 
                            row_count += Title_Block(table, dt_Get_Customer_Cancel_Record_Block_F.Rows[0]["Cancellation"].ToString(), bold_font_9, "10f");


                            // Body
                            for (int i = 0; i < dt_Get_Customer_Cancel_Record_Block_F.Rows.Count; i++)
                            {
                                string date = Beginning_Data(dt_Get_Customer_Cancel_Record_Block_F.Rows[i]["Date"].ToString(), 10);
                                string SellerName = Beginning_Data(crypt.DecryptFromBase64(dt_Get_Customer_Cancel_Record_Block_F.Rows[i]["SellerName"].ToString(), decryptionKey), 10);
                                string mansionName = Beginning_Data(dt_Get_Customer_Cancel_Record_Block_F.Rows[i]["MansionName"].ToString(), 40);
                                string title = date + " " + SellerName + " " + mansionName;
                                string times = dt_Get_Customer_Cancel_Record_Block_F.Rows[i]["times"].ToString();
                                string unitPrice = dt_Get_Customer_Cancel_Record_Block_F.Rows[i]["UnitPrice"].ToString();
                                string Amount = dt_Get_Customer_Cancel_Record_Block_F.Rows[i]["Amount"].ToString();

                                block_D_E_F_Sub_Total += Convert.ToInt32(Amount.Replace(",", string.Empty));

                                row_count += Body_Block(table, title, times, unitPrice, Amount, normal_font_9, "10f");
                            }
                        }
                        // enf Customer_Cancel_Record_Block_F
                        if (dt_Customer_Send_Record_Block_D.Rows.Count != 0 || dt_Customer_Transfer_Record_Block_E.Rows.Count != 0 || dt_Get_Customer_Cancel_Record_Block_F.Rows.Count != 0)
                            row_count += Sub_Total(table, String.Format("{0:n0}", block_D_E_F_Sub_Total), bold_font_9, normal_font_9);

                        header.block_all_total += block_D_E_F_Sub_Total;

                        if (row_count < 34) //row count less than 35
                        {
                            for (int k = row_count; k < 34; k++)
                            {
                                AddNullRow_To_Table(table, k);
                            }
                        }
                        else //row count greater than 35
                        {
                            int j = row_count / 33;
                            int i = row_count % 33;
                            if (i != 0)
                            {
                                while(i<33)
                                {
                                    AddNullRow_To_Table(table, i);
                                    i++;
                                }
                            }
                        }
                        header.content_table_row_count = table.Rows.Count;
                       
                       // AddLastRow_To_Table(table, String.Format("{0:n0}", header.block_all_total), bold_font_9, normal_font_9);

                        

                        // table.WriteSelectedRows(0, -1, 25, pdfDoc.Top - 227, pdfWriter.DirectContent);

                        pdfDoc.Add(table);

                        pdfDoc.Close();

                        pdfWriter.Close();

                        Response.Clear();
                        Response.ClearContent();
                        Response.Write(Convert.ToBase64String(ms.ToArray()));
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            return View();
        }


        [HttpPost]
        public ActionResult Get_D_Billing_List(r_invoiceModel model)
        {
            r_invoiceBL bl = new r_invoiceBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            model.RealECD = user.RealECD;
            var dt = bl.Get_D_Billing_List(model);
            return OKResult(DataTableToJSON(dt));
        }

        public int Title_Block(PdfPTable table, string title_text,Font font_size, [Optional] string padding)
        {
            int row_count = 0;

            PdfPCell pdfCell = new PdfPCell();


            pdfCell = new PdfPCell(new Phrase(title_text, font_size));
            pdfCell.Colspan = 2;
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.BorderWidthBottom = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            pdfCell.BorderWidthTop = 0;
            pdfCell.FixedHeight = 15f;
            if(!string.IsNullOrEmpty(padding))
                pdfCell.PaddingLeft= 10f;
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(""));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.BorderWidthBottom = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            pdfCell.BorderWidthTop = 0;
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(""));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.BorderWidthBottom = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            pdfCell.BorderWidthTop = 0;
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(""));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthBottom = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            pdfCell.BorderWidthTop = 0;
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table.AddCell(pdfCell);
            row_count += 1;

            return row_count;

        }

        public int Body_Block(PdfPTable table, string title_text,string times,string unitPrice,string Amount, Font font_size,[Optional] string padding)
        {
            int row_count = 0;

            PdfPCell pdfCell = new PdfPCell();
            double temp_row = table.Rows.Count <= 32 ? 32 : (table.Rows.Count/32*32)+ (table.Rows.Count / 32 -1);

            pdfCell = new PdfPCell(new Phrase(title_text, font_size));
            pdfCell.Colspan = 2;
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.PaddingLeft = string.IsNullOrEmpty(padding) ? 10f : 20f;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.BorderWidthTop = 0;
            if (table.Rows.Count == temp_row)
            {
                pdfCell.BorderWidthBottom = 0;
            }
            else
            {
                pdfCell.BorderWidthBottom = 0;
                pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            }
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(times, font_size));
            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.BorderWidthTop = 0;
            if (table.Rows.Count == temp_row)
            {
                pdfCell.BorderWidthBottom = 0;
            }
            else
            {
                pdfCell.BorderWidthBottom = 0;
                pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            }
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(unitPrice, font_size));
            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.BorderWidthTop = 0;
            if (table.Rows.Count == temp_row)
            {
                pdfCell.BorderWidthBottom = 0;
            }
            else
            {
                pdfCell.BorderWidthBottom = 0;
                pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            }
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(Amount, font_size));
            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthTop = 0;
            if (table.Rows.Count == temp_row)
            {
                pdfCell.BorderWidthBottom = 0;
            }
            else
            {
                pdfCell.BorderWidthBottom = 0;
                pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            }
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table.AddCell(pdfCell);
            


            row_count += 1;

            return row_count;

        }

        public int Sub_Total(PdfPTable table,string sub_total,Font font_size,Font font_size1)
        {
            int row_count = 0;

            PdfPCell pdfCell = new PdfPCell();

            pdfCell = new PdfPCell(new Phrase("小　　　　計", font_size));
            pdfCell.Colspan = 2;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.BorderWidthTop = 0;
            pdfCell.BorderWidthBottom = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            pdfCell.FixedHeight = 15f;
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("", font_size1));
            pdfCell.Colspan = 2;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.BorderWidthTop = 0;
            pdfCell.BorderWidthBottom = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(sub_total, font_size1));
            pdfCell.Colspan = 2;
            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthTop = 0;
            pdfCell.BorderWidthBottom = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table.AddCell(pdfCell);

            row_count += 1;

            return row_count;
        }

        public string Beginning_Data(string ori_text, int len)
        {
            Encoding encoding = Encoding.GetEncoding("Shift_JIS");
            int length = encoding.GetByteCount(ori_text);
            string beg_text = ori_text;
            if (length > 10)
            {
                beg_text = beg_text.Substring(0, len/2);
            }
            return beg_text;
        }

        public static void AddNullRow_To_Table(PdfPTable table,int row_count)
        {
            PdfPCell pdfCell = new PdfPCell();


            pdfCell = new PdfPCell(new Phrase(""));
            pdfCell.Colspan = 2;
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.BorderWidthTop = 0;
            if(row_count % 32 != 0)
            {
                pdfCell.BorderWidthBottom = 0;
                pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            }
            else
            {
                pdfCell.BorderWidthBottom = 0;
            }            
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(""));
            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.BorderWidthTop = 0;
            if (row_count % 32 != 0)
            {
                pdfCell.BorderWidthBottom = 0;
                pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            }
            else
            {
                pdfCell.BorderWidthBottom = 0;
            }
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(""));
            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.BorderWidthTop = 0;
            if (row_count % 32 != 0)
            {
                pdfCell.BorderWidthBottom = 0;
                pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            }
            else
            {
                pdfCell.BorderWidthBottom = 0;
            }
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(""));
            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthTop = 0;
            if (row_count % 32 != 0)
            {
                pdfCell.BorderWidthBottom = 0;
                pdfCell.CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER);
            }
            else
            {
                pdfCell.BorderWidthBottom = 0;
            }
            table.AddCell(pdfCell);
        }

        public static void AddLastRow_To_Table(PdfPTable table,string total, Font font_size, Font font_size1)
        {
            PdfPCell pdfCell = new PdfPCell();

            pdfCell = new PdfPCell(new Phrase("合　　　　計", font_size));
            pdfCell.Colspan = 2;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.FixedHeight = 15f;
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("", font_size1));
            pdfCell.Colspan = 2;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(total, font_size1));
            pdfCell.Colspan = 2;
            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table.AddCell(pdfCell);
        }

    }
}