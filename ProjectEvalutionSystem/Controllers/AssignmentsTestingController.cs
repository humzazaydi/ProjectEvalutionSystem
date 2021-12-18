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
    public class AssignmentsTestingController : Controller
    {
        private ProjectEvalutionSystemEntities db = new ProjectEvalutionSystemEntities();

        // GET: AssignmentsTesting
        public async Task<ActionResult> Index()
        {
            var assignments = db.Assignments.Include(a => a.Student).Include(a => a.Teacher);
            return View(await assignments.ToListAsync());
        }

        // GET: AssignmentsTesting/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = await db.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // GET: AssignmentsTesting/Create
        public ActionResult Create()
        {
            ViewBag.StudentID = new SelectList(db.Students, "ID", "FullName");
            ViewBag.TeacherID = new SelectList(db.Teachers, "ID", "FullName");
            return View();
        }

        // POST: AssignmentsTesting/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,StudentID,TeacherID,Name,Description,Path,IsDeleted,CreationTimeStamp")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                db.Assignments.Add(assignment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.StudentID = new SelectList(db.Students, "ID", "FullName", assignment.StudentID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "ID", "FullName", assignment.TeacherID);
            return View(assignment);
        }

        // GET: AssignmentsTesting/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = await db.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentID = new SelectList(db.Students, "ID", "FullName", assignment.StudentID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "ID", "FullName", assignment.TeacherID);
            return View(assignment);
        }

        // POST: AssignmentsTesting/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,StudentID,TeacherID,Name,Description,Path,IsDeleted,CreationTimeStamp")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.StudentID = new SelectList(db.Students, "ID", "FullName", assignment.StudentID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "ID", "FullName", assignment.TeacherID);
            return View(assignment);
        }

        // GET: AssignmentsTesting/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = await db.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: AssignmentsTesting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Assignment assignment = await db.Assignments.FindAsync(id);
            db.Assignments.Remove(assignment);
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
