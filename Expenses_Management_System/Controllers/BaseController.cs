using Expenses_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Expenses_Management_System.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected MenuService MenuService { get; private set; }

        public BaseController(MenuService menuService)
        {
            MenuService = menuService;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //ViewBag.MenuItems = MenuService.GetMenuItems();
            //base.OnActionExecuting(filterContext);
            if (Session["Type"]!=null)
            {
                var username = User.Identity.IsAuthenticated ? User.Identity.Name : Session["Type"].ToString();
                ViewBag.MenuItems = MenuService.GetMenuItemsForUser(username, Session["Name"].ToString());
                base.OnActionExecuting(filterContext);

            }
            else
            {
                 RedirectToAction("Index", "SignIn");
                return;
            }

           
        }
    }
}