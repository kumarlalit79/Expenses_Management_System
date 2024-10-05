using Expenses_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data;
using System.IO;

namespace Expenses_Management_System.Controllers
{
    public class ExpensesController : BaseController
    {
        public ExpensesController(MenuService menuService) : base(menuService)
        {
        }
        // GET: Expenses


        public ActionResult Index(int? monthno)
        {
            if (Session["Type"] == null)
            {
                return RedirectToAction("Index", "SignIn");
            }

            using (ExpensesEntities db = new ExpensesEntities())
            {
                int id = int.Parse(Session["userid"].ToString());

                // Get all expenses for the user
                var data = db.expenses_tbl.Include(i => i.category_tbl)
                                          .Include(i => i.sub_category_tbl)
                                          .Include(i => i.sub_sub_category_tbl)
                                          .Include(i => i.user_tbl)
                                          .Where(i => i.fkUserId == id);

                // Filter data by month if a month is selected
                if (monthno.HasValue)
                {
                    data = data.Where(i => i.sdate.Month == monthno.Value);
                }

                var expensesList = data.ToList();

                // Create month dropdown
                var distinctMonths = db.expenses_tbl.Where(e => e.fkUserId == id)
                    .AsEnumerable()
                    .Select(e => new
                    {
                        MonthNumber = e.sdate.ToString("MM"),
                        MonthName = e.sdate.ToString("MMM")
                    })
                    .Distinct()
                    .OrderByDescending(m => m.MonthNumber)
                    .ToList();

                List<MonthM> mm = new List<MonthM>();
                foreach (var item in distinctMonths)
                {
                    mm.Add(new MonthM
                    {
                        monthno = int.Parse(item.MonthNumber),
                        Monthname = item.MonthName
                    });
                }
                ViewBag.Month = new SelectList(mm, "monthno", "Monthname", monthno);

                if (Request.IsAjaxRequest())
                {
                    // Return the updated view data without partial view
                    return View("Index", expensesList);
                }

                return View(expensesList);
            }
        }

        public JsonResult GetLatestMonth()
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                int id = int.Parse(Session["userid"].ToString());

                // Determine the latest month
                var latestMonth = db.expenses_tbl
                                     .Where(e => e.fkUserId == id)
                                     .AsEnumerable()
                                     .Select(e => e.sdate.Month)
                                     .Distinct()
                                     .OrderByDescending(m => m)
                                     .FirstOrDefault();

