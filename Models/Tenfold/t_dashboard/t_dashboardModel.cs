using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_dashboard
{
    public class t_dashboardModel
    {
        public int Customer_Information_Waiting_Count { get; set; }
        public int Chat_Confirmation_Waiting_Count { get; set; }
        public int New_Request_Cases_Count { get; set; }
        public int During_negotiations_Cases_Count { get; set; }
        public int Contract_Cases_Count { get; set; }
        public int Decline_Cases_Count { get; set; }
    }
}
