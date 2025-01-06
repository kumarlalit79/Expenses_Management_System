using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using Expenses_Management_System.Models;

namespace Expenses_Management_System.Controllers
{
    {
        }
<<<<<<< HEAD
        ExpensesEntities db = new ExpensesEntities();
=======
        
>>>>>>> 03f38ba69071c00deaa00ed70c8d638487cbd43e

        // GET: SubCategory
        public ActionResult Index()
        {
            {
                int userId = int.Parse(Session["userid"].ToString());

                var selectedCategoryIds = db.user_categories_tbl
                        .Where(uc => uc.uid == userId)
                        .Select(uc => uc.catId)
                        .ToList();

                // Fetch selected subcategories that belong to the selected categories
                var selectedSubcategories = db.sub_category_tbl
                    .Where(sc => db.user_subcategories_tbl
                        .Any(usc => usc.uid == userId && usc.subcatId == sc.subcat_id)
                        && selectedCategoryIds.Contains(sc.fkcat_id)) // Filter by selected categories
                    .Include(sc => sc.category_tbl)
                    .ToList();

                // Fetch unselected subcategories that belong to the selected categories
                var unselectedSubcategories = db.sub_category_tbl
                    .Where(sc => !db.user_subcategories_tbl
                        .Any(usc => usc.uid == userId && usc.subcatId == sc.subcat_id)
                        && selectedCategoryIds.Contains(sc.fkcat_id)) // Filter by selected categories
                    .Include(sc => sc.category_tbl)
                    .ToList();

                // Store selected subcategories in ViewBag for future use (like checkboxes)
                ViewBag.UserSelectedSubcategories = selectedSubcategories;

                return View(unselectedSubcategories);

            }

        }

        


        public ActionResult LoadSubcategories()
        {
            int userId = int.Parse(Session["userid"].ToString());

            // Fetch the IDs of the categories selected by the user
            var selectedCategoryIds = db.user_categories_tbl
                .Where(uc => uc.uid == userId)
                .Select(uc => uc.catId)
                .ToList();

            // Fetch all subcategories that belong to the selected categories
            var allSubcategories = db.sub_category_tbl
                .Where(sc => selectedCategoryIds.Contains(sc.fkcat_id)) // Filter by selected categories
                .Include(sc => sc.category_tbl)
                .ToList();

            var selectedSubcategoryIds = db.user_subcategories_tbl
                .Where(usc => usc.uid == userId)
                .Select(usc => usc.subcatId)
                .ToList();

            ViewBag.SelectedSubcategoryIds = selectedSubcategoryIds;

            return PartialView("_SubcategoriesPartial", allSubcategories);
        }


