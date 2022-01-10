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
    public class EvalutionIndexesController : Controller
    {
        private ProjectEvalutionSystemEntities db = new ProjectEvalutionSystemEntities();

        // GET: EvalutionIndexes
        public async Task<ActionResult> Index()
        {
            var evalutionIndexes = db.EvalutionIndexes.Include(e => e.Assignment);
            return View(await evalutionIndexes.ToListAsync());
        }
        // GET: EvalutionIndexes/Create
        public ActionResult Create()
        {
            ViewBag.Assignment = new SelectList(db.Assignments, "ID", "Name");
            return View();
        }

        // POST: EvalutionIndexes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,SubmissionDate,Remarks,Comments,Assignment")] EvalutionIndex evalutionIndex)
        {
            evalutionIndex.EvalutionDate = DateTime.Now;
            db.EvalutionIndexes.Add(evalutionIndex);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: EvalutionIndexes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvalutionIndex evalutionIndex = await db.EvalutionIndexes.FindAsync(id);
            if (evalutionIndex == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignmentID = new SelectList(db.Assignments, "ID", "Name", evalutionIndex.AssignmentID);
            return View(evalutionIndex);
        }

        // POST: EvalutionIndexes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,SubmissionDate,Remarks,Comments,AssignmentID")] EvalutionIndex evalutionIndex)
        {
            var GetevalutionIndex = await db.EvalutionIndexes.FindAsync(evalutionIndex.ID);
            if (GetevalutionIndex !=null)
            {
                GetevalutionIndex.AssignmentID = evalutionIndex.AssignmentID;
                GetevalutionIndex.Remarks = evalutionIndex.Remarks;
                GetevalutionIndex.Comments = evalutionIndex.Comments;
                GetevalutionIndex.SubmissionDate = evalutionIndex.SubmissionDate;
                db.Entry(GetevalutionIndex).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Assignment = new SelectList(db.Assignments, "ID", "Name", evalutionIndex.AssignmentID);
            return View();

        }

        // GET: EvalutionIndexes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvalutionIndex evalutionIndex = await db.EvalutionIndexes.FindAsync(id);
            if (evalutionIndex == null)
            {
                return HttpNotFound();
            }
            return View(evalutionIndex);
        }

        // POST: EvalutionIndexes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EvalutionIndex evalutionIndex = await db.EvalutionIndexes.FindAsync(id);
            db.EvalutionIndexes.Remove(evalutionIndex);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("StartEvaluation")]
        public async Task<ActionResult> StartEvaluation(int id)
        {
            return View();
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
