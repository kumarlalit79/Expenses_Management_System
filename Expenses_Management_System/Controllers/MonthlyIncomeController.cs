using Expenses_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Expenses_Management_System.Controllers
{
    public class MonthlyIncomeController : BaseController
    {
        public MonthlyIncomeController(MenuService menuService) : base(menuService)
        {
        }
        // GET: MonthlyIncome
        public ActionResult Index()
        {
            // Check if the user is authenticated
            if (Session["Type"] == null)
            {
                return RedirectToAction("Index", "SignIn");
            }
            using (ExpensesEntities db = new ExpensesEntities())
                {
                    //int id = int.Parse(Session["userid"].ToString());
                    var data = db.Monthly_Income_tbl.ToList();
                    //var data = db.expenses_tbl.Where(i => i.fkUserId==id).ToList();
                    return View(data);

                }
        }

        public ActionResult Create()
        {
            var Yearlist = new List<string>()
                {
                   "2024","2025","2026","2027","2028","2029",
                    "2030","2031","2032","2033","2034","2035",
                };

            ViewBag.YearlistDDl = Yearlist;
            var Monthlist = new List<string>()
                {
                   "January","February","March","April","May","June",
                    "July","August","September","October","November","December",
                };
            ViewBag.MonthlistDDl = Monthlist;


            return View();
        }

        [HttpPost]
        public ActionResult Create(Monthly_Income_tbl c)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                c.created_by = "GAJ";
                c.created_on = DateTime.Now;
                c.fkuserid = int.Parse(Session["userid"].ToString());
                var category = db.Monthly_Income_tbl.Add(c);
                int a = db.SaveChanges();
                if (a > 0)
                {
                    TempData["Monthlyincome"] = "<script>alert('Monthly income created successfully')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index", "MonthlyIncome");
                }
                else
                {
                    TempData["Monthlyincome"] = "<script>alert('Monthly income not created')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index", "MonthlyIncome");
                }
            }
        }

        public ActionResult Edit(int id)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                var Yearlist = new List<string>()
                {
                   "2024","2025","2026","2027","2028","2029",
                    "2030","2031","2032","2033","2034","2035",
                };

                ViewBag.YearlistDDl = Yearlist;
                var Monthlist = new List<string>()
                {
                   "January","February","March","April","May","June",
                    "July","August","September","October","November","December",
                };
                ViewBag.MonthlistDDl = Monthlist;
                var catId = db.Monthly_Income_tbl.Where(model => model.Mid == id).FirstOrDefault();
                return View(catId);
            }
        }

        [HttpPost]
        public ActionResult Edit(Monthly_Income_tbl c)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                if (ModelState.IsValid == true)
                {
                    c.created_by = "Gaj";
                    c.created_on = DateTime.Now;
                    c.fkuserid = int.Parse(Session["userid"].ToString());
                    db.Entry(c).State = EntityState.Modified;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["UpdateMsg"] = "<script>alert('Monthly Income Updated successfully')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "MonthlyIncome");
                    }
                    else
                    {
                        TempData["UpdateMsg"] = "<script>alert('Monthly Income not Updated')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "MonthlyIncome");
                    }
                }
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                if (id > 0)
                {
                    var catId = db.Monthly_Income_tbl.Where(modal => modal.Mid == id).FirstOrDefault();
                    if (catId != null)
                    {
                        db.Entry(catId).State = EntityState.Deleted;
                        int a = db.SaveChanges();
                        if (a > 0)
                        {
                            TempData["DeleteMsg"] = "<script>alert('Record Deleted')</script>";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["DeleteMsg"] = "<script>alert('Faild Deleted')</script>";

                        }
                    }
                }
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                var catId = db.Monthly_Income_tbl.Where(modal => modal.Mid == id).FirstOrDefault();
                return View(catId);
            }
        }


    }
}