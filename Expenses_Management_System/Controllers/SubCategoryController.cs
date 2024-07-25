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
    public class SubCategoryController : Controller
    {
        

        // GET: SubCategory
        public ActionResult Index()
        {
            using (EMSEntities8 db = new EMSEntities8())
            {
                var allData = db.sub_category_tbl.Include(i => i.category_tbl).ToList();
                return View(allData);
            }
            
        }

        public ActionResult Create()
        {
            List<category_tbl> catmst= new List<category_tbl>();
            using (EMSEntities8 db=new EMSEntities8())
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
        public ActionResult Create(int cat_id,string subcat_name)
        {
            using (EMSEntities8 db = new EMSEntities8())
            {
                sub_category_tbl s = new sub_category_tbl();

                s.created_on = DateTime.Now;
                s.created_by = "Lalit";
                s.fkcat_id = cat_id;
                s.subcat_name= subcat_name;
                var sub = db.sub_category_tbl.Add(s);
                int a = db.SaveChanges();
                if(a > 0)
                {
                    TempData["InsertMsg"] = "<script>alert('Inserted Successfully')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index" , "SubCategory");
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
            using (EMSEntities8 db = new EMSEntities8())
            {
                //var catId = db.sub_category_tbl.Where(model => model.subcat_id == id).FirstOrDefault();

                //var allData = db.category_tbl.ToList();

                //foreach (var item in allData)
                //{
                //    catmst.Add(new category_tbl
                //    {
                //        cat_id = int.Parse(item.cat_id.ToString()),
                //        cat_name = item.cat_name.ToString(),
                //    }); ;
                //}

                //ViewBag.CAT = new SelectList(catmst, "cat_id", "cat_name");
                //return View();



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
            using(EMSEntities8 db = new EMSEntities8())
            {
                //sub_category_tbl s = new sub_category_tbl();

                //s.created_on = DateTime.Now;
                //s.created_by = "lalit";
                ////s.fkcat_id = cat_id;
                ////s.subcat_name = subcat_name;
                //db.Entry(s).State = EntityState.Modified;
                //int a = db.SaveChanges();
                //if (a > 0)
                //{
                //    TempData["UpdateSubMsg"] = "<script>alert('Category Updated successfully')</script>";
                //    ModelState.Clear();
                //    return RedirectToAction("Index", "SubCategory");
                //}
                //else
                //{
                //    TempData["UpdateSubMsg"] = "<script>alert('Category not Updated')</script>";
                //    ModelState.Clear();
                //    return RedirectToAction("Index", "SubCategory");
                //}

                if (s.subcat_id == 0)
                {
                    TempData["UpdateSubMsg"] = "<script>alert('Invalid SubCategory ID')</script>";
                    return RedirectToAction("Index", "SubCategory");
                }


                try
                {
                    var existingCat = db.sub_category_tbl.Find(s.subcat_id);
                    if(existingCat == null)
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
                catch(DBConcurrencyException)
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
            using (EMSEntities8 db = new EMSEntities8())
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
            using (EMSEntities8 db = new EMSEntities8())
            {
                var catId = db.sub_category_tbl.Include(i => i.category_tbl).Where(modal => modal.subcat_id == id).FirstOrDefault();
                return View(catId);
            }
          
        }
    }
}