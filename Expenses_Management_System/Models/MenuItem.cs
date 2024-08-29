using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expenses_Management_System.Models
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public List<MenuItem> SubMenuItems { get; set; }
    
    }
}