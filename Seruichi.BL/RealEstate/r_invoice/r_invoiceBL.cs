using Models.RealEstate.r_invoice;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seruichi.BL.RealEstate.r_invoice
{
    public class r_invoiceBL
    {
        public Dictionary<string, string> ValidateAll(r_invoiceModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckYM("StartMonth", model.StartDate);//E125
            validator.CheckYM("EndMonth", model.EndDate);//E125

            validator.CheckCompareYM("StartMonth", model.StartDate, model.EndDate);//E126
            validator.CheckCompareYM("EndMonth", model.StartDate, model.EndDate);//E126
            return validator.GetValidationResult();
        }
        public DataTable Get_D_Billing_List(r_invoiceModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = model.RealECD },
                new SqlParameter("@Range", SqlDbType.VarChar){Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){Value = model.EndDate.ToStringOrNull() },
                new SqlParameter("@Option", SqlDbType.TinyInt){Value = model.Option.ToStringOrNull() },

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_invoice_Select_D_Billing_By_RealECD", sqlParams);
            return dt;
        }

        public DataTable Get_M_RealEstate_For_PDFHeader(string RealECD)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = RealECD }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_invoice_pdf_get_M_RealEstate_For_PDFHeader", sqlParams);
            return dt; 
        }

        public DataTable Get_D_Billing_For_PDFHeader(string invoiceNO)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@InvoiceNo", SqlDbType.VarChar) { Value = invoiceNO }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_invoice_pdf_get_D_Billing_For_PDFHeader", sqlParams);
            return dt;
        }

        public DataTable Get_M_Control_For_PDFHeader()
        {
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_invoice_pdf_get_M_Control_For_PDFHeader");
            return dt;
        }
        public DataTable Get_M_Image_For_PDFHeader()
        {
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_invoice_pdf_get_M_Image_For_PDFHeader");
            return dt;
        }
        public DataTable Get_Service_Registration_Block_A(string RealECD,string yyyyMM)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = RealECD },
                new SqlParameter("@InvoiceYYYYMM", SqlDbType.VarChar){Value = yyyyMM.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_invoice_pdf_Service_Registration_Block_A", sqlParams);
            return dt;
            
        }
        public DataTable Get_Contract_Area_Block_B(string RealECD, string yyyyMM)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = RealECD },
                new SqlParameter("@InvoiceYYYYMM", SqlDbType.VarChar){Value = yyyyMM.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_invoice_pdf_Contract_Area_Block_B", sqlParams);
            return dt;

        }
        public DataTable Get_Contract_Apartments_Block_C(string RealECD, string yyyyMM)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = RealECD },
                new SqlParameter("@InvoiceYYYYMM", SqlDbType.VarChar){Value = yyyyMM.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_invoice_pdf_Contract_Apartments_Block_C", sqlParams);
            return dt;

        }
        public DataTable Get_Customer_Send_Record_Block_D(string RealECD, string yyyyMM)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = RealECD },
                new SqlParameter("@InvoiceYYYYMM", SqlDbType.VarChar){Value = yyyyMM.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_invoice_pdf_Customer_Send_Record_Block_D", sqlParams);
            return dt;

        }
      
        public DataTable Get_Customer_Transfer_Record_Block_E(string RealECD, string yyyyMM)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = RealECD },
                new SqlParameter("@InvoiceYYYYMM", SqlDbType.VarChar){Value = yyyyMM.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_invoice_pdf_Customer_Transfer_Record_Block_E", sqlParams);
            return dt;

        }

        public DataTable Get_Customer_Cancel_Record_Block_F(string RealECD, string yyyyMM)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = RealECD },
                new SqlParameter("@InvoiceYYYYMM", SqlDbType.VarChar){Value = yyyyMM.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_invoice_pdf_Customer_Cancel_Record_Block_F", sqlParams);
            return dt;

        }
        public DataTable Get_M_MultPurpose_for_Footer()
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@DataID", SqlDbType.Int) { Value = 121 },
                new SqlParameter("@DataKey", SqlDbType.Int){Value = 1 }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_invoice_get_M_MultPurpose_for_Footer", sqlParams);
            return dt;

        }
        public bool UpdateD_Billing(r_invoice_PDFModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = model.RealECD },
                new SqlParameter("@REStaffCD", SqlDbType.VarChar) { Value = model.REStaffCD },
                new SqlParameter("@TargetYYYYMM", SqlDbType.VarChar) { Value = model.TargetYYYYMM },
                new SqlParameter("@InvoiceYYYYMM", SqlDbType.VarChar) { Value = model.InvoiceYYYYMM },
                new SqlParameter("@LoginName", SqlDbType.VarChar){Value = model.LoginName },
                new SqlParameter("@Operator", SqlDbType.VarChar) { Value = model.Operator },
                new SqlParameter("@IPAddress", SqlDbType.VarChar) { Value = model.IPAddress },
                new SqlParameter("@Remarks", SqlDbType.VarChar){Value = model.Remarks}
            };
            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_r_invoice_pdf_Update_D_Billing", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }

    }
}
