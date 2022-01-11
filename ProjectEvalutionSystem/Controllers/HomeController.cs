using Newtonsoft.Json;
using ProjectEvalutionSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProjectEvalutionSystem.Models.Auth;

namespace ProjectEvalutionSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            return View(GetAllAssignmentDetails());
        }

        [HttpGet]
        public async Task<JsonResult> GetStats()
        {
            try
            {
                using (ProjectEvalutionSystemEntities _context = new ProjectEvalutionSystemEntities())
                {
                    int _students = 0, _teachers = 0, _assignments = 0, _evalutionIndexes = 0, _courses = 0;
                    var sessionID = (int)Session["CurrentLoginId"];
                    switch ((UserRole)Session["UserRole"])
                    {
                        case UserRole.Teacher:
                            _students = await _context.Students.Where(x=> x.TeacherID == sessionID).CountAsync();
                            _assignments = await _context.Assignments
                                .Include(x => x.Cours)
                                .Where(x=> x.Cours.TeacherID == sessionID)
                                .CountAsync();
                            _evalutionIndexes = await _context.EvalutionIndexes
                                .Include(a => a.Assignment)
                                .Include(x => x.Assignment.Cours)
                                .Where(x => x.Assignment.Cours.TeacherID == sessionID).CountAsync();
                            _courses = await _context.Courses.Where(x=> x.TeacherID == sessionID).CountAsync();
                            break;

                        case UserRole.SuperAdmin:
                            _students = await _context.Students.CountAsync();
                            _teachers = await _context.Teachers.CountAsync();
                            _assignments = await _context.Assignments.CountAsync();
                            _evalutionIndexes = await _context.EvalutionIndexes.CountAsync();
                            _courses = await _context.Courses.CountAsync();
                            break;
                    }
                    
                    var dataSet = new
                    {
                        students = _students,
                        teachers = _teachers,
                        assignments = _assignments,
                        evalutionIndexes = _evalutionIndexes,
                        courses = _courses
                    };
                    return Json(dataSet, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<Assignment> GetAllAssignmentDetails()
        {
            try
            {
                using (ProjectEvalutionSystemEntities _context = new ProjectEvalutionSystemEntities())
                {
                    return (from us in _context.Assignments
                            //where us.CreationTimeStamp.Date == DateTime.Now.Date
                            select us).ToList();
                    //return _context.Assignments.Where(x => Convert.ToDateTime(x.CreationTimeStamp).Date == DateTime.Now.Date).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}