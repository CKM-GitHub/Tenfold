using System;
using System.Collections.Generic;

namespace Models.RealEstate.r_asmc_set_train_check_tab
{
    public class M_RECondLineStaModel
    {
        public string RealECD { get; set; }
        public string StationCD { get; set; }
        public int ConditionSEQ { get; set; }
        public string StationName { get; set; }
        public int ValidFLG { get; set; }
        public string ValidFLGText { get; set; }
        public int Expired { get; set; } //1:有効期限切れ
        public string ExpDateText { get; set; }
        public string REStaffName { get; set; }
        public string ExpDate { get; set; }
        public int Priority { get; set; }
        public byte PrecedenceFlg { get; set; }
        public string Remark { get; set; }
    }

    public class M_RECondLineRateModel
    {
        public List<string[]> ColHeader = new List<string[]>();             //List item -> ["5", "以内"]
        public List<string[]> RowHeader = new List<string[]>();             //List item -> ["5", "以内"]
        public List<List<string>> RowData = new List<List<string>>();       //List item -> Rate1, Rate2, Rate3...
    }

    public class M_RECondLineRentModel
    {
        public List<string[]> ColHeader = new List<string[]>();             //List item -> ["5", "以内"]
        public List<string[]> RowHeader = new List<string[]>();             //List item -> ["5", "以内"]
        public List<List<string[]>> RowData = new List<List<string[]>>();   //List item -> [RentHigh1, RentLow1][RentHigh2, RentLow2]...
    }

    public class M_RECondLineOptRow : r_asmc_set_train.RECondLineOptTable
    {
    }
}
