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
    public class PDF_Font
    {
        public Font Normal_Font_10(string font_folder)
        {
            // BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            // BaseFont baseFT = BaseFont.CreateFont(font_folder + "Meiryo W53 Regular.otf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED); 
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "Meiryo UI W53 Regular.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new iTextSharp.text.Font(baseFT, 10);
            return font;
        }

        public Font bold_font_16_white(string font_folder)
        {
            // BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            // BaseFont baseFT = BaseFont.CreateFont(font_folder + "Meiryo W53 Regular.otf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED); 
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "Meiryo UI W53 Regular.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new iTextSharp.text.Font(baseFT, 16,Font.BOLD,BaseColor.WHITE);
            return font;
        }
        public Font Normal_Font_9(string font_folder)
        {
            // BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            // BaseFont baseFT = BaseFont.CreateFont(font_folder + "Meiryo W53 Regular.otf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED); 
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "Meiryo UI W53 Regular.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new iTextSharp.text.Font(baseFT, 9);
            return font;
        }
        public Font Bold_Font_9(string font_folder)
        {
            // BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            // BaseFont baseFT = BaseFont.CreateFont(font_folder + "Meiryo W53 Regular.otf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED); 
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "Meiryo UI W53 Regular.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new iTextSharp.text.Font(baseFT, 9, Font.BOLD);
            return font;
        }
        public Font Bold_Font_9_White(string font_folder)
        {
            // BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            // BaseFont baseFT = BaseFont.CreateFont(font_folder + "Meiryo W53 Regular.otf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED); 
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "Meiryo UI W53 Regular.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new iTextSharp.text.Font(baseFT, 9, Font.BOLD,BaseColor.WHITE);
            return font;
        }

        public Font Bold_Font_8(string font_folder)
        {
            // BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            // BaseFont baseFT = BaseFont.CreateFont(font_folder + "Meiryo W53 Regular.otf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED); 
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "Meiryo UI W53 Regular.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new iTextSharp.text.Font(baseFT, 8, Font.BOLD);
            return font;
        }

        public Font Normal_Font_8(string font_folder)
        {
            // BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            // BaseFont baseFT = BaseFont.CreateFont(font_folder + "Meiryo W53 Regular.otf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED); 
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "Meiryo UI W53 Regular.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new iTextSharp.text.Font(baseFT, 8);
            return font;
        }
    }

    public class DottedCell : IPdfPCellEvent
    {
        private readonly int _border = 0;

        public DottedCell(int border)
        {
            _border = border;
        }

        public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
        {
            var canvas = canvases[PdfPTable.LINECANVAS];
            canvas.SaveState();
            canvas.SetLineDash(0, 2, 2);

            cell.Border = Rectangle.NO_BORDER;

            if ((_border & Rectangle.TOP_BORDER) == Rectangle.TOP_BORDER)
            {
                canvas.MoveTo(position.GetRight(1), position.GetTop(1));
                canvas.LineTo(position.GetLeft(1), position.GetTop(1));
            }
            if ((_border & Rectangle.BOTTOM_BORDER) == Rectangle.BOTTOM_BORDER)
            {
                canvas.MoveTo(position.GetRight(0.1f), position.GetBottom(0.1f));
                canvas.LineTo(position.GetLeft(0.1f), position.GetBottom(0.1f));
            }
            if ((_border & Rectangle.RIGHT_BORDER) == Rectangle.RIGHT_BORDER)
            {
                canvas.MoveTo(position.GetRight(1), position.GetTop(1));
                canvas.LineTo(position.GetRight(1), position.GetBottom(1));
            }
            if ((_border & Rectangle.LEFT_BORDER) == Rectangle.LEFT_BORDER)
            {
                canvas.MoveTo(position.GetLeft(1), position.GetTop(1));
                canvas.LineTo(position.GetLeft(1), position.GetBottom(1));
            }
            canvas.Stroke();
            canvas.RestoreState();
        }
    }

   
}
