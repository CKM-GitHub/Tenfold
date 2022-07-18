using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_assess_guide
{
   public class t_assess_guideModel
    {
        public string Key { get; set; }
        public string RealECD { get; set; }
        public string ReStaffCD { get; set; }
        public string REName { get; set; }
        public byte chk_Purchase { get; set; }
        public byte chk_Checking { get; set; }
        public byte chk_Nego { get; set; }
        public byte chk_Contract { get; set; }
        public byte chk_SellerDeclined { get; set; }
        public byte chk_BuyerDeclined { get; set; }
        public string Range { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string AssReqID { get; set; }
        public string SellerCD { get; set; }
        public string SellerMansionID { get; set; }
        public byte Chk_MiShouri { get; set; }
        public byte Chk_Shouri { set; get; }
        public byte Chk_YouKakunin { get; set; }
        public byte Chk_HouShuu { get; set; }
        public byte Chk_Soukyaku { get; set; }
        public byte Chk_NonMemberSeller { get; set; }
        public bool IsCSV { get; set; }
        public string Kanritantou { get; set; }
        //public string BukkenNO { get; set; }
        //public int BukkenFileRows { get; set; }
        //public byte BukkenFileShurui { get; set; }
        //public string BukkenFileName { get; set; }
        public string HiddenUpdateDatetime { get; set; }
        public string UserCD { get; set; }
        public string IntroReqID { get; set; }
        public string AttachSEQ { get; set; }
        public string AttachFileName { get; set; }
        public string AttachFileType { get; set; }
        public string AttachSize { get; set; }
        public string AttachFileUnzipPW { get; set; }
        public string ZippedFileName { get; set; }
        public string AttachFilePath { get; set; }
        public string AttachFileZippedFullPathName { get; set; }
    }
    public class t_assess_guide_L_Log_Model
    {
        public byte LoginKBN { get; set; }
        public string LoginID { get; set; }
        public string RealECD { get; set; }
        public string LoginName { get; set; }
        public string IPAddress { get; set; }
        public string Page { get; set; }
        public string Processing { get; set; }
        public string Remarks { get; set; }
    }
}