                return Json(latestMonth, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Create()
        {
            List<category_tbl> CatMaster = new List<category_tbl>();
            List<sub_category_tbl> SubCatMaster = new List<sub_category_tbl>();
            List<sub_sub_category_tbl> SubSubCatMaster = new List<sub_sub_category_tbl>();
            List<PaymentMode_tbl> paymode = new List<PaymentMode_tbl>();
            List<Assbank> assbank = new List<Assbank>();

            using (ExpensesEntities db = new ExpensesEntities())
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

                var paym = db.PaymentMode_tbl.ToList();
                foreach (var item in paym)
                {
                    paymode.Add(new PaymentMode_tbl
                    {
                        PM_id = int.Parse(item.PM_id.ToString()),
                        PaymentMode = item.PaymentMode.ToString(),
                    });
                };

                ViewBag.paym = new SelectList(paymode, "PM_id", "PaymentMode");


                assbank.Add(new Assbank
                {
                    acid = 0,
                    bankname = "SelectBank",
                });


                ViewBag.bank = new SelectList(assbank, "acid", "bankname");


                return View();
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
                    subcatmst.Add(new sub_category_tbl
                    {
                        subcat_id = item.subcat_id,
                        subcat_name = item.subcat_name,
                    });

                }

                // Return the subcategories as JSON
                return Json(subcatmst, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSubSubcat(int subcategoryid, int categoryid)
        {
            List<sub_sub_category_tbl> subcatmst = new List<sub_sub_category_tbl>();
            using (ExpensesEntities db = new ExpensesEntities())
            {
                //var allData = db.sub_category_tbl.Include(i => i.category_tbl).Where(i => i.fkcat_id == categoryid).ToList();
                //return Json(new { categoryid });

                var allData = db.sub_sub_category_tbl.Where(i => i.fkSubCatId == subcategoryid && i.fkCatId == categoryid).ToList();

                foreach (var item in allData)
                {
                    subcatmst.Add(new sub_sub_category_tbl
                    {
                        sub_sub_catId = item.sub_sub_catId,
                        sub_sub_catName = item.sub_sub_catName,
                    });

                }

                // Return the subcategories as JSON
                return Json(subcatmst, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBank(int paymodeid)
        {
            List<AssignCardToBank_tbl> Assbankmst = new List<AssignCardToBank_tbl>();
            List<Assbank> bankmst = new List<Assbank>();
            using (ExpensesEntities db = new ExpensesEntities())
            {
                // var data = db.AssignCardToBank_tbl.Include(i => i.BankCard).Include(i => i.PaymentMode_tbl).ToList().Where(i => i.fkpaymodeid == paymodeid);
                var data = db.AssignCardToBank_tbl
           .Include(i => i.BankCard) // Ensure this matches the actual navigation property name
           .Include(i => i.PaymentMode_tbl) // Ensure this matches the actual navigation property name
           .Where(i => i.fkpaymodeid == paymodeid)
           .ToList();
                //foreach (var item in data)
                //{
                //    Console.WriteLine($"BankCard: {item.BankCard?.CardName}, PaymentMode: {item.PaymentMode_tbl?.PM_id}");
                //}
                foreach (var item in data)
                {
                    bankmst.Add(new Assbank
                    {


                        acid = item.ACID,
                        bankname = item.BankCard?.CardName,
                    });
                }

                // Return the subcategories as JSON
                return Json(bankmst, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Create(int cat_id, int subcat_id, string monthly_income, string item_name, string item_qty, string total_price, string remark, string user_name, int sub_sub_catId, int acid, DateTime sdate, HttpPostedFileBase ImageFile)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                try
                {
                    var details = db.sub_sub_category_tbl.Include(i => i.category_tbl).Include(i => i.sub_category_tbl).Where(c => c.fkCatId == cat_id &&
                           c.fkSubCatId == subcat_id &&
                            c.sub_sub_catId == sub_sub_catId).ToList();
                    var path = "";
                    var newfilename = "";
                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {
                        // Generate a unique filename
                        var fileName = Path.GetFileName(ImageFile.FileName);

                        //foreach (var item in details)
                        //{
                        //    newfilename = item.category_tbl?.cat_name +" "+item.sub_category_tbl?.subcat_name+" "+item.sub_sub_catName+ fileName+sdate;

                        //}
                        DateTime now = DateTime.Now;

                        // Format the date and time separately
                        string datePart = now.ToString("yyyy-MM-dd"); // Format: "2024-08-06"
                        string timePart = now.ToString("HH-mm-ss");   // Format: "21-35-47" (note the dashes instead of colons)

                        // Combine date and time with an underscore
                        string timestamp = $"{datePart}_{timePart}";

                        // Create a valid file path
                        newfilename = $"{Session["Mobile"].ToString()}_{timestamp}_{fileName}";

                        //  newfilename = Session["Mobile"].ToString() + "_" + dateTimeString + "_" + fileName;
                        path = Path.Combine(Server.MapPath("~/Upload_Img"), newfilename);

                        // Save the file to the server
                        ImageFile.SaveAs(path);
                    }

                    expenses_tbl e = new expenses_tbl();
                    e.created_on = DateTime.Now;
                    e.created_by = "gaj";
                    e.fkCatId = cat_id;
                    e.fkSubCatId = subcat_id;
                    e.fkUserId = int.Parse(Session["userid"].ToString());
                    // e.monthly_income = monthly_income;
                    e.item_name = item_name;
                    e.item_qty = item_qty;
                    e.total_price = total_price;
                    e.remark = remark;
                    //e.user_name = user_name;
                    e.fkSubSubCatId = sub_sub_catId;
                    e.FKACID = acid;
                    e.sdate = sdate;
                    e.docpath = Path.Combine("~/Upload_Img", newfilename); ;
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



        //public ActionResult Edit(int id)
        //{
        //    List<category_tbl> CatMaster = new List<category_tbl>();
        //    List<sub_category_tbl> SubMaster = new List<sub_category_tbl>();
        //    List<sub_sub_category_tbl> SubSubMaster = new List<sub_sub_category_tbl>();
        //    List<PaymentMode_tbl> paymode = new List<PaymentMode_tbl>();
        //    List<Assbank> assbank = new List<Assbank>();
        //    using (ExpensesEntities db = new ExpensesEntities())
        //    {
        //        var CatData = db.category_tbl.ToList();
        //        foreach (var item in CatData)
        //        {
        //            CatMaster.Add(new category_tbl
        //            {
        //                cat_id = int.Parse(item.cat_id.ToString()),
        //                cat_name = item.cat_name.ToString(),
        //            });
        //        };
        //        ViewBag.CatMsg = new SelectList(CatMaster, "cat_id", "cat_name");




        //        var SubCat = db.sub_category_tbl.ToList();
        //        foreach (var item in SubCat)
        //        {
        //            SubMaster.Add(new sub_category_tbl
        //            {
        //                subcat_id = int.Parse(item.subcat_id.ToString()),
        //                subcat_name = item.subcat_name.ToString(),
        //            });
        //        };

        //        ViewBag.SubMsg = new SelectList(SubCat, "subcat_id", "subcat_name");



        //        var SubSubData = db.sub_sub_category_tbl.ToList();
        //        foreach (var item in SubSubData)
        //        {
        //            SubSubMaster.Add(new sub_sub_category_tbl
        //            {
        //                sub_sub_catId = int.Parse(item.sub_sub_catId.ToString()),
        //                sub_sub_catName = item.sub_sub_catName.ToString(),
        //            });
        //        }
        //        ViewBag.SubSubMsg = new SelectList(SubSubMaster, "sub_sub_catId", "sub_sub_catName");

        //        var paym = db.PaymentMode_tbl.ToList();
        //        foreach (var item in paym)
        //        {
        //            paymode.Add(new PaymentMode_tbl
        //            {
        //                PM_id = int.Parse(item.PM_id.ToString()),
        //                PaymentMode = item.PaymentMode.ToString(),
        //            });
        //        };

        //        ViewBag.paym = new SelectList(paymode, "PM_id", "PaymentMode");

        //        assbank.Add(new Assbank
        //        {
        //            acid = 0,
        //            bankname = "SelectBank",
        //        });


        //        ViewBag.bank = new SelectList(assbank, "acid", "bankname");

        //        var ExpenseId = db.expenses_tbl.Find(id);
        //        if (ExpenseId == null)
        //        {
        //            TempData["ErrorMsg"] = "<script>alert('Expense Id not found')</script>";

        //        }


        //        return View(ExpenseId);


        //    }
        //}

        public ActionResult Edit(int id)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                // Fetching categories
                var CatData = db.category_tbl.ToList();
                ViewBag.CatMsg = new SelectList(CatData, "cat_id", "cat_name");

                // Fetching subcategories
                var SubCat = db.sub_category_tbl.ToList();
                ViewBag.SubMsg = new SelectList(SubCat, "subcat_id", "subcat_name");

                // Fetching sub-subcategories
                var SubSubData = db.sub_sub_category_tbl.ToList();
                ViewBag.SubSubMsg = new SelectList(SubSubData, "sub_sub_catId", "sub_sub_catName");

                // Fetching payment modes
                List<PaymentMode_tbl> paymode = new List<PaymentMode_tbl>();
                var paym = db.PaymentMode_tbl.ToList();
                foreach (var item in paym)
                {
                    paymode.Add(new PaymentMode_tbl
                    {
                        PM_id = item.PM_id,
                        PaymentMode = item.PaymentMode
                    });
                }
                ViewBag.paym = new SelectList(paymode, "PM_id", "PaymentMode");



                var assbank = new List<Assbank>
                {
                    new Assbank { acid = 0, bankname = "SelectBank" }
                };
                ViewBag.bank = new SelectList(assbank, "acid", "bankname");




                // Fetch the expense record from database
                var ExpenseId = db.expenses_tbl.Find(id);
                if (ExpenseId == null)
                {
                    TempData["ErrorMsg"] = "<script>alert('Expense Id not found')</script>";
                    return RedirectToAction("Index");
                }

                // Map expenses_tbl to Expenses ViewModel
                var expenseViewModel = new Expenses
                {
                    exp_id = ExpenseId.exp_id,
                    item_name = ExpenseId.item_name,
                    item_qty = ExpenseId.item_qty,
                    total_price = ExpenseId.total_price,
                    remark = ExpenseId.remark,
                    sdate = ExpenseId.sdate,
                    created_on = ExpenseId.created_on,
                    cat_id = ExpenseId.fkCatId.HasValue ? ExpenseId.fkCatId.Value : 0,
                    subcat_id = ExpenseId.fkSubCatId.HasValue ? ExpenseId.fkSubCatId.Value : 0,
                    sub_sub_catId = ExpenseId.fkSubSubCatId.HasValue ? ExpenseId.fkSubSubCatId.Value.ToString() : "0",
                    //PaymentMode = ExpenseId.FKACID.HasValue ? ExpenseId.FKACID.ToString() : string.Empty,
                    PaymentMode = ExpenseId.FKACID.HasValue ? ExpenseId.FKACID.ToString() : string.Empty, // This line
                };

                if (ExpenseId.FKACID.HasValue)
                {
                    var banks = db.AssignCardToBank_tbl
                                .Include(i => i.BankCard)
                                .Where(i => i.fkpaymodeid == ExpenseId.FKACID.Value)
                                .Select(item => new Assbank
                                {
                                    acid = item.ACID,
                                    bankname = item.BankCard.CardName,
                                })
                                .ToList();

                    ViewBag.bank = new SelectList(banks, "acid", "bankname", ExpenseId.FKACID.Value);
                }
                else
                {
                    ViewBag.bank = new SelectList(new List<Assbank>(), "acid", "bankname");
                }

                return View(expenseViewModel);
            }
        }


        public JsonResult GetSubcatEdit(int categoryid)
        {
            List<sub_category_tbl> subcatmst = new List<sub_category_tbl>();
            using (ExpensesEntities db = new ExpensesEntities())
            {
                var allData = db.sub_category_tbl
                          .Where(i => i.fkcat_id == categoryid)
                          .ToList();

                foreach (var item in allData)
                {
                    subcatmst.Add(new sub_category_tbl
                    {
                        subcat_id = item.subcat_id,
                        subcat_name = item.subcat_name,
                    });

                }

                // Return the subcategories as JSON
                return Json(subcatmst, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSubSubcatEdit(int subcategoryid, int categoryid)
        {
            List<sub_sub_category_tbl> subcatmst = new List<sub_sub_category_tbl>();
            using (ExpensesEntities db = new ExpensesEntities())
            {
                //var allData = db.sub_category_tbl.Include(i => i.category_tbl).Where(i => i.fkcat_id == categoryid).ToList();
                //return Json(new { categoryid });

                var allData = db.sub_sub_category_tbl.Where(i => i.fkSubCatId == subcategoryid && i.fkCatId == categoryid).ToList();

                foreach (var item in allData)
                {
                    subcatmst.Add(new sub_sub_category_tbl
                    {
                        sub_sub_catId = item.sub_sub_catId,
                        sub_sub_catName = item.sub_sub_catName,
                    });

                }

                // Return the subcategories as JSON
                return Json(subcatmst, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBankEdit(int paymodeid)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                var data = db.AssignCardToBank_tbl
                             .Include(i => i.BankCard)
                             .Include(i => i.PaymentMode_tbl)
                             .Where(i => i.fkpaymodeid == paymodeid)
                             .ToList();

                var bankmst = data.Select(item => new Assbank
                {
                    acid = item.ACID,
                    bankname = item.BankCard?.CardName,
                }).ToList();

                return Json(bankmst, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Edit(expenses_tbl expensesData)
        {

            using (ExpensesEntities db = new ExpensesEntities())
            {
                var existingExpensesData = db.expenses_tbl.Find(expensesData.exp_id);
                if (existingExpensesData == null)
                {
                    TempData["UpdateSubMsg"] = "<script>alert('Expenses not found')</script>";
                    return RedirectToAction("Index", "Expenses");
                }

                //  existingExpensesData.monthly_income = expensesData.monthly_income;
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
            using (ExpensesEntities db = new ExpensesEntities())
            {

                var details = db.expenses_tbl.Include(i => i.category_tbl).Include(i => i.sub_category_tbl).Include(i => i.user_tbl).Where(model => model.exp_id == id).FirstOrDefault();
                return View(details);
            }
        }


        public ActionResult Delete(int id)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                var expId = db.expenses_tbl.Where(x => x.exp_id == id).FirstOrDefault();
                db.expenses_tbl.Remove(expId);
                db.Entry(expId).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
