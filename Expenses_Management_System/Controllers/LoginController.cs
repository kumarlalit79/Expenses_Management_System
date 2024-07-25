using Expenses_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Expenses_Management_System.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(user_tbl u)
        {
            using(EMSEntities8 db = new EMSEntities8())
            {
                if (ModelState.IsValid == true)
                {
                    var MobNum = db.user_tbl.Where(model => model.mobile_num == u.mobile_num).FirstOrDefault();
                    if(MobNum == null)
                    {
                        return RedirectToAction("Create" , "User");
                    }
                    else
                    {
                        Session["Mobile"] = u.mobile_num;
                        return RedirectToAction("Index" , "Expenses");
                    }
                }
                return View();
            }
        }
    }
}