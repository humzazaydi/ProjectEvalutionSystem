using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectEvalutionSystem.Models;

namespace ProjectEvalutionSystem.Controllers
{
    public class StudentsController : Controller
    {
        private ProjectEvalutionSystemEntities db = new ProjectEvalutionSystemEntities();

        // GET: Students
        public ActionResult Index()
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            return View(db.Students.Include(x=> x.Teacher).ToList());
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FullName,EmailAddress,Username,Password,TeacherID,UserRole,IsActive,CreationTimStamp,ModificationTimeStamp")] Student student)
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FullName,EmailAddress,Username,Password,UserRole,IsActive,CreationTimStamp,ModificationTimeStamp")] Student student)
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
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
