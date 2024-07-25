using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expenses_Management_System.Models
{
    public class Expenses
    {
        public int exp_id { get; set; }
        public string monthly_income { get; set; }
        public string item_name { get; set; }
        public string item_qty { get; set; }
        public string total_price { get; set; }
        public string remark { get; set; }
        public System.DateTime sdate { get; set; }
        public System.DateTime created_on { get; set; }
        public string created_by { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public int cat_id { get; set; }
        public string cat_name { get; set; }
        public int subcat_id { get; set; }
        public string subcat_name { get; set; }
        //public int sub_sub_catId { get; set; }
        //public string sub_sub_catName { get; set; }
        
    }
}