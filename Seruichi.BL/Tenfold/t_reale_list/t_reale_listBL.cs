using Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Seruichi.Common;
using System.Data;
using Models.Tenfold.t_reale_list;
using System.Threading.Tasks;

namespace Seruichi.BL.Tenfold.t_reale_list
{
    public class t_reale_listBL
    {
        public List<DropDownListItem> GetDropDownListForPref()
        {
            string spName = "pr_t_seller_list_Select_DropDownlistofPref";
            return GetDropDownListItems(spName);
        }
        private List<DropDownListItem> GetDropDownListItems(string spName, params SqlParameter[] sqlParams)
        {
            var items = new List<DropDownListItem>();

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable(spName, sqlParams);

            foreach (DataRow dr in dt.Rows)
            {
                var option = new DropDownListItem()
                {
                    Value = dr["Value"].ToStringOrEmpty(),
                    DisplayText = dr["DisplayText"].ToStringOrEmpty()
                };
                items.Add(option);
            }

            return items;
        }
    }
}
