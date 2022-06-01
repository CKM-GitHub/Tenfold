using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Serializable]
    public class LoginUser
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string VerificationToken { get; set; }
        public string SuperAdminFLG { get; set; }
    }
}
