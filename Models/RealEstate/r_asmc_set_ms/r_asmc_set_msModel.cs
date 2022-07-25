using System;
using System.Collections.Generic;

namespace Models.RealEstate.r_asmc_set_ms
{
    [Serializable]
    public class r_asmc_set_ms_Model
    {
        public string RealECD { get; set; }
        public int ConditionSEQ { get; set; }
        public string MansionCD { get; set; }
        public byte PrecedenceFlg { get; set; }
        public byte NotApplicableFlg { get; set; }
        public byte ValidFLG { get; set; }
        public string ExpDate { get; set; }
        public int Expired { get; set; }
        public int ExpiredSoon { get; set; }
        public int Priority { get; set; }
        public string Remark { get; set; }
        public string REStaffCD { get; set; }
        public string REStaffName { get; set; }
        public string Operator { get; set; }
        public string IPAddress { get; set; }

        //表示用
        public string MansionName { get; set; }
        public string Address { get; set; }
        public string RealEstateCount { get; set; }

        //M_RECondManRate
        public string Rate { get; set; }
        
        //M_RECondManRent
        public string RentLow { get; set; }
        public string RentHigh { get; set; }

        //M_RECondManOpt
        public string RECondManOptJson { get; set; }
        public List<RECondManOptTable> RECondManOptList { get; set; } = new List<RECondManOptTable>();

        public string ValidationResultJson { get; set; }
    }

    [Serializable]
    public class RECondManOptTable
    {
        public int OptionKBN { get; set; }
        public int OptionSEQ { get; set; }
        public int CategoryKBN { get; set; }
        public byte NotApplicableFLG { get; set; }
        public int Value1 { get; set; }
        public byte HandlingKBN1 { get; set; }
        public int Value2 { get; set; } = 0;
        public byte HandlingKBN2 { get; set; } = 0;
        public string IncDecRate { get; set; }
    }
}
