using Expenses_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Expenses_Management_System.Controllers
{
    public class UserController : Controller
    {
        //public UserController(MenuService menuService) : base(menuService)
        //{

        //}
        // GET: User
        public ActionResult Index()
        {
            // Check if the user is authenticated
            //if (Session["Type"] == null)
            //{
            //    return RedirectToAction("Index", "SignIn");
            //}
            using (ExpensesEntities db = new ExpensesEntities())
            {
                var data = db.user_tbl.ToList();
                return View(data);
            }

        }

        public ActionResult Create()
        {
            var Statelist = new List<string>()
            {
            "Andhra Pradesh", "Arunachal Pradesh", "Assam", "Bihar", "Chhattisgarh", "Goa",
            "Gujarat", "Haryana", "Himachal Pradesh", "Jharkhand", "Karnataka", "Kerala",
            "Madhya Pradesh", "Maharashtra", "Manipur", "Meghalaya", "Mizoram", "Nagaland",
            "Odisha", "Punjab", "Rajasthan", "Sikkim", "Tamil Nadu", "Telangana", "Tripura",
            "Uttar Pradesh", "Uttarakhand", "West Bengal"
            };
            ViewBag.StatelistDDl = Statelist;

            string mobileSession = Session["Mobile"] as string;
            ViewBag.mobileNumData = mobileSession;
            
            return View();
        }

        [HttpPost]
        public ActionResult Create(user_tbl u)
        {
            {
                u.created_on = DateTime.Now;
                u.created_by = "gaj";
                int a = db.SaveChanges();
                if (a > 0)
                {
<<<<<<< HEAD
                    var MobNum = db.user_tbl.Where(model => model.mobile_num == u.mobile_num).FirstOrDefault();
                    if (MobNum == null)
                    {
                        Session["Mobile"] = u.mobile_num;
                        // Session["Type"] = u.type;

                        return RedirectToAction("Create", "User");
                    }
                    else
                    {
                       // string a = u.@type;
                        Session["userid"] = MobNum.user_id;
                        Session["Type"] = MobNum.type;
                        Session["Mobile"] = MobNum.mobile_num;
                        Session["Name"] = MobNum.user_name;
                        return RedirectToAction("Index", "SignIn");
                    }
=======
                }
>>>>>>> 03f38ba69071c00deaa00ed70c8d638487cbd43e
                }
                else
                {
                    ModelState.Clear();
                }

            }

        }

        public ActionResult Edit(int id)
        {
            {
                "Andhra Pradesh", "Arunachal Pradesh", "Assam", "Bihar", "Chhattisgarh", "Goa",
                "Gujarat", "Haryana", "Himachal Pradesh", "Jharkhand", "Karnataka", "Kerala",
                "Madhya Pradesh", "Maharashtra", "Manipur", "Meghalaya", "Mizoram", "Nagaland",
                "Odisha", "Punjab", "Rajasthan", "Sikkim", "Tamil Nadu", "Telangana", "Tripura",
                "Uttar Pradesh", "Uttarakhand", "West Bengal"
                };
                ViewBag.StatelistDDl = Statelist;

                var uId = db.user_tbl.Where(model => model.user_id == id).FirstOrDefault();
                return View(uId);
            }
        }

        [HttpPost]
        public ActionResult Edit(user_tbl u)
        {
            {
                db.Entry(u).State = EntityState.Modified;
                int a = db.SaveChanges();
                if (a > 0)
                {
                    TempData["UpdateMsg"] = "<script>alert('Updated successfully')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    TempData["UpdateMsg"] = "<script>alert('Failed to Update')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index", "User");
                }
            }

        }

        public ActionResult Details(int id)
        {
            {
                var ssCatId = db.user_tbl.Where(model => model.user_id == id).FirstOrDefault();
                return View(ssCatId);
            }

        }

        public ActionResult Delete(int id)
        {
            {
                if (id > 0)
                {
                    var uId = db.user_tbl.Where(model => model.user_id == id).FirstOrDefault();
                    if (uId != null)
                    {
                        db.Entry(uId).State = EntityState.Deleted;
                        int a = db.SaveChanges();
                        if (a > 0)
                        {
                            TempData["DeleteMsg"] = "<script>alert('Record Deleted')</script>";
                            return RedirectToAction("Index", "SubSubCategory");
                        }
                        else
                        {

                        }
                    }
                }
            }
            return View();
        }
    }
}