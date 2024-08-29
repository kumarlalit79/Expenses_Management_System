using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;


namespace Expenses_Management_System.Models
{
    public class MenuService
    {
        public List<MenuItem> GetMenuItemsForUser(string Type,string Name )
        {
            // Replace this with actual logic to fetch menu items from a database or other data source
            var menuItems = new List<MenuItem>
        {
                 new MenuItem { Title = Type,Url =Name},
            //new MenuItem { Title = "Home", Url = "/Home/Index" },
            //new MenuItem { Title = "About", Url = "/Home/About" },
            //new MenuItem { Title = "Contact", Url = "/Home/Contact" }
        };

            // Example logic to modify menu items based on username
            //if (username == "admin")
            //{
            //    menuItems.Add(new MenuItem { Title = "Admin", Url = "/Admin/Index" });
            //}

            return menuItems;
        }
    }
}