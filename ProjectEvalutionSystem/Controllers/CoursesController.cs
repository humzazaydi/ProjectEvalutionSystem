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
using ProjectEvalutionSystem.Models.Auth;

namespace ProjectEvalutionSystem.Controllers
{
    public class CoursesController : Controller
    {
        private ProjectEvalutionSystemEntities db = new ProjectEvalutionSystemEntities();

        // GET: Courses
        public async Task<ActionResult> Index()
        {
            IQueryable<Cours> courses = null;
            var sessionID = (int)Session["CurrentLoginId"];
            switch ((UserRole)Session["UserRole"])
            {
                case UserRole.Teacher:
                    courses = db.Courses.Include(a => a.Teacher).Where(x => x.TeacherID == sessionID);
                    break;

                case UserRole.SuperAdmin:
                    courses = db.Courses.Include(a => a.Teacher);
                    break;
            }

            return View(await courses.ToListAsync());
        }
        

        // GET: Courses/Create
        public ActionResult Create()
        {
            var sessionID = (int)Session["CurrentLoginId"];
            switch ((UserRole)Session["UserRole"])
            {
                case UserRole.Teacher:
                    ViewBag.TeacherID = new SelectList(db.Teachers.Where(x => x.ID == sessionID).ToList(), "ID", "FullName");
                    break;

                case UserRole.SuperAdmin:
                    ViewBag.TeacherID = new SelectList(db.Teachers.ToList(), "ID", "FullName");
                    break;
            }
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,Description,TeacherID")] Cours cours)
        {
            if (ModelState.IsValid)
            {
                cours.CreationTimeStamp = DateTime.Now;
                cours.CreatorUserId = (int) Session["CurrentLoginId"];
                db.Courses.Add(cours);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var sessionID = (int)Session["CurrentLoginId"];
            switch ((UserRole)Session["UserRole"])
            {
                case UserRole.Teacher:
                    ViewBag.TeacherID = new SelectList(db.Teachers.Where(x => x.ID == sessionID).ToList(), "ID", "FullName");
                    break;

                case UserRole.SuperAdmin:
                    ViewBag.TeacherID = new SelectList(db.Teachers.ToList(), "ID", "FullName");
                    break;
            }
            return View(cours);
        }

        // GET: Courses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = await db.Courses.FindAsync(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeacherID = new SelectList(db.Teachers, "ID", "FullName", cours.TeacherID);
            return View(cours);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,Description,TeacherID")] Cours cours)
        {
            if (ModelState.IsValid)
            {
                cours.ModificationTimeStamp = DateTime.Now;
                cours.ModificationUserId = (int)Session["CurrentLoginId"];
                db.Entry(cours).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TeacherID = new SelectList(db.Teachers, "ID", "FullName", cours.TeacherID);
            return View(cours);
        }

        // GET: Courses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = await db.Courses.FindAsync(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            return View(cours);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Cours cours = await db.Courses.FindAsync(id);
            db.Courses.Remove(cours);
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
