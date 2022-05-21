using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
