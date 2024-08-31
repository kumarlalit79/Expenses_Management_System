using Expenses_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Expenses_Management_System.Controllers
{
    public class MyAccountController : Controller
    {
        // GET: MyAccount
        public ActionResult Index()
        {
            using(ExpensesEntities db = new ExpensesEntities())
            {
                var data = db.user_tbl.ToList();
                return View(data);
            }
            
        }
    }
}