using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectEvalutionSystem.Models;

namespace ProjectEvalutionSystem.Controllers
{ 
    public class AssignmentsController : Controller
    {
        private ProjectEvalutionSystemEntities db = new ProjectEvalutionSystemEntities();

        // GET: Assignments
        public ActionResult Index()
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }

            return View(db.Assignments.Include(a => a.Student).Include(a => a.Teacher).ToList());
        }

        // GET: Assignments/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception","ErrorHandling");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // GET: Assignments/Create
        public ActionResult Create()
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }

            ViewBag.StudentID = new SelectList(db.Students, "ID", "FullName");
            ViewBag.TeacherID = new SelectList(db.Teachers, "ID", "FullName");
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StudentID,TeacherID,Name,Description,Path")] Assignment assignment, HttpPostedFileBase file)
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/App_Data"), _FileName.Trim());
                    file.SaveAs(_path);
                    assignment.Path = _FileName;
                    assignment.IsDeleted = false;
                }
                assignment.CreationTimeStamp = DateTime.Now;
                db.Assignments.Add(assignment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.StudentID = new SelectList(db.Students, "ID", "FullName", assignment.StudentID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "ID", "FullName", assignment.TeacherID);
            return View(assignment);
        }

        // GET: Assignments/Edit/5
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
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }

            ViewBag.StudentID = new SelectList(db.Students, "ID", "FullName", assignment.StudentID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "ID", "FullName", assignment.TeacherID);
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StudentID,TeacherID,Name,Description,Path")] Assignment assignment, HttpPostedFileBase file)
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }

            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0)
                {

                    var _assignment = db.Assignments.Where(x => x.ID == assignment.ID).FirstOrDefault();
                    if (_assignment != null)
                    {
                        string _pathToBeCheck = string.Empty;
                        if (!string.IsNullOrEmpty(_assignment.Path))
                        {
                            _pathToBeCheck = Path.Combine(Server.MapPath("~/App_Data"), _assignment.Student.FullName.Trim() + "_" + _assignment.Name.Trim());
                            //Removing Previous Assignments
                            if (System.IO.File.Exists(_pathToBeCheck))
                            {
                                // If file found, delete it    
                                System.IO.File.Delete(_pathToBeCheck);
                            }
                        }
                    }
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/App_Data"), _FileName);
                    file.SaveAs(_path);
                    assignment.Path = _FileName;
                    assignment.IsDeleted = false;
                    assignment.CreationTimeStamp = DateTime.Now;
                }
                db.Entry(assignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentID = new SelectList(db.Students, "ID", "FullName", assignment.StudentID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "ID", "FullName", assignment.TeacherID);
            return View(assignment);
        }

        // GET: Assignments/Delete/5
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
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }

            Assignment assignment = db.Assignments.Find(id);
            db.Assignments.Remove(assignment);
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
