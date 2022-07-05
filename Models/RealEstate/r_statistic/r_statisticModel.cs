using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.RealEstate.r_statistic
{
    public class r_statisticModel : RealEstate_L_Log_Model
    {
        public byte dac_route { get; set; }
        public byte dac_apartment { get; set; }
        public byte top5_route { get; set; }
        public byte top5_apartment { get; set; }
        public byte contracts_route { get; set; }
        public byte contracts_apartment { get; set; }
        public byte bd_route { get; set; }
        public byte bd_apartment { get; set; }
        public byte sd_route { get; set; }
        public byte sd_apartment { get; set; }
        public byte ryudo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
