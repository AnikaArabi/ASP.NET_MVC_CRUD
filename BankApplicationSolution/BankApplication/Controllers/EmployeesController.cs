using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BankApplication.Models;
using BankApplication.EmployeeModel;
using System.IO;
using System.Threading;

namespace BankApplication.Controllers
{
    public class EmployeesController : Controller
    {
        BankDbContext db = new BankDbContext();
        // GET: Employees
        public ActionResult Index()
        {
            return View(db.Employees.Include(x => x.Bank).ToList());
        }
        public ActionResult Create()
        {
            ViewBag.Banks = db.Banks.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(EmployeeInputModel e)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    EmployeeName = e.EmployeeName,
                    Email = e.Email,
                    StratDate = e.StratDate,
                    EndDate = e.EndDate,
                    IsWorking = e.IsWorking,
                    BankId = e.BankId
                };
                string ext = Path.GetExtension(e.Picture.FileName);
                string p = Guid.NewGuid() + ext;
                e.Picture.SaveAs(Server.MapPath("~/Pictures/") + p);
                employee.Picture = p;
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Banks = db.Banks.ToList();
            return View(e);
        }
        public ActionResult Edit(int id)
        {
            var e = db.Employees.First(x => x.EmployeeId == id);
            ViewBag.Banks = db.Banks.ToList();
            ViewBag.CurrentPic = e.Picture;
            return View(new EmployeeEditModel { EmployeeId = e.EmployeeId, EmployeeName = e.EmployeeName, Email = e.Email, StratDate = e.StratDate, EndDate = e.EndDate, IsWorking = e.IsWorking, BankId = e.BankId });
        }
        [HttpPost]
        public ActionResult Edit(EmployeeEditModel e)
        {
            Thread.Sleep(2000);
            var employee = db.Employees.First(x => x.EmployeeId == e.EmployeeId);
            if (ModelState.IsValid)
            {

                employee.EmployeeName = e.EmployeeName;
                employee.Email = e.Email;
                employee.StratDate = e.StratDate;
                employee.EndDate = e.EndDate;
                employee.IsWorking = e.IsWorking;

                if (e.Picture != null)
                {
                    string ext = Path.GetExtension(e.Picture.FileName);
                    string p = Guid.NewGuid() + ext;
                    e.Picture.SaveAs(Server.MapPath("~/Pictures/") + p);
                    employee.Picture = p;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CurrentPic = employee.Picture;
            ViewBag.Banks = db.Banks.ToList();
            return View(e);
        }
        public ActionResult Delete(int id)
        {
            return View(db.Employees.Include(x => x.Bank).First(x => x.EmployeeId == id));
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            Employee t = new Employee { EmployeeId = id };
            db.Entry(t).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}