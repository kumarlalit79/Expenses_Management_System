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
    public class SubSubCategoryController : Controller
    {
        // GET: SubSubCategory
        public ActionResult Index()
        {
            using (EMSEntities8 db = new EMSEntities8())
            {
                var data = db.sub_sub_category_tbl.Include(i => i.category_tbl).Include(i => i.sub_category_tbl).ToList();
                return View(data);
            }
        }
        public ActionResult Create()
        {
            List<category_tbl> catmst = new List<category_tbl>();
            using (EMSEntities8 db = new EMSEntities8())
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
            using(EMSEntities8 db = new EMSEntities8())
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

        public ActionResult Edit(int id)
        {
            List<category_tbl> catmst = new List<category_tbl>();
            List<sub_category_tbl> subcatmst = new List<sub_category_tbl>();

            using (EMSEntities8 db = new EMSEntities8())
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
                    return RedirectToAction("Index", "SubSubCategory");
                }

                var subCategoryViewModel = new Expenses_Management_System.Models.subcategory
                {
                    subcat_id = subCategoryEntity.subcat_id,
                    subcat_name = subCategoryEntity.subcat_name,
                    cat_id = subCategoryEntity.fkcat_id,
                    created_on = subCategoryEntity.created_on,
                    created_by = subCategoryEntity.created_by
                };

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
                return View();
            }

            
        }

        [HttpPost]
        public ActionResult Edit(Expenses_Management_System.Models.SubSubCategory ss)
        {
            if (ModelState.IsValid)
            {
                using (EMSEntities8 db = new EMSEntities8())
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
                        TempData["UpdateSubMsg"] = "<script>alert('Sub-Sub-Category updated successfully')</script>";
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
            using (EMSEntities8 db = new EMSEntities8())
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
                            TempData["DeleteMsg"] = "<script>alert('Record Deleted')</script>";
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
            using (EMSEntities8 db = new EMSEntities8())
            {
                var ssCatId = db.sub_sub_category_tbl.Include(i => i.category_tbl).Include(i => i.sub_category_tbl).Where(model => model.sub_sub_catId == id).FirstOrDefault();
                return View(ssCatId);
            }

        }

    }
}