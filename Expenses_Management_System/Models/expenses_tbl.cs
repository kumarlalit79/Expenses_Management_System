//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Expenses_Management_System.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class expenses_tbl
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
        public Nullable<int> fkUserId { get; set; }
        public Nullable<int> fkCatId { get; set; }
        public Nullable<int> fkSubCatId { get; set; }
    
        public virtual category_tbl category_tbl { get; set; }
        public virtual sub_category_tbl sub_category_tbl { get; set; }
        public virtual user_tbl user_tbl { get; set; }
    }
}