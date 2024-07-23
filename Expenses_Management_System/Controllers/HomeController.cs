using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Expenses_Management_System.Models;

namespace Expenses_Management_System.Controllers
{
    public class HomeController : Controller
    {
        EMSEntities8 db = new EMSEntities8();
        public ActionResult Index()
        {
            var data = db.category_tbl.ToList();
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(category_tbl c)
        {
            c.created_by = "GAJ";
            c.created_on = DateTime.Now;
            var category = db.category_tbl.Add(c);
            int a = db.SaveChanges();
            if (a > 0)
            {
                TempData["CreateCategory"] = "<script>alert('Category created successfully')</script>";
                ModelState.Clear();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["CreateCategory"] = "<script>alert('Category not created')</script>";
                ModelState.Clear();
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Edit(int id)
        {
            var catId = db.category_tbl.Where(model => model.cat_id == id).FirstOrDefault();
            return View(catId);
        }

        [HttpPost]
        public ActionResult Edit(category_tbl c)
        {
            if (ModelState.IsValid == true)
            {
                c.created_by = "Gaj";
                c.created_on= DateTime.Now;
                db.Entry(c).State = EntityState.Modified;
                int a = db.SaveChanges();
                if (a > 0)
                {
                    TempData["UpdateMsg"] = "<script>alert('Category Updated successfully')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["UpdateMsg"] = "<script>alert('Category not Updated')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            if(id > 0)
            {
                var catId = db.category_tbl.Where(modal => modal.cat_id == id).FirstOrDefault();
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

        public ActionResult Details(int id)
        {
            var catId = db.category_tbl.Where(modal => modal.cat_id == id ).FirstOrDefault();
            return View(catId);
        }
    }
}