using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Models.RealEstate.r_invoice
{
    public class r_Invoice_PageEvent: PdfPageEventHelper
    {
        private Encoding encoding = Encoding.GetEncoding("Shift_JIS");

        public int page_Number = 0;
        public Font normal_font_10 { get; set; }
        public Font bold_font_16_white { get; set; }
        public Font normal_font_9 { get; set; }
        public Font bold_font_9 { get; set; }
        public Font bold_font_9_white { get; set; }
        public Font bold_font_8 { get; set; }
        public Font normal_font_8 { get; set; }
        public DataTable dt_M_RealEstate_pdfheader { get; set; }
        public DataTable dt_D_Billing_For_PDFHeader { get; set; }
        public DataTable dt_M_Control_For_PDFHeader { get; set; }
        public DataTable dt_M_Image_For_PDFHeader { get; set; }
        public float header_total_height { get; set; }
        public DataTable dt_Get_M_MultPurpose_for_Footer { get; set; }
        public int block_all_total = 0;
        public int content_table_row_count = 0;

        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        //BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;

        #region Fields
        private string _header;
        #endregion

        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion

        //public override void OnOpenDocument(PdfWriter writer, Document document)
        //{
        //    try
        //    {
        //        //PrintTime = DateTime.Now;
        //        //bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //        cb = writer.DirectContent;
        //        headerTemplate = cb.CreateTemplate(100, 100);
        //        footerTemplate = cb.CreateTemplate(50, 50);
        //    }
        //    catch (DocumentException de)
        //    {
        //    }
        //    catch (System.IO.IOException ioe)
        //    {
        //    }
        //}

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            page_Number = page_Number + 1; 

            base.OnEndPage(writer, document);

            //PdfContentByte content = writer.DirectContent;
            //Rectangle rectangle = new Rectangle(document.PageSize);
            //rectangle.Left += document.LeftMargin;
            //rectangle.Right -= document.RightMargin;
            //rectangle.Top -= document.TopMargin;
            //rectangle.Bottom += document.BottomMargin;
            //rectangle.BorderWidthBottom = 2f;
            //rectangle.BorderWidthLeft = 2f;
            //rectangle.BorderWidthRight = 2f;
            //content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
            //content.SetLineWidth(2);
            ////content.EoFill();
            //content.Stroke();

            //PdfPTable pdf_Main = new PdfPTable(1);
            //pdf_Main.HorizontalAlignment = 1;
            //pdf_Main.TotalWidth = 545f;
            //float[] _widths_Main = new float[] { 545f };
            //pdf_Main.SetWidths(_widths_Main);


            //Create PdfTable object
            PdfPTable pdfTab = new PdfPTable(4);
            pdfTab.HorizontalAlignment = 1;
            pdfTab.TotalWidth = 545f;
            pdfTab.LockedWidth = true;
            float[] _widths = new float[] { 150f, 100f, 170f,125f };
            pdfTab.SetWidths(_widths);

            float first_table_height = 0;
            float second_table_height = 0;
            float third_table_height = 0;
            


            PdfPCell pdfCell = new PdfPCell();
            
            //start 1
            pdfCell = new PdfPCell(new Phrase(dt_D_Billing_For_PDFHeader.Rows[0]["InvoiceYYYYMM"].ToString(), normal_font_10));            
            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.FixedHeight = 20f;
            pdfCell.BorderWidth = 0;
            pdfTab.AddCell(pdfCell);

            var c = new Chunk("　　　請　求　書　　　", bold_font_16_white);
            c.SetBackground(BaseColor.BLACK);
            Phrase p2 = new Phrase(c);
            pdfCell = new PdfPCell(p2);
            pdfCell.Colspan = 2;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.FixedHeight = 20f;
            pdfCell.BorderWidth = 0;
            pdfTab.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_D_Billing_For_PDFHeader.Rows[0]["InsertDateTime"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.FixedHeight = 20f;
            pdfCell.BorderWidth = 0;
            pdfTab.AddCell(pdfCell);
            first_table_height += pdfCell.FixedHeight;
            //end 1

            // start 2
            
            pdfCell = new PdfPCell(new Phrase("〒" + dt_M_RealEstate_pdfheader.Rows[0]["ZipCode1"]+ "-"+ dt_M_RealEstate_pdfheader.Rows[0]["ZipCode2"], normal_font_10));
            pdfCell.Colspan = 3;
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.FixedHeight = 14f;
            pdfCell.BorderWidth = 0;
            pdfTab.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("page-" + page_Number.ToString("D3"), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.FixedHeight = 14f;
            pdfCell.BorderWidth = 0;
            pdfTab.AddCell(pdfCell);
            first_table_height += pdfCell.FixedHeight;
            //end 2

            // start 3
            pdfCell = new PdfPCell(new Phrase(dt_M_RealEstate_pdfheader.Rows[0]["address1"].ToString(), normal_font_10));
            pdfCell.Colspan = 3;
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.FixedHeight = 14f;
            pdfCell.BorderWidth = 0;
            pdfTab.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_D_Billing_For_PDFHeader.Rows[0]["InvoiceNo"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.FixedHeight = 14f;
            pdfCell.BorderWidth = 0;
            pdfCell.BorderWidthBottom = 0.1f;
            pdfTab.AddCell(pdfCell);
            first_table_height += pdfCell.FixedHeight;
            //end 3

            // start 4
            pdfCell = new PdfPCell(new Phrase(dt_M_RealEstate_pdfheader.Rows[0]["address2"].ToString(), normal_font_9));
            pdfCell.Colspan = 3;
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.FixedHeight = 14f;
            pdfCell.BorderWidth = 0;
            pdfTab.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(""));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.FixedHeight = 14f;
            pdfCell.BorderWidth = 0;
            pdfTab.AddCell(pdfCell);
            first_table_height += pdfCell.FixedHeight;
            //end 4

            //Create PdfTable object
            PdfPTable pdfTab_s = new PdfPTable(2);
            pdfTab_s.HorizontalAlignment = 1;
            pdfTab_s.TotalWidth = 545f;
            pdfTab_s.LockedWidth = true;
            float[] _widths_s = new float[] { 300f, 245f };
            pdfTab_s.SetWidths(_widths_s);

            // start 1,2,3
            string REname = dt_M_RealEstate_pdfheader.Rows[0]["REName"].ToString();
            string strREname = String.Empty;
            int length = encoding.GetByteCount(REname);
            if (length > 40)
            {
                string first = REname.Substring(0, 20);
                string last = REname.Substring(21, REname.Length-21);
                strREname = first + "\r\n" + last;
            }
            else
                strREname = REname;
            pdfCell = new PdfPCell(new Phrase(strREname + "  御中", normal_font_10));
            pdfCell.Rowspan = 3;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfCell.BorderWidth = 0;
            pdfCell.BorderWidthBottom = 0.1f;
            pdfCell.PaddingBottom = 2f;
            pdfTab_s.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_M_Control_For_PDFHeader.Rows[0]["InvoicePrint1"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BorderWidth = 0;
            pdfCell.PaddingLeft = 5f;
            pdfCell.FixedHeight = 14f;
            second_table_height += pdfCell.FixedHeight;
            pdfTab_s.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_M_Control_For_PDFHeader.Rows[0]["InvoicePrint2"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BorderWidth = 0;
            pdfCell.PaddingLeft = 5f;
            pdfCell.FixedHeight = 14f;
            second_table_height += pdfCell.FixedHeight;
            pdfTab_s.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_M_Control_For_PDFHeader.Rows[0]["InvoicePrint3"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BorderWidth = 0;
            pdfCell.PaddingLeft = 5f;
            pdfCell.FixedHeight = 14f;
            second_table_height += pdfCell.FixedHeight;
            pdfTab_s.AddCell(pdfCell);
            //end 1,2,3

            // start 4
            pdfCell = new PdfPCell(new Phrase("お客様コード"+ dt_M_RealEstate_pdfheader.Rows[0]["UserCode"].ToString(), normal_font_8));           
            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BorderWidth = 0;
            pdfTab_s.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_M_Control_For_PDFHeader.Rows[0]["InvoicePrint4"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BorderWidth = 0;
            pdfCell.PaddingLeft = 5f;
            pdfCell.FixedHeight = 14f;
            second_table_height += pdfCell.FixedHeight;
            pdfTab_s.AddCell(pdfCell);
            //end 4

            // start 5
            pdfCell = new PdfPCell(new Phrase(""));
            pdfCell.BorderWidth = 0;
            pdfTab_s.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_M_Control_For_PDFHeader.Rows[0]["InvoicePrint5"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BorderWidth = 0;
            pdfCell.PaddingLeft = 5f;
            pdfCell.FixedHeight = 14f;
            second_table_height += pdfCell.FixedHeight;
            pdfTab_s.AddCell(pdfCell);
            //end 5

            // start 6
            pdfCell = new PdfPCell(new Phrase("ご利用ありがとうございます。以下の通りご請求申し上げます。",normal_font_9));
            pdfCell.Rowspan = 2;
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BorderWidth = 0;
            pdfTab_s.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_M_Control_For_PDFHeader.Rows[0]["InvoicePrint6"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BorderWidth = 0;
            pdfCell.PaddingLeft = 5f;
            pdfCell.FixedHeight = 14f;
            second_table_height += pdfCell.FixedHeight;
            pdfTab_s.AddCell(pdfCell);
            //end 6

            // start 7
            pdfCell = new PdfPCell(new Phrase(dt_M_Control_For_PDFHeader.Rows[0]["InvoicePrint7"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BorderWidth = 0;
            pdfCell.PaddingLeft = 5f;
            pdfCell.FixedHeight = 14f;
            second_table_height += pdfCell.FixedHeight;
            pdfTab_s.AddCell(pdfCell);
            //end 7

            //Create PdfTable object
            PdfPTable pdfTab_t = new PdfPTable(5);
            pdfTab_t.HorizontalAlignment = 1;
            pdfTab_t.TotalWidth = 400f;
            pdfTab_t.LockedWidth = true;
            float[] _widths_t = new float[] { 80f, 80f, 80f, 80f, 80f };
            pdfTab_t.SetWidths(_widths_t);

            // start 1,2
            pdfCell = new PdfPCell(new Phrase("前回ご請求額", bold_font_8));
            pdfCell.Rowspan = 2;
            pdfCell.FixedHeight = 28f;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;           
            pdfTab_t.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("ご入金額", bold_font_8));
            pdfCell.Rowspan = 2;
            pdfCell.FixedHeight = 28f;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfTab_t.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("繰越金額", bold_font_8));
            pdfCell.Rowspan = 2;
            pdfCell.FixedHeight = 28f;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfTab_t.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("合計", bold_font_8));
            pdfCell.FixedHeight = 14f;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfTab_t.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("今回ご請求額", bold_font_8));
            pdfCell.FixedHeight = 14f;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfTab_t.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("内訳（本体/税）", bold_font_8));
            pdfCell.FixedHeight = 14f;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfTab_t.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("税率(10%)", bold_font_8));
            pdfCell.FixedHeight = 14f;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            third_table_height += 28;
            pdfTab_t.AddCell(pdfCell);
            //end 1,2

            // start 3,4
            pdfCell = new PdfPCell(new Phrase(dt_D_Billing_For_PDFHeader.Rows[0]["TotalPrevAmount"].ToString(), normal_font_9));
            pdfCell.Rowspan = 2;
            pdfCell.FixedHeight = 28f;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfTab_t.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_D_Billing_For_PDFHeader.Rows[0]["CreditedAmount"].ToString(), normal_font_9));
            pdfCell.Rowspan = 2;
            pdfCell.FixedHeight = 28f;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfTab_t.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_D_Billing_For_PDFHeader.Rows[0]["RemainingAmount"].ToString(), normal_font_9));
            pdfCell.Rowspan = 2;
            pdfCell.FixedHeight = 28f;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfTab_t.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_D_Billing_For_PDFHeader.Rows[0]["BillingAmount"].ToString(), normal_font_9));
            pdfCell.Rowspan = 2;
            pdfCell.FixedHeight = 28f;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfTab_t.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_D_Billing_For_PDFHeader.Rows[0]["BaseAmount"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.PaddingBottom = 2f;
            pdfCell.PaddingTop = 0f;
            pdfCell.FixedHeight = 14f;
            pdfTab_t.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_D_Billing_For_PDFHeader.Rows[0]["TaxAmount"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.PaddingBottom = 2f;
            pdfCell.PaddingTop = 0f;
            pdfCell.FixedHeight = 14f;            
            pdfTab_t.AddCell(pdfCell);

            third_table_height += 28;
            //end 3,4

            //Create PdfTable object
            PdfPTable pdfTab_f = new PdfPTable(1);
            pdfTab_f.HorizontalAlignment = 1;
            pdfTab_f.TotalWidth = 125f;
            pdfTab_f.LockedWidth = true;
            float[] _widths_f = new float[] { 125f };
            pdfTab_f.SetWidths(_widths_f);
            pdfTab_f.DefaultCell.BorderColor = BaseColor.RED;
            pdfTab_f.DefaultCell.Border = Rectangle.BOX;


            //start 1,2

            pdfCell = new PdfPCell(new Phrase("今回ご請求額", bold_font_9_white));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.FixedHeight = 28f;
            pdfCell.BackgroundColor = BaseColor.BLACK;
            pdfTab_f.AddCell(pdfCell);

            RoundRectangle rr = new RoundRectangle();
            PdfPCell cell = new PdfPCell()
            {
                CellEvent = rr, // rr is RoundRectangle object
                //Border = PdfPCell.NO_BORDER,
               // Padding = 4,
                Phrase = new Phrase(dt_D_Billing_For_PDFHeader.Rows[0]["TotaAmount"].ToString(), bold_font_9)
            };

            cell.BackgroundColor = BaseColor.WHITE;
            cell.BorderColor = BaseColor.BLACK;
            cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;

            cell.BorderWidthBottom = 0.5f;
            cell.BorderWidthLeft = 0.5f;
            cell.BorderWidthRight = 0.5f;

            cell.FixedHeight = 28f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_CENTER;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 5f;
            pdfTab_f.AddCell(cell);
            //end 1,2


            //create first row of table content
            #region 
            PdfPTable table_header = new PdfPTable(5);
            table_header.HorizontalAlignment = 1;
            table_header.TotalWidth = 545f;
            table_header.LockedWidth = true;
            float[] widths_header = new float[] { 90f, 210f, 65f, 90f, 90f };
            table_header.SetWidths(widths_header);


            pdfCell = new PdfPCell(new Phrase(""));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.FixedHeight = 20f;
            table_header.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(""));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.FixedHeight = 20f;
            table_header.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("数量", bold_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.FixedHeight = 20f;
            table_header.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("単価", bold_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.FixedHeight = 20f;
            table_header.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("金額(税抜", bold_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.FixedHeight = 20f;
            table_header.AddCell(pdfCell);

            #endregion

            //create last row of table content
            #region 
            PdfPTable table_total = new PdfPTable(5);
            table_total.HorizontalAlignment = 1;
            table_total.TotalWidth = 545f;
            table_total.LockedWidth = true;
            float[] widths = new float[] { 90f, 210f, 65f, 90f, 90f };
            table_total.SetWidths(widths);

            string total_string = "";
            string total_value = "";

            if (page_Number == content_table_row_count / 33)
            {
                total_string = "合　　　　計";
                total_value = String.Format("{0:n0}", block_all_total);
            }
                

            pdfCell = new PdfPCell(new Phrase(total_string, bold_font_9));
            pdfCell.Colspan = 2;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.FixedHeight = 15f;
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table_total.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("", normal_font_9));
            pdfCell.Colspan = 2;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.BorderWidthRight = 0;
            pdfCell.CellEvent = new DottedCell(Rectangle.RIGHT_BORDER);
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table_total.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(total_value, normal_font_9));
            pdfCell.Colspan = 2;
            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfCell.FixedHeight = 15f;
            pdfCell.BorderWidthLeft = 0;
            pdfCell.PaddingBottom = 1f;
            pdfCell.PaddingTop = 1f;
            table_total.AddCell(pdfCell);
            #endregion           

            //create pdf footer
            #region 
            PdfPTable pdfTab_footer = new PdfPTable(1);
            pdfTab_footer.HorizontalAlignment = 1;
            pdfTab_footer.TotalWidth = 545f;
            pdfTab_footer.LockedWidth = true;
            float[] _widths_foote = new float[] { 545 };
            pdfTab_footer.SetWidths(_widths_foote);
            pdfTab_footer.DefaultCell.FixedHeight = 15f;

            // start 1,2,3,4,5 footer
            pdfCell = new PdfPCell(new Phrase(dt_Get_M_MultPurpose_for_Footer.Rows[0]["Remark1"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.Border = 0;
            pdfTab_footer.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_Get_M_MultPurpose_for_Footer.Rows[0]["Remark2"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.Border = 0;
            pdfTab_footer.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_Get_M_MultPurpose_for_Footer.Rows[0]["Remark3"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.Border = 0;
            pdfTab_footer.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(dt_Get_M_MultPurpose_for_Footer.Rows[0]["Remark4"].ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.Border = 0;
            pdfTab_footer.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(page_Number.ToString(), normal_font_9));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.Border = 0;
            pdfTab_footer.AddCell(pdfCell);
            //end 1,2,3,4,5 footer
            #endregion

            pdfTab.WriteSelectedRows(0, -1, document.Left, document.PageSize.Height-10, writer.DirectContent);
            pdfTab_s.WriteSelectedRows(0, -1, document.Left, document.PageSize.Height-first_table_height, writer.DirectContent);
            pdfTab_t.WriteSelectedRows(0, -1, document.Left, document.PageSize.Height - (first_table_height + second_table_height), writer.DirectContent);
            pdfTab_f.WriteSelectedRows(0, -1, 445, document.PageSize.Height - (first_table_height + second_table_height), writer.DirectContent);
            table_header.WriteSelectedRows(0, -1, document.Left, document.PageSize.Height - (first_table_height + second_table_height+third_table_height + 5), writer.DirectContent);
            table_total.WriteSelectedRows(0, -1, document.Left , document.PageSize.Height - 735, writer.DirectContent);
            pdfTab_footer.WriteSelectedRows(0, -1, document.Left, document.PageSize.Height - 755, writer.DirectContent);
           
        }
        //public override void OnCloseDocument(PdfWriter writer, Document document)
        //{
        //    base.OnCloseDocument(writer, document);

        //    //headerTemplate.BeginText();
        //    //headerTemplate.SetFontAndSize(bf, 12);
        //    //headerTemplate.SetTextMatrix(0, 0);
        //    //headerTemplate.ShowText((writer.PageNumber - 1).ToString());
        //    //headerTemplate.EndText();

        //    //footerTemplate.BeginText();
        //    //footerTemplate.SetFontAndSize(bf, 12);
        //    //footerTemplate.SetTextMatrix(0, 0);
        //    //footerTemplate.ShowText((writer.PageNumber - 1).ToString());
        //    //footerTemplate.EndText();
        //}
    }

    public class RoundRectangle : IPdfPCellEvent
    {
        public void CellLayout(
          PdfPCell cell, iTextSharp.text.Rectangle rect, PdfContentByte[] canvas
        )
        {
            PdfContentByte cb = canvas[PdfPTable.LINECANVAS];
            cb.RoundRectangle(
              rect.Left+3,
              rect.Bottom+3,
              rect.Width -6,
              rect.Height-3,
              0 // change to adjust how "round" corner is displayed
            );
            
            cb.SetLineWidth(0.5f);
            cb.SetCMYKColorStrokeF(0f, 0f, 0f, 1f);
            cb.Stroke();
        }
    }


}
