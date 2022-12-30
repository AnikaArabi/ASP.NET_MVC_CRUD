using BankApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading;

namespace BankApplication.Controllers
{
    public class BanksController : Controller
    {
        BankDbContext db = new BankDbContext();
        // GET: Banks
        public ActionResult Index()
        {
            return View(db.Banks.Include(b => b.Employees).ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        public PartialViewResult CreateBank()
        {
            return PartialView("_CreateBank");
        }
        [HttpPost]
        public PartialViewResult CreateBank(Bank b)
        {
            if (ModelState.IsValid)
            {
                db.Banks.Add(b);
                db.SaveChanges();
                return PartialView("_Success");
            }
            return PartialView("_Fail");
        }
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public PartialViewResult EditBank(int id)
        {
            var b = db.Banks.First(x => x.BankId == id);
            return PartialView("_EditBank", b);
        }
        [HttpPost]
        public PartialViewResult EditBatch(Bank b)
        {
            Thread.Sleep(4000);
            if (ModelState.IsValid)
            {
                db.Entry(b).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_Success");
            }
            return PartialView("_Fail");
        }
        public ActionResult Delete(int id)
        {
            return View(db.Banks.First(x => x.BankId == id));
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DoDelete(int BankId)
        {
            var b = new Bank { BankId = BankId };
            db.Entry(b).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}