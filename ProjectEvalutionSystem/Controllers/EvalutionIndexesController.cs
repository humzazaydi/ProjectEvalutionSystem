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
using EvoPdf.PdfToText;
using ProjectEvalutionSystem.Helper;
using ProjectEvalutionSystem.Models;
using ProjectEvalutionSystem.Models.Auth;

namespace ProjectEvalutionSystem.Controllers
{
    public class EvalutionIndexesController : Controller
    {
        private ProjectEvalutionSystemEntities db = new ProjectEvalutionSystemEntities();

        // GET: EvalutionIndexes
        public async Task<ActionResult> Index()
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }

            IQueryable<EvalutionIndex> evalutionIndexes = null;
            var sessionID = (int)Session["CurrentLoginId"];
            switch ((UserRole)Session["UserRole"])
            {
                case UserRole.Teacher:
                    evalutionIndexes = db.EvalutionIndexes
                        .Include(a => a.Assignment)
                        .Include(x => x.Assignment.Cours)
                        .Where(x => x.Assignment.Cours.TeacherID == sessionID);
                    break;

                case UserRole.SuperAdmin:
                    evalutionIndexes = db.EvalutionIndexes.Include(a => a.Assignment);
                    break;
                case UserRole.Student:
                    evalutionIndexes = db.EvalutionIndexes
                        .Include(a => a.Assignment)
                        .Include(x => x.Assignment.Cours)
                        .Where(x => x.Assignment.StudentID == sessionID);
                    break;
            }

            return View(await evalutionIndexes.ToListAsync());
        }
        // GET: EvalutionIndexes/Create
        public ActionResult Create()
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }

            var sessionID = (int)Session["CurrentLoginId"];
            switch ((UserRole)Session["UserRole"])
            {
                case UserRole.Teacher:
                    ViewBag.AssignmentID = new SelectList(db.Assignments
                        .Include(x => x.Cours)
                        .Where(x => x.Cours.TeacherID == sessionID).ToList(), "ID", "Name");
                    break;

                case UserRole.SuperAdmin:
                    ViewBag.AssignmentID = new SelectList(db.Assignments.ToList(), "ID", "Name");
                    break;
            }

            return View();
        }

        // POST: EvalutionIndexes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,SubmissionDate,Remarks,Comments,AssignmentID")] EvalutionIndex evalutionIndex)
        {
            evalutionIndex.EvalutionDate = DateTime.Now;
            db.EvalutionIndexes.Add(evalutionIndex);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: EvalutionIndexes/Edit/5
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
            EvalutionIndex evalutionIndex = await db.EvalutionIndexes.FindAsync(id);
            if (evalutionIndex == null)
            {
                return HttpNotFound();
            }
            var sessionID = (int)Session["CurrentLoginId"];
            switch ((UserRole)Session["UserRole"])
            {
                case UserRole.Teacher:
                    ViewBag.AssignmentID = new SelectList(db.Assignments
                        .Include(x => x.Cours)
                        .Where(x => x.Cours.TeacherID == sessionID).ToList(), "ID", "Name");
                    break;

                case UserRole.SuperAdmin:
                    ViewBag.AssignmentID = new SelectList(db.Assignments.ToList(), "ID", "Name");
                    break;
            }
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
            if (GetevalutionIndex != null)
            {
                GetevalutionIndex.AssignmentID = evalutionIndex.AssignmentID;
                GetevalutionIndex.Remarks = evalutionIndex.Remarks;
                GetevalutionIndex.Comments = evalutionIndex.Comments;
                GetevalutionIndex.SubmissionDate = evalutionIndex.SubmissionDate;
                db.Entry(GetevalutionIndex).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var sessionID = (int)Session["CurrentLoginId"];
            switch ((UserRole)Session["UserRole"])
            {
                case UserRole.Teacher:
                    ViewBag.Assignment = new SelectList(db.Assignments
                        .Include(x => x.Cours)
                        .Where(x => x.Cours.TeacherID == sessionID).ToList(), "ID", "Name");
                    break;

                case UserRole.SuperAdmin:
                    ViewBag.Assignment = new SelectList(db.Assignments.ToList(), "ID", "Name");
                    break;
            }
            return View();

        }

        // GET: EvalutionIndexes/Delete/5
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

        [HttpGet]
        [Route(template: "PlagiarismResults")]
        public async Task<ActionResult> StartEvaluation(int id)
        {
            EvalutionIndex evalutionIndex = await db.EvalutionIndexes.Include(x => x.Assignment).FirstOrDefaultAsync(x => x.ID == id);

            Assignment assignment = evalutionIndex.Assignment;

            string fileText = string.Empty;

            if (assignment.Path.Contains(".pdf"))
            {
                fileText = GetDocumentText(assignment.Path);
            }
            else if (assignment.Path.Contains(".txt"))
            {
                fileText = GetFileText(assignment.Path);
            }
            else
            {
                fileText = GetFileText(assignment.Path);
            }

            var response = CheckPlagiarism.StartProcess(fileText, id);

            ViewBag.PlagCount = response.PlagCount;
            ViewBag.UniqueCount = response.UniqueCount;
            ViewBag.Websites = response.matchingUrls;
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

        private string GetFileText(string path)
        {
            return System.IO.File.ReadAllText(Path.Combine(Server.MapPath("~/App_Data/"), path));
        }
        private string GetDocumentText(string path)
        {
            PdfToTextConverter pdfToTextConverter = new PdfToTextConverter();

            pdfToTextConverter.LicenseKey = "ujQlNSAgNSU1IzslNSYkOyQnOywsLCw1JQ==";

            pdfToTextConverter.Layout = TextLayout.OriginalLayout;
            pdfToTextConverter.MarkPageBreaks = true;

            try
            {
                string extractedText = pdfToTextConverter.ConvertToText(Path.Combine(Server.MapPath("~/App_Data/"), path));
                return extractedText;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
