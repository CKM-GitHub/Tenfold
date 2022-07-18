using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.RealEstate.r_invoice
{
    public class r_invoiceModel
    {
        public string RealECD { get; set; }
        public string Range { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Option { get; set; }
    }
    public class r_invoice_PDFModel
    {
        public string RealECD { get; set; }
        public string REStaffCD { get; set; }
        public string TargetYYYYMM { get; set; }
        public string InvoiceYYYYMM { get; set; }
        public string LoginName { get; set; }
        public string Operator { get; set; }
        public string IPAddress { get; set; }
        public string Remarks { get; set; }
    }
}
