using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.RealEstate.r_template
{

    public class r_templateModel : RealEstate_L_Log_Model
    {
        public string RealECD { get; set; }
        public string SEQ { get; set; }
        public string MessageSEQ { get; set; }
        public string MessageTitle { get; set; }
        public string MessageTEXT { get; set; }
        public string PermissionSetting { get; set; }
        public string Applying { get; set; }
        public byte ValidFlg { get; set; }
        public string Mode { get; set; }
        public string TemplateNo { get; set; }
        public string TemplateKBN { get; set; }
        public string Page { get; set; }
        public string Processing { get; set; }
        public string permission_setting { get; set; }

    }
}
