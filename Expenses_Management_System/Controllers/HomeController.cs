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
        public ActionResult Index()
        {
            int userId = int.Parse(Session["userid"].ToString());

            var selectedCategories = db.category_tbl
                               .Where(c => db.user_categories_tbl
                                            .Any(uc => uc.uid == userId && uc.catId == c.cat_id))
                               .ToList();

            var unselectedCategories = db.category_tbl
                                 .Where(c => !db.user_categories_tbl
                                             .Any(uc => uc.uid == userId && uc.catId == c.cat_id))
                                 .ToList();
            if (selectedCategories.Any())
            {
                ViewBag.UserSelectedCategories = selectedCategories;
                ViewBag.ShowMessage = false; 
            }
            else
            {
                
                ViewBag.ShowMessage = true;
            }

            return View(unselectedCategories);
        }

        public ActionResult LoadCategories()
        {
            int userId = int.Parse(Session["userid"].ToString());

            
            var allCategories = db.category_tbl.ToList();

            
            var selectedCategoryIds = db.user_categories_tbl
                                       .Where(uc => uc.uid == userId)
                                       .Select(uc => uc.catId)
                                       .ToList();

            ViewBag.SelectedCategoryIds = selectedCategoryIds;

            return PartialView("_CategoriesPartial", allCategories);
        }


        [HttpPost]
        public ActionResult SubmitCategories(List<int> categories)
        {
            try
            {
                
                int userId = int.Parse(Session["userid"].ToString());

                
                var userExists = db.user_tbl.Any(u => u.user_id == userId);
                if (!userExists)
                {
                    return Json(new { success = false, message = "Invalid user." });
                }

                
                if (categories == null || !categories.Any())
                {
                    return Json(new { success = false, message = "No categories selected." });
                }

                
                foreach (var catId in categories)
                {
                
                    var categoryExists = db.category_tbl.Any(c => c.cat_id == catId);
                    if (!categoryExists)
                    {
                        return Json(new { success = false, message = "Invalid category selection." });
                    }

                
                    var userCategory = new user_categories_tbl
                    {
                        uid = userId,  // Set the user ID
                        catId = catId  // Set the category ID
                    };

                    db.user_categories_tbl.Add(userCategory);
                }

                
                db.SaveChanges();

                
                var selectedCategories = db.user_categories_tbl
                    .Where(uc => uc.uid == userId)
                    .Select(uc => new
                    {
                        uc.category_tbl.cat_id,
                        uc.category_tbl.cat_name
                    })
                    .ToList();

                
                return Json(new { success = true, selectedCategories = selectedCategories });
            }
            catch (Exception ex)
            {
                
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
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