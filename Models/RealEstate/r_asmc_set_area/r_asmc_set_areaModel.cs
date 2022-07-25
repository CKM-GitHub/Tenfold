using System;
using System.Collections.Generic;

namespace Models.RealEstate.r_asmc_set_area
{
    [Serializable]
    public class RECondAreaOptTable
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
