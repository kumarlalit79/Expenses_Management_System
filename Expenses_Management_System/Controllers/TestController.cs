using Expenses_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Expenses_Management_System.Controllers
{
    public class TestController : BaseController
    {
        public TestController(MenuService menuService) : base(menuService)
        {
        }
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
    }
}