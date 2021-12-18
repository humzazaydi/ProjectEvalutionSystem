using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectEvalutionSystem.Models;

namespace ProjectEvalutionSystem.Controllers
{
    public class StudentTeachersController : Controller
    {
        private ProjectEvalutionSystemEntities db = new ProjectEvalutionSystemEntities();

        // GET: StudentTeachers
        public async Task<ActionResult> Index()
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            var studentTeachers = db.StudentTeachers.Include(s => s.Student).Include(s => s.Teacher);
            return View(await studentTeachers.ToListAsync());
        }

        // GET: StudentTeachers/Details/5
        public async Task<ActionResult> Details(int? id)
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
            StudentTeacher studentTeacher = await db.StudentTeachers.FindAsync(id);
            if (studentTeacher == null)
            {
                return HttpNotFound();
            }
            return View(studentTeacher);
        }

        // GET: StudentTeachers/Create
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

        // POST: StudentTeachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,StudentID,TeacherID")] StudentTeacher studentTeacher)
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            if (ModelState.IsValid)
            {
                db.StudentTeachers.Add(studentTeacher);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.StudentID = new SelectList(db.Students, "ID", "FullName", studentTeacher.StudentID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "ID", "FullName", studentTeacher.TeacherID);
            return View(studentTeacher);
        }

        // GET: StudentTeachers/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            StudentTeacher studentTeacher = await db.StudentTeachers.FindAsync(id);
            if (studentTeacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentID = new SelectList(db.Students, "ID", "FullName", studentTeacher.StudentID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "ID", "FullName", studentTeacher.TeacherID);
            return View(studentTeacher);
        }

        // POST: StudentTeachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,StudentID,TeacherID")] StudentTeacher studentTeacher)
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            if (ModelState.IsValid)
            {
                db.Entry(studentTeacher).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.StudentID = new SelectList(db.Students, "ID", "FullName", studentTeacher.StudentID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "ID", "FullName", studentTeacher.TeacherID);
            return View(studentTeacher);
        }

        // GET: StudentTeachers/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
            StudentTeacher studentTeacher = await db.StudentTeachers.FindAsync(id);
            if (studentTeacher == null)
            {
                return HttpNotFound();
            }
            return View(studentTeacher);
        }

        // POST: StudentTeachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            StudentTeacher studentTeacher = await db.StudentTeachers.FindAsync(id);
            db.StudentTeachers.Remove(studentTeacher);
            await db.SaveChangesAsync();
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
