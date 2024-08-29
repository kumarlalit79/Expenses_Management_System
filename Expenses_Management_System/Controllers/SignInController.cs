using Expenses_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Expenses_Management_System.Controllers
{
    public class SignInController : Controller
    {
        // GET: SignIn
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(user_tbl u)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                if (ModelState.IsValid == true)
                {
                    var MobNum = db.user_tbl.Where(model => model.mobile_num == u.mobile_num).FirstOrDefault();
                    if (MobNum == null)
                    {
                        Session["Mobile"] = u.mobile_num;
                       //  Session["Type"] = u.type;

                        return RedirectToAction("Create", "User");
                    }
                    else
                    {
                        string a = u.@type;
                        Session["userid"] = MobNum.user_id;
                        Session["Type"] = MobNum.type;
                        Session["Mobile"] = MobNum.mobile_num;
                        Session["Name"] = MobNum.user_name;
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                return View();
            }
        }
    }
}