using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Models.RealEstate.r_invoice
{
    public class r_Invoice_PageEvent: PdfPageEventHelper
    {
        int page_Number = 0;

        public override void OnEndPage(PdfWriter pdfWriter, Document pdfDoc)
        {
            page_Number = page_Number + 1;
            
            PdfPTable inv_Tbl = new PdfPTable(4);

            //declare new cell
            PdfPCell inv_Cell = new PdfPCell();
            //start 1
            inv_Cell.Colspan = 2;
            inv_Cell = new PdfPCell(new Phrase("9999年99月度"));
            inv_Cell.HorizontalAlignment = Element.ALIGN_CENTER;
            inv_Cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            inv_Tbl.AddCell(inv_Cell);

            inv_Cell = new PdfPCell(new Phrase("請　求　書"));
            inv_Cell.HorizontalAlignment = Element.ALIGN_CENTER;
            inv_Cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            inv_Tbl.AddCell(inv_Cell);

            inv_Cell = new PdfPCell(new Phrase("9999/99/99"));
            inv_Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            inv_Cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            inv_Tbl.AddCell(inv_Cell);
            //end 1

            // start 2
            inv_Cell.Colspan = 3;
            inv_Cell = new PdfPCell(new Phrase("〒999-9999"));
            inv_Cell.HorizontalAlignment = Element.ALIGN_LEFT;
            inv_Cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            inv_Tbl.AddCell(inv_Cell);

            inv_Cell = new PdfPCell(new Phrase("page-" + page_Number));
            inv_Cell.HorizontalAlignment = Element.ALIGN_CENTER;
            inv_Cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            inv_Tbl.AddCell(inv_Cell);
            //end 2

            inv_Tbl.WriteSelectedRows(0, -1, 34, 807, pdfWriter.DirectContent);

        }
    }
}
