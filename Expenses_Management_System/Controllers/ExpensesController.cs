using Expenses_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data;

namespace Expenses_Management_System.Controllers
{
    public class ExpensesController : Controller
    {
        // GET: Expenses
        public ActionResult Index()
        {
            using (EMSEntities8 db = new EMSEntities8())
            {
                int id =int.Parse( Session["userid"].ToString());
                  var data = db.expenses_tbl.Include(i => i.category_tbl).Include(i => i.sub_category_tbl).Include(i => i.user_tbl).Where(i => i.fkUserId == id).ToList();
                //var data = db.expenses_tbl.Where(i => i.fkUserId==id).ToList();
                return View(data);
            }
        }
        public ActionResult Create()
        {
            List<category_tbl> CatMaster = new List<category_tbl>();
            List<sub_category_tbl> SubCatMaster = new List<sub_category_tbl>();
            List<sub_sub_category_tbl> SubSubCatMaster = new List<sub_sub_category_tbl>();

            using (EMSEntities8 db = new EMSEntities8())
            {
                var CatData = db.category_tbl.ToList();
                foreach (var item in CatData)
                {
                    CatMaster.Add(new category_tbl
                    {
                        cat_id = int.Parse(item.cat_id.ToString()),
                        cat_name = item.cat_name.ToString(),
                    });
                };

                ViewBag.catMsg = new SelectList(CatMaster, "cat_id", "cat_name");

                var SubCatData = db.sub_category_tbl.ToList();
                foreach (var item in SubCatData)
                {
                    SubCatMaster.Add(new sub_category_tbl
                    {
                        subcat_id = int.Parse(item.subcat_id.ToString()),
                        subcat_name = item.subcat_name.ToString(),
                    });
                }

                ViewBag.subCatMsg = new SelectList(SubCatMaster, "subcat_id", "subcat_name");

                var SubSubCatData = db.sub_sub_category_tbl.ToList();
                foreach (var item in SubSubCatData)
                {
                    SubSubCatMaster.Add(new sub_sub_category_tbl
                    {
                        sub_sub_catId = int.Parse(item.sub_sub_catId.ToString()),
                        sub_sub_catName = item.sub_sub_catName.ToString(),
                    });
                }

                ViewBag.subsubCatMsg = new SelectList(SubSubCatMaster, "sub_sub_catId", "sub_sub_catName");


                return View();
            }
        }
        [HttpPost]
        public ActionResult Create(int cat_id, int subcat_id, string monthly_income, string item_name, string item_qty, string total_price, string remark , string user_name )
        {
            using (EMSEntities8 db = new EMSEntities8())
            {
                try
                {
                    expenses_tbl e = new expenses_tbl();
                    e.created_on = DateTime.Now;
                    e.created_by = "gaj";
                    e.fkCatId  = cat_id;
                    e.fkSubCatId = subcat_id;
                    e.fkUserId = int.Parse(Session["userid"].ToString());
                    e.monthly_income = monthly_income;
                    e.item_name = item_name;
                    e.item_qty = item_qty;
                    e.total_price = total_price;
                    e.remark = remark;
                    //e.user_name = user_name;

                    db.expenses_tbl.Add(e);
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["InsertMsg"] = "<script>alert('Inserted Successfully')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Expenses");
                    }
                    else
                    {
                        TempData["InsertMsg"] = "<script>alert('Failed to Insert')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Expenses");
                    }
                }
                catch (Exception ex)
                {
                    TempData["ExceptionMsg"] = $"<script>alert('Error: {ex.Message}')</script>";
                    return View("Index", "Expenses");
                }

            }
        }

        public ActionResult Edit(int id)
        {
            List<category_tbl> CatMaster = new List<category_tbl>();
            List<sub_category_tbl> SubMaster = new List<sub_category_tbl>();
            List<sub_sub_category_tbl> SubSubMaster = new List<sub_sub_category_tbl>();
            using (EMSEntities8 db = new EMSEntities8())
            {
                var CatData = db.category_tbl.ToList();
                foreach (var item in CatData)
                {
                    CatMaster.Add(new category_tbl
                    {
                        cat_id = int.Parse(item.cat_id.ToString()),
                        cat_name = item.cat_name.ToString(),
                    });
                };
                ViewBag.CatMsg = new SelectList(CatMaster, "cat_id", "cat_name");

                //var SubCategoryEntity = db.sub_category_tbl.ToList();
                //if (SubCategoryEntity == null)
                //{
                //    TempData["ErrorMsg"] = "<script>alert('SubCategory not found')</script>";
                //    //return RedirectToAction("Edit", "Expenses");
                //}

                //var subCategoryViewModel = new subcategory
                //{
                //    subcat_id = SubCategoryEntity.subcat_id,
                //    subcat_name = SubCategoryEntity.subcat_name,
                //    cat_id = SubCategoryEntity.fkcat_id,
                //    created_on = SubCategoryEntity.created_on,
                //    created_by = SubCategoryEntity.created_by
                //};


                var SubCat = db.sub_category_tbl.ToList();
                foreach (var item in SubCat)
                {
                    SubMaster.Add(new sub_category_tbl
                    {
                        subcat_id = int.Parse(item.subcat_id.ToString()),
                        subcat_name = item.subcat_name.ToString(),
                    });
                };

                ViewBag.SubMsg = new SelectList(SubCat, "subcat_id", "subcat_name");

                //var SubSubCategoryEntity = db.sub_sub_category_tbl.Find(id);
                //if (SubSubCategoryEntity == null)
                //{
                //    TempData["ErrorMsg"] = "<script>alert('SubSubCategory not found')</script>";
                //    //return RedirectToAction("Edit", "Expenses");
                //}

                //var subSubCategoryViewModel = new SubSubCategory
                //{
                //    sub_sub_catId = SubSubCategoryEntity.sub_sub_catId,
                //    subcat_name = SubSubCategoryEntity.sub_sub_catName,
                //    subcat_id = int.Parse(SubSubCategoryEntity.fkSubCatId.ToString()),
                //    cat_id = int.Parse(SubSubCategoryEntity.fkCatId.ToString()),
                //    created_on = SubSubCategoryEntity.created_on,
                //    created_by = SubSubCategoryEntity.created_by,
                //};

                var SubSubData = db.sub_sub_category_tbl.ToList();
                foreach (var item in SubSubData)
                {
                    SubSubMaster.Add(new sub_sub_category_tbl
                    {
                        sub_sub_catId = int.Parse(item.sub_sub_catId.ToString()),
                        sub_sub_catName = item.sub_sub_catName.ToString(),
                    });
                }
                ViewBag.SubSubMsg = new SelectList(SubSubMaster, "sub_sub_catId", "sub_sub_catName");

                //var userEntity = db.user_tbl.Find(id);
                //if (userEntity == null)
                //{
                //    TempData["ErrorMsg"] = "<script>alert('User Id not found')</script>";
                //    //return RedirectToAction("Edit", "Expenses");
                //}

                //var UserViewModel = new user_tbl
                //{
                //    //user_id = userEntity.user_id,
                //    user_name = userEntity.user_name,
                //};

                var ExpenseId = db.expenses_tbl.Find(id);
                if(ExpenseId == null)
                {
                    TempData["ErrorMsg"] = "<script>alert('Expense Id not found')</script>";
                    
                }
                var expenseViewModel = new expenses_tbl
                {
                    exp_id = ExpenseId.exp_id,

                };
                //return RedirectToAction("Edit", "Expenses");
                //return View("Edit");
                return View(ExpenseId);


            }
        }

        [HttpPost]
        public ActionResult Edit(expenses_tbl expensesData)
        {

            using (EMSEntities8 db = new EMSEntities8())
            {
                var existingExpensesData = db.expenses_tbl.Find(expensesData.exp_id);
                if (existingExpensesData == null)
                {
                    TempData["UpdateSubMsg"] = "<script>alert('Expenses not found')</script>";
                    return RedirectToAction("Index", "Expenses");
                }

                existingExpensesData.monthly_income = expensesData.monthly_income;
                existingExpensesData.item_name = expensesData.item_name;
                existingExpensesData.item_qty = expensesData.item_qty;
                existingExpensesData.total_price = expensesData.total_price;
                existingExpensesData.remark = expensesData.remark;
                existingExpensesData.created_by = "Gaj";
                existingExpensesData.created_on = DateTime.Now;
                existingExpensesData.fkCatId = 1;
                

                //db.Entry(expensesData).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    TempData["UpdateSubMsg"] = "<script>alert('Sub-Sub-Category updated successfully')</script>";
                    return RedirectToAction("Index", "Expenses");
                }
                
                catch (Exception ex)
                {
                    TempData["UpdateSubMsg"] = $"<script>alert('Error: {ex.Message}')</script>";
                }
                return View(expensesData);
            }
        }

        public ActionResult Details(int id)
        {
            using(EMSEntities8 db = new EMSEntities8())
            {
                var details = db.expenses_tbl.Include(i => i.category_tbl).Include(i => i.sub_category_tbl).Include(i => i.user_tbl).Where(model => model.exp_id == id).FirstOrDefault();
                return View(details);
            }
        }

        public ActionResult Delete(int id)
        {
            using(EMSEntities8 db = new EMSEntities8())
            {
                if(id > 0)
                {
                    var expId = db.expenses_tbl.Where(x => x.exp_id == id).FirstOrDefault();
                    if(expId != null)
                    {
                        db.Entry(expId).State = EntityState.Deleted;
                        db.SaveChanges();
                    }
                }
                return View(id);
            }
        }
    }
}
