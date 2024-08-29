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
using System.Net;
using System.Drawing;
using System.Text;
using OfficeOpenXml;
using Newtonsoft.Json;

namespace Expenses_Management_System.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController(MenuService menuService) : base(menuService)
        {
        }
        
        // GET: Dashboard
        public ActionResult Index()
        {
            int id = int.Parse(Session["userid"].ToString());
            // Check if the user is authenticated
            if (Session["Type"] == null)
            {
                return RedirectToAction("Index", "SignIn");
            }

            using (ExpensesEntities db = new ExpensesEntities())
            {
                
                List<PaymentMode> pay=new List<PaymentMode>();
                
                var data = db.expenses_tbl
                    .Include(i => i.category_tbl)
                    .Include(i => i.sub_category_tbl)
                    .Include(i => i.sub_sub_category_tbl)
                    .Include(i => i.user_tbl)
                    .Where(i => i.fkUserId == id && i.sdate.Month == 8).ToList();

                var assignCardToBanks = db.AssignCardToBank_tbl.Include(acb => acb.PaymentMode_tbl).ToList();

                // Associate related data with main data
                foreach (var expense in data)
                {
                    expense.AssignCardToBank_tbl = assignCardToBanks.FirstOrDefault(acb => acb.ACID == expense.FKACID);
                }

                //var data = db.expenses_tbl.Where(i => i.fkUserId==id).ToList();
                //foreach (var item in data)
                //{
                //    item.docpath = Server.MapPath(item.docpath);
                //}

                int currentmonth = 8;
                //CategoryWiseSpending(currentmonth);

                //List<Expenses> exp = new List<Expenses>();
                var rawData = db.expenses_tbl
                 .Include(e => e.category_tbl)
                 .Where(e => e.sdate.Month == 8) // Filter data by month
                 .AsEnumerable() // Switch to client-side processing
                 .GroupBy(e => new
                 {
                     cat_name = e.category_tbl.cat_name,
                     MonthNo = e.sdate.Month,
                     MonthName = e.sdate.ToString("MMM")
                 })
                 .Select(g => new
                 {
                     cat_name = g.Key.cat_name,
                     total_price = g.Sum(e => Convert.ToDecimal(e.total_price)),
                     MonthNo = g.Key.MonthNo,
                     MonthName = g.Key.MonthName
                 })
                 .ToList();

                //foreach (var item in rawData)
                //{
                //    exp.Add(new Expenses
                //    {
                //        cat_name = item.cat_name.ToString(),
                //        total_price = item.total_price.ToString(),
                //    });
                //};
                //ViewBag.Data = exp;

                // paymentmode wise report
                var expensesWithDetails = db.expenses_tbl
           .Include(e => e.AssignCardToBank_tbl) // Eager load AssignCardToBank
           .Include(e => e.AssignCardToBank_tbl.PaymentMode_tbl) // Eager load PaymentMode via AssignCardToBank
           .ToList();

                // Perform the grouping and aggregation in-memory
                var result = expensesWithDetails.Where(e => e.sdate.Month == 8) // Filter data by month
.AsEnumerable() // Switch to client-side processing
                    .GroupBy(e => new
                    {
                        // e.AssignCardToBank_tbl.PaymentMode_tbl.PaymentMode
                        PaymentMode = e.AssignCardToBank_tbl.PaymentMode_tbl.PaymentMode,
                        MonthNo = e.sdate.Month,
                        MonthName = e.sdate.ToString("MMM")
                    }
                   
                    
                    ) // Group by PaymentModeName
                    .Select(g => new
                    {
                        PaymentMode = g.Key.PaymentMode,
                        TotalPrice = g.Sum(e => Convert.ToDecimal(e.total_price)),
                        MonthNo = g.Key.MonthNo,
                        MonthName = g.Key.MonthName
                    })
                    .ToList();

                foreach (var item in result)
                {
                    pay.Add(new PaymentMode
                    {
                        PayMode = item.PaymentMode.ToString(),
                        total_price = item.TotalPrice.ToString(),
                    });
                };
                ViewBag.PayModes = JsonConvert.SerializeObject(pay);

                ViewBag.pay = pay;

                var distinctYears = db.expenses_tbl
               .AsEnumerable() // Switch to LINQ to Objects to use the `Year` property
               .Select(e => e.sdate.Year)
               .Distinct()
               .ToList();

                List<YearM> ym = new List<YearM>();
                foreach (var iteam in distinctYears)
                {
                    ym.Add(new YearM
                    {
                        year = int.Parse(iteam.ToString()),

                    });
                };
                ViewBag.Year = new SelectList(ym, "year", "year");


                var distinctMonths = db.expenses_tbl.Where(e => e.fkUserId == id)
                 .AsEnumerable() // Switch to LINQ to Objects
                 .Select(e => new
                 {
                     MonthNumber = e.sdate.ToString("MM"),
                     MonthName = e.sdate.ToString("MMM")
                 })
                 .Distinct()
                 .OrderByDescending(m => m.MonthNumber)// Order by MonthNumber in descending order
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
                ViewBag.Month = new SelectList(mm, "monthno", "Monthname");

                return View(data);
            }
        }
        [HttpGet]
        public ActionResult Monthly_Spending_Details_new(int currentmonth,int Catidno)
        {
            // Example logic: return a JSON object
            //var exampleData = new { message = "Success", month = currentmonth };
            //return Json(exampleData, JsonRequestBehavior.AllowGet);
           
            using (ExpensesEntities db = new ExpensesEntities())
            {
                List<Expenses> exp = new List<Expenses>();
                int id = int.Parse(Session["userid"].ToString());
                var query = db.expenses_tbl
                    .Include(i => i.category_tbl)
                    .Include(i => i.sub_category_tbl)
                    .Include(i => i.sub_sub_category_tbl)
                    .Include(i => i.user_tbl)
                    .Where(i => i.fkUserId == id && i.sdate.Month == currentmonth);

                if (Catidno > 0)
                {
                    query = query.Where(i => i.fkCatId == Catidno);
                }
                // Execute the query and retrieve data
                var data = query.ToList();

                var assignCardToBanks = db.AssignCardToBank_tbl.Include(acb => acb.PaymentMode_tbl).ToList();

                // Associate related data with main data
                foreach (var expense in data)
                {
                    expense.AssignCardToBank_tbl = assignCardToBanks.FirstOrDefault(acb => acb.ACID == expense.FKACID);
                }

                foreach (var item in data)
                {
                    exp.Add(new Expenses
                    {
                        cat_name = item.category_tbl.cat_name,
                        subcat_name = item.sub_category_tbl.subcat_name,
                        sub_sub_catName = item.sub_sub_category_tbl.sub_sub_catName,
                        item_name = item.item_name,
                        total_price = item.total_price,
                        PaymentMode = item.AssignCardToBank_tbl.PaymentMode_tbl.PaymentMode,
                        docpath = item.docpath,
                        cat_id = item.category_tbl.cat_id,

                    });
                }

                return Json(exp, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Monthly_Spending_Details(int currentmonth)
        {
            // Example logic: return a JSON object
            //var exampleData = new { message = "Success", month = currentmonth };
            //return Json(exampleData, JsonRequestBehavior.AllowGet);

            using (ExpensesEntities db = new ExpensesEntities())
            {
                
                List<Expenses> exp = new List<Expenses>();
                int id = int.Parse(Session["userid"].ToString());
                var data = db.expenses_tbl
                    .Include(i => i.category_tbl)
                    .Include(i => i.sub_category_tbl)
                    .Include(i => i.sub_sub_category_tbl)
                    .Include(i => i.user_tbl)
                    .Where(i => i.fkUserId == id && i.sdate.Month == currentmonth).ToList();

                var assignCardToBanks = db.AssignCardToBank_tbl.Include(acb => acb.PaymentMode_tbl).ToList();

                // Associate related data with main data
                foreach (var expense in data)
                {
                    expense.AssignCardToBank_tbl = assignCardToBanks.FirstOrDefault(acb => acb.ACID == expense.FKACID);
                }
              
                foreach (var item in data)
                {
                    exp.Add(new Expenses
                    {
                        cat_name = item.category_tbl.cat_name,
                        subcat_name = item.sub_category_tbl.subcat_name,
                        sub_sub_catName=item.sub_sub_category_tbl.sub_sub_catName,
                        item_name=item.item_name,
                        total_price = item.total_price,
                        PaymentMode=item.AssignCardToBank_tbl.PaymentMode_tbl.PaymentMode,
                        docpath=item.docpath,
                        cat_id=item.category_tbl.cat_id,

                    });
                }

                return Json(exp, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Paymode(int currentmonth)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                int id = int.Parse(Session["userid"].ToString());
                List<PaymentMode> pay = new List<PaymentMode>();
                // paymentmode wise report
                var expensesWithDetails = db.expenses_tbl
               .Include(e => e.AssignCardToBank_tbl) // Eager load AssignCardToBank
               .Include(e => e.AssignCardToBank_tbl.PaymentMode_tbl) // Eager load PaymentMode via AssignCardToBank
               .ToList();

                // Perform the grouping and aggregation in-memory
                var result = expensesWithDetails.Where(e => e.sdate.Month == currentmonth && e.fkUserId == id) // Filter data by month
                            .AsEnumerable() // Switch to client-side processing
                            .GroupBy(e => new
                            {
                                // e.AssignCardToBank_tbl.PaymentMode_tbl.PaymentMode
                                PaymentMode = e.AssignCardToBank_tbl.PaymentMode_tbl.PaymentMode,
                                MonthNo = e.sdate.Month,
                                MonthName = e.sdate.ToString("MMM")
                            }
                            ) // Group by PaymentModeName
                            .Select(g => new
                            {
                                PaymentMode = g.Key.PaymentMode,
                                TotalPrice = g.Sum(e => Convert.ToDecimal(e.total_price)),
                                MonthNo = g.Key.MonthNo,
                                MonthName = g.Key.MonthName
                            })
                            .ToList();

                foreach (var item in result)
                {
                    pay.Add(new PaymentMode
                    {
                        PayMode = item.PaymentMode.ToString(),
                        total_price = item.TotalPrice.ToString(),
                    });
                };
                ViewBag.PayModes = JsonConvert.SerializeObject(pay);

                ViewBag.pay = pay;
                return Json(pay, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult CategoryWiseSpending(int currentmonth)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                //int monthnumber = 8;
                //if (string.IsNullOrEmpty(currentmonth))
                //{
                //    monthnumber = 8;
                //}
                //else
                //{
                //    monthnumber = int.Parse(currentmonth);


                //}

                int id = int.Parse(Session["userid"].ToString());

                List<Expenses> exp = new List<Expenses>();
                var rawData = db.expenses_tbl
                 .Include(e => e.category_tbl)
                 .Where(e => e.sdate.Month == currentmonth &&  e.fkUserId == id) // Filter data by month
                 .AsEnumerable() // Switch to client-side processing
                 .GroupBy(e => new
                 {
                     cat_name = e.category_tbl.cat_name,
                     MonthNo = e.sdate.Month,
                     MonthName = e.sdate.ToString("MMM")
                 })
                 .Select(g => new
                 {
                     cat_name = g.Key.cat_name,
                     total_price = g.Sum(e => Convert.ToDecimal(e.total_price)),
                     MonthNo = g.Key.MonthNo,
                     MonthName = g.Key.MonthName
                 })
                 .ToList();

                foreach (var item in rawData)
                {
                    exp.Add(new Expenses
                    {
                        cat_name = item.cat_name.ToString(),
                        total_price = item.total_price.ToString(),
                    });
                };
                ViewBag.Data = exp;
                return Json(exp, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult Download(string fileName)
        {
            // Validate input
            if (string.IsNullOrEmpty(fileName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid file name.");
            }

            // Construct the file path
            var filePath = Server.MapPath(Path.Combine(fileName));

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound("File not found.");
            }

            // Get the file's content type (you can specify a default if needed)
            var contentType = System.Web.MimeMapping.GetMimeMapping(filePath);

            // Read the file into a byte array
            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Return the file to the client
            return File(fileBytes, contentType, fileName);
        }

        [HttpGet]
        public ActionResult GetMonthWiseWiseRPT()
        {
            int id = int.Parse(Session["userid"].ToString());
            using (ExpensesEntities db = new ExpensesEntities())
            {
                var rawData = db.expenses_tbl.Where(e => e.fkUserId == id)
               .AsEnumerable() // Switch to client-side processing
               .GroupBy(e => new
               {
                  
                   MonthNo = e.sdate.Month,
                   MonthName = e.sdate.ToString("MMM")
               })
               .Select(g => new
               {
                  
                   total_price = g.Sum(e => Convert.ToDecimal(e.total_price)),
                   MonthNo = g.Key.MonthNo,
                   MonthName = g.Key.MonthName
               })
                .OrderBy(result => result.MonthNo) // Order by MonthNo ascending
               .ToList();
                return Json(rawData, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult GetCategoryWiseRPT(string monthn)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                int id = int.Parse(Session["userid"].ToString());

                int monthnumber = 8;
                if ( string.IsNullOrEmpty(monthn))
                {
                    monthnumber = 8;
                }
                else
                {
                    monthnumber = int.Parse(monthn);


                }




                var rawData = db.expenses_tbl
                .Include(e => e.category_tbl)
                .Where(e => e.sdate.Month == monthnumber && e.fkUserId == id) // Filter data by month
                .AsEnumerable() // Switch to client-side processing
                .GroupBy(e => new
                {
                    cat_name = e.category_tbl.cat_name,
                    MonthNo = e.sdate.Month,
                    MonthName = e.sdate.ToString("MMM")
                })
                .Select(g => new
                {
                    cat_name = g.Key.cat_name,
                    total_price = g.Sum(e => Convert.ToDecimal(e.total_price)),
                    MonthNo = g.Key.MonthNo,
                    MonthName = g.Key.MonthName
                })
                .ToList();

                return Json(rawData, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetSubCategoryWiseRPT(string monthn)
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                int id = int.Parse(Session["userid"].ToString());
                int monthnumber = 8;
                if (string.IsNullOrEmpty(monthn))
                {
                    monthnumber = 8;
                }
                else
                {
                    monthnumber = int.Parse(monthn);
                    
                }






                var data = db.expenses_tbl
                .Include(e => e.category_tbl)
                .Include(e => e.sub_category_tbl)
                .Include(e => e.sub_sub_category_tbl)
                .Where(e => e.sdate.Month == monthnumber && e.fkUserId == id) // Filter data by month
                .AsEnumerable() // Bring data into memory
                .GroupBy(e => new
                {
                    e.category_tbl.cat_name,
                    e.category_tbl.cat_id,
                    e.sub_category_tbl.subcat_name,
                    e.sub_sub_category_tbl.sub_sub_catName,
                    e.sub_category_tbl.subcat_id,
                    e.sub_sub_category_tbl.sub_sub_catId,
                    MonthNo = e.sdate.Month,
                    MonthName = e.sdate.ToString("MMM")
                })
                .Select(g => new
                {
                    cat_id = g.Key.cat_id,
                    cat_name = g.Key.cat_name,
                    subcat_name = g.Key.subcat_name,
                    sub_sub_catName = g.Key.sub_sub_catName,
                    total_price = g.Sum(e => Convert.ToDecimal(e.total_price)), // Convert if necessary,
                    subcat_id = g.Key.subcat_id,
                    sub_sub_catid = g.Key.sub_sub_catId,
                    MonthNo = g.Key.MonthNo,
                    MonthName = g.Key.MonthName
                })
                .OrderBy(result => result.cat_name)
                .ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadFile(string format)
        {
       
            if (format == "csv")
            {
                return DownloadCsv();
            }
            else if (format == "excel")
            {
                return DownloadExcel();
            }
            else
            {
                return new HttpStatusCodeResult(400, "Invalid format");
            }
        }

        public ActionResult DownloadCsv()
        {
            using (ExpensesEntities db = new ExpensesEntities())
            {
                List<Expenses> exp = new List<Expenses>();

                int id = int.Parse(Session["userid"].ToString());
                var data = db.expenses_tbl
                    .Include(i => i.category_tbl)
                    .Include(i => i.sub_category_tbl)
                    .Include(i => i.sub_sub_category_tbl)
                    .Include(i => i.user_tbl)
                    .Where(i => i.fkUserId == id && i.sdate.Month == 8).ToList();

                var assignCardToBanks = db.AssignCardToBank_tbl.Include(acb => acb.PaymentMode_tbl).ToList();

                // Associate related data with main data
                foreach (var expense in data)
                {
                    expense.AssignCardToBank_tbl = assignCardToBanks.FirstOrDefault(acb => acb.ACID == expense.FKACID);
                }

                // var data = GetTableData(); // Replace with your data retrieval logic
                var csv = new StringBuilder();

                // Add headers
                csv.AppendLine("SRNO.,Category,Sub Category,Sub-Sub Category,Item_Name,Price,PaymentMode");
                int srno = 0;
                // Add rows
                foreach (var item in data)
                {
                    srno++;
                    csv.AppendLine($"{srno},{item.category_tbl.cat_name},{item.sub_category_tbl.subcat_name},{item.sub_sub_category_tbl.sub_sub_catName},{item.item_name},{item.total_price},{item.AssignCardToBank_tbl.PaymentMode_tbl.PaymentMode}");
                }
                var bytes = Encoding.UTF8.GetBytes(csv.ToString());
                var stream = new MemoryStream(bytes);
                return File(stream, "text/csv", "data.csv");
            }
        }
        public ActionResult DownloadExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExpensesEntities db = new ExpensesEntities())
            {
                List<Expenses> exp = new List<Expenses>();

                int id = int.Parse(Session["userid"].ToString());
                var data = db.expenses_tbl
                    .Include(i => i.category_tbl)
                    .Include(i => i.sub_category_tbl)
                    .Include(i => i.sub_sub_category_tbl)
                    .Include(i => i.user_tbl)
                    .Where(i => i.fkUserId == id && i.sdate.Month == 8).ToList();

                var assignCardToBanks = db.AssignCardToBank_tbl.Include(acb => acb.PaymentMode_tbl).ToList();

                // Associate related data with main data
                foreach (var expense in data)
                {
                    expense.AssignCardToBank_tbl = assignCardToBanks.FirstOrDefault(acb => acb.ACID == expense.FKACID);
                }
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].Value = "Category";
                    worksheet.Cells["B1"].Value = "Price";

                    int row = 2;
                    foreach (var item in data)
                    {
                        worksheet.Cells[$"A{row}"].Value = item.category_tbl.cat_name;
                        worksheet.Cells[$"B{row}"].Value = item.total_price;
                        row++;
                    }

                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;

                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
                }
            }
        }

        
    }
}