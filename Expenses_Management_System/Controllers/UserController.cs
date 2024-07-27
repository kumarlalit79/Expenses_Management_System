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
        // GET: User
        public ActionResult Index()
        {
            using (EMSEntities8 db = new EMSEntities8())
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
            using (EMSEntities8 db = new EMSEntities8())
            {
                u.created_on = DateTime.Now;
                u.created_by = "gaj";

                db.user_tbl.Add(u);
                int a = db.SaveChanges();
                if (a > 0)
                {
                    //TempData["InsertMsg"] = "<script>alert('Inserted Successfully')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index", "Expenses");
                }
                else
                {
                    //TempData["InsertMsg"] = "<script>alert('Failed Inserting')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index", "Expenses");
                }

            }

        }

        public ActionResult Edit(int id)
        {
            using (EMSEntities8 db = new EMSEntities8())
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

                var uId = db.user_tbl.Where(model => model.user_id == id).FirstOrDefault();
                return View(uId);
            }
        }

        [HttpPost]
        public ActionResult Edit(user_tbl u)
        {
            using (EMSEntities8 db = new EMSEntities8())
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
            using (EMSEntities8 db = new EMSEntities8())
            {
                var ssCatId = db.user_tbl.Where(model => model.user_id == id).FirstOrDefault();
                return View(ssCatId);
            }

        }

        public ActionResult Delete(int id)
        {
            using (EMSEntities8 db = new EMSEntities8())
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
                            TempData["DeleteMsg"] = "<script>alert('Failed Deleted')</script>";

                        }
                    }
                }
            }
            return View();
        }
    }
}