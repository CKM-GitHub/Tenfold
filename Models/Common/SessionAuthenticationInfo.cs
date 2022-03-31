using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Serializable]
    public class SessionAuthenticationInfo
    {
        public string UserID { get; set; }
        public string VerificationToken { get; set; }
    }
}
