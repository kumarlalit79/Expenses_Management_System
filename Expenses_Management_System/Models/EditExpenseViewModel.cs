using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace Expenses_Management_System.Models
{
    public class EditExpenseViewModel
    {
        public expenses_tbl Expense { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> SubCategories { get; set; }
        public IEnumerable<SelectListItem> SubSubCategories { get; set; }
    }
}