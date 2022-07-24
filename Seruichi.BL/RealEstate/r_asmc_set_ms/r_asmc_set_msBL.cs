using Models.RealEstate.r_asmc_set_ms;
using Seruichi.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Seruichi.BL.RealEstate.r_asmc_set_ms
{
    public class r_asmc_set_msBL
    {
        public r_asmc_set_ms_Model Get_M_RECondManAll(string realECD, string mansionCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = realECD },
                new SqlParameter("@MansionCD", SqlDbType.VarChar){ Value = mansionCD },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_set_ms_Select_V_RECondMan_by_MansionCD", sqlParams);

            if (dt.Rows.Count == 0)
            {
                return new r_asmc_set_ms_Model();
            }

            r_asmc_set_ms_Model model = dt.Rows[0].ToEntity<r_asmc_set_ms_Model>();

            Get_M_RECondManRate(model);
            Get_M_RECondManRent(model);
            Get_M_RECondManOpt(model);

            return model;
        }

        private void Get_M_RECondManRate(r_asmc_set_ms_Model model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD },
                new SqlParameter("@ConditionSEQ", SqlDbType.Int){ Value = model.ConditionSEQ },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_set_ms_Select_M_RECondManRate_by_ConditionSEQ", sqlParams);

            if (dt.Rows.Count > 0)
            {
                model.Rate = dt.Rows[0]["Rate"].ToStringOrEmpty();
            }
        }

        private void Get_M_RECondManRent(r_asmc_set_ms_Model model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD },
                new SqlParameter("@ConditionSEQ", SqlDbType.Int){ Value = model.ConditionSEQ },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_set_ms_Select_M_RECondManRent_by_ConditionSEQ", sqlParams);

            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                model.RentLow = dr["RentLow"].ToStringOrEmpty("#,##0");
                model.RentHigh = dr["RentHigh"].ToStringOrEmpty("#,##0");
            }
        }

        private void Get_M_RECondManOpt(r_asmc_set_ms_Model model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD },
                new SqlParameter("@ConditionSEQ", SqlDbType.Int){ Value = model.ConditionSEQ },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_set_ms_Select_M_RECondManOpt_by_ConditionSEQ", sqlParams);

            model.RECondManOptList = dt.AsEnumerableEntity<RECondManOptTable>().ToList();
        }

        public List<RECondManOptTable> Get_M_TemplateOpt(string realECD, int templateNo)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = realECD },
                new SqlParameter("@TemplateNo", SqlDbType.Int){ Value = templateNo },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_set_ms_Select_M_TemplateOpt_by_TemplateNo", sqlParams);

            return dt.AsEnumerableEntity<RECondManOptTable>().ToList();

        }

        public bool CheckExistsTemplateName(string realECD, string templateName)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = realECD.ToStringOrNull() },
                new SqlParameter("@TemplateName", SqlDbType.VarChar){ Value = templateName.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            DataTable dt = db.SelectDatatable("pr_r_asmc_set_ms_Check_Exists_TemplateName", sqlParams);

            return dt.Rows.Count == 0;
        }





        public Dictionary<string, string> ValidateAll(r_asmc_set_ms_Model model, bool isOptOnly = false)
        {
            ValidatorAllItems validator = new ValidatorAllItems();
            ValidatorAllItems validator2 = new ValidatorAllItems();

            if (!isOptOnly)
            {
                //備考
                validator.CheckByteCount("Remark", model.Remark, 500);
            }

            //査定買取不可がOFFの場合のみチェック（ONの場合はM_RECondManしか更新しないため)
            if (model.NotApplicableFlg == 0)
            {

                if (!isOptOnly)
                {
                    //買取利回り設定
                    validator.CheckRequiredNumber("Rate", model.Rate, true);
                    validator.CheckIsNumeric("Rate", model.Rate, 3, 2);
                    //家賃設定
                    validator.CheckIsMoney("RentHigh", model.RentHigh, 10);
                    validator.CheckIsMoney("RentLow", model.RentLow, 10);
                    if (!validator.IsContains("RentLow"))
                    {
                        if (model.RentLow.ToDecimal(0) > model.RentHigh.ToDecimal(0))
                        {
                            validator.AddValidationResult("RentLow", "E113");
                        }
                    }
                }

                foreach (var item in model.RECondManOptList)
                {
                    string title = "";

                    //オプション 1:総戸数
                    if (item.OptionKBN == 1)
                    {
                        title = "総戸数";
                        validator2.CheckRequiredNumber("tableOpt", item.Value1.ToStringOrEmpty(), true);
                        validator2.CheckIsNumeric("tableOpt", item.Value1.ToStringOrEmpty(), 3, 0);
                        if (item.NotApplicableFLG != 1)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                    }
                    //オプション 2:所在階(1階)
                    else if (item.OptionKBN == 2)
                    {
                        title = "所在階";
                        if (item.NotApplicableFLG != 1)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                    }
                    //オプション 3:所在階(最上階)
                    else if (item.OptionKBN == 3)
                    {
                        title = "所在階";
                        validator2.CheckRequiredNumber("tableOpt", item.IncDecRate.ToStringOrEmpty(), true);
                        validator2.CheckIsNumeric("tableOpt", item.IncDecRate.ToStringOrEmpty(), 3, 2);
                        if (item.NotApplicableFLG != 0)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                    }
                    //オプション 4:専有面積
                    else if (item.OptionKBN == 4)
                    {
                        title = "専有面積";
                        validator2.CheckRequiredNumber("tableOpt", item.Value1.ToStringOrEmpty(), true);
                        validator2.CheckIsNumeric("tableOpt", item.Value1.ToStringOrEmpty(), 3, 0);
                        if (item.HandlingKBN1 != 1 && item.HandlingKBN1 != 4)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                        if (item.NotApplicableFLG != 1)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                    }
                    //オプション 5:バルコニー
                    else if (item.OptionKBN == 5)
                    {
                        title = "バルコニー";
                        if (item.NotApplicableFLG != 1)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                    }
                    //オプション 6:主採光
                    else if (item.OptionKBN == 6)
                    {
                        title = "主採光";
                        if (item.NotApplicableFLG != 1)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                    }
                    //オプション 8:部屋数
                    else if (item.OptionKBN == 8)
                    {
                        title = "部屋数";
                        validator2.CheckRequiredNumber("tableOpt", item.Value1.ToStringOrEmpty(), true);
                        validator2.CheckIsNumeric("tableOpt", item.Value1.ToStringOrEmpty(), 3, 0);
                        if (item.NotApplicableFLG == 0)
                        {
                            validator2.CheckRequiredNumber("tableOpt", item.IncDecRate.ToStringOrEmpty(), true);
                            validator2.CheckIsNumeric("tableOpt", item.IncDecRate.ToStringOrEmpty(), 3, 2);
                        }
                        else if (item.NotApplicableFLG != 1)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                    }
                    //オプション 9:バス・トイレ
                    else if (item.OptionKBN == 9)
                    {
                        title = "バス・トイレ";
                        if (item.CategoryKBN < 1 || item.CategoryKBN > 3)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                        if (item.NotApplicableFLG == 0)
                        {
                            validator2.CheckRequiredNumber("tableOpt", item.IncDecRate.ToStringOrEmpty(), true);
                            validator2.CheckIsNumeric("tableOpt", item.IncDecRate.ToStringOrEmpty(), 3, 2);
                        }
                        else if (item.NotApplicableFLG != 1)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                    }
                    //オプション 10:土地権利
                    else if (item.OptionKBN == 10)
                    {
                        title = "土地権利";
                        if (item.NotApplicableFLG != 1)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                    }
                    //オプション 11:賃貸状況
                    else if (item.OptionKBN == 11)
                    {
                        title = "賃貸状況";
                        if (item.CategoryKBN < 1 || item.CategoryKBN > 3)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                        if (item.NotApplicableFLG == 0)
                        {
                            validator2.CheckRequiredNumber("tableOpt", item.IncDecRate.ToStringOrEmpty(), true);
                            validator2.CheckIsNumeric("tableOpt", item.IncDecRate.ToStringOrEmpty(), 3, 2);
                        }
                        else if (item.NotApplicableFLG != 1)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                    }
                    //オプション 12:管理状況
                    else if (item.OptionKBN == 12)
                    {
                        title = "管理状況";
                        if (item.CategoryKBN < 1 || item.CategoryKBN > 2)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                        if (item.NotApplicableFLG == 0)
                        {
                            validator2.CheckRequiredNumber("tableOpt", item.IncDecRate.ToStringOrEmpty(), true);
                            validator2.CheckIsNumeric("tableOpt", item.IncDecRate.ToStringOrEmpty(), 3, 2);
                        }
                        else if (item.NotApplicableFLG != 1)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                    }
                    //オプション 13:売却希望時期
                    else if (item.OptionKBN == 13)
                    {
                        title = "売却希望時期";
                        if (item.CategoryKBN < 1 || item.CategoryKBN > 6)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                        if (item.NotApplicableFLG != 1)
                        {
                            validator2.AddValidationResult("tableOpt", "E103");
                        }
                    }

                    if (!validator2.IsValid)
                    {
                        var result = validator.GetValidationResult();
                        var result2 = validator2.GetValidationResult();
                        foreach (var key in result2.Keys)
                        {
                            result.Add(key, "【" + title + "】" + result2[key]);
                        }
                        return result;
                    }
                }
            }

            return validator.GetValidationResult();
        }

        public Dictionary<string, string> ValidateTemplateName(string templateName)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckRequired("TemplateName", templateName);
            validator.CheckByteCount("TemplateName", templateName, 50);

            //CheckExistsTemplateName()

            return validator.GetValidationResult();
        }





        public bool InsertAll(r_asmc_set_ms_Model model, out string msgid)
        {
            msgid = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@MansionCD", SqlDbType.VarChar){ Value = model.MansionCD.ToStringOrNull() },
                new SqlParameter("@PrecedenceFlg", SqlDbType.TinyInt){ Value = model.PrecedenceFlg.ToInt16(0) },
                new SqlParameter("@NotApplicableFlg", SqlDbType.TinyInt){ Value = model.NotApplicableFlg.ToInt16(0) },
                new SqlParameter("@ValidFLG", SqlDbType.TinyInt){ Value = model.ValidFLG.ToInt16(0) },
                new SqlParameter("@ExpDate", SqlDbType.Date){ Value = model.ExpDate.ToStringOrNull() },
                new SqlParameter("@Priority", SqlDbType.TinyInt){ Value = model.Priority.ToInt32(0) },
                new SqlParameter("@Remark", SqlDbType.VarChar){ Value = model.Remark.ToStringOrNull() },
                new SqlParameter("@REStaffCD", SqlDbType.VarChar){ Value = model.REStaffCD.ToStringOrNull() },
                new SqlParameter("@Rate", SqlDbType.Decimal){ Value = model.Rate.ToDecimal(0) },
                new SqlParameter("@RentLow", SqlDbType.Money){ Value = model.RentLow.ToDecimal(0) },
                new SqlParameter("@RentHigh", SqlDbType.Money){ Value = model.RentHigh.ToDecimal(0)  },
                new SqlParameter("@RECondOptTable", SqlDbType.Structured) { TypeName = "dbo.T_RECondOpt", Value = model.RECondManOptList.ToDataTable() },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.Operator.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress.ToStringOrNull() },
                new SqlParameter("@REStaffName", SqlDbType.VarChar){ Value = model.REStaffName.ToStringOrNull() },
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_r_asmc_set_ms_Insert_RECondMan_All", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }

        public bool SaveTemplate(r_asmc_set_ms_Model model, string templateName, out string msgid)
        {
            msgid = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@TemplateName", SqlDbType.VarChar){ Value = templateName.ToStringOrNull() },
                new SqlParameter("@TemplateOptTable", SqlDbType.Structured) { TypeName = "dbo.T_RECondOpt", Value = model.RECondManOptList.ToDataTable() },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.Operator.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress.ToStringOrNull() },
                new SqlParameter("@REStaffName", SqlDbType.VarChar){ Value = model.REStaffName.ToStringOrNull() },
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_r_asmc_set_ms_Insert_Template", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }

    }
}
