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
using BankingApp.Services;

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
            if (ModelState.IsValid)
            {
                string userPass = UserVM.Password.Cipher();
                bool isUser = db.Users.Any(u => u.Id == UserVM.Id && u.Password == userPass);

                if (isUser)
                {
                    User userFromStore = db.Users.Find(UserVM.Id);
                    Session["User"] = userFromStore;
                    return RedirectToAction("Index", "Users");
                }
                ModelState.AddModelError("", "Wrong Credentials.");
            }
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
            if (ModelState.IsValid)
            {
                if (Session["User"] == null)
                {
                    User userEncrypted = new User() { Balance = user.Balance, Name = user.Name, Password = user.Password.Cipher() };
                    db.Users.Add(userEncrypted);
                    Transaction transaction = new Transaction(user, user.Balance, "D");
                    db.Transactions.Add(transaction);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
            }
            ModelState.AddModelError("", "Enter details.");
            return RedirectToAction("Index", user);
        }

        public PartialViewResult Passbook()
        {
            User user = Session["User"] as User;
            List<Transaction> model = db.Transactions.Where(u => u.Name == user.Name).ToList();
            return PartialView("_Passbook", model);
        }

        public PartialViewResult Transact()
        {
            return PartialView("_Transact");
        }

        [HttpPost]
        public ActionResult Transact(TransactionViewModel transactionVM)
        {
            if (ModelState.IsValid)
            {
                User user = Session["User"] as User;
                User userToModify = db.Users.Find(user.Id);
                if (transactionVM.TransactionType == "D")
                {
                    userToModify.Balance += transactionVM.Amount;
                }
                else
                {
                    if (transactionVM.Amount < user.Balance - 500)
                    {
                        userToModify.Balance -= transactionVM.Amount;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Transaction Failed! Minimum remaining balance should be 500.");
                        return RedirectToAction("Index", "Users");
                    }
                }
                Transaction transaction = new Transaction(user, transactionVM.Amount, transactionVM.TransactionType);
                Session["User"] = userToModify;
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index", "Users");
            }
            return View();
        }

        public void CsvDownload()
        {
            User user = Session["User"] as User;

            StringWriter sw = new StringWriter();
            sw.WriteLine("\"Name\",\"Date\",\"Amount\",\"Transaction Type\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attchment;filename=${user} Passbook.csv");
            Response.ContentType = ("text/csv");
            List<Transaction> model = db.Transactions.Where(u => u.Name == user.Name).ToList();
            foreach (var item in model)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", item.Name, item.Created, item.Amount, item.TransactionType));

            }
            Response.Write(sw.ToString());
            Response.End();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
