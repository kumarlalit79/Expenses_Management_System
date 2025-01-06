using Expenses_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;

namespace Expenses_Management_System.Controllers
{
    {
        public SubSubCategoryController(MenuService menuService) : base(menuService)
        { 
        
        }
        ExpensesEntities db = new ExpensesEntities();
        // GET: SubSubCategory
        //public ActionResult Index()
        //{
        //    // Check if the user is authenticated
        //    if (Session["Type"] == null)
        //    {
        //        return RedirectToAction("Index", "SignIn");
        //    }
        //    using (ExpensesEntities db = new ExpensesEntities())
        //    {
        //        //var data = db.sub_sub_category_tbl.Include(i => i.category_tbl).Include(i => i.sub_category_tbl).ToList();

        //        int userId = int.Parse(Session["userid"].ToString());

        //        // Selected Sub-Subcategories
        //        var selectedSubSubcategories = db.sub_sub_category_tbl
        //            .Where(ssc => db.user_sub_subcategory_tbl
        //            .Any(usc => usc.user_tbl.user_id == userId && usc.sub_sub_category_tbl.sub_sub_catId == ssc.sub_sub_catId))
        //            .Include(ssc => ssc.sub_category_tbl)
        //            .ToList();

        //        // Unselected Sub-Subcategories
        //        var unselectedSubSubcategories = db.sub_sub_category_tbl
        //            .Where(ssc => !db.user_sub_subcategory_tbl
        //            .Any(usc => usc.user_tbl.user_id == userId && usc.sub_sub_category_tbl.sub_sub_catId == ssc.sub_sub_catId))
        //            .Include(ssc => ssc.sub_category_tbl)
        //            .ToList();

        //        ViewBag.UserSelectedSubSubcategories = selectedSubSubcategories;

        //        return View(unselectedSubSubcategories);

        //    }

        //    //return View();
        //}

        //public ActionResult Index()
        //{
        //    // Check if the user is authenticated
        //    if (Session["Type"] == null)
        //    {
        //        return RedirectToAction("Index", "SignIn");
        //    }

        //    using (ExpensesEntities db = new ExpensesEntities())
        //    {
        //        int userId = int.Parse(Session["userid"].ToString());

        //        // Selected Sub-Subcategories with eager loading for all required relationships
        //        var selectedSubSubcategories = db.sub_sub_category_tbl
        //            .Where(ssc => db.user_sub_subcategory_tbl
        //            .Any(usc => usc.user_tbl.user_id == userId && usc.sub_sub_category_tbl.sub_sub_catId == ssc.sub_sub_catId))
        //            .Include(ssc => ssc.sub_category_tbl.category_tbl) // Include both subcategory and category tables
        //            .ToList();

        //        // Unselected Sub-Subcategories with eager loading
        //        var unselectedSubSubcategories = db.sub_sub_category_tbl
        //            .Where(ssc => !db.user_sub_subcategory_tbl
        //            .Any(usc => usc.user_tbl.user_id == userId && usc.sub_sub_category_tbl.sub_sub_catId == ssc.sub_sub_catId))
        //            .Include(ssc => ssc.sub_category_tbl.category_tbl) // Include both subcategory and category tables
        //            .ToList();

        //        ViewBag.UserSelectedSubSubcategories = selectedSubSubcategories;

        //        return View(unselectedSubSubcategories);
        //    }
        //}
        public ActionResult Index()
        {
            {
                return RedirectToAction("Index", "SignIn");
            }

            using (ExpensesEntities db = new ExpensesEntities())
            {
                db.Configuration.ProxyCreationEnabled = false; // Disable proxy creation

                int userId = int.Parse(Session["userid"].ToString());

                // Selected Sub-Subcategories with eager loading
                var selectedSubSubcategories = db.sub_sub_category_tbl
                    .Where(ssc => db.user_sub_subcategory_tbl
                    .Any(usc => usc.user_tbl.user_id == userId && usc.sub_sub_category_tbl.sub_sub_catId == ssc.sub_sub_catId))
                    .Include(ssc => ssc.sub_category_tbl.category_tbl) // Eager loading for relationships
                    .ToList();

                // Unselected Sub-Subcategories
                var unselectedSubSubcategories = db.sub_sub_category_tbl
                    .Where(ssc => !db.user_sub_subcategory_tbl
                    .Any(usc => usc.user_tbl.user_id == userId && usc.sub_sub_category_tbl.sub_sub_catId == ssc.sub_sub_catId))
                    .Include(ssc => ssc.sub_category_tbl.category_tbl) // Eager loading for relationships
                    .ToList();

                ViewBag.UserSelectedSubSubcategories = selectedSubSubcategories;

                return View(unselectedSubSubcategories);
            }
        }



        public ActionResult LoadSubSubCategories()
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                int userId = int.Parse(Session["userid"].ToString());

                //var allSubSubcategories = db.sub_sub_category_tbl.Include(ssc => ssc.sub_category_tbl).ToList();
                //var selectedSubSubcategoryIds = db.user_sub_subcategory_tbl
                //    .Where(usc => usc.user_tbl.user_id == userId)
                //    .Select(usc => usc.subsubcatId)
                //    .ToList();

                //ViewBag.SelectedSubSubcategoryIds = selectedSubSubcategoryIds;

                //return PartialView("_SubSubCategoriesPartial", allSubSubcategories);

                var selectedSubcategoryIds = db.user_subcategories_tbl
            .Where(usc => usc.uid == userId)
            .Select(usc => usc.subcatId)
            .ToList();

                // Get all sub-subcategories that belong to the selected subcategories
                var allSubSubcategories = db.sub_sub_category_tbl
                    .Where(ssc => selectedSubcategoryIds.Contains(ssc.fkSubCatId)) // Filter by selected subcategories
                    .Include(ssc => ssc.sub_category_tbl) // Include subcategory info
                    .ToList();

                var selectedSubSubcategoryIds = db.user_sub_subcategory_tbl
                    .Where(usc => usc.user_tbl.user_id == userId)
                    .Select(usc => usc.subsubcatId)
                    .ToList();

                ViewBag.SelectedSubSubcategoryIds = selectedSubSubcategoryIds;

                return PartialView("_SubSubCategoriesPartial", allSubSubcategories);
            }
        }

        [HttpPost]
        
        public ActionResult SubmitSubSubCategories(List<int> subSubcategories)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                try
                {
                    int userId = int.Parse(Session["userid"].ToString());

                    if (subSubcategories == null || !subSubcategories.Any())
                    {
                        return Json(new { success = false, message = "No sub-subcategories selected." });
                    }

                    foreach (var subSubcatId in subSubcategories)
                    {
                        var subSubcategoryExists = db.sub_sub_category_tbl.Any(ssc => ssc.sub_sub_catId == subSubcatId);
                        if (!subSubcategoryExists)
                        {
                            return Json(new { success = false, message = "Invalid sub-subcategory selection." });
                        }

                        if (!db.user_sub_subcategory_tbl.Any(usc => usc.uid == userId && usc.subsubcatId == subSubcatId))
                        {
                            var userSubSubcategory = new user_sub_subcategory_tbl
                            {
                                uid = userId,
                                subsubcatId = subSubcatId,
                                catId = db.sub_sub_category_tbl
                                            .Where(ssc => ssc.sub_sub_catId == subSubcatId)
                                            .Select(ssc => ssc.sub_category_tbl.fkcat_id)
                                            .FirstOrDefault(),
                                subcatId = db.sub_sub_category_tbl
                                            .Where(ssc => ssc.sub_sub_catId == subSubcatId)
                                            .Select(ssc => ssc.fkSubCatId)
                                            .FirstOrDefault()
                            };
                            db.user_sub_subcategory_tbl.Add(userSubSubcategory);
                        }
                    }

                    db.SaveChanges();

                    return Json(new { success = true, message = "Sub-Subcategories saved successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }
        }

        //public ActionResult SubmitSubSubCategories(List<int> subSubcategories)
        //{
        //    try
        //    {
        //        int userId = int.Parse(Session["userid"].ToString());

        //        if (subSubcategories == null || !subSubcategories.Any())
        //        {
        //            return Json(new { success = false, message = "No sub-subcategories selected." });
        //        }

        //        foreach (var subSubcatId in subSubcategories)
        //        {
        //            var subSubcategoryExists = db.sub_sub_category_tbl.Any(ssc => ssc.sub_sub_catId == subSubcatId);
        //            if (!subSubcategoryExists)
        //            {
        //                return Json(new { success = false, message = "Invalid sub-subcategory selection." });
        //            }

        //            var userSubSubcategory = new user_sub_subcategory_tbl
        //            {
        //                uid = userId,
        //                subsubcatId = subSubcatId,
        //                catId = db.sub_sub_category_tbl
        //                            .Where(ssc => ssc.sub_sub_catId == subSubcatId)
        //                            .Select(ssc => ssc.sub_category_tbl.fkcat_id)
        //                            .FirstOrDefault(),
        //                subcatId = db.sub_sub_category_tbl
        //                            .Where(ssc => ssc.sub_sub_catId == subSubcatId)
        //                            .Select(ssc => ssc.fkSubCatId)
        //                            .FirstOrDefault()
        //            };

        //            db.user_sub_subcategory_tbl.Add(userSubSubcategory);
        //        }

        //        db.SaveChanges();

        //        var selectedSubSubcategories = db.user_sub_subcategory_tbl
        //            .Where(usc => usc.uid == userId)
        //            .Select(usc => new
        //            {
        //                usc.sub_sub_category_tbl.sub_sub_catId,
        //                usc.sub_sub_category_tbl.sub_sub_catName
        //            })
        //            .ToList();

        //        return Json(new { success = true, selectedSubSubcategories = selectedSubSubcategories });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = "Error: " + ex.Message });
        //    }
        //}

        public ActionResult Create()
        {
            List<category_tbl> catmst = new List<category_tbl>();
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




                List<sub_category_tbl> subcatMst = new List<sub_category_tbl>();


                var allDataSub = db.sub_category_tbl.ToList();
                foreach (var item in allDataSub)
                {
                    subcatMst.Add(new sub_category_tbl
                    {
                        subcat_id = int.Parse(item.subcat_id.ToString()),
                        subcat_name = item.subcat_name.ToString(),
                    });


                }
                ViewBag.subCat = new SelectList(subcatMst, "subcat_id", "subcat_name");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Create(int cat_id , string subcat_name , int subcat_id , string sub_sub_catName)
        {
            {
                sub_sub_category_tbl ss = new sub_sub_category_tbl();
                ss.created_on = DateTime.Now;
                ss.created_by = "gaj";
                ss.fkCatId = cat_id;
                ss.fkSubCatId = subcat_id;
                ss.sub_sub_catName =sub_sub_catName;
                var ssub = db.sub_sub_category_tbl.Add(ss);
                int a = db.SaveChanges();
                if(a > 0)
                {
                    TempData["InsertMsg"] = "<script>alert('Inserted Successfully')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index", "SubSubCategory");
                }
                else
                {
                    TempData["InsertMsg"] = "<script>alert('Failed Inserting')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index", "SubSubCategory");
                }

            }
        }

        public JsonResult GetSubcat(int categoryid)
        {
            List<sub_category_tbl> subcatmst = new List<sub_category_tbl>();
            using (ExpensesEntities db = new ExpensesEntities())
            {
                //var allData = db.sub_category_tbl.Include(i => i.category_tbl).Where(i => i.fkcat_id == categoryid).ToList();
                //return Json(new { categoryid });

                var allData = db.sub_category_tbl
                          .Where(i => i.fkcat_id == categoryid)
                          .ToList();

                foreach (var item in allData)
                {
                    subcatmst.Add(new sub_category_tbl {
                        subcat_id = item.subcat_id,
                        subcat_name = item.subcat_name, 
                    });

                }

                // Return the subcategories as JSON
                return Json(subcatmst, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Edit(int id)
        {
            List<category_tbl> catmst = new List<category_tbl>();
            List<sub_category_tbl> subcatmst = new List<sub_category_tbl>();

            {
                var allData = db.category_tbl.ToList();
                var catmstt = allData.Select(item => new category_tbl
                {
                    cat_id = item.cat_id,
                    cat_name = item.cat_name,
                }).ToList();

                ViewBag.CAT = new SelectList(catmstt, "cat_id", "cat_name");

                //var subCategoryEntity = db.sub_category_tbl.Find(id).fkcat_id;
                var subCategoryEntity = db.sub_sub_category_tbl.FirstOrDefault(x => x.sub_sub_catId == id);

                if (subCategoryEntity == null)
                {
                    TempData["ErrorMsg"] = "<script>alert('SubCategory not found')</script>";
                    return RedirectToAction("Index", "SubSubCategory");
                }

                //var subCategoryViewModel = new Expenses_Management_System.Models.subcategory
                //{
                //    subcat_id = subCategoryEntity.subcat_id,
                //    subcat_name = subCategoryEntity.subcat_name,
                //    cat_id = subCategoryEntity.fkcat_id,
                //    created_on = subCategoryEntity.created_on,
                //    created_by = subCategoryEntity.created_by
                //};

                // Sub Category.

                var allSubData = db.sub_category_tbl.ToList();
                var subcatmstt = allSubData.Select(item => new sub_category_tbl
                {
                    subcat_id = item.subcat_id,
                    subcat_name = item.subcat_name,
                }).ToList();

                ViewBag.SubCAT = new SelectList(subcatmstt, "subcat_id", "subcat_name");

                var subsubCategoryEntity = db.sub_sub_category_tbl.Find(id);
                if (subsubCategoryEntity == null)
                {
                    TempData["ErrorMsg"] = "<script>alert('Sub SubCategory not found')</script>";
                    return RedirectToAction("Index", "SubSubCategory");
                }

                var subsubCategoryViewModel = new Expenses_Management_System.Models.SubSubCategory
                {
                    sub_sub_catId = subsubCategoryEntity.sub_sub_catId,
                    sub_sub_catName = subsubCategoryEntity.sub_sub_catName,
                    subcat_id = int.Parse(subsubCategoryEntity.fkSubCatId.ToString()),
                    cat_id = int.Parse(subsubCategoryEntity.fkCatId.ToString()),
                    created_on = subsubCategoryEntity.created_on,
                    created_by = subsubCategoryEntity.created_by,
                };
                return View(subsubCategoryViewModel);
            }

            
        }

        [HttpPost]
        public ActionResult Edit(Expenses_Management_System.Models.SubSubCategory ss)
        {
            if (ModelState.IsValid)
            {
                {
                    var existingCat = db.sub_sub_category_tbl.Find(ss.sub_sub_catId);
                    if (existingCat == null)
                    {
                        TempData["UpdateSubMsg"] = "<script>alert('SubSubCategory not found')</script>";
                        return RedirectToAction("Index", "SubCategory");
                    }

                    existingCat.sub_sub_catName = ss.sub_sub_catName;
                    existingCat.fkSubCatId = ss.subcat_id;
                    existingCat.fkCatId = ss.cat_id;
                    existingCat.created_on = DateTime.Now;
                    existingCat.created_by = "Gaj";

                    db.Entry(existingCat).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DBConcurrencyException)
                    {
                        TempData["UpdateSubMsg"] = "<script>alert('Concurrency error occurred while updating SubCategory')</script>";
                    }
                    catch (Exception ex)
                    {
                        TempData["UpdateSubMsg"] = $"<script>alert('Error: {ex.Message}')</script>";
                    }
                }
            }
            else
            {
                TempData["UpdateSubMsg"] = "<script>alert('Model state is invalid')</script>";
            }

            return RedirectToAction("Index", "SubSubCategory");
        }
        public ActionResult Delete(int id)
        {
            {
                if(id > 0)
                {
                    var ssCatId = db.sub_sub_category_tbl.Where(model => model.sub_sub_catId==id).FirstOrDefault();
                    if (ssCatId != null)
                    {
                        db.Entry(ssCatId).State = EntityState.Deleted;
                        int a = db.SaveChanges();
                        if (a > 0)
                        {
                            return RedirectToAction("Index" , "SubSubCategory");
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
            {
<<<<<<< HEAD
                var ssCatId = db.sub_sub_category_tbl.Include(i => i.category_tbl).Include(i => i.sub_category_tbl).Where(model => model.sub_sub_catId == id).FirstOrDefault();
                return View(ssCatId);
=======
>>>>>>> master
            }

        }

    }
}