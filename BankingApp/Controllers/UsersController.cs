using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankingApp.Models;

namespace BankingApp.Controllers
{
    public class UsersController : Controller
    {
        private BankDbContext db = new BankDbContext();

        public ActionResult Index()
        {
            User user = Session["User"] as User;

            if (Session["User"] != null)
            {
                return View(user);
            }
            return RedirectToAction("Login");

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginViewModel UserVM)
        {
            //if (Session.IsNewSession)
            //{
                if (ModelState.IsValid)
                {
                    bool isUser = db.Users.Any(u => u.Id == UserVM.Id && u.Password == UserVM.Password);

                    if (isUser)
                    {
                        User userFromStore = db.Users.Find(UserVM.Id);
                        Session["User"] = userFromStore;
                        return RedirectToAction("Index", "Users");
                    }
                    ModelState.AddModelError("", "Wrong Credentials.");
                }
            //}
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User user)
        {
            if (Session["User"] == null)
            {
                db.Users.Add(user);
                Transaction transaction = new Transaction(user, user.Balance, "D");
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return RedirectToAction("Index",user);
        }
 
        public PartialViewResult Passbook()
        {
            User user = Session["User"] as User;
            List<Transaction> model = db.Transactions.Where(u=> u.Name == user.Name).ToList();
            return PartialView("_Passbook", model);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public void CsvDownload()
        {
            User user = Session["User"] as User;

            StringWriter sw = new StringWriter();
            sw.WriteLine("\"Name\",\"Date\",\"Amount\",\"Transaction Type\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attchment;filename=${user} Passbook.csv");
            Response.ContentType=("text/csv");
            List<Transaction> model = db.Transactions.Where(u => u.Name == user.Name).ToList();
            foreach (var item in model)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", item.Name, item.Created, item.Amount, item.TransactionType));

            }
            Response.Write(sw.ToString());
            Response.End();

        }
    }
}
