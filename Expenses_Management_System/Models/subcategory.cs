using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expenses_Management_System.Models
{
    public class subcategory
    {
        public int subcat_id { get; set; }
        public string subcat_name { get; set; }
        public System.DateTime created_on { get; set; }
        public string created_by { get; set; }
        public int cat_id { get; set; }

        public string cat_name { get; set; }

    }
}