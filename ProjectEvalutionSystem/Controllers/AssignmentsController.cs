using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProjectEvalutionSystem.Models;

namespace ProjectEvalutionSystem.Controllers
{
    public class AssignmentsController : Controller
    {
        private ProjectEvalutionSystemEntities db = new ProjectEvalutionSystemEntities();
        // GET: Assignments
       
        [HttpGet]
        public async Task<JsonResult> GetAsync()
        {
            try
            {
                var assignments = await db.Assignments.Where(x => x.IsDeleted == false).ToListAsync();
                return Json(assignments, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(exception.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpGet]
        [Route("GetAsync/{assignmentId}")]
        public async Task<JsonResult> GetAsync(int assignmentId)
        {
            try
            {
                var assignments = await db.Assignments.Where(x => x.ID == assignmentId && x.IsDeleted == false).FirstOrDefaultAsync();
                return Json(assignments, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(exception.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public async Task<JsonResult> PostAsync(Assignment input)
        {
            try
            {
                input.CreationTimeStamp = DateTime.Now;
                db.Assignments.Add(input);
                await db.SaveChangesAsync();

                return Json(input.ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(exception.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
       
        [HttpPut]
        public async Task<JsonResult> PutAsync(Assignment input)
        {
            try
            {
                var assignment = await db.Assignments.Where(x => x.ID == input.ID).FirstOrDefaultAsync();
                if (assignment != null)
                {
                    assignment.Name = input.Name;
                    assignment.Description = input.Description;
                    assignment.TeacherStudent = input.TeacherStudent;

                    db.Entry(assignment).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return Json(assignment.ID, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                return Json(exception.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
    }
}