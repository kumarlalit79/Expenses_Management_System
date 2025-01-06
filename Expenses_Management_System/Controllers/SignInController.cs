using Expenses_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Expenses_Management_System.Controllers
{
    public class SignInController : Controller
    {
        // GET: SignIn
        public ActionResult Index()
        {
            ViewBag.otp = false;
            return View();
        }

        [HttpPost]
        public ActionResult Index(user_tbl u)
        {

            using (ExpensesEntities db = new ExpensesEntities())
            {
                var MobNum = db.user_tbl.Where(model => model.mobile_num == u.mobile_num).FirstOrDefault();

                if (string.IsNullOrEmpty(u.OTP))
                {
                    if (ModelState.IsValid == true)
                    {
                        if (MobNum == null)
                        {
                            Session["Mobile"] = u.mobile_num;

                            return RedirectToAction("Create", "User");
                        }
                        else
                        {
                            string a = u.@type;
                            Session["userid"] = MobNum.user_id;
                            Session["Type"] = MobNum.type;
                            Session["Mobile"] = MobNum.mobile_num;
                            Session["Name"] = MobNum.user_name;



                            string email = MobNum.email_id;
                            Random random = new Random();

                            int otp = random.Next(1000, 9999); // Generates a 4-digit number between 1000 and 9999

                            MobNum.OTP = otp.ToString();
                            MobNum.OTP_created_on = System.DateTime.Now;

                            // Set the OTP expiration time to 10 minutes from now
                            MobNum.OTP_ends_on = System.DateTime.Now.AddMinutes(10);
                            db.Entry(MobNum).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();




                            using (MailMessage mm = new MailMessage("infotechnologyems@gmail.com", email))//it takes 2 parameter, jiske mail bhejni hai or jo mail bhej ra hai.
                            {
                                mm.Subject = otp + " is your OTP for EMS";


                                mm.Body = "Dear " + Session["Name"] + ",\n\n" +
                                              "You are almost done. Use the OTP code below to access your account:" +
                                              otp.ToString() + "\n\n" +
                                              "OTP is valid for the next 10 minutes.\n\n" +
                                              "Warm Regards,\n" +
                                              "Team EMS";


                                mm.IsBodyHtml = false; // sirf ham string bhej sakte hai html nahi 

                                //smtp server
                                using (SmtpClient smtp = new SmtpClient())
                                {
                                    smtp.Host = "smtp.gmail.com";
                                    smtp.EnableSsl = true;

                                    // checking credentials
                                    NetworkCredential cred = new NetworkCredential("infotechnologyems@gmail.com", "rlvp lztu vpqm qobu");
                                    smtp.UseDefaultCredentials = false;
                                    smtp.Credentials = cred;

                                    smtp.Port = 587;

                                    smtp.Send(mm);

                                    ViewBag.Message = "Email sent";

                                    ViewBag.otp = true;
                                }
                            }

                            return View();

                            
                        }
                    }
                }
                else
                {
                    if (MobNum.OTP == u.OTP )
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else if (u.OTP == null)
                    {
                        ViewBag.otperror = "OTP should not be empty";
                        ViewBag.otp = true;
                    }
                    else if (u.OTP_ends_on < DateTime.Now)
                    {
                        ViewBag.otperror = "OTP is Expired, generate new OTP";
                        ViewBag.otp = true;
                    }
                    else if(u.OTP.Length > 4)
                    {
                        ViewBag.otperror = "OTP length should be of 4-digit.";
                        ViewBag.otp = true;
                    }
                    else 
                    {
                        ViewBag.otperror = "Invalid OTP";
                        ViewBag.otp = true; // Keep OTP input field visible
                    }
                    
                }
                if (ViewBag.otp == null)
                {
                    ViewBag.otp = false;
                }
                return View();
            }
        }
    }
}