        [HttpPost]
        public ActionResult SubmitSubcategories(List<int> subcategories)
        {
            try
            {
                int userId = int.Parse(Session["userid"].ToString());

                if (subcategories == null || !subcategories.Any())
                {
                    return Json(new { success = false, message = "No subcategories selected." });
                }

                foreach (var subcatId in subcategories)
                {
                    var subcategoryExists = db.sub_category_tbl.Any(sc => sc.subcat_id == subcatId);
                    if (!subcategoryExists)
                    {
                        return Json(new { success = false, message = "Invalid subcategory selection." });
                    }

                    var userSubcategory = new user_subcategories_tbl
                    {
                        uid = userId,
                        subcatId = subcatId,
                        catId = db.sub_category_tbl
                                    .Where(sc => sc.subcat_id == subcatId)
                                    .Select(sc => sc.category_tbl.cat_id)
                                    .FirstOrDefault()
                    };
                                       

                    db.user_subcategories_tbl.Add(userSubcategory);
                }

                db.SaveChanges();

                var selectedSubcategories = db.user_subcategories_tbl
                    .Where(usc => usc.uid == userId)
                    .Select(usc => new
                    {
                        usc.sub_category_tbl.subcat_id,
                        usc.sub_category_tbl.subcat_name
                    })
                    .ToList();

                return Json(new { success = true, selectedSubcategories = selectedSubcategories });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        } 
    


        public ActionResult Create()
        {
<<<<<<< HEAD
            List<category_tbl> catmst = new List<category_tbl>();
            using (ExpensesEntities db = new ExpensesEntities())
=======
            List<category_tbl> catmst= new List<category_tbl>();
>>>>>>> 03f38ba69071c00deaa00ed70c8d638487cbd43e
            {
                var allData = db.category_tbl.ToList();


                foreach (var item in allData)
                {
                    catmst.Add(new category_tbl
                    {
                        cat_id = int.Parse(item.cat_id.ToString()),
                        cat_name = item.cat_name.ToString(),
                    }); ;
                }

                ViewBag.CAT = new SelectList(catmst, "cat_id", "cat_name");
                return View();
            }

        }

        [HttpPost]
        public ActionResult Create(int cat_id, string subcat_name)
        {
            {
                sub_category_tbl s = new sub_category_tbl();

                s.created_on = DateTime.Now;
                s.created_by = "Lalit";
                s.fkcat_id = cat_id;
                s.subcat_name = subcat_name;
                var sub = db.sub_category_tbl.Add(s);
                int a = db.SaveChanges();
                if (a > 0)
                {
                    TempData["InsertMsg"] = "<script>alert('Inserted Successfully')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index", "SubCategory");
                }
                else
                {
                    TempData["InsertMsg"] = "<script>alert('Failed Inserting')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index", "SubCategory");
                }

            }

        }



        public ActionResult Edit(int id)
        {
            List<category_tbl> catmst = new List<category_tbl>();
            {

                var allData = db.category_tbl.ToList();
                var catmstt = allData.Select(item => new category_tbl
                {
                    cat_id = item.cat_id,
                    cat_name = item.cat_name,
                }).ToList();

                ViewBag.CAT = new SelectList(catmstt, "cat_id", "cat_name");

                var subCategoryEntity = db.sub_category_tbl.Find(id);
                if (subCategoryEntity == null)
                {
                    TempData["ErrorMsg"] = "<script>alert('SubCategory not found')</script>";
                    return RedirectToAction("Index", "SubCategory");
                }

                var subCategoryViewModel = new Expenses_Management_System.Models.subcategory
                {
                    subcat_id = subCategoryEntity.subcat_id,
                    subcat_name = subCategoryEntity.subcat_name,
                    cat_id = subCategoryEntity.fkcat_id,
                    created_on = subCategoryEntity.created_on,
                    created_by = subCategoryEntity.created_by
                };

                return View(subCategoryViewModel);
            }





        }
        [HttpPost]
        public ActionResult Edit(Expenses_Management_System.Models.subcategory s)
        {
<<<<<<< HEAD
            using (ExpensesEntities db = new ExpensesEntities())
=======
>>>>>>> 03f38ba69071c00deaa00ed70c8d638487cbd43e
            {
                if (s.subcat_id == 0)
                {
                    TempData["UpdateSubMsg"] = "<script>alert('Invalid SubCategory ID')</script>";
                    return RedirectToAction("Index", "SubCategory");
                }


                try
                {
                    var existingCat = db.sub_category_tbl.Find(s.subcat_id);
                    if (existingCat == null)
                    {
                        TempData["UpdateSubMsg"] = "<script>alert('SubCategory not found')</script>";
                        return RedirectToAction("Index", "SubCategory");
                    }
                    existingCat.subcat_name = s.subcat_name;
                    existingCat.fkcat_id = s.cat_id;
                    existingCat.created_on = DateTime.Now;
                    existingCat.created_by = "Gaj";
                    db.Entry(existingCat).State = EntityState.Modified;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["UpdateSubMsg"] = "<script>alert('SubCategory updated successfully')</script>";
                    }
                    else
                    {
                        TempData["UpdateSubMsg"] = "<script>alert('SubCategory not updated')</script>";
                    }

                }
                catch (DBConcurrencyException)
                {
                    TempData["UpdateSubMsg"] = "<script>alert('Concurrency error occurred while updating SubCategory')</script>";
                }
                catch (Exception ex)
                {
                    TempData["UpdateSubMsg"] = $"<script>alert('Error: {ex.Message}')</script>";
                }

                return RedirectToAction("Index", "SubCategory");
            }

        }

        public ActionResult Delete(int id)
        {
            {
                var catId = db.sub_category_tbl.Where(modal => modal.subcat_id == id).FirstOrDefault();
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
                return View();
            }


        }

        public ActionResult Details(int id)
        {
            {
                var catId = db.sub_category_tbl.Include(i => i.category_tbl).Where(modal => modal.subcat_id == id).FirstOrDefault();
                return View(catId);
            }

        }
    }
}