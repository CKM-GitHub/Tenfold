using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_reale_new
{
    public class t_reale_newModel:BaseModel
    {
        public string REName { get; set; }
        public string REKana { get; set; }
        public string ZipCode1 { get; set; }
        public string ZipCode2 { get; set; }
        public string PrefCD { get; set; }
        public string PrefName { get; set; }
        public string CityName { get; set; }
        public string TownName { get; set; }
        public string Address1 { get; set; }
        public string HousePhone { get; set; }
        public string Fax { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }
        public string President { get; set; }
        public string PICName { get; set; }
        public string PICKana { get; set; }
        public string LicenceNo1 { get; set; }
        public string LicenceNo1Name { get; set; }
        public string LicenceNo2 { get; set; }
        public string LicenceNo3 { get; set; }
        public string CompanyHoliday { get; set; }
        public string BusinessHours { get; set; }
        public string SourceBankCD { get; set; }
        public string SourceBankName { get; set; }
        public string SourceBranchCD { get; set; }
        public string SourceBranchName { get; set; }
        public int SourceAccountType { get; set; }
        public int SourceAccountTypeName { get; set; }
        public string SourceAccount { get; set; }
        public string SourceAccountName { get; set; }
        public string Remark { get; set; }
        public string JoinedDate { get; set; }
        public decimal InitialFee { get; set; }
        public string REPassword { get; set; }
        public string CourseCD { get; set; }
        public string CourseName { get; set; }
        public string NextCourseCD { get; set; }

        public string LoginName { get; set; }

    }
}
