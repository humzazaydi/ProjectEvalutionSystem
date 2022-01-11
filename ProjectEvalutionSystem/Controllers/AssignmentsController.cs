using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectEvalutionSystem.Models;
using ProjectEvalutionSystem.Models.Auth;

namespace ProjectEvalutionSystem.Controllers
{
    public class AssignmentsController : Controller
    {
        private ProjectEvalutionSystemEntities db = new ProjectEvalutionSystemEntities();

        // GET: Assignments
        public async Task<ActionResult> Index()
        {
            IQueryable<Assignment> assignments = null;
            var sessionID = (int)Session["CurrentLoginId"];
            switch ((UserRole)Session["UserRole"])
            {
                case UserRole.Teacher:
                    assignments = db.Assignments.Include(a => a.Cours).Where(x=> x.Cours.TeacherID == sessionID);
                    break;

                case UserRole.SuperAdmin:
                    assignments = db.Assignments.Include(a => a.Cours);
                    break;
            }
            
            return View(await assignments.ToListAsync());
        }

        // GET: Assignments/Create
        public ActionResult Create()
        {
            var sessionID = (int)Session["CurrentLoginId"];
            switch ((UserRole)Session["UserRole"])
            {
                case UserRole.Teacher:
                    ViewBag.CourseID = new SelectList(db.Courses.Where(x => x.TeacherID == sessionID).ToList(), "ID", "Name");
                    break;

                case UserRole.SuperAdmin:
                    ViewBag.CourseID = new SelectList(db.Courses.ToList(), "ID", "Name");
                    break;
            }
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,Description,CourseID")] Assignment assignment, HttpPostedFileBase assignmentFile)
        {
            if (ModelState.IsValid)
            {
                if (assignmentFile != null)
                {
                    var filename = Path.Combine(Server.MapPath("~/App_Data/"), assignmentFile.FileName);
                    assignmentFile.SaveAs(filename);
                    assignment.Path = assignmentFile.FileName;
                }

                assignment.IsDeleted = false;
                assignment.CreationTimeStamp = DateTime.Now;
                db.Assignments.Add(assignment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var sessionID = (int)Session["CurrentLoginId"];
            switch ((UserRole)Session["UserRole"])
            {
                case UserRole.Teacher:
                    ViewBag.CourseID = new SelectList(db.Courses.Where(x => x.TeacherID == sessionID).ToList(), "ID", "Name");
                    break;

                case UserRole.SuperAdmin:
                    ViewBag.CourseID = new SelectList(db.Courses.ToList(), "ID", "Name");
                    break;
            }
            return View(assignment);
        }

        // GET: Assignments/Edit/5
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
            var sessionID = (int)Session["CurrentLoginId"];
            switch ((UserRole)Session["UserRole"])
            {
                case UserRole.Teacher:
                    ViewBag.CourseID = new SelectList(db.Courses.Where(x => x.TeacherID == sessionID).ToList(), "ID", "Name");
                    break;

                case UserRole.SuperAdmin:
                    ViewBag.CourseID = new SelectList(db.Courses.ToList(), "ID", "Name");
                    break;
            }
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,Description,Path,CourseID")] Assignment assignment, HttpPostedFileBase assignmentFile)
        {
            if (ModelState.IsValid)
            {
                if (assignmentFile != null)
                {
                    DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/App_Data/"));
                    var files = dir.GetFiles();
                    foreach (var item in files)
                    {
                        if (item.Name == assignment.Path)
                        {
                            item.Delete();

                            var filename = Path.Combine(Server.MapPath("~/App_Data/"), assignmentFile.FileName);
                            assignmentFile.SaveAs(filename);
                        }
                    }

                    assignment.Path = assignmentFile.FileName;
                }

                assignment.CreationTimeStamp = DateTime.Now;
                db.Entry(assignment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var sessionID = (int)Session["CurrentLoginId"];
            switch ((UserRole)Session["UserRole"])
            {
                case UserRole.Teacher:
                    ViewBag.CourseID = new SelectList(db.Courses.Where(x => x.TeacherID == sessionID).ToList(), "ID", "Name");
                    break;

                case UserRole.SuperAdmin:
                    ViewBag.CourseID = new SelectList(db.Courses.ToList(), "ID", "Name");
                    break;
            }
            return View(assignment);
        }

        // GET: Assignments/Delete/5
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

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Assignment assignment = await db.Assignments.FindAsync(id);
            
            db.Assignments.Remove(assignment);
            await db.SaveChangesAsync();

            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/App_Data/"));
            var files = dir.GetFiles();
            foreach (var item in files)
            {
                if (item.Name == assignment.Path)
                {
                    item.Delete();
                }
            }
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